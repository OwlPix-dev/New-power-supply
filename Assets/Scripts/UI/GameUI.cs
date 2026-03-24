using UnityEngine;

public abstract class GameUI : MonoBehaviour
{
    public virtual void StartUI(UIManager uIManager) { }

    public virtual void OpenUI(UIManager uIManager) { }

    public virtual void CloseUI(UIManager uIManager) { }
}
