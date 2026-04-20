using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private MoveKey[] _moveKeys;

    [SerializeField] private SpeedFactor _acceleration;
    [SerializeField] private SpeedFactor _braking;

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

    private void Update()
    {
        if (_inputVector != Vector3.zero)
        {
            _inputVector = Vector3.zero;
        }
        
        foreach (MoveKey moveKey in _moveKeys)
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

        Vector3 nextPosition = _transform.position + cameraMove * _speed * speedFactor * Time.deltaTime;

        bool isCanMove =
            nextPosition.x > _minRestrictionPoint.x && nextPosition.x < _maxRestrictionPoint.x &&
            nextPosition.z > _minRestrictionPoint.z && nextPosition.z < _maxRestrictionPoint.z;

        if (cameraMove != Vector3.zero && isCanMove == true)
        {
            _transform.position += cameraMove * _speed * speedFactor * Time.deltaTime;
        }
    }

    private float CalculateSpeedFactor(float timer, SpeedFactor speedFactor)
    {
        if (timer > speedFactor.StartDuration && timer < speedFactor.FactorDuration)
        {
            return speedFactor.FactorCurve.Evaluate(timer / speedFactor.FactorDuration);
        }

        return 1;
    }
}

[System.Serializable]
public class MoveKey
{
    [SerializeField] private KeyCode _key;
    [SerializeField] private Vector3Int _moveVector;

    public KeyCode Key => _key;
    public Vector3Int MoveVector => _moveVector;
}

[System.Serializable]
public class SpeedFactor
{
    [SerializeField] private AnimationCurve _factorCurve;

    [SerializeField] private float _factorDuration;
    [SerializeField] private float _startDuration = 0.1f;

    public AnimationCurve FactorCurve => _factorCurve;

    public float FactorDuration => _factorDuration;
    public float StartDuration => _startDuration;
}