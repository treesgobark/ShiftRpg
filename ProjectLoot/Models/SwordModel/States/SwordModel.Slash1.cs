using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;
using ProjectLoot.Effects;
using ProjectLoot.Effects.Base;
using ProjectLoot.Entities;

namespace ProjectLoot.Models.SwordModel;

public class Slash1 : ParentedTimedState<SwordModel>
{
    private readonly IReadonlyStateMachine _states;
    private static TimeSpan Duration => TimeSpan.FromMilliseconds(120);
    private static TimeSpan HitstopDuration => TimeSpan.FromMilliseconds(50);
    private float NormalizedProgress => (float)(TimeInState / Duration);

    private MeleeHitbox? Hitbox { get; set; }
    private Rotation AttackDirection { get; set; }
    private Rotation HitboxStartDirection => AttackDirection - Rotation.QuarterTurn;
    
    private static int TotalSegments => 1;
    private int SegmentsHandled { get; set; }
    private int GoalSegmentsHandled => Math.Clamp((int)(NormalizedProgress * TotalSegments) + 1, 0, TotalSegments);
    
    private IState? NextState { get; set; }
    
    public Slash1(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel parent)
        : base(timeManager, parent)
    {
        _states = states;
    }
    
    protected override void AfterTimedStateActivate()
    {
        SegmentsHandled = 0;
        
        NextState = null;

        AttackDirection = Parent.MeleeWeaponComponent.AttackDirection;

        Hitbox = MeleeHitbox.CreateHitbox(Parent)
                            .AddCircle(10, 20)
                            .AddCircle(2,  8)
                            .AddSpriteInfo("ThreeEighthsSlash", Duration)
                            .Build();

        GlobalContent.BladeSwingA.Play(0.1f, 0, 0);
        GlobalContent.WhooshA.Play(0.2f, 0, 0);
    }

    public override IState? EvaluateExitConditions()
    {
        if (TimeInState > TimeSpan.Zero && Parent.MeleeWeaponComponent.MeleeWeaponInputDevice.LightAttack.WasJustPressed)
        {
            NextState = _states.Get<Slash2>();
        }
        
        if (TimeInState >= Duration)
        {
            if (!Parent.IsEquipped)
            {
                return _states.Get<NotEquipped>();
            }

            if (NextState is not null)
            {
                return NextState;
            }

            return _states.Get<Slash1Recovery>();
        }

        return null;
    }

    protected override void AfterTimedStateActivity()
    {
        Hitbox.RelativeRotationZ =
            (HitboxStartDirection + Rotation.HalfTurn * NormalizedProgress).NormalizedRadians;
        Hitbox.SpriteInstance.Alpha = 1f - NormalizedProgress;

        if (SegmentsHandled < GoalSegmentsHandled)
        {
            EffectBundle targetHitEffects = new();
    
            targetHitEffects.AddEffect(new AttackEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
            
            targetHitEffects.AddEffect(new HitstopEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword,
                                                         HitstopDuration));

            targetHitEffects.AddEffect(
                new KnockbackEffect(
                    ~Parent.MeleeWeaponComponent.Team,
                    SourceTag.Sword,
                    450,
                    AttackDirection + Rotation.EighthTurn / 2,
                    KnockbackBehavior.Replacement
                )
            );
            
            targetHitEffects.AddEffect(new PoiseDamageEffect(~Parent.MeleeWeaponComponent.Team, SourceTag.Sword, 10));
    
            Hitbox.TargetHitEffects = targetHitEffects;
            
            EffectBundle holderHitEffects = new();
    
            holderHitEffects.AddEffect(new HitstopEffect(Parent.MeleeWeaponComponent.Team, SourceTag.Sword, HitstopDuration));
    
            Hitbox.HolderHitEffects = holderHitEffects;

            SegmentsHandled++;
        }
    }

    public override void BeforeDeactivate()
    {
        Hitbox?.Destroy();
    }
}
