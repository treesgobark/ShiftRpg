using ANLG.Utilities.Core.Constants;
using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using Microsoft.Xna.Framework;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Effects;
using ProjectLoot.Entities;
using ProjectLoot.Factories;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Models;

partial class StandardGunModel
{
    private class Ready : TimedState
    {
        private StandardGunModel GunModel { get; }

        public Ready(IReadonlyStateMachine stateMachine, ITimeManager timeManager, StandardGunModel gunModel)
            : base(stateMachine, timeManager)
        {
            GunModel  = gunModel;
        }

        public override void Initialize()
        {
        }

        protected override void AfterTimedStateActivate()
        {
        }

        public override IState? EvaluateExitConditions()
        {
            if (!GunModel.IsEquipped)
            {
                return StateMachine.Get<NotEquipped>();
            }

            if (GunModel.CurrentRoundsInMagazine <= 0
                || GunModel.GunComponent.GunInputDevice.Reload.WasJustPressed && !GunModel.IsFull)
            {
                return StateMachine.Get<Reloading>();
            }

            if (GunModel.GunData.IsSingleShot     && GunModel.GunComponent.GunInputDevice.Fire.WasJustPressed
                || !GunModel.GunData.IsSingleShot && GunModel.GunComponent.GunInputDevice.Fire.IsDown)
            {
                Fire();
                return StateMachine.Get<Recovery>();
            }

            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }

        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize()
        {
        }

        private void Fire()
        {
            var dir = GunModel.GunComponent.GunRotation.ToVector3();

            Bullet? proj = BulletFactory.CreateNew(GunModel.GunComponent.GunPosition);
            proj.InitializeProjectile(GunModel.GunData.ProjectileRadius,
                                      dir * GunModel.GunData.ProjectileSpeed, ~GunModel.GunComponent.Team);

            var effects = new EffectBundle(GunModel.GunComponent.Team, SourceTag.Gun);
            effects.AddEffect(new KnockbackEffect(GunModel.GunComponent.Team, SourceTag.Gun, 50,
                                                  GunModel.GunComponent.GunRotation + Rotation.HalfTurn,
                                                  KnockbackBehavior.Additive));
            // Holder.Effects.Handle(effects);

            int roundsSpent = Math.Clamp(GunModel.GunData.AmmoCostPerShot, 0,
                                         GunModel.CurrentRoundsInMagazine);
            GunModel.CurrentRoundsInMagazine -= roundsSpent;
            
            GunModel.GunViewModel.PublishGunFiredEvent(roundsSpent);

            // Saiga12SingleShot1mSide.Play(0.1f, 0, 0);
        }
    }
}