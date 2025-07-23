using DG.Tweening;
using MaskTransitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerUI : MonoBehaviour
{
    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _quitButton;

    [SerializeField]
    Button _howToPlay;

    [SerializeField]
    Button _credits;

    [Header("References")]
    [SerializeField]
    private RectTransform _buttonsTransform;

    [SerializeField]
    private RectTransform _howToPlayTransform;

    [SerializeField]
    private RectTransform _creditsTransform;

    [SerializeField]
    private Button _backFromHowToPlay;

    [SerializeField]
    private Button _backFromCredits;

    void Awake()
    {
        _playButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.Play(SoundType.TransitionSound);
            TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);
        });

        _quitButton.onClick.AddListener(() =>
        {
            AudioManager.Instance.Play(SoundType.ButtonClickSound);
            Application.Quit();
        });

        _howToPlay.onClick.AddListener(() =>
        {
            SlideToPanel(_howToPlayTransform);
        });

        _credits.onClick.AddListener(() =>
        {
            SlideToPanel(_creditsTransform);
        });

        _backFromHowToPlay.onClick.AddListener(() =>
        {
            SlideBackToMenu(_howToPlayTransform);
        });

        _backFromCredits.onClick.AddListener(() =>
        {
            SlideBackToMenu(_creditsTransform);
        });
    }

    void Start()
    {
        float panelWidth = _buttonsTransform.rect.width;

        _howToPlayTransform.anchoredPosition = new Vector2(panelWidth, 0);
        _creditsTransform.anchoredPosition = new Vector2(panelWidth, 0);
    }

    private void SlideToPanel(RectTransform targetPanel)
    {
        float panelWidth = _buttonsTransform.rect.width;

        // Slide current menu off to the right
        _buttonsTransform.DOAnchorPos(new Vector2(panelWidth, 0), 0.5f).SetEase(Ease.InOutSine);

        // Slide target panel into place (visible on right)
        targetPanel.DOAnchorPos(_buttonsTransform.anchoredPosition, 0.5f).SetEase(Ease.InOutSine);
    }

    private void SlideBackToMenu(RectTransform currentPanel)
    {
        float panelWidth = _buttonsTransform.rect.width;

        // Hide current panel by sliding it right
        currentPanel.DOAnchorPos(new Vector2(panelWidth, 0), 0.5f).SetEase(Ease.InOutSine);

        // Bring main menu back into its original anchored position
        _buttonsTransform.DOAnchorPos(new Vector2(0, 0), 0.5f).SetEase(Ease.InOutSine);
    }
}
