// ReSharper disable All

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Microsoft.Extensions.AI;

namespace SemanticTextSplitting;

public static class SemanticTextBlocksGrouper
{
    public static async Task<Dictionary<string, float[]>> GenerateTextBlocksEmbeddingsAsync(
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        IEnumerable<string> textBlocks)
    {
        var sentenceEmbeddings = new ConcurrentDictionary<string, float[]>();

        var tasks = textBlocks.Distinct().Select(async block =>
        {
            var embedding = await embeddingGenerator.GenerateVectorAsync(block);
            sentenceEmbeddings[block] = embedding.ToArray();
        });

        await Task.WhenAll(tasks);

        return sentenceEmbeddings.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /*
    // Non-Async version
    public static async Task<Dictionary<string, float[]>> GenerateTextBlocksEmbeddingsAsync(
        IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator,
        IEnumerable<string> textBlocks)
    {
        var sentenceEmbeddings = new Dictionary<string, float[]>();

        foreach (var textBlock in textBlocks)
        {
            if (sentenceEmbeddings.ContainsKey(textBlock)) continue;

            var embedding = await embeddingGenerator.GenerateVectorAsync(textBlock);
            sentenceEmbeddings[textBlock] = embedding.ToArray();
        }

        return sentenceEmbeddings;
    }
    */

    public static IEnumerable<IEnumerable<string>> GroupTextBlocksBySimilarity(
        Dictionary<string, float[]> textBlocksEmbeddings, float threshold)
    {
        // List to store groups of similar text blocks
        var groups = new List<List<string>>();

        // Track which texts have already been assigned to groups
        var processed = new HashSet<string>();

        // Iterate through each text block as potential group leader
        foreach (var text in textBlocksEmbeddings.Keys)
        {
            // Skip if this text is already in a group
            if (processed.Contains(text)) continue;

            // Start new group with current text as leader
            var group = new List<string> { text };

            // Mark current text as processed
            processed.Add(text);

            // Compare current text with all other unprocessed texts
            foreach (var otherText in textBlocksEmbeddings.Keys)
            {
                // Skip if other text is already processed
                if (processed.Contains(otherText)) continue;

                // Calculate cosine similarity between embeddings
                var similarity = CosineSimilarity(
                    textBlocksEmbeddings[text],
                    textBlocksEmbeddings[otherText]);

                // Add to group if similarity exceeds threshold
                if (similarity >= threshold)
                {
                    group.Add(otherText);

                    // Mark other text as processed to avoid duplicate grouping
                    processed.Add(otherText);
                }
            }

            // Add completed group to results
            groups.Add(group);
        }

        return groups;
    }

    public static IEnumerable<string> AggregateGroupedTextBlocks(IEnumerable<IEnumerable<string>> groupedTextBlocks)
    {
        var aggregatedGroups = new List<string>();

        foreach (var group in groupedTextBlocks)
        {
            var combined = string.Join(" ", group);
            aggregatedGroups.Add(combined);
        }

        return aggregatedGroups;
    }

    public static List<string> SplitTextIntoParagraphs(string text)
    {
        return text.Split(["\r\n\r\n", "\n\n"], StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();
    }

    public static List<string> SplitTextIntoSentences(string text)
    {
        var sentenceEndings = new Regex(@"(?<=[.!?])\s+(?=[A-Z])");
        return sentenceEndings.Split(text)
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();
    }

    // Calculates cosine similarity between two embedding vectors (returns 0-1, where 1 = identical)
    private static float CosineSimilarity(float[] vectorA, float[] vectorB)
    {
        // Validate inputs are not null
        if (vectorA == null) throw new ArgumentNullException(nameof(vectorA));
        if (vectorB == null) throw new ArgumentNullException(nameof(vectorB));

        // Ensure vectors have same dimensionality
        if (vectorA.Length != vectorB.Length)
            throw new ArgumentException("Vectors must have the same length.");

        // Initialize accumulators for dot product and magnitudes
        var dotProduct = 0f;
        var magnitudeA = 0f;
        var magnitudeB = 0f;

        // Calculate dot product and squared magnitudes in single pass
        for (var i = 0; i < vectorA.Length; i++)
        {
            var a = vectorA[i];
            var b = vectorB[i];
            dotProduct += a * b;        // Sum of element-wise products
            magnitudeA += a * a;        // Sum of squares for vector A
            magnitudeB += b * b;        // Sum of squares for vector B
        }

        // Prevent division by zero for zero-magnitude vectors
        var epsilon = 1e-6f;
        if (magnitudeA < epsilon || magnitudeB < epsilon)
            return 0f;

        // Formula: cos(θ) = (A·B) / (|A| × |B|)
        return dotProduct / (MathF.Sqrt(magnitudeA) * MathF.Sqrt(magnitudeB));
    }

}