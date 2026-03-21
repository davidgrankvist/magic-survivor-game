using System.Numerics;

namespace MagicSurvivor.Game.State;

public class Entity
{
    public bool IsDeleted;

    public EntityHandle Handle = EntityHandle.InvalidHandle;
    public StaticHandle DefinitionHandle = StaticHandle.InvalidHandle;

    public Vector3 Position;
    public Vector3 Velocity;
}
