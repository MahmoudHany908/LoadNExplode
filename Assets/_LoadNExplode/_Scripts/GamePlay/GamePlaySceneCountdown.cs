using UnityEngine;


public class GamePlaySceneCountdown : MonoBehaviour
{
    [Header("Countdown Settings")]
    [SerializeField] private float countdownDuration = 10f;




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