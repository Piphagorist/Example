using UnityEngine;

namespace Example.Scripts.Architecture.Input
{
    [CreateAssetMenu(fileName = nameof(InputSettings), menuName = "[SakuraGirls2]/Input/InputSettings")]
    public class InputSettings : ScriptableObject
    {
        [SerializeField] private float maxClickDistance;
        [SerializeField] private float maxClickDuration;

        public float MaxClickDistance => maxClickDistance;
        public float MaxClickDuration => maxClickDuration;
    }
}