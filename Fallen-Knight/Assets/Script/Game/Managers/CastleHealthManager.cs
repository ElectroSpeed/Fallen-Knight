using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealthManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _castleHealthText;
    [SerializeField] private Image _damageImage;
    [SerializeField] private MenuEventsManager _menuManager;
    [SerializeField] private GameObject _losePanel;
    [HideInInspector] public int _castleHealth;
    private Color _originalColor;

    private void Start()
    {
        _originalColor = _damageImage.color;
        _castleHealth = 5;
        _castleHealthText.text = _castleHealth.ToString() + "/5";
    }

    public void RemoveCastleHealth(int amount)
    {
        _castleHealth -= amount;
        FeedBackDamage();
        UpdateCastleHealthNumber();
        if (_castleHealth <= 0)
        {
            _menuManager.PauseScene();
            _losePanel.SetActive(true);
        }
    }

    private void UpdateCastleHealthNumber()
    {
        if (_castleHealthText != null)
        {
            _castleHealthText.text = _castleHealth.ToString() + "/5";
        }
    }
    
    private void FeedBackDamage()
    {
        if (_damageImage != null)
        {
            StartCoroutine(LerpDamageImageTransparency());
        }
    }
    
    private IEnumerator LerpDamageImageTransparency()
    {
        float duration = 0.75f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.PingPong(elapsed * 50, 25) / 255f;
            _damageImage.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _damageImage.color = _originalColor;
    }
}