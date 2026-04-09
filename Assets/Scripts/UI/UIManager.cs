using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private MainUIController _mainUIController;

    [SerializeField] private BasicUIController _basicUIController;
    [SerializeField] private PhoneUIController _phoneUIController;

    [SerializeField] private GameUI _initialUI;

    [SerializeField] private OpenPhoneByKeyCode[] _opensPhone;

    private GameUI _currentUI;

    public PlayerManager PlayerManager => _playerManager;

    public MainUIController MainUIController => _mainUIController;

    public BasicUIController BasicUIController => _basicUIController;
    public PhoneUIController PhoneUIController => _phoneUIController;

    public GameUI[] _uIStates => new GameUI[] { _basicUIController, _phoneUIController };

    public GameUI CurrentUI
    {
        get => _currentUI;
        set
        {
            if (_currentUI == value) { return; }

            _currentUI?.CloseUI(this);
            _currentUI = value;
            _currentUI?.OpenUI(this);
        }
    }

    private void Start()
    {
        foreach (GameUI uIState in _uIStates) { uIState.StartUI(this); }

        if (_initialUI != null) { CurrentUI = _initialUI; }
    }

    private void Update()
    {
        foreach (OpenPhoneByKeyCode openPhone in _opensPhone)
        {
            if (Input.GetKeyDown(openPhone.KeyCode))
            {
                _phoneUIController.OpenPhoneByScreens(openPhone.NewScreens);
            }
        }
    }
}
