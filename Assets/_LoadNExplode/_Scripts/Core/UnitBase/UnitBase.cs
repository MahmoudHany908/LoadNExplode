using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable
{

    public abstract void TakeDamage(int damage);
    public abstract void Die();

}
