using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using SKMultiAgentsApp.Plugins;

namespace SKMultiAgentsApp.Agents;

public class JobMatcherAgent(Kernel kernel)
{
    public const string AgentName = "JobMatcher";

    public ChatCompletionAgent Create()
    {
        var getJobs = kernel.Plugins.GetFunction(nameof(JobAssistantPlugin), nameof(JobAssistantPlugin.GetJobs));

        return new ChatCompletionAgent
        {
            Name = AgentName,
            Instructions = GetInstructions(),
            Kernel = kernel,
            Arguments = new KernelArguments(new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Required([getJobs])
            })
        };
    }

    private static string GetInstructions()
    {
        return """
               Get list of available jobs to find the best job matches based on a candidate's resume.
               Be direct and professional. Do not engage in chitchat.
               """;
    }
}