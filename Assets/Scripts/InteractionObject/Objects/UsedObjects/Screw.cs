using UnityEngine;

public class Screw : UsedObject
{
    [SerializeField] private Item[] _screwdrivers;

    public override void ItemActive(PlayerManager playerManager)
    {
        foreach (Item screwdriver in _screwdrivers)
        {
            if (playerManager.PlayerScrollItems.CurrentItem == screwdriver)
            {
                Destroy(gameObject);
            }
        }
    }
}
