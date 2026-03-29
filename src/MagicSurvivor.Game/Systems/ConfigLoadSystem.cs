using MagicSurvivor.Game.Configuration.Services;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.State;

namespace MagicSurvivor.Game.Systems;

/// <summary>
/// Loads game and level configuration. This enables both level loading and live tweaking of parameters while in editor mode.
/// </summary>
public class ConfigLoadSystem : ISystem
{
    private readonly GameConfigLoader configLoader;

    public ConfigLoadSystem(GameConfigLoader configLoader)
    {
        this.configLoader = configLoader;
    }

    public void Update(GameState state, float deltaTime)
    {
        if (state.Editor.ShouldReloadGameConfig)
        {
            configLoader.LoadGameConfig(state);
            state.Editor.ShouldReloadGameConfig = false;
        }

        if (state.Level.ShouldLoadLevel)
        {
            configLoader.LoadLevel(state, state.Level.CurrentLevelId);
            state.Level.ShouldLoadLevel = false;
        }
    }
}