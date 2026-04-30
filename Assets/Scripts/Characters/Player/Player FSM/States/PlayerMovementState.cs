using UnityEngine;

public class PlayerMovementState : PlayerState
{
    [SerializeField] private PlayerMove _playerMove;

    [SerializeField] private PlayerRotate _playerRotate;

    [SerializeField] private PlayerChangeCollider _playerChangeCollider;

    public PlayerMove PlayerMove => _playerMove;

    public PlayerRotate PlayerRotate => _playerRotate;

    public PlayerChangeCollider PlayerChangeCollider => _playerChangeCollider;

    public override void EnterState(StateMachine<PlayerState> stateMachine)
    {
        base.EnterState(stateMachine);

        SetState(true);
    }

    public override void ExitState(StateMachine<PlayerState> stateMachine)
    {
        base.ExitState(stateMachine);

        SetState(false);
    }

    private void SetState(bool isActive)
    {
        _playerMove.enabled = isActive;
        _playerRotate.enabled = isActive;
        _playerChangeCollider.enabled = isActive;
    }
}
