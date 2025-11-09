using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;

public class ColorSlot : SlotBase<Color>
{
    [SerializeField] private Color colorDefault;
    [SerializeField] private Color colorWin;
    [SerializeField] private Color colorLose;
    [SerializeField] private GameObject objHighlight;
    [SerializeField] private SpriteRenderer spriteColor;
    [SerializeField] private SpriteRenderer sprHint;
    [SerializeField] private Color[] colorHints;
    [SerializeField] private int index;
    [SerializeField] private int colorIndex = -1;
    public int ColorIndex => colorIndex;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
    }

    public override void InitData(Color data)
    {
        base.InitData(data);
        spriteColor.color = data;
    }

    public void DeSelect()
    {
        //Debug.Log("Deselect color slot");
        objHighlight.SetActive(false);
    }

    public void Select()
    {
        //Debug.Log("Select color slot");
        objHighlight.SetActive(true);
    }

    public void ResetColor()
    {
        spriteColor.color = colorDefault;
        sprHint.color = colorDefault;
        colorIndex = -1;
        data = Color.clear;
        objHighlight.SetActive(false);
        sprHint.color = colorHints[0];
    }

    private void SetColorHint(ColorHintType hintType)
    {
        switch (hintType)
        {
            case ColorHintType.None:
                SoundManager.Instance.PlaySfx(SoundType.Wrong);
                break;
            case ColorHintType.Correct:
                SoundManager.Instance.PlaySfx(SoundType.Right);
                break;
            case ColorHintType.Right:
                SoundManager.Instance.PlaySfx(SoundType.Next);
                break;
            default:
                break;
        }
       
        sprHint.color = colorHints[(int)hintType];
    }
   
    public bool CheckHintColor()
    {
        var hintType = GameManager.Instance.GetHintColorType(data, index);
        SetColorHint(hintType);
        return hintType == ColorHintType.Correct;
    }

    public void ChangeColor(int maxColor, int change)
    {
        colorIndex += change;
        if (colorIndex < 0)
            colorIndex = maxColor - 1;
        else if (colorIndex == maxColor)
            colorIndex = 0;
        var color = GameManager.Instance.GetColor(colorIndex);
        InitData(color);
    }

    public async UniTask AnimWin()
    {
        LMotion.Create(sprHint.color, colorWin, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(spriteColor.color, colorWin, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        LMotion.Create(colorWin, colorDefault, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(colorWin, colorDefault, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
    }

    public async UniTask AnimLose()
    {
        LMotion.Create(sprHint.color, colorLose, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(spriteColor.color, colorLose, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
        LMotion.Create(colorLose, colorDefault, 0.15f).Bind(x => sprHint.color = x).AddTo(this);
        await LMotion.Create(colorLose, colorDefault, 0.15f).Bind(x => spriteColor.color = x).AddTo(this);
    }
}

public enum ColorHintType
{
    None,
    Correct,
    Right
}