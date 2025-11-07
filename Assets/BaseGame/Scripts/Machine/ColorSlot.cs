using UnityEngine;

public class ColorSlot : SlotBase<Color>
{
    [SerializeField] private Color colorDefault;
    [SerializeField] private GameObject objHighlight;
    [SerializeField] private SpriteRenderer spriteColor;
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
    }
}
