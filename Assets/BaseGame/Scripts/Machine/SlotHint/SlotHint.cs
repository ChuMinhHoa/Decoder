using UnityEngine;

public class SlotHint : SlotBase<int>
{
    [SerializeField] private Color[] colors;
    [SerializeField] private SpriteRenderer sprColorChange;
    public override void InitData(int dataChange)
    {
        base.InitData(dataChange);
    }
}
