using System;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.States;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;

namespace ShiftRpg.Entities;

partial class MeleeWeapon
{
    protected class Idle : TimedState<MeleeWeapon>
    {
        protected IState? NextState { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            Parent.Owner.SetPlayerColor(Color.Green);
        }

        public override void CustomActivity()
        {
            if (Parent.InputDevice.Attack.WasJustPressed)
            {
                PrepareAttackData();
            
                NextState = StateMachine.Get<Startup>();
            }
        }

        public override IState? EvaluateExitConditions()
        {
            return NextState;
        }

        public override void BeforeDeactivate()
        {
            NextState = null;
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

        public Idle(MeleeWeapon parent, IStateMachine stateMachine) : base(parent, stateMachine)
        {
        }
    }
}
