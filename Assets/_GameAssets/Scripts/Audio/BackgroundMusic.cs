using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance { get; private set; }

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void SetMusicMute(bool isMuted)
    {
        _audioSource.mute = isMuted;
    }

    public void PlayBackgroundMusic(bool isMusicPlaying)
    {
        if (_audioSource == null)
            return;

        if (isMusicPlaying)
            _audioSource.Play();
        else
            _audioSource.Stop();
    }
}
