using UnityEngine;

public struct EnemyDeathEvent : IGameEvent
{
    readonly public GameObject EnemyGameObject;
    readonly public Vector3 DeathPosition;

    EnemyDeathEvent(GameObject EnemyGameObject, Vector3 DeathPosition)
    {
        this.EnemyGameObject = EnemyGameObject;
        this.DeathPosition = DeathPosition;
    }
}
