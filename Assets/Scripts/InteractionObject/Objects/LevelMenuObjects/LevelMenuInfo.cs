using UnityEngine;

public class LevelMenuInfo : InteractionObjectMenuItem
{
    [SerializeField] private Sprite _levelPhoto;

    [SerializeField] private string _levelName;
    [SerializeField] private string _levelDescrip;

    [SerializeField] private string _sceneName;

    public Sprite LevelPhoto => _levelPhoto;

    public string LevelName => _levelName;
    public string LevelDescrip => _levelDescrip;

    public string SceneName => _sceneName;

    public override void ItemActive(PlayerManager playerManager)
    {
        playerManager.UIManager.LevelMenuUIController.OpenLevelInfo(this);
    }
}
