using UnityEngine;
using UnityEngine.UIElements;

public class MainUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    public UIDocument UIDocument => _uIDocument;

    public void WriteNotification() { }
}
