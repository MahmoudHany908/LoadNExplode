using UnityEngine;

public enum NPCBehaviorType
{
    Civilian,
    Hostile
}

[CreateAssetMenu(fileName = "NPCDefinition", menuName = "NPC/NPC Definition")]
public class NPCDefinition : ScriptableObject
{
    [Header("Behavior")]
    public NPCBehaviorType Behavior = NPCBehaviorType.Civilian;

    [Header("Movement Speeds")]
    public float PatrolSpeed = 2f;
    public float ChaseSpeed = 4.5f;
    public float FleeSpeed = 5f;
    public float CharmedMovementSpeed = 2.5f;

    [Header("Agent Tuning")]
    [Tooltip("How fast the NPC turns (degrees/sec). Lower = smoother arcs.")]
    public float AngularSpeed = 240f;
    [Tooltip("How fast the NPC accelerates/decelerates.")]
    public float Acceleration = 6f;
    [Tooltip("Distance from destination at which the agent considers itself arrived.")]
    public float StoppingDistance = 0.3f;

    [Header("Patrol Settings")]
    public float PatrolRadius = 30f;
    [Tooltip("Min distance for a new patrol point to avoid tiny shuffles.")]
    public float MinPatrolDistance = 5f;
    [Tooltip("Min idle seconds at a patrol point before moving on.")]
    public float PatrolIdleMin = 1f;
    [Tooltip("Max idle seconds at a patrol point before moving on.")]
    public float PatrolIdleMax = 3f;

    [Header("Chase Settings")]
    [Tooltip("Seconds between NavMesh repath calls during chase.")]
    public float ChaseRepathInterval = 0.2f;
    [Tooltip("Acceleration during chase — high value = instant top speed.")]
    public float ChaseAcceleration = 40f;
    [Tooltip("Turn rate during chase — high value = snappy facing.")]
    public float ChaseAngularSpeed = 720f;

    [Header("Vision")]
    public float VisionRange = 10f;
    [Range(0f, 360f)] public float VisionFOVAngle = 90f;
    public float VisionCheckInterval = 0.15f;
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    [Header("Combat (Hostile only)")]
    public float AttackRange = 2f;
    public float AttackDamage = 10f;
    public float AttackCooldown = 1f;
    public float AttackSphereRadius = 0.5f;

    [Header("Flee (Civilian only)")]
    public float FleeDistance = 8f;
    public float SafeDistance = 12f;

    [Header("Search")]
    public float SearchDuration = 4f;



}