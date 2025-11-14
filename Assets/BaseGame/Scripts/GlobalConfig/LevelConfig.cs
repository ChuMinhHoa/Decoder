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

    public ColorLineMachine[] colorLineMachines;
}

//[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
// [Serializable]
// public class LevelDataConfig : ScriptableObject
// {
//     public int level;
//
//     public int[] colorShowFirstIndex;
//     public int[] colorSecretIndex;
//     public int[] colorInLevelIndex;
//
//     public ColorLineMachine[] colorLineMachines;
// }

[System.Serializable]
public class ColorLineMachine
{
    public ColorSlotMachine[] colorSlots;
}

[System.Serializable]
public class ColorSlotMachine
{
    public ColorSlotType slotType = ColorSlotType.Normal;
}

public enum ColorSlotType
{
    Normal = 0,
    Locked = 1,
    Linked = 2,
    Frozen = 3
}

public enum Difficult
{
    Easy,
    Medium,
    Hard
}