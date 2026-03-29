namespace MagicSurvivor.Game.State;

/// <summary>
/// Refers to an entity without using a pointer. This is to avoid situations where a an entity is removed
/// and other entities still hold a reference to it. Uses a generation counter to allow entity slots to be recycled
/// when entities are removed.
/// </summary>
public struct EntityHandle
{
    public int Index;
    public int Generation;

    public static readonly EntityHandle InvalidHandle = new EntityHandle
    {
        Index = -1,
        Generation = -1
    };

    public static bool operator ==(EntityHandle first, EntityHandle second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(EntityHandle first, EntityHandle second)
    {
        return !(first == second);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is not EntityHandle handle)
        {
            return false;
        }

        return Index == handle.Index && Generation == handle.Generation;
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Index, Generation);
    }
}
