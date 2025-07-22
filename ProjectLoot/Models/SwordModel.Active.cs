// using System.Diagnostics;
// using ANLG.Utilities.NonStaticUtilities;
// using ANLG.Utilities.States;
// using Microsoft.Xna.Framework;
// using ProjectLoot.Components;
// using ProjectLoot.Components.Interfaces;
// using ProjectLoot.Contracts;
// using ProjectLoot.Effects;
// using ProjectLoot.Entities;
// using ProjectLoot.Factories;
//
// namespace ProjectLoot.Models;
//
// partial class SwordModel
// {
//     protected class Active : TimedState
//     {
//         private SwordModel SwordModel { get; }
//         private IMeleeWeaponComponent MeleeWeaponComponent { get; }
//
//         public Active(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel, IMeleeWeaponComponent meleeWeaponComponent)
//             : base(states, timeManager)
//         {
//             SwordModel                = swordModel;
//             MeleeWeaponComponent = meleeWeaponComponent;
//         }
//
//         private MeleeHitbox? Hitbox { get; set; }
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
//             if (TimeInState > SwordModel.CurrentAttackData.ActiveTimeSpan)
//             {
//                 return States.Get<Recovery>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate(IState? nextState)
//         {
//             Hitbox.Destroy();
//         }
//
//         public override void Uninitialize() { }
//
//         public MeleeHitbox SpawnHitbox()
//         {
//             var hitbox = MeleeHitboxFactory.CreateNew();
//             hitbox.RelativePosition = Vector2ExtensionMethods.ToVector3(SwordModel.CurrentAttackData.HitboxOffset);
//             
//             float leftX   = SwordModel.CurrentAttackData.HitboxSizeX / -2;
//             float rightX  = SwordModel.CurrentAttackData.HitboxSizeX /  2;
//             float topY    = SwordModel.CurrentAttackData.HitboxSizeY /  2;
//             float bottomY = SwordModel.CurrentAttackData.HitboxSizeY / -2;
//             
//             hitbox.PolygonInstance.SetPoint(4, leftX, topY);
//             
//             hitbox.PolygonInstance.SetPoint(0, leftX, topY);
//             hitbox.PolygonInstance.SetPoint(1, rightX, topY);
//             hitbox.PolygonInstance.SetPoint(2, rightX, bottomY);
//             hitbox.PolygonInstance.SetPoint(3, leftX, bottomY);
//
//             hitbox.AttachTo(MeleeWeaponComponent.Holder ?? throw new UnreachableException("idk how this weapon's parent is null but here we are"));
//
//             hitbox.AppliesTo = ~MeleeWeaponComponent.Team;
//             
//             var holderHitEffects = new EffectBundle(MeleeWeaponComponent.Team, SourceTag.Melee);
//             // holderHitEffects.AddEffect(new KnockbackEffect(MeleeWeaponComponent.Team, SourceTag.Melee,
//             //     CurrentAttackData.ForwardMovementVelocity, Rotation.FromRadians(RotationZ), KnockbackBehavior.Additive, true));
//             holderHitEffects.AddEffect(new HitstopEffect(MeleeWeaponComponent.Team, SourceTag.Melee, TimeSpan.FromMilliseconds(150)));
//             
//             hitbox.HolderHitEffects = holderHitEffects;
//             
//             var targetEffects = new EffectBundle(MeleeWeaponComponent.Team, SourceTag.Melee);
//             targetEffects.AddEffect(new DamageEffect(~MeleeWeaponComponent.Team, SourceTag.Melee, SwordModel.CurrentAttackData.Damage));
//             targetEffects.AddEffect(new KnockbackEffect(~MeleeWeaponComponent.Team, SourceTag.Melee, SwordModel.CurrentAttackData.KnockbackVelocity,
//                 Rotation.FromRadians(MeleeWeaponComponent.Holder.RotationZ), KnockbackBehavior.Replacement));
//             targetEffects.AddEffect(new ApplyShatterEffect(~MeleeWeaponComponent.Team, SourceTag.Melee));
//             targetEffects.AddEffect(new WeaknessDamageEffect(~MeleeWeaponComponent.Team, SourceTag.Melee, .2f));
//             targetEffects.AddEffect(new HitstopEffect(MeleeWeaponComponent.Team, SourceTag.Melee, TimeSpan.FromMilliseconds(150)));
//             
//             hitbox.TargetHitEffects = targetEffects;
//
//             return hitbox;
//         }
//     }
// }