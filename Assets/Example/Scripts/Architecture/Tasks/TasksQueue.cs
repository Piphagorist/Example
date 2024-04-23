using System.Collections.Generic;
using System.Threading.Tasks;

namespace Example.Scripts.Architecture.Tasks
{
    public class TasksQueue : TaskObject
    {
        private readonly List<ITask> _tasks = new();
        private int _currentTaskIndex = -1;

        public void AddTask(ITask task)
        {
            _tasks.Add(task);
        }

        protected override void StartInternal()
        {
            TryToStartNextTask();
        }

        private async void TryToStartNextTask()
        {
            if (_currentTaskIndex == _tasks.Count - 1)
            {
                InvokeComplete();
                return;
            }
            
            await Task.Delay(100);

            _currentTaskIndex++;
            
            _tasks[_currentTaskIndex].OnUpdate += HandleTaskUpdate;
            _tasks[_currentTaskIndex].OnComplete += HandleTaskComplete;
            _tasks[_currentTaskIndex].Start();
        }

        private void HandleTaskUpdate(ITask task)
        {
            UpdateProgress();
        }

        private void HandleTaskComplete(ITask task)
        {
            _tasks[_currentTaskIndex].OnUpdate -= HandleTaskUpdate;
            _tasks[_currentTaskIndex].OnComplete -= HandleTaskComplete;
            
            TryToStartNextTask();
        }

        private void UpdateProgress()
        {
            float progress = 0;
            
            foreach (ITask task in _tasks)
                progress += task.Progress;

            progress /= _tasks.Count;

            Progress = progress;
        }
    }
}