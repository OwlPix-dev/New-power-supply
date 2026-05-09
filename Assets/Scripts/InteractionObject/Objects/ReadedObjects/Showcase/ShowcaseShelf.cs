using System.Collections.Generic;
using UnityEngine;

public class ShowcaseShelf : InventoryPhoneAppUI
{
    [SerializeField] private Vector2Int _shelfSize = new Vector2Int(5, 3);

    [SerializeField] private List<Item> _goods = new List<Item>();

    public override void Start()
    {
        base.Start();

        foreach (Item goods in _goods)
        {
            AddItem(goods, null);
        }
    }

    public override Vector2Int GetInventoryGridSize()
    {
        return _shelfSize;
    }

    public override bool PickItem(Vector2Int itemPosition, PlayerManager playerManager)
    {
        return true;
    }

    public override bool PutItem(Item item, Vector2Int itemPosition, PlayerManager playerManager, InventorySystem inventorySystem)
    {
        switch (inventorySystem)
        {
            case ShowcaseShelf or PlayerInventory:
                return false;
            default:
                return base.PutItem(item, itemPosition, playerManager, inventorySystem);
        }
    }
}
