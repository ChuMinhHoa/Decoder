using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "GlobalConfig/LevelMachineDesign", fileName = "LevelMachineDesign")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class LevelMachineDesign : GlobalConfig<LevelMachineDesign>
{
    [field: SerializeField] public LevelDesign levelDesign;

    [field: SerializeField, LevelDataEditor]
    public LevelGenerate CurrentLevel { get; set; }
}

[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class LevelDataEditor : System.Attribute
{
}

#if UNITY_EDITOR
public class LevelDataEditorAttributeDrawer : OdinAttributeDrawer<LevelDataEditor, LevelGenerate>
{
    private static int defaultWidthHeight = 30;
    private static int Highlight { get; set; } = -1;
    private LevelConvert levelDataConfig;

    private static Vector2 scaleSlot = new Vector2(60f, 80f);
    private static Vector2 spacingSlot = new Vector2(10, 10);

    protected override void DrawPropertyLayout(GUIContent label)
    {
        Rect rect1 = EditorGUILayout.GetControlRect();
        defaultWidthHeight = (int)rect1.height;
        //ValueEntry.SmartValue = SirenixEditorFields.UnityObjectField(rect1, ValueEntry.SmartValue, typeof(LevelDataConfig),true) as LevelDataConfig;
        CallNextDrawer(label);

        rect1 = EditorGUILayout.GetControlRect();
        Highlight = SirenixEditorFields.IntField(rect1, Highlight);
        if (ValueEntry.SmartValue == null) return;
        levelDataConfig = ValueEntry.SmartValue.levelConvert;
        var totalLineMachines = levelDataConfig.colorLineMachines.Length;
        var maxLayerHeight = scaleSlot.y * totalLineMachines + spacingSlot.y * (totalLineMachines - 1) +
                             defaultWidthHeight;

        var totalSlot = 4;
        var width = totalSlot * scaleSlot.x + (totalSlot - 1) * spacingSlot.x + 20f;
        Rect rect = EditorGUILayout.GetControlRect(false, maxLayerHeight);
        rect.width = width;
        SirenixEditorGUI.DrawSolidRect(rect, new Color(0.69f, 0.75f, 0.77f, 1.0f));
        SirenixEditorGUI.DrawBorders(rect, 1);

        for (var i = 0; i < levelDataConfig.colorLineMachines.Length; i++)
        {
            DrawColorSlot(rect, levelDataConfig.colorLineMachines[i], i);
        }
    }

    private void DrawColorSlot(Rect rect, ColorLineMachine colorLineMachine, int colorLineIndex)
    {
        for (int i = 0; i < colorLineMachine.colorSlots.Length; i++)
        {
            Rect rectSlot = EditorGUILayout.GetControlRect(false, scaleSlot.y, GUILayout.Width(scaleSlot.x));
            var slotPosition = new Vector2();
            slotPosition.x = rect.x + 10 + scaleSlot.x * i + spacingSlot.x * i;
            slotPosition.y = rect.y + 10 + scaleSlot.y * colorLineIndex + spacingSlot.y * colorLineIndex;
            rectSlot.position = slotPosition;
            
            SirenixEditorGUI.DrawSolidRect(rectSlot, Color.silver);
            SirenixEditorGUI.DrawBorders(rectSlot, 1);
            
            var slotType = colorLineMachine.colorSlots[i].slotType;
            float popupWidth = rectSlot.width;
            Rect popupRect = EditorGUILayout.GetControlRect(false, 20, GUILayout.Width(scaleSlot.x));
            popupRect.position = rectSlot.position;
            //Rect popupRect = new Rect(rectSlot.xMax - popupWidth - 4f, rectSlot.y + 2f, popupWidth, rectSlot.height - 4f);
            colorLineMachine.colorSlots[i].slotType =
                (ColorSlotType)EditorGUI.EnumPopup(popupRect, slotType);
            
            var rectType = rectSlot.AlignLeft(10).AlignBottom(10);
            rectType.position += new Vector2(5, -5);
            SirenixEditorGUI.DrawSolidRect(rectType, GetColorBySlotType(colorLineMachine.colorSlots[i].slotType));
            SirenixEditorGUI.DrawBorders(rectType, 1);
            if (colorLineIndex == 0)
            {
                DrawColorFirstHint(rectSlot, i);
            }
        }
    }

    private void DrawColorFirstHint(Rect rect, int slotIndex)
    {
        Rect rectColor = rect.AlignCenter(scaleSlot.x - 20);
        rectColor.height = scaleSlot.x - 20;
        rectColor.position += new Vector2(0,20);
        var color = ColorGlobalConfig.Instance.GetColorByIndex(levelDataConfig.colorShowFirstIndex[slotIndex]);
        SirenixEditorGUI.DrawSolidRect(rectColor, color);
        SirenixEditorGUI.DrawBorders(rectColor, 1);
    }

    private Color GetColorBySlotType(ColorSlotType slotType)
    {
        switch (slotType)
        {
            case ColorSlotType.Normal:
                return Color.silver;
            case ColorSlotType.Locked:
                return Color.red;
            case ColorSlotType.Linked:
                return Color.green;
            case ColorSlotType.Frozen:
                return Color.cyan;
            default:
                return Color.black;
        }
    }
}
#endif