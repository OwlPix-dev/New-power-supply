using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySingleUI : InventorySystem
{
    [SerializeField] private InventoryUIData _uIData;

    public override void InventoryRender()
    {
        RenderInventoryUI(GetUIDocument(), _uIData);
    }

    public abstract UIDocument GetUIDocument();
}
