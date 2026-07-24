using System.Collections;
using UnityEngine;
using TMPro;

public class Bomb : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private int _explosionDamage = 100;
    [SerializeField] private bool _destroyOnExplode = false;

    [Header("Visuals & Effects")]
    [SerializeField] private GameObject _explodeEffectsPrefab;
    [SerializeField] private GameObject bombVisual;
    [SerializeField] private TextMeshPro _countdownText;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _tickingSound;

    private Transform _parent;

    private void Start()
    {
        _parent = transform.parent;
        PositionCountdownText();
    }


    public void TriggerExplosion(float timeToExplode)
    {
        bombVisual.SetActive(false);
        Vector3 currentScale = bombVisual.transform.localScale;
        bombVisual.transform.localScale = new Vector3(_explosionRadius, currentScale.y, _explosionRadius);
        StartCoroutine(CountdownAndExplodeCoroutine(timeToExplode));
    }

    private IEnumerator CountdownAndExplodeCoroutine(float totalTime)
    {
        float remainingTime = totalTime;

        while (remainingTime > 0f)
        {
            if (_countdownText != null)
            {
                _countdownText.text = remainingTime.ToString("F2");
            }

            yield return null;
            remainingTime -= Time.deltaTime;
        }


        if (_countdownText != null)
        {
            _countdownText.text = "0.00";
        }

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


        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_explosionDamage);
            }
        }
        if (!_destroyOnExplode) bombVisual.SetActive(false);

        if (_destroyOnExplode)
        {
            Destroy(gameObject);
        }
    }

    private void PositionCountdownText()
    {
        if (_countdownText == null || _parent == null) return;


        Collider parentCollider = _parent.GetComponent<Collider>();

        if (parentCollider != null)
        {
            _countdownText.transform.position = parentCollider.bounds.max;
        }
        else
        {
            _countdownText.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
        }
    }
}