namespace MagicSurvivor.Game.State;

/// <summary>
/// Refers to an object without using a pointer. This is to enable reloading of settings during runtime
/// without ending up with invalid references.
/// </summary>
public struct StaticHandle
{
    public int Index;

    public static readonly StaticHandle InvalidHandle = new StaticHandle
    {
        Index = -1
    };

    public static bool operator ==(StaticHandle first, StaticHandle second)
    {
        return Equals(first, second);
    }

    public static bool operator !=(StaticHandle first, StaticHandle second)
    {
        return !(first == second);
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj is not StaticHandle handle)
        {
            return false;
        }

        return Index == handle.Index;
    }

    public override readonly int GetHashCode()
    {
        return Index.GetHashCode();
    }
}