using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using FlatRedBall;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;
using ProjectLoot.Entities;

namespace ProjectLoot.Models.SwordModel;

public class SpinWindup : ModularState
{
    public SpinWindup(ITimeManager timeManager, IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
    {
        DurationModule chargeDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(0.25)));
        DurationModule timeoutDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(2)));
        AddModule(new SpinWindupModule(chargeDuration, states, weaponModel));
        AddModule(new DurationExitModule<EmptyState>(timeoutDuration, states));
    }
    
    private class SpinWindupModule : IState
    {
        private readonly IDurationModule _chargeDuration;
        private readonly IReadonlyStateMachine _states;
        private readonly IMeleeWeaponModel _weaponModel;
        private MeleeHitbox? _hitbox;
        private Rotation _attackDirection;

        public SpinWindupModule(IDurationModule chargeDuration, IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
        {
            _chargeDuration = chargeDuration;
            _states         = states;
            _weaponModel    = weaponModel;
        }

        public void OnActivate()
        {
            _hitbox = MeleeHitbox.CreateHitbox(_weaponModel)
                                 .AddCircle(32)
                                 .AddSpriteInfo("StillSword", _chargeDuration.Duration)
                                 .Build();
            _hitbox.SpriteInstance.RelativeX                     = 20;
            _hitbox.SpriteInstance.ParentRotationChangesPosition = true;
            _hitbox.IsActive                                     = false;

            _attackDirection = _weaponModel.MeleeWeaponComponent.AttackDirection;
        }

        public void CustomActivity()
        {
            if (_hitbox is not null)
            {
                _hitbox.RelativeRotationZ = (-_chargeDuration.NormalizedProgress * Rotation.QuarterTurn + _attackDirection - Rotation.EighthTurn).NormalizedRadians;
            }
        }

        public IState? EvaluateExitConditions()
        {
            if (_weaponModel.MeleeWeaponComponent.MeleeWeaponInputDevice.HeavyAttack.WasJustReleased)
            {
                if (_chargeDuration.HasDurationCompleted)
                {
                    return _states.Get<SpinActive>();
                }

                return EmptyState.Instance;
            }

            return null;
        }

        public void BeforeDeactivate()
        {
            _hitbox?.Destroy();
        }
    }
}
