using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerCenter;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    [SerializeField] private PlayerChangeCollider _changeCollider;
    [SerializeField] private PlayerRotate _playerRotate;
    [SerializeField] private PlayerMove _playerMove;

    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerScrollItems _playerScrollItems;

    [SerializeField] private PlayerInteractionObject _playerInteractionObject;

    [Space]

    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private UIManager _uIManager;

    public Player Player => _player;
    public Transform PlayerTransform => _playerTransform;
    public Transform PlayerCenter => _playerCenter;

    public CharacterController CharacterController => _characterController;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;

    public PlayerChangeCollider ChangeCollider => _changeCollider;
    public PlayerRotate PlayerRotate => _playerRotate;
    public PlayerMove PlayerMove => _playerMove;

    public PlayerInventory PlayerInventory => _playerInventory;
    public PlayerScrollItems PlayerScrollItems => _playerScrollItems;

    public PlayerInteractionObject PlayerInteractionObject => _playerInteractionObject;

    public CameraManager CameraManager => _cameraManager;

    public UIManager UIManager => _uIManager;
}
