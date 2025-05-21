var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama("ollama",11445)
    .WithDataVolume()
    .WithAnnotation(new ContainerImageAnnotation
    {
        Image = "ollama/ollama",
        Tag = "0.7.0",
    });
var llamaVision = ollama.AddModel("granite3.2-vision");

builder.AddProject<Projects.OllamaOCRServer>("ollamaocrserver")
    .WithReference(llamaVision)
    .WaitFor(llamaVision)
    ;

builder.Build().Run();
