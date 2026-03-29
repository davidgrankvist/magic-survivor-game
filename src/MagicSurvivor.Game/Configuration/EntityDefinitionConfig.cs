using System.Numerics;

namespace MagicSurvivor.Game.Configuration;

public class EntityDefinitionConfig
{
    public string Type = string.Empty;
    public string Category = string.Empty;
    public float Speed;
    public Vector3 Collider;

    // Player or npc
    public float Health;

    // Npc attacks
    public string AttackType = string.Empty;

    // Npc melee or projectile impact
    public float Damage;

    // Npc melee
    public float MeleeRange;
    public float StrikeWindup;
    public float StrikeCooldown;
}