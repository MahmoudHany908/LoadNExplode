public class NPCStates
{
    public readonly PatrolState Patrol = new PatrolState();
    public readonly ChaseState Chase = new ChaseState();
    public readonly FleeState Flee = new FleeState();
    public readonly SearchState Search = new SearchState();
    public readonly AttackState Attack = new AttackState();
    public readonly CharmedState Charmed = new CharmedState();
    public readonly StunnedState Stunned = new StunnedState();
}