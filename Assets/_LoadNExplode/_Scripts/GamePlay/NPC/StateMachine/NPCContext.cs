using UnityEngine;
using UnityEngine.AI;

public class NPCContext
{
    public NavMeshAgent Agent;
    public Transform Self;
    public Transform Eye;
    public Transform Player;
    public NPCDefinition Definition;
    public NPCStateMachine StateMachine;
    public NPCStates States;
    public float AttackCooldownTimer;

    public Vector3 SpawnPosition;

    public bool CanSeePlayer;
    public Vector3 LastKnownPlayerPosition;
    public float SearchTimer;

    public Vector3 NextPatrolPoint()
    {
        float minDist = Definition.MinPatrolDistance * Definition.MinPatrolDistance;
        Vector3 bestPoint = SpawnPosition;
        float bestScore = -1f;

        // Try several candidates and pick the best one rather than accepting the first random hit.
        const int maxAttempts = 6;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * Definition.PatrolRadius;
            randomDir.y = 0f; // keep patrol on the horizontal plane
            randomDir += SpawnPosition;

            if (!NavMesh.SamplePosition(randomDir, out NavMeshHit hit, Definition.PatrolRadius, NavMesh.AllAreas))
                continue;

            float sqrDist = (hit.position - Self.position).sqrMagnitude;

            // Reject points too close to the NPC — these cause tiny shuffles.
            if (sqrDist < minDist)
                continue;

            // Prefer points roughly ahead of the NPC to avoid constant 180° turns.
            Vector3 toPoint = (hit.position - Self.position).normalized;
            float forwardDot = Vector3.Dot(Self.forward, toPoint);
            float score = sqrDist + forwardDot * 4f; // bias toward forward-ish points

            if (score > bestScore)
            {
                bestScore = score;
                bestPoint = hit.position;
            }
        }

        return bestPoint;
    }
}