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

    private float timeTakesToExplode = 10;
    public int Health { get; private set; }

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerVisuals = GetComponent<PlayerVisuals>();
        bomb = GetComponentInChildren<Bomb>();
    }

    private void Start()
    {
        bomb.TriggerExplosion(timeTakesToExplode);
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

        GetPlayerMovement().enabled = false;
        GetPlayerVisuals().PlayerVisualTransform.gameObject.SetActive(false);
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;



        if (deathEffects == null) return;
        GameObject deathEffectInstance = Instantiate(deathEffects, transform.position, Quaternion.identity);
        Destroy(deathEffectInstance, 3f);
    }

    public void Respawn(Vector3 spawnPosition)
    {
        Health = maxHealth;
        transform.position = spawnPosition;

        GetPlayerMovement().enabled = true;
        GetPlayerVisuals().PlayerVisualTransform.gameObject.SetActive(true);

        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }
        bomb.TriggerExplosion(timeTakesToExplode);
    }

    #region Component Accessors

    public PlayerMovement GetPlayerMovement() => playerMovement;
    public PlayerVisuals GetPlayerVisuals() => playerVisuals;
    public Bomb GetBomb() => bomb;

    #endregion
}