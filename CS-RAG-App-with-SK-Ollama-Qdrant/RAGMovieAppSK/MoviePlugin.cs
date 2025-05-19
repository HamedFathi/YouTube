using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace RAGMovieAppSK;

public class MoviePlugin
{
    private readonly MovieApiService _movieApiService;
    private static readonly List<Movie> Movies = MovieDatabase.GetMovies();

    public MoviePlugin(MovieApiService movieApiService)
    {
        _movieApiService = movieApiService;
    }

    [KernelFunction, Description("Get the current date and time")]
    public string Now()
    {
        return DateTime.Now.ToString("f");
    }

    [KernelFunction, Description("Compare two movies side by side")]
    public async Task<string> CompareMovies(
        [Description("First movie title")] string movieTitle1,
        [Description("Second movie title")] string movieTitle2)
    {
        var movie1 = Movies.FirstOrDefault(m => m.Title.Equals(movieTitle1, StringComparison.OrdinalIgnoreCase));
        var movie2 = Movies.FirstOrDefault(m => m.Title.Equals(movieTitle2, StringComparison.OrdinalIgnoreCase));

        var extraMovie1Data = await _movieApiService.GetMovieDetailsAsync(movieTitle1);
        var extraMovie2Data = await _movieApiService.GetMovieDetailsAsync(movieTitle2);

        var sb = new StringBuilder();
        sb.AppendLine($"\nComparison between '{movieTitle1}' and '{movieTitle2}':\n");
        sb.AppendLine($"Title: {movieTitle1.PadRight(30)} | {movieTitle2}");
        sb.AppendLine($"Description: {movie1?.Description.PadRight(30)} | {movie2?.Description}");
        sb.AppendLine($"Release Year: {movie1?.PublishDate.ToString().PadRight(30)} | {movie2?.PublishDate}");
        sb.AppendLine($"Genres: {string.Join(", ", extraMovie1Data?.Genre ?? ["N/A"]).PadRight(30)} | {string.Join(", ", extraMovie2Data?.Genre ?? ["N/A"])}");
        sb.AppendLine($"Cast: {string.Join(", ", extraMovie1Data?.Cast ?? ["N/A"]).PadRight(30)} | {string.Join(", ", extraMovie2Data?.Cast ?? ["N/A"])}");
        sb.AppendLine($"Rating: {extraMovie1Data?.Rating.ToString(CultureInfo.InvariantCulture) ?? "N/A".PadRight(30)} | {extraMovie2Data?.Rating.ToString(CultureInfo.InvariantCulture) ?? "N/A"}");
        sb.AppendLine($"Box Office: {extraMovie1Data?.BoxOffice ?? "N/A".PadRight(30)} | {extraMovie2Data?.BoxOffice ?? "N/A"}");
        sb.AppendLine($"More Info: {movie1?.Reference.PadRight(30)} | {movie2?.Reference}");

        return sb.ToString();
    }
}