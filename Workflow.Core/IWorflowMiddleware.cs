using System;

namespace Workflow.Core
{
    public interface IWorkflowMiddleware
    {
        void BeforeExecute(WorkflowContext context, IStep step);
        
        void AfterExecute(WorkflowContext context, IStep step);
        
        void BeforeRollback(WorkflowContext context, IStep step);
        
        void AfterRollback(WorkflowContext context, IStep step);

        void OnError(WorkflowContext context, IStep step, Exception ex);
    }
}
