using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private ShopItemDefinition item;
    [SerializeField] private Button buyButton;

    private void OnEnable()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(OnBuyClicked);
    }

    private void OnDisable()
    {
        if (buyButton != null)
            buyButton.onClick.RemoveListener(OnBuyClicked);
    }

    private void OnBuyClicked()
    {
        EventBus.Publish(new BuyItemRequestedEvent(item));
    }
}