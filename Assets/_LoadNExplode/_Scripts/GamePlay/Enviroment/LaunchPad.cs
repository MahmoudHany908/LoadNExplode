using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Full-option 3D launch pad. Detects objects entering its trigger and launches them.
/// If the object implements ILaunchable, that implementation decides how to receive the
/// launch (recommended for players/enemies with custom movement controllers). Otherwise
/// falls back to directly manipulating a Rigidbody (fine for crates/physics props).
/// </summary>
[RequireComponent(typeof(Collider))]
public class LaunchPad : MonoBehaviour
{
    public enum DirectionMode
    {
        LocalUp,
        WorldDirection,
        TowardTarget,
        CustomCurveArc
    }

    [Header("Detection")]
    public LayerMask launchLayers = ~0;
    [Tooltip("If set, only objects with this tag are launched. Leave empty to ignore.")]
    public string requiredTag = "";
    [Tooltip("If no ILaunchable is found, require a Rigidbody to fall back to direct manipulation.")]
    public bool requireRigidbodyFallback = true;

    [Header("Direction")]
    public DirectionMode directionMode = DirectionMode.LocalUp;
    public Vector3 worldDirection = Vector3.up;
    public Transform aimTarget;
    public Transform arcTargetPoint;
    public float arcApexHeight = 5f;

    [Header("Force")]
    public LaunchApplyMode applyMode = LaunchApplyMode.VelocityChange;
    public float launchPower = 15f;
    [Range(0f, 1f)] public float retainHorizontalVelocity = 0f;
    public bool resetVerticalVelocityFirst = true;

    [Header("Timing")]
    public float perObjectCooldown = 0.5f;
    public float globalCooldown = 0f;
    public float launchDelay = 0f;

    [Header("Effects & Events")]
    public ParticleSystem launchParticles;
    public AudioSource launchAudio;
    public UnityEvent<Component> onLaunch; // Rigidbody or the ILaunchable's MonoBehaviour

    [Header("Gizmos")]
    public bool drawGizmos = true;
    public Color gizmoColor = new Color(0f, 1f, 1f, 0.6f);

    private readonly Dictionary<Component, float> _lastLaunchTime = new Dictionary<Component, float>();
    private float _lastGlobalLaunchTime = -999f;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryLaunch(other);
    }

    private bool CanLaunch(Component key)
    {
        if (Time.time - _lastGlobalLaunchTime < globalCooldown) return false;
        if (_lastLaunchTime.TryGetValue(key, out float last) && Time.time - last < perObjectCooldown) return false;
        return true;
    }

    private void TryLaunch(Collider other)
    {
        if (((1 << other.gameObject.layer) & launchLayers) == 0) return;
        if (!string.IsNullOrEmpty(requiredTag) && !other.CompareTag(requiredTag)) return;

        ILaunchable launchable = other.GetComponentInParent<ILaunchable>();
        Rigidbody rb = other.attachedRigidbody;

        Component key = launchable as Component ?? rb;
        if (key == null) return;
        if (launchable == null && (rb == null || !requireRigidbodyFallback)) return;
        if (!CanLaunch(key)) return;

        _lastLaunchTime[key] = Time.time;
        _lastGlobalLaunchTime = Time.time;

        Vector3 originPos = launchable != null ? ((Component)launchable).transform.position : rb.position;

        if (launchDelay <= 0f)
            DoLaunch(launchable, rb, originPos);
        else
            StartCoroutine(LaunchAfterDelay(launchable, rb, originPos, launchDelay));
    }

    private IEnumerator LaunchAfterDelay(ILaunchable launchable, Rigidbody rb, Vector3 originPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        DoLaunch(launchable, rb, originPos);
    }

    private void DoLaunch(ILaunchable launchable, Rigidbody rb, Vector3 originPos)
    {
        Vector3 finalVelocity = ComputeVelocity(originPos);

        if (launchable != null)
        {
            Debug.Log("launchable.apply lach");
            launchable.Launch(finalVelocity, applyMode, this);
        }
        else if (rb != null)
        {
            Vector3 currentVel = rb.linearVelocity;
            Vector3 horizontal = Vector3.ProjectOnPlane(currentVel, Vector3.up) * retainHorizontalVelocity;
            Vector3 vertical = resetVerticalVelocityFirst ? Vector3.zero : Vector3.Project(currentVel, Vector3.up);

            switch (applyMode)
            {
                case LaunchApplyMode.SetVelocityDirect:
                    rb.linearVelocity = horizontal + vertical + finalVelocity;
                    break;
                case LaunchApplyMode.VelocityChange:
                    rb.linearVelocity = horizontal + vertical;
                    rb.AddForce(finalVelocity, ForceMode.VelocityChange);
                    break;
                case LaunchApplyMode.Impulse:
                    rb.linearVelocity = horizontal + vertical;
                    rb.AddForce(finalVelocity, ForceMode.Impulse);
                    break;
            }
        }

        if (launchParticles != null) launchParticles.Play();
        if (launchAudio != null) launchAudio.Play();
        onLaunch?.Invoke(launchable != null ? (Component)launchable : rb);
    }

    private Vector3 ComputeVelocity(Vector3 fromPosition)
    {
        if (directionMode == DirectionMode.CustomCurveArc && arcTargetPoint != null)
            return SolveArcVelocity(fromPosition, arcTargetPoint.position, arcApexHeight);

        return GetDirection(fromPosition).normalized * launchPower;
    }

    private Vector3 GetDirection(Vector3 fromPosition)
    {
        switch (directionMode)
        {
            case DirectionMode.LocalUp:
                return transform.up;
            case DirectionMode.WorldDirection:
                return worldDirection.sqrMagnitude > 0.0001f ? worldDirection : Vector3.up;
            case DirectionMode.TowardTarget:
                return aimTarget != null ? (aimTarget.position - fromPosition) : transform.up;
            default:
                return transform.up;
        }
    }

    private Vector3 SolveArcVelocity(Vector3 start, Vector3 end, float apexHeight)
    {
        float gravity = Mathf.Abs(Physics.gravity.y);
        if (gravity <= 0.0001f) gravity = 9.81f;

        float displacementY = end.y - start.y;
        Vector3 displacementXZ = new Vector3(end.x - start.x, 0f, end.z - start.z);

        float apexAboveStart = Mathf.Max(apexHeight, displacementY + 0.1f);
        float velocityY = Mathf.Sqrt(2f * gravity * apexAboveStart);
        float timeUp = velocityY / gravity;
        float timeDown = Mathf.Sqrt(Mathf.Max(0f, 2f * (apexAboveStart - displacementY) / gravity));
        float totalTime = timeUp + timeDown;

        if (totalTime <= 0.0001f) return transform.up * launchPower;

        Vector3 velocityXZ = displacementXZ / totalTime;
        return velocityXZ + Vector3.up * velocityY;
    }

    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = gizmoColor;

        Vector3 origin = transform.position;
        Vector3 dir = GetDirection(origin);
        Gizmos.DrawLine(origin, origin + dir.normalized * 3f);
        Gizmos.DrawSphere(origin + dir.normalized * 3f, 0.15f);

        if (directionMode == DirectionMode.CustomCurveArc && arcTargetPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origin, arcTargetPoint.position);
            Gizmos.DrawWireSphere(arcTargetPoint.position, 0.3f);
        }
    }
}