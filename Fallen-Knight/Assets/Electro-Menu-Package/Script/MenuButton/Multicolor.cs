using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Multicolor : MonoBehaviour
{
    private Image _image;
    private TMP_Text _text;

    private float _transitionPoint;

    public List<Color> _colors = new List<Color>();
    public float _time;
    private int _currentColorIndex;
    private int _targetColorIndex;

    private void Start()
    {
        if (this.GetComponent<Image>())
        {
            _image = this.GetComponent<Image>();
            _colors.Add(_image.color);
        }
        if (this.GetComponent<TMP_Text>())
        {
            _text = this.GetComponent<TMP_Text>();
            _colors.Add(_text.color);
        }
    }

    void Update()
    {
        CheckColorsList();
    }

    public void ResetColorIndex()
    {
        _currentColorIndex = 0;
        _targetColorIndex = 1;
    }

    private void CheckColorsList()
    {
        if (_colors == null)
            return;

        if (_colors.Count > 1)
        {
            Transition();
        }
        else
        {
            if (_image != null)
            {
                _image.color = _colors[0];
            }
            if (_text != null)
            {
                _text.color = _colors[0];
            }
        }
    }

    private void Transition()
    {
        _transitionPoint += Time.deltaTime / _time;

        if (_image != null)
        {
            _image.color = Color.Lerp(_colors[_currentColorIndex], _colors[_targetColorIndex], _transitionPoint);
        }
        if (_text != null)
        {
            _text.color = Color.Lerp(_colors[_currentColorIndex], _colors[_targetColorIndex], _transitionPoint);
        }
        if(_transitionPoint >= 1f)
        {
            _transitionPoint = 0f;
            _currentColorIndex = _targetColorIndex;
            _targetColorIndex++;
            if (_targetColorIndex == _colors.Count)
            {
                _targetColorIndex = 0;
            }
        }
    }
}
