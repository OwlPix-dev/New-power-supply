using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneUIController : GameUI
{
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private List<PhoneApp> _menuApps = new List<PhoneApp>();

    [SerializeField] private GameObject _screensObject;
    [SerializeField] private PanelSettings _screenPanelSettings;

    [SerializeField] private UIDocument _phoneFrameUIDocument;

    [SerializeField] private PhonePositionSynchronization[] _phonePositionSynchronizations;

    [SerializeField] private PhonePositionType[] _availablePhonePositions;

    private PhonePositionType _currentPhonePosition;

    public List<PhoneApp> MenuApps => _menuApps;

    public GameObject ScreensObject => _screensObject;
    public PanelSettings ScreenPanelSettings => _screenPanelSettings;

    public UIDocument PhoneFrameUIDocument => _phoneFrameUIDocument;

    public PhonePositionType CurrentPhonePosition => _currentPhonePosition;

    public bool IsPhoneOpen => _currentPhonePosition != null;

    public override void StartUI(UIManager uIManager)
    {
        base.StartUI(uIManager);

        foreach (PhonePositionType availablePosition in _availablePhonePositions)
        {
            foreach (PhoneUIScreen availablePositionScreen in availablePosition.Screens)
            {
                availablePositionScreen.CloseApp(this);
            }
        }
    }

    public override void OpenUI(UIManager uIManager)
    {
        base.OpenUI(uIManager);

        enabled = true;
    }

    public override void CloseUI(UIManager uIManager)
    {
        base.CloseUI(uIManager);

        ClosePhone();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uIManager.CurrentUI = _uIManager.BasicUIController;
        }
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

    public void ClosePhone()
    {
        if (IsPhoneOpen == false) { return; }

        SetPhoneFrame(false);

        foreach (PhoneUIScreen currentPositionScreen in _currentPhonePosition.Screens)
        {
            currentPositionScreen.CloseScreen(this);
        }

        _currentPhonePosition = null;

        enabled = false;
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

        if (_uIManager.CurrentUI != this) { _uIManager.CurrentUI = this; }

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