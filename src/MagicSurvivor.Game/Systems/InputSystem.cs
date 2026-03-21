using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;

namespace MagicSurvivor.Game.Systems;

public class InputSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        if (state.Editor.CanEnableEditor)
        {
            ReadEditorInput(state);
        }
    }

    private void ReadEditorInput(GameState state)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.F1))
        {
            state.Editor.IsEnabled = !state.Editor.IsEnabled;
        }

        if (state.Editor.IsEnabled)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.G))
            {
                state.Editor.ShouldReloadGameConfig = true;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.L))
            {
                state.Level.ShouldLoadLevel = true;
            }
        }
    }
}