using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel;
using System.Diagnostics.CodeAnalysis;

namespace SKMultiAgentsApp.Agents;

public class AgentGroupOrchestrator(Kernel kernel)
{
    private const string ResumeParserName = ResumeParserAgent.AgentName;
    private const string JobMatcherName = JobMatcherAgent.AgentName;
    private const string CareerAdvisorName = CareerAdvisorAgent.AgentName;
    private const string InterviewCoachName = InterviewCoachAgent.AgentName;

    [Experimental("SKEXP0110")]
    public AgentGroupChatSettings CreateExecutionSettings(ChatCompletionAgent[] agents)
    {
        return new()
        {
            TerminationStrategy = CreateTerminationStrategy(agents),
            SelectionStrategy = CreateSelectionStrategy()
        };
    }

    [Experimental("SKEXP0110")]
    private KernelFunctionSelectionStrategy CreateSelectionStrategy()
    {
        var selectionFunction = KernelFunctionFactory.CreateFromPrompt(
            $$$"""
               Your job is to determine which participant takes the next turn in the job assistance conversation based on the current state.
               State ONLY the name of the participant to take the next turn, without any additional text.

               Choose only from these participants:
               - {{{ResumeParserName}}}
               - {{{JobMatcherName}}}
               - {{{CareerAdvisorName}}}
               - {{{InterviewCoachName}}}

               Always follow these rules when selecting the next participant:
               1) If the last message was from the user and contains a resume, it is {{{ResumeParserName}}}'s turn to extract structured information.
               2) If the last message was from {{{ResumeParserName}}} and contains extracted resume data (e.g., in Markdown), it's {{{JobMatcherName}}}'s turn to find matching jobs.
               3) If the last message was from {{{JobMatcherName}}} and contains job matching results, it's {{{CareerAdvisorName}}}'s turn to provide career advice.
               4) If the last message was from {{{CareerAdvisorName}}} and contains career advice, it's {{{InterviewCoachName}}}'s turn to offer interview preparation guidance.
               5) If the last message was from {{{InterviewCoachName}}} and contains interview guidance, the conversation is complete.

               History:
               {{$history}}

               Return only one of these names: {{{ResumeParserName}}}, {{{JobMatcherName}}}, {{{CareerAdvisorName}}}, or {{{InterviewCoachName}}}.
               Do not include any explanation, just the name.
               """
        );

        return new KernelFunctionSelectionStrategy(selectionFunction, kernel)
        {
            HistoryVariableName = "history"
        };
    }

    [Experimental("SKEXP0110")]
    private KernelFunctionTerminationStrategy CreateTerminationStrategy(ChatCompletionAgent[] agents)
    {
        var terminateFunction = KernelFunctionFactory.CreateFromPrompt(
            """
            Determine if the job assistance process is complete based on whether the InterviewCoach has provided their recommendations. If so, respond with a single word: yes.

            History:
            {{$history}}
            """
        );

        return new KernelFunctionTerminationStrategy(terminateFunction, kernel)
        {
            Agents = agents,
            ResultParser = result => result.GetValue<string>()?.Contains("yes", StringComparison.OrdinalIgnoreCase) ?? false,
            HistoryVariableName = "history",
            MaximumIterations = 10
        };
    }
}