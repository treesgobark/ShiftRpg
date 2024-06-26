using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall.Input;
using FlatRedBall.Math;
using Microsoft.Xna.Framework;

namespace ProjectLoot.InputDevices;

public class VirtualAimer : I2DInput
{
    private readonly Mouse _mouse;
    private readonly IPositionable _position;
    private readonly float _aimThreshold;

    public VirtualAimer(Mouse mouse, IPositionable position, float aimThreshold)
    {
        _mouse = mouse;
        _position = position;
        _aimThreshold = aimThreshold;
    }

    public float X => NormalizedDirection.X;
    public float Y => NormalizedDirection.Y;
    public float XVelocity => 64;
    public float YVelocity => 64;
    public float Magnitude => NormalizedDirection.Length();

    private Vector2 NormalizedDirection
    {
        get
        {
            var mouseWorldPos = new Vector2(_mouse.WorldXAt(_position.Z), _mouse.WorldYAt(_position.Z));
            Vector2 mouseToPlayer = mouseWorldPos - _position.PositionAsVec3().ToVector2();
            float newMagnitude = mouseToPlayer.Length() / _aimThreshold;
            Vector2 newM2P = mouseToPlayer.AtLength(newMagnitude);
            return newM2P; 
        }
    }
}
