using UnityEngine;



[CreateAssetMenu(fileName = "NewItemMetadata", menuName = "Shop/Item Metadata")]
public class ItemMetadata : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price = 1;
    [SerializeField] private GameObject itemPrefab;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public int Price => price;
    public GameObject ItemPrefab => itemPrefab;
}
