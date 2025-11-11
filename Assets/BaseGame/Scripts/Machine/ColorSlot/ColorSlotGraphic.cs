using Cysharp.Threading.Tasks;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorSlotGraphic : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteColor;
    [SerializeField] private SpriteRenderer sprHint;
    [SerializeField] private SpriteRenderer sprColorGlow;
    
    [SerializeField] private Color colorDefault;
    [SerializeField] private Color colorWin;
    [SerializeField] private Color colorLose;
  
    [SerializeField] private Color[] colorHints;
    
    MotionHandle motionHighlight;
    MotionHandle motionHighlight2;
    
    private void ActiveColorGlow(bool isActive) => sprColorGlow.gameObject.SetActive(isActive);
    
    public async UniTask AnimWin()
    {
        ActiveColorGlow(true);
        LMotion.Create(sprHint.color, colorWin, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        LMotion.Create(sprColorGlow.color, colorWin, 0.15f).Bind(x => sprColorGlow.color = x).AddTo(this);
        await LMotion.Create(spriteColor.color, colorWin, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        LMotion.Create(colorWin, colorDefault, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(colorWin, colorDefault, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        ActiveColorGlow(false);
    }

    public async UniTask AnimLose()
    {
        ActiveColorGlow(true);
        LMotion.Create(sprHint.color, colorLose, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        LMotion.Create(sprColorGlow.color, colorLose, 0.25f).Bind(x => sprColorGlow.color = x).AddTo(this);
        await LMotion.Create(spriteColor.color, colorLose, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        LMotion.Create(colorLose, colorDefault, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(colorLose, colorDefault, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        ActiveColorGlow(false);
    }
    
    public void Select()
    {
        motionHighlight = LMotion.Create(sprHint.color, colorHints[^1], 1f)
            .WithLoops(-1)
            .Bind(x => sprHint.color = x)
            .AddTo(this);
        
        motionHighlight2 = LMotion.Create(colorHints[^1], colorDefault, 1f)
            .WithLoops(-1)
            .Bind(x => sprHint.color = x)
            .AddTo(this);
    }
    
    public void DeSelect()
    {
        if (motionHighlight.IsActive())
            motionHighlight.TryCancel();
        if (motionHighlight2.IsActive())
            motionHighlight2.TryCancel();
        sprHint.color = colorDefault;
    }

    public void SetColorHint(ColorHintType hintType, bool needHint)
    {
        if (!needHint)
            return;
        sprHint.color = colorHints[(int)hintType];
    }

    public void Reset()
    {
        spriteColor.color = colorDefault;
        sprHint.color = colorDefault;
        sprHint.color = colorHints[0];
        ActiveColorGlow(false);
    }

    [Button]
    public void SetColor(Color color)
    {
        spriteColor.color = color;
        sprColorGlow.color = color;
        ActiveColorGlow(true);
    }
}
