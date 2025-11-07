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
            .Subscribe(_ => MouseClickCallBack());
    }

    private void MouseClickCallBack()
    {
        CameraManager.Instance.MouseClickCallBack();
        mainMachine.CheckMouseClickDown();
    }

}
