using System;
using System.Collections;
using Example.Scripts.Patterns.Singleton;
using UnityEngine;

namespace Example.Scripts.Tools
{
    public class UnityEventsProvider : SingletonMonoBehaviour<UnityEventsProvider>
    {
        public event Action OnUpdate;
        public event Action<bool> OnFocus;

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnFocus?.Invoke(hasFocus);
        }

        public static Coroutine CoroutineStart(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        public static void CoroutineStop(IEnumerator routine)
        {
            Instance.StopCoroutine(routine);
        }
    }
}