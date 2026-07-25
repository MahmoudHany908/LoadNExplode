using System.Collections;
using UnityEngine;

/// <summary>
/// Shield logic: follows player and blocks damage for a limited time.
/// </summary>
public class ShieldItem : MonoBehaviour, IItem, IPlayerReceivable
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 1.2f, 0f);

    private Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    // Called once when the item is spawned, mirrors IAbility.Started()
    public void Started()
    {
        if (_player == null)
            _player = GetComponentInParent<Player>();
    }

    // The shield turns on immediately on use, mirrors IAbility.Activate()
    public void Activate()
    {
        if (_player != null)
            _player.AddInvulnerability();

        StartCoroutine(ExpireCoroutine());
    }

    public void Tick(float _deltaTime)
    {
        if (_player == null)
            return;

        // Keep following even if not parented.
        if (transform.parent != _player.transform)
            transform.position = _player.transform.position + followOffset;
    }

    private IEnumerator ExpireCoroutine()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_player != null)
            _player.RemoveInvulnerability();
    }
}