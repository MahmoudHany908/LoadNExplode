using System.Collections;
using TMPro;
using UnityEngine;

public class ShopFeedbackUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float displaySeconds = 2f;

    private Coroutine _hideRoutine;

    private void OnEnable()
    {
        EventBus.Subscribe<BuyItemFailedEvent>(OnBuyFailed);
        EventBus.Subscribe<BuyItemSucceededEvent>(OnBuySucceeded);

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BuyItemFailedEvent>(OnBuyFailed);
        EventBus.Unsubscribe<BuyItemSucceededEvent>(OnBuySucceeded);
    }

    private void OnBuySucceeded(BuyItemSucceededEvent evt)
    {
        string itemName = evt.Item != null ? evt.Item.ItemName : "Item";
        Show($"Bought {itemName} → Slot {evt.SlotIndex + 1}");
    }

    private void OnBuyFailed(BuyItemFailedEvent evt)
    {
        string message = evt.Reason switch
        {
            BuyItemFailReason.InventoryFull => "Inventory full (max 2 items)",
            BuyItemFailReason.NotEnoughGold => "Not enough gold",
            _ => "Cannot buy item"
        };

        Show(message);
    }

    private void Show(string message)
    {
        if (feedbackText == null)
            return;

        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);

        if (_hideRoutine != null)
            StopCoroutine(_hideRoutine);

        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displaySeconds);

        if (feedbackText != null)
            feedbackText.gameObject.SetActive(false);

        _hideRoutine = null;
    }
}
