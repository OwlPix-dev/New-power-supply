using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _player;

    [Space]

    [SerializeField] private Transform _cameraTransform;

    public PlayerManager Player => _player;
    public Transform CameraTransform => _cameraTransform;
}
