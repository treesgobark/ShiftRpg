using System;
using ANLG.Utilities.FlatRedBall.Constants;
using Microsoft.Xna.Framework;
using ShiftRpg.Effects;

namespace ShiftRpg.Controllers.Gun;

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

        var proj = Parent.SpawnProjectile();
        proj.Position = Parent.Position;
        
        // bullet.DamageToDeal          = data.Damage;
        proj.CircleInstance.Radius = data.ProjectileRadius;
        proj.Velocity              = dir * data.ProjectileSpeed;
        proj.ApplyHolderEffects    = Parent.ApplyHolderEffects;
        proj.TargetHitEffects      = Parent.TargetHitEffects;
        proj.HolderHitEffects      = Parent.HolderHitEffects;

        Parent.ApplyHolderEffects(new[]
        {
            new KnockbackEffect(Parent.Team, Guid.NewGuid(), 100, Parent.RotationZ + MathConstants.HalfTurn)
        });

        Parent.MagazineRemaining--;
    }
}