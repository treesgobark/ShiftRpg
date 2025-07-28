using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class SpinActive : ModularState
    {
        public SpinActive(ITimeManager timeManager, IReadonlyStateMachine states, IMeleeWeaponModel weaponModel)
        {
            DurationModule timeoutDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(0.5)));
            SegmentModule  segmentModule   = AddModule(new SegmentModule(timeoutDuration, 6));
            AddModule(new SpinActiveModule(timeoutDuration, weaponModel, segmentModule));
            AddModule(new DurationExitModule<EmptyState>(timeoutDuration, states));
            AddModule(new LoggingModule(" " + nameof(SpinActive)));
        }
    }

    private class SpinActiveModule : IState
    {
        private readonly IDurationModule _duration;
        private readonly IMeleeWeaponModel _weaponModel;
        private readonly ISegmentModule _segmentModule;
        private MeleeHitbox? _hitbox;

        private TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(25);
        private TimeSpan IncreasedHitstopDuration => TimeSpan.FromMilliseconds(150);

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
            AddHitEffects();
        }

        public void CustomActivity()
        {
            if (_hitbox is not null)
            {
                _hitbox.RotationZ = (_segmentModule.TotalSegments * Rotation.FullTurn * _duration.NormalizedProgress).NormalizedRadians;
                while (_segmentModule.TryHandleSegment())
                {
                    EffectBundle newTargetEffects = _hitbox.TargetHitEffects.Clone();
                    _hitbox.TargetHitEffects = newTargetEffects;
                    
                    EffectBundle newHolderEffects = _hitbox.HolderHitEffects.Clone();
                    _hitbox.HolderHitEffects = newHolderEffects;
                        
                    float pitch = Random.Shared.NextSingle(-0.1f, 0.1f);
                    GlobalContent.BladeSwingF.Play(0.15f, pitch, 0);
                    
                    if (_segmentModule.CurrentSegmentIndex == _segmentModule.TotalSegments - 1)
                    {
                        newTargetEffects.UpsertEffect(
                            new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 25));
                        newTargetEffects.UpsertEffect(
                            new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team,
                                              SourceTag.Sword,
                                              IncreasedHitstopDuration));

                        newTargetEffects.UpsertEffect(
                            new KnockbackEffect(
                                ~_weaponModel.MeleeWeaponComponent.Team,
                                SourceTag.Sword,
                                1000,
                                Rotation.Zero,
                                KnockbackBehavior.Replacement,
                                relativeDirection: true
                            )
                        );

                        newHolderEffects.UpsertEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team,
                                                                        SourceTag.Sword, IncreasedHitstopDuration));
                    }
                }
            }
        }

        public IState? EvaluateExitConditions() => null;

        public void BeforeDeactivate()
        {
            _hitbox?.Destroy();
        }
        
        private void AddHitEffects()
        {
            EffectBundle targetHitEffects = new();
        
            targetHitEffects.AddEffect(new AttackEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 6));
                
            targetHitEffects.AddEffect(new HitstopEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                         HitstopDuration));

            targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~_weaponModel.MeleeWeaponComponent.Team,
                    SourceTag.Sword,
                    50,
                    Rotation.EighthTurn / 2,
                    KnockbackBehavior.Replacement
                )
            );
                
            targetHitEffects.AddEffect(new PoiseDamageEffect(~_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
        
            _hitbox.TargetHitEffects = targetHitEffects;
                
            EffectBundle holderHitEffects = new();
        
            holderHitEffects.AddEffect(new HitstopEffect(_weaponModel.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
        
            _hitbox.HolderHitEffects = holderHitEffects;
        }
    }
}