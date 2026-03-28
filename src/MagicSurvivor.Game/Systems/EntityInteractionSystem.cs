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
        if (firstDef.Category == EntityCategory.Npc && secondDef.Category == EntityCategory.Projectile)
        {
            HandleNpcProjectileHit(state, first, firstDef, second, secondDef);
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