using System;

namespace Workflow.Abstraction
{
    public class WorkflowState<TEnum>
        where TEnum: Enum
    {
        public string Name { get; set; }
        public TEnum Id { get; set; }
    }
}