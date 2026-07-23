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

    public bool CanSeePlayer;
    public Vector3 LastKnownPlayerPosition;
    public float SearchTimer;

    public Transform[] PatrolPoints;
    private int _patrolIndex = -1;

    public Vector3 NextPatrolPoint()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0)
            return Self.position;

        _patrolIndex = (_patrolIndex + 1) % PatrolPoints.Length;
        return PatrolPoints[_patrolIndex].position;
    }
}