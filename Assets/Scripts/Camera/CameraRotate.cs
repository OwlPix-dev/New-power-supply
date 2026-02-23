using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private CameraTighten[] _cameraTightens;

    [SerializeField] private float _sensitivity = 100f;

    [SerializeField] private float _tightenTime = 0.05f;

    private Vector3? _tightenVector;

    private Vector2? _startMousePosition;
    private Vector2 _lastMousePosition;

    private Transform _transform;

    private bool _isDragging => _startMousePosition != null;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _startMousePosition = Input.mousePosition;
            _lastMousePosition = _startMousePosition.Value;
            _tightenVector = null;
        }

        if (_isDragging && Input.GetMouseButton(2))
        {
            float mouseDistance = Input.mousePosition.x - _lastMousePosition.x;
            _lastMousePosition = Input.mousePosition;

            _transform.Rotate(new Vector3(0, mouseDistance / Screen.width * _sensitivity, 0), Space.World);
        }

        if (Input.GetMouseButtonUp(2) && _isDragging == true)
        {
            _startMousePosition = null;

            foreach (CameraTighten cameraTighten in _cameraTightens)
            {
                if (_transform.eulerAngles.y >= cameraTighten.Range.x && _transform.eulerAngles.y < cameraTighten.Range.y)
                {
                    _tightenVector = new Vector3(
                        _transform.eulerAngles.x,
                        cameraTighten.Angle,
                        _transform.eulerAngles.z);
                }
            }
        }

        if (_tightenVector != null)
        {
            if (Mathf.Abs(_transform.eulerAngles.y - _tightenVector.Value.y) > 0.01f)
            {
                _transform.eulerAngles = Vector3.Lerp(
                    _transform.eulerAngles,
                    _tightenVector.Value,
                    _tightenTime);
            }
            else
            {
                _transform.eulerAngles = new Vector3(
                    _transform.eulerAngles.x,
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
    public float Angle;
    public Vector2 Range;
}