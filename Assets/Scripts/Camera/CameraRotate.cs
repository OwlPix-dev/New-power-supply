using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private CameraTighten[] _cameraTightens;

    [SerializeField] private float _sensitivity = 100f;
    [SerializeField] private float _tightenTime = 0.05f;
    [SerializeField] private float _minDistance = 0.001f;

    [SerializeField] private int _mouseRotateIndex = 2;

    [SerializeField] private Vector3 _cameraAngle = new Vector3(15f, 0f, 0f);

    private Vector3? _tightenVector;

    private Vector2? _startMousePosition;
    private Vector2 _lastMousePosition;

    private Transform _transform;

    public Vector3 CameraAngle => _cameraAngle;

    public Vector3? TightenVector
    {
        get => _tightenVector;
        set => _tightenVector = value;
    }

    private bool _isDragging => _startMousePosition != null;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(_mouseRotateIndex))
        {
            _startMousePosition = Input.mousePosition;
            _lastMousePosition = _startMousePosition.Value;
            _tightenVector = null;
        }

        if (_isDragging == true && Input.GetMouseButton(_mouseRotateIndex))
        {
            float mouseDistance = Input.mousePosition.x - _lastMousePosition.x;
            _lastMousePosition = Input.mousePosition;

            _transform.Rotate(new Vector3(0, mouseDistance / Screen.width * _sensitivity, 0), Space.World);
        }

        if (Input.GetMouseButtonUp(_mouseRotateIndex) && _isDragging == true)
        {
            _startMousePosition = null;

            foreach (CameraTighten cameraTighten in _cameraTightens)
            {
                bool isAngleLarger = _transform.eulerAngles.y >= cameraTighten.TightenRange.x;
                bool isAngleLess = _transform.eulerAngles.y < cameraTighten.TightenRange.y;

                if (isAngleLarger && isAngleLess)
                {
                    _tightenVector = new Vector3(
                        _transform.eulerAngles.x,
                        cameraTighten.TightenAngle,
                        _transform.eulerAngles.z);
                }
            }
        }

        if (_tightenVector != null)
        {
            if ((_transform.eulerAngles - _tightenVector.Value).sqrMagnitude > _minDistance)
            {
                _transform.eulerAngles = Vector3.Lerp(
                    _transform.eulerAngles,
                    _tightenVector.Value,
                    _tightenTime);
            }
            else
            {
                _transform.eulerAngles = new Vector3(
                    _cameraAngle.x,
                    _tightenVector.Value.y,
                    _transform.eulerAngles.z);
                _tightenVector = null;
            }
        }
    }
}

[System.Serializable]
public class CameraTighten
{
    [SerializeField] private float _tightenAngle;
    [SerializeField] private Vector2 _tightenRange;

    public float TightenAngle => _tightenAngle;
    public Vector2 TightenRange => _tightenRange;
}