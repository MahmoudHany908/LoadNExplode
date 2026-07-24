using UnityEngine;
using TMPro;

public class NameSetupUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInputField;

    public void SaveNameAndContinue()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "Player_" + Random.Range(1000, 9999);
        }

        PlayerPrefs.SetString("SavedPlayerName", playerName);
        PlayerPrefs.Save();

        EventBus.Publish(new RequestSceneLoadEvent("MainMenuScene"));
    }
}