using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PhoneMenu : SystemicApp
{
    [SerializeField] private string _systemicAppsContainerClassName;
    [SerializeField] private string _externalAppsContainerClassName;

    [SerializeField] private string _menuAppContainerClassName;
    [SerializeField] private string _menuAppIconClassName;
    [SerializeField] private string _menuAppNameClassName;

    [SerializeField] private List<SystemicApp> _systemicApps = new List<SystemicApp>();
    private List<ExternalApp> _externalApps = new List<ExternalApp>();

    public List<SystemicApp> SystemicApps => _systemicApps;
    public List<ExternalApp> ExternalApps => _externalApps;

    public override void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.OpenApp(appScreen, phoneController);

        UIDocument uIDocument = appScreen.AppUIDocument;
        VisualElement root = uIDocument.rootVisualElement;

        void DrawApps(string appsContainerClassName, PhoneApp[] apps)
        {
            VisualElement appsContainer = root.Query<VisualElement>(className: appsContainerClassName);

            foreach (PhoneApp app in apps)
            {
                VisualElement newApp = new VisualElement();
                newApp.AddToClassList(_menuAppContainerClassName);

                VisualElement newAppIcon = new VisualElement();
                newAppIcon.AddToClassList(_menuAppIconClassName);
                newAppIcon.style.backgroundImage = new StyleBackground(app.AppIcon);

                Label newAppText = new Label();
                newAppText.AddToClassList(_menuAppNameClassName);
                newAppText.text = app.AppName;

                newApp.Add(newAppIcon);
                newApp.Add(newAppText);

                appsContainer.Add(newApp);

                newApp.RegisterCallback<ClickEvent>(evt =>
                {
                    appScreen.OpenApp(app, phoneController);
                });
            }
        }

        DrawApps(_systemicAppsContainerClassName, _systemicApps.Select(app => app as PhoneApp).ToArray());
        DrawApps(_externalAppsContainerClassName, _externalApps.Select(app => app as PhoneApp).ToArray());
    }
}
