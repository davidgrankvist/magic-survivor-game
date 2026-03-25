using MagicSurvivor.Game.Configuration.Services;
using MagicSurvivor.Game.Systems;

namespace MagicSurvivor.Game.Infrastructure;

public static class SystemRegistry
{
    /// <summary>
    /// Creates systems in update order.
    /// </summary>
    public static ISystem[] CreateSystems(GameConfigLoader configLoader)
    {
        ISystem[] systems = [
            new ConfigLoadSystem(configLoader),
            new InputSystem(),
            new MovementSystem(),
            new PhysicsSystem(),
            new GraphicsSystem(),
        ];
        return systems;
    }
}