using MagicSurvivor.Game.Systems;

namespace MagicSurvivor.Game.Infrastructure;

public static class SystemRegistry
{
    /// <summary>
    /// Creates systems in update order.
    /// </summary>
    public static ISystem[] CreateSystems()
    {
        ISystem[] systems = [
            new InputSystem(),
            new GraphicsSystem(),
        ];
        return systems;
    }
}