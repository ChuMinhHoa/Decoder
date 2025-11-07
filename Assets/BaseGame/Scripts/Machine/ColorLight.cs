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
    
    public void NextSlot()
    {
        lastSlotIndex = currentSlotIndex;
        currentSlotIndex++;
        if (currentSlotIndex >= colorSlots.Length)
            currentSlotIndex = 0;
        colorSlots[currentSlotIndex].Select();
        colorSlots[lastSlotIndex].DeSelect();
    }

    public async UniTask InitData(Color[] dataColorFirstHint)
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
             colorSlots[i].InitData(dataColorFirstHint[i]);
             await UniTask.WaitForSeconds(0.15f);
        }
    }

    public void ResetColor()
    {
        for (var i = 0; i < colorSlots.Length; i++)
        {
            colorSlots[i].ResetColor();
            colorSlots[i].DeSelect();
        }
    }
}
