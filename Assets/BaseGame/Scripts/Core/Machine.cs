using System;
using System.Collections.Generic;
using System.Text;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class Machine : MonoBehaviour
{
    private StateMachine stateMachine;
    [SerializeField] private MachineAnim machineAnim;
    [SerializeField] private BoxCollider2D btnNextSlot;
    [SerializeField] private BoxCollider2D btnChangeColorLeft;
    [SerializeField] private BoxCollider2D btnChangeColorRight;
    [SerializeField] private BoxCollider2D btnCheck;
    [SerializeField] private Collider2D btnPlay;
    [SerializeField] private ColorLine[] colorLines;
    [SerializeField] private ColorLine colorExpertLine;
    [SerializeField] private int currentColorLineIndex = 1;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxColor { get; set; }

    private LevelConfig levelConfig;
    private LevelConvert levelConvert;

    private void Awake()
    {
        InitMachine();
    }

    private void InitMachine()
    {
        stateMachine = new StateMachine();
        stateMachine.RegisterState(MachineSleepState);
        stateMachine.Run();
        ChangeEnableButtonGamePlay(false);
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

    public void CheckMouseClickDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector2 worldPoint = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (btnNextSlot != null && btnNextSlot.OverlapPoint(worldPoint))
        {
            HandleNextSlotClick();
        }
        
        if (btnChangeColorLeft != null && btnChangeColorLeft.OverlapPoint(worldPoint))
        {
            HandleChangeColorClick(-1);
        }
        
        if (btnChangeColorRight != null && btnChangeColorRight.OverlapPoint(worldPoint))
        {
            HandleChangeColorClick(1);
        }
        
        if (btnCheck != null && btnCheck.OverlapPoint(worldPoint))
        {
            _ = HandleCheckClick();
        }
        
        if (btnPlay != null && btnPlay.OverlapPoint(worldPoint))
        {
            _ = HandlePlayClick();
        }
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

    public bool IsHaveThisColor(int colorIndex, int indexIgnore)
    {
        return colorLines[currentColorLineIndex].IsHaveThisColor(colorIndex, indexIgnore);
    }
    
    private void ChangeEnableButtonGamePlay(bool active)
    {
        btnCheck.enabled = active;
        btnChangeColorLeft.enabled = active;
        btnNextSlot.enabled = active;
        btnChangeColorRight.enabled = active;
    }

    private void ChangeEnableButtonPlay(bool active) => btnPlay.enabled = active;
}
