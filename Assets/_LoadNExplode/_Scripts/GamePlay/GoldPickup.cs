using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    public void SetAmount(int value)
    {
        amount = Mathf.Max(0, value);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        GoldManager goldManager = FindFirstObjectByType<GoldManager>();
        if (goldManager == null)
            return;

        goldManager.AddGold(amount);
        Destroy(gameObject);
    }
}
