using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySingleUI : InventorySystem
{
    public override void InventoryRender(PlayerManager playerManager)
    {
        RenderInventoryUI(GetUIDocument());
    }

    public abstract UIDocument GetUIDocument();
}
