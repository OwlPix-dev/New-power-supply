using UnityEngine;
using UnityEngine.UIElements;

public class BasicUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _uIDocument;

    public UIDocument UIDocument => _uIDocument;
}
