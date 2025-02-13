﻿using Workflow.Core;

namespace Workflow.Sample
{
    public class Step1 : IStep
    {
        public void Execute(WorkflowContext context)
        {
            Console.WriteLine("Executing Step 1");
            context.Set("Step1Result", "Result from Step 1");
        }

        public void Rollback(WorkflowContext context)
        {
            Console.WriteLine("Compensating Step 1");
            context.Remove("Step1Result");
        }
    }
}
