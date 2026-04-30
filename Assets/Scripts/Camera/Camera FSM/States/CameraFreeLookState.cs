using UnityEngine;

public class CameraFreeLookState : CameraState
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private CameraTracking _cameraTracking;
    [SerializeField] private CameraRotate _cameraRotate;

    public CameraTracking CameraTracking => _cameraTracking;
    public CameraRotate CameraRotate => _cameraRotate;

    public override void EnterState(StateMachine<CameraState> stateMachine)
    {
        base.EnterState(stateMachine);

        _cameraManager.CameraFreeLookState.CameraRotate.TightenVector = new Vector3(
            _cameraManager.CameraFreeLookState.CameraRotate.CameraAngle.x,
            _cameraManager.CameraTransform.eulerAngles.y,
            _cameraManager.CameraTransform.eulerAngles.z);

        SetState(true);
    }

    public override void ExitState(StateMachine<CameraState> stateMachine)
    {
        base.ExitState(stateMachine);

        SetState(false);
    }

    private void SetState(bool isActive)
    {
        _cameraTracking.enabled = isActive;
        _cameraRotate.enabled = isActive;
    }
}
