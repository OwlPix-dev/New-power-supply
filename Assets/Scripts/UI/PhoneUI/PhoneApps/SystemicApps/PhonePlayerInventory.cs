using UnityEngine;
using UnityEngine.UIElements;

public class PhonePlayerInventory : SystemicApp
{
    public override void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.OpenApp(appScreen, phoneController);

        VisualElement root = appScreen.AppUIDocument.rootVisualElement;

        void RegisterUI(GeometryChangedEvent evt)
        {
            PlayerManager playerManager = phoneController.UIManager.PlayerManager;
            PhoneController.UIManager.PlayerManager.PlayerInventory.AddInventoryScreen(appScreen, playerManager);

            root.UnregisterCallback<GeometryChangedEvent>(RegisterUI);
        }

        root.RegisterCallback<GeometryChangedEvent>(RegisterUI);
    }

    public override void CloseApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.CloseApp(appScreen, phoneController);

        PlayerManager playerManager = phoneController.UIManager.PlayerManager;
        phoneController.UIManager.PlayerManager.PlayerInventory.RemoveInventoryScreen(appScreen, playerManager);
    }
}
