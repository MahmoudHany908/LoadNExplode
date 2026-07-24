using UnityEngine;

[CreateAssetMenu(fileName = "ForwardPushSpawn", menuName = "Shop/Spawn/Forward Push")]
public class ForwardPushSpawnBehavior : ItemSpawnBehavior
{
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float pushForce = 6f;
    [SerializeField] private float spawnHeightOffset = 0.5f;

    public override GameObject Spawn(GameObject prefab, in ItemUseContext context)
    {
        Vector3 spawnPos = context.PlayerPosition
                           + context.AimDirection * spawnDistance
                           + Vector3.up * spawnHeightOffset;

        Quaternion rotation = Quaternion.LookRotation(context.AimDirection, Vector3.up);
        GameObject instance = Object.Instantiate(prefab, spawnPos, rotation);

        if (instance.TryGetComponent(out Rigidbody rb))
        {
            rb.AddForce(context.AimDirection * pushForce, ForceMode.VelocityChange);
        }

        return instance;
    }
}
