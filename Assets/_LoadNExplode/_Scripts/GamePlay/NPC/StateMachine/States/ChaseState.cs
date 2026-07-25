public class ChaseState : INPCState
{
    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.ChaseSpeed;
        ctx.Agent.isStopped = false;

        ctx.LastKnownPlayerPosition = ctx.Player.position;
        ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);
    }

    public void Tick(NPCContext ctx)
    {
        if (ctx.CanSeePlayer)
        {
            ctx.LastKnownPlayerPosition = ctx.Player.position;
            ctx.Agent.SetDestination(ctx.LastKnownPlayerPosition);



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

    public void Exit(NPCContext ctx) { }
}