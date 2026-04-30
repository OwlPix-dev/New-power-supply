using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionObjectMenuItem[] _menuItems;

    [SerializeField] private bool _isMustPlayerSee = true;
    [SerializeField] private bool _isCanInteraction = true;

    public InteractionObjectMenuItem[] MenuItems => _menuItems;

    public bool IsMustPlayerSee => _isMustPlayerSee;
    public bool IsCanInteraction
    {
        get => _isCanInteraction;
        set => _isCanInteraction = value;
    }
}
