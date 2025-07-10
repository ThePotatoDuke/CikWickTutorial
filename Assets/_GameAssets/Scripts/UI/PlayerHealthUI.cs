
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Refeerences")]
    [SerializeField] private Image[] _playerHealthImages;
    [Header("Images")]
    [SerializeField] private Sprite _playerHealtySprite;
    [SerializeField] private Sprite _playerUnhealtySprite;
    private RectTransform[] _playerHealthTransforms;

    [Header("Settings")]
    [SerializeField] private float _scaleDuration;

    private void Awake()
    {
        _playerHealthTransforms = new RectTransform[_playerHealthImages.Length];

        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthTransforms[i] = _playerHealthImages[i].gameObject.GetComponent<RectTransform>();
        }
    }

    public void AnimateDamage()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            if (_playerHealthImages[i].sprite == _playerHealtySprite)
            {
                AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
                break;
            }
        }
    }

    public void AnimateDamageForAll()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            AnimateDamageSprite(_playerHealthImages[i], _playerHealthTransforms[i]);
        }
    }
    private void AnimateDamageSprite(Image activeImage, RectTransform activeImageTransfrom)
    {
        activeImageTransfrom.DOScale(0f, _scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _playerUnhealtySprite;
            activeImageTransfrom.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack);
        });
    }
}
