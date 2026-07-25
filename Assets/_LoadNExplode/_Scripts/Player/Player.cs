using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerVisuals))]
public class Player : UnitBase
{
    [Header("Configuration")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject deathEffects;

    [Header("Component References")]
    private PlayerMovement playerMovement;
    private PlayerVisuals playerVisuals;
    private Bomb bomb;
    private CapsuleCollider _collider;
    private float timeTakesToExplode = 10;
    private int _invulnerabilityCount;

    public int Health { get; private set; }
    public bool IsInvulnerable => _invulnerabilityCount > 0;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerVisuals = GetComponent<PlayerVisuals>();
        _collider = GetComponent<CapsuleCollider>();
        bomb = GetComponentInChildren<Bomb>();
    }

    private void Start()
    {
        Health = maxHealth;

        if (bomb != null)
        {
            bomb.TriggerExplosion(timeTakesToExplode);
        }
    }

    public void AddInvulnerability()
    {
        _invulnerabilityCount++;
    }

    public void RemoveInvulnerability()
    {
        _invulnerabilityCount = Mathf.Max(0, _invulnerabilityCount - 1);
    }


    public override void TakeDown()
    {
        if (IsInvulnerable)
            return;

        if (bomb != null)
        {
            bomb.Defuse();
        }
        _collider.enabled = false;
        GetPlayerMovement().rb.isKinematic = true;
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        EventBus.Publish(new PlayerWasTakenDownEvent(player: this));
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (IsInvulnerable || Health <= 0)
            return;

        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Die();
        }
    }

    public override void Die()
    {
        EventBus.Publish(new PlayerDeathEvent(this, transform.position));

        if (bomb != null)
        {
            bomb.Defuse(" ");
        }


        _collider.enabled = false;
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (playerVisuals != null)
        {
            playerVisuals.PlayerVisualTransform.gameObject.SetActive(false);
        }

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (deathEffects == null) return;

        GameObject deathEffectInstance = Instantiate(deathEffects, transform.position, Quaternion.identity);
        Destroy(deathEffectInstance, 3f);
    }

    public void Respawn(Vector3 spawnPosition)
    {
        Health = maxHealth;
        _invulnerabilityCount = 0;
        transform.position = spawnPosition;

        // Re-enable movement and visuals

        StartCoroutine(ToggleComponents());

        // Reset physics velocity
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }



        // Restart the bomb countdown on respawn
        if (bomb != null)
        {
            bomb.TriggerExplosion(timeTakesToExplode);
        }
    }
    private IEnumerator ToggleComponents()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
        GetPlayerMovement().rb.isKinematic = false;
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            playerMovement.SetSpeedMultiplier(1f);
        }

        if (playerVisuals != null)
        {
            playerVisuals.PlayerVisualTransform.gameObject.SetActive(true);
        }

    }
    #region Component Accessors

    public PlayerMovement GetPlayerMovement() => playerMovement;
    public PlayerVisuals GetPlayerVisuals() => playerVisuals;
    public Bomb GetBomb() => bomb;

    #endregion
}