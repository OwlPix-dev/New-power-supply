using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private Vector3 _offset = new Vector3(0f, 3.6f, -5.6f);

    [SerializeField] private float _moveSpeed = 15f;

    [SerializeField] private Transform _trackerTransform;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Vector3 realyOffset = new Vector3(
            _offset.x * _transform.right.x + _offset.z * _transform.forward.x,
            _offset.y,
            _offset.z * _transform.forward.z + _offset.x * _transform.right.z);

        _transform.position = Vector3.Lerp(
            _transform.position,
            _trackerTransform.position + realyOffset,
            _moveSpeed * Time.deltaTime);
    }
}
