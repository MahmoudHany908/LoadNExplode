using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StunGrenade : MonoBehaviour
{
    [Header("Fuse Settings")]
    [SerializeField] private float fuseTime = 2f;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Effects")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject stunEffectPrefab;

    private bool hasExploded;

    private void Start()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    private void Explode()
    {
        if (hasExploded)
        {
            return;
        }
        hasExploded = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, targetLayerMask);

        if (hits == null || hits.Length == 0)
        {
            Debug.Log($"[{gameObject.name}] StunGrenade found no targets in range ({explosionRadius}).");
        }
        else
        {
            for (int i = 0; i < hits.Length; i++)
            {
                NPCController npc = hits[i].GetComponent<NPCController>();
                if (npc == null)
                {
                    continue;
                }

                Debug.Log($"[{gameObject.name}] StunGrenade stunned NPC: {npc.name}");
                npc.Stun(stunDuration);

                if (stunEffectPrefab != null)
                {
                    GameObject stunFx = Instantiate(stunEffectPrefab, npc.transform.position, Quaternion.identity, npc.transform);
                    Destroy(stunFx, stunDuration);
                }
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, stunDuration);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}