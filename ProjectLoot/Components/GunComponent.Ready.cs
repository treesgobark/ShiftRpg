using ANLG.Utilities.Core.Constants;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using Microsoft.Xna.Framework;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;
using ProjectLoot.Models;

namespace ProjectLoot.Components;

public partial class GunComponent
{
    protected class Ready : ParentedTimedState<GunComponent>
    {
        public Ready(GunComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
        
        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
        
        protected override void AfterTimedStateActivity() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (Parent is { CurrentGun.CurrentRoundsInMagazine: <= 0 }
                or { InputDevice.Reload.WasJustPressed: true, CurrentGun.IsFull: false })
            {
                return StateMachine.Get<Reloading>();
            }
            
            if (Parent.InputDevice.Fire.IsDown)
            {
                Fire();
                return StateMachine.Get<Recovery>();
            }
    
            return null;
        }
    
        public override void BeforeDeactivate() { }

        public override void Uninitialize() { }

        private void Fire()
        {
            var dir = Vector2ExtensionMethods.FromAngle(Parent.GunSprite.RotationZ).NormalizedOrZero().ToVector3();
            if (dir == Vector3.Zero)
            {
                return;
            }

            Bullet? proj = BulletFactory.CreateNew(Parent.GunSprite.Position);
            proj.InitializeProjectile(Parent.CurrentGun.GunData.ProjectileRadius, dir * Parent.CurrentGun.GunData.ProjectileSpeed, ~Parent.Team);

            var effects = new EffectBundle(Parent.Team, SourceTag.Gun);
            effects.AddEffect(new KnockbackEffect(Parent.Team, SourceTag.Gun, 50,
                                                  Rotation.FromRadians(Parent.GunSprite.RotationZ + MathConstants.HalfTurn),
                                                  KnockbackBehavior.Additive));
            // Holder.Effects.Handle(effects);

            int roundsSpent = Math.Clamp(Parent.CurrentGun.GunData.AmmoCostPerShot, 0, Parent.CurrentGun.CurrentRoundsInMagazine);
            Parent.CurrentGun.CurrentRoundsInMagazine -= roundsSpent;
            Parent.GunFired?.Invoke(roundsSpent);

            // Saiga12SingleShot1mSide.Play(0.1f, 0, 0);
        }
    }
}
