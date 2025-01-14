using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GetSetAudioMixer : MonoBehaviour
{
    public AudioMixer _audioMixer;

    public Slider _sfxSlider;
    public Slider _musicSlider;
    public Slider _mainSlider;

    public static GetSetAudioMixer Instance;

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
    }

    public void SetMusicSound(float volume)
    {
        SetVolume("Music", volume);
    }

    public void SetSFXSound(float volume)
    {
        SetVolume("SFX", volume);
    }

    public void SetMainSound(float volume)
    {
        SetVolume("Master", volume);
    }

    public void SetVolume(string volumeName, float volume)
    {
        if (!_audioMixer.SetFloat(volumeName, volume))
        {
            Debug.LogWarning("Audio parameter not found : " + volumeName);
        }
    }

    public float GetVolume(string volumeName)
    {
        float value;
        if (_audioMixer.GetFloat(volumeName, out value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Audio parameter not found : " + volumeName);
            return 0f;
        }
    }
}
