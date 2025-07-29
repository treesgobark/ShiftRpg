using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;

namespace ProjectLoot.Models.SwordModel;

public class SpinActive : ModularState
{
    public SpinActive(ITimeManager timeManager, IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
    {
        DurationModule timeoutDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(0.67)));
        SegmentModule  segmentModule   = AddModule(new SegmentModule(timeoutDuration, 8));
        AddModule(new SpinActiveModule(timeoutDuration, weaponModel, segmentModule));
        AddModule(new DurationExitModule<EmptyState>(timeoutDuration, states));
    }
    
    private class SpinActiveModule : IActivate, IActivity, IDeactivate
    {
        private readonly IDurationModule _duration;
        private readonly IMeleeWeaponModel _weaponModel;
        private readonly ISegmentModule _segmentModule;
        private MeleeHitbox? _hitbox;
        private Rotation _attackDirection;

        private TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(25);
        private TimeSpan IncreasedHitstopDuration => TimeSpan.FromMilliseconds(250);

        public SpinActiveModule(IDurationModule duration, IMeleeWeaponModel weaponModel, ISegmentModule segmentModule)
        {
            _duration      = duration;
            _weaponModel   = weaponModel;
            _segmentModule = segmentModule;
        }

        public void OnActivate()
        {
            _hitbox = MeleeHitbox.CreateHitbox(_weaponModel)
                                 .AddCircle(32)
                                 .AddSpriteInfo("ThreeQuartersSlash", _duration.Duration)
                                 .Build();
            _attackDirection = _weaponModel.MeleeWeaponComponent.AttackDirection;
            UpdateHitEffects();
        }

        public void CustomActivity()
        {
            if (_hitbox is not null)
            {
                _hitbox.RelativeRotationZ =
                    (_segmentModule.TotalSegments * Rotation.FullTurn * _duration.NormalizedProgress -
                        Rotation.QuarterTurn - Rotation.EighthTurn + _attackDirection).NormalizedRadians;
                while (_segmentModule.TryHandleSegment())
                {
                    UpdateHitEffects();
                    
                    float pitch = Random.Shared.NextSingle(-0.1f, 0.1f);
                    GlobalContent.BladeSwingF.Play(0.15f, pitch, 0);
                    
                    if (_segmentModule.CurrentSegmentIndex == _segmentModule.TotalSegments - 1)
                    {
                        _hitbox?.TargetHitEffects.UpsertEffect(
                            new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 25));
                        _hitbox?.TargetHitEffects.UpsertEffect(
                            new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team,
                                              SourceTag.Sword,
                                              IncreasedHitstopDuration));

                        _hitbox?.TargetHitEffects.UpsertEffect(
                            new KnockTowardEffect
                            {
                                AppliesTo      = ~_weaponModel.MeleeWeaponComponent.Team,
                                Source         = SourceTag.Sword,
                                TargetPosition = _weaponModel.MeleeWeaponComponent.HolderGameplayCenterPosition,
                                Strength       = -1000f,
                            }
                        );

                        _hitbox?.HolderHitEffects.UpsertEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team,
                                                                                 SourceTag.Sword, IncreasedHitstopDuration));
                    }
                }
            }
        }

        public void BeforeDeactivate()
        {
            _hitbox?.Destroy();
        }
        
        private void UpdateHitEffects()
        {
            _hitbox?.TargetHitEffects.ResetId();
            _hitbox?.HolderHitEffects.ResetId();

            _hitbox?.TargetHitEffects.UpsertEffect(new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 4));
                
            _hitbox?.TargetHitEffects.UpsertEffect(new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                                     HitstopDuration));
                
            _hitbox?.TargetHitEffects.UpsertEffect(new PoiseDamageEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 4));

            _hitbox?.TargetHitEffects.UpsertEffect(
                new KnockTowardEffect
                {
                    AppliesTo      = ~_weaponModel.MeleeWeaponComponent.Team,
                    Source         = SourceTag.Sword,
                    TargetPosition = _weaponModel.MeleeWeaponComponent.HolderGameplayCenterPosition,
                    Strength       = 100f,
                }
            );
        
            _hitbox?.HolderHitEffects.UpsertEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        }
    }
}
