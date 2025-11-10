using System;
using UnityEngine;

[Serializable]
public class AnimStateBase<T>  where T : Enum
{
    public Animation anim;
    public AnimationClip[] clips;
    
    public virtual void PlayAnim(T stateType)
    {
        var index = Convert.ToInt32(stateType);
        if (index < 0 || index >= clips.Length)
        {
            Debug.LogError("Invalid state type index");
            return;
        }
        anim.clip = clips[index];
    }
} 