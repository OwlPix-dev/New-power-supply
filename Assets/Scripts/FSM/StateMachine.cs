using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : State<T>
{
    [SerializeField] private T[] _states;
    [SerializeField] private T _initialState;

    private T _currentState;

    public T CurrentState
    {
        get => _currentState;
        set
        {
            _currentState?.ExitState(this);
            _currentState = value;
            _currentState?.EnterState(this);
        }
    }

    private void Awake()
    {
        CurrentState = _initialState;
    }

    private void Start()
    {
        foreach (T state in _states)
        {
            state.StartState(this);
        }
    }
}