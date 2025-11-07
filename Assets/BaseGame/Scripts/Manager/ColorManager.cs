using System;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
   public Color[] colors;

   private void Start()
   {
       LoadData();
   }

   private void LoadData()
   {
       
   }

   public Span<Color> GetColors(int amount)
   {
       if (colors == null || colors.Length == 0)
           return Array.Empty<Color>();

       amount = Mathf.Clamp(amount, 1, colors.Length);

       var temp = (Color[])colors.Clone();
       for (var i = temp.Length - 1; i > 0; i--)
       {
           var j = UnityEngine.Random.Range(0, i + 1);
           (temp[i], temp[j]) = (temp[j], temp[i]);
       }

       var result = new Color[amount];
       Array.Copy(temp, 0, result, 0, amount);
       return result;
   }
}
