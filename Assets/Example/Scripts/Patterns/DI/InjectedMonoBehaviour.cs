using System;
using UnityEngine;

namespace Example.Scripts.Patterns.DI
{
    public class InjectedMonoBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            GlobalContainer.Instance.InjectAt(this);
            Init();
        }

        protected virtual void Init() {}
    }
}