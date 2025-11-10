using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    public Machine machine;

    public LevelDesign levelDesign;

    public ColorHintType GetHintColorType(int colorID, int index)
    {
        return machine.GetHintColorType(colorID, index);
    }

    public int GetColor(int index)
    {
        return machine.GetColor(index);
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

        for (var i = 0; i < level.colorInLevelIndex.Length; i++)
        {
            var oldColor = level.colorInLevelIndex[i];
            var newColor = GetNewColor();
            if(newColor == -1)
                continue;

            ChangeColorOther(newColor, oldColor);
            level.colorInLevelIndex[i] = newColor;
        }
    }

    private void ChangeColorOther(int newColorID, int oldColorID)
    {
        ReplaceColorInArray(level.colorShowFirstIndex, oldColorID, newColorID);
        ReplaceColorInArray(level.colorSecretIndex,    oldColorID, newColorID);
    }

    private static void ReplaceColorInArray(int[] arr, int oldColor, int newColor)
    {
        for (var i = 0; i < arr.Length; i++)
        {
            if (arr[i] == oldColor)
                arr[i] = newColor;
        }
    }

    private int GetNewColor()
    {
        var seen = 0;
        var pick = -1;

        for (int i = 0; i < levelDesign.colorDefault.Count; i++)
        {
            if (HaveThisColor(i, level.colorInLevelIndex) ||
                HaveThisColor(i, level.colorShowFirstIndex) ||
                HaveThisColor(i, level.colorSecretIndex))
                continue;

            // Uniformly select 1 item from all valid candidates seen so far
            seen++;
            if (Random.Range(0, seen) == 0)
                pick = i;
        }

        if (seen == 0)
        {
            Debug.Log("No available color");
            return -1;
        }

        return pick;
    }

    private bool HaveThisColor(int colorIndex, Span<int> colors)
    {
        foreach (var c in colors)
        {
            if (c == colorIndex)
                return true;
        }
        return false;
    }
}