// using System.Diagnostics;
// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
// using Microsoft.Xna.Framework;
// using ProjectLoot.Contracts;
// using ProjectLoot.Controllers;
// using ProjectLoot.Effects;
// using ProjectLoot.Entities;
// using ProjectLoot.Factories;
//
// namespace ProjectLoot.Components;
//
// partial class MeleeWeaponComponent
// {
//     protected class Active : ParentedTimedState<MeleeWeaponComponent>
//     {
//         public Active(MeleeWeaponComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
//             : base(parent, stateMachine, timeManager) { }
//
//         protected MeleeHitbox Hitbox { get; set; }
//         
//         public override void Initialize() { }
//
//         protected override void AfterTimedStateActivate()
//         {
//             Hitbox = SpawnHitbox();
//         }
//
//         protected override void AfterTimedStateActivity() { }
//
//         public override IState? EvaluateExitConditions()
//         {
//             if (TimeInState > Parent.CurrentAttackData.ActiveTimeSpan)
//             {
//                 return StateMachine.Get<Recovery>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate()
//         {
//             Hitbox.Destroy();
//         }
//
//         public override void Uninitialize() { }
//
//         public MeleeHitbox SpawnHitbox()
//         {
//             var hitbox = MeleeHitboxFactory.CreateNew();
//             hitbox.RelativePosition = Vector2ExtensionMethods.ToVector3(CurrentAttackData.HitboxOffset);
//             
//             float leftX   = CurrentAttackData.HitboxSizeX / -2;
//             float rightX  = CurrentAttackData.HitboxSizeX /  2;
//             float topY    = CurrentAttackData.HitboxSizeY /  2;
//             float bottomY = CurrentAttackData.HitboxSizeY / -2;
//             
//             hitbox.PolygonInstance.SetPoint(4, leftX, topY);
//             
//             hitbox.PolygonInstance.SetPoint(0, leftX, topY);
//             hitbox.PolygonInstance.SetPoint(1, rightX, topY);
//             hitbox.PolygonInstance.SetPoint(2, rightX, bottomY);
//             hitbox.PolygonInstance.SetPoint(3, leftX, bottomY);
//
//             hitbox.Holder = Holder;
//             hitbox.AttachTo(Parent ?? throw new UnreachableException("idk how this weapon's parent is null but here we are"));
//
//             hitbox.AppliesTo = ~Effects.Team;
//             
//             var holderHitEffects = new EffectBundle(Effects.Team, Effects.Source);
//             // holderHitEffects.AddEffect(new KnockbackEffect(Effects.Team, Effects.Source,
//             //     CurrentAttackData.ForwardMovementVelocity, Rotation.FromRadians(RotationZ), KnockbackBehavior.Additive, true));
//             holderHitEffects.AddEffect(new HitstopEffect(Effects.Team, Effects.Source, TimeSpan.FromMilliseconds(150)));
//             
//             hitbox.HolderHitEffects = holderHitEffects;
//             
//             var targetEffects = new EffectBundle(Effects.Team, Effects.Source);
//             targetEffects.AddEffect(new DamageEffect(~Effects.Team, Effects.Source, CurrentAttackData.Damage));
//             targetEffects.AddEffect(new KnockbackEffect(~Effects.Team, Effects.Source, CurrentAttackData.KnockbackVelocity,
//                 Rotation.FromRadians(RotationZ), KnockbackBehavior.Replacement));
//             targetEffects.AddEffect(new ApplyShatterEffect(~Effects.Team, Effects.Source));
//             targetEffects.AddEffect(new WeaknessDamageEffect(~Effects.Team, Effects.Source, .2f));
//             targetEffects.AddEffect(new HitstopEffect(~Effects.Team, Effects.Source, TimeSpan.FromMilliseconds(150)));
//             
//             hitbox.TargetHitEffects = targetEffects;
//
//             return hitbox;
//         }
//     }
// }