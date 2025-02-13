using System;
using System.Threading.Tasks;

namespace Workflow.Core.Steps
{
    public class RetryStep : IStep
    {
        private readonly IStep innerStep;
        private readonly int maxRetries;
        private readonly TimeSpan retryInterval;

        public RetryStep(IStep innerStep, int maxRetries, TimeSpan retryInterval)
        {
            this.innerStep = innerStep;
            this.maxRetries = maxRetries;
            this.retryInterval = retryInterval;
        }

        public void Execute(WorkflowContext context)
        {
            int attempt = 0;
            while (attempt < maxRetries)
            {
                try
                {
                    innerStep.Execute(context);
                    return;
                }
                catch (Exception)
                {
                    attempt++;
                    if (attempt >= maxRetries)
                    {
                        throw;
                    }
                    Task.Delay(retryInterval).Wait();
                }
            }
        }

        public void Rollback(WorkflowContext context)
        {
            innerStep.Rollback(context);
        }
    }
}
