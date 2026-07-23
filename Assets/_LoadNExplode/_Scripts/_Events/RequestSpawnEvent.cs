using UnityEngine;

public struct RequestSpawnEvent : IGameEvent
{
    public readonly Transform SpawnPoint;

    public RequestSpawnEvent(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
}