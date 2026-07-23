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

    public Vector3 SpawnPosition;

    public bool CanSeePlayer;
    public Vector3 LastKnownPlayerPosition;
    public float SearchTimer;

    public Vector3 NextPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * Definition.PatrolRadius;
        randomDir += SpawnPosition;

        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, Definition.PatrolRadius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return SpawnPosition;
    }
}