using Workflow.Core;

namespace Workflow.Sample
{
    public class Step2 : IStep
    {
        public void Execute(WorkflowContext context)
        {
            Console.WriteLine("Executing Step 2");
            var step1Result = context.Get<string>("Step1Result");
            context.Set("Step2Result", $"Result from Step 2, using {step1Result}");
        }

        public void Rollback(WorkflowContext context)
        {
            Console.WriteLine("Compensating Step 2");
            context.Remove("Step2Result");
        }
    }
}
