using UnityEngine;
using UnityEngine.InputSystem;

public class StunGrenadeItem : MonoBehaviour, IItem, IPlayerReceivable
{
    [Header("Grenade Settings")]
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private float throwForce = 12f;
    [SerializeField] private float upwardForce = 2f;
    [SerializeField] private Transform throwPoint; // optional - defaults to player position/forward if null
    [SerializeField] private LayerMask aimLayerMask; // optional - what the mouse raycast can hit (ground/environment)
    [SerializeField] private float maxAimDistance = 100f;

    private Player player;
    private Transform playerTransform;

    public void SetPlayer(Player player)
    {
        this.player = player;
        this.playerTransform = player != null ? player.transform : null;

        if (this.playerTransform == null)
        {
            Debug.LogWarning($"[{gameObject.name}] StunGrenadeItem received a null player reference.");
        }
    }

    public void Activate()
    {
        if (grenadePrefab == null)
        {
            Debug.LogWarning($"[{gameObject.name}] StunGrenadeItem has no grenadePrefab assigned.");
            Destroy(gameObject);
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogWarning($"[{gameObject.name}] StunGrenadeItem cannot activate — player reference is null.");
            Destroy(gameObject);
            return;
        }

        Vector3 spawnPos = throwPoint != null ? throwPoint.position : playerTransform.position + playerTransform.forward + Vector3.up;
        Vector3 aimDirection = GetMouseAimDirection(spawnPos);
        Quaternion spawnRot = Quaternion.LookRotation(aimDirection, Vector3.up);

        GameObject grenadeInstance = Instantiate(grenadePrefab, spawnPos, spawnRot);

        Rigidbody rb = grenadeInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 throwDir = (aimDirection * throwForce) + (Vector3.up * upwardForce);
            rb.AddForce(throwDir, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Grenade prefab has no Rigidbody — it won't be thrown, just dropped.");
        }

        Debug.Log($"[{gameObject.name}] StunGrenadeItem thrown by {playerTransform.name}.");

        Destroy(gameObject);
    }

    public void Started()
    {
    }

    private Vector3 GetMouseAimDirection(Vector3 spawnPos)
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning($"[{gameObject.name}] StunGrenadeItem found no Camera.main — falling back to player forward.");
            return playerTransform.forward;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimLayerMask))
        {
            Vector3 dir = hit.point - spawnPos;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                return dir.normalized;
            }
        }

        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, playerTransform.position.y, 0f));
        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 dir = hitPoint - spawnPos;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.0001f)
            {
                return dir.normalized;
            }
        }

        Debug.LogWarning($"[{gameObject.name}] StunGrenadeItem mouse aim resolved to no direction — falling back to player forward.");
        return playerTransform.forward;
    }

    public void Tick(float deltaTime)
    {
    }
}