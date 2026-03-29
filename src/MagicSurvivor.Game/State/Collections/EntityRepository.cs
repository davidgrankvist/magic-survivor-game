using System.Collections;

namespace MagicSurvivor.Game.State.Collections;

public class EntityRepository : IEnumerable<Entity>
{
    private List<EntitySlot> slots = [];
    private List<int> freeIndices = [];

    public int Count => slots.Count;
    
    public Entity this[int i]
    {
        get => slots[i].Entity;
    }

    public Entity? Get(EntityHandle handle)
    {
        if (handle.Index < 0 || handle.Index >= slots.Count)
        {
            return null;
        }

        var slot = slots[handle.Index];
        if (slot.Generation != handle.Generation)
        {
            return null;
        }

        if (slot.Entity.IsDeleted)
        {
            return null;
        }

        return slot.Entity;
    }

    public void Add(Entity entity)
    {
        int targetIndex;
        if (freeIndices.Count == 0)
        {
            slots.Add(new EntitySlot());
            targetIndex = slots.Count - 1;
        }
        else
        {
            targetIndex = freeIndices[freeIndices.Count - 1];
            freeIndices.RemoveAt(freeIndices.Count - 1);
        }

        var slot = slots[targetIndex];
        entity.Handle.Index = targetIndex;
        entity.Handle.Generation = slot.Generation;
        slot.Entity = entity;
    }

    public void Remove(EntityHandle handle)
    {
        if (handle.Index < 0 || handle.Index >= slots.Count)
        {
            return;
        }

        var slot = slots[handle.Index];
        if (slot.Generation != handle.Generation)
        {
            return;
        }

        slot.Generation++;
        slot.Entity.IsDeleted = true;

        freeIndices.Add(handle.Index);
    }

    public void Clear()
    {
        slots.Clear();
        freeIndices.Clear();
    }

    public IEnumerator<Entity> GetEnumerator()
    {
        return slots.Select(s => s.Entity).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class EntitySlot
    {
        public Entity Entity = new();
        public int Generation;
    }
}
