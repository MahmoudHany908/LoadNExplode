using _LoadNExplode._Scripts.Audio;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    private GoldManager goldManager;

    public void SetAmount(int value, GoldManager goldManager)
    {
        amount = Mathf.Max(0, value);
        this.goldManager = goldManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            goldManager.AddGold(amount);
            MusicManager.Instance.PlayCoin();
            Destroy(gameObject);
        }



    }
}
