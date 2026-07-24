using UnityEngine;

/// <summary>
/// Spawn placement only. Item gameplay logic lives on the prefab (MineItem, ShieldItem, etc.).
/// </summary>
public interface IItemSpawnBehavior
{
    GameObject Spawn(GameObject prefab, in ItemUseContext context);
}
