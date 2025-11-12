using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public class MachineAnim : MonoBehaviour
{
    [SerializeField] private Color colorLightIn;
    [SerializeField] private SpriteRenderer lightInRenderer;
    [SerializeField] private Transform trsLightRotate;
    [SerializeField] private SpriteRenderer sprLight;
    [SerializeField] private Animation lightAnimation;
    [SerializeField] private AnimationClip[] animationClips;

    private Vector3 vectorRotate = new Vector3(0, 0, 1);
    public async UniTask AnimLightPlay()
    {
        var rotate = 130f;
        for (int i = 0; i < 4; i++)
        {
            var x = trsLightRotate.eulerAngles.z - rotate / 4;
            trsLightRotate.eulerAngles = vectorRotate * x;
            await UniTask.Delay(100);
        }
    }
    
    public async UniTask AnimLightQuit()
    {
        var rotate = 130f;
        for (int i = 0; i < 4; i++)
        {
            var x = trsLightRotate.eulerAngles.z + rotate / 4;
            trsLightRotate.eulerAngles = vectorRotate * x;
            await UniTask.Delay(100);
        }
    }

    [Button]
    public void PlayLightAnim(AnimLightMachine animLightMachine)
    {
        var index = (int)animLightMachine;
        if (index < 0 || index >= animationClips.Length) return;
        lightAnimation.clip = null;
        var clip = GetAnimClip(index);
        lightAnimation.clip =clip;
        lightAnimation.Play();
    }

    private AnimationClip GetAnimClip(int index)
    {
        return index >= 0 && index < animationClips.Length ? animationClips[index] : null;
    }


    public async UniTask LightStart()
    {
        sprLight.gameObject.SetActive(true);
        await LMotion.Create(0f, 1f, 0.5f)
            .Bind(x =>
            {
                var c = sprLight.color;
                c.a = x;
                sprLight.color = c;
            }).AddTo(this);
    }
    
    public async UniTask LightEnd()
    {
        await LMotion.Create(1f, 0f, 0.5f)
            .Bind(x =>
            {
                var c = sprLight.color;
                c.a = x;
                sprLight.color = c;
            }).AddTo(this);
        sprLight.gameObject.SetActive(false);
        PlayLightAnim(AnimLightMachine.Default);
        lightInRenderer.color = colorLightIn;
    }
}

public enum AnimLightMachine
{
    Idle = 0,
    Warning = 1,
    Win = 2,
    Lose = 3,
    Default = 4
}