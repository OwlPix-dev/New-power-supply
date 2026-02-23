using UnityEngine;

public abstract class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _gravityForce = 9.8f;
    [SerializeField] private float _pressingForce = 0.1f;

    private Vector3 _characterMveVector;
    private Vector3 _velocity;

    private CharacterMoveType _currentMoveType;

    private CharacterController _characterController;

    public Vector3 CharacterMoveVector
    {
        get => _characterMveVector;
        set => _characterMveVector = value;
    }

    public CharacterMoveType CurrentMoveType
    {
        get => _currentMoveType;
        set => _currentMoveType = value;
    }

    public CharacterController CharacterController => _characterController;

    public virtual void Awake()
    {
        _characterController = GetCharacterController();
    }

    public virtual void Start() { }

    public virtual void Update()
    {
        if (CharacterController.isGrounded == true)
        {
            _velocity.y = -_pressingForce * Time.deltaTime;
        }
        else
        {
            _velocity.y = Mathf.Clamp(_velocity.y - _gravityForce * Time.deltaTime, -_gravityForce * Time.deltaTime, 0);
        }

        CharacterMoveVector = new Vector3(CharacterMoveVector.x, _velocity.y, CharacterMoveVector.z);

        CharacterController.Move(CharacterMoveVector);
    }

    public abstract CharacterController GetCharacterController();
}
