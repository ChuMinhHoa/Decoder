using Sirenix.OdinInspector;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource sfxSourceLoop;
    public SoundData[] soundData;
    
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }
    
    [Button]
    public void PlaySfx(SoundType soundType)
    {
        for (var i = 0; i < soundData.Length; i++)
        {
            if (soundType == soundData[i].soundType)
            {
                sfxSource.volume = soundData[i].volume;
                sfxSource.PlayOneShot(soundData[i].clip);
                break;
            }
            
        }
    }
    
    [Button]
    public void PlaySfxLoop(SoundType soundType)
    {
        for (var i = 0; i < soundData.Length; i++)
        {
            if (soundType == soundData[i].soundType)
            {
                sfxSourceLoop.volume = soundData[i].volume;
                sfxSourceLoop.loop = soundData[i].loop;
                sfxSourceLoop.clip = soundData[i].clip;
                sfxSourceLoop.Play();
                break;
            }
            
        }
    }
    
}

[System.Serializable]
public class SoundData
{
    public AudioClip clip;
    public float volume = 1f;
    public bool loop = false;
    public SoundType soundType;
}

public enum SoundType
{
    None,
    Bg,
    Next,
    Change,
    Right,
    Wrong,
    Check,
    Win,
    PlayGear,
    Lose,
    Warning
}