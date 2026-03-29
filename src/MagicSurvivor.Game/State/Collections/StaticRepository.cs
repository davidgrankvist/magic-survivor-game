using System.Collections;

namespace MagicSurvivor.Game.State.Collections;

/// <summary>
/// A collection of objects that can be accessed with a <see cref="StaticHandle"/>.
/// This is for objects that are in a stable index between frames, such as entity or spell definitions.
/// </summary>
public class StaticRepository<TObject> : IEnumerable<TObject>
{
    private List<TObject> objects = [];

    public int Count => objects.Count;

    public TObject this[int i]
    {
        get => objects[i];
    }

    public TObject Get(StaticHandle handle)
    {
        return objects[handle.Index]!;
    }

    public void Add(TObject obj)
    {
        objects.Add(obj);
    }

    public void Clear()
    {
        objects.Clear();
    }

    public IEnumerator<TObject> GetEnumerator()
    {
        return objects.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}