using UnityEngine;

public abstract class UsedObject : InteractionObjectMenuItem
{
    [SerializeField] private string _itemName = "Use";

    public override string GetItemName(PlayerManager playerManager)
    {
        return _itemName;
    }

    public override string GetItemDescrip(PlayerManager playerManager)
    {
        return playerManager.PlayerScrollItems.CurrentItem.Name;
    }
}
