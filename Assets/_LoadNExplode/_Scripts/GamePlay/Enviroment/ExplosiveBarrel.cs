using System.Collections;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IDamageable, ILaunchable
{
    [Header("Settings")]
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _timeToExplode = 0.25f;
    [SerializeField] private int _explosionDamage = 100;
    [SerializeField] private LayerMask _explosionLayers;

    [Tooltip("The minimum speed required to trigger the explosion")]
    public float explosionThreshold = 10f;


    [Header("Effects")]
    [SerializeField] private GameObject _explodeEffectsPrefab;
    [SerializeField] private AudioClip _explosionSound;


    private bool _isExplode = false;
    private Rigidbody rb;

    public Vector3 Position => transform.position;
    public void TakeDown() { }
    public void TakeDamage(int damage)
    {
        if (_isExplode) return;
        _isExplode = true;
        StartCoroutine(StartExplosion());
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator StartExplosion()
    {
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

    public void Launch(Vector3 velocity, LaunchApplyMode mode, LaunchPad source)
    {
        if (rb)
            switch (mode)
            {
                case LaunchApplyMode.SetVelocityDirect:
                    rb.linearVelocity = velocity;
                    break;
                case LaunchApplyMode.VelocityChange:
                    rb.AddForce(velocity, ForceMode.VelocityChange);
                    break;
                case LaunchApplyMode.Impulse:

                    rb.AddForce(velocity, ForceMode.Impulse);
                    break;
            }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ColCol");
        if (rb != null && rb.linearVelocity.magnitude >= explosionThreshold)
        {
            StartCoroutine(StartExplosion());
        }
    }
}