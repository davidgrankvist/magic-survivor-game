using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        // Assume a stable FPS and use a fixed time step.
        const int fps = 60;
        const float deltaTime = 1.0f / fps;

        var state = new GameState();
        var systems = SystemRegistry.CreateSystems();

        Raylib.SetTargetFPS(fps);
        Raylib.InitWindow(800, 480, "Magic Survivor");

        while (!Raylib.WindowShouldClose())
        {
            foreach (var system in systems)
            {
                system.Update(state, deltaTime);
            }
        }

        Raylib.CloseWindow();
    }
}