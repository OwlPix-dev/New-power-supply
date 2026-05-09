using UnityEngine;

public class LevelMenuInfoActive : InteractionObjectActive
{
    //[SerializeField] private Sprite _levelPhoto;

    //[SerializeField] private string _levelName;
    //[SerializeField] private string _levelDescrip;

    //public Sprite LevelPhoto => _levelPhoto;

    //public string LevelName => _levelName;
    //public string LevelDescrip => _levelDescrip;

    public override bool Active(PlayerManager playerManager)
    {
        playerManager.UIManager.LevelMenuUIController.OpenLevelInfo(this);

        return true;
    }
}
