using UnityEngine;

public class ReviewObject : InteractionObjectMenuItem
{
    [SerializeField] private string _itemName = "Review";

    [SerializeField] private Transform _reviewPoint;

    public override string GetItemName(PlayerManager playerManager)
    {
        return _itemName;
    }

    public override void ItemActive(PlayerManager playerManager)
    {
        Debug.Log("ReviewObject");
    }
}
