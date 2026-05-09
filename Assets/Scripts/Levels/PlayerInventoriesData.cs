using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventoriesData : MonoBehaviour
{
    private LevelSettings _currentLevelSettings;

    public LevelSettings CurrentLevelSettings
    {
        get => _currentLevelSettings;
        set => _currentLevelSettings = value;
    }

    private void Awake()
    {
        List<GameObject> dataObjects = GameObject.FindGameObjectsWithTag(gameObject.tag).ToList();
        dataObjects.RemoveAll(x => x == gameObject);

        foreach (GameObject dataObject in dataObjects)
        {
            Destroy(dataObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
