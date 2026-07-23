using UnityEngine;

public class SearchState : INPCState
{
    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.PatrolSpeed;
        ctx.Agent.isStopped = false;
        ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);
    }

    public void Tick(NPCContext ctx)
    {
        if (ctx.CanSeePlayer)
        {
            if (ctx.Definition.Behavior == NPCBehaviorType.Hostile)
                ctx.StateMachine.ChangeState(ctx.States.Chase, ctx);
            else
                ctx.StateMachine.ChangeState(ctx.States.Flee, ctx);
            return;
        }

        ctx.SearchTimer -= Time.deltaTime;

        bool reachedSpot = !ctx.Agent.pathPending && ctx.Agent.remainingDistance < 0.5f;
        if (reachedSpot && ctx.SearchTimer <= 0f)
            ctx.StateMachine.ChangeState(ctx.States.Patrol, ctx);
    }

    public void Exit(NPCContext ctx) { }
}