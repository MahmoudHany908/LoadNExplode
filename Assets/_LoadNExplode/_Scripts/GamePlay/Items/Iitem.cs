public interface IItem
{
    public void Activate(); // called when the item is used
    public void Started();
    public void Tick(float deltaTime);
}