using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextCustom
{
    public TMP_Text _text;

    public List<Color> _enterColor = new List<Color>();
    public List<Color> _clickColor = new List<Color>();
    public List<Color> _exitColor = new List<Color>();
}