
using System.Diagnostics.CodeAnalysis;
using System.Text;
using OllamaSharp;
using OllamaSharp.Models.Chat;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace OllamaOCRServer
{
    public class Program
    {
        [Experimental("SKEXP0070")]
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();

            builder.Services.AddOpenApi();

            builder.Services.AddSingleton<OllamaApiClient>(sp =>
            {
                var ollamaEndpoint = sp.GetRequiredService<IConfiguration>()["ConnectionStrings:OllamaEndpoint"] ?? "http://localhost:11445";

                var httpClient = new HttpClient()
                {
                    BaseAddress = new Uri(ollamaEndpoint),
                    Timeout = TimeSpan.FromMinutes(5)
                };
                var client = new OllamaApiClient(httpClient);
                return client;
            });


            var app = builder.Build();

            app.MapDefaultEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });
            }

            app.UseHttpsRedirection();


            app.MapPost("/ocr", async (HttpRequest request, OllamaApiClient ollamaClient) =>
            {
                if (request.ContentType == null || !request.ContentType.StartsWith("image/"))
                {
                    return Results.BadRequest("Please upload an image file (e.g., image/jpeg, image/png).");
                }

                try
                {
                    using var memoryStream = new MemoryStream();
                    await request.Body.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();
                    var base64Image = Convert.ToBase64String(imageBytes);

                    var chatRequest = new ChatRequest
                    {
                        Model = "granite3.2-vision",
                        Messages = new List<Message>
                        {
                            new Message(ChatRole.System, "You are a professional assistant to work as an OCR and extract the whole text from an image as it is."),
                            new Message
                            {
                                Role = ChatRole.User,
                                Content = "Extract all visible text from this image. Provide the exact text only, without any additional commentary or formatting also do not engage in chitchat.",
                                Images = [base64Image]
                            }
                        },
                        Stream = false
                    };

                    var response = ollamaClient.ChatAsync(chatRequest);

                    var extractedText = new StringBuilder();
                    await foreach (var message in response)
                    {
                        if (message != null)
                            extractedText.Append(message.Message.Content);
                    }

                    var output = extractedText.ToString();

                    if (string.IsNullOrEmpty(output))
                    {
                        return Results.Problem("Could not extract text from the image. LLM response was empty.", statusCode: StatusCodes.Status500InternalServerError);
                    }
                    return Results.Text(output, "text/plain", Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "Error processing OCR request with OllamaSharp.");
                    return Results.Problem("An error occurred while processing the image for OCR.", statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithName("PerformOcr")
            .WithOpenApi();

            app.Run();
        }
    }
}
