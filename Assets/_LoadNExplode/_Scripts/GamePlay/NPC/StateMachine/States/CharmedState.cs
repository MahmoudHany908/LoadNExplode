using UnityEngine;

public class CharmedState : INPCState
{
    private float _charmedDuration;
    private float _charmedTimer;
    private Transform _target;

    public CharmedState(float duration = 5f)
    {
        _charmedDuration = duration;
    }

    public void SetDuration(float duration, Transform target)
    {
        _charmedDuration = duration;
        _target = target;
    }

    public void Enter(NPCContext ctx)
    {
        if (ctx.Agent != null && ctx.Agent.isOnNavMesh)
        {
            ctx.Agent.isStopped = false;
            ctx.Agent.speed = ctx.Definition.CharmedMovementSpeed;
            _charmedTimer = _charmedDuration;
        }

    }

    public void Tick(NPCContext ctx)
    {
        if (_target != null)
        {
            ctx.Agent.SetDestination(_target.position);
        }

        _charmedTimer -= Time.deltaTime;

        if (_charmedTimer <= 0f)
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
        _target = null;
        ctx.Agent.ResetPath();
    }
}