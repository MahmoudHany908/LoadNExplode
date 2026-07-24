using UnityEngine;
using TMPro;
using LootLocker.Requests;

public class PlayerStickyRowUI : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    private readonly string leaderboardID = "35697";

    private void OnEnable()
    {
        string savedName = PlayerPrefs.GetString("SavedPlayerName", "Unknown");
        nameText.text = savedName;
        scoreText.text = PlayerPrefs.GetInt("PersonalBest", 0).ToString();
        rankText.text = "...";

        string playerID = PlayerPrefs.GetString("LL_PlayerID", "");

        if (PlayerPrefs.GetInt("PersonalBest", 0) > 0)
        {
            LootLockerSDKManager.GetMemberRank(leaderboardID, playerID, (response) =>
            {
                if (response.statusCode == 404)
                {
                    // Player is unranked, gracefully set rank to "-"
                    rankText.text = "-";
                }
                else if (response.success && response.rank > 0)
                {
                    rankText.text = response.rank.ToString();
                    scoreText.text = response.score.ToString();
                }
                else
                {
                    if (!response.success)
                    {
                        Debug.LogWarning("Failed to get member rank: " + (response.errorData != null ? response.errorData.message : "Unknown error"));
                    }
                    rankText.text = "-";
                }
            });
        }
        else
        {
            rankText.text = "-";
        }
    }
}