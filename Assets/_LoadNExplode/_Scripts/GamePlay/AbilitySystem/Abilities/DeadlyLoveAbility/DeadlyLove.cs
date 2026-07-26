using UnityEngine;
using UnityEngine.UI;

public class DeadlyLove : MonoBehaviour, IAbility
{
    [Header("Ability Settings")]
    [SerializeField] private float abilityRange = 5f;
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Effects")]
    [SerializeField] private GameObject charmEffectPrefab;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 5f;
    [SerializeField] private Image cooldownImage;

    private float currentCooldown;
    private GameObject player;

    public void Activate()
    {
        if (currentCooldown > 0f)
        {
            Debug.LogWarning($"[{gameObject.name}] DeadlyLove is on cooldown ({currentCooldown:F1}s remaining).");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning($"[{gameObject.name}] DeadlyLove cannot activate — player reference is null.");
            return;
        }

        Collider[] hits = Physics.OverlapSphere(player.transform.position, abilityRange, targetLayerMask);

        if (hits == null || hits.Length == 0)
        {
            Debug.Log($"[{gameObject.name}] DeadlyLove found no targets in range ({abilityRange}).");
            return;
        }

        bool charmedAny = false;

        for (int i = 0; i < hits.Length; i++)
        {
            NPCController npc = hits[i].GetComponent<NPCController>();

            if (npc == null)
            {
                continue;
            }

            Debug.Log($"[{gameObject.name}] DeadlyLove charmed NPC: {npc.name}");
            npc.SetCharm(abilityDuration, player.transform);

            if (charmEffectPrefab != null)
            {
                GameObject fx = Object.Instantiate(charmEffectPrefab, npc.transform.position, Quaternion.identity, npc.transform);
                Object.Destroy(fx, abilityDuration);
            }

            charmedAny = true;
        }

        if (!charmedAny)
        {
            Debug.Log($"[{gameObject.name}] DeadlyLove found colliders but no NPCController components.");
            return;
        }

        // Start cooldown only if at least one NPC was charmed
        currentCooldown = cooldownTime;

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f;
        }
    }

    public void Started()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning($"[{gameObject.name}] DeadlyLove could not find a GameObject with tag 'Player'.");
        }

        // Reset cooldown UI on start
        currentCooldown = 0f;

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }
    }

    public void Tick(float _deltaTime)
    {
        if (currentCooldown <= 0f)
        {
            return;
        }

        currentCooldown -= _deltaTime;

        if (currentCooldown < 0f)
        {
            currentCooldown = 0f;
        }

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = currentCooldown / cooldownTime;
        }
    }
}
