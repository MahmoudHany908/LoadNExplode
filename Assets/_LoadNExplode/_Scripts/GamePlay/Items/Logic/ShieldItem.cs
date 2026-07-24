using System.Collections;
using UnityEngine;

/// <summary>
/// Shield logic: follows player and blocks damage for a limited time.
/// </summary>
public class ShieldItem : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private Vector3 followOffset = new Vector3(0f, 1.2f, 0f);

    private Player _player;

    private void Start()
    {
        _player = GetComponentInParent<Player>();
        if (_player == null)
            _player = FindFirstObjectByType<Player>();

        if (_player != null)
            _player.AddInvulnerability();

        StartCoroutine(ExpireCoroutine());
    }

    private void LateUpdate()
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
