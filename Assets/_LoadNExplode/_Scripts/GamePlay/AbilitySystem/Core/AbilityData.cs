using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Data Data")]
public class AbilityData : ScriptableObject
{
    public enum Rarity { Common, Rare, Epic, Legendary }




    [TextArea(3, 5)] public string description;
    public Sprite icon;
    public Rarity rarity;

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case Rarity.Common: return new Color(0.8f, 0.8f, 0.8f); // Light Grey
            case Rarity.Rare: return new Color(0.3f, 0.6f, 1.0f);   // Blue
            case Rarity.Epic: return new Color(0.7f, 0.3f, 1.0f);   // Purple
            case Rarity.Legendary: return new Color(1.0f, 0.6f, 0.1f); // Orange/Gold
            default: return Color.white;
        }
    }
}
