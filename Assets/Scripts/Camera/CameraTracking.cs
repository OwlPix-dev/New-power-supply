using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 3.6f, -5.6f);

    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _minDistance = 0.001f;

    [SerializeField] private Transform _trackerTransform;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        Vector3 realyOffset = new Vector3(
            _cameraOffset.x * _transform.right.x + _cameraOffset.z * _transform.forward.x,
            _cameraOffset.y,
            _cameraOffset.z * _transform.forward.z + _cameraOffset.x * _transform.right.z);

        Vector3 targetPosition = _trackerTransform.position + realyOffset;

        if ((targetPosition - _transform.position).sqrMagnitude > _minDistance)
        {
            _transform.position = Vector3.Lerp(
            _transform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime);
        }
    }
}
