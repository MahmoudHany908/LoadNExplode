using _LoadNExplode._Scripts.Audio;
using UnityEngine;


public class GamePlaySceneCountdown : MonoBehaviour
{
    [Header("Countdown Settings")]
    [SerializeField] private float countdownDuration = 10f;
    [SerializeField] private float playAccMusicAfter = 10f;



    private bool isAccelarted = false;
    private float timeRemaining;
    private bool isCountingDown = true;

    private void Start()
    {

        timeRemaining = countdownDuration;
        EventBus.Publish(new OnCountdownStartedEvent(this));
    }

    private void Update()
    {
        if (!isCountingDown) return;


        timeRemaining -= Time.unscaledDeltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isCountingDown = false;

            OnCountdownFinished();
        }

        if (timeRemaining < playAccMusicAfter) {
            HandleAccMusic();
        }

    }

    private void HandleAccMusic() {
        if (isAccelarted) return;
        isAccelarted = true;
        MusicManager.Instance.PlayAccGameLoopMusic();
    }

    private void OnCountdownFinished()
    {
        Debug.Log("Countdown finished! Firing event...");

        EventBus.Publish(new OnCountdownFinishedEvent());
        this.enabled = false;
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

}