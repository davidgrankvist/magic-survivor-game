using Raylib_cs;
using MagicSurvivor.Game.State;
using MagicSurvivor.Game.Infrastructure;
using System.Numerics;

namespace MagicSurvivor.Game.Systems;

public class InputSystem : ISystem
{
    public void Update(GameState state, float deltaTime)
    {
        if (state.Editor.CanEnableEditor)
        {
            ReadEditorInput(state);
        }

        ReadCharacterControls(state);
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

    private void ReadCharacterControls(GameState state)
    {
        ReadMovementControls(state);
        ReadSpellControls(state);
    }

    private void ReadMovementControls(GameState state)
    {
        var entityDefinition = state.EntityDefinitions.Get(state.PlayerEntityDefinitionHandle);
        var entity = state.Entities.GetEntity(state.PlayerEntityHandle)!;
        var speed = entityDefinition.Speed;
        var velocity = new Vector3(0, 0, 0);

        // WASD movement on XZ plane.
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            velocity.X = -speed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            velocity.X = speed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            velocity.Z = -speed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            velocity.Z = speed;
        }

        entity.Velocity = velocity;
    }

    private void ReadSpellControls(GameState state)
    {
        ReadSpellSelectionControls(state);
        ReadAimControls(state);
        ReadCastControls(state);
    }

    private void ReadSpellSelectionControls(GameState state)
    {
        for (var i = 0; i < state.Spells.Count; i++)
        {
            var spell = state.Spells[i];
            var key = KeyboardKey.One + i;
            if (Raylib.IsKeyPressed(key))
            {
                state.SpellState.SelectedSpell = spell.Handle;
                break;
            }
        }
    }

    private void ReadAimControls(GameState state)
    {
        // Cast a ray and find position in XZ plane
        var ray = Raylib.GetScreenToWorldRay(Raylib.GetMousePosition(), state.Camera.RayCamera);
        var boundingBox = new BoundingBox
        {
            Min = new Vector3(float.MinValue, 0, float.MinValue),
            Max = new Vector3(float.MaxValue, 0, float.MaxValue),
        };
        var collision = Raylib.GetRayCollisionBox(ray, boundingBox);
        state.SpellState.AimEnabled = collision.Hit;
        state.SpellState.AimPos = collision.Point;
    }

    private void ReadCastControls(GameState state)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            state.SpellState.ShouldAttemptCast = true;
        }
    }
}