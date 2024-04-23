using System;
using Example.Scripts.Architecture.Assets;
using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Patterns.DI;

namespace Example.Scripts.Game.Locations
{
    public class LocationsManager : SharedObject, IExecutable
    {
        [Inject] private AssetsManager _assetsManager;

        public event Action OnLocationLoaded;

        public LocationHierarchy Hierarchy { get; private set; }

        public ITask Execute()
        {
            var queue = new TasksQueue();
            queue.AddTask(_assetsManager.LoadSceneFromAddressableTask("Game"));

            return queue;
        }

        public void SetHierarchy(LocationHierarchy hierarchy)
        {
            Hierarchy = hierarchy;
            OnLocationLoaded?.Invoke();
        }
    }
}