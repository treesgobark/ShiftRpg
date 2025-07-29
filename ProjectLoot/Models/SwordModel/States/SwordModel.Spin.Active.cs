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
        DurationModule timeoutDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(0.5)));
        SegmentModule  segmentModule   = AddModule(new SegmentModule(timeoutDuration, 5));
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
                                 .AddCircle(32, 0)
                                 .AddSpriteInfo("ThreeQuartersSlash", _duration.Duration);
            _attackDirection = _weaponModel.MeleeWeaponComponent.AttackDirection;
            AddHitEffects();
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
                    EffectBundle newTargetEffects = _hitbox.TargetHitEffects.Clone();
                    _hitbox.TargetHitEffects = newTargetEffects;
                    
                    EffectBundle newHolderEffects = _hitbox.HolderHitEffects.Clone();
                    _hitbox.HolderHitEffects = newHolderEffects;
                        
                    float pitch = Random.Shared.NextSingle(-0.1f, 0.1f);
                    GlobalContent.BladeSwingF.Play(0.15f, pitch, 0);

                    newTargetEffects.UpsertEffect(
                        new KnockTowardEffect
                        {
                            AppliesTo      = ~_weaponModel.MeleeWeaponComponent.Team,
                            Source         = SourceTag.Sword,
                            TargetPosition = _weaponModel.MeleeWeaponComponent.HolderGameplayCenterPosition,
                            Strength       = 200f,
                        }
                    );
                    
                    if (_segmentModule.CurrentSegmentIndex == _segmentModule.TotalSegments - 1)
                    {
                        newTargetEffects.UpsertEffect(
                            new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 25));
                        newTargetEffects.UpsertEffect(
                            new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team,
                                              SourceTag.Sword,
                                              IncreasedHitstopDuration));

                        newTargetEffects.UpsertEffect(
                            new KnockTowardEffect
                            {
                                AppliesTo      = ~_weaponModel.MeleeWeaponComponent.Team,
                                Source         = SourceTag.Sword,
                                TargetPosition = _weaponModel.MeleeWeaponComponent.HolderGameplayCenterPosition,
                                Strength       = -800f,
                            }
                        );

                        newHolderEffects.UpsertEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team,
                                                                        SourceTag.Sword, IncreasedHitstopDuration));
                    }
                }
            }
        }

        public void BeforeDeactivate()
        {
            _hitbox?.Destroy();
        }
        
        private void AddHitEffects()
        {
            EffectBundle targetHitEffects = new();
        
            targetHitEffects.AddEffect(new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
                
            targetHitEffects.AddEffect(new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                         HitstopDuration));

            targetHitEffects.AddEffect(
                new KnockTowardEffect
                {
                    AppliesTo = ~_weaponModel.MeleeWeaponComponent.Team,
                    Source = SourceTag.Sword,
                    TargetPosition = _weaponModel.MeleeWeaponComponent.HolderGameplayCenterPosition,
                    Strength = 100f,
                }
            );
                
            targetHitEffects.AddEffect(new PoiseDamageEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
        
            _hitbox.TargetHitEffects = targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
            _hitbox.HolderHitEffects = holderHitEffects;
        }
    }
}
