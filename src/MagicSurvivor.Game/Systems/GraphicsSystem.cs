using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;

namespace MagicSurvivor.Game.Systems;

public class GraphicsSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        // UpdateCameraFollowPlayer(state);

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);

        Raylib.BeginMode3D(state.Camera.RayCamera);

        DrawScene(state);

        Raylib.EndMode3D();

        DrawUi(state);

        Raylib.EndDrawing();
    }

    // TODO: follows, but is blurry. add some dead zone mechanism probably?
    private void UpdateCameraFollowPlayer(GameState state)
    {
        var entity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        state.Camera.RayCamera.Target = entity.Position;
        state.Camera.RayCamera.Position = entity.Position + state.Camera.OffsetFromTarget;
    }

    private void DrawScene(GameState state)
    {
        DrawGround();

        if (state.SpellState.AimEnabled)
        {
            DrawAim(state);
        }

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

    private void DrawAim(GameState state)
    {
        Raylib.DrawCube(state.SpellState.AimPos, 1, 1, 1, Color.Red);
    }

    private void DrawEntity(GameState state, Entity entity)
    {
        var definition = state.EntityDefinitions.Get(entity.DefinitionHandle);
        var collider = definition.Collider;
        var position = entity.Position;

        var color = GetEntityColor(definition.Category);
        Raylib.DrawCube(position, collider.X, collider.Y, collider.Z, color);
        Raylib.DrawCubeWires(position, collider.X, collider.Y, collider.Z, Color.Black);
    }

    private Color GetEntityColor(EntityCategory entityCategory)
    {
        switch (entityCategory)
        {
            case EntityCategory.Player:
                return Color.Blue;
            case EntityCategory.Npc:
                return Color.DarkGreen;
            case EntityCategory.Projectile:
                return Color.Orange;
            default:
                return Color.Black;
        }
    }

    private void DrawUi(GameState state)
    {
        if (state.Editor.IsEnabled)
        {
            var fontSize = 16;
            var padding = 5;
            Raylib.DrawFPS(padding, 0);
            Raylib.DrawText("Editor commands:", padding, 32, fontSize, Color.Green);
            Raylib.DrawText("G - Reload game config", padding, 32 + fontSize, fontSize, Color.Green);
            Raylib.DrawText("L - Reload level", padding, 32 + fontSize * 2, fontSize, Color.Green);
        }
    }
}