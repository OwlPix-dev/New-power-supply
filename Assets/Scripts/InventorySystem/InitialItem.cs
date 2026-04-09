using UnityEngine;

public class InitialItem : ScriptableObject
{
    [SerializeField] private string _name = "Item";
    [SerializeField] private string _description = "Item description";

    [SerializeField] private Sprite _icon;

    [SerializeField] private int _price = 10;

    [SerializeField] private Vector2Int _size = new Vector2Int(2, 1);

    public string Name => _name;
    public string Description => _description;

    public Sprite Icon => _icon;

    public int Price => _price;

    public Vector2Int Size => _size;
}
