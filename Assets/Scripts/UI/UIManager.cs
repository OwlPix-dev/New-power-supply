using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainUIController _mainUIController;

    [SerializeField] private DragAndDropItems _dragAndDropItems;

    [SerializeField] private PauseMenuUIController _pauseMenuUIController;
    [SerializeField] private BasicUIController _basicUIController;
    [SerializeField] private PhoneUIController _phoneUIController;

    [Space]

    [SerializeField] private LevelMenuUIController _levelMenuUIController;

    [Space]

    [SerializeField] private UIState[] _uIStates;
    [SerializeField] private UIState _initialUI;

    [Space]

    [SerializeField] private PlayerManager _playerManager;

    private UIState _previousUI;
    private UIState _currentUI;


    public MainUIController MainUIController => _mainUIController;

    public DragAndDropItems DragAndDropItems => _dragAndDropItems;

    public PauseMenuUIController PauseMenuUIController => _pauseMenuUIController;
    public BasicUIController BasicUIController => _basicUIController;
    public PhoneUIController PhoneUIController => _phoneUIController;

    public LevelMenuUIController LevelMenuUIController => _levelMenuUIController;

    public UIState[] UIStates => _uIStates;

    public UIState PreviousUI => _previousUI;

    public PlayerManager PlayerManager => _playerManager;

    public UIState CurrentUI
    {
        get => _currentUI;
        set
        {
            if (_currentUI == value) { return; }

            _previousUI = _currentUI;
            _previousUI?.CloseUI(this);
            _currentUI = value;
        }
    }

    private void Awake()
    {
        if (_initialUI != null) { _initialUI.OpenUI(this); }
    }

    private void Start()
    {
        foreach (UIState uIState in _uIStates) { uIState.StartUI(this); }
    }

    private void Update()
    {
        foreach (UIState uIState in _uIStates)
        {
            if (_currentUI == uIState) { continue; }

            uIState.UpdatePassiveUI(this);
        }
    }
}
