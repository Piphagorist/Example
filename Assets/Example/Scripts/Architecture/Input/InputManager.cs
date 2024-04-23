using System;
using Example.Scripts.Architecture.LocalConfigs;
using Example.Scripts.Patterns.DI;
using Example.Scripts.Tools;
using UnityEngine;

namespace Example.Scripts.Architecture.Input
{
    public class InputManager : SharedObject
    {
        [Inject] private ScriptableObjectsProvider _scriptableObjectsProvider;
        
        public event Action<Vector3> OnClick;
        public event Action<Vector3> OnDragStart;
        public event Action<Vector3> OnDrag;
        public event Action<Vector3> OnDragEnd;
        
        private Vector3 _mouseDownPosition;
        private Vector3 _lastMousePosition;

        private float _mouseDownTime;

        private bool _isDragging;

        private InputSettings _inputSettings;
        
        public override void Init()
        {
            UnityEventsProvider.Instance.OnUpdate += HandleUnityUpdate;
            _inputSettings = _scriptableObjectsProvider.GetConfig<InputSettings>();
        }

        private void HandleUnityUpdate()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
                OnMouseDown();
            if (UnityEngine.Input.GetMouseButtonUp(0))
                OnMouseUp();
            if (UnityEngine.Input.GetMouseButton(0))
                OnMouseHold();
        }
        
        private void OnMouseDown()
        {
            _mouseDownPosition = GetMousePosition();
            _lastMousePosition = _mouseDownPosition;
            _mouseDownTime = Time.time;
        }

        private void OnMouseHold()
        {
            _lastMousePosition = GetMousePosition();

            if (_isDragging)
            {
                OnDrag?.Invoke(_lastMousePosition);
                return;
            }

            float duration = Time.time - _mouseDownTime;
            float distance = Vector3.Distance(_mouseDownPosition, GetMousePosition());

            if (CheckDrag(duration, distance))
            {
                _isDragging = true;
                OnDragStart?.Invoke(_lastMousePosition);
            }
        }

        private void OnMouseUp()
        {
            Vector3 mousePosition = GetMousePosition();

            if (!_isDragging)
                OnClick?.Invoke(mousePosition);
            else
                OnDragEnd?.Invoke(mousePosition);

            _isDragging = false;
        }

        private Vector3 GetMousePosition()
        {
            return UnityEngine.Input.mousePosition;
        }

        private bool CheckDrag(float duration, float distance)
        {
            return duration > _inputSettings.MaxClickDuration ||
                   distance > _inputSettings.MaxClickDistance;
        }
    }
}