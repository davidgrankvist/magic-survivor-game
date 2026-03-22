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
        // CreateSizedWindow(800, 400);
        CreateFullscreenWindow();

        while (!Raylib.WindowShouldClose())
        {
            foreach (var system in systems)
            {
                system.Update(state, deltaTime);
            }
        }

        Raylib.CloseWindow();
    }

    private static void CreateSizedWindow(int w, int h)
    {
        CreateDummyWindow();
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(w, h, "Magic Survivor");
    }

    private static void CreateFullscreenWindow()
    {
        CreateDummyWindow();
        Raylib.SetConfigFlags(ConfigFlags.UndecoratedWindow);
        Raylib.InitWindow(Raylib.GetScreenWidth(), Raylib.GetScreenHeight(), "Magic Survivor");
    }

    /// <summary>
    /// This is a workaround to prepare the window related API calls. Before first window creation calls like GetScreenWidth return 0.
    /// There may also be additional re-positioning of smaller windows as the screen center is not known yet.
    /// </summary>
    private static void CreateDummyWindow()
    {
        Raylib.SetConfigFlags(ConfigFlags.HiddenWindow);
        Raylib.InitWindow(0, 0, "Dummy window");
        Raylib.CloseWindow();
        Raylib.ClearWindowState(ConfigFlags.HiddenWindow);
    }
}