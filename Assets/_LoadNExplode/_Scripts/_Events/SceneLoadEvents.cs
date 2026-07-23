using UnityEngine;


public struct RequestSceneLoadEvent : IGameEvent
{
    public readonly string SceneName;

    public RequestSceneLoadEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}


public struct SceneLoadStartEvent : IGameEvent
{
    public readonly string SceneName;

    public SceneLoadStartEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}

public struct SceneLoadCompleteEvent : IGameEvent
{
    public readonly string SceneName;

    public SceneLoadCompleteEvent(string sceneName)
    {
        SceneName = sceneName;
    }
}