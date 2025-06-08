// ReSharper disable All

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace SemanticTextSplitting
{
    public enum GroupingStrategy
    {
        Paragraph,
        Sentence
    }

    public static class SemanticTextSplitter
    {
        [Experimental("SKEXP0070")]
        public static async Task<IEnumerable<string>> SemanticSplitAsync(this string text, int chunkSize, int chunkOverlap, string modelId, Uri endpoint,
            float threshold, GroupingStrategy groupingStrategy = GroupingStrategy.Paragraph)
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOllamaEmbeddingGenerator(modelId, endpoint);
            var kernel = builder.Build();

            var embeddingGenerator = kernel.Services.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

            var textBlocks = groupingStrategy == GroupingStrategy.Paragraph
                ? SemanticTextBlocksGrouper.SplitTextIntoParagraphs(text)
                : SemanticTextBlocksGrouper.SplitTextIntoSentences(text);

            var embeddings = await SemanticTextBlocksGrouper.GenerateTextBlocksEmbeddingsAsync(embeddingGenerator, textBlocks);
            var groupedTextBlocks = SemanticTextBlocksGrouper.GroupTextBlocksBySimilarity(embeddings, threshold);
            var aggregateGroupedTextBlocks = SemanticTextBlocksGrouper.AggregateGroupedTextBlocks(groupedTextBlocks);

            var chunks = aggregateGroupedTextBlocks
                .Select(txt => txt.RecursiveSplit(chunkSize, chunkOverlap))
                .SelectMany(x => x)
                .ToList();

            return chunks;
        }
    }
}
