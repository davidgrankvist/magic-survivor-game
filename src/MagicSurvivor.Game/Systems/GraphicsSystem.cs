using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;
using System.Numerics;

namespace MagicSurvivor.Game.Systems;

public class GraphicsSystem : ISystem
{
    private readonly Vector3 upAxis = new Vector3(0, 1, 0);

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

    // follows, but is blurry. add some dead zone mechanism probably?
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

        if (state.SpellState.AoeIsActive)
        {
            var spell = state.Spells.Get(state.SpellState.ActiveAoe);
            DrawAoe(state, spell, FadeColor(Color.Red, 200));
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
        var spell = state.Spells.Get(state.SpellState.SelectedSpell);
        switch (spell.Category)
        {
            case SpellCategory.Projectile:
                DrawArrowAim(state, spell);
                break;
            case SpellCategory.Aoe:
                if (!state.SpellState.AoeIsActive)
                {
                    DrawAoe(state, spell, FadeColor(Color.Red, 100));
                }
                break;
        }
    }

    private void DrawArrowAim(GameState state, Spell spell)
    {
        var playerEntity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        var start = playerEntity.Position;
        var direction = Vector3.Normalize(state.SpellState.AimPos - start);
        var offset = direction * spell.AimLength;
        var end = start + offset;

        var aimColor = Color.Red;
        var aimWidth = 2;
        DrawThickLineXZ(start, end, aimWidth, aimColor);

        var arrowHeadLength = spell.AimLength * 0.3f;
        var firstHeadOffset = Raymath.Vector3RotateByAxisAngle(-direction, upAxis, -MathF.PI / 4) * arrowHeadLength;
        var secondHeadOffset = Raymath.Vector3RotateByAxisAngle(-direction, upAxis, MathF.PI / 4) * arrowHeadLength;
        DrawThickLineXZ(end, end + firstHeadOffset, aimWidth, aimColor);
        DrawThickLineXZ(end, end + secondHeadOffset, aimWidth, aimColor);
    }
    
    private void DrawAoe(GameState state, Spell spell, Color color)
    {
        var player = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        Raylib.DrawCylinderEx(player.Position, player.Position + new Vector3(0, 0.001f, 0), spell.Range, spell.Range, 50, color);
    }

    private Color FadeColor(Color color, byte alpha)
    {
        return new Color
        {
            R = color.R,
            G = color.G,
            B = color.B,
            A = alpha,
        };
    }

    private void DrawThickLineXZ(Vector3 start, Vector3 end, float width, Color color)
    {
        var direction = Vector3.Normalize(end - start);
        var perpDirection = Raymath.Vector3RotateByAxisAngle(direction, upAxis, -MathF.PI / 2);
        var startLeft = start - perpDirection * width / 2;
        var startRight = start + perpDirection * width / 2;
        var endLeft = end - perpDirection * width / 2;
        var endRight = end + perpDirection * width / 2;

        DrawQuad(startLeft, startRight, endRight, endLeft, color);
    }

    private void DrawQuad(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Color color)
    {
        Raylib.DrawTriangle3D(a, b, c, color);
        Raylib.DrawTriangle3D(c, d, a, color);
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
        DrawSpellToolbar(state);

        if (state.Editor.IsEnabled)
        {
            DrawEditorUi(state);
        }
    }

    private void DrawEditorUi(GameState state)
    {
        var fontSize = 16;
        var padding = 5;
        Raylib.DrawFPS(padding, 0);
        Raylib.DrawText("Editor commands:", padding, 32, fontSize, Color.Green);
        Raylib.DrawText("G - Reload game config", padding, 32 + fontSize, fontSize, Color.Green);
        Raylib.DrawText("L - Reload level", padding, 32 + fontSize * 2, fontSize, Color.Green);
    }

    private void DrawSpellToolbar(GameState state)
    {
        var center = Raylib.GetScreenCenter();
        var height = Raylib.GetScreenHeight();

        var toolbarCenter = new Vector2(center.X, center.Y + height / 2f);
        var itemWidth = 50f;
        var itemHeight = 50f;
        var gapX = 10f;
        var paddingY = 15f;
        var toolbarStartX = toolbarCenter.X - state.Spells.Count * itemWidth / 2;
        var itemY = toolbarCenter.Y - itemHeight;
        var borderColor = Color.Black;

        var fontSize = 16;
        var fontColor = Color.Black;
        for (var i = 0; i < state.Spells.Count; i++)
        {
            var spell = state.Spells[i];
            var isSelected = spell.Handle == state.SpellState.SelectedSpell;

            var x = toolbarStartX + i * (itemWidth + gapX);
            var y = itemY - paddingY;

            Raylib.DrawRectangleLines((int)x, (int)y, (int)itemWidth, (int)itemHeight, borderColor);
            if (isSelected)
            {
                var borderThickness = 5f;
                var rect = new Rectangle
                {
                    Width = itemWidth + borderThickness,
                    Height = itemHeight + borderThickness,
                    X = x - borderThickness,
                    Y = y - borderThickness,
                };
                Raylib.DrawRectangleLinesEx(rect, borderThickness, Color.Orange);
            }

            var textX = x;
            var textY = y;
            Raylib.DrawText(spell.Name, (int)textX, (int)textY, fontSize, fontColor);
        }
    }
}