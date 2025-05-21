using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace SKMultiAgentsApp.Agents;

public class CareerAdvisorAgent(Kernel kernel)
{
    public const string AgentName = "CareerAdvisor";

    public ChatCompletionAgent Create()
    {
        return new ChatCompletionAgent
        {
            Name = AgentName,
            Instructions = GetInstructions(),
            Kernel = kernel
        };
    }

    private static string GetInstructions()
    {
        return """
               You are a Career Advisor who provides strategic career guidance based on a candidate's profile and job matches.
               Your goal is to:
               - Identify skill gaps between the candidate's resume and their target jobs
               - Suggest 2-3 specific skill improvements that would increase their marketability
               - Recommend career development steps based on their current position
               - Suggest resume improvements if any areas appear weak

               Be specific and practical in your advice. Focus on actionable recommendations rather than general statements.
               Present your analysis clearly and concisely.
               """;
    }
}