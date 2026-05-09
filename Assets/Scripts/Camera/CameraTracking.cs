using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private Vector3 _cameraOffset = new Vector3(0f, 3.6f, -5.6f);

    [SerializeField] private float _moveSpeed = 15f;
    [SerializeField] private float _minDistance = 0.001f;

    [SerializeField] private Transform _trackerTransform;

    private void LateUpdate()
    {
        Vector3 realyOffset = new Vector3(
            _cameraOffset.x * _cameraManager.CameraTransform.right.x + _cameraOffset.z * _cameraManager.CameraTransform.forward.x,
            _cameraOffset.y,
            _cameraOffset.z * _cameraManager.CameraTransform.forward.z + _cameraOffset.x * _cameraManager.CameraTransform.right.z);

        Vector3 targetPosition = _trackerTransform.position + realyOffset;

        if ((targetPosition - _cameraManager.CameraTransform.position).sqrMagnitude > _minDistance)
        {
            _cameraManager.CameraTransform.position = Vector3.Lerp(
            _cameraManager.CameraTransform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime);
        }
    }
}
