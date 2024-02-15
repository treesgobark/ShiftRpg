using System;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;

namespace ShiftRpg.Controllers.MeleeWeapon;

public class Idle(Entities.MeleeWeapon obj) : MeleeWeaponController(obj)
{
    protected MeleeWeaponController? NextState { get; set; }
    
    public override void Initialize() { }

    public override void OnActivate()
    {
        if (Parent.Owner is not null)
        {
            Parent.Owner.CircleInstance.Color = Color.Green;
        }
        
        base.OnActivate();
    }

    public override void CustomActivity()
    {
        if (Parent.InputDevice.Attack.WasJustPressed)
        {
            BeginAttack();
        }
    }

    public override MeleeWeaponController? EvaluateExitConditions()
    {
        return NextState;
    }

    public override void BeforeDeactivate()
    {
        NextState = null;
    }

    public override void BeginAttack()
    {
        PrepareAttackData();
        
        NextState = Get<Startup>();
    }

    private void PrepareAttackData()
    {
        var data = Parent.CurrentAttackData;
        
        Parent.PolygonSave.Points[0].X = data.HitboxOffsetX;
        Parent.PolygonSave.Points[4].X = data.HitboxOffsetX;
        Parent.PolygonSave.Points[0].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        Parent.PolygonSave.Points[4].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        
        Parent.PolygonSave.Points[1].X = data.HitboxOffsetX + data.HitboxSizeX;
        Parent.PolygonSave.Points[1].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        
        Parent.PolygonSave.Points[2].X = data.HitboxOffsetX + data.HitboxSizeX;
        Parent.PolygonSave.Points[2].Y = data.HitboxOffsetY - data.HitboxSizeY / 2;
        
        Parent.PolygonSave.Points[3].X = data.HitboxOffsetX;
        Parent.PolygonSave.Points[3].Y = data.HitboxOffsetY - data.HitboxSizeY / 2;
        
        Parent.PolygonSave.MapShapeRelative(Parent.PolygonInstance);

        // Parent.DamageToDeal = data.Damage;

        Parent.ApplyHolderEffects(new[]
        {
            new KnockbackEffect(Parent.Team, Parent.Source, data.KnockbackVelocity, Parent.Owner.RotationZ)
        });
        
        Parent.TargetHitEffects = new IEffect[]
        {
            new DamageEffect(~Parent.Team, Parent.Source, 2),
            new KnockbackEffect(~Parent.Team, Parent.Source, 100, Parent.RotationZ),
            new DamageOverTimeEffect(~Parent.Team, Parent.Source, 1, 2, 5, 1),
            new ApplyShatterEffect(~Parent.Team, Parent.Source),
        };
    }

    public override void EndAttack()
    {
    }
}