using System.ComponentModel;
using System.Text;
using Microsoft.SemanticKernel;
using SKMultiAgentsApp.Factories;

namespace SKMultiAgentsApp.Plugins;

public class JobAssistantPlugin
{
    [KernelFunction, Description("Get list of available jobs")]
    public string GetJobs()
    {
        var jobs = JobFactory.CreateJobs().ToList();

        var output = new StringBuilder();
        output.AppendLine("List of all available jobs: ");

        for (var i = 0; i < jobs.Count; i++)
        {
            output.Append($"Job {i + 1} = ");
            output.Append("Title: ").Append(jobs[i].Title).Append("| ");
            output.Append("Company: ").Append(jobs[i].Company).Append("| ");
            output.Append("Location: ").Append(jobs[i].Location).Append("| ");
            output.Append("Description: ").Append(jobs[i].Description).Append("| ");
            output.Append("Required Skills: ")
                .Append(jobs[i].RequiredSkills.Any()
                    ? string.Join(", ", jobs[i].RequiredSkills)
                    : "None")
                .Append("| ");
            output.Append("Salary Range: ").Append(jobs[i].SalaryRange).Append("| ");
            output.Append("Application URL: ").Append(jobs[i].ApplicationUrl);
            output.AppendLine();
        }

        var text = output.ToString();
        return text;
    }

    [KernelFunction, Description("Get resume based on the only name")]
    public string GetResume([Description("the name")] string name)
    {
        // You can use the name parameter to customize the resume content if needed
        return Resume.Content;
    }
}