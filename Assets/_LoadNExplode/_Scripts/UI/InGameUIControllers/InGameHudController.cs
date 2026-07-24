using TMPro;
using UnityEngine;
public class InGameHudController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private TextMeshProUGUI _goldText;

    private GamePlaySceneCountdown _gamePlaySceneCountdown;
    private void OnEnable()
    {
        EventBus.Subscribe<OnCountdownStartedEvent>(OnCountdownStarted);
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<OnCountdownStartedEvent>(OnCountdownStarted);
        EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
    }

    private void OnCountdownStarted(OnCountdownStartedEvent e)
    {
        _gamePlaySceneCountdown = e.countdown;

    }
    private void OnGoldChanged(GoldChangedEvent evt)
    {

        _goldText.text = evt.Gold.ToString("N0");
    }
    private void Update()
    {

        _countDownText.text = _gamePlaySceneCountdown.GetTimeRemaining().ToString("F2");

    }
}