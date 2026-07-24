

public struct OnCountdownFinishedEvent : IGameEvent
{

}
public struct OnCountdownStartedEvent : IGameEvent
{
    public readonly GamePlaySceneCountdown countdown;
    public OnCountdownStartedEvent(GamePlaySceneCountdown countdown)
    {
        this.countdown = countdown;
    }
}