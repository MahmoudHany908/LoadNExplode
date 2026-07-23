
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private float lifeTime = 4f;

    private List<Collider> _collidersInRange = new List<Collider>();

    private void Start()
    {

        TriggerExplosion();
    }
    public void TriggerExplosion()
    {
        transform.localScale = Vector3.one * _explosionRadius;
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        Explode();
    }

    private void Explode()
    {
        _collidersInRange.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                _collidersInRange.Add(collider);
                damageable.TakeDamage(100);
            }
        }

    }
}
