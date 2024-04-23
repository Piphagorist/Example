using Example.Scripts.Patterns.DI;

namespace Example.Scripts.Architecture.UI
{
    public abstract class UIWindowController<Window> : SharedObject where Window : UIWindow
    {
        [Inject] private UIManager _uiManager;

        protected Window _window;

        public void ShowWindow()
        {
            if (_window == null)
                _window = GetWindow();
            
            if (_window.IsActive) return; 
            
            _window.Show();
        }

        public void HideWindow()
        {
            if (_window == null) return;
            if (!_window.IsActive) return;
            
            _window.Hide();
        }

        private Window GetWindow()
        {
            return _uiManager.GetWindow<Window>();
        }
    }
}