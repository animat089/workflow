using System;

namespace Workflow.Core.Steps
{
    public class ConditionalStep : IStep
    {
        private readonly Func<WorkflowContext, bool> condition;
        private readonly IStep trueStep;
        private readonly IStep falseStep;

        public ConditionalStep(Func<WorkflowContext, bool> condition, IStep trueStep, IStep falseStep)
        {
            this.condition = condition;
            this.trueStep = trueStep;
            this.falseStep = falseStep;
        }

        public void Execute(WorkflowContext context)
        {
            var step = condition(context) ? trueStep : falseStep;
            step.Execute(context);
        }

        public void Rollback(WorkflowContext context)
        {
            var step = condition(context) ? trueStep : falseStep;
            step.Rollback(context);
        }
    }
}
