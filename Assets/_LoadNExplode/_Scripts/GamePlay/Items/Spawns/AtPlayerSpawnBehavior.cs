using UnityEngine;

[CreateAssetMenu(fileName = "AtPlayerSpawn", menuName = "Shop/Spawn/At Player")]
public class AtPlayerSpawnBehavior : ItemSpawnBehavior
{
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private bool parentToPlayer = true;

    public override GameObject Spawn(GameObject prefab, in ItemUseContext context)
    {
        Transform parent = parentToPlayer && context.Player != null
            ? context.Player.transform
            : null;

        return Object.Instantiate(
            prefab,
            context.PlayerPosition + offset,
            Quaternion.identity,
            parent);
    }
}
