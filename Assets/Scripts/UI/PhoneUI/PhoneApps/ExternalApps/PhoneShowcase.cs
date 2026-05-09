using UnityEngine;
using UnityEngine.UIElements;

public class PhoneShowcase : ExternalApp
{
    [SerializeField] private ShowcaseShelf[] _showcaseShelves;

    public override void OpenApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.OpenApp(appScreen, phoneController);

        VisualElement root = appScreen.AppUIDocument.rootVisualElement;

        void RegisterUI(GeometryChangedEvent evt)
        {
            foreach (ShowcaseShelf showcaseShelf in _showcaseShelves)
            {
                PlayerManager playerManager = phoneController.UIManager.PlayerManager;
                showcaseShelf.AddInventoryScreen(appScreen, playerManager);
            }

            root.UnregisterCallback<GeometryChangedEvent>(RegisterUI);
        }

        root.RegisterCallback<GeometryChangedEvent>(RegisterUI);
    }

    public override void CloseApp(PhoneUIScreen appScreen, PhoneUIController phoneController)
    {
        base.CloseApp(appScreen, phoneController);

        foreach (ShowcaseShelf showcaseShelf in _showcaseShelves)
        {
            PlayerManager playerManager = phoneController.UIManager.PlayerManager;
            showcaseShelf.RemoveInventoryScreen(appScreen, playerManager);
        }
    }
}
