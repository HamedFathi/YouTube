using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SqliteMCP.Extensions;

public static class YamlExtensions
{
    public static string ToYaml(this object obj)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

        return serializer.Serialize(obj);
    }
}