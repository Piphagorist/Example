using System;
using UnityEngine;

namespace Example.Scripts.Patterns.Singleton
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static readonly Lazy<T> _instance = new (CreateInstance);
        public static T Instance => _instance.Value;

        protected SingletonMonoBehaviour() {}

        private static T CreateInstance()
        {
            GameObject go = new();
            go.name = typeof(T).Name;
            DontDestroyOnLoad(go);

            return go.AddComponent(typeof(T)) as T;
        }
    }
}