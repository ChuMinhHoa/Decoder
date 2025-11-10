using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelConfig
{
    public int level;
    public float time;
    public int totalColor;
    public Difficult difficult;
    public TextAsset levelData;
}

[Serializable]
public class LevelConvert
{
    public int level;

    public int[] colorShowFirstIndex;
    public int[] colorSecretIndex;
    public int[] colorInLevelIndex;
}

public enum Difficult
{
    Easy,
    Medium,
    Hard
}