using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource _audioSourceMusic;
    public AudioSource _audioSourceSFX;

    public Sound[] _musicSounds;
    public Sound[] _sfxSounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlaySound(AudioSource audioSource, Sound[] soundList, string name)
    {
        Sound sound = Array.Find(soundList, s => s._name == name);

        if (sound == null)
        {
            Debug.Log($"{name} Not Found");
        }
        else
        {
            audioSource.clip = sound._clip;
            audioSource.Play();
        }
    }

    private void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void StopMusic()
    {
        StopSound(_audioSourceMusic);
    }

    public void StopSFX()
    {
        StopSound(_audioSourceSFX);
    }

    public void PlayMusic(string name)
    {
        PlaySound(_audioSourceMusic, _musicSounds, name);
    }

    public void PlaySFX(string name)
    {
        PlaySound(_audioSourceSFX, _sfxSounds, name);
    }
    public bool IsSoundInList(Sound[] soundList, string name)
    {
        return Array.Exists(soundList, s => s._name == name);
    }
}
