using UnityEngine;

public class SceneTransitionObject : InteractionObject
{
    [SerializeField] private SceneSettings _sceneSettings;

    public SceneSettings SceneSettings => _sceneSettings;
}
