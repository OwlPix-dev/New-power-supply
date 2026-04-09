using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PhoneApp : ScriptableObject
{
    [SerializeField] private VisualTreeAsset _appUI;

    [SerializeField] private string _appName = "App";

    [SerializeField] private Sprite _appIcon;

    private PhoneUIController _phoneController;
    private List<PhoneUIScreen> _appScreens = new List<PhoneUIScreen>();

    public VisualTreeAsset AppUI => _appUI;

    public string AppName => _appName;

    public Sprite AppIcon => _appIcon;

    public PhoneUIController PhoneController
    {
        get => _phoneController;
        set => _phoneController = value;
    }

    public List<PhoneUIScreen> AppScreens
    {
        get => _appScreens;
        set => _appScreens = value;
    }

    public virtual void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        _appScreens.Add(appScreen);
        _phoneController = phoneController;
    }

    public virtual void CloseApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        _appScreens.Remove(appScreen);
    }

    public virtual void BackClick(PhoneUIScreen appScreen, PhoneUIController phoneController) { }
}
