using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinLooseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _blackBackgroundObject;
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private GameObject _losePopup;

    [Header("Settings")]
    [SerializeField] private float _animationDuration = 0.3f;

    private Image _blackBackgroundImage;
    private RectTransform _winPopupTransform;
    private RectTransform _losePopupTransform;

    void Awake()
    {
        _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
        _winPopupTransform = _winPopup.GetComponent<RectTransform>();
        _losePopupTransform = _losePopup.GetComponent<RectTransform>(); // ✅ Fixed
    }

    public void OnGameWin()
    {
        _blackBackgroundObject.SetActive(true);
        _winPopup.SetActive(true);

        // Reset alpha to 0 before fading
        var color = _blackBackgroundImage.color;
        _blackBackgroundImage.color = new Color(color.r, color.g, color.b, 0f);

        // Reset popup scale to 0 for animation
        _winPopupTransform.localScale = Vector3.zero;

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _winPopupTransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
    }
    public void OnGameLose()
    {
        _blackBackgroundObject.SetActive(true);
        _losePopup.SetActive(true);

        // Reset alpha to 0 before fading
        var color = _blackBackgroundImage.color;
        _blackBackgroundImage.color = new Color(color.r, color.g, color.b, 0f);

        // Reset popup scale to 0 for animation
        _winPopupTransform.localScale = Vector3.zero;

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _losePopupTransform.DOScale(1.5f, _animationDuration).SetEase(Ease.OutBack);
    }
}
