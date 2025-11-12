using System;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : Singleton<InputManager>
{
    public Machine mainMachine;
    
 
    // Update is called once per frame
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => MouseClickDownCallBack());
        this.UpdateAsObservable()
            .Where(_=> Input.GetMouseButtonUp(0))
            .Subscribe(_=> MouseClickUpCallBack());
    }

    private void MouseClickUpCallBack()
    {
        CameraManager.Instance.MouseClickCallBack();
        mainMachine.CheckMouseClickUp();
    }

    private void MouseClickDownCallBack()
    {
        CameraManager.Instance.MouseClickCallBack();
        mainMachine.CheckMouseClickDown();
    }

}
