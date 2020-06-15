using System;

namespace Workflow.Abstraction
{
    public abstract class Workflow<TEnum>
        where TEnum : Enum
    {
        public TEnum Id { get; set; }
        public string Name { get; set; }
    }
}
