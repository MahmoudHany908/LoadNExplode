using UnityEngine;

public readonly struct ItemUseContext
{
    public readonly Player Player;
    public readonly Vector3 PlayerPosition;
    public readonly Vector3 AimDirection;

    public ItemUseContext(Player player, Vector3 playerPosition, Vector3 aimDirection)
    {
        Player = player;
        PlayerPosition = playerPosition;
        AimDirection = aimDirection.sqrMagnitude > 0.0001f ? aimDirection.normalized : Vector3.forward;
    }
}
