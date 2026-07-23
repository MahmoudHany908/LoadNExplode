using UnityEngine;

public class VisionSensor
{
    private readonly Transform _eye;
    private readonly float _range;
    private readonly float _fovAngle;
    private readonly LayerMask _targetMask;
    private readonly LayerMask _obstacleMask;

    public VisionSensor(Transform eye, float range, float fovAngle, LayerMask targetMask, LayerMask obstacleMask)
    {
        _eye = eye;
        _range = range;
        _fovAngle = fovAngle;
        _targetMask = targetMask;
        _obstacleMask = obstacleMask;
    }

    public bool CanSee(Transform target)
    {
        if (target == null) return false;

        Vector3 dir = target.position - _eye.position;
        float dist = dir.magnitude;
        if (dist > _range) return false;

        float angle = Vector3.Angle(_eye.forward, dir);
        if (angle > _fovAngle * 0.5f) return false;

        if (Physics.Raycast(_eye.position, dir.normalized, out var hit, dist, _obstacleMask))
        {
            return hit.transform == target || hit.transform.IsChildOf(target);
        }

        return true;
    }
}