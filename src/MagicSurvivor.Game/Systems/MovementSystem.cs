using System.Numerics;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

public class MovementSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        var playerEntity = state.Entities.Get(state.PlayerEntityHandle)!;
        var playerEntityDefinition = state.EntityDefinitions.Get(state.PlayerEntityDefinitionHandle);
        var playerPosition = playerEntity.Position;

        foreach (var entity in state.Entities)
        {
            if (entity.IsDeleted)
            {
                continue;
            }

            var entityDefinition = state.EntityDefinitions.Get(entity.DefinitionHandle);

            if (entityDefinition.Category != EntityCategory.Npc)
            {
                continue;
            }

            // Follow player.
            var target = playerPosition;
            var distance = Vector3.Distance(target, entity.Position);
            var gap = playerEntityDefinition.Collider.X / 2 + entityDefinition.Collider.X / 2;
            if (distance > gap)
            {
                var direction = Vector3.Normalize(target - entity.Position);
                var velocity = direction * entityDefinition.Speed;
                entity.Velocity = velocity;
            }
            else
            {
                entity.Velocity = Vector3.Zero;
            }
        }
    }
}