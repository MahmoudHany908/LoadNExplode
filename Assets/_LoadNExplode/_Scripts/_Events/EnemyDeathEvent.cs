using UnityEngine;

public struct EnemyDeathEvent : IGameEvent
{
    public readonly GameObject EnemyGameObject;
    public readonly Vector3 DeathPosition;
    public readonly int GoldReward;

    public EnemyDeathEvent(GameObject enemyGameObject, Vector3 deathPosition, int goldReward)
    {
        EnemyGameObject = enemyGameObject;
        DeathPosition = deathPosition;
        GoldReward = goldReward;
    }
}
