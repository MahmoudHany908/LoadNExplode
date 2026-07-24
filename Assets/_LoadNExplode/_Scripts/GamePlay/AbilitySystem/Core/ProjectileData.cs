using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public GameObject Prefab;
    public float Speed;
    public float Lifetime;


    // What happens on hit
    [Header("Impact Effects ")]
    public List<ScriptableObject> ImpactEffects;

    // VFX
    [Header("VFX")]
    public ScriptableObject TrailVFX;
    public ScriptableObject ImpactVFX;
}