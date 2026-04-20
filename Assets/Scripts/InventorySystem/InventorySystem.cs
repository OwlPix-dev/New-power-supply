using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySystem : MonoBehaviour
{
    [SerializeField] private string _contentContainerClassName = "content-container";

    [SerializeField] private Vector2 _itemPadding = new Vector2(15, 15);

    [SerializeField] private InventoryUIContent _contentSlot;
    [SerializeField] private InventoryUIContent _contentItem;

    private InventoryUIContent[] _contents => new InventoryUIContent[] { _contentSlot, _contentItem };

    private List<List<Item>> _inventoryGrid = new List<List<Item>>();

    public string ContentContainerClassName => _contentContainerClassName;

    public InventoryUIContent ContentSlot => _contentSlot;
    public InventoryUIContent ContentItem => _contentItem;

    public List<List<Item>> InventoryGrid => _inventoryGrid;

    public Vector2Int InventoryGridMaxAxis => new Vector2Int(
        _inventoryGrid.Count,
        Mathf.Max(_inventoryGrid.Select(items => items.Count).ToArray()));

    public Item[] Items
    {
        get
        {
            List<Item> returnItems = new List<Item>();
            Vector2Int maxAxis = InventoryGridMaxAxis;

            for (int y = 0; y < maxAxis.y; y++)
            {
                for (int x = 0; x < maxAxis.x; x++)
                {
                    if (_inventoryGrid[x][y] != null)
                    {
                        returnItems.Add(_inventoryGrid[x][y]);
                    }
                }
            }

            return returnItems.ToArray();
        }
    }

    private void Start()
    {
        CalculateInventoryGrid();
    }

    public void CalculateGrid<T>(List<List<T>> grid, Vector2Int newGridSize)
    {
        int maxX = grid.Count;

        if (maxX < newGridSize.x)
        {
            for (int x = maxX; x < newGridSize.x; x++)
            {
                grid.Add(new List<T>());
            }
        }
        else if (maxX > newGridSize.x)
        {
            grid.RemoveRange(newGridSize.x, maxX - newGridSize.x);
        }

        foreach (List<T> gridElements in grid)
        {
            if (gridElements.Count < newGridSize.y)
            {
                for (int y = gridElements.Count; y < newGridSize.y; y++)
                {
                    gridElements.Add(default);
                }
            }
            else if (gridElements.Count > newGridSize.y)
            {
                gridElements.RemoveRange(newGridSize.y, gridElements.Count - newGridSize.y);
            }
        }
    }

    public virtual void CalculateInventoryGrid(bool isInventoryRender = false)
    {
        CalculateGrid(_inventoryGrid, GetInventoryGridSize());

        if (isInventoryRender == true) { InventoryRender(); }
    }

    public void RenderInventoryUI(UIDocument uIDocument, bool isFixedX = true, bool isFixedY = true)
    {
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement contentContainer = root.Q<VisualElement>(className: _contentContainerClassName);

        Vector2 slotSize = Vector2.one;

        Vector2Int maxAxis = InventoryGridMaxAxis;

        if (isFixedX == true)
        {
            slotSize.x = contentContainer.resolvedStyle.width / maxAxis.x;
        }

        if (isFixedY == true)
        {
            slotSize.y = contentContainer.resolvedStyle.height / maxAxis.y;
        }

        slotSize.x = isFixedX == false ? slotSize.y : slotSize.x;
        slotSize.y = isFixedY == false ? slotSize.x : slotSize.y;

        RenderInventoryGrid(slotSize, maxAxis, contentContainer);
    }

    public void RenderInventoryUI(UIDocument uIDocument, Vector2 slotSize)
    {
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement contentContainer = root.Q<VisualElement>(className: _contentContainerClassName);

        RenderInventoryGrid(slotSize, InventoryGridMaxAxis, contentContainer);
    }

    private void RenderInventoryGrid(Vector2 slotSize, Vector2Int maxAxis, VisualElement contentContainer)
    {
        contentContainer.style.width = maxAxis.x * slotSize.x;
        contentContainer.style.height = maxAxis.y * slotSize.y;

        foreach (InventoryUIContent content in _contents)
        {
            foreach (VisualElement element in GetContents(contentContainer, content.ContentClassName))
            {
                switch (content)
                {
                    case InventoryUIContent item when item == _contentItem:
                        RemoveItemTitle(element);
                        break;
                    case InventoryUIContent slot when slot == _contentSlot:
                        RemoveSlotTitle(element);
                        break;
                }

                element.RemoveFromHierarchy();
            }
        }

        for (int x = 0; x < InventoryGrid.Count; x++)
        {
            for (int y = 0; y < InventoryGrid[x].Count; y++)
            {
                int backSlotIndex = _contentSlot.LayerIndex - 1;
                int slotNumber = backSlotIndex < 0 ? 0 : GetContents(contentContainer, _contents[backSlotIndex].ContentClassName).Length;

                VisualElement slot = SlotConfigure(_contentSlot.ContentClassName, slotSize, new Vector2Int(x, y));
                contentContainer.Insert(slotNumber, slot);

                Item item = InventoryGrid[x][y];
                if (item != null)
                {
                    int backItemIndex = _contentItem.LayerIndex - 1;
                    int itemNumber = backItemIndex < 0 ? 0 : GetContents(contentContainer, _contents[backItemIndex].ContentClassName).Length;

                    VisualElement itemTitle = ItemConfigure(item, _contentItem.ContentClassName, slotSize, new Vector2Int(x, y));
                    contentContainer.Insert(itemNumber, itemTitle);
                }
            }
        }
    }

    public VisualElement[] GetContents(VisualElement contentContainer, string contentClassName)
    {
        return contentContainer.Children().Where(element => element.ClassListContains(contentClassName) == true).ToArray();
    }

    public virtual void RemoveItemTitle(VisualElement item) { }
    public virtual void RemoveSlotTitle(VisualElement slot) { }

    public virtual VisualElement ItemConfigure(Item item, string itemClassName, Vector2 slotSize, Vector2Int itemPosition)
    {
        VisualElement newItem = new VisualElement();
        newItem.AddToClassList(itemClassName);

        newItem.style.backgroundImage = new StyleBackground(item.Icon);

        newItem.style.width = item.Size.x * slotSize.x - _itemPadding.x * 2;
        newItem.style.height = item.Size.y * slotSize.y - _itemPadding.y * 2;

        newItem.style.left = itemPosition.x * slotSize.x + _itemPadding.x;
        newItem.style.top = itemPosition.y * slotSize.y + _itemPadding.y;

        return newItem;
    }

    public virtual VisualElement SlotConfigure(string slotClassName, Vector2 slotSize, Vector2Int slotPosition)
    {
        VisualElement newSlot = new VisualElement();
        newSlot.AddToClassList(slotClassName);

        newSlot.style.width = slotSize.x;
        newSlot.style.height = slotSize.y;

        newSlot.style.left = slotPosition.x * slotSize.x;
        newSlot.style.top = slotPosition.y * slotSize.y;

        return newSlot;
    }

    public bool AddItem(Item newItem, bool isInventoryRender = false)
    {
        if (newItem == null) { return false; }

        bool[,] occupancyGrid = GetOccupancyGrid();

        Vector2Int maxAxis = InventoryGridMaxAxis;

        for (int y = 0; y < maxAxis.y; y++)
        {
            for (int x = 0; x < maxAxis.x; x++)
            {
                if (TryPlaceItem(newItem, new Vector2Int(x, y), occupancyGrid, isInventoryRender) == true)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool AddItemByPosition(Item newItem, Vector2Int newItemPosition, bool isInventoryRender = false)
    {
        if (newItem == null) { return false; }

        return TryPlaceItem(newItem, newItemPosition, GetOccupancyGrid(), isInventoryRender);
    }

    private bool TryPlaceItem(Item item, Vector2Int itemPosition, bool[,] occupancyGrid, bool isInventoryRender)
    {
        if (IsCanPlaceItem(item, itemPosition, occupancyGrid) == true)
        {
            PlaceItem(item, itemPosition, isInventoryRender);

            return true;
        }

        return false;
    }

    public virtual void PlaceItem(Item item, Vector2Int itemPosition, bool isInventoryRender)
    {
        _inventoryGrid[itemPosition.x][itemPosition.y] = item;

        if (isInventoryRender == true) { InventoryRender(); }
    }

    public bool RemoveItem(Item removeItem, bool isInventoryRender = false)
    {
        Vector2Int maxAxis = InventoryGridMaxAxis;

        for (int y = 0; y < maxAxis.y; y++)
        {
            for (int x = 0; x < maxAxis.x; x++)
            {
                if (removeItem == _inventoryGrid[x][y])
                {
                    PlaceItem(null, new Vector2Int(x, y), isInventoryRender);

                    return true;
                }
            }
        }

        return false;
    }

    public bool RemoveItemByPosition(Vector2Int removeItemPosition, bool isInventoryRender = false)
    {
        if (_inventoryGrid[removeItemPosition.x][removeItemPosition.y] != null)
        {
            PlaceItem(null, removeItemPosition, isInventoryRender);

            return true;
        }

        return false;
    }

    private bool IsCanPlaceItem(Item item, Vector2Int itemPosition, bool[,] occupancyGrid)
    {
        Vector2Int maxAxis = InventoryGridMaxAxis;

        if (itemPosition.x < 0 ||
            itemPosition.x + item.Size.x > maxAxis.x ||
            itemPosition.y < 0 ||
            itemPosition.y + item.Size.y > maxAxis.y)
        {
            return false;
        }

        for (int x = itemPosition.x; x < itemPosition.x + item.Size.x; x++)
        {
            for (int y = itemPosition.y; y < itemPosition.y + item.Size.y; y++)
            {
                if (occupancyGrid[x, y] == true)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool[,] GetOccupancyGrid()
    {
        Vector2Int maxAxis = InventoryGridMaxAxis;

        bool[,] occupancyGrid = new bool[maxAxis.x, maxAxis.y];

        for (int x = 0; x < maxAxis.x; x++)
        {
            for (int y = 0; y < maxAxis.y; y++)
            {
                Item item = _inventoryGrid[x][y];

                if (item != null)
                {
                    for (int itemX = x; itemX < x + item.Size.x; itemX++)
                    {
                        for (int itemY = y; itemY < y + item.Size.y; itemY++)
                        {
                            occupancyGrid[itemX, itemY] = true;
                        }
                    }
                }
            }
        }

        return occupancyGrid;
    }

    public virtual bool PickItem(Vector2Int itemPosition)
    {
        return RemoveItemByPosition(itemPosition, isInventoryRender: true);
    }

    public virtual bool PutItem(Item item, Vector2Int itemPosition)
    {
        return AddItemByPosition(item, itemPosition, isInventoryRender: true);
    }

    public virtual bool LoseItem(Item item, Vector2Int itemBackPosition)
    {
        return AddItemByPosition(item, itemBackPosition, isInventoryRender: true);
    }

    public abstract Vector2Int GetInventoryGridSize();

    public abstract void InventoryRender();
}

[System.Serializable]
public class InventoryUIContent
{
    [SerializeField] private string _contentClassName;
    [SerializeField] private int _layerIndex;

    public string ContentClassName => _contentClassName;
    public int LayerIndex => _layerIndex;
}