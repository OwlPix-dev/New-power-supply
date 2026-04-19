using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerScrollItems : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private ScrollAction[] _scrollActions;
    [SerializeField] private SlotGridException[] _slotGridExceptions;

    [SerializeField] private string _scrollItemsContainerClassName;

    [SerializeField] private string _currentItemClassName;
    [SerializeField] private string _slotClassName;

    [SerializeField] private Vector2 _slotSize = new Vector2(100, 100);

    [SerializeField] private Vector2Int _defaultSlotGridSize = new Vector2Int(3, 2);

    private int _currentItemNumber;

    private bool _isUIOpen => _playerManager.UIManager.IsUIOpen(_playerManager.UIManager.BasicUIController);

    public Item CurrentItem
    {
        get
        {
            Item[] items = _playerManager.PlayerInventory.Items;
            return _currentItemNumber <= 0 || _currentItemNumber > items.Length ? null : items[_currentItemNumber - 1];
        }
    }

    private void OnEnable()
    {
        _playerManager.UIManager.BasicUIController.OnOpenUI += () =>
        {
            VisualElement root = _playerManager.UIManager.BasicUIController.UIDocument.rootVisualElement;

            foreach (ScrollAction scrollAction in _scrollActions)
            {
                VisualElement buttonContainer = root.Q<VisualElement>(className: scrollAction.ScrollButtonContainerClassName);

                Button scrollButton = new Button();

                scrollButton.AddToClassList(scrollAction.ScrollButtonClassName);

                scrollButton.clicked += () =>
                {
                    ScrollItem(scrollAction.ScrollDistance);
                };

                buttonContainer.Insert(scrollAction.ScrollButtonIndexInContainer, scrollButton);
            }

            RenderScrollItems();
        };

    }

    private void Start()
    {
        if (_isUIOpen == true) { RenderScrollItems(); }
    }

    private void Update()
    {
        if (_isUIOpen == false) { return; }

        foreach (ScrollAction scrollAction in _scrollActions)
        {
            if (Input.GetKeyDown(scrollAction.ScrollKey))
            {
                ScrollItem(scrollAction.ScrollDistance);
            }
        }
    }

    private void ScrollItem(int scrollDistance)
    {
        int itemsLength = _playerManager.PlayerInventory.Items.Length;
        int newNumber = _currentItemNumber + scrollDistance;

        _currentItemNumber = newNumber > itemsLength ? 0 : newNumber < 0 ? itemsLength : newNumber;

        if (_isUIOpen == true) { RenderScrollItems(); }
    }

    private void RenderScrollItems()
    {
        VisualElement root = _playerManager.UIManager.BasicUIController.UIDocument.rootVisualElement;
        VisualElement titleItem = root.Q<VisualElement>(className: _currentItemClassName);
        VisualElement container = root.Q<VisualElement>(className: _scrollItemsContainerClassName);

        VisualElement[] removeSlots = container.Children().
            Where(element => element.ClassListContains(_slotClassName)).
            ToArray();

        foreach (VisualElement removeSlot in removeSlots)
        {
            removeSlot.RemoveFromHierarchy();
        }

        Item currentItem = CurrentItem;
        Vector2Int slotsGrid = CurrentItem == null ? _defaultSlotGridSize : CurrentItem.Size;

        foreach (SlotGridException slotGridException in _slotGridExceptions)
        {
            if (slotsGrid == slotGridException.GridSizeException)
            {
                slotsGrid = slotGridException.NewGridSize;
                break;
            }
        }

        container.style.width = _slotSize.x * slotsGrid.x;
        container.style.height = _slotSize.y * slotsGrid.y;

        for (int x = 0; x < slotsGrid.x; x++)
        {
            for (int y = 0; y < slotsGrid.y; y++)
            {
                VisualElement newSlot = new VisualElement();
                newSlot.AddToClassList(_slotClassName);

                newSlot.style.left = _slotSize.x * x;
                newSlot.style.top = _slotSize.y * y;

                container.Insert(0, newSlot);
            }
        }

        titleItem.style.width = currentItem == null ? 0 : _slotSize.x * currentItem.Size.x;
        titleItem.style.height = currentItem == null ? 0 : _slotSize.y * currentItem.Size.y;

        titleItem.style.backgroundImage = currentItem == null ? new StyleBackground() : new StyleBackground(currentItem.Icon);
    }
}

[System.Serializable]
public class ScrollAction
{
    [SerializeField] private KeyCode _scrollKey;
    [SerializeField] private int _scrollDistance;

    [SerializeField] private string _scrollButtonClassName;
    [SerializeField] private string _scrollButtonContainerClassName;
    [SerializeField] private int _scrollButtonIndexInContainer;

    public KeyCode ScrollKey => _scrollKey;
    public int ScrollDistance => _scrollDistance;

    public string ScrollButtonClassName => _scrollButtonClassName;
    public string ScrollButtonContainerClassName => _scrollButtonContainerClassName;
    public int ScrollButtonIndexInContainer => _scrollButtonIndexInContainer;
}

[System.Serializable]
public class SlotGridException
{
    [SerializeField] private Vector2Int _gridSizeException;
    [SerializeField] private Vector2Int _newGridSize;

    public Vector2Int GridSizeException => _gridSizeException;
    public Vector2Int NewGridSize => _newGridSize;
}