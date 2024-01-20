using FlatRedBall.Input;
using Microsoft.Xna.Framework;
using ShiftRpg.Entities;

namespace ShiftRpg.InputDevices;

public class VirtualAimer : I2DInput
{
    private readonly Mouse _mouse;
    private readonly Player _player;

    public VirtualAimer(Mouse mouse, Player player)
    {
        _mouse = mouse;
        _player = player;
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
            var mouseWorldPos = new Vector2(_mouse.WorldXAt(_player.Z), _mouse.WorldYAt(_player.Z));
            var normalizedPos = (mouseWorldPos - _player.Position.ToVector2()).NormalizedOrZero();
            return normalizedPos;
        }
    }
}