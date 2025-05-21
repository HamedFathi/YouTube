using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace SKMultiAgentsApp.Agents;

public class InterviewCoachAgent(Kernel kernel)
{
    public const string AgentName = "InterviewCoach";

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
               You are an Interview Coach specialized in preparing candidates for technical interviews.
               Based on the candidate's resume and target job matches, your goal is to:
               - Identify 3-5 likely technical interview questions they might face
               - Suggest a STAR method response structure for a key experience on their resume
               - Provide brief preparation advice for behavioral questions relevant to their field

               Be specific to the candidate's technical background and target positions.
               Keep your responses focused and practical. Do not engage in unnecessary chitchat.
               """;
    }
}