using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class Machine : MonoBehaviour
{
    [SerializeField] private BoxCollider2D btnNextSlot;
    [SerializeField] private BoxCollider2D btnChangeColor;
    [SerializeField] private BoxCollider2D btnCheck;
    [SerializeField] private ColorLine[] colorLines;
    [SerializeField] private int currentColorLineIndex = 1;
    [SerializeField] private int currentLevel;

    [Button]
    private async UniTask LoadData()
    {
        ResetMachine();
        var data = LevelGlobalConfig.Instance.GetLevelData(currentLevel);
        await colorLines[0].InitData(data.colorFirstHint);
        colorLines[1].SelectFirstSlot();
    }
    
    [Button]
    private void ResetMachine()
    {
        currentColorLineIndex = 1;
        for (int i = 0; i < colorLines.Length; i++)
        {
            colorLines[i].ResetColor();
        }
    }

    public void CheckMouseClickDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Vector2 worldPoint = CameraManager.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (btnNextSlot != null && btnNextSlot.OverlapPoint(worldPoint))
        {
            HandleNextSlotClick();
        }
        
        if (btnChangeColor != null && btnChangeColor.OverlapPoint(worldPoint))
        {
            HandleChangeColorClick();
        }
        
        if (btnCheck != null && btnCheck.OverlapPoint(worldPoint))
        {
            HandleCheckClick();
        }
    }

    private void HandleCheckClick()
    {
        Debug.Log("on click Check");
    }

    private void HandleChangeColorClick()
    {
        Debug.Log("on click Change slot");
    }

    private void HandleNextSlotClick()
    {
        Debug.Log("on click next slot");
        colorLines[currentColorLineIndex].NextSlot();
    }
}
