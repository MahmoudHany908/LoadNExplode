using System;
using UnityEngine;

public class NPC : UnitBase
{
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
        EventBus.Publish(new EnemyDeathEvent());
        Destroy(gameObject);
    }
}



