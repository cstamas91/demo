using System;

namespace Workflow.Abstraction
{
    public class WorkflowTransition<TTransitionEnum, TStateEnum>
        where TTransitionEnum : Enum
        where TStateEnum : Enum
    {
        public TTransitionEnum Id { get; set; }
        public TStateEnum FromStateId { get; set; }
        public TStateEnum ToStateId { get; set; }
        public string Name { get; set; }
    }
}