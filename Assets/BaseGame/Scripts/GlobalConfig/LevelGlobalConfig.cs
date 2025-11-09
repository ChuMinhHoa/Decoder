using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "GlobalConfig/LevelGlobalConfig")]
[GlobalConfig("Resources/GlobalConfigs/LevelGlobalConfig")]
public class LevelGlobalConfig : GlobalConfig<LevelGlobalConfig>
{
    public LevelConfig[] levelConfigs;

    public LevelConfig GetLevelData(int currentLevel)
    {
        return levelConfigs[currentLevel];
    }
    
    [Button]
    public void GetLevelAsset()
    {
        for (int i = 0; i < levelConfigs.Length; i++)
        {
            var data = AssetDatabase.LoadAssetAtPath<TextAsset>($"Assets/Resources/Level/Level{levelConfigs[i].level}.txt");
            levelConfigs[i].levelData = data;
        }
    }

    [Button]
    private void AddLevel(int level)
    {
        if (level <= levelConfigs.Length)
            return;
        List<LevelConfig> levels = levelConfigs.ToList();
        for (var i = levelConfigs.Length+1; i <= level; i++)
        {
            LevelConfig newLevel = new LevelConfig();
            newLevel.level = i;
            levels.Add(newLevel);
        }
        levelConfigs = levels.ToArray();
    }

}
