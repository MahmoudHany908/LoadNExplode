using UnityEngine;
using UnityEngine.AI;

public class SearchState : INPCState
{
    private bool _reachedLastKnown;
    private float _lookTimer;
    private Vector3 _lookTarget;

    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.PatrolSpeed;
        ctx.Agent.isStopped = false;
        ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);
        _reachedLastKnown = false;
        _lookTimer = 0f;
    }

    public void Tick(NPCContext ctx)
    {
        // Re-acquire player at any time during search.
        if (ctx.CanSeePlayer)
        {
            if (ctx.Definition.Behavior == NPCBehaviorType.Hostile)
                ctx.StateMachine.ChangeState(ctx.States.Chase, ctx);
            else
                ctx.StateMachine.ChangeState(ctx.States.Flee, ctx);
            return;
        }

        ctx.SearchTimer -= Time.deltaTime;

        // Phase 1: walk to last known position.
        if (!_reachedLastKnown)
        {
            if (!ctx.Agent.pathPending && ctx.Agent.remainingDistance < 0.5f)
            {
                _reachedLastKnown = true;
                ctx.Agent.isStopped = true;
                PickLookDirection(ctx);
            }
            return;
        }

        // Phase 2: look around by rotating toward random nearby directions.
        _lookTimer -= Time.deltaTime;
        if (_lookTimer <= 0f)
        {
            PickLookDirection(ctx);
        }

        // Smoothly rotate toward the look target.
        Vector3 dir = (_lookTarget - ctx.Self.position);
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            ctx.Self.rotation = Quaternion.Slerp(ctx.Self.rotation, targetRot, Time.deltaTime * 3f);
        }

        // Time's up — go back to patrol.
        if (ctx.SearchTimer <= 0f)
            ctx.StateMachine.ChangeState(ctx.States.Patrol, ctx);
    }

    public void Exit(NPCContext ctx)
    {
        ctx.Agent.isStopped = false;
    }

    private void PickLookDirection(NPCContext ctx)
    {
        // Pick a random point on a small circle around the NPC to look toward.
        Vector2 circle = Random.insideUnitCircle.normalized * 3f;
        _lookTarget = ctx.Self.position + new Vector3(circle.x, 0f, circle.y);
        _lookTimer = Random.Range(0.8f, 1.5f);
    }
}