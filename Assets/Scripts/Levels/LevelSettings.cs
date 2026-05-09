using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Scene settings/Level settings")]
public class LevelSettings : SceneSettings
{
    private Backpack _levelCurrentBackpack;

    private List<List<Item>> _levelInventoryGrid = new List<List<Item>>();

    public Backpack CurrentBackpack
    {
        get => _levelCurrentBackpack;
        set => _levelCurrentBackpack = value;
    }

    public List<List<Item>> LevelInventoryGrid => _levelInventoryGrid;
}
