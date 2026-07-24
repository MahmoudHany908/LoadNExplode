using TMPro;
using UnityEngine;

public class GoldUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private string format = "Gold: {0}";

    private void Awake()
    {

    }

    private void OnEnable()
    {
        EventBus.Subscribe<GoldChangedEvent>(OnGoldChanged);
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
