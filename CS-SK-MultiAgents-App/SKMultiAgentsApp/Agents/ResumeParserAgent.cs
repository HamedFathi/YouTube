using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel;
using SKMultiAgentsApp.Plugins;

namespace SKMultiAgentsApp.Agents;

public class ResumeParserAgent(Kernel kernel)
{
    public const string AgentName = "ResumeParser";

    public ChatCompletionAgent Create()
    {
        var getResume = kernel.Plugins.GetFunction(nameof(JobAssistantPlugin), nameof(JobAssistantPlugin.GetResume));

        return new ChatCompletionAgent
        {
            Name = AgentName,
            Instructions = GetInstructions(),
            Kernel = kernel,
            Arguments = new KernelArguments(new PromptExecutionSettings()
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Required([getResume])
            })
        };
    }

    private static string GetInstructions()
    {
        return """
               You are a Resume Parser specializing in extracting structured information from resume text.
               Your job is to carefully analyze the provided resume and extract key information including:
               - Full name
               - Email and phone
               - Current job title
               - Years of experience
               - Skills (technical and soft skills)
               - Education history
               - Work experience

               Be precise and comprehensive.
               Do not engage in chitchat or provide additional commentary.
               """;
    }
}