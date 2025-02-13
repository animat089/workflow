using System;

namespace Workflow.Core.Middleware
{
    public class LoggingMiddleware : IWorkflowMiddleware
    {
        public void BeforeExecute(WorkflowContext context, IStep step)
        {
            Console.WriteLine($"Starting execution of step: {step.GetType().Name}");
        }

        public void AfterExecute(WorkflowContext context, IStep step)
        {
            Console.WriteLine($"Completed execution of step: {step.GetType().Name}");
        }

        public void BeforeRollback(WorkflowContext context, IStep step)
        {
            Console.WriteLine($"Starting rollback of step: {step.GetType().Name}");
        }

        public void AfterRollback(WorkflowContext context, IStep step)
        {
            Console.WriteLine($"Completed rollback of step: {step.GetType().Name}");
        }

        public void OnError(WorkflowContext context, IStep step, Exception ex)
        {
            Console.WriteLine($"Error in step {step.GetType().Name}: {ex.Message}");
        }
    }
}
