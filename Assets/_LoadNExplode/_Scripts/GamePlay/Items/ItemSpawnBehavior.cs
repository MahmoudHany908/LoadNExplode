using UnityEngine;

public abstract class ItemSpawnBehavior : ScriptableObject, IItemSpawnBehavior
{
    public abstract GameObject Spawn(GameObject prefab, in ItemUseContext context);
}
