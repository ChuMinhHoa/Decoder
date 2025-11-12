using System.Threading;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.Serialization;

public partial class ColorSlot : SlotBase<int>
{
    [SerializeField] ColorSlotGraphic colorSlotGraphic;
    [SerializeField] private int index;
    [SerializeField] private int colorIndex = -1;
    private ReactiveValue<bool> needHint = new();
    public int ColorIndex => colorIndex;
    
   
    private StateMachine stateMachine;
    

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        InitStateMachine();
        needHint = GameManager.Instance.machine.NeedHint;
    }

    private void InitStateMachine()
    {
        stateMachine = new StateMachine();
        stateMachine.RegisterState(ColorSlotSleepState);
        stateMachine.Run();
    }

    public override void InitData(int dataChange)
    {
        base.InitData(dataChange);
        _ = stateMachine.ChangeState(ColorSlotInitState);
    }
    
    public void ResetColor()
    {
        colorSlotGraphic.Reset();
        colorIndex = -1;
        data = -1;
    }

    private void SetColorHint(ColorHintType hintType)
    {
        switch (hintType)
        {
            case ColorHintType.None:
                SoundManager.Instance.PlaySfx(SoundType.Wrong);
                break;
            case ColorHintType.Correct:
                SoundManager.Instance.PlaySfx(SoundType.Right);
                break;
            case ColorHintType.Right:
                SoundManager.Instance.PlaySfx(SoundType.Next);
                break;
            default:
                break;
        }

        colorSlotGraphic.SetColorHint(hintType, needHint.Value);
    }
   
    public bool CheckHintColor()
    {
        var hintType = GameManager.Instance.GetHintColorType(data, index);
        SetColorHint(hintType);
        return hintType == ColorHintType.Correct;
    }

    public void ChangeColor(int maxColor, int change)
    {
        colorIndex += change;
        if (colorIndex < 0)
            colorIndex = maxColor - 1;
        else if (colorIndex == maxColor)
            colorIndex = 0;
        var colorID = GameManager.Instance.GetColor(colorIndex);
        InitData(colorID);
    }


    public void Select(bool resetColor = false)
    {
        if (resetColor)
            colorSlotGraphic.Reset();
        colorSlotGraphic.Select();
    }
    
    public void DeSelect()
    {
        colorSlotGraphic.DeSelect();
    }

    public async UniTask AnimWin() => await colorSlotGraphic.AnimWin();

    public async UniTask AnimLose() => await colorSlotGraphic.AnimLose();
}

public enum ColorHintType
{
    None,
    Correct,
    Right
}