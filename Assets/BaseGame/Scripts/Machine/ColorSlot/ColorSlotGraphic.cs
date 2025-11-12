using Cysharp.Threading.Tasks;
using LitMotion;
using Sirenix.OdinInspector;
using UnityEngine;

public class ColorSlotGraphic : MonoBehaviour
{
    [SerializeField] private Animation animationHint;
    [SerializeField] private SpriteRenderer spriteColor;
    [SerializeField] private SpriteRenderer sprHint;
    [SerializeField] private SpriteRenderer sprColorGlow;
    
    [SerializeField] private Color colorDefault;
    [SerializeField] private Color colorWin;
    [SerializeField] private Color colorLose;
  
    [SerializeField] private Color[] colorHints;
    
    private void ActiveColorGlow(bool isActive) => sprColorGlow.gameObject.SetActive(isActive);
    
    public async UniTask AnimWin()
    {
        ActiveColorGlow(true);
        
        await LMotion.Create(spriteColor.color, colorWin, 0.15f).Bind(x =>
        {
            sprHint.color = x;
            sprColorGlow.color = x;
            spriteColor.color = x;
        }).AddTo(this);
        
        await LMotion.Create(colorWin, colorDefault, 0.15f).Bind(x =>
        {
            sprHint.color = x;
            sprColorGlow.color = x;
            spriteColor.color = x;
        }).AddTo(this);
        
        ActiveColorGlow(false);
    }

    public async UniTask AnimLose()
    {
        ActiveColorGlow(true);
        
        await LMotion.Create(spriteColor.color, colorLose, 0.15f).Bind(x =>
        {
            sprHint.color = x;
            sprColorGlow.color = x;
            spriteColor.color = x;
        }).AddTo(this);
        
        await LMotion.Create(colorLose, colorDefault, 0.15f).Bind(x =>
        {
            sprHint.color = x;
            sprColorGlow.color = x;
            spriteColor.color = x;
        }).AddTo(this);
        ActiveColorGlow(false);
    }
    
    public void Select()
    {
        sprHint.color = colorHints[^1];
        animationHint.clip = animationHint.GetClip(MyCache.animHintLoop);
        animationHint.Play();
    }
    
    public void DeSelect()
    {
        animationHint.Stop();
        animationHint.clip = null;
       
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
