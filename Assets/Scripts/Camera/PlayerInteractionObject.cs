using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInteractionObject : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private string _contextMenuClassName;

    [SerializeField] private string _menuItemClassName;
    [SerializeField] private string _menuItemNameClassName;
    [SerializeField] private string _menuItemDescripClassName;
    [SerializeField] private string _menuItemHotKeyNameClassName;

    [SerializeField] private int _openContextMenuMouseIndex;
    [SerializeField] private int[] _closeContextMenuMouseIndices;

    [SerializeField] private float _rayMaxDistance = 100f;
    [SerializeField] private LayerMask _ignoreMask;

    private VisualElement _contextMenu;

    private HashSet<InteractionObject> _availableInteractionObjects = new HashSet<InteractionObject>();

    public HashSet<InteractionObject> AvailableInteractionObjects => _availableInteractionObjects;

    public bool IsOpenContextMenu => _contextMenu.style.visibility == Visibility.Visible;

    private void Awake()
    {
        VisualElement root = _playerManager.UIManager.BasicUIController.UIDocument.rootVisualElement;

        _contextMenu = root.Q<VisualElement>(className: _contextMenuClassName);
    }

    private void Update()
    {
        if (_playerManager.UIManager.CurrentUI != _playerManager.UIManager.BasicUIController ||
            _availableInteractionObjects.Count <= 0) { return; }

        if (Input.GetMouseButtonDown(_openContextMenuMouseIndex) && _playerManager.PlayerMove.IsPlayerMove == false)
        {
            InteractionObject pickInteractionObject = PickInteractionObject();

            if (pickInteractionObject != null && pickInteractionObject.IsCanInteraction == true)
            {
                OpenContextMenu(pickInteractionObject.MenuItems);
                return;
            }
        }

        if (IsOpenContextMenu == true)
        {
            foreach (int closeContextMenuMouseIndex in _closeContextMenuMouseIndices)
            {
                if (Input.GetMouseButtonDown(closeContextMenuMouseIndex))
                {
                    VisualElement root = _playerManager.UIManager.BasicUIController.UIDocument.rootVisualElement;

                    if (root.panel.Pick(GetMousePositionUI()) != null) { return; }

                    InteractionObject pickInteractionObject = PickInteractionObject();

                    if (pickInteractionObject == null)
                    {
                        CloseContextMenu();
                    }
                }
            }

            if (_playerManager.PlayerMove.IsPlayerMove == true)
            {
                CloseContextMenu();
            }
        }
    }

    private InteractionObject PickInteractionObject()
    {
        LayerMask layerMask = ~_ignoreMask;

        RaycastHit cameraHit = default;
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        InteractionObject cameraInteractionObject = null;

        bool isCameraHit = Physics.Raycast(cameraRay, out cameraHit, _rayMaxDistance, layerMask) == true;

        RaycastHit playerHit = default;
        InteractionObject playerInteractionObject = null;

        bool isPlayerHit = false;

        if (isCameraHit == true)
        {
            Vector3 playerPosition = _playerManager.PlayerCenter.position;
            Vector3 cameraHitPosition = cameraHit.transform.position;

            isPlayerHit = Physics.Linecast(playerPosition, cameraHitPosition, out playerHit, layerMask) == true;

            cameraInteractionObject = cameraHit.collider.GetComponent<InteractionObject>();
        }
       
        if (isPlayerHit == true)
        {
            playerInteractionObject = playerHit.collider.GetComponent<InteractionObject>();

            bool isInteractionObjectAvailable = _availableInteractionObjects.Contains(playerInteractionObject);
            bool isInteractionObjectMatches = cameraInteractionObject == playerInteractionObject;


            return isInteractionObjectMatches == true && isInteractionObjectAvailable == true ? playerInteractionObject : null;
        }

        return null;
    }

    private void OpenContextMenu(InteractionObjectMenuItem[] menuItems)
    {
        SetContextMenu(Visibility.Visible, GetMousePositionUI());

        foreach (InteractionObjectMenuItem menuItem in menuItems)
        {
            Button newMenuItemButton = new Button();
            newMenuItemButton.AddToClassList(_menuItemClassName);

            void AddMenuItemText(string menuItemText, string textClassName)
            {
                Label newMenuItemButtonText = new Label();
                newMenuItemButtonText.AddToClassList(textClassName);

                newMenuItemButtonText.text = menuItemText;

                newMenuItemButton.Add(newMenuItemButtonText);
            }

            AddMenuItemText(menuItem.GetItemName(_playerManager), _menuItemNameClassName);
            AddMenuItemText(menuItem.GetItemDescrip(_playerManager), _menuItemDescripClassName);
            AddMenuItemText(menuItem.GetItemHotKeyName(_playerManager), _menuItemHotKeyNameClassName);

            newMenuItemButton.clicked += () =>
            {
                CloseContextMenu();
                menuItem.ItemActive(_playerManager);
            };

            _contextMenu.Add(newMenuItemButton);
        }
    }

    private void CloseContextMenu()
    {
        SetContextMenu(Visibility.Hidden, Vector2.zero);
    }

    private void SetContextMenu(Visibility contextMenuVisibility, Vector2 contextMenuPosition)
    {
        _contextMenu.style.visibility = contextMenuVisibility;

        _contextMenu.style.left = contextMenuPosition.x;
        _contextMenu.style.top = contextMenuPosition.y;

        _contextMenu.Clear();
    }

    private Vector2 GetMousePositionUI()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);
        return mousePosition;
    }
}
