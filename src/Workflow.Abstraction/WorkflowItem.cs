using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflow.Abstraction
{
    public abstract class WorkflowItem<TEnum>
        where TEnum : Enum
    {
        public WorkflowItemState<TEnum> CurrentItemState { get; set; }

        public abstract void StepTo(TEnum state);
        public abstract void Init(TEnum state);

        public virtual ValidationResult Validate(IEnumerable<WorkflowRule<WorkflowItem<TEnum>, TEnum>> rules)
        {
            var result = ValidationResult.Ok();
            result = rules.Aggregate(result, (current, rule) => current.And(rule(this)));
            return result;
        }
    }
}