using Whisper.net;
using Whisper.net.Ggml;

namespace WhisperApi;

public static class WhisperService
{
    private static readonly Dictionary<string, GgmlType> ModelFilenameToGgmlType = new()
    {
        { "ggml-tiny.bin",            GgmlType.Tiny },
        { "ggml-tiny.en.bin",         GgmlType.TinyEn },
        { "ggml-base.bin",            GgmlType.Base },
        { "ggml-base.en.bin",         GgmlType.BaseEn },
        { "ggml-small.bin",           GgmlType.Small },
        { "ggml-small.en.bin",        GgmlType.SmallEn },
        { "ggml-medium.bin",          GgmlType.Medium },
        { "ggml-medium.en.bin",       GgmlType.MediumEn },
        { "ggml-large-v1.bin",        GgmlType.LargeV1 },
        { "ggml-large-v2.bin",        GgmlType.LargeV2 },
        { "ggml-large-v3.bin",        GgmlType.LargeV3 },
        { "ggml-large-v3-turbo.bin", GgmlType.LargeV3Turbo },
    };

    public static async Task<bool> DownloadModelAsync(string modelFileName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // ggml-base.en.bin
            // base.en

            modelFileName = WhisperHelper.FixModelName(modelFileName);

            GgmlType ggmlType;
            if (ModelFilenameToGgmlType.TryGetValue(modelFileName, out var value))
            {
                ggmlType = value;
            }
            else
                return false;

            if (!File.Exists(modelFileName))
            {
                await using var modelStream = await WhisperGgmlDownloader.Default.GetGgmlModelAsync(ggmlType, cancellationToken: cancellationToken);
                await using var fileWriter = File.OpenWrite(modelFileName);
                await modelStream.CopyToAsync(fileWriter, cancellationToken);

                return true;
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<string> TranscribeAudioAsync(
        string audioFilePath, // Only .wav files are supported if you need others use NAudio to convert
        string modelFileName,
        WhisperLanguage language = WhisperLanguage.Auto,
        CancellationToken cancellationToken = default
    )
    {

        modelFileName = WhisperHelper.FixModelName(modelFileName);

        using var whisperFactory = WhisperFactory.FromPath(modelFileName);

        await using var processor = whisperFactory.CreateBuilder()
            .WithLanguage(language.GetDescription())
            .Build();

        await using var fileStream = File.OpenRead(audioFilePath);

        var segments = new List<SegmentData>();
        await foreach (var segment in processor.ProcessAsync(fileStream, cancellationToken))
        {
            segments.Add(segment);
        }

        var output = SrtExporter.ConvertToSrt(segments);
        return output;
    }
}