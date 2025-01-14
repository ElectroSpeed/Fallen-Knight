using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ImageCustom
{
    public Image _image;

    public List<Color> _enterColor = new List<Color>();
    public List<Color> _clickColor = new List<Color>();
    public List<Color> _exitColor = new List<Color>();

    public Sprite _enterSprite;
    public Sprite _clickSprite;
    public Sprite _exitSprite;
}