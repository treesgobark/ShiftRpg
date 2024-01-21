using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using FlatRedBall.Debugging;
using Microsoft.Xna.Framework;
using ShiftRpg.Factories;

namespace ShiftRpg.Controllers.DefaultGun;

public class DefaultGunController(Entities.DefaultGun parent)
    : EntityController<Entities.DefaultGun, DefaultGunController>(parent)
{
    protected DefaultGunController? NextState { get; set; }
    
    public override void Initialize() { }

    public override void OnActivate()
    {
        Debugger.CommandLineWrite($"[{TimeManager.CurrentFrame}] Ready to fire!");
    }

    public override void CustomActivity() { }

    public override DefaultGunController? EvaluateExitConditions()
    {
        if (NextState is not null)
        {
            return NextState;
        }
        
        if (parent.MagazineRemaining <= 0)
        {
            return Get<Reloading>();
        }

        return null;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    public virtual void BeginFire()
    {
        var dir = Vector2ExtensionMethods.FromAngle(Parent.RotationZ).NormalizedOrZero().ToVector3();
        if (dir == Vector3.Zero) return;
        
        var bullet = BulletFactory.CreateNew();
        bullet.Position = Parent.Position;
        bullet.Velocity = dir * 500;

        parent.MagazineRemaining--;
    }

    public virtual void EndFire()
    {
        
    }

    public virtual void Reload()
    {
        NextState = Get<Reloading>();
    }
}