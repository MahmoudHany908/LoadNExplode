using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BoomyJump : MonoBehaviour, IAbility
{
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float upwardForce = 6f;
    [SerializeField] private LaunchApplyMode launchMode = LaunchApplyMode.VelocityChange;

    [Header("Aim Settings")]
    [SerializeField] private LayerMask aimLayerMask;
    [SerializeField] private float maxAimDistance = 100f;

    [Header("Cooldown")]
    [SerializeField] private float cooldownTime = 3f;
    [SerializeField] private Image cooldownImage;

    private float currentCooldown;
    private ILaunchable launchable;
    private Transform selfTransform;

    public void Started()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BoomyJump could not find a GameObject with tag 'Player'.");
            return;
        }

        selfTransform = player.transform;
        launchable = player.GetComponent<ILaunchable>();

        if (launchable == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BoomyJump could not find an ILaunchable component on the Player.");
        }

        currentCooldown = 0f;
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 0f;
        }
    }

    public void Activate()
    {
        if (currentCooldown > 0f)
        {
            Debug.LogWarning($"[{gameObject.name}] BoomyJump is on cooldown ({currentCooldown:F1}s remaining).");
            return;
        }

        if (launchable == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BoomyJump cannot activate — no ILaunchable found.");
            return;
        }

        Vector3 aimDirection = GetMouseAimDirection(selfTransform.position);
        Vector3 launchVelocity = (aimDirection * jumpForce) + (Vector3.up * upwardForce);

        launchable.Launch(launchVelocity, launchMode, null);

        Debug.Log($"[{gameObject.name}] BoomyJump launched toward {aimDirection} with velocity {launchVelocity}.");

        currentCooldown = cooldownTime;
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = 1f;
        }
    }

    public void Tick(float _deltaTime)
    {
        if (currentCooldown <= 0f)
        {
            return;
        }

        currentCooldown -= _deltaTime;
        if (currentCooldown < 0f)
        {
            currentCooldown = 0f;
        }

        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = currentCooldown / cooldownTime;
        }
    }

    private Vector3 GetMouseAimDirection(Vector3 origin)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning($"[{gameObject.name}] BoomyJump found no Camera.main — falling back to forward.");
            return selfTransform.forward;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);

        // Prefer hitting real geometry if aimLayerMask is configured.
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimLayerMask))
        {
            Vector3 dir = hit.point - origin;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                return dir.normalized;
            }
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, origin.y, 0f));
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - origin;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                return dir.normalized;
            }
        }

        Debug.LogWarning($"[{gameObject.name}] BoomyJump mouse aim resolved to no direction — falling back to forward.");
        return selfTransform.forward;
    }
}