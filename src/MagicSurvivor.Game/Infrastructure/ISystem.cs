using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Infrastructure;

/// <summary>
/// A step to run in the update loop. Systems should avoid holding state on their own and rely on the given game state.
/// Systems can communicate with other systems via the game state.
/// </summary>
public interface ISystem
{
    void Update(GameState state, float deltaTime);
}