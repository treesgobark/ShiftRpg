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

            Projectile? proj = ProjectileFactory.CreateNew(GunModel.GunComponent.BulletOrigin);
            
            proj.InitializeProjectile(GunModel.GunData.ProjectileRadius,
                                      dir * GunModel.GunData.ProjectileSpeed,
                                      ~GunModel.GunComponent.Team,
                                      GunModel.OnHitTargetEffects,
                                      GunModel.OnHitHolderEffects,
                                      GunModel.HolderEffects);
            
            GunModel.HolderEffects.Handle(GunModel.OnFireHolderEffects);

            int roundsSpent = Math.Clamp(GunModel.GunData.AmmoCostPerShot, 0,
                                         GunModel.CurrentRoundsInMagazine);
            GunModel.CurrentRoundsInMagazine -= roundsSpent;
            
            GunModel.GunViewModel.PublishGunFiredEvent(roundsSpent);

            GlobalContent.Saiga12SingleShot1mSide.Play(0.1f, 0, 0);
        }
    }
}