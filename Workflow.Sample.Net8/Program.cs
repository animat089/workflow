using Workflow.Core;
using Workflow.Core.Middleware;
using Workflow.Core.Steps;
using Workflow.Sample.Net8;

var workflowSteps = new List<IStep>
{
    new Step1(),
    new ConditionalStep(ctx => ctx != null, new Step2(), new Step1())
};

var workflowMiddleware = new List<IWorkflowMiddleware>
{
    new LoggingMiddleware()
};

var cancellationToken = new CancellationTokenSource();
cancellationToken.CancelAfter(10);

var context = new WorkflowContext(cancellationToken.Token);
var workflow = new Workflow.Core.Workflow(workflowSteps, workflowMiddleware);

try
{
    workflow.Execute(context);
}
catch (Exception ex)
{
    Console.WriteLine($"Workflow execution failed: {ex.Message}");
}

Console.WriteLine("Workflow execution completed.");