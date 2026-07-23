using UnityEngine;
using LootLocker.Requests;

public class LootLockerInitializer : MonoBehaviour
{
    private void Start()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("LootLocker Guest Session Started Successfully");
            }
            else
            {
                Debug.LogError("Failed to start LootLocker session: " + response.errorData.message);
            }
        });
    }
}