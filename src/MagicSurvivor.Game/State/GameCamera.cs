using System.Numerics;
using Raylib_cs;

namespace MagicSurvivor.Game.State;

public class GameCamera
{
    public GameCamera()
    {
        RayCamera = new Camera3D()
        {
            Target = new Vector3(0, 0, 0),
            Up = new Vector3(0, 1, 0),
            Projection = CameraProjection.Perspective,
            // Position and FovY will be read from the config
        };
    }

    public Camera3D RayCamera;
    public Vector3 OffsetFromTarget;

    public void Reset()
    {
        RayCamera.Position = OffsetFromTarget;
        RayCamera.Target = new Vector3(0, 0, 0);
    }
}
