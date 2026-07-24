using UnityEngine;

[CreateAssetMenu(fileName = "AttachToPlayerSpawn", menuName = "Shop/Spawn/Attach To Player")]
public class AttachToPlayerSpawnBehavior : ItemSpawnBehavior
{
    [SerializeField] private Vector3 localOffset = new Vector3(0f, 1.2f, 0f);

    public override GameObject Spawn(GameObject prefab, in ItemUseContext context)
    {
        Transform parent = context.Player != null ? context.Player.transform : null;
        GameObject instance = Object.Instantiate(prefab, context.PlayerPosition + localOffset, Quaternion.identity, parent);

        if (parent != null)
            instance.transform.localPosition = localOffset;

        return instance;
    }
}
