using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;
using MagicSurvivor.Game.Configuration.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        // Assume a stable FPS and use a fixed time step.
        const int fps = 60;
        const float deltaTime = 1.0f / fps;
#if DEBUG
        const bool canEnableEditor = true;
#else
        const bool canEnableEditor = false;
#endif

        var configLoader = new GameConfigLoader(loadFromSourceDir: canEnableEditor);
        var systems = SystemRegistry.CreateSystems(configLoader);

        var state = new GameState();
        state.Editor.CanEnableEditor = canEnableEditor;
        state.Level.CurrentLevelId = "dummy";

        configLoader.LoadGameConfig(state);
        configLoader.LoadLevel(state, state.Level.CurrentLevelId);

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