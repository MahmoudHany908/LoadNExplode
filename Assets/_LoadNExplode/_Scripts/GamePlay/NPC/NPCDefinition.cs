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

    [Header("PatrolSettings")]
    public float PatrolRadius = 30f;

    [Header("Vision")]
    public float VisionRange = 10f;
    [Range(0f, 360f)] public float VisionFOVAngle = 90f;
    public float VisionCheckInterval = 0.15f;
    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    [Header("Combat (Hostile only)")]
    public float AttackRange = 2f;

    [Header("Flee (Civilian only)")]
    public float FleeDistance = 8f;
    public float SafeDistance = 12f;

    [Header("Search")]
    public float SearchDuration = 4f;
}