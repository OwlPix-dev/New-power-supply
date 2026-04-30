using System;
using UnityEngine;

public abstract class UsedObject : InteractionObjectMenuItem
{
    [SerializeField] private int _menuItemDescripIndex = 1;

    private Action _scrollItem;

    public override string GetItemDescrip(PlayerManager playerManager)
    {
        string GetDescrip()
        {
            Item currentItem = playerManager.PlayerScrollItems.CurrentItem;

            return currentItem != null ? currentItem.Name : "";
        }

        if (_scrollItem == null)
        {
            _scrollItem = () =>
            {
                CameraInteractionObject playerInteractionObject = playerManager.CameraManager.CameraInteractionObject;

                if (playerInteractionObject.CurrentMenuItems.Contains(this))
                {
                    int indexInMenu = playerInteractionObject.CurrentMenuItems.IndexOf(this);

                    playerInteractionObject.SetContextMenuItemText(indexInMenu, _menuItemDescripIndex, GetDescrip());
                }
            };
        }

        playerManager.PlayerScrollItems.OnScrollItem += _scrollItem;

        return GetDescrip();
    }

    public override void CloseMenuItem(PlayerManager playerManager)
    {
        base.CloseMenuItem(playerManager);

        playerManager.PlayerScrollItems.OnScrollItem -= _scrollItem;
    }
}
