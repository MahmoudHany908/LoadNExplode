using TMPro;
using UnityEngine;
public class InGameHudController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _countDownText;

    private GamePlaySceneCountdown _gamePlaySceneCountdown;
    private void OnEnable()
    {
        EventBus.Subscribe<OnCountdownStartedEvent>(OnCountdownStarted);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnCountdownStartedEvent>(OnCountdownStarted);
    }

    private void OnCountdownStarted(OnCountdownStartedEvent e)
    {
        _gamePlaySceneCountdown = e.countdown;

    }
    private void Update()
    {

        _countDownText.text = _gamePlaySceneCountdown.GetTimeRemaining().ToString("F2");

    }
}

