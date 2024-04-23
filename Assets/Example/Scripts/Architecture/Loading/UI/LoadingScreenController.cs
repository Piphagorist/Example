using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Architecture.UI;

namespace Example.Scripts.Architecture.Loading.UI
{
    public class LoadingScreenController : UIWindowController<LoadingScreen>
    {
        public void SetTasksQueue(TasksQueue tasksQueue)
        {
            tasksQueue.OnUpdate += HandleQueueUpdate;
        }

        private void HandleQueueUpdate(ITask obj)
        {
            _window.SetProgress(obj.Progress);
        }
    }
}