using Example.Scripts.Architecture.Tasks;
using Example.Scripts.Architecture.UI;
using Example.Scripts.Extensions;
using TMPro;
using UnityEngine;

namespace Example.Scripts.Architecture.Loading.UI
{
    public class LoadingScreen : UIWindow
    {
        [SerializeField] private TMP_Text progressTxt;

        public void SetProgress(float progress)
        {
            progressTxt.text = progress.GetPercents();
        }
    }
}