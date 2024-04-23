using UnityEngine;

namespace Example.Scripts.Game.Locations.Viewing
{
    [RequireComponent(
        typeof(Camera),
        typeof(Rigidbody2D),
        typeof(PolygonCollider2D)
        )]
    public class LocationIsometricCamera : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private PolygonCollider2D _polygonCollider2D;
        private Camera _camera;
        
        public float Width => Height * _camera.aspect;
        public float Height => _camera.orthographicSize * 2;
        public float ScreenWidth => _camera.pixelWidth;
        public float ScreenHeight => _camera.pixelHeight;
        
        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _polygonCollider2D = GetComponent<PolygonCollider2D>();
            _camera = GetComponent<Camera>();
        }

        public void SetPosition(Vector3 position)
        {
            _rigidbody2D.MovePosition(position);
        }

        public void EnableMove()
        {
            _rigidbody2D.simulated = true;
        }

        public void DisableMove()
        {
            _rigidbody2D.simulated = false;
            _rigidbody2D.velocity = Vector2.zero;
        }

        public void UpdateCollider()
        {
            var points = _polygonCollider2D.points;

            points[0].x = Width / 2;
            points[0].y = Height / 2;
            
            points[1].x = -Width / 2;
            points[1].y = Height / 2;
            
            points[2].x = -Width / 2;
            points[2].y = -Height / 2;
            
            points[3].x = Width / 2;
            points[3].y = -Height / 2;

            _polygonCollider2D.points = points;
        }
    }
}