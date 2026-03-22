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

        Raylib.BeginMode3D(state.Camera.RayCamera);

        DrawScene(state);

        Raylib.EndMode3D();

        DrawUi(state);

        Raylib.EndDrawing();
    }

    private void DrawScene(GameState state)
    {
        DrawGround();

        foreach (var entity in state.Entities)
        {
            if (entity.IsDeleted)
            {
                continue;
            }

            DrawEntity(state, entity);
        }
    }

    private void DrawGround()
    {
        Raylib.DrawGrid(100, 1.0f);
    }

    private void DrawEntity(GameState state, Entity entity)
    {
        var definition = state.EntityDefinitions.Get(entity.DefinitionHandle);
        var collider = definition.Collider;
        var position = entity.Position;

        var color = entity.DefinitionHandle.Index == state.PlayerEntityDefinitionHandle.Index ? Color.Blue : Color.DarkGreen;
        Raylib.DrawCube(position, collider.X, collider.Y, collider.Z, color);
        Raylib.DrawCubeWires(position, collider.X, collider.Y, collider.Z, Color.Black);
    }

    private void DrawUi(GameState state)
    {
        if (state.Editor.IsEnabled)
        {
            Raylib.DrawText("Editor commands:", 0, 0, 16, Color.Green);
            Raylib.DrawText("G - Reload game config", 0, 16, 16, Color.Green);
            Raylib.DrawText("L - Reload level", 0, 32, 16, Color.Green);
        }
    }
}