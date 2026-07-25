using System.Collections;
using UnityEngine;

/// <summary>
/// Heroin / speed boost: raises player move speed for a limited time.
/// </summary>
public class HeroinItem : MonoBehaviour, IItem, IPlayerReceivable
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float speedMultiplier = 1.75f;

    private Player _player;
    private PlayerMovement _movement;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    // Called once when the item is spawned, mirrors IAbility.Started()
    public void Started()
    {
        if (_player == null)
            _player = GetComponentInParent<Player>();

        if (_player != null)
            _movement = _player.GetPlayerMovement();
    }

    // The boost is applied immediately on use, mirrors IAbility.Activate()
    public void Activate()
    {
        if (_movement != null)
            _movement.SetSpeedMultiplier(speedMultiplier);

        StartCoroutine(ExpireCoroutine());
    }

    public void Tick(float _deltaTime)
    {
    }

    private IEnumerator ExpireCoroutine()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_movement != null)
            _movement.SetSpeedMultiplier(1f);
    }
}