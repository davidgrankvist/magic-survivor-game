using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;

namespace MagicSurvivor.Game.Systems;

public class GraphicsSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);

        Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Black);

        Raylib.EndDrawing();
    }
}