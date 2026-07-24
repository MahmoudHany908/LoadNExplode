using System.Collections;
using UnityEngine;

/// <summary>
/// Heroin / speed boost: raises player move speed for a limited time.
/// </summary>
public class HeroinItem : MonoBehaviour
{
    [SerializeField] private float duration = 5f;
    [SerializeField] private float speedMultiplier = 1.75f;

    private PlayerMovement _movement;

    private void Start()
    {
        Player player = GetComponentInParent<Player>();
        if (player == null)
            player = FindFirstObjectByType<Player>();

        if (player != null)
            _movement = player.GetPlayerMovement();

        if (_movement != null)
            _movement.SetSpeedMultiplier(speedMultiplier);

        StartCoroutine(ExpireCoroutine());
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
