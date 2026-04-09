using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySingleUI : InventorySystem
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private InventoryUIData _uIData;

    public UIDocument UIDocument => _uIDocument;

    public override void InventoryRender()
    {
        RenderInventoryUI(_uIDocument, _uIData);
    }
}
