using UnityEngine;
using TMPro;

public class PlayerStickyRowUI : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        nameText.text = PlayerPrefs.GetString("SavedPlayerName", "Unknown");
        scoreText.text = PlayerPrefs.GetInt("PersonalBest", 0).ToString();

        rankText.text = "-";
    }
}