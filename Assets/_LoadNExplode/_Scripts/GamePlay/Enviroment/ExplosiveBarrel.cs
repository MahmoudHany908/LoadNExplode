using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _timeToExplode = 1.5f;
    [SerializeField] private int _explosionDamage = 100;
    [SerializeField] private LayerMask _explosionLayers;

    [Header("Effects")]
    [SerializeField] private GameObject _explodeEffectsPrefab;
    [SerializeField] private GameObject _explosionRadiusVisual;
    [SerializeField] private AudioClip _explosionSound;

    public Vector3 Position => transform.position;

    private bool _isExplode = false;

    private void Start()
    {
        if (_explosionRadiusVisual != null)
        {
            _explosionRadiusVisual.SetActive(false);
        }
    }
    public void TakeDown() { }
    public void TakeDamage(int damage)
    {
        if (_isExplode) return;
        _isExplode = true;
        StartCoroutine(StartExplosion());
    }

    private IEnumerator StartExplosion()
    {
        if (_explosionRadiusVisual != null)
        {
            _explosionRadiusVisual.SetActive(true);

            Vector3 currentScale = _explosionRadiusVisual.transform.localScale;

            _explosionRadiusVisual.transform.localScale = new Vector3(_explosionRadius, currentScale.y, _explosionRadius);
        }

        yield return new WaitForSecondsRealtime(_timeToExplode);

        Explode();
    }

    private void Explode()
    {
        if (_explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(_explosionSound, transform.position);
        }

        if (_explodeEffectsPrefab != null)
        {
            Instantiate(_explodeEffectsPrefab, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius, _explosionLayers);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_explosionDamage);
            }
        }

        Destroy(gameObject);
    }
}