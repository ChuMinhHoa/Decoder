using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera mainCamera;
    private readonly RaycastHit2D[] hits = new RaycastHit2D[5];
    public LayerMask WhatIsColorSlot;

    public void MouseClickCallBack()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        int count = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hits, Mathf.Infinity, WhatIsColorSlot);
    }
}
