using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySingleUI : InventorySystem
{
    public override void InventoryRender()
    {
        RenderInventoryUI(GetUIDocument());
    }

    public abstract UIDocument GetUIDocument();
}
