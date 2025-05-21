using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using Azure.AI.OpenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using SKMultiAgentsApp.Agents;
using SKMultiAgentsApp.Plugins;

namespace SKMultiAgentsApp;

[Experimental("SKEXP0110")]
public class JobAssistantSystem
{
    private readonly Kernel _kernel;
    private readonly AgentGroupChat _chat;

    public JobAssistantSystem()
    {
        _kernel = CreateKernel();
        _chat = CreateAgentGroupChat();
    }

    private Kernel CreateKernel()
    {
        var builder = Kernel.CreateBuilder();

        builder.AddAzureOpenAIChatClient("deployment-name", new AzureOpenAIClient(new Uri("endpoint-url"), new ApiKeyCredential("api-key")));

        builder.Plugins.AddFromType<JobAssistantPlugin>();

        return builder.Build();
    }

    public async Task RunAsync(string initialUserMessage)
    {
        Console.WriteLine("Job Assistant Multi-Agent System");
        Console.WriteLine("--------------------------------");

        _chat.AddChatMessage(new ChatMessageContent(AuthorRole.User, initialUserMessage));

        await foreach (var content in _chat.InvokeAsync())
        {
            if (!string.IsNullOrWhiteSpace(content.Content))
            {
                Console.WriteLine($"# {content.Role} - {content.AuthorName ?? "*"}: '{content.Content}'");
                Console.WriteLine("<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>");
            }
        }

        Console.WriteLine($"# IS COMPLETE: {_chat.IsComplete}");
    }

    private AgentGroupChat CreateAgentGroupChat()
    {
        var resumeParserAgent = new ResumeParserAgent(_kernel);
        var jobMatcherAgent = new JobMatcherAgent(_kernel);
        var careerAdvisorAgent = new CareerAdvisorAgent(_kernel);
        var interviewCoachAgent = new InterviewCoachAgent(_kernel);

        var resumeParser = resumeParserAgent.Create();
        var jobMatcher = jobMatcherAgent.Create();
        var careerAdvisor = careerAdvisorAgent.Create();
        var interviewCoach = interviewCoachAgent.Create();

        var chatOrchestrator = new AgentGroupOrchestrator(_kernel);

        return new AgentGroupChat(resumeParser, jobMatcher, careerAdvisor, interviewCoach)
        {
            ExecutionSettings = chatOrchestrator.CreateExecutionSettings([interviewCoach])
        };
    }
}