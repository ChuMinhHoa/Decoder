using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

[GlobalConfig("Resources/GlobalConfigs/ColorGlobalConfig")]
[CreateAssetMenu(fileName = "ColorGlobalConfig", menuName = "GlobalConfig/ColorGlobalConfig")]
public class ColorGlobalConfig : GlobalConfig<ColorGlobalConfig>
{
    public List<Color> colors = new();

    public Color GetColorByIndex(int index)
    {
        if (index < 0 || index >= colors.Count)
        {
            Debug.LogError($"Index {index} is out of range for colors list.");
            return Color.clear; // or handle the error as appropriate
        }
        return colors[index];
    }
}
