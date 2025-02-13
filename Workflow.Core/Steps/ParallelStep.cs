using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workflow.Core.Steps
{
    public class ParallelStep : IStep
    {
        private readonly IEnumerable<IStep> parallelSteps;

        public ParallelStep(IEnumerable<IStep> parallelSteps)
        {
            this.parallelSteps = parallelSteps;
        }

        public void Execute(WorkflowContext context)
        {
            var tasks = parallelSteps.Select(step => Task.Run(() => step.Execute(context)));
            Task.WhenAll(tasks).Wait();
        }

        public void Rollback(WorkflowContext context)
        {
            foreach (var step in parallelSteps)
            {
                step.Rollback(context);
            }
        }
    }
}
