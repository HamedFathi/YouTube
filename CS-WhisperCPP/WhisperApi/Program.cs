using System.Text;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
namespace WhisperApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1024 * 1024 * 1024; // 1 GB
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapPost("/transcribe", async (IFormFile audio, [FromForm] string model, [FromForm] string lang = "auto") =>
        {
            if (!audio.ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest("Uploaded file is not an audio file.");
            }

            if (audio.Length == 0)
            {
                return Results.BadRequest("Uploaded audio file is empty.");
            }

            if (string.IsNullOrEmpty(model))
            {
                return Results.BadRequest("Missing required field: 'model'.");
            }

            var langType = WhisperHelper.GetWhisperLanguageFromDescription(lang);
            if (langType == null)
            {
                return Results.BadRequest("Invalid language.");
            }

            model = WhisperHelper.FixModelName(model);

            var status = await WhisperService.DownloadModelAsync(model);

            if (!status)
            {
                return Results.BadRequest($"The model '{model}' could not be found. An error occurred during the download process.");
            }

            var tempAudioPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(audio.FileName));

            await using (var stream = File.Create(tempAudioPath))
            {
                await audio.CopyToAsync(stream);
            }

            var transcription = await WhisperService.TranscribeAudioAsync(tempAudioPath, model, langType.Value);
            File.Delete(tempAudioPath);

            return Results.Text(transcription, "application/x-subrip", Encoding.UTF8);
        }).DisableAntiforgery();

        app.MapPost("/transcribe-cli", async (IFormFile audio, [FromForm] string model, [FromForm] string format, [FromForm] string lang = "auto") =>
        {
            if (!audio.ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest("Uploaded file is not an audio file.");
            }

            if (audio.Length == 0)
            {
                return Results.BadRequest("Uploaded audio file is empty.");
            }

            if (string.IsNullOrEmpty(model))
            {
                return Results.BadRequest("Missing required field: 'model'.");
            }

            if (string.IsNullOrEmpty(format))
            {
                return Results.BadRequest("Missing required field: 'format'.");
            }

            if (!Enum.TryParse<WhisperOutputFormat>(format, true, out var formatType))
            {
                return Results.BadRequest("Invalid format. Allowed values: txt, vtt, srt, lrc.");
            }

            var langType = WhisperHelper.GetWhisperLanguageFromDescription(lang);
            if (langType == null)
            {
                return Results.BadRequest("Invalid language.");
            }

            model = WhisperHelper.FixModelName(model);

            var status = await WhisperCliService.DownloadModelAsync(model);

            if (!status)
            {
                return Results.BadRequest($"The model '{model}' could not be found. An error occurred during the download process.");
            }

            var tempAudioPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + Path.GetExtension(audio.FileName));

            await using (var stream = File.Create(tempAudioPath))
            {
                await audio.CopyToAsync(stream);
            }

            var transcription = await WhisperCliService.TranscribeAudioAsync(tempAudioPath, model, formatType, langType.Value);
            File.Delete(tempAudioPath);

            return Results.Text(transcription, WhisperHelper.GetContentType(formatType), Encoding.UTF8);
        }).DisableAntiforgery();

        app.Run();
    }
}