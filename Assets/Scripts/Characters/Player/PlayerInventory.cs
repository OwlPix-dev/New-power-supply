using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventory : InventoryPhoneAppUI
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private Vector2Int _defaultInventorySize = new Vector2Int(5, 2);

    [SerializeField] private string _backpackClassName;

    [SerializeField] private Backpack _debugBackpack;

    private Backpack _currentBackpack;

    private PlayerInventoriesData _playerInventoriesData;

    public override void Start()
    {
        GameObject playerInventoriesDataObject = GameObject.FindWithTag(_playerManager.PlayerInventoriesDataTag);
        if (playerInventoriesDataObject != null)
        {
            _playerInventoriesData = playerInventoriesDataObject.GetComponent<PlayerInventoriesData>();

            _currentBackpack = _playerInventoriesData.CurrentLevelSettings.CurrentBackpack;

            if (_playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid.Count == 0)
            {
                base.Start();
                return;
            }

            CalculateGrid(InventoryGrid, new Vector2Int(_playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid.Count, _playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid[0].Count));

            for (int x = 0; x < _playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid.Count; x++)
            {
                for (int y = 0; y < _playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid[x].Count; y++)
                {
                    PlaceItem(_playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid[x][y], new Vector2Int(x, y), IsInventoryOpen, _playerManager);
                }
            }
        }
        else
        {
            base.Start();
        }
    }

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
                        if (occupancyGrid[x, y] == true)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        _currentBackpack = backpack;
        _playerInventoriesData.CurrentLevelSettings.CurrentBackpack = _currentBackpack;

        CalculateInventoryGrid(_playerManager, isInventoryRender: IsInventoryOpen);

        return true;
    }

    public override void CalculateInventoryGrid(PlayerManager playerManager, bool isInventoryRender = false)
    {
        base.CalculateInventoryGrid(playerManager, isInventoryRender);

        CalculateGrid(_playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid, GetInventoryGridSize());
    }

    public override void PlaceItem(Item item, Vector2Int itemPosition, bool isInventoryRender, PlayerManager playerManager)
    {
        base.PlaceItem(item, itemPosition, isInventoryRender, playerManager);

        _playerInventoriesData.CurrentLevelSettings.LevelInventoryGrid[itemPosition.x][itemPosition.y] = item;
    }

    public override void RenderAppScreen(PhoneUIScreen appScreen, PlayerManager playerManager)
    {
        base.RenderAppScreen(appScreen, playerManager);

        VisualElement root = appScreen.AppUIDocument.rootVisualElement;
        VisualElement backpackTitle = root.Q<VisualElement>(className: _backpackClassName);

        backpackTitle.style.backgroundImage = new StyleBackground(_currentBackpack?.Icon);
    }

    public override Vector2Int GetInventoryGridSize()
    {
        return _currentBackpack == null ? _defaultInventorySize : _currentBackpack.Size;
    }
}
