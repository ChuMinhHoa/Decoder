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
        colorSlots[currentSlotIndex].Select(false);
    }

    public void DeselectCurrentSlot()
    {
        colorSlots[currentSlotIndex].DeSelect();
    }

    public void NextSlot()
    {
        var indexSlotSameColor = GetIndexSameColor(colorSlots[currentSlotIndex].ColorIndex);
        lastSlotIndex = currentSlotIndex;
        if (indexSlotSameColor != -1)
            currentSlotIndex = indexSlotSameColor;
        else 
            currentSlotIndex++;
        
        if (currentSlotIndex >= colorSlots.Length)
            currentSlotIndex = 0;
        colorSlots[currentSlotIndex].Select(indexSlotSameColor != -1);
        colorSlots[lastSlotIndex].DeSelect();
    }

    private int GetIndexSameColor(int colorIndex)
    {
        if (colorIndex == -1)
            return -1;
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i].ColorIndex == colorIndex && i != currentSlotIndex)
                return i;
        }

        return -1;
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

    public async UniTask InitData(int[] dataColorFirstHint)
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

    public int GetColor(int index)
    {
        return colorSlots[index].data;
    }

    public bool IsEnoughColorSlot()
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            if (colorSlots[i].data == -1)
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
                //Debug.Log(i + " " + colorIndex);
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
