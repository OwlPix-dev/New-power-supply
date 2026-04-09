using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewScreenSettings", menuName = "Phone/ScreenSettings")]
public class PhoneUIScreen : ScriptableObject
{
    [SerializeField] private Vector2 _screenPosition = new Vector2(430, 185);
    [SerializeField] private Vector2 _screenSize = new Vector2(500, 700);

    [SerializeField] private PhoneApp _defaultApp;
    private PhoneApp _currentApp;

    private UIDocument _appUIDocument;

    public Vector2 ScreenPosition => _screenPosition;
    public Vector2 ScreenSize => _screenSize;

    public PhoneApp DefaultApp => _defaultApp;
    public PhoneApp CurrentApp => _currentApp;

    public UIDocument AppUIDocument => _appUIDocument;

    public void OpenApp(PhoneApp newApp, PhoneUIController phoneController)
    {
        newApp = newApp ?? _defaultApp;

        if (_currentApp == newApp) { return; }

        _currentApp = newApp;

        if (_appUIDocument == null)
        {
            GameObject screenObject = new GameObject("Screen");
            screenObject.transform.SetParent(phoneController.ScreensObject.transform);

            _appUIDocument = screenObject.AddComponent<UIDocument>();
            _appUIDocument.panelSettings = phoneController.ScreenPanelSettings;
        }
        else
        {
            _appUIDocument.enabled = true;
        }

        _appUIDocument.visualTreeAsset = _currentApp.AppUI;

        VisualElement root = _appUIDocument.rootVisualElement;

        root.style.left = _screenPosition.x;
        root.style.top = _screenPosition.y;

        root.style.width = _screenSize.x;
        root.style.height = _screenSize.y;

        _currentApp.OpenApp(this, phoneController);
    }

    public void CloseApp(PhoneUIController phoneController)
    {
        if (_currentApp == null) { return; }

        _currentApp.CloseApp(this, phoneController);
        _currentApp = null;

        if (_appUIDocument != null) { _appUIDocument.enabled = false; }
    }

    public void OpenScreen(PhoneApp newApp, PhoneUIController phoneController)
    {
        OpenApp(newApp, phoneController);
    }

    public void CloseScreen(PhoneUIController phoneController)
    {
        if (_currentApp != null) { _currentApp.PhoneController = null; }
        if (_currentApp != null) { _currentApp.AppScreens.Clear(); }

        CloseApp(phoneController);
    }
}
