using System;
using System.Collections.Generic;

namespace Workflow.Abstraction
{
    public struct ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }

        public static ValidationResult Error(string message)
        {
            return message;
        }

        public static ValidationResult Ok()
        {
            return new ValidationResult() {IsValid = true, Errors = new List<string>()};
        }

        public ValidationResult And(ValidationResult other)
        {
            var result = new ValidationResult() {IsValid = this.IsValid && other.IsValid, Errors = this.Errors };
            result.Errors.AddRange(other.Errors);
            return result;
        }

        public static implicit operator ValidationResult(string message) => new ValidationResult
        {
            IsValid = false, Errors = new List<string>{message}
        };
    }
    
    public delegate ValidationResult WorkflowRule<TItem, TStateEnum>(TItem item)
        where TItem : WorkflowItem<TStateEnum>
        where TStateEnum : Enum;

    public delegate ValidationResult WorkflowRule<TItem, TStateEnum, TTransitionEnum>(
        TItem item,
        WorkflowTransition<TTransitionEnum, TStateEnum> transition)
        where TItem : WorkflowItem<TStateEnum>
        where TStateEnum : Enum
        where TTransitionEnum : Enum;
}
