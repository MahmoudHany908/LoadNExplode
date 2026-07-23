using UnityEngine;

public struct PlayerDeathEvent : IGameEvent
{
    public readonly Player Player;
    public readonly Vector3 DeathPosition;

    public PlayerDeathEvent(Player player, Vector3 deathPosition)
    {
        this.Player = player;
        this.DeathPosition = deathPosition;
    }
}

