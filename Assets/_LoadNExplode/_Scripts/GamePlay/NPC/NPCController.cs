using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Rigidbody))]
public class NPCController : MonoBehaviour, ILaunchable
{
    [SerializeField] private NPCDefinition definition;
    [SerializeField] private Transform eye;                // vision origin - defaults to this transform
    [SerializeField] private Transform player;              // auto-found via "Player" tag if left empty

    private NPCContext _context;
    private NPCStateMachine _stateMachine;
    private VisionSensor _vision;
    private float _visionTimer;

    [Header("Launch")]
    private Rigidbody rb;
    [SerializeField] private LayerMask groundMask;
    private bool _isLaunched;

    private void Awake()
    {
        var agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Apply movement tuning from definition so turns and stops feel natural.
        agent.angularSpeed = definition.AngularSpeed;
        agent.acceleration = definition.Acceleration;
        agent.stoppingDistance = definition.StoppingDistance;

        if (eye == null) eye = transform;
        if (player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        _vision = new VisionSensor(eye, definition.VisionRange, definition.VisionFOVAngle,
            definition.TargetMask, definition.ObstacleMask);

        _stateMachine = new NPCStateMachine();

        _context = new NPCContext
        {
            Agent = agent,
            Self = transform,
            Eye = eye,
            Player = player,
            Definition = definition,
            StateMachine = _stateMachine,
            States = new NPCStates(),
            SpawnPosition = transform.position
        };
    }

    private void Start()
    {
        _stateMachine.ChangeState(_context.States.Patrol, _context);
    }
    public void SetCharm(float duration = 5f, Transform target = null)
    {
        _context.States.Charmed.SetDuration(duration, target);
        _stateMachine.ChangeState(_context.States.Charmed, _context);
    }

    public void Stun(float time = 2f)
    {
        _context.States.Stunned.SetDuration(time);
        _stateMachine.ChangeState(_context.States.Stunned, _context);
    }


    #region launch
    public void Launch(Vector3 velocity, LaunchApplyMode mode, LaunchPad source)
    {
        if (rb == null) return;
        StartCoroutine(LaunchRoutine(velocity, mode));
        Debug.Log("Launch");
    }

    private IEnumerator LaunchRoutine(Vector3 velocity, LaunchApplyMode mode)
    {
        _isLaunched = true;
        var agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        rb.isKinematic = false;

        switch (mode)
        {
            case LaunchApplyMode.SetVelocityDirect:
                rb.linearVelocity = velocity;
                break;
            case LaunchApplyMode.VelocityChange:
                rb.AddForce(velocity, ForceMode.VelocityChange);
                break;
            case LaunchApplyMode.Impulse:
                rb.AddForce(velocity, ForceMode.Impulse);
                break;
        }

        // Require the NPC to actually leave the ground before we start checking for landing,
        // so we don't instantly snap back on the same frame we launched.
        float minAirTime = 0.2f;
        float maxAirTime = 5f; // safety net so it can never get stuck airborne forever
        float timer = 0f;

        while (timer < minAirTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        while (!Physics.Raycast(transform.position, Vector3.down, 1.3f, groundMask) && timer < maxAirTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        rb.isKinematic = true;
        agent.enabled = true;
        agent.Warp(transform.position);
        _isLaunched = false;
    }
    #endregion

    private void Update()
    {
        if (_isLaunched) return;

        _visionTimer -= Time.deltaTime;
        if (_visionTimer <= 0f)
        {
            _context.CanSeePlayer = _vision.CanSee(player);
            _visionTimer = definition.VisionCheckInterval;
        }

        _stateMachine.Tick(_context);
    }

    private void OnDrawGizmosSelected()
    {
        if (definition == null) return;

        Transform originT = eye != null ? eye : transform;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(originT.position, definition.VisionRange);

        Vector3 forward = originT.forward * definition.VisionRange;
        Quaternion leftRot = Quaternion.AngleAxis(-definition.VisionFOVAngle * 0.5f, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(definition.VisionFOVAngle * 0.5f, Vector3.up);
        Gizmos.DrawRay(originT.position, leftRot * forward);
        Gizmos.DrawRay(originT.position, rightRot * forward);

        Gizmos.color = Color.cyan;
        Vector3 wanderCenter = Application.isPlaying ? _context.SpawnPosition : transform.position;
        Gizmos.DrawWireSphere(wanderCenter, definition.PatrolRadius);
    }
}