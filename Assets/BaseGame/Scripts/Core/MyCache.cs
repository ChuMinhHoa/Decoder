using System;
using System.Text;
using UnityEngine;

public static class MyCache
{
    public const string animHintLoop = "HintAnim";
    public static T DecodeBase64Json<T>(string base64)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        
        Debug.Log(json);
        return JsonUtility.FromJson<T>(json);
    }
}
