using UnityEngine;

namespace Example.Scripts.Game.Locations.Viewing
{
    [CreateAssetMenu(fileName = nameof(LocationDragSettings), menuName = "[SakuraGirls2]/Locations/DragSettings")]
    public class LocationDragSettings : ScriptableObject
    {
        [SerializeField] private float dragSpeed;
        [SerializeField] private float inertiaTime;
        [SerializeField] private AnimationCurve inertiaCurve;

        public float DragSpeed => dragSpeed;
        public float InertiaTime => inertiaTime;
        public AnimationCurve InertiaCurve => inertiaCurve;
    }
}