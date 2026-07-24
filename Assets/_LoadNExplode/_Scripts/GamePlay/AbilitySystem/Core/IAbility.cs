
public interface IAbility
{
    public void Activate(); // called when the ability is activated
    public void Started(); // called when the ability is started
    public void Tick(float _deltaTime);
}

