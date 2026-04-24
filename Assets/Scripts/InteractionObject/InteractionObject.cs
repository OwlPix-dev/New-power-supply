using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private InteractionObjectMenuItem[] _menuItems;

    [SerializeField] private bool _isCanInteraction = true;

    public InteractionObjectMenuItem[] MenuItems => _menuItems;

    public bool IsCanInteraction => _isCanInteraction;
}
