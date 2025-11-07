using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    public int level;
    public float time;
    public int totalColor;
    public Difficult difficult;
    public Color[] colorFirstHint;
    public Color[] colorSecret;
}

public enum Difficult
{
    Easy,
    Medium,
    Hard
}
