using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    [SerializeField] private LevelSettings _levelSettings;

    private void Awake()
    {
        GameObject playerInventoriesDataObject = GameObject.FindWithTag(_playerManager.PlayerInventoriesDataTag);
        if (playerInventoriesDataObject != null)
        {
            PlayerInventoriesData playerInventoriesData = playerInventoriesDataObject.GetComponent<PlayerInventoriesData>();
            playerInventoriesData.CurrentLevelSettings = _levelSettings;
        }
    }
}
