using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using ANLG.Utilities.FlatRedBall.States;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;

namespace ProjectLoot.Entities;

partial class MeleeWeapon
{
    protected class Idle : TimedState<MeleeWeapon>
    {
        protected IState? NextState { get; set; }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate()
        {
            // Parent.Holder.SetPlayerColor(Color.Green);
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

        public override void Uninitialize() { }

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

            var holderEffects = new EffectBundle(Parent.Team, Parent.Source);
            holderEffects.AddEffect(new KnockbackEffect(Parent.Team, Parent.Source, data.KnockbackVelocity, Rotation.Zero, true));
            Parent.Holder.EffectsComponent.HandlerCollection.Handle(holderEffects);
            
            var targetEffects = new EffectBundle(Parent.Team, Parent.Source);
            targetEffects.AddEffect(new DamageEffect(~Parent.Team, Parent.Source, 10));
            targetEffects.AddEffect(new KnockbackEffect(~Parent.Team, Parent.Source, 100, Parent.GetRotationZ()));
            targetEffects.AddEffect(new DamageOverTimeEffect(~Parent.Team, Parent.Source, 1, 2, 5, 1));
            targetEffects.AddEffect(new ApplyShatterEffect(~Parent.Team, Parent.Source));
            targetEffects.AddEffect(new WeaknessDamageEffect(~Parent.Team, Parent.Source, .2f));
            Parent.TargetHitEffects = targetEffects;
        }

        public Idle(MeleeWeapon parent, IStateMachine stateMachine) : base(parent, stateMachine)
        {
        }
    }
}
