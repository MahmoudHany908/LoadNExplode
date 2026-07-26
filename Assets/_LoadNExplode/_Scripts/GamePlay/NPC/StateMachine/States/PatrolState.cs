using UnityEngine;

public class PatrolState : INPCState
{
    private float _idleTimer;
    private bool _isIdling;

    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.PatrolSpeed;
        ctx.Agent.isStopped = false;
        ctx.Agent.SetDestination(ctx.NextPatrolPoint());
        _isIdling = false;
    }

    public void Tick(NPCContext ctx)
    {
        // --- Vision transitions (checked every frame regardless of idle) ---
        if (ctx.CanSeePlayer)
        {
            if (ctx.Definition.Behavior == NPCBehaviorType.Hostile)
            {
                ctx.StateMachine.ChangeState(ctx.States.Chase, ctx);
                return;
            }

            // Civilian – flee from the player.
            ctx.StateMachine.ChangeState(ctx.States.Flee, ctx);
            return;
        }

        // --- Idle-at-point logic ---
        if (_isIdling)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
            {
                _isIdling = false;
                ctx.Agent.isStopped = false;
                ctx.Agent.SetDestination(ctx.NextPatrolPoint());
            }
            return;
        }

        // Arrived at the current patrol point — start idling.
        if (!ctx.Agent.pathPending && ctx.Agent.remainingDistance < 0.5f)
        {
            _isIdling = true;
            _idleTimer = Random.Range(ctx.Definition.PatrolIdleMin, ctx.Definition.PatrolIdleMax);
            ctx.Agent.isStopped = true;
        }
    }

    public void Exit(NPCContext ctx)
    {
        _isIdling = false;
    }
}