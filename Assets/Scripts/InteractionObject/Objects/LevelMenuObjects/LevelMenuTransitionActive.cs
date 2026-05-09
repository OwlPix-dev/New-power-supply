using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenuTransitionActive : InteractionObjectActive
{
    public override bool Active(PlayerManager playerManager)
    {
        TransitionLevel();

        return true;
    }

    public void TransitionLevel()
    {
        SceneManager.LoadScene((MainObject as SceneTransitionObject).SceneSettings.SceneName);
    }
}
