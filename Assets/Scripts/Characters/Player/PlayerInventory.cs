using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventory : InventorySystem
{
    [SerializeField] private Vector2Int _defaultInventorySize = new Vector2Int(5, 2);

    [SerializeField] private InventoryUIData _uIData;

    [SerializeField] private Item _debugItem;

    [SerializeField] private Vector2Int _debugAddItemPosition;
    [SerializeField] private Vector2Int _debugRemoveItemPosition;

    [SerializeField] private Backpack _debugBackpack;

    private Backpack _currentBackpack;

    private List<PhoneUIScreen> _appScreens = new List<PhoneUIScreen>();

    public List<PhoneUIScreen> AppScreens => _appScreens;

    public PhoneUIScreen NewScreen
    {
        set
        {
            DebugButtons(value.AppUIDocument);

            RenderInventoryUIByScreen(value);

            _appScreens.Add(value);
        }
    }

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
        }

        if (newGridSize.y < oldGridSize.y)
        {
            for (int x = 0; x < newGridSize.x; x++)
            {
                for (int y = newGridSize.y; y < oldGridSize.y; y++)
                {
                    if (occupancyGrid[x, y] == true)
                    {
                        return false;
                    }
                }
            }
        }

        _currentBackpack = backpack;

        CalculateInventoryGrid(isInventoryRender: IsInventoryOpen);

        return true;
    }

    public override Vector2Int GetInventoryGridSize()
    {
        return _currentBackpack == null ? _defaultInventorySize : _currentBackpack.Size;
    }

    public override void InventoryRender()
    {
        foreach (PhoneUIScreen appScreen in _appScreens)
        {
            RenderInventoryUIByScreen(appScreen);
        }
    }

    private void RenderInventoryUIByScreen(PhoneUIScreen appScreen)
    {
        RenderInventoryUI(appScreen.AppUIDocument, _uIData, isFixedY: false);
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
