using UnityEngine;

public class PlayerMove : CharacterMove
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private CharacterMoveType _defaultMoveType;

    [SerializeField] private KeyCode _forwardKey = KeyCode.W;
    [SerializeField] private KeyCode _backKey = KeyCode.S;
    [SerializeField] private KeyCode _rightKey = KeyCode.D;
    [SerializeField] private KeyCode _leftKey = KeyCode.A;

    [SerializeField] private PlayerAdditionalMoveType[] _additionalMoveTypes;

    private Transform _cameraTransform;

    private float _verticalInput;
    private float _horizontalInput;

    private Vector3 _playerMoveVector;

    public Vector3 PlayerMoveVector => _playerMoveVector;

    public override void Awake()
    {
        base.Awake();
        _cameraTransform = _playerManager.CameraManager.CameraTransform;
    }

    public override void Start()
    {
        CurrentMoveType = _defaultMoveType;
    }

    public override void Update()
    {
        base.Update();

        foreach (PlayerAdditionalMoveType additionalMoveType in _additionalMoveTypes)
        {
            if (Input.GetKeyDown(additionalMoveType.ActiveKey))
            {
                CharacterMoveType newMoveType = _defaultMoveType;

                if (CurrentMoveType == _defaultMoveType)
                {
                    newMoveType = additionalMoveType.MoveType;
                }

                bool isChangeCollider = _playerManager.ChangeCollider.ChangeCollider(
                    newMoveType.ColliderRadius,
                    newMoveType.ColliderHeight,
                    newMoveType.ColliderCenter);

                if (isChangeCollider) { CurrentMoveType = newMoveType; }
            }
        }

        _verticalInput = GetAxisInput(_verticalInput, _forwardKey, _backKey);
        _horizontalInput = GetAxisInput(_horizontalInput, _rightKey, _leftKey);

        Vector3 cameraForward = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 cameraRight = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f) * Vector3.right;

        _playerMoveVector.x = _horizontalInput * cameraRight.x + _verticalInput * cameraForward.x;
        _playerMoveVector.z = _horizontalInput * cameraRight.z + _verticalInput * cameraForward.z;

        float moveVectorSum = Mathf.Abs(_playerMoveVector.x) + Mathf.Abs(_playerMoveVector.z);
        if (moveVectorSum > 1) { _playerMoveVector /= moveVectorSum; }

        CharacterMoveVector = new Vector3(
            _playerMoveVector.x * CurrentMoveType.MoveSpeed * Time.deltaTime,
            CharacterMoveVector.y,
            _playerMoveVector.z * CurrentMoveType.MoveSpeed * Time.deltaTime);
    }

    private float GetAxisInput(float axis, KeyCode forwardKey, KeyCode backKey)
    {
        int input = (Input.GetKey(forwardKey) ? 1 : 0) - (Input.GetKey(backKey) ? 1 : 0);

        if (input != 0)
        {
            return Mathf.Clamp(axis + input * CurrentMoveType.AccelerationSpeed * Time.deltaTime, -1, 1);
        }
        else
        {
            return Mathf.Clamp(Mathf.MoveTowards(axis, 0, CurrentMoveType.BrakingSpeed * Time.deltaTime), -1, 1);
        }
    }

    public override CharacterController GetCharacterController() { return _playerManager.CharacterController; }
}

[System.Serializable]
public class PlayerAdditionalMoveType
{
    [SerializeField] private CharacterMoveType _moveType;
    [SerializeField] private KeyCode _activeKey;

    public CharacterMoveType MoveType => _moveType;
    public KeyCode ActiveKey => _activeKey;
}