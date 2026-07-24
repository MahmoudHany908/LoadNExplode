using TMPro;
using UnityEngine;

public class GoldUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private string format = "Gold: {0}";
    [SerializeField] private GoldManager goldManager;

    private void Awake()
    {
        if (goldManager == null)
            goldManager = FindFirstObjectByType<GoldManager>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);

        if (goldManager != null)
            Refresh(goldManager.Gold);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GoldChangedEvent>(OnGoldChanged);
    }

    private void OnGoldChanged(GoldChangedEvent evt)
    {
        Refresh(evt.Gold);
    }

    private void Refresh(int gold)
    {
        if (goldText == null)
            return;

        goldText.text = string.Format(format, gold);
    }
}
