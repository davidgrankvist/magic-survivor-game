using System.Numerics;

namespace MagicSurvivor.Game.Configuration;

public class EntityDefinitionConfig
{
    public string Type = string.Empty;
    public string Category = string.Empty;
    public float Speed;
    public Vector3 Collider;
    public float Health;
    public float Damage;
}