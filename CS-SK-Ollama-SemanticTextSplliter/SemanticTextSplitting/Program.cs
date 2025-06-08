// ReSharper disable All

using System.Diagnostics.CodeAnalysis;

namespace SemanticTextSplitting
{
    internal class Program
    {
        [Experimental("SKEXP0070")]
        static async Task Main(string[] args)
        {
            /*

            Artificial intelligence is transforming every industry. From healthcare to finance, automation is becoming smarter and more adaptive. 
            However, challenges like bias, interpretability, and safety remain important areas of research. [229]

            Artificial intelligence is transforming every industry.  [56]
            From healthcare to finance, automation is becoming smarter and more adaptive.  [78]

            However, challenges like bias, interpretability, and safety remain important areas of research. [95] (bigger than 80 characters)

            However, challenges like bias, interpretability, and safety remain important are [80] 'are != areas' (Naive split)

            However, challenges like bias, interpretability,  [49]
            and safety remain important areas of research. [46]

            */

            // Naive split
            var naiveChunks = PlainText.RecursiveSample.ToCharArray().Chunk(80)
                .Select(chars => new string(chars))
                .ToList();


            // Final chunk size = chunkSize + chunkOverlap => 80 + 0 = 80 characters
            var chunks = PlainText.RecursiveSample.RecursiveSplit(80, 0).ToList();

            /*
            
            sforming every industry. [25] (Naive overlap split)
            every industry. [16] (better overlap split)

            */

            // Final chunk size = chunkSize + chunkOverlap => 80 + 25 = 105 characters\
            var chunksWithOverlap = PlainText.RecursiveSample.RecursiveSplit(80, 25).ToList();


            var semanticChunks = (await PlainText.SemanticSample.SemanticSplitAsync(2000, 200
                , "mxbai-embed-large", new Uri("http://localhost:11434"), 0.8f, GroupingStrategy.Paragraph)).ToList();
        }
    }
}
