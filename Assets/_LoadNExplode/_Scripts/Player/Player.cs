using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerVisuals))]
public class Player : UnitBase
{
    [SerializeField] private GameObject Bomb;
    [SerializeField] private int maxHealth = 100;
    private PlayerMovement playerMovement;

    private PlayerVisuals playerVisuals;

    private int Health = 100;
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerVisuals = GetComponent<PlayerVisuals>();
        Health = maxHealth;

    }

    public override void TakeDamage(int damage)
    {
        if (Health >= 0)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Die();
            }

        }
        else if (Health <= 0)
        {
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
            rb.linearVelocity = Vector3.zero;
    }


    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Instantiate(Bomb, transform.position, Quaternion.identity, transform);
        }
    }


    public PlayerMovement GetPlayerMovement() => playerMovement;
    public PlayerVisuals GetPlayerVisuals() => playerVisuals;


}
