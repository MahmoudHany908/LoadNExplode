public struct PlayerGainXPEvent : IGameEvent
{
    public readonly int AmountGained;
    public readonly int CurrentXP;
    public readonly int RequiredXP;
    public readonly int CurrentLevel;

    public PlayerGainXPEvent(int amountGained, int currentXP, int requiredXP, int currentLevel)
    {
        AmountGained = amountGained;
        CurrentXP = currentXP;
        RequiredXP = requiredXP;
        CurrentLevel = currentLevel;
    }
}