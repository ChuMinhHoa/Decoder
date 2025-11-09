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
    public Color[] colorShowFirst;
    public Color[] colorSecret;
    public Color[] colorInLevel;
}

public enum Difficult
{
    Easy,
    Medium,
    Hard
}