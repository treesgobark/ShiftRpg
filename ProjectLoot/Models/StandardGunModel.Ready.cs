using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ANLG.Utilities.FlatRedBall.Extensions;
using ProjectLoot.Entities;
using ProjectLoot.Factories;

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
            for (int i = 1; i <= GunModel.GunData.ProjectileCount; i++)
            {
                Rotation coneSize          = Rotation.FromDegrees(GunModel.GunData.ProjectileSpreadDegrees);
                float    accuracyValue     = AccuracyTransform(i / (float)(GunModel.GunData.ProjectileCount + 1));
                Rotation aimDifferential   = coneSize * accuracyValue          - coneSize / 2;
                Rotation finalAimDirection = GunModel.GunComponent.GunRotation + aimDifferential;

                var aimVector = finalAimDirection.ToVector3();

                Projectile? proj = ProjectileFactory.CreateNew(GunModel.GunComponent.BulletOrigin);
            
                proj.InitializeProjectile(GunModel.GunData.ProjectileRadius,
                                          aimVector * GunModel.GunData.ProjectileSpeed,
                                          ~GunModel.GunComponent.Team,
                                          GunModel.OnHitTargetEffects,
                                          GunModel.OnHitHolderEffects,
                                          GunModel.HolderEffects);
            }
            
            int roundsSpent = Math.Clamp(GunModel.GunData.AmmoCostPerShot, 0,
                                         GunModel.CurrentRoundsInMagazine);
            
            GunModel.HolderEffects.Handle(GunModel.OnFireHolderEffects);
            GunModel.CurrentRoundsInMagazine -= roundsSpent;
            
            GunModel.GunViewModel.PublishGunFiredEvent(roundsSpent);

            GlobalContent.Saiga12SingleShot1mSide.Play(0.1f, 0, 0);
        }

        private static float AccuracyTransform(float t)
        {
            return t switch
            {
                < 0    => 0,
                > 1    => 1,
                < 0.5f => MathF.Sqrt(0.25f - (t - 0.5f) * (t - 0.5f)),
                _      => -MathF.Sqrt(0.25f - (t - 0.5f) * (t - 0.5f)) + 1
            };
        }
    }
}