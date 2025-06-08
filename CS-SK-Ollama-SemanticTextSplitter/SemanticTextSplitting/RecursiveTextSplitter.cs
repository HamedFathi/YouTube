// ReSharper disable All

using System.Text;

namespace SemanticTextSplitting;

public static class RecursiveTextSplitter
{
    public static IEnumerable<string> RecursiveSplit(this string text, int chunkSize, int chunkOverlap = 0
        , string[]? separators = null)
    {

        if (string.IsNullOrEmpty(text))
            return [];

        if (chunkSize <= 0)
            throw new ArgumentException("Chunk size must be positive", nameof(chunkSize));

        if (chunkOverlap < 0)
            throw new ArgumentException("Chunk overlap cannot be negative", nameof(chunkOverlap));

        if (chunkOverlap >= chunkSize)
            throw new ArgumentException("Chunk overlap must be less than chunk size", nameof(chunkOverlap));

        var originalLineEnding = DetectLineEnding(text);
        var normalizedText = NormalizeLineEndings(text);

        if (separators == null || separators.Length == 0)
            separators ??= GetDefaultSeparators();

        var chunks = SplitRecursively(normalizedText, chunkSize, separators, 0);

        if (chunkOverlap > 0 && chunks.Count > 1)
        {
            chunks = ApplyOverlap(chunks, chunkOverlap);
        }

        if (originalLineEnding != "\n")
        {
            return chunks.Select(chunk => chunk.Replace("\n", originalLineEnding));
        }

        return chunks;
    }

    private static List<string> ApplyOverlap(List<string> chunks, int chunkOverlap)
    {
        if (chunks.Count == 0)
            return chunks;

        var overlappedChunks = new List<string> { chunks[0] };
        var sb = new StringBuilder();

        for (int i = 1; i < chunks.Count; i++)
        {
            // Get previous and current chunk
            var prev = chunks[i - 1];
            var current = chunks[i];

            // Extract word-safe overlap from end of previous chunk
            var overlap = GetWordSafeOverlap(prev, chunkOverlap);

            // Build new chunk: overlap + current content
            sb.Clear();
            sb.Append(overlap);
            sb.Append(current);

            overlappedChunks.Add(sb.ToString());
        }

        return overlappedChunks;
    }
    private static string GetWordSafeOverlap(string text, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(text) || maxLength <= 0)
            return string.Empty;

        // Calculate starting position from end of text
        var start = Math.Max(0, text.Length - maxLength);

        // Extract candidate overlap substring from end
        var candidate = text.Substring(start);

        // Find first word/sentence boundary character in candidate
        int wordStart = candidate.IndexOfAny([
            ' ', '\n', '\r', '\t', '\f',       // Whitespace characters
            '.', ',', ';', ':', '!', '?',      // Punctuation marks
            '(', ')', '[', ']', '{', '}',      // Brackets
            '\"', '\'', '`'                    // Quote characters
        ]);

        // If boundary found and not at end, start from after boundary (safe block)
        if (wordStart >= 0 && wordStart < candidate.Length - 1)
        {
            return candidate.Substring(wordStart + 1);
        }

        // No good boundary found, return full candidate
        return candidate;
    }

    private static string DetectLineEnding(string text)
    {
        // Windows style (CRLF) - check first as it contains both \r and \n
        if (text.Contains("\r\n"))
            return "\r\n";

        // Old Mac style (CR only)
        if (text.Contains("\r"))
            return "\r";

        // Unix/Linux style (LF) - default fallback
        return "\n";
    }

    private static string[] GetDefaultSeparators()
    {
        return [
            "\n\n",    // Paragraph breaks (largest semantic unit)
            ".\n",     // Sentence endings with newline
            "!\n",     // Exclamation with newline
            "?\n",     // Question with newline
            ":\n",     // Colon with newline
            ";\n",     // Semicolon with newline
            "\n",      // Single newlines (line breaks)
            ". ",      // Sentence endings with space
            "! ",      // Exclamation with space
            "? ",      // Question with space
            "; ",      // Semicolon with space
            ", ",      // Comma with space
            " ",       // Single spaces (word boundaries)
            ""         // Character-by-character (last resort)
        ];
    }

    private static string NormalizeLineEndings(string text)
    {
        return text.Replace("\r\n", "\n").Replace("\r", "\n");
    }

    private static List<string> SplitRecursively(string text, int chunkSize, string[] separators, int separatorIndex)
    {
        // Container for resulting chunks
        var chunks = new List<string>();

        // Base case: text fits in one chunk
        if (text.Length <= chunkSize)
        {
            // Only add non-empty chunks
            if (text.Length > 0)
                chunks.Add(text);
            return chunks;
        }

        // No more separators available, force split by character
        if (separatorIndex >= separators.Length)
        {
            // Split into character-based chunks
            for (var i = 0; i < text.Length; i += chunkSize)
            {
                // Calculate remaining characters to avoid overflow
                var length = Math.Min(chunkSize, text.Length - i);
                chunks.Add(text.Substring(i, length));
            }
            return chunks;
        }

        // Get current separator to try
        var separator = separators[separatorIndex];

        // Track remaining text to process
        var remainingText = text;

        // Handle empty separator (character-by-character splitting)
        if (separator == "")
        {
            // Use StringBuilder for efficient string building
            var sb = new StringBuilder(chunkSize);

            // Split into character chunks
            for (var i = 0; i < text.Length; i += chunkSize)
            {
                sb.Clear();
                var length = Math.Min(chunkSize, text.Length - i);
                sb.Append(text, i, length);
                chunks.Add(sb.ToString());
            }
            return chunks;
        }

        // Process text while there's content remaining
        while (remainingText.Length > 0)
        {
            // Remaining text fits in one chunk
            if (remainingText.Length <= chunkSize)
            {
                chunks.Add(remainingText);
                break;
            }

            // Current separator not found, try next separator level
            if (!remainingText.Contains(separator))
            {
                chunks.AddRange(SplitRecursively(remainingText, chunkSize, separators, separatorIndex + 1));
                break;
            }

            // Find last occurrence of separator within chunk size limit
            var splitAt = remainingText.LastIndexOf(separator, 
                Math.Min(chunkSize - 1, remainingText.Length - 1), StringComparison.Ordinal);

            // No separator found within chunk size
            if (splitAt == -1)
            {
                // Find first separator occurrence anywhere in text
                var firstSeparatorIndex = remainingText.IndexOf(separator, StringComparison.Ordinal);

                // Text starts with separator, skip it
                if (firstSeparatorIndex == 0)
                {
                    remainingText = remainingText.Substring(separator.Length);
                    continue;
                }

                // Extract oversized chunk before first separator
                var oversizedChunk = remainingText.Substring(0, firstSeparatorIndex);

                // Update remaining text to continue after separator
                remainingText = remainingText.Substring(firstSeparatorIndex + separator.Length);

                // Recursively split the oversized chunk with next separator
                chunks.AddRange(SplitRecursively(oversizedChunk, chunkSize, separators, separatorIndex + 1));
            }
            else
            {
                // Found good split point, create chunk including separator
                chunks.Add(remainingText.Substring(0, splitAt + separator.Length));

                // Continue with text after the separator
                remainingText = remainingText.Substring(splitAt + separator.Length);
            }
        }

        return chunks;
    }
}