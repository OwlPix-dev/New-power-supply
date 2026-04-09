using UnityEngine;
using UnityEngine.UIElements;

public class Showcase : InventorySingleUI
{
    [SerializeField] private Vector2Int _shelfGrid = new Vector2Int(5, 3);

    public override Vector2Int GetInventoryGridSize()
    {
        return _shelfGrid;
    }

    private void RegisterUI(GeometryChangedEvent evt)
    {
        InventoryRender();
        UIDocument.rootVisualElement.UnregisterCallback<GeometryChangedEvent>(RegisterUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            UIDocument.enabled = !UIDocument.enabled;
            if (UIDocument.enabled == true) UIDocument.rootVisualElement.RegisterCallback<GeometryChangedEvent>(RegisterUI);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CalculateInventoryGrid();
        }
    }
}
