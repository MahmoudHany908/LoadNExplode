using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
    public Vector3 Position { get; }
}
