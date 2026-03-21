using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;

namespace MagicSurvivor.Game.Systems;

public class InputSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.A))
        {
            Raylib.TraceLog(TraceLogLevel.Info, "Pressed A");
        }
    }
}