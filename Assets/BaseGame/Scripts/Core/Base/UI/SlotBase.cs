using UnityEngine;

public class SlotBase<T> : MonoBehaviour
{
    public T data;

    public virtual void InitData(T data)
    {
        this.data = data;
    }
}
