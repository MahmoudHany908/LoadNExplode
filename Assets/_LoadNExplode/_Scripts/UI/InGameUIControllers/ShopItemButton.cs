using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] private ItemMetadata itemMetadata;
    [SerializeField] private Button buyButton;
    
    [Header("Optional UI Linking")]
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private TMPro.TextMeshProUGUI priceText;
    [SerializeField] private Image iconImage;

    private void Start()
    {
        if (itemMetadata != null)
        {
            if (nameText != null) nameText.text = itemMetadata.ItemName;
            if (priceText != null) priceText.text = itemMetadata.Price.ToString();
            if (iconImage != null)
            {
                iconImage.sprite = itemMetadata.Icon;
                iconImage.enabled = itemMetadata.Icon != null;
            }
        }
    }

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
        EventBus.Publish(new BuyItemRequestedEvent(itemMetadata));
    }
}