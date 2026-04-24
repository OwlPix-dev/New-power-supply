using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    [Space]

    [SerializeField] private PlayerManager _playerManager;

    public Transform CameraTransform => _cameraTransform;

    public PlayerManager PlayerManager => _playerManager;
}
