using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Example.Scripts.Patterns.Singleton
{
    public class SingletonScriptableObject : ScriptableObject { }
    public class SingletonScriptableObject<T> : SingletonScriptableObject where T : SingletonScriptableObject<T>
    {
        private static readonly Lazy<T> _instance = new (LoadAsset);
        public static T Instance => _instance.Value;

        private static T LoadAsset()
        {
            var handle = Addressables.LoadAssetAsync<T>("Config");
            handle.Completed += _ =>
            {
                Debug.Log(handle.Result);
            };
            return handle.Result;
        }
    }
}