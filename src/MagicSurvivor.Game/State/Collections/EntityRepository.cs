namespace MagicSurvivor.Game.State.Collections;

public class EntityRepository
{
    private List<EntitySlot> slots = [];
    private List<int> freeIndices = [];

    public Entity? GetEntity(EntityHandle handle)
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

        return slot.Entity;
    }

    public void AddEntity(Entity entity)
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

    public void RemoveEntity(EntityHandle handle)
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
        slot.Entity = null!;

        freeIndices.Add(handle.Index);
    }

    private class EntitySlot
    {
        public Entity Entity = null!;
        public int Generation;
    }
}
