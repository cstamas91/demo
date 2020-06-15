using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflow.Abstraction.Implementation
{
    public enum WorkflowEnum
    {
        DentistWorkflow
    }

    public enum DentistStates
    {
        Waiting,
        BeingExamined,
        WaitingForAnesthesia,
        InSurgery,
        Done
    }

    public enum DentistTransitions
    {
        SitInDentistChair,
        ReceiveAnesthesia,
        LeaveWithoutSurgery,
        ReturnToDentistChair,
        LeaveDentist
    }

    public static class DentistRepo
    {
        public static IEnumerable<WorkflowState<DentistStates>> _states =
            Enum.GetValues(typeof(DentistStates))
                .Cast<DentistStates>()
                .Select(e => new WorkflowState<DentistStates> {Name = e.ToString(), Id = e})
                .ToList();

        
        private static WorkflowTransition<DentistTransitions, DentistStates> Make(
            DentistTransitions transition,
            DentistStates from,
            DentistStates to)
        {
            return Make(transition, from, to, transition.ToString());
        }
        
        private static WorkflowTransition<DentistTransitions, DentistStates> Make(
            DentistTransitions transition,
            DentistStates from,
            DentistStates to,
            string name)
        {
            return new WorkflowTransition<DentistTransitions, DentistStates>
            {
                FromStateId = from, ToStateId = to, Id = transition, Name = name
            };
        }
        
        public static IEnumerable<WorkflowTransition<DentistTransitions, DentistStates>> _transitions =
            new List<WorkflowTransition<DentistTransitions, DentistStates>>
            {
                Make(DentistTransitions.SitInDentistChair, 
                    DentistStates.Waiting, 
                    DentistStates.BeingExamined),
                Make(DentistTransitions.ReceiveAnesthesia,
                    DentistStates.BeingExamined,
                    DentistStates.WaitingForAnesthesia),
                Make(DentistTransitions.ReturnToDentistChair, 
                    DentistStates.WaitingForAnesthesia, 
                    DentistStates.InSurgery),
                Make(DentistTransitions.LeaveDentist,
                    DentistStates.InSurgery,
                    DentistStates.Done),
                Make(DentistTransitions.LeaveWithoutSurgery,
                    DentistStates.BeingExamined,
                    DentistStates.Done)
            };
        
        public static IEnumerable<WorkflowRule<Patient, DentistStates>> GlobalRules()
        {
            yield return p => (p.CurrentItemState.StateId, p.IsDrowsy) switch
            {
                (DentistStates.WaitingForAnesthesia, true) => ValidationResult.Ok(),
                (DentistStates.InSurgery, true) => ValidationResult.Ok(),
                (_, true) => "Patient should only be under anesthesia after getting the shot and during surgery",
                (_, _) => ValidationResult.Ok()
            };
        }

        public static IEnumerable<WorkflowRule<Patient, DentistStates, DentistTransitions>> TransitionRules()
        {
            yield return (p, transition) =>
                (p.NeedsSurgery, transition.Id) switch
                {
                    (true, DentistTransitions.ReceiveAnesthesia) => ValidationResult.Ok(),
                    (false, DentistTransitions.ReceiveAnesthesia) => 
                        "Patient should only receive anesthesia when they need it.",
                    (_, _) => ValidationResult.Ok()
                };
        }
    }
    
    public static class DentistService
    {
        public static void Run()
        {
            var item = new Patient();
            item.Init(DentistStates.Waiting);
            item.StepTo(DentistStates.BeingExamined);
            item.NeedsSurgery = false;
            item.StepTo(DentistStates.WaitingForAnesthesia);
            item.IsDrowsy = true;
            item.StepTo(DentistStates.InSurgery);
            item.IsDrowsy = false;
            item.StepTo(DentistStates.Done);
        }
    }

    public class Patient : WorkflowItem<DentistStates>
    {
        public Patient()
        {
            CurrentItemState = null;
            IsDrowsy = false;
            NeedsSurgery = null;
        }
        public bool IsDrowsy { get; set; } = false;
        public bool? NeedsSurgery { get; set; } = null;

        public override void StepTo(DentistStates state)
        {
            var result = DentistRepo.GlobalRules()
                .Aggregate(
                    ValidationResult.Ok(),
                    (current, rule) => current.And(rule(this)));
            var transition = DentistRepo._transitions
                .FirstOrDefault(t => t.FromStateId == CurrentItemState.StateId &&
                                     t.ToStateId == state);
            if (transition == null)
                throw new WorkflowException("Invalid target state");
            result = DentistRepo.TransitionRules()
                .Aggregate(
                    result,
                    (current, rule) => current.And(rule(this, transition)));
            if (!result.IsValid)
                throw new WorkflowException(string.Join(Environment.NewLine, result.Errors));

            CurrentItemState = new WorkflowItemState<DentistStates>
            {
                Item = this, CreatedAt = DateTime.Now, StateId = state
            };
        }

        public override void Init(DentistStates state)
        {
            if (CurrentItemState != null)
                throw new WorkflowException("WorkflowItem is already in the middle of a workflow");
            
            CurrentItemState = new WorkflowItemState<DentistStates>()
            {
                Item = this, CreatedAt = DateTime.Now, StateId = DentistStates.Waiting
            };
        }
    }

    public class WorkflowException : Exception
    {
        public WorkflowException(string message) : base(message)
        {
            
        }
    }
}
