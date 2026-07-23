using UnityEngine;

public class AttackState : INPCState
{
    public void Enter(NPCContext ctx)
    {
        ctx.Agent.isStopped = true;
    }

    public void Tick(NPCContext ctx)
    {
        // face the player
        Vector3 dir = ctx.Player.position - ctx.Self.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            ctx.Self.rotation = Quaternion.Slerp(ctx.Self.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        // TODO: trigger your attack ability here (e.g. hook into your RookAbility-style system)

        float dist = Vector3.Distance(ctx.Self.position, ctx.Player.position);
        if (dist > ctx.Definition.AttackRange * 1.2f) // small buffer to avoid state flicker
        {
            ctx.StateMachine.ChangeState(ctx.States.Chase, ctx);
            return;
        }

        if (!ctx.CanSeePlayer)
        {
            ctx.LastKnownPlayerPosition = ctx.Player.position;
            ctx.SearchTimer = ctx.Definition.SearchDuration;
            ctx.StateMachine.ChangeState(ctx.States.Search, ctx);
        }
    }

    public void Exit(NPCContext ctx)
    {
        ctx.Agent.isStopped = false;
    }
}