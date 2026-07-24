using System.Collections;
using UnityEngine;

/// <summary>
/// Mine logic: arm delay, NPC trigger explodes, player touch kills player without exploding.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MineItem : MonoBehaviour
{
    [Header("Arming")]
    [SerializeField] private float armDelay = 1.5f;
    [SerializeField] private GameObject armedVisual;

    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private int explosionDamage = 100;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip explosionSound;

    private bool _armed;
    private bool _resolved;

    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;

        if (armedVisual != null)
            armedVisual.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(ArmCoroutine());
    }

    private IEnumerator ArmCoroutine()
    {
        _armed = false;
        yield return new WaitForSeconds(armDelay);
        _armed = true;

        if (armedVisual != null)
            armedVisual.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_armed || _resolved)
            return;

        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            KillPlayerWithoutExploding(player);
            return;
        }

        if (other.GetComponentInParent<NPC>() != null)
            Explode();
    }

    private void KillPlayerWithoutExploding(Player player)
    {
        _resolved = true;

        if (player != null && player.Health > 0)
            player.Die();

        Destroy(gameObject);
    }

    private void Explode()
    {
        _resolved = true;

        if (explosionSound != null)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 3f);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].GetComponentInParent<Player>() != null)
                continue;

            IDamageable damageable = hits[i].GetComponentInParent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(explosionDamage);
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
#endif
}
