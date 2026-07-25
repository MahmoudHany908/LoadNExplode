using UnityEngine;

public abstract class UnitBase : MonoBehaviour, IDamageable
{
    public Vector3 Position => transform.position;

    public abstract void TakeDamage(int damage);
    public abstract void TakeDown();
    public abstract void Die();


}
