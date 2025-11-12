using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

public class MachineButton : MonoBehaviour
{
    [SerializeField] private Collider2D btnCollider;
    [SerializeField] private Transform trsBg;
    [SerializeField] private Transform trsShadow;
    [SerializeField] private Vector3 vectorScale;
    [SerializeField] private Vector3 vectorOffset;
    private MotionHandle motionHandle;
    private MotionHandle motionMoveHandle;
    private Action actionCallBack;
    private Vector3 vectorDefault;
    private Vector3 vectorMove;
    private bool isOnMotion;
    private void Awake()
    {
        vectorDefault = trsShadow.position;
        vectorMove = trsShadow.position - vectorOffset;
    }

    public bool IsOnClick( Vector2 worldPoint )
    {
        return btnCollider.OverlapPoint(worldPoint);
    }

    public void OnButtonDown()
    {
        if (isOnMotion) return;
        
        isOnMotion = true;
        
        LMotion.Create(Vector3.one, vectorScale, .15f).Bind(x=>
        {
            trsBg.localScale = x;
            trsShadow.localScale = x;
        }).AddTo(this);
        
        LMotion.Create(trsBg.position, vectorMove, 0.15f).Bind(x =>
        {
            x.z = 0;
            trsBg.position = x;
        }).AddTo(this);
        LMotion.Create(trsShadow.position, vectorMove, 0.15f).Bind(x =>trsShadow.position = x).AddTo(this);
    }
    
    public async UniTask OnButtonUp()
    {
        if (!isOnMotion) return;
        actionCallBack();
        LMotion.Create(vectorScale, Vector3.one, .1f).Bind(x=>
        {
            trsBg.localScale = x;
            trsShadow.localScale = x;
        }).AddTo(this);

        LMotion.Create(trsBg.localPosition, Vector3.zero, 0.1f).Bind(x => trsBg.localPosition = x).AddTo(this);
        await LMotion.Create(trsShadow.position, vectorDefault, 0.1f).Bind(x => trsShadow.position = x)
            .AddTo(this);

        isOnMotion = false;
    }

    public void SetActionCallBack(Action actionCallBackChange) => actionCallBack = actionCallBackChange;

    public void ChangeEnableButton(bool active)
    {
        btnCollider.enabled = active;
    }
}
