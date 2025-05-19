using Microsoft.Extensions.VectorData;

namespace RAGMovieAppSK;

public class Movie
{
    [VectorStoreRecordKey]
    public Guid Key { get; set; } = Guid.NewGuid();

    [VectorStoreRecordData]
    public string Title { get; set; } = null!;

    [VectorStoreRecordData]
    public string Reference { get; set; } = null!;

    [VectorStoreRecordData]
    public int PublishDate { get; set; }

    [VectorStoreRecordData]
    public string Description { get; set; } = null!;

    [VectorStoreRecordVector(768, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }
}