using UnityEngine;

namespace Example.Scripts.Architecture.UI
{
    public abstract class UIWindow : MonoBehaviour
    {
        [SerializeField] private GameObject content;

        public bool IsActive => content.activeSelf;
        
        public void Show()
        {
            content.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
        }
    }
}