using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewPhonePosition", menuName = "Phone/PositionType")]
public class PhonePositionType : ScriptableObject
{
    [SerializeField] private VisualTreeAsset _positionUI;

    [SerializeField] private PhonePositionScreen[] _positionScreens;

    [SerializeField] private SwitchPositionType[] _switchPositionTypes;

    public PhoneUIScreen[] Screens => _positionScreens.Select(screen => screen.PhoneScreen).ToArray();

    public void OpenPosition(PhoneUIController phoneController)
    {
        UIDocument phoneUIDocument = phoneController.PhoneFrameUIDocument;

        phoneUIDocument.visualTreeAsset = _positionUI;

        phoneUIDocument.rootVisualElement.Clear();
        _positionUI.CloneTree(phoneUIDocument.rootVisualElement);

        VisualElement root = phoneController.PhoneFrameUIDocument.rootVisualElement;

        foreach (SwitchPositionType switchPosition in _switchPositionTypes)
        {
            Button switchPositionButton = new Button();

            switchPositionButton.AddToClassList(switchPosition.SwitchPositionClassName);

            switchPositionButton.clicked += () =>
            {
                NewScreen[] newScreens = switchPosition.SwitchScreens.Select(screen => new NewScreen(screen, null)).ToArray();
                phoneController.OpenPhoneByScreens(newScreens);
            };

            root.Add(switchPositionButton);
        }

        foreach (PhonePositionScreen positionScreen in _positionScreens)
        {
            PhoneUIScreen screen = positionScreen.PhoneScreen;

            Button backButton = new Button();
            backButton.AddToClassList(positionScreen.BackButtonClassName);

            backButton.clicked += () =>
            {
                screen.CurrentApp.BackClick(screen, phoneController);
            };

            root.Add(backButton);

            Button homeButton = new Button();
            homeButton.AddToClassList(positionScreen.HomeButtonClassName);

            homeButton.clicked += () =>
            {
                if (screen.CurrentApp == screen.DefaultApp) { return; }

                screen.OpenApp(screen.DefaultApp, phoneController);
            };

            root.Add(homeButton);
        }
    }

    public void ClosePosition(PhoneUIController phoneController)
    {
        foreach (PhoneUIScreen screen in Screens)
        {
            screen.CloseScreen(phoneController);
        }
    }
}

[System.Serializable]
public class SwitchPositionType
{
    [SerializeField] private string _switchPositionClassName;
    [SerializeField] private PhoneUIScreen[] _switchScreens;

    public string SwitchPositionClassName => _switchPositionClassName;
    public PhoneUIScreen[] SwitchScreens => _switchScreens;
}

[System.Serializable]
public class PhonePositionScreen
{
    [SerializeField] private PhoneUIScreen _phoneScreen;

    [SerializeField] private string _backButtonClassName;
    [SerializeField] private string _homeButtonClassName;

    public PhoneUIScreen PhoneScreen => _phoneScreen;

    public string BackButtonClassName => _backButtonClassName;
    public string HomeButtonClassName => _homeButtonClassName;
}