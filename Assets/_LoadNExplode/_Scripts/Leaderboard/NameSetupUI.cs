using UnityEngine;
using TMPro;
using LootLocker.Requests; 

public class NameSetupUI : MonoBehaviour
{
    public TMP_InputField nameInputField;

    public void SaveNameAndContinue()
    {
        string playerName = nameInputField.text.Trim();

        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("SavedPlayerName", playerName);
            PlayerPrefs.Save();

            LootLockerSDKManager.SetPlayerName(playerName, (response) =>
            {
                if (response.success) 
                { 
                    Debug.Log("Player name updated on server."); 
                    EventBus.Publish(new RequestSceneLoadEvent("MainMenuScene"));
                }
                else
                {
                    Debug.LogError("Failed to set name: " + response.errorData.message);
                }
            });
        }
    }
}