using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Infrastructure;

public interface ISystem
{
    void Update(GameState state, float deltaTime);
}