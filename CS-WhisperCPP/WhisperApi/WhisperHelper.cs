using System.ComponentModel;
using System.Reflection;

namespace WhisperApi;

public static class WhisperHelper
{
    public static string FixModelName(string modelFileName)
    {
        if (string.IsNullOrWhiteSpace(modelFileName))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(modelFileName));

        if (!modelFileName.StartsWith("ggml-"))
            modelFileName = "ggml-" + modelFileName;
        if (!modelFileName.EndsWith(".bin"))
            modelFileName += ".bin";

        return modelFileName;
    }

    public static string GetContentType(WhisperOutputFormat format)
    {
        return format switch
        {
            WhisperOutputFormat.Txt => "text/plain",
            WhisperOutputFormat.Vtt => "text/vtt",
            WhisperOutputFormat.Srt => "application/x-subrip",
            WhisperOutputFormat.Lrc => "application/x-lrc",
            _ => "text/plain"
        };
    }

    public static WhisperLanguage? GetWhisperLanguageFromDescription(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        input = input.ToLower();

        foreach (WhisperLanguage lang in Enum.GetValues(typeof(WhisperLanguage)))
        {
            var memberInfo = typeof(WhisperLanguage).GetMember(lang.ToString()).FirstOrDefault();
            var descriptionAttr = memberInfo?.GetCustomAttribute<DescriptionAttribute>();

            if (descriptionAttr != null && descriptionAttr.Description.ToLower().Contains(input))
            {
                return lang;
            }
        }

        return null;
    }
}