using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public enum GamePlayMode
{
    HintMode,
    NoHintMode
}

public partial class Machine : MonoBehaviour
{
    private StateMachine stateMachine;
    [SerializeField] private MachineAnim machineAnim;
    [SerializeField] private MachineButton btnNextSlot;
    [SerializeField] private MachineButton btnChangeColorLeft;
    [SerializeField] private MachineButton btnChangeColorRight;
    [SerializeField] private MachineButton btnCheck;
    [SerializeField] private Collider2D btnPlay;
    [SerializeField] private ColorLine[] colorLines;
    [SerializeField] private ColorLine colorExpertLine;
    [SerializeField] private int currentColorLineIndex = 1;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxColor { get; set; }

    private LevelConfig levelConfig;
    private LevelConvert levelConvert;
    
    public GamePlayMode gamePlayMode;

    [OdinSerialize]public ReactiveValue<bool> NeedHint = new();

    private void Awake()
    {
        InitMachine();
        ChangeMode(gamePlayMode);
    }

    private void InitMachine()
    {
        stateMachine = new StateMachine();
        stateMachine.RegisterState(MachineSleepState);
        stateMachine.Run();
        ChangeEnableButtonGamePlay(false);

        btnNextSlot.SetActionCallBack(HandleNextSlotClick);
        btnChangeColorLeft.SetActionCallBack(() => HandleChangeColorClick(-1));
        btnChangeColorRight.SetActionCallBack(() => HandleChangeColorClick());
        btnCheck.SetActionCallBack(() => _ = HandleCheckClick());
    }
    
    [Button]
    private void ResetMachine()
    {
        currentColorLineIndex = 1;
        for (var i = 0; i < colorLines.Length; i++)
        {
            colorLines[i].ResetColor();
        }

        colorExpertLine.ResetColor();
    }

    [Button]
    public void ChangeMode(GamePlayMode mode)
    {
        gamePlayMode = mode;
        NeedHint.Value = gamePlayMode == GamePlayMode.HintMode;
    }

    public void CheckMouseClickDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector2 worldPoint = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (btnNextSlot.IsOnClick(worldPoint)) btnNextSlot.OnButtonDown();

        if (btnChangeColorLeft.IsOnClick(worldPoint)) btnChangeColorLeft.OnButtonDown();

        if (btnChangeColorRight.IsOnClick(worldPoint)) btnChangeColorRight.OnButtonDown();

        if (btnCheck.IsOnClick(worldPoint)) btnCheck.OnButtonDown();
        
        if (btnPlay != null && btnPlay.OverlapPoint(worldPoint))
        {
            _ = HandlePlayClick();
        }
    }

    public void CheckMouseClickUp()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector2 worldPoint = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (btnNextSlot.IsOnClick(worldPoint)) _ = btnNextSlot.OnButtonUp();

        if (btnChangeColorLeft.IsOnClick(worldPoint)) _ = btnChangeColorLeft.OnButtonUp();

        if (btnChangeColorRight.IsOnClick(worldPoint)) _ = btnChangeColorRight.OnButtonUp();

        if (btnCheck.IsOnClick(worldPoint)) _ = btnCheck.OnButtonUp();
    }

    private async UniTask HandlePlayClick()
    {
        SoundManager.Instance.PlaySfx(SoundType.PlayGear);
        await machineAnim.AnimLightPlay();
        await stateMachine.ChangeState(MachineInitState);
    }

    private async UniTask HandleCheckClick()
    {
        SoundManager.Instance.PlaySfx(SoundType.Check);
        ChangeEnableButtonGamePlay(false);
        if (!CheckEnoughColorSlot())
        {
            ChangeEnableButtonGamePlay(true);
            return;
        }
        colorLines[currentColorLineIndex].DeselectCurrentSlot();
        var result = await colorLines[currentColorLineIndex].CheckHintColor(500);
        if (result)
            await stateMachine.ChangeState(MachineWinState);
        else
        {
            currentColorLineIndex++;
            if (currentColorLineIndex >= colorLines.Length)
            {
                await stateMachine.ChangeState(MachineLoseState);
                return;
            }
            colorLines[currentColorLineIndex].SelectFirstSlot();
            colorLines[currentColorLineIndex].ChangeColorSlot(maxColor, 1);
            ChangeEnableButtonGamePlay(true);
        }
    }

    private bool CheckEnoughColorSlot()
    {
        return colorLines[currentColorLineIndex].IsEnoughColorSlot();
    }

    private void HandleChangeColorClick(int change = 1 )
    {
        SoundManager.Instance.PlaySfx(SoundType.Change);
        colorLines[currentColorLineIndex].ChangeColorSlot(maxColor, change);
    }

    private void HandleNextSlotClick()
    {
        SoundManager.Instance.PlaySfx(SoundType.Next);
        colorLines[currentColorLineIndex].NextSlot();
    }

    public ColorHintType GetHintColorType(int colorID, int index)
    {
        var findIndex = -1;
        for (var i = 0; i < levelConvert.colorSecretIndex.Length; i++)
        {
            if (levelConvert.colorSecretIndex[i] != colorID) continue;
            findIndex = i;
            break;
        }

        if (findIndex == index) return ColorHintType.Correct;
        return findIndex >= 0 ? ColorHintType.Right : ColorHintType.None;
    }

    public int GetColor(int index)
    {
        return colorExpertLine.GetColor(index);
    }
    
    private void ChangeEnableButtonGamePlay(bool active)
    {
        btnCheck.ChangeEnableButton(active);
        btnChangeColorLeft.ChangeEnableButton(active);
        btnNextSlot.ChangeEnableButton(active);
        btnChangeColorRight.ChangeEnableButton(active);
    }

    private void ChangeEnableButtonPlay(bool active) => btnPlay.enabled = active;
}
