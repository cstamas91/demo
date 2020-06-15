using System;

namespace Workflow.Abstraction.Implementation
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            DentistService.Run();
            Console.ReadKey();
        }
    }
}
