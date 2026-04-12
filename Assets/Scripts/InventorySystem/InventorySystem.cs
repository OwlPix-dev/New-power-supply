using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class InventorySystem : MonoBehaviour
{
    private List<List<Item>> _inventoryGrid = new List<List<Item>>();

    public Vector2Int InventoryGridMaxAxis => new Vector2Int(
        _inventoryGrid.Count,
        Mathf.Max(_inventoryGrid.Select(items => items.Count).ToArray()));

    public List<List<Item>> InventoryGrid => _inventoryGrid;

    public Item[] Items
    {
        get
        {
            List<Item> returnItems = new List<Item>();
            Vector2Int maxAxis = InventoryGridMaxAxis;

            for (int y = 0; y < InventoryGridMaxAxis.y; y++)
            {
                for (int x = 0; x < InventoryGridMaxAxis.x; x++)
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

    public void CalculateInventoryGrid(bool isInventoryRender = false)
    {
        Vector2Int inventoryNewGridSize = GetInventoryGridSize();

        int maxX = InventoryGridMaxAxis.x;

        if (maxX < inventoryNewGridSize.x)
        {
            for (int x = maxX; x < inventoryNewGridSize.x; x++)
            {
                _inventoryGrid.Add(new List<Item>());
            }
        }
        else if (maxX > inventoryNewGridSize.x)
        {
            _inventoryGrid.RemoveRange(inventoryNewGridSize.x, maxX - inventoryNewGridSize.x);
        }

        foreach (List<Item> items in _inventoryGrid)
        {
            if (items.Count < inventoryNewGridSize.y)
            {
                for (int y = items.Count; y < inventoryNewGridSize.y; y++)
                {
                    items.Add(null);
                }
            }
            else if (items.Count > inventoryNewGridSize.y)
            {
                items.RemoveRange(inventoryNewGridSize.y, items.Count - inventoryNewGridSize.y);
            }
        }

        if (isInventoryRender == true) { InventoryRender(); }
    }

    public void RenderInventoryUI(UIDocument uIDocument, InventoryUIData uIData, bool isFixedX = true, bool isFixedY = true)
    {
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement slotsContainer = root.Q<VisualElement>(className: uIData.SlotsContainerClassName);
        VisualElement itemsContainer = root.Q<VisualElement>(className: uIData.ItemsContainerClassName);

        Vector2 slotSize = Vector2.one;

        Vector2Int maxAxis = InventoryGridMaxAxis;

        if (isFixedX == true)
        {
            slotSize.x = slotsContainer.resolvedStyle.width / maxAxis.x;
        }

        if (isFixedY == true)
        {
            slotSize.y = slotsContainer.resolvedStyle.height / maxAxis.y;
        }

        slotSize.x = isFixedX == false ? slotSize.y : slotSize.x;
        slotSize.y = isFixedY == false ? slotSize.x : slotSize.y;

        RenderInventoryGrid(slotSize, maxAxis, slotsContainer, itemsContainer, uIData);
    }

    public void RenderInventoryUI(UIDocument uIDocument, InventoryUIData uIData, Vector2 slotSize)
    {
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement slotsContainer = root.Q<VisualElement>(className: uIData.SlotsContainerClassName);
        VisualElement itemsContainer = root.Q<VisualElement>(className: uIData.ItemsContainerClassName);

        RenderInventoryGrid(slotSize, InventoryGridMaxAxis, slotsContainer, itemsContainer, uIData);
    }

    private void RenderInventoryGrid(Vector2 slotSize, Vector2Int maxAxis, VisualElement slotsContainer, VisualElement itemsContainer, InventoryUIData uIData)
    {
        void ContainerConfigure(VisualElement container, string contentClassName)
        {
            container.style.width = maxAxis.x * slotSize.x;
            container.style.height = maxAxis.y * slotSize.y;

            VisualElement[] removeElements = container.Children().
            Where(element => element.ClassListContains(contentClassName)).
            ToArray();

            foreach (VisualElement removeElement in removeElements)
            {
                removeElement.RemoveFromHierarchy();
            }
        }

        ContainerConfigure(slotsContainer, uIData.SlotClassName);
        ContainerConfigure(itemsContainer, uIData.ItemClassName);

        for (int x = 0; x < InventoryGrid.Count; x++)
        {
            for (int y = 0; y < InventoryGrid[x].Count; y++)
            {
                VisualElement newSlot = new VisualElement();
                newSlot.AddToClassList(uIData.SlotClassName);

                newSlot.style.width = slotSize.x;
                newSlot.style.height = slotSize.y;

                newSlot.style.left = x * slotSize.x;
                newSlot.style.top = y * slotSize.y;

                slotsContainer.Add(newSlot);

                Item item = InventoryGrid[x][y];

                if (item != null)
                {
                    VisualElement newItem = new VisualElement();
                    newItem.AddToClassList(uIData.ItemClassName);

                    newItem.style.backgroundImage = new StyleBackground(item.Icon);

                    newItem.style.width = item.Size.x * slotSize.x;
                    newItem.style.height = item.Size.y * slotSize.y;

                    newItem.style.left = x * slotSize.x;
                    newItem.style.top = y * slotSize.y;

                    itemsContainer.Add(newItem);
                }
            }
        }
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

    private void PlaceItem(Item item, Vector2Int itemPosition, bool isInventoryRender)
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

    public abstract Vector2Int GetInventoryGridSize();

    public abstract void InventoryRender();
}

[System.Serializable]
public class InventoryUIData
{
    [SerializeField] private string _slotsContainerClassName = "slots-container";
    [SerializeField] private string _slotClassName = "slot";

    [SerializeField] private string _itemsContainerClassName = "items-container";
    [SerializeField] private string _itemClassName = "item";

    public string SlotsContainerClassName => _slotsContainerClassName;
    public string SlotClassName => _slotClassName;

    public string ItemsContainerClassName => _itemsContainerClassName;
    public string ItemClassName => _itemClassName;
}