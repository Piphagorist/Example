using System;
using Example.Scripts.Patterns.DI;

namespace Example.Scripts.Architecture.Tasks
{
    public abstract class TaskObject : ITask
    {
        public event Action<ITask> OnUpdate;
        public event Action<ITask> OnComplete;

        public virtual float Progress
        {
            get => _progress;
            protected set
            {
                _progress = value;
                InvokeUpdate();
            }
        }

        private float _progress;

        public void Start()
        {
            GlobalContainer.Instance.InjectAt(this);
            StartInternal();
        }
        protected abstract void StartInternal();

        protected void InvokeUpdate()
        {
            OnUpdate?.Invoke(this);
        }

        protected void InvokeComplete()
        {
            Progress = 1.0f;
            OnComplete?.Invoke(this);
            OnComplete = null;
            OnUpdate = null;
        }
    }
}