public interface IItem
{
    public void Activate(); // called when the item is used
    public void Started(); // called when the item is spawned/picked up
    public void Tick(float _deltaTime);
}