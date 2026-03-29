namespace MagicSurvivor.Game.State;

public class Spell
{
    // General
    public StaticHandle Handle  = StaticHandle.InvalidHandle;
    public string Name = string.Empty;
    public SpellCategory Category = SpellCategory.None;
    public float Cooldown;
    public float CastBegin;

    // Projectile
    public StaticHandle SpawnEntity = StaticHandle.InvalidHandle;
    public float AimLength;

    // Aoe
    public float Damage;
    public float Range;
    public float Duration;
    public float TickCooldown;
    public float TickBegin;
    public bool TickActive;
}
