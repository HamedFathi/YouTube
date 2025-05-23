using System.Diagnostics;
using System.Text;

namespace WhisperApi;

public static class WhisperCliService
{
    private static readonly SemaphoreSlim Semaphore = new(1);
    private static readonly HttpClient HttpClient = new();

    public static async Task<bool> DownloadModelAsync(string modelFileName, CancellationToken cancellationToken = default)
    {
        // ggml-base.en.bin
        // base.en

        modelFileName = WhisperHelper.FixModelName(modelFileName);

        var whisperFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "whisper");
        var filePath = Path.Combine(whisperFolder, modelFileName);

        if (File.Exists(filePath))
            return true;

        // See models: https://huggingface.co/ggerganov/whisper.cpp/tree/main
        var url = $"https://huggingface.co/ggerganov/whisper.cpp/resolve/main/{modelFileName}";
        try
        {
            using var response = await HttpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            if (!Directory.Exists(whisperFolder))
                Directory.CreateDirectory(whisperFolder);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await response.Content.CopyToAsync(fileStream, cancellationToken);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<string> TranscribeAudioAsync(
        string audioFilePath,
        string modelFileName,
        WhisperOutputFormat format,
        WhisperLanguage language = WhisperLanguage.Auto,
        CancellationToken cancellationToken = default
    )
    {
        var appBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var whisperCliPath = Path.Combine(appBaseDirectory, "whisper", "whisper-cli.exe");
        var modelPath = Path.Combine(appBaseDirectory, "whisper", modelFileName);

        var formatArgument = format switch
        {
            WhisperOutputFormat.Txt => "-otxt",
            WhisperOutputFormat.Vtt => "-ovtt",
            WhisperOutputFormat.Srt => "-osrt",
            WhisperOutputFormat.Lrc => "-olrc",
            _ => "-osrt"
        };

        var audioFileDirectory = Path.GetDirectoryName(audioFilePath)!;
        var audioFileWithoutExt = Path.GetFileNameWithoutExtension(audioFilePath);

        var outputFilePath = Path.Combine(audioFileDirectory, audioFileWithoutExt);

        var arguments = $"-f \"{audioFilePath}\" -m \"{modelPath}\" -of {outputFilePath} -l {language.GetDescription()} {formatArgument}";

        await Semaphore.WaitAsync(cancellationToken);
        try
        {
            if (!File.Exists(whisperCliPath))
            {
                return
                    $"'whisper-cli.exe' not found at '{whisperCliPath}'. Ensure it's in the 'whisper' subfolder.";
            }

            if (!File.Exists(modelPath))
            {
                var status = await DownloadModelAsync(modelFileName, cancellationToken);
                if (!status)
                {
                    return $"Failed to download the specified Whisper model. '{modelFileName}'";
                }
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = whisperCliPath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = startInfo;
            var standardOutputBuilder = new StringBuilder();
            var standardErrorBuilder = new StringBuilder();

            process.OutputDataReceived += (_, e) =>
            {
                if (e.Data != null) standardOutputBuilder.AppendLine(e.Data);
            };
            process.ErrorDataReceived += (_, e) =>
            {
                if (e.Data != null) standardErrorBuilder.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode == 0)
            {
                var outputFilePathWithExt = outputFilePath + format switch
                {
                    WhisperOutputFormat.Txt => ".txt",
                    WhisperOutputFormat.Vtt => ".vtt",
                    WhisperOutputFormat.Srt => ".srt",
                    WhisperOutputFormat.Lrc => ".lrc",
                    _ => ".srt"
                };

                if (File.Exists(outputFilePathWithExt))
                {
                    var output = await File.ReadAllTextAsync(outputFilePathWithExt, cancellationToken);
                    File.Delete(outputFilePathWithExt);
                    return output;
                }
            }

            return
                $"Whisper-cli failed with exit code {process.ExitCode}.\nError Output:\n{standardErrorBuilder}\nStandard Output:\n{standardOutputBuilder}";
        }
        catch (Exception ex)
        {
            return $"An unhandled exception occurred during transcription: {ex.Message}";
        }
        finally
        {
            Semaphore.Release();
        }
    }
}