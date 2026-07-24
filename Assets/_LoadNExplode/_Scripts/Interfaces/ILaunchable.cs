using UnityEngine;

public enum LaunchApplyMode
{
    Impulse,
    VelocityChange,
    SetVelocityDirect
}


public interface ILaunchable
{
    void Launch(Vector3 velocity, LaunchApplyMode mode, LaunchPad source);
}