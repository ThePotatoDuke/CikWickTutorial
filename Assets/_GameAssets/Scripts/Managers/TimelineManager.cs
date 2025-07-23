using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager;
    private PlayableDirector _playableDirector;

    void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    void OnEnable()
    {
        _playableDirector.Play();
        _playableDirector.stopped += OnTimelineFinished;
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        _gameManager.ChangeGameState(GameState.Play);
    }
}
