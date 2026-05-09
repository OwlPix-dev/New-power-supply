using UnityEngine;

public class PickUpItemActive : InteractionObjectActive
{
    [SerializeField] private Item _pickUpItem;

    public override bool Active(PlayerManager playerManager)
    {
        if (playerManager.PlayerInventory.AddItem(_pickUpItem, playerManager) == true)
        {
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
