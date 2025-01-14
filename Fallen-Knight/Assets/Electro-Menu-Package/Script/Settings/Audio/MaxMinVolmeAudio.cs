using UnityEngine;
using UnityEngine.UI;

public class MaxMinVolumeAudio : MonoBehaviour
{
    private Slider _audioSlider;
    private float _sliderValue;

    public int _maxSliderValue = 0;
    public int _minSliderValue = -45;

    private void Start()
    {
        _audioSlider = GetComponent<Slider>();
        _audioSlider.maxValue = _maxSliderValue;
        _audioSlider.minValue = _minSliderValue;
    }

    private void Update()
    {
        _sliderValue = _audioSlider.value;

        if (_audioSlider.value == _minSliderValue)
        {
            _audioSlider.minValue = -80;
            _audioSlider.value = _audioSlider.minValue;
        }
        else if (_audioSlider.minValue == -80 && _audioSlider.value != -80)
        {
            _audioSlider.minValue = _minSliderValue;
            _audioSlider.value = _audioSlider.minValue + 1;
        }
    }
}