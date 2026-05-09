using System;
using UnityEngine;

public class Blocker : UsedItemActive
{
    [SerializeField] private Item[] _unblockers;

    public Action OnDestroy;

    public override bool Active(PlayerManager playerManager)
    {
        foreach (Item unblocker in _unblockers)
        {
            if (playerManager.PlayerScrollItems.CurrentItem == unblocker)
            {
                OnDestroy?.Invoke();
                Destroy(gameObject);
                return true;
            }
        }

        return false;
    }
}
