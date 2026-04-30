using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    [SerializeField] private CameraInteractionObject _cameraInteractionObject;

    [SerializeField] private CameraStateMachine _cameraStateMachine;

    [SerializeField] private CameraIdleState _cameraIdleState;
    [SerializeField] private CameraFreeLookState _cameraFreeLookState;
    [SerializeField] private CameraReviewObjectState _cameraReviewObjectState;
    [SerializeField] private CameraRTSState _cameraRTSState;

    [Space]

    [SerializeField] private PlayerManager _playerManager;

    public Transform CameraTransform => _cameraTransform;

    public CameraInteractionObject CameraInteractionObject => _cameraInteractionObject;

    public CameraStateMachine CameraStateMachine => _cameraStateMachine;

    public PlayerManager PlayerManager => _playerManager;

    public CameraIdleState CameraIdleState => _cameraIdleState;
    public CameraFreeLookState CameraFreeLookState => _cameraFreeLookState;
    public CameraReviewObjectState CameraReviewObjectState => _cameraReviewObjectState;
    public CameraRTSState CameraRTSState => _cameraRTSState;
}
