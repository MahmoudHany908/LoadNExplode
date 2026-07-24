public class PatrolState : INPCState
{
    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.PatrolSpeed;
        ctx.Agent.isStopped = false;
        ctx.Agent.SetDestination(ctx.NextPatrolPoint());
    }

    public void Tick(NPCContext ctx)
    {
        if (!ctx.Agent.pathPending && ctx.Agent.remainingDistance < 0.5f)
            ctx.Agent.SetDestination(ctx.NextPatrolPoint());

        if (ctx.CanSeePlayer)
            ctx.StateMachine.ChangeState(ctx.States.Flee, ctx);
    }
    public void Exit(NPCContext ctx) { }
}