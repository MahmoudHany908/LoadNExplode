public struct PlayerLeveledUpEvent : IGameEvent
{
    public readonly int PreviousLevel;
    public readonly int NewLevel;
    public readonly int OverflowXP;

    public PlayerLeveledUpEvent(int previousLevel, int newLevel, int overflowXP)
    {
        PreviousLevel = previousLevel;
        NewLevel = newLevel;
        OverflowXP = overflowXP;
    }
}