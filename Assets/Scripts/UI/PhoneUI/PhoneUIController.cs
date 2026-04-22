using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneUIController : UIState
{
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private List<PhoneApp> _menuApps = new List<PhoneApp>();

    [SerializeField] private OpenPhoneByKeyCode[] _opensPhone;

    [SerializeField] private GameObject _screensObject;
    [SerializeField] private PanelSettings _screenPanelSettings;

    [SerializeField] private UIDocument _phoneFrameUIDocument;

    [SerializeField] private PhonePositionSynchronization[] _phonePositionSynchronizations;

    [SerializeField] private PhonePositionType[] _availablePhonePositions;

    private PhonePositionType _currentPhonePosition;

    private List<NewScreen> _lastScreens = new List<NewScreen>();

    public UIManager UIManager => _uIManager;

    public List<PhoneApp> MenuApps => _menuApps;

    public GameObject ScreensObject => _screensObject;
    public PanelSettings ScreenPanelSettings => _screenPanelSettings;

    public UIDocument PhoneFrameUIDocument => _phoneFrameUIDocument;

    public PhonePositionType CurrentPhonePosition => _currentPhonePosition;

    public bool IsPhoneOpen => _currentPhonePosition != null;

    private void Update()
    {
        foreach (OpenPhoneByKeyCode openPhone in _opensPhone)
        {
            if (Input.GetKeyDown(openPhone.KeyCode))
            {
                _uIManager.BasicUIController.OpenUI(_uIManager);
            }
        }
    }

    public override void UpdatePassiveUI(UIManager uIManager)
    {
        base.UpdatePassiveUI(uIManager);

        foreach (OpenPhoneByKeyCode openPhone in _opensPhone)
        {
            if (Input.GetKeyDown(openPhone.KeyCode))
            {
                OpenPhoneByScreens(openPhone.NewScreens);
            }
        }
    }

    public override void StartUI(UIManager uIManager)
    {
        base.StartUI(uIManager);

        foreach (PhonePositionType availablePosition in _availablePhonePositions)
        {
            foreach (PhoneUIScreen availablePositionScreen in availablePosition.Screens)
            {
                availablePositionScreen.CloseScreen(this);
            }
        }
    }

    public override void OpenUI(UIManager uIManager)
    {
        if (IsPhoneOpen == true) { return; }

        base.OpenUI(uIManager);
    }

    public override void CloseUI(UIManager uIManager)
    {
        if (IsPhoneOpen == false) { return; }

        base.CloseUI(uIManager);

        SetPhoneFrame(false);

        foreach (PhoneUIScreen currentPositionScreen in _currentPhonePosition.Screens)
        {
            currentPositionScreen.CloseScreen(this);
        }

        _currentPhonePosition = null;
    }

    public override void ResumeUI(UIManager uIManager)
    {
        base.ResumeUI(uIManager);

        OpenPhoneByScreens(_lastScreens.ToArray());
    }

    public bool OpenPhoneByScreens(params NewScreen[] newScreens)
    {
        foreach (PhonePositionType availablePosition in _availablePhonePositions)
        {
            if (availablePosition.Screens.SequenceEqual(newScreens.Select(newScreen => newScreen.Screen)) == true)
            {
                PhoneApp[] newPositionApps = newScreens.Select(newScreen => newScreen.App).ToArray();

                return SetCurrentPhonePosition(availablePosition, newPositionApps);
            }
        }

        return false;
    }

    public bool OpenPhoneByPhonePosition(PhonePositionType newPhonePosition, params PhoneApp[] newApps)
    {
        if (newPhonePosition.Screens.Length != newApps.Length) { return false; }

        return SetCurrentPhonePosition(newPhonePosition, newApps);
    }

    public bool SetScreenApp(NewScreen newScreen)
    {
        if (IsPhoneOpen == false) { return false; }
        
        foreach (PhoneUIScreen currentPositionScreen in _currentPhonePosition.Screens)
        {
            if (currentPositionScreen == newScreen.Screen)
            {
                currentPositionScreen.OpenApp(newScreen.App, this);
                return true;
            }
        }

        return false;
    }

    public bool SetScreenAppByIndex(int index, PhoneApp newApp)
    {
        if (IsPhoneOpen == false ||
            _currentPhonePosition.Screens.Length >= index + 1 ||
            _currentPhonePosition.Screens[index].CurrentApp == newApp)
        {
            return false;
        }

        _currentPhonePosition.Screens[index].OpenApp(newApp, this);

        return true;
    }

    private bool SetCurrentPhonePosition(PhonePositionType newPhonePosition, PhoneApp[] newApps)
    {
        bool ComparePositionSynchronization(PhonePositionType mainPhonePosition, PhonePositionType comparePhonePosition, PhoneUIScreen mainScreen)
        {
            if (_currentPhonePosition == mainPhonePosition &&
                newPhonePosition == comparePhonePosition)
            {
                PhoneUIScreen oldPhoneScreen = _currentPhonePosition.Screens.FirstOrDefault(screen => screen == mainScreen);

                PhoneApp newApp = oldPhoneScreen.CurrentApp;

                int screenIndex = Array.IndexOf(newPhonePosition.Screens, oldPhoneScreen);
                newApps[screenIndex] = newApp;

                return true;
            }
            return false;
        }

        if (_currentPhonePosition == newPhonePosition) { return false; }

        OpenUI(_uIManager);
        SetPhoneFrame(true);

        foreach (PhonePositionSynchronization positionSynchronization in _phonePositionSynchronizations)
        {
            PhonePositionType mainPhonePosition = positionSynchronization.MainPhonePosition;
            PhonePositionType comparePhonePosition = positionSynchronization.ComparePhonePosition;

            PhoneUIScreen mainScreen = positionSynchronization.MainScreen;
            PhoneUIScreen compareScreen = positionSynchronization.CompareScreen;

            if (ComparePositionSynchronization(mainPhonePosition, comparePhonePosition, mainScreen) == true ||
                ComparePositionSynchronization(comparePhonePosition, mainPhonePosition, compareScreen) == true)
            {
                break;
            }
        }

        _currentPhonePosition?.ClosePosition(this);
        _currentPhonePosition = newPhonePosition;
        _currentPhonePosition?.OpenPosition(this);

        for (int screenIndex = 0; screenIndex < _currentPhonePosition.Screens.Length; screenIndex++)
        {
            _currentPhonePosition.Screens[screenIndex].OpenScreen(newApps[screenIndex], this);
        }

        if (_currentPhonePosition != null)
        {
            _lastScreens.Clear();
            foreach (PhoneUIScreen screen in _currentPhonePosition.Screens)
            {
                _lastScreens.Add(new NewScreen(screen, screen.CurrentApp));
            }
        }

        return true;
    }

    private void SetPhoneFrame(bool isActive)
    {
        if (IsPhoneOpen == isActive) { return; }

        _phoneFrameUIDocument.enabled = isActive;
    }
}

[System.Serializable]
public class NewScreen
{
    [SerializeField] private PhoneUIScreen _screen;
    [SerializeField] private PhoneApp _app;

    public PhoneUIScreen Screen => _screen;
    public PhoneApp App => _app;

    public NewScreen(PhoneUIScreen screen, PhoneApp app)
    {
        _screen = screen;
        _app = app;
    }
}

[System.Serializable]
public class PhonePositionSynchronization
{
    [SerializeField] private PhonePositionType _mainPhonePosition;
    [SerializeField] private PhoneUIScreen _mainScreen;

    [Space]

    [SerializeField] private PhonePositionType _comparePhonePosition;
    [SerializeField] private PhoneUIScreen _compareScreen;

    public PhonePositionType MainPhonePosition => _mainPhonePosition;
    public PhoneUIScreen MainScreen => _mainScreen;

    public PhonePositionType ComparePhonePosition => _comparePhonePosition;
    public PhoneUIScreen CompareScreen => _compareScreen;
}

[System.Serializable]
public class OpenPhoneByKeyCode
{
    [SerializeField] private KeyCode _keyCode;
    [SerializeField] private NewScreen[] _newScreens;

    public KeyCode KeyCode => _keyCode;
    public NewScreen[] NewScreens => _newScreens;
}