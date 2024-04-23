using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Patterns.DI;

namespace Example.Scripts.Architecture.Connection
{
    public class RequestTask : TaskObject
    {
        [Inject] private ConnectionController _connectionController;

        public string Result { get; private set; }
        
        private string _url;
        private IPostData _data;
        
        public RequestTask(string url, IPostData data)
        {
            _url = url;
            _data = data;
        }
        
        protected override void StartInternal()
        {
            _connectionController.PostRequest(_url, _data, HandleRequestProgressChanged, HandleRequestComplete);
        }

        private void HandleRequestProgressChanged(float progress)
        {
            Progress = progress;
        }
        
        private void HandleRequestComplete(string result)
        {
            Result = result;
            InvokeComplete();
        }
    }
}