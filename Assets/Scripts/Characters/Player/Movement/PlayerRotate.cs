using UnityEngine;

public class PlayerRotate : CharacterRotate
{
    [SerializeField] private PlayerManager _playerManager;

    private Vector3 _lastDirection;

    public override void Update()
    {
        base.Update();

        Vector3 playerMoveVector = _playerManager.PlayerMovementState.PlayerMove.PlayerMoveVector;

        if (playerMoveVector.sqrMagnitude > MinMoveSqrMagnitude)
        {
            _lastDirection = playerMoveVector;
        }

        if (_lastDirection.sqrMagnitude > MinMoveSqrMagnitude)
        {
            TargetRotation = Quaternion.LookRotation(_lastDirection.normalized);
        }
    }

    public override Transform GetCharacterTransform() { return _playerManager.PlayerTransform; }
}
