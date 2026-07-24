using System;
using UnityEngine;

public class Person : UnitBase
{
    [SerializeField] private int goldReward = 1;

    private int Health = 100;

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
        Destroy(gameObject);
    }
}



