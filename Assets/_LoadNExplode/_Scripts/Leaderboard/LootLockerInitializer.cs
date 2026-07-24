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
                PlayerPrefs.SetString("LL_PlayerID", response.player_id.ToString());
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Failed to start LootLocker session: " + response.errorData.message);
            }
        });
    }
}