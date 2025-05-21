using System.Diagnostics.CodeAnalysis;

namespace SKMultiAgentsApp;

internal class Program
{
    [Experimental("SKEXP0110")]
    static async Task Main(string[] args)
    {
        var query = "My name is hamed fathi and I am looking for a senior .NET developer job.";

        var jobAssistantSystem = new JobAssistantSystem();
        await jobAssistantSystem.RunAsync(query);

        Console.WriteLine("\nJob assistance process complete!");
        Console.ReadLine();
    }
}