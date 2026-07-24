using System.Collections.Generic;
using UnityEngine;

public class DeadlyLove : MonoBehaviour, IAbility
{
    [SerializeField] private float abilityRange = 5f;
    [SerializeField] private float abilityDuration = 5f;
    [SerializeField] private LayerMask targetLayerMask;

    private GameObject player;
    public void Activate()
    {
        Debug.Log($"DeadlyLove.Activate() called at position {transform.position}, range {abilityRange}, layerMask {targetLayerMask.value}");
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("DeadlyLove Ability: Player not found in the scene!");
                return;
            }
        }
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, abilityRange, targetLayerMask.value);

        Debug.Log($"OverlapSphere found {hitColliders.Length} colliders");

        for (int i = 0; i < hitColliders.Length; i++)
        {
            var col = hitColliders[i];
            Debug.Log($"  Hit: {col.gameObject.name} (layer {col.gameObject.layer})");

            if (col.TryGetComponent(out NPCController npc))
            {
                Debug.Log($"DeadlyLove Ability activated on NPC: {npc.name}");
                npc.SetCharm(abilityDuration, player.transform);
            }
        }
    }

    // Implement the interface methods implicitly as well
    public void Started()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("DeadlyLove Ability has started.");
    }

    public void Tick(float _deltaTime)
    {

    }



}

