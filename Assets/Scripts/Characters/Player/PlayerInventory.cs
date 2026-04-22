using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventory : InventorySystem
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private Vector2Int _defaultInventorySize = new Vector2Int(5, 2);

    [SerializeField] private string _backpackClassName;

    [SerializeField] private Item _debugItem;

    [SerializeField] private Vector2Int _debugAddItemPosition;
    [SerializeField] private Vector2Int _debugRemoveItemPosition;

    [SerializeField] private Backpack _debugBackpack;

    private Backpack _currentBackpack;

    private List<PhoneUIScreen> _appScreens = new List<PhoneUIScreen>();

    public bool IsInventoryOpen => _appScreens.Count > 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SetBackpack(_debugBackpack);
        }
    }

    public bool SetBackpack(Backpack backpack)
    {
        if (_currentBackpack == backpack) { return false; }

        Vector2Int oldGridSize = GetInventoryGridSize();
        Vector2Int newGridSize = backpack == null ? _defaultInventorySize : backpack.Size;

        bool[,] occupancyGrid = GetOccupancyGrid();

        if (newGridSize.x < oldGridSize.x)
        {
            for (int x = newGridSize.x; x < oldGridSize.x; x++)
            {
                for (int y = 0; y < oldGridSize.y; y++)
                {
                    if (occupancyGrid[x, y] == true)
                    {
                        return false;
                    }
                }
            }

            if (newGridSize.y < oldGridSize.y)
            {
                for (int x = 0; x < newGridSize.x; x++)
                {
                    for (int y = newGridSize.y; y < oldGridSize.y; y++)
                    {
                        Debug.Log(new Vector2Int(x, y));
                        if (occupancyGrid[x, y] == true)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else
        {
            if (newGridSize.y < oldGridSize.y)
            {
                for (int x = 0; x < oldGridSize.x; x++)
                {
                    for (int y = newGridSize.y; y < oldGridSize.y; y++)
                    {
                        Debug.Log(new Vector2Int(x, y));
                        if (occupancyGrid[x, y] == true)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        _currentBackpack = backpack;

        CalculateInventoryGrid(isInventoryRender: IsInventoryOpen);

        return true;
    }


    public void AddInventoryScreen(PhoneUIScreen newScreen)
    {
        _appScreens.Add(newScreen);

        if (_appScreens.Count == 1)
        {
            _playerManager.UIManager.DragAndDropItems.DragAndDropData.Add(new DragAndDropData(this));
        }

        GetNecessaryDragAndDropData().ItemsGrids.Add(new List<List<VisualElement>>());
        GetNecessaryDragAndDropData().ItemPlacesGrids.Add(new List<List<VisualElement>>());

        DebugButtons(newScreen.AppUIDocument);

        InventoryRender();
    }

    public void RemoveInventoryScreen(PhoneUIScreen removeScreen)
    {
        int removeScreenIndex = _appScreens.IndexOf(removeScreen);

        if (removeScreenIndex == -1) { return; }

        _appScreens.RemoveAt(removeScreenIndex);

        GetNecessaryDragAndDropData().ItemsGrids.RemoveAt(removeScreenIndex);
        GetNecessaryDragAndDropData().ItemPlacesGrids.RemoveAt(removeScreenIndex);

        if (IsInventoryOpen == false)
        {
            _playerManager.UIManager.DragAndDropItems.DragAndDropData.Remove(GetNecessaryDragAndDropData());
        }
    }

    private DragAndDropData GetNecessaryDragAndDropData()
    {
        return _playerManager.UIManager.DragAndDropItems.DragAndDropData.FirstOrDefault(data => data.InventorySystem == this);
    }

    public override void CalculateInventoryGrid(bool isInventoryRender = false)
    {
        base.CalculateInventoryGrid(isInventoryRender);
    }

    private void CalculateDragAndDropGrids(bool isInventoryRender = false)
    {
        DragAndDropData necessaryDragAndDropData = GetNecessaryDragAndDropData();

        if (necessaryDragAndDropData == null) { return; }

        Vector2Int maxAxis = InventoryGridMaxAxis;

        foreach (List<List<VisualElement>> itemsGrid in necessaryDragAndDropData.ItemsGrids)
        {
            CalculateGrid(itemsGrid, maxAxis);
        }

        foreach (List<List<VisualElement>> itemPlacesGrid in necessaryDragAndDropData.ItemPlacesGrids)
        {
            CalculateGrid(itemPlacesGrid, maxAxis);
        }

        for (int appIndex = 0; appIndex < _appScreens.Count; appIndex++)
        {
            VisualElement root = _appScreens[appIndex].AppUIDocument.rootVisualElement;
            VisualElement contentContainer = root.Q<VisualElement>(className: ContentContainerClassName);
            VisualElement backpackTitle = root.Q<VisualElement>(className: _backpackClassName);

            backpackTitle.style.backgroundImage = _currentBackpack == null ? new StyleBackground() : new StyleBackground(_currentBackpack.Icon);

            VisualElement[] slots = GetContents(contentContainer, ContentSlot.ContentClassName);
            VisualElement[] items = GetContents(contentContainer, ContentItem.ContentClassName);

            Array.Reverse(slots);
            Array.Reverse(items);

            int slotIndex = 0;
            int itemIndex = 0;

            for (int x = 0; x < maxAxis.x; x++)
            {
                for (int y = 0; y < maxAxis.y; y++)
                {
                    necessaryDragAndDropData.ItemPlacesGrids[appIndex][x][y] = slots[slotIndex];
                    slotIndex++;

                    if (InventoryGrid[x][y] == null)
                    {
                        necessaryDragAndDropData.ItemsGrids[appIndex][x][y] = null;
                    }
                    else
                    {
                        necessaryDragAndDropData.ItemsGrids[appIndex][x][y] = items[itemIndex];
                        itemIndex++;
                    }
                }
            }
        }

        if (isInventoryRender == true) { InventoryRender(); }
    }

    public override Vector2Int GetInventoryGridSize()
    {
        return _currentBackpack == null ? _defaultInventorySize : _currentBackpack.Size;
    }

    public override void InventoryRender()
    {
        foreach (PhoneUIScreen appScreen in _appScreens)
        {
            RenderInventoryUI(appScreen.AppUIDocument, isFixedY: false);
        }

        CalculateDragAndDropGrids();
    }

    public void DebugButtons(UIDocument uIDocument)
    {
        VisualElement root = uIDocument.rootVisualElement;

        VisualElement container = root.Q<VisualElement>("Title");

        Button addButton = new Button();
        addButton.AddToClassList("add-button");

        addButton.clicked += () =>
        {
            AddItemByPosition(_debugItem, _debugAddItemPosition, isInventoryRender: IsInventoryOpen);
        };

        container.Add(addButton);

        Button removeButton = new Button();
        removeButton.AddToClassList("remove-button");

        removeButton.clicked += () =>
        {
            RemoveItemByPosition(_debugRemoveItemPosition, isInventoryRender: IsInventoryOpen);
        };

        container.Add(removeButton);
    }
}
