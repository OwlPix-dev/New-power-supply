using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropItems : MonoBehaviour
{
    [SerializeField] private UIManager _uIManager;

    [SerializeField] private string _dragTitleClassName;

    private List<DragAndDropData> _dragAndDropData = new List<DragAndDropData>();

    private InventorySystem _currentInventory;

    private Item _dragItem;
    private Vector2Int? _dragItemBackPosition;

    private VisualElement _dragTitle;

    public List<DragAndDropData> DragAndDropData => _dragAndDropData;

    private bool IsDrag => _dragItem != null;

    private void Start()
    {
        VisualElement root = _uIManager.MainUIController.UIDocument.rootVisualElement;

        _dragTitle = root.Q<VisualElement>(className: _dragTitleClassName);
    }

    private void Update()
    {
        if (_dragAndDropData.Count <= 0) { return; }

        Vector2 mousePosition = Input.mousePosition;
        mousePosition.y = Mathf.Abs(mousePosition.y - Screen.height);

        if (IsDrag == true)
        {
            if (Input.GetMouseButton(0))
            {
                _dragTitle.style.left = mousePosition.x;
                _dragTitle.style.top = mousePosition.y;
            }

            if (Input.GetMouseButtonUp(0))
            {
                foreach (DragAndDropData dragAndDropData in _dragAndDropData)
                {
                    foreach (List<List<VisualElement>> grid in dragAndDropData.ItemPlacesGrids)
                    {
                        Vector2Int gridSize = dragAndDropData.InventorySystem.InventoryGridMaxAxis;

                        for (int x = 0; x < gridSize.x; x++)
                        {
                            for (int y = 0; y < gridSize.y; y++)
                            {
                                VisualElement itemPlace = grid[x][y];

                                if (itemPlace != null && itemPlace.worldBound.Contains(mousePosition))
                                {
                                    if (_currentInventory.PutItem(_dragItem, new Vector2Int(x, y)) == false)
                                    {
                                        CancelDrag();

                                        return;
                                    }

                                    SetDragState(null, null, null, Visibility.Hidden);

                                    return;
                                }
                            }
                        }
                    }
                }

                CancelDrag();
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            foreach (DragAndDropData dragAndDropData in _dragAndDropData)
            {
                foreach (List<List<VisualElement>> grid in dragAndDropData.ItemsGrids)
                {
                    Vector2Int gridSize = dragAndDropData.InventorySystem.InventoryGridMaxAxis;

                    for (int x = 0; x < gridSize.x; x++)
                    {
                        for (int y = 0; y < gridSize.y; y++)
                        {
                            Vector2Int itemDragPosition = new Vector2Int(x, y);
                            VisualElement item = grid[itemDragPosition.x][itemDragPosition.y];

                            if (item != null && item.worldBound.Contains(mousePosition))
                            {
                                Item dragItem = dragAndDropData.InventorySystem.InventoryGrid[itemDragPosition.x][itemDragPosition.y];

                                SetDragState(dragAndDropData.InventorySystem, dragItem, itemDragPosition, Visibility.Visible);

                                _dragTitle.style.width = item.worldBound.size.x;
                                _dragTitle.style.height = item.worldBound.size.y;

                                _dragTitle.style.backgroundImage = item.resolvedStyle.backgroundImage;

                                _currentInventory.PickItem(itemDragPosition);

                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetDragState(InventorySystem currentInventory, Item dragItem, Vector2Int? dragItemBackPosition, Visibility dragTitleVisibility)
    {
        _currentInventory = currentInventory;

        _dragItem = dragItem;
        _dragItemBackPosition = dragItemBackPosition;

        _dragTitle.style.visibility = dragTitleVisibility;
    }

    private void CancelDrag()
    {
        _currentInventory.LoseItem(_dragItem, _dragItemBackPosition.Value);

        SetDragState(null, null, null, Visibility.Hidden);
    }
}

public class DragAndDropData
{
    private InventorySystem _inventorySystem;

    private List<List<List<VisualElement>>> _itemsGrids = new List<List<List<VisualElement>>>();
    private List<List<List<VisualElement>>> _itemPlacesGrids = new List<List<List<VisualElement>>>();

    public InventorySystem InventorySystem => _inventorySystem;

    public List<List<List<VisualElement>>> ItemsGrids => _itemsGrids;
    public List<List<List<VisualElement>>> ItemPlacesGrids => _itemPlacesGrids;

    public DragAndDropData(InventorySystem inventorySystem)
    {
        _inventorySystem = inventorySystem;
    }
}