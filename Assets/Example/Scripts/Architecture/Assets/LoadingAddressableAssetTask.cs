using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Patterns.DI;

namespace Example.Scripts.Architecture.Assets
{
    public class LoadingAddressableAssetTask<T> : TaskObject
    {
        [Inject] private AssetsManager _assetsManager;
        
        public T Result { get; private set; }
        
        private string _label;
        
        public LoadingAddressableAssetTask(string label)
        {
            _label = label;
        }
        
        protected override void StartInternal()
        {
            _assetsManager.LoadAssetFromAddressable<T>(_label, HandleLoadingProgress, HandleLoadingComplete);
        }
        
        private void HandleLoadingProgress(float progress)
        {
            Progress = progress;
        }

        private void HandleLoadingComplete(T result)
        {
            Result = result;
            InvokeComplete();
        }
    }
}