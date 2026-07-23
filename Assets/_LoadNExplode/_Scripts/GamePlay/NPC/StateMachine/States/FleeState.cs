using UnityEngine;
using UnityEngine.AI;

public class FleeState : INPCState
{
    private const float RepathInterval = 0.3f;
    private float _timer;

    public void Enter(NPCContext ctx)
    {
        ctx.Agent.speed = ctx.Definition.FleeSpeed;
        ctx.Agent.isStopped = false;
        _timer = 0f;
        SetFleeDestination(ctx);
    }

    public void Tick(NPCContext ctx)
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SetFleeDestination(ctx);
            _timer = RepathInterval;
        }

        float distToPlayer = Vector3.Distance(ctx.Self.position, ctx.Player.position);
        if (distToPlayer > ctx.Definition.SafeDistance && !ctx.CanSeePlayer)
            ctx.StateMachine.ChangeState(ctx.States.Search, ctx);
    }

    public void Exit(NPCContext ctx) { }

    private void SetFleeDestination(NPCContext ctx)
    {
        Vector3 awayDir = (ctx.Self.position - ctx.Player.position).normalized;
        Vector3 desired = ctx.Self.position + awayDir * ctx.Definition.FleeDistance;

        if (NavMesh.SamplePosition(desired, out var hit, ctx.Definition.FleeDistance, NavMesh.AllAreas))
            ctx.Agent.SetDestination(hit.position);
        else
            ctx.Agent.SetDestination(ctx.Self.position - awayDir * 2f); // small fallback step
    }
}