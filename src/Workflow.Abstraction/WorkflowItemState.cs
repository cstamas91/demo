using System;

namespace Workflow.Abstraction
{
    public class WorkflowItemState<TStateEnum>
        where TStateEnum : Enum
    {
        public TStateEnum StateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public WorkflowItem<TStateEnum> Item { get; set; }
    }
}