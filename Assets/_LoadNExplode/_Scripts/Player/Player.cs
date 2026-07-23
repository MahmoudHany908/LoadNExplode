using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerVisuals))]
public class Player : UnitBase
{
    [Header("Configuration")]
    [SerializeField] private int maxHealth = 100;

    [Header("Component References")]
    private PlayerMovement playerMovement;
    private PlayerVisuals playerVisuals;
    private Bomb bomb;


    public int Health { get; private set; }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerVisuals = GetComponent<PlayerVisuals>();
        bomb = GetComponentInChildren<Bomb>();
    }

    private void Start()
    {

        Health = maxHealth;
    }

    public override void TakeDamage(int damage)
    {

        if (Health <= 0) return;

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
    }

    public void Respawn(Vector3 spawnPosition)
    {
        Health = maxHealth;
        transform.position = spawnPosition;

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }
        bomb.TriggerExplosion();
    }

    #region Component Accessors

    public PlayerMovement GetPlayerMovement() => playerMovement;
    public PlayerVisuals GetPlayerVisuals() => playerVisuals;
    public Bomb GetBomb() => bomb;

    #endregion
}