using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPUiController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image xpProgressBar;
    [SerializeField] private TextMeshProUGUI xpText;       
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Effects")]
    [SerializeField] private GameObject levelUpVFX;
    [SerializeField] private AudioClip levelUpSFX;

    private void OnEnable()
    {

        EventBus.Subscribe<PlayerGainXPEvent>(OnEvent);
        EventBus.Subscribe<PlayerLeveledUpEvent>(OnEvent);
    }

    private void OnDisable()
    {

        EventBus.Unsubscribe<PlayerGainXPEvent>(OnEvent);
        EventBus.Unsubscribe<PlayerLeveledUpEvent>(OnEvent);
    }



    public void OnEvent(PlayerGainXPEvent e)
    {
        // Update the Progress Bar

        xpProgressBar.fillAmount = (float)e.CurrentXP / e.RequiredXP;


        // Update the Text
        xpText.text = $"{e.CurrentXP} / {e.RequiredXP}";
        levelText.text = $"Level {e.CurrentLevel}";

    }

    public void OnEvent(PlayerLeveledUpEvent e)
    {
        // Update level text immediately
        levelText.text = $"Level {e.NewLevel}";

        // Play Level Up effects
        if (levelUpVFX != null) Instantiate(levelUpVFX, transform.position, Quaternion.identity);
        if (levelUpSFX != null) AudioSource.PlayClipAtPoint(levelUpSFX, transform.position);

  
    }
}