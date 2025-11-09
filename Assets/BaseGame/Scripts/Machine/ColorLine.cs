using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    [SerializeField] private ColorSlot[] colorSlots;
    [SerializeField] private int currentSlotIndex;
    [SerializeField] private int lastSlotIndex;

    public void SelectFirstSlot()
    {
        currentSlotIndex = 0;
        lastSlotIndex = -1;
        colorSlots[currentSlotIndex].Select();
    }

    public void DeselectCurrentSlot()
    {
        colorSlots[currentSlotIndex].DeSelect();
    }

    public void NextSlot()
    {
        lastSlotIndex = currentSlotIndex;
        currentSlotIndex++;
        if (currentSlotIndex >= colorSlots.Length)
            currentSlotIndex = 0;
        colorSlots[currentSlotIndex].Select();
        colorSlots[lastSlotIndex].DeSelect();
    }

    public async UniTask<bool> CheckHintColor(int timeDelay = 100)
    {
        var count = 0;
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i].CheckHintColor()) count++;
            await UniTask.Delay(timeDelay);
        }

        return count == colorSlots.Length;
    }

    public async UniTask InitData(Color[] dataColorFirstHint)
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (i >= dataColorFirstHint.Length)
                break;
            colorSlots[i].InitData(dataColorFirstHint[i]);
            SoundManager.Instance.PlaySfx(SoundType.Next);
            await UniTask.WaitForSeconds(0.15f);
        }
    }

    public void ResetColor()
    {
        currentSlotIndex = 0;
        lastSlotIndex = -1;
        for (var i = 0; i < colorSlots.Length; i++)
        {
            colorSlots[i].ResetColor();
            colorSlots[i].DeSelect();
        }
    }

    public void ChangeColorSlot(int maxColor, int change)
    {
        colorSlots[currentSlotIndex].ChangeColor(maxColor, change);
    }

    public Color GetColor(int index)
    {
        return colorSlots[index].data;
    }

    public bool IsEnoughColorSlot()
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i].data == Color.clear)
                return false;
        }

        return true;
    }

    public bool IsHaveThisColor(int colorIndex, int indexIgnore)
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i].ColorIndex == colorIndex && i != indexIgnore)
            {
                Debug.Log(i + " " + colorIndex);
                return true;
            }
        }

        return false;
    }
    
    public async UniTask AnimWin()
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            _ = colorSlots[i].AnimWin();
            await UniTask.Delay(100);
        }
    }

    public async UniTask AnimLose()
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            _ = colorSlots[i].AnimLose();
            await UniTask.Delay(100);
        }
    }
}
