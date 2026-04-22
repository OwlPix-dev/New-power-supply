using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private CameraMoveKey[] _moveKeys;

    [SerializeField] private CameraSpeedFactor _acceleration;
    [SerializeField] private CameraSpeedFactor _braking;

    [SerializeField] private Vector3 _minRestrictionPoint;
    [SerializeField] private Vector3 _maxRestrictionPoint;

    [SerializeField] private float _speed = 10f;

    private float _moveTimer;

    private Vector3 _inputVector;
    private Vector3 _moveVector;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        MoveCameraToRestrictionZone();
    }

    private void Update()
    {
        if (_inputVector != Vector3.zero)
        {
            _inputVector = Vector3.zero;
        }
        
        foreach (CameraMoveKey moveKey in _moveKeys)
        {
            if (Input.GetKey(moveKey.Key) == true)
            {
                _inputVector = new Vector3(
                    Mathf.Clamp(_inputVector.x + moveKey.MoveVector.x, -1, 1),
                    0,
                    Mathf.Clamp(_inputVector.z + moveKey.MoveVector.z, -1, 1));
            }
        }

        if (_inputVector != Vector3.zero)
        {
            if (_moveTimer < 0) { _moveTimer = 0; }

            _moveTimer += Time.deltaTime;
            _moveVector = _inputVector;
        }
        else
        {
            if (_moveTimer > 0) { _moveTimer = 0; }

            _moveTimer -= Time.deltaTime;

            if (Mathf.Abs(_moveTimer) > _braking.FactorDuration)
            {
                _moveVector = Vector3.zero;
                return;
            }
        }

        float speedFactor = 1 -
            (1 - CalculateSpeedFactor(_moveTimer, _acceleration)) -
            (1 - CalculateSpeedFactor(_moveTimer < 0 ? Mathf.Abs(_moveTimer) : 0, _braking));

        Vector3 forward = _transform.forward;
        Vector3 right = _transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 cameraMove =
            forward * _moveVector.z +
            right * _moveVector.x;

        if (cameraMove != Vector3.zero)
        {
            _transform.position += cameraMove * _speed * speedFactor * Time.deltaTime;
            MoveCameraToRestrictionZone();
        }
    }

    private float CalculateSpeedFactor(float timer, CameraSpeedFactor speedFactor)
    {
        if (timer > speedFactor.StartDuration && timer < speedFactor.FactorDuration)
        {
            return speedFactor.FactorCurve.Evaluate(timer / speedFactor.FactorDuration);
        }

        return 1;
    }

    private void MoveCameraToRestrictionZone()
    {
        _transform.position = new Vector3(
            Mathf.Clamp(_transform.position.x, _minRestrictionPoint.x, _maxRestrictionPoint.x),
            _transform.position.y,
            Mathf.Clamp(_transform.position.z, _minRestrictionPoint.z, _maxRestrictionPoint.z)
        );
    }
}

[System.Serializable]
public class CameraMoveKey
{
    [SerializeField] private KeyCode _key;
    [SerializeField] private Vector3Int _moveVector;

    public KeyCode Key => _key;
    public Vector3Int MoveVector => _moveVector;
}

[System.Serializable]
public class CameraSpeedFactor
{
    [SerializeField] private AnimationCurve _factorCurve;

    [SerializeField] private float _factorDuration;
    [SerializeField] private float _startDuration = 0.1f;

    public AnimationCurve FactorCurve => _factorCurve;

    public float FactorDuration => _factorDuration;
    public float StartDuration => _startDuration;
}