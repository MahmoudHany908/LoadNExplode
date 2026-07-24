using UnityEngine;

public class StunnedState : INPCState
{
    private float _stunDuration;
    private float _stunTimer;

    public StunnedState(float duration = 2f)
    {
        _stunDuration = duration;
    }

    public void SetDuration(float duration)
    {
        _stunDuration = duration;
    }

    public void Enter(NPCContext ctx)
    {
        if (ctx.Agent != null)
        {
            ctx.Agent.isStopped = true;
            ctx.Agent.ResetPath();
        }
        _stunTimer = _stunDuration;
    }

    public void Tick(NPCContext ctx)
    {
        _stunTimer -= Time.deltaTime;

        if (_stunTimer <= 0f)
        {
            if (ctx.Definition.Behavior == NPCBehaviorType.Hostile)
            {
                ctx.StateMachine.ChangeState(ctx.States.Chase, ctx);
            }
            else
            {
                ctx.StateMachine.ChangeState(ctx.States.Patrol, ctx);
            }
        }
    }

    public void Exit(NPCContext ctx)
    {
        if (ctx.Agent != null)
        {
            ctx.Agent.isStopped = false;
        }
    }
}
