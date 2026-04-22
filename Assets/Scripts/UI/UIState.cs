using System;
using UnityEngine;

public abstract class UIState : MonoBehaviour
{
    public Action OnOpenUI;
    public Action OnCloseUI;

    public virtual void StartUI(UIManager uIManager) { }

    public virtual void UpdatePassiveUI(UIManager uIManager) { }

    public virtual void OpenUI(UIManager uIManager)
    {
        uIManager.CurrentUI = this;
        enabled = true;

        OnOpenUI?.Invoke();
    }

    public virtual void CloseUI(UIManager uIManager)
    {
        OnCloseUI?.Invoke();
        enabled = false;
    }

    public virtual void ResumeUI(UIManager uIManager)
    {
        OpenUI(uIManager);
    }
}
