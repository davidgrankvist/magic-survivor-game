namespace MagicSurvivor.Game.Configuration;

public class SpellConfig
{
    // General
    public string Name = string.Empty;
    public string Category = string.Empty;
    public float Cooldown;

    // Projectile
    public string SpawnEntity = string.Empty;
    public float AimLength;

    // Aoe
    public float Damage;
    public float Range;
    public float Duration;
    public float TickCooldown;
}