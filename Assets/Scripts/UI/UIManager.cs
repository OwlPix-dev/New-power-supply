using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private MainUIController _mainUIController;
    [SerializeField] private BasicUIController _basicUIController;

    public MainUIController MainUIController => _mainUIController;
    public BasicUIController BasicUIController => _basicUIController;
}
