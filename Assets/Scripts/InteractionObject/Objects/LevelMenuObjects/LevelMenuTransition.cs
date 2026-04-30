using System.Linq;
using UnityEngine.SceneManagement;

public class LevelMenuTransition : InteractionObjectMenuItem
{
    public override void ItemActive(PlayerManager playerManager)
    {
        LevelMenuInfo levelInfo = MainObject.MenuItems.FirstOrDefault(item => item is LevelMenuInfo) as LevelMenuInfo;
        SceneManager.LoadScene(levelInfo.SceneName);
    }
}
