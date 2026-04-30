using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _playerCenter;

    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private PlayerScrollItems _playerScrollItems;

    [SerializeField] private PlayerStateMachine _playerStateMachine;

    [SerializeField] private PlayerMovementState _playerMovementState;
    [SerializeField] private PlayerIdleState _playerIdleState;

    [Space]

    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private UIManager _uIManager;

    public Player Player => _player;
    public Transform PlayerTransform => _playerTransform;
    public Transform PlayerCenter => _playerCenter;

    public CharacterController CharacterController => _characterController;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;

    public PlayerInventory PlayerInventory => _playerInventory;
    public PlayerScrollItems PlayerScrollItems => _playerScrollItems;

    public PlayerStateMachine PlayerStateMachine => _playerStateMachine;

    public PlayerMovementState PlayerMovementState => _playerMovementState;
    public PlayerIdleState PlayerIdleState => _playerIdleState;

    public CameraManager CameraManager => _cameraManager;

    public UIManager UIManager => _uIManager;
}
