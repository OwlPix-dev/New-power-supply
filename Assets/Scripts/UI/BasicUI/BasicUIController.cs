using UnityEngine;
using UnityEngine.UIElements;

public class BasicUIController : GameUI
{
    [SerializeField] private UIDocument _uIDocument;

    public UIDocument UIDocument => _uIDocument;

    public override void OpenUI(UIManager uIManager)
    {
        _uIDocument.enabled = true;
    }

    public override void CloseUI(UIManager uIManager)
    {
        _uIDocument.enabled = false;
    }
}
