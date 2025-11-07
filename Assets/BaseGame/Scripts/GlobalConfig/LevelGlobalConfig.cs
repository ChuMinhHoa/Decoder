using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(menuName = "GlobalConfig/LevelGlobalConfig")]
[GlobalConfig("Resources/GlobalConfigs/LevelGlobalConfig")]
public class LevelGlobalConfig : GlobalConfig<LevelGlobalConfig>
{
    public LevelConfig[] levelConfigs;

    public Color[] listColors;

    public LevelConfig GetLevelData(int currentLevel)
    {
        return levelConfigs[currentLevel];
    }
}
