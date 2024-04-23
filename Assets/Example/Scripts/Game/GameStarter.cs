using Example.Scripts.Architecture.Assets;
using Example.Scripts.Architecture.Input;
using Example.Scripts.Architecture.Loading.UI;
using Example.Scripts.Architecture.LocalConfigs;
using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Architecture.UI;
using Example.Scripts.Game.Locations;
using Example.Scripts.Game.Locations.Viewing;
using Example.Scripts.Patterns.DI;
using UnityEngine;

namespace Example.Scripts.Game
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private UIHierarchy uiHierarchy;
        
        private GlobalContainer _container;
        private LoadingScreenController _loadingScreenController;
        
        private void Awake()
        {
            Physics2D.simulationMode = SimulationMode2D.Update;
            
            CreateModules();

            _container.Get<UIManager>().SetHierarchy(uiHierarchy);
            
            var queue = new TasksQueue();
            AddPreloadTasksToQueue(queue);
            queue.AddTask(new TaskAction(_container.InitAll));
            AddExecutableTasksToQueue(queue);
            
            queue.OnComplete += HandleLoadingComplete;

            _loadingScreenController = _container.Get<LoadingScreenController>();
            _loadingScreenController.ShowWindow();
            _loadingScreenController.SetTasksQueue(queue);
            
            queue.Start();
        }

        private void CreateModules()
        {
            _container = GlobalContainer.Instance;
            
            _container.Add<ScriptableObjectsProvider>();
            _container.Add<UIManager>();
            _container.Add<LoadingScreenController>();
            _container.Add<AssetsManager>();
            _container.Add<LocationsManager>();
            _container.Add<LocationCameraDragManager>();
            _container.Add<InputManager>();
            
            _container.InjectAll();
        }

        private void AddPreloadTasksToQueue(TasksQueue queue)
        {
            queue.AddTask(_container.Get<ScriptableObjectsProvider>().Preload());
        }
        
        private void AddExecutableTasksToQueue(TasksQueue queue)
        {
            queue.AddTask(_container.Get<LocationsManager>().Execute());
        }

        private void HandleLoadingComplete(ITask task)
        {
            _loadingScreenController.HideWindow();
        }
    }
}