using UnityEngine;

public struct PlayerSpawnedEvent : IGameEvent
{
    public readonly Player Player;
    public readonly Transform SpawnPoint;

    public PlayerSpawnedEvent(Player player, Transform spawnPoint)
    {
        Player = player;
        SpawnPoint = spawnPoint;
    }
}
