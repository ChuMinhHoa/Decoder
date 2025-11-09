using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Machine machine;

    public LevelDesign levelDesign;

    public ColorHintType GetHintColorType(Color data, int index)
    {
        return machine.GetHintColorType(data, index);
    }

    public Color GetColor(int index)
    {
        return machine.GetColor(index);
    }

    public bool CheckColorIndex(int colorIndex, int indexIgnore)
    {
        return machine.IsHaveThisColor(colorIndex, indexIgnore);
    }

    [Button]
    private void Test(int currentLevel)
    {
        var levelConfig = LevelGlobalConfig.Instance.GetLevelData(currentLevel);
        var text = levelConfig.levelData.text;
        var levelConvert = MyCache.DecodeBase64Json<LevelConvert>(text);
        CreateNewColor(levelConvert);
    }

    public LevelConvert level;

    [Button]
    public void CreateNewColor(LevelConvert levelConvert)
    {
        level = levelConvert;

        for (int i = 0; i < level.colorInLevel.Length; i++)
        {
            Color oldColor = level.colorInLevel[i];
            Color newColor = GetNewColor();
            if(newColor == default)
                continue;

            ChangeColorOther(newColor, oldColor);
            level.colorInLevel[i] = newColor;
        }
    }

    private void ChangeColorOther(Color newColor, Color oldColor)
    {
        ReplaceColorInArray(level.colorShowFirst, oldColor, newColor);
        ReplaceColorInArray(level.colorSecret,    oldColor, newColor);
    }

    private static void ReplaceColorInArray(Color[] arr, Color oldColor, Color newColor)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == oldColor)
                arr[i] = newColor;
        }
    }

    private Color GetNewColor()
    {
        var defaults = levelDesign.colorDefault;
        int seen = 0;
        Color pick = default;

        for (int i = 0; i < defaults.Count; i++)
        {
            var c = defaults[i];
            if (HaveThisColor(c, level.colorInLevel) ||
                HaveThisColor(c, level.colorShowFirst) ||
                HaveThisColor(c, level.colorSecret))
                continue;

            // Uniformly select 1 item from all valid candidates seen so far
            seen++;
            if (Random.Range(0, seen) == 0)
                pick = c;
        }

        if (seen == 0)
        {
            Debug.Log("No available color");
            return default;
        }

        return pick;
    }

    private bool HaveThisColor(Color color, Span<Color> colors)
    {
        foreach (var c in colors)
        {
            if (c == color)
                return true;
        }
        return false;
    }

}