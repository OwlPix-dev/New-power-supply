using UnityEngine;
using UnityEngine.UIElements;

public class BasicUIController : UIState
{
    [SerializeField] private UIDocument _uIDocument;

    public UIDocument UIDocument => _uIDocument;

    public override void StartUI(UIManager uIManager)
    {
        base.StartUI(uIManager);
    }

    public override void OpenUI(UIManager uIManager)
    {
        _uIDocument.enabled = true;

        base.OpenUI(uIManager);
    }

    public override void CloseUI(UIManager uIManager)
    {
        base.CloseUI(uIManager);

        _uIDocument.enabled = false;
    }
}
