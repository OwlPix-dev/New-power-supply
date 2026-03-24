using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PhoneMenu", menuName = "Phone/PhoneApp/PhoneMenu")]
public class PhoneMenu : PhoneApp
{
    [SerializeField] private string _menuContainerClassName;
    [SerializeField] private string _menuAppContainerClassName;
    [SerializeField] private string _menuAppIconClassName;
    [SerializeField] private string _menuAppNameClassName;

    public override void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.OpenApp(appScreen, phoneController);

        UIDocument uIDocument = appScreen.AppUIDocument;
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement menuContainer = root.Query<VisualElement>(className: _menuContainerClassName);

        foreach (PhoneApp app in phoneController.MenuApps)
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

            menuContainer.Add(newApp);

            newApp.RegisterCallback<ClickEvent>(evt =>
            {
                appScreen.OpenApp(app, phoneController);
            });
        }
    }
}
