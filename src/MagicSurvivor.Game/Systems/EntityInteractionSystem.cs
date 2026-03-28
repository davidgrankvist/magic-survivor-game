using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;
using Raylib_cs;

namespace MagicSurvivor.Game.Systems;

public class EntityInteractionSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        // TODO(optimize): Naive pairwise checks for now. OK for few entities.
        for (var i = 0; i < state.Entities.Count; i++)
        {
            var first = state.Entities[i];
            if (first.IsDeleted)
            {
                continue;
            }

            for (var j = i + 1; j < state.Entities.Count; j++)
            {
                var second = state.Entities[j];
                if (second.IsDeleted)
                {
                    continue;
                }

                var firstDef = state.EntityDefinitions.Get(first.DefinitionHandle);
                var secondDef = state.EntityDefinitions.Get(second.DefinitionHandle);
                HandleInteraction(state, first, firstDef, second, secondDef);
            }
        }
    }

    private void HandleInteraction(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        switch (firstDef.Category)
        {
            case EntityCategory.Player:
                if (secondDef.Category == EntityCategory.Npc)
                {
                    HandlePlayerNpcInteraction(state, first, firstDef, second, secondDef);
                }
                break;
            case EntityCategory.Npc:
                if (secondDef.Category == EntityCategory.Projectile)
                {
                    HandleNpcProjectileHit(state, first, firstDef, second, secondDef);
                }
                break;
        }
    }

    private void HandlePlayerNpcInteraction(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        if (state.SpellState.AoeIsActive)
        {
            ApplyAoe(state, first, second);
        }
    }

    private void ApplyAoe(GameState state, Entity first, Entity second)
    {
        var spell = state.Spells.Get(state.SpellState.ActiveAoe);
        if (!spell.TickActive)
        {
            return;
        }

        var distance = Vector3.Distance(first.Position, second.Position);
        if (distance >= spell.Range)
        {
            return;
        }

        second.Health -= spell.Damage;
        if (second.Health <= 0)
        {
            state.Entities.RemoveEntity(second.Handle);
        }
    }

    private void HandleNpcProjectileHit(GameState state,
        Entity first, EntityDefinition firstDef,
        Entity second, EntityDefinition secondDef)
    {
        var firstBox = new BoundingBox
        {
            Min = first.Position - firstDef.Collider / 2f,
            Max = first.Position + firstDef.Collider / 2f,
        };
        var secondBox = new BoundingBox
        {
            Min = second.Position - secondDef.Collider / 2f,
            Max = second.Position + secondDef.Collider / 2f,
        };
        var didCollide = Raylib.CheckCollisionBoxes(firstBox, secondBox);
        if (!didCollide)
        {
            return;
        }

        first.Health -= secondDef.Damage;
        state.Entities.RemoveEntity(second.Handle);
        if (first.Health <= 0)
        {
            state.Entities.RemoveEntity(first.Handle);
        }
    }
}