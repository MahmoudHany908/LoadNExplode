using System;
using UnityEngine;

public class NPC : UnitBase
{
    [SerializeField] private int goldReward = 1;

    private int Health = 100;
    private NPCController NPCController;

    private void Awake()
    {
        NPCController = GetComponent<NPCController>();
    }

    public override void TakeDamage(int damage)
    {
        if (Health >= 0)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }

        }
        else if (Health <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        EventBus.Publish(new EnemyDeathEvent(gameObject, transform.position, goldReward));
        //EventBus.Publish(new EnemyDeathEvent());

        int xp = UnityEngine.Random.Range(20, 30);
        PlayerStats.Instance.GainXP(xp);

        Debug.Log($"Player Gained{xp}XP By Killing An NPC ");

        Destroy(gameObject);
    }
}



