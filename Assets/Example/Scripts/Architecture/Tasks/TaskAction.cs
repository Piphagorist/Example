using System;

namespace Example.Scripts.Architecture.Tasks
{
    public class TaskAction : TaskObject
    {
        private readonly Action _action;

        public TaskAction(Action action)
        {
            _action = action;
        }
        
        protected override void StartInternal()
        {
            _action?.Invoke();
            InvokeComplete();
        }
    }
}