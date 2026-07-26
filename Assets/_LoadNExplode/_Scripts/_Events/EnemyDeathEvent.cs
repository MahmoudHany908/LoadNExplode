using UnityEngine;

public struct EnemyDeathEvent : IGameEvent
{
    public readonly GameObject EnemyGameObject;
    public readonly Vector3 DeathPosition;
    public readonly int GoldReward;
    public readonly NPCBehaviorType BehaviorType;

    public EnemyDeathEvent(GameObject enemyGameObject, Vector3 deathPosition, int goldReward, NPCBehaviorType behaviorType)
    {
        EnemyGameObject = enemyGameObject;
        DeathPosition = deathPosition;
        GoldReward = goldReward;
        BehaviorType = behaviorType;
    }
}
