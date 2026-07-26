using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(int damage);
    public void TakeDown();
    public Vector3 Position { get; }
}
