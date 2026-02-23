using UnityEngine;

public class PlayerRotate : CharacterRotate
{
    [SerializeField] private PlayerManager _playerManager;

    private Vector3 _lastDirection;

    public override void Update()
    {
        base.Update();

        if (_playerManager.PlayerMove.PlayerMoveVector.sqrMagnitude > MinMoveSqrMagnitude)
        {
            _lastDirection = _playerManager.PlayerMove.PlayerMoveVector;
        }

        if (_lastDirection.sqrMagnitude > MinMoveSqrMagnitude)
        {
            TargetRotation = Quaternion.LookRotation(_lastDirection.normalized);
        }
    }

    public override Transform GetCharacterTransform() { return _playerManager.PlayerTransform; }
}
