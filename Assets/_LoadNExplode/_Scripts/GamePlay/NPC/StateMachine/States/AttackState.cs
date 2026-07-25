using UnityEngine;

public class AttackState : INPCState
{
    public void Enter(NPCContext ctx)
    {
        ctx.Agent.isStopped = true;
        ctx.AttackCooldownTimer = 0f; // allow an immediate first swing
    }

    public void Tick(NPCContext ctx)
    {
        // face the player
        Vector3 dir = ctx.Player.position - ctx.Self.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
            ctx.Self.rotation = Quaternion.Slerp(ctx.Self.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

        ctx.AttackCooldownTimer -= Time.deltaTime;
        if (ctx.AttackCooldownTimer <= 0f)
        {
            TryAttack(ctx);
            ctx.AttackCooldownTimer = ctx.Definition.AttackCooldown;
        }

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

    private void TryAttack(NPCContext ctx)
    {
        Vector3 origin = (ctx.Eye != null ? ctx.Eye.position : ctx.Self.position + Vector3.up);
        Vector3 dir = ctx.Self.forward;

        if (Physics.SphereCast(origin, ctx.Definition.AttackSphereRadius, dir,
                out RaycastHit hit, ctx.Definition.AttackRange, ctx.Definition.TargetMask))
        {
            var damageable = hit.collider.GetComponentInParent<IDamageable>();
            damageable?.TakeDown();
        }
    }
}