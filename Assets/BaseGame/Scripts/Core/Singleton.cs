// csharp
// File: `Assets/BaseGame/Scripts/Core/Singleton.cs`
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting;

    /// <summary>
    /// Override to control whether the singleton GameObject should persist across scenes.
    /// Default: true
    public bool dontDestroyOnLoadEnabled;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning($"[Singleton<{typeof(T).Name}>] Instance already destroyed on application quit. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    var singleton = _instance as Singleton<T>;
                    if (singleton != null && singleton.dontDestroyOnLoadEnabled)
                        Object.DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (dontDestroyOnLoadEnabled)
                Object.DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }
}