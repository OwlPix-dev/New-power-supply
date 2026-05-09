using System.Collections.Generic;
using UnityEngine;

public class CameraReviewObjectState : CameraState
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _reviewSpeed = 5f;

    [SerializeField] private float _minMoveDistance = 0.001f;
    [SerializeField] private float _minRotateDistance = 0.001f;

    [SerializeField] private KeyCode _backKey = KeyCode.X;

    private List<ReviewActive> _reviewObjects = new List<ReviewActive>();

    private Transform _reviewPoint => _reviewObjects[_reviewObjects.Count - 1].ReviewPoint;

    private void Update()
    {
        if (_reviewPoint == null)
        {
            RemoveReviewObject(_reviewObjects[_reviewObjects.Count - 1]);
            return;
        }

        if ((_reviewPoint.position - _cameraManager.CameraTransform.position).sqrMagnitude > _minMoveDistance)
        {
            _cameraManager.CameraTransform.position = Vector3.Lerp(
                _cameraManager.CameraTransform.position,
                _reviewPoint.position,
                _reviewSpeed * Time.deltaTime);
        }

        if ((_cameraManager.CameraTransform.eulerAngles - _reviewPoint.forward).sqrMagnitude > _minRotateDistance)
        {
            _cameraManager.CameraTransform.rotation = Quaternion.RotateTowards(
                _cameraManager.CameraTransform.rotation,
                Quaternion.LookRotation(_reviewPoint.forward),
                _rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(_backKey) == true)
        {
            RemoveReviewObject(_reviewObjects[_reviewObjects.Count - 1]);
        }
    }

    public void AddReviewObject(ReviewActive newReviewObject)
    {
        _reviewObjects.Add(newReviewObject);

        if (_reviewObjects.Count == 1)
        {
            _cameraManager.CameraStateMachine.CurrentState = this;

            PlayerManager playerManager = _cameraManager.PlayerManager;
            playerManager.PlayerStateMachine.CurrentState = playerManager.PlayerIdleState;
        }
    }

    public void RemoveReviewObject(ReviewActive removeReviewObject)
    {
        removeReviewObject.CancelReview(_cameraManager.PlayerManager);
        _reviewObjects.Remove(removeReviewObject);

        if (_reviewObjects.Count == 0)
        {
            _cameraManager.CameraStateMachine.CurrentState = _cameraManager.CameraFreeLookState;

            PlayerManager playerManager = _cameraManager.PlayerManager;
            playerManager.PlayerStateMachine.CurrentState = playerManager.PlayerMovementState;
        }
    }
}
