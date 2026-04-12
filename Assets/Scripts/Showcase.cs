using UnityEngine;
using UnityEngine.UIElements;

public class Showcase : InventorySingleUI
{
    [SerializeField] private UIDocument _uIDocument;

    [SerializeField] private Vector2Int _shelfGrid = new Vector2Int(5, 3);

    public override Vector2Int GetInventoryGridSize()
    {
        return _shelfGrid;
    }

    private void RegisterUI(GeometryChangedEvent evt)
    {
        InventoryRender();
        _uIDocument.rootVisualElement.UnregisterCallback<GeometryChangedEvent>(RegisterUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _uIDocument.enabled = !_uIDocument.enabled;
            if (_uIDocument.enabled == true) { _uIDocument.rootVisualElement.RegisterCallback<GeometryChangedEvent>(RegisterUI); }
        }
    }

    public override UIDocument GetUIDocument()
    {
        return _uIDocument;
    }
}
