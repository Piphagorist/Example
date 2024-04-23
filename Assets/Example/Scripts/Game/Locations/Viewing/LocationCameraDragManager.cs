using Example.Scripts.Architecture.Input;
using Example.Scripts.Architecture.LocalConfigs;
using Example.Scripts.Patterns.DI;
using Example.Scripts.Tools;
using UnityEngine;

namespace Example.Scripts.Game.Locations.Viewing
{
    public class LocationCameraDragManager : SharedObject
    {
        [Inject] private InputManager _inputManager;
        [Inject] private LocationsManager _locationManager;
        [Inject] private ScriptableObjectsProvider _scriptableObjectsProvider;

        private Vector3 _lastPosition;
        private Vector3 _lastCameraPosition;
        private Vector3 _velocity;
        private float _timeStartInertia;
        private float _lastScreenWidth;
        private float _lastScreenHeight;

        private bool _inertia;

        private LocationIsometricCamera _locationCamera;
        private LocationDragSettings _dragSettings;
        
        public override void Init()
        {
            _dragSettings = _scriptableObjectsProvider.GetConfig<LocationDragSettings>();
            
            _locationManager.OnLocationLoaded += HandleLocationLoaded;
            
            _inputManager.OnDragStart += HandleDragStart;
            _inputManager.OnDrag += HandleDrag;
            _inputManager.OnDragEnd += HandleDragEnd;

            UnityEventsProvider.Instance.OnUpdate += HandleUnityUpdate;
        }

        private void HandleLocationLoaded()
        {
            _locationCamera = _locationManager.Hierarchy.LocationCamera;
            CheckScreenChanged();
        }

        private void HandleDragStart(Vector3 position)
        {
            _inertia = false;
            _lastPosition = ScreenToIsometricPosition(position);
            _locationCamera.EnableMove();
        }
        
        private void HandleDrag(Vector3 position)
        {
            position = ScreenToIsometricPosition(position);
            
            if (position == _lastPosition) return;
            
            Vector3 cameraPosition = _locationCamera.transform.position;
            Vector3 deltaPosition = (position - _lastPosition) * -1.0f * _dragSettings.DragSpeed;
            Vector3 newPosition = cameraPosition + deltaPosition;

            _lastCameraPosition = cameraPosition;
            _locationCamera.SetPosition(newPosition);

            _lastPosition = position;
        }

        private void HandleDragEnd(Vector3 position)
        {
            _inertia = true;
            
            Vector3 camPos = _locationCamera.transform.position;
            _velocity = camPos - _lastCameraPosition;
            _timeStartInertia = Time.time;
        }

        private Vector3 ScreenToIsometricPosition(Vector3 position)
        {
            float cameraXPosition = position.x / _locationCamera.ScreenWidth * _locationCamera.Width;
            float cameraYPosition = position.y / _locationCamera.ScreenHeight * _locationCamera.Height;

            position.x = cameraXPosition;
            position.y = cameraYPosition;

            return position;
        }
        
        private void HandleUnityUpdate()
        {
            CheckScreenChanged();
            MoveInertial();
        }

        private void CheckScreenChanged()
        {
            if (_locationCamera == null) return;
            
            if (_lastScreenWidth == _locationCamera.ScreenWidth && _lastScreenHeight == _locationCamera.ScreenHeight)
                return;

            _lastScreenWidth = _locationCamera.ScreenWidth;
            _lastScreenHeight = _locationCamera.ScreenHeight;
            _locationCamera.UpdateCollider();
        }

        private void MoveInertial()
        {
            if (!_inertia) return;

            float inertiaTime = Time.time - _timeStartInertia;

            if (inertiaTime >= _dragSettings.InertiaTime)
            {
                _locationCamera.DisableMove();
                _inertia = false;
                return;
            }

            float inertiaValue = _dragSettings.InertiaCurve.Evaluate(inertiaTime);
            var camPos = _locationCamera.transform.position;
            
            _locationCamera.SetPosition(camPos + _velocity * inertiaValue);
        }
    }
}