public interface INPCState
{
    void Enter(NPCContext ctx);
    void Tick(NPCContext ctx);
    void Exit(NPCContext ctx);
}

public class NPCStateMachine
{
    private INPCState _current;
    public INPCState Current => _current;

    public void ChangeState(INPCState next, NPCContext ctx)
    {
        _current?.Exit(ctx);
        _current = next;
        _current?.Enter(ctx);
    }

    public void Tick(NPCContext ctx) => _current?.Tick(ctx);
}