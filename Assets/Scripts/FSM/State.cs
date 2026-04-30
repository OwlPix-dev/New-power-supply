using UnityEngine;

public abstract class State<T> : MonoBehaviour where T : State<T>
{
    public virtual void StartState(StateMachine<T> stateMachine) { }

    public virtual void EnterState(StateMachine<T> stateMachine)
    {
        enabled = true;
    }

    public virtual void ExitState(StateMachine<T> stateMachine)
    {
        enabled = false;
    }
}