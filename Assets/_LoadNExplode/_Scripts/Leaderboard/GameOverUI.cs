using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField nameInputField;
    public Button submitButton;

    public void OnSubmitScoreButtonClicked()
    {
        submitButton.interactable = false;

        string chosenName = nameInputField.text;

        if (string.IsNullOrWhiteSpace(chosenName))
        {
            chosenName = "Anonymous";
        }

        Debug.Log($"Submitting score for: {chosenName}");
        GameScoreManager.Instance.EndRun(chosenName);
    }
}