using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;
using Color = UnityEngine.Color;

[CreateAssetMenu(menuName = "ScriptableObject/LevelDesign", fileName = "LevelDesign")]
public class LevelDesign : ScriptableObject
{
    [HideInInspector]public List<Color> colorDefault;
    public int level;
    public List<Color> colorShowFirst;
    public List<Color> colorSecret;
    public List<Color> colorInLevel;

    [Button]
    private void SaveLevel()
    {
        string ToHexList(List<Color> list)
        {
            if (list == null || list.Count == 0) return "[]";
            var parts = new List<string>(list.Count);
            foreach (var c in list)
                parts.Add($"\"#{ColorUtility.ToHtmlStringRGB(c)}\"");
            return "[" + string.Join(", ", parts) + "]";
        }

        var dir = "Assets/Resources/Level";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        // maps to SecretCode
        LevelConvert levelConvert = new LevelConvert();
        levelConvert.level = level;
        levelConvert.colorShowFirst = colorShowFirst.ToArray();
        levelConvert.colorSecret = colorSecret.ToArray();
        levelConvert.colorInLevel = colorInLevel.ToArray();

        var jsonObj = JsonUtility.ToJson(levelConvert, true);
        var json = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(jsonObj));
        var path = Path.Combine(dir, $"Level{level}.txt");
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log($"Saved level file: {path}");
    }

    private LevelDesignFromProposal levelDesignFromProposal;

    [Button(ButtonSizes.Large)]
    private void InitLevelProposal()
    {
        var path = "Assets/BaseGame/ScriptAbleObject/LevelDesign/LevelProposal.txt";
        var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        if (!textAsset)
        {
            Debug.LogWarning($"File not found at path: {path}");
            return;
        }

        var json = textAsset.text.Trim();

        // If the file contains a raw JSON array, wrap it
        if (json.StartsWith("["))
            json = "{\"levelProposals\":" + json + "}";

        try
        {
            levelDesignFromProposal = JsonUtility.FromJson<LevelDesignFromProposal>(json);
            Debug.Log("Loaded level proposals: " + (levelDesignFromProposal?.levelProposals?.Count ?? 0));
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to parse level proposals JSON: " + ex.Message);
        }
    }

    [Button(ButtonSizes.Large)]
    private void LoadLevelData()
    {
        var levelProposal = levelDesignFromProposal?.GetLevelProposal(level);
        if (levelProposal == null) return;

        if (colorShowFirst == null) colorShowFirst = new List<Color>();
        colorShowFirst.Clear();
        ApplyColor(levelProposal.colorShowFirst, colorShowFirst);
        ApplyColor(levelProposal.colorSecret, colorSecret);
        ApplyColor(levelProposal.colorInLevel, colorInLevel);
    }

    private void ApplyColor(List<string> colorHex, List<Color> colors)
    {
        colors.Clear();
        foreach (var hex in colorHex)
        {
            var hexStr = hex.StartsWith("#") ? hex : $"#{hex}";
            if (ColorUtility.TryParseHtmlString(hexStr, out var col))
                colors.Add(col);
            else
                colors.Add(Color.clear);
        }
    }

    [Button]
    private void LoadAndSaveLevel(int levelStart, int levelEnd)
    {
        for (var lvl = levelStart; lvl <= levelEnd; lvl++)
        {
            level = lvl;
            LoadLevelData();
            SaveLevel();
        }
    }
}

[System.Serializable]
public class LevelDesignFromProposal
{
    public List<LevelProposal> levelProposals;

    public LevelProposal GetLevelProposal(int level)
    {
        for (var i = 0; i < levelProposals.Count; i++)
        {
            if (levelProposals[i].level == level)
                return levelProposals[i];
        }

        return null;
    }
}

[System.Serializable]
public class LevelProposal
{
    public int level;
    public List<string> colorShowFirst;
    public List<string> colorSecret;
    public List<string> colorInLevel;
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelDesign))]
public class LevelDesignEditor : Sirenix.OdinInspector.Editor.OdinEditor
{
    const int cellSize = 50;
    const int spacing = 10;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        var levelProp      = serializedObject.FindProperty("level");
        var colorDefault   = serializedObject.FindProperty("colorDefault");
        var colorShowFirst = serializedObject.FindProperty("colorShowFirst");
        var colorSecret    = serializedObject.FindProperty("colorSecret");
        var colorInLevel   = serializedObject.FindProperty("colorInLevel");

        EditorGUILayout.PropertyField(levelProp);

