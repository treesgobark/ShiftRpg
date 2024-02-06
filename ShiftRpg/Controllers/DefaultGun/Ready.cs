using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using FlatRedBall.Debugging;
using Microsoft.Xna.Framework;
using ShiftRpg.Factories;

namespace ShiftRpg.Controllers.DefaultGun;

public class Ready(Entities.Gun obj) : GunController(obj)
{
    protected GunController? NextState { get; set; }
    protected bool IsFiring { get; set; }

    public override void Initialize() { }

    public override void CustomActivity()
    {
        if (Parent.InputDevice.Fire.WasJustPressed)
        {
            FireBullet();
        }

        if (Parent.InputDevice.Reload.WasJustPressed)
        {
            NextState = Get<Reloading>();
        }
    }

    public override GunController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }
        
        if (Parent.MagazineRemaining <= 0)
        {
            return Get<Reloading>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    private void FireBullet()
    {
        var data = Parent.CurrentGunData;
        
        var dir = Vector2ExtensionMethods.FromAngle(Parent.RotationZ).NormalizedOrZero().ToVector3();
        if (dir == Vector3.Zero) return;
        
        var bullet = BulletFactory.CreateNew();
        bullet.Position = Parent.Position;
        
        bullet.DamageToDeal          = data.Damage;
        bullet.CircleInstance.Radius = data.ProjectileRadius;
        bullet.Velocity              = dir * data.ProjectileSpeed;

        Parent.MagazineRemaining--;
    }
}