using UnityEngine;
using UnityEngine.UI;

public class SetAudioSlider : MonoBehaviour
{
    [SerializeField] private SliderType _sliderType;

    private Slider _audioSlider;

    private enum SliderType
    {
        SFX,
        Main,
        Music
    }

    private void Start()
    {
        _audioSlider = GetComponent<Slider>();
        ApplyAudioSliderSettings();
    }

    private void ApplyAudioSliderSettings()
    {
        switch (_sliderType)
        {
            case SliderType.SFX:
                GetSetAudioMixer.Instance._sfxSlider = _audioSlider;
                _audioSlider.value = GetSetAudioMixer.Instance.GetVolume("SFX");
                _audioSlider.onValueChanged.AddListener(GetSetAudioMixer.Instance.SetSFXSound);
                break;

            case SliderType.Main:
                GetSetAudioMixer.Instance._mainSlider = _audioSlider;
                _audioSlider.value = GetSetAudioMixer.Instance.GetVolume("Master");
                _audioSlider.onValueChanged.AddListener(GetSetAudioMixer.Instance.SetMainSound);
                break;

            case SliderType.Music:
                GetSetAudioMixer.Instance._musicSlider = _audioSlider;
                _audioSlider.value = GetSetAudioMixer.Instance.GetVolume("Music");
                _audioSlider.onValueChanged.AddListener(GetSetAudioMixer.Instance.SetMusicSound);
                break;
        }
    }
}
