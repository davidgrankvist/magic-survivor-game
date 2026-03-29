using System.Numerics;

namespace MagicSurvivor.Game.State;

public class EntityDefinition
{
    public StaticHandle Handle = StaticHandle.InvalidHandle;
    public EntityCategory Category = EntityCategory.None;
    public float Speed;
    public Vector3 Collider;

    // Player or npc
    public float Health;

    // Npc attacks
    public EntityAttackType AttackType = EntityAttackType.None;

    // Npc melee or projectile impact
    public float Damage;

    // Npc melee
    public float MeleeRange;
    public float StrikeWindup;
    public float StrikeCooldown;
}
