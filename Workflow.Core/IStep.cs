namespace Workflow.Core
{
    public interface IStep
    {
        void Execute(WorkflowContext workflowContext);

        void Rollback(WorkflowContext workflowContext);
    }
}
