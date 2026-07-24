using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStats : MonoBehaviour
{

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;


    public static PlayerStats Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }


    private void Update()
    {
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            GainXP(50);
        }
    }

    #region XP and Leveling System
    private int GetRequiredXPForLevel(int level)
    {
        return 100 + (level * 50);
    }
    public void GainXP(int amount)
    {
        if (amount <= 0) return;

        CurrentXP += amount;
        int requiredXP = GetRequiredXPForLevel(CurrentLevel);

        while (CurrentXP >= requiredXP)
        {
            int previousLevel = CurrentLevel;
            CurrentLevel++;

            int overflowXP = CurrentXP - requiredXP;
            CurrentXP = overflowXP;

            EventBus.Publish(new PlayerLeveledUpEvent(previousLevel, CurrentLevel, overflowXP));
            requiredXP = GetRequiredXPForLevel(CurrentLevel);
        }

        EventBus.Publish(new PlayerGainXPEvent(amount, CurrentXP, requiredXP, CurrentLevel));
    }

    #endregion

}

