using UnityEngine;

[CreateAssetMenu(fileName = "ShopItemDefinition", menuName = "Shop/Item Definition")]
public class ShopItemDefinition : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price = 1;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemSpawnBehavior spawnBehavior;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public int Price => price;
    public GameObject ItemPrefab => itemPrefab;
    public ItemSpawnBehavior SpawnBehavior => spawnBehavior;
}
