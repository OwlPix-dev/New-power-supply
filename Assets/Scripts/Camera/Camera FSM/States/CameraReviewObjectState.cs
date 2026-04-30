using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraReviewObjectState : CameraState
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _reviewSpeed = 5f;

    [SerializeField] private float _minMoveDistance = 0.001f;
    [SerializeField] private float _minRotateDistance = 0.001f;

    [SerializeField] private KeyCode _backKey = KeyCode.X;

    private List<ReviewObject> _reviewObjects = new List<ReviewObject>();

    private Transform _transform;

    private Transform _reviewPoint => _reviewObjects[_reviewObjects.Count - 1].ReviewPoint;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (_reviewPoint == null) { return; }

        if ((_reviewPoint.position - _transform.position).sqrMagnitude > _minMoveDistance)
        {
            _transform.position = Vector3.Lerp(
                _transform.position,
                _reviewPoint.position,
                _reviewSpeed * Time.deltaTime);
        }

        if ((_transform.eulerAngles - _reviewPoint.forward).sqrMagnitude > _minRotateDistance)
        {
            _transform.rotation = Quaternion.RotateTowards(
                _transform.rotation,
                Quaternion.LookRotation(_reviewPoint.forward),
                _rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(_backKey) == true)
        {
            RemoveReviewObject(_reviewObjects[_reviewObjects.Count - 1]);
        }
    }

    public void AddReviewObject(ReviewObject newReviewObject)
    {
        _reviewObjects.Add(newReviewObject);

        if (_reviewObjects.Count == 1)
        {
            _cameraManager.CameraStateMachine.CurrentState = this;

            PlayerManager playerManager = _cameraManager.PlayerManager;
            playerManager.PlayerStateMachine.CurrentState = playerManager.PlayerIdleState;
        }
    }

    public void RemoveReviewObject(ReviewObject removeReviewObject)
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
