using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    [SerializeField] private NPCDefinition definition;
    [SerializeField] private Transform eye;                // vision origin - defaults to this transform
    [SerializeField] private Transform player;              // auto-found via "Player" tag if left empty
    [SerializeField] private Transform[] patrolPoints;

    private NPCContext _context;
    private NPCStateMachine _stateMachine;
    private VisionSensor _vision;
    private float _visionTimer;

    private void Awake()
    {
        var agent = GetComponent<NavMeshAgent>();

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
            PatrolPoints = patrolPoints
        };
    }

    private void Start()
    {
        _stateMachine.ChangeState(_context.States.Patrol, _context);
    }

    private void Update()
    {
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
    }
}