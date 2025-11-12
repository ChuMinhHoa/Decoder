using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[System.Serializable]
public class MainContentBase<T, TSlot> : MonoBehaviour where TSlot : SlotBase<T>
{
    [SerializeField] private SlotBase<T> slotPref;
    [SerializeField] private Transform trsParents;
    [SerializeField] private List<SlotBase<T>> slotsActive;
    [SerializeField] private List<SlotBase<T>> slotsDeActive;
    
    public void InitData(Span<T> data)
    {
        for (var i = 0; i < data.Length; i++)
        {
            var slot = GetSlotData();
            slot.InitData(data[i]);
            slot.gameObject.SetActive(true);
        }
    }

    public void InitDataAllExist(Span<T> data)
    {
        for (var i = 0; i < slotsActive.Count; i++)
        {
            if (i < data.Length)
            {
                slotsActive[i].InitData(data[i]);
            }
        }   
    }

    private SlotBase<T> GetSlotData()
    {
        if (slotsDeActive.Count > 0)
        {
            slotsActive.Add(slotsDeActive[0]);
            slotsDeActive.RemoveAt(0);
            return slotsActive[^1];
        }
        var slot = Object.Instantiate(slotPref, trsParents);
        slotsActive.Add(slot);
        return slot;
    }
    
    public void DeSpawnSlot(SlotBase<T> slot)
    {
        slotsActive.Remove(slot);
        slotsDeActive.Add(slot);
        slot.gameObject.SetActive(false);
    }
}
