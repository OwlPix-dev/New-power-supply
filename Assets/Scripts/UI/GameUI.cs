using System;
using UnityEngine;

public abstract class GameUI : MonoBehaviour
{
    public Action OnOpenUI;
    public Action OnCloseUI;

    public virtual void StartUI(UIManager uIManager) { }

    public virtual void OpenUI(UIManager uIManager) { OnOpenUI?.Invoke(); }

    public virtual void CloseUI(UIManager uIManager) { OnCloseUI?.Invoke(); }
}
