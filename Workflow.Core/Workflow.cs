using System;
using System.Collections.Generic;

namespace Workflow.Core
{
    public class Workflow : IWorkflow
    {
        private readonly List<IStep> steps;
        private readonly List<IWorkflowMiddleware> middlewares;
        private int executingStepIndex = -1;
        private readonly object lockObject = new object();

        public Workflow(List<IStep> steps, List<IWorkflowMiddleware> workflowMiddlewares)
        {
            this.steps = steps ?? new List<IStep>();
            this.middlewares = workflowMiddlewares ?? new List<IWorkflowMiddleware>();
        }

        public virtual void Execute(WorkflowContext workflowContext)
        {
            foreach (var step in steps)
            {
                lock (lockObject)
                {
                    executingStepIndex++;
                }

                try
                {
                    // Handle cancellation if needed
                    if (workflowContext.CancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(workflowContext.CancellationToken);
                    }

                    foreach (var middleware in middlewares)
                    {
                        middleware.BeforeExecute(workflowContext, step);
                    }

                    step.Execute(workflowContext);

                    foreach (var middleware in middlewares)
                    {
                        middleware.AfterExecute(workflowContext, step);
                    }
                }
                catch (OperationCanceledException)
                {
                    Rollback(workflowContext);
                    throw;
                }
                catch (Exception ex)
                {
                    foreach (var middleware in middlewares)
                    {
                        middleware.OnError(workflowContext, step, ex);
                    }

                    Rollback(workflowContext);
                    throw;
                }
            }
        }

        public virtual void Rollback(WorkflowContext workflowContext)
        {
            lock (lockObject)
            {
                for (; executingStepIndex >= 0; executingStepIndex--)
                {
                    var step = steps[executingStepIndex];

                    try
                    {
                        foreach (var middleware in middlewares)
                        {
                            middleware.BeforeRollback(workflowContext, step);
                        }

                        step.Rollback(workflowContext);

                        foreach (var middleware in middlewares)
                        {
                            middleware.AfterRollback(workflowContext, step);
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (var middleware in middlewares)
                        {
                            middleware.OnError(workflowContext, step, ex);
                        }

                        throw;
                    }
                }
            }
        }
    }
}
