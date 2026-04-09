using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PhonePlayerInventory", menuName = "Phone/PhoneApp/PhonePlayerInventory")]
public class PhonePlayerInventory : PhoneApp
{
    public override void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.OpenApp(appScreen, phoneController);

        VisualElement root = appScreen.AppUIDocument.rootVisualElement;

        void RegisterUI(GeometryChangedEvent evt)
        {
            PhoneController.UIManager.PlayerManager.PlayerInventory.NewScreen = appScreen;

            root.UnregisterCallback<GeometryChangedEvent>(RegisterUI);
        }

        root.RegisterCallback<GeometryChangedEvent>(RegisterUI);
    }

    public override void CloseApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.CloseApp(appScreen, phoneController);

        phoneController.UIManager.PlayerManager.PlayerInventory.AppScreens.Remove(appScreen);
    }
}
