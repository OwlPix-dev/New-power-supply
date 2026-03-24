using System;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class PhoneApp : ScriptableObject
{
    [SerializeField] private VisualTreeAsset _appUI;

    [SerializeField] private string _appName = "App";

    [SerializeField] private Sprite _appIcon;

    public VisualTreeAsset AppUI => _appUI;

    public string AppName => _appName;

    public Sprite AppIcon => _appIcon;

    public virtual void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController) { }
    public virtual void CloseApp(PhoneUIScreen appScreen, PhoneUIController phoneController) { }

    public virtual void BackClick(PhoneUIScreen appScreen, PhoneUIController phoneController) { }
}