        DrawColorGridWithRemove(colorDefault, 7, "Default Colors");
        DrawColorRowWithRemove(colorShowFirst, "Color First", false, false);
        DrawColorRowWithRemove(colorSecret, "Color Secret", false, false);
        DrawColorGridWithRemove(colorInLevel, 8, "Color In Level", allowAddRemove: true); // dynamic already had +/- buttons

        serializedObject.ApplyModifiedProperties();
    }

    void DrawColorRowWithRemove(SerializedProperty prop, string label, bool needAddButton = true, bool needXButton = true)
    {
        EditorGUILayout.LabelField(label);
        int count = prop.arraySize;
        if (count == 0)
        {
            prop.InsertArrayElementAtIndex(0);
            prop.GetArrayElementAtIndex(0).colorValue = Color.clear;
            count = 1;
        }

        float totalW = count * cellSize + (count - 1) * spacing;
        var rowRect = GUILayoutUtility.GetRect(totalW, cellSize);

        for (int i = 0; i < count; i++)
        {
            var colProp = prop.GetArrayElementAtIndex(i);
            var cellRect = new Rect(rowRect.x + i * (cellSize + spacing), rowRect.y, cellSize, cellSize);

            DrawColorCell(colProp, cellRect, () =>
            {
                prop.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
            }, needXButton);
        }

        if (needAddButton)
        {
            // Add button after row
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                prop.InsertArrayElementAtIndex(prop.arraySize);
                prop.GetArrayElementAtIndex(prop.arraySize - 1).colorValue = Color.clear;
            }
        }
        
    }

    void DrawColorGridWithRemove(SerializedProperty prop, int maxPerRow, string label, bool allowAddRemove = true, bool needXButton =true)
    {
        EditorGUILayout.LabelField(label);
        int count = prop.arraySize;
        if (count == 0 && allowAddRemove)
        {
            prop.InsertArrayElementAtIndex(0);
            prop.GetArrayElementAtIndex(0).colorValue = Color.clear;
            count = 1;
        }

        int rows = Mathf.CeilToInt(count / (float)maxPerRow);
        for (int r = 0; r < rows; r++)
        {
            int start = r * maxPerRow;
            int cols = Mathf.Min(maxPerRow, count - start);
            var rowRect = GUILayoutUtility.GetRect(cols * cellSize + (cols - 1) * spacing, cellSize);

            for (int c = 0; c < cols; c++)
            {
                int i = start + c;
                var colProp = prop.GetArrayElementAtIndex(i);
                var cellRect = new Rect(rowRect.x + c * (cellSize + spacing), rowRect.y, cellSize, cellSize);

                DrawColorCell(colProp, cellRect, () =>
                {
                    prop.DeleteArrayElementAtIndex(i);
                    serializedObject.ApplyModifiedProperties();
                }, needXButton);
            }
        }

        if (allowAddRemove)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                prop.InsertArrayElementAtIndex(prop.arraySize);
                prop.GetArrayElementAtIndex(prop.arraySize - 1).colorValue = Color.clear;
            }
            if (prop.arraySize > 0 && GUILayout.Button("-", GUILayout.Width(30)))
            {
                prop.DeleteArrayElementAtIndex(prop.arraySize - 1);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    void DrawColorCell(SerializedProperty colorProp, Rect rect, System.Action onRemove, bool needXbutton)
    {
        const int btnSize = 16;
        Rect btnRect = new Rect(rect.x + rect.width - btnSize - 2, rect.y + 2, btnSize, btnSize);

        // Intercept mouse down on the button before drawing the ColorField
        if (needXbutton)
        {
            Event e = Event.current;
            if (e.type == EventType.MouseDown && e.button == 0 && btnRect.Contains(e.mousePosition))
            {
                onRemove?.Invoke();
                e.Use();
                GUIUtility.ExitGUI();
                return;
            }
        }

        // Draw full-size color field
        EditorGUI.BeginChangeCheck();
        var newCol = EditorGUI.ColorField(rect, GUIContent.none, colorProp.colorValue, false, true, false);
        if (EditorGUI.EndChangeCheck())
            colorProp.colorValue = newCol;

        // Overlay the remove button (draw after so it is visible)
        if (needXbutton)
        {
            // Optional dark backdrop
            EditorGUI.DrawRect(btnRect, new Color(0, 0, 0, 0.4f));
            if (GUI.Button(btnRect, "x", EditorStyles.miniButton))
            {
                onRemove?.Invoke();
                GUIUtility.ExitGUI();
            }
        }
    }
}
#endif