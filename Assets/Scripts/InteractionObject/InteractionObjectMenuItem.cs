using UnityEngine;

public abstract class InteractionObjectMenuItem : MonoBehaviour
{
    [SerializeField] private InteractionObject _mainObject;

    [SerializeField] private KeyCode _itemHotKey;

    public InteractionObject MainObject => _mainObject;

    public KeyCode ItemHotKey => _itemHotKey;

    public virtual Sprite ItemIcon(PlayerManager playerManager) { return null; }

    public virtual string GetItemDescrip(PlayerManager playerManager) { return null; }
    public virtual string GetItemHotKeyName(PlayerManager playerManager) { return null; }
    public abstract string GetItemName(PlayerManager playerManager);

    public abstract void ItemActive(PlayerManager playerManager);
}
