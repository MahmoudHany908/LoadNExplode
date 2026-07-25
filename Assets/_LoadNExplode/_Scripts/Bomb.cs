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
    private bool _isDefused = false;

    private Coroutine _countdownCoroutine;

    // CHANGED: moved init logic from Start() to Awake() so it's guaranteed
    // to run before Player.Start() calls TriggerExplosion() on it.
    private void Awake()
    {
        _parent = transform.parent;
        PositionCountdownText();
        _countdownText.color = Color.white;
    }

    public void TriggerExplosion(float timeToExplode)
    {
        _isDefused = false;
        _countdownText.color = Color.red;

        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
        }

        if (_countdownText != null)
        {
            _countdownText.color = Color.white;
        }

        // CHANGED: SetActive(false) -> SetActive(true).
        // This visual is meant to represent the bomb during the countdown,
        // so it should turn ON here, not off.
        if (bombVisual != null)
        {
            bombVisual.SetActive(true);
            Vector3 currentScale = bombVisual.transform.localScale;
            bombVisual.transform.localScale = new Vector3(_explosionRadius, currentScale.y, _explosionRadius);
        }

        _countdownCoroutine = StartCoroutine(CountdownAndExplodeCoroutine(timeToExplode));
    }

    public void Defuse(string txt = "DEFUSED")
    {
        if (_isDefused) return;

        _isDefused = true;

        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }

        if (_countdownText != null)
        {
            _countdownText.text = txt;
            _countdownText.color = Color.green;
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // CHANGED: removed Destroy(gameObject, 1). Destroying the bomb here
        // permanently breaks it, since Player caches its `bomb` reference
        // once in Awake() and never re-fetches it. After one defuse, every
        // future TriggerExplosion() call on respawn was silently doing
        // nothing because `bomb` pointed to a destroyed object.
        // Just hide the visual instead so the same Bomb component can be reused.
        if (bombVisual != null)
        {
            bombVisual.SetActive(false);
        }
    }

    private IEnumerator CountdownAndExplodeCoroutine(float totalTime)
    {
        float remainingTime = totalTime;

        while (remainingTime > 0f)
        {
            if (_isDefused)
            {
                yield break;
            }

            if (_countdownText != null)
            {
                _countdownText.text = remainingTime.ToString("F2");
            }

            yield return null;
            remainingTime -= Time.deltaTime;
        }

        if (_isDefused)
        {
            yield break;
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

        if (bombVisual != null && !_destroyOnExplode)
        {
            bombVisual.SetActive(false);
        }

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