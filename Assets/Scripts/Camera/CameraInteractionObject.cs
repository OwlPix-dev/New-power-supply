using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraInteractionObject : MonoBehaviour
{
    [SerializeField] private CameraManager _cameraManager;

    [SerializeField] private bool _isPlayerCheck = true;

    [SerializeField] private string _contextMenuClassName;

    [SerializeField] private string _menuItemClassName;
    [SerializeField] private string _menuItemNameClassName;
    [SerializeField] private string _menuItemDescripClassName;
    [SerializeField] private string _menuItemHotKeyNameClassName;

    [SerializeField] private int _openContextMenuMouseIndex;
    [SerializeField] private int[] _closeContextMenuMouseIndices;

    [SerializeField] private float _rayMaxDistance = 100f;
    [SerializeField] private LayerMask _ignoreMask;

    [SerializeField] private List<InteractionObject> _availableInteractionObjects = new List<InteractionObject>();

    private VisualElement _contextMenu;

    private List<InteractionObjectMenuItem> _currentMenuItems = new List<InteractionObjectMenuItem>();

    public List<InteractionObject> AvailableInteractionObjects => _availableInteractionObjects;

    public List<InteractionObjectMenuItem> CurrentMenuItems => _currentMenuItems;

    public bool IsOpenContextMenu => _contextMenu.style.visibility == Visibility.Visible;

    private void Awake()
    {
        VisualElement root = _cameraManager.PlayerManager.UIManager.MainUIController.UIDocument.rootVisualElement;

        _contextMenu = root.Q<VisualElement>(className: _contextMenuClassName);
    }

    private void Update()
    {
        if (_cameraManager.PlayerManager.UIManager.CurrentUI != _cameraManager.PlayerManager.UIManager.BasicUIController ||
            _availableInteractionObjects.Count <= 0) { return; }

        if (Input.GetMouseButtonDown(_openContextMenuMouseIndex) == true)
        {
            if (_isPlayerCheck == true &&
                _cameraManager.PlayerManager.PlayerMovementState.PlayerMove.IsPlayerMove == true) { return; }

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
                if (Input.GetMouseButtonDown(closeContextMenuMouseIndex) == true)
                {
                    VisualElement root = _cameraManager.PlayerManager.UIManager.MainUIController.UIDocument.rootVisualElement;

                    if (root.panel.Pick(GetMousePositionUI()) != null) { return; }

                    InteractionObject pickInteractionObject = PickInteractionObject();

                    if (pickInteractionObject == null || pickInteractionObject.IsCanInteraction == false)
                    {
                        CloseContextMenu();
                    }
                }
            }

            if (_isPlayerCheck == true &&
                _cameraManager.PlayerManager.PlayerMovementState.PlayerMove.IsPlayerMove == true)
            {
                CloseContextMenu();
            }
        }
    }

    private InteractionObject PickInteractionObject()
    {
        LayerMask layerMask = ~_ignoreMask;
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool isCameraHit = Physics.Raycast(cameraRay, out RaycastHit cameraHit, _rayMaxDistance, layerMask);

        if (isCameraHit == true)
        {
            InteractionObject cameraInteractionObject = cameraHit.collider.GetComponent<InteractionObject>();

            Vector3 cameraHitPosition = cameraHit.transform.position;

            if (cameraInteractionObject != null)
            {
                if (_isPlayerCheck == false || cameraInteractionObject.IsMustPlayerSee == false)
                {
                    return cameraInteractionObject;
                }

                Vector3 playerPosition = _cameraManager.PlayerManager.PlayerCenter.position;

                bool isPlayerHit = Physics.Linecast(playerPosition, cameraHitPosition, out RaycastHit playerHit, layerMask);

                if (isPlayerHit == true)
                {
                    InteractionObject playerInteractionObject = playerHit.collider.GetComponent<InteractionObject>();

                    bool isInteractionObjectAvailable = _availableInteractionObjects.Contains(playerInteractionObject);
                    bool isInteractionObjectMatches = cameraInteractionObject == playerInteractionObject;
                    bool isReturnInteractionObject = isInteractionObjectMatches == true && isInteractionObjectAvailable == true;

                    return isReturnInteractionObject == true ? playerInteractionObject : null;
                }
            }
        }

        return null;
    }

    private void OpenContextMenu(InteractionObjectMenuItem[] menuItems)
    {
        RemoveMenuItems();

        _currentMenuItems = menuItems.ToList();

        foreach (InteractionObjectMenuItem menuItem in _currentMenuItems)
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

            AddMenuItemText(menuItem.GetItemName(_cameraManager.PlayerManager), _menuItemNameClassName);
            AddMenuItemText(menuItem.GetItemDescrip(_cameraManager.PlayerManager), _menuItemDescripClassName);
            AddMenuItemText(menuItem.GetItemHotKeyName(_cameraManager.PlayerManager), _menuItemHotKeyNameClassName);

            newMenuItemButton.clicked += () =>
            {
                CloseContextMenu();
                menuItem.ItemActive(_cameraManager.PlayerManager);
            };

            _contextMenu.Add(newMenuItemButton);
        }

        SetContextMenu(Visibility.Visible, GetMousePositionUI());
    }

    public void CloseContextMenu()
    {
        RemoveMenuItems();
        SetContextMenu(Visibility.Hidden, Vector2.zero);
    }

    public void SetContextMenuItemText(int menuItemIndex, int itemTextIndex, string text)
    {
        Label label = _contextMenu[menuItemIndex][itemTextIndex] as Label;

        label.text = text;
    }

    private void RemoveMenuItems()
    {
        for (int oldMenuItemIndex = 0; oldMenuItemIndex < _currentMenuItems.Count; oldMenuItemIndex++)
        {
            _currentMenuItems[oldMenuItemIndex].CloseMenuItem(_cameraManager.PlayerManager);
            _currentMenuItems.Remove(_currentMenuItems[oldMenuItemIndex]);
        }

        _contextMenu.Clear();
    }

    private void SetContextMenu(Visibility contextMenuVisibility, Vector2 contextMenuPosition)
    {
        _contextMenu.style.visibility = contextMenuVisibility;

        _contextMenu.style.left = contextMenuPosition.x;
        _contextMenu.style.top = contextMenuPosition.y;
    }

    private Vector2 GetMousePositionUI()
    {
        Vector2 mousePosition = Input.mousePosition;
        mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);
        return mousePosition;
    }
}
