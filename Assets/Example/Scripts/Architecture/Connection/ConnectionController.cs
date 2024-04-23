using System;
using System.Collections;
using System.Collections.Generic;
using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Patterns.DI;
using Example.Scripts.Tools;
using UnityEngine;
using UnityEngine.Networking;

namespace Example.Scripts.Architecture.Connection
{
    public class ConnectionController : SharedObject
    {
        private List<UnityWebRequest> _activeRequests = new();

        public bool HasActiveRequest => _activeRequests.Count > 0;

        public virtual void PostRequest(string url, IPostData data, Action<float> OnProgress = null,
            Action<string> OnComplete = null)
        {
            var form = GetFormFromData(data);

            UnityEventsProvider.CoroutineStart(Post(url, form, OnProgress, OnComplete));
        }

        public ITask PostRequestTask(string url, IPostData data)
        {
            return new RequestTask(url, data);
        }
        
        protected IEnumerator Post(string url, WWWForm data, Action<float> OnProgress = null,
            Action<string> OnComplete = null)
        {
            UnityWebRequest webRequest = UnityWebRequest.Post(url, data);
            webRequest.disposeUploadHandlerOnDispose = true;
            webRequest.disposeDownloadHandlerOnDispose = true;
            
            _activeRequests.Add(webRequest);

            webRequest.SendWebRequest();

            while (!webRequest.isDone)
            {
                OnProgress?.Invoke(webRequest.downloadProgress);
                yield return null;
            }

            _activeRequests.Remove(webRequest);
            
            OnProgress?.Invoke(1.0f);
            OnComplete?.Invoke(webRequest.downloadHandler.text);
            
            webRequest.Dispose();
        }

        protected virtual WWWForm GetFormFromData(IPostData data) => null;
    }
}