using UnityEngine;

public class SceneSettings : ScriptableObject
{
    [SerializeField] private Sprite _scenePhoto;

    [SerializeField] private string _sceneTitle;
    [SerializeField] private string _sceneDescrip;

    [SerializeField] private string _sceneName;

    public Sprite ScenePhoto => _scenePhoto;

    public string SceneTitle => _sceneTitle;
    public string SceneDescrip => _sceneDescrip;

    public string SceneName => _sceneName;
}
