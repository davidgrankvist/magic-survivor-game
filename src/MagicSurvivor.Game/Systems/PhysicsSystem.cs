using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

/// <summary>
/// Updates entity positions.
/// </summary>
public class PhysicsSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        foreach (var entity in state.Entities)
        {
            if (entity.IsDeleted)
            {
                continue;
            }

            var delta = entity.Velocity * deltaTime;
            entity.Position += delta;
        }
    }
}