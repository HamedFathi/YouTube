using System.Text;
using Whisper.net;

namespace WhisperApi;

public static class SrtExporter
{
    public static string ConvertToSrt(List<SegmentData> segments)
    {
        var sb = new StringBuilder();
        var index = 1;

        foreach (var segment in segments)
        {
            sb.AppendLine(index.ToString());
            sb.AppendLine($"{FormatTime(segment.Start)} --> {FormatTime(segment.End)}");
            sb.AppendLine(segment.Text);
            sb.AppendLine();
            index++;
        }

        return sb.ToString();
    }

    private static string FormatTime(TimeSpan time)
    {
        // Format as HH:MM:SS,mmm (SRT format)
        return string.Format("{0:00}:{1:00}:{2:00},{3:000}",
            (int)time.TotalHours,
            time.Minutes,
            time.Seconds,
            time.Milliseconds);
    }
}