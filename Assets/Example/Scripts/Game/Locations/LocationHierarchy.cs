using Example.Scripts.Game.Locations.Viewing;
using Example.Scripts.Patterns.DI;
using UnityEngine;

namespace Example.Scripts.Game.Locations
{
    public class LocationHierarchy : InjectedMonoBehaviour
    {
        [Inject] private LocationsManager _locationsManager;

        [SerializeField] private LocationIsometricCamera locationCamera;

        public LocationIsometricCamera LocationCamera => locationCamera;

        private void OnEnable()
        {
            _locationsManager.SetHierarchy(this);
        }
    }
}