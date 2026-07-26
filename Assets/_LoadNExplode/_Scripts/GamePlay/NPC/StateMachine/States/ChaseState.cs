using UnityEngine;

public class ChaseState : INPCState
{
    private float _repathTimer;

    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.ChaseSpeed;
        ctx.Agent.acceleration = ctx.Definition.ChaseAcceleration;
        ctx.Agent.angularSpeed = ctx.Definition.ChaseAngularSpeed;
        ctx.Agent.isStopped = false;
        _repathTimer = 0f;

        ctx.LastKnownPlayerPosition = ctx.Player.position;
        ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);
    }

    public void Tick(NPCContext ctx)
    {
        if (ctx.CanSeePlayer)
        {
            ctx.LastKnownPlayerPosition = ctx.Player.position;

            // Repath on an interval instead of every frame to avoid NavMesh micro-stutter.
            _repathTimer -= Time.deltaTime;
            if (_repathTimer <= 0f)
            {
                ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);
                _repathTimer = ctx.Definition.ChaseRepathInterval;
            }

            float sqrRange = ctx.Definition.AttackRange * ctx.Definition.AttackRange;
            if ((ctx.Self.position - ctx.Player.position).sqrMagnitude <= sqrRange)
            {
                ctx.StateMachine.ChangeState(ctx.States.Attack, ctx);
                return;
            }

            return;
        }

        ctx.SearchTimer = ctx.Definition.SearchDuration;
        ctx.StateMachine.ChangeState(ctx.States.Search, ctx);
    }

    public void Exit(NPCContext ctx)
    {
        // Restore patrol-level tuning so other states aren't stuck with chase values.
        ctx.Agent.acceleration = ctx.Definition.Acceleration;
        ctx.Agent.angularSpeed = ctx.Definition.AngularSpeed;
    }
}