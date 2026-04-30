using UnityEngine;

public class PlayerChangeCollider : CharacterChangeCollider
{
    [SerializeField] private PlayerManager _playerManager;

    public override CharacterController GetCharacterController() { return _playerManager.CharacterController; }
    public override CapsuleCollider GetCapsuleCollider() { return _playerManager.CapsuleCollider; }
}
