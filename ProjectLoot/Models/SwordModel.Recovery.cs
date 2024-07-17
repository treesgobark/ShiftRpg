// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
// using ProjectLoot.Components.Interfaces;
//
// namespace ProjectLoot.Models;
//
// partial class SwordModel
// {
//     protected class Recovery : TimedState
//     {
//         private SwordModel SwordModel { get; }
//         private IMeleeWeaponComponent MeleeWeaponComponent { get; }
//
//         public Recovery(IReadonlyStateMachine stateMachine, ITimeManager timeManager, SwordModel swordModel, IMeleeWeaponComponent meleeWeaponComponent) : base(stateMachine, timeManager)
//         {
//             SwordModel                = swordModel;
//             MeleeWeaponComponent = meleeWeaponComponent;
//         }
//
//         public override void Initialize() { }
//
//         protected override void AfterTimedStateActivate() { }
//
//         protected override void AfterTimedStateActivity() { }
//
//         public override IState? EvaluateExitConditions()
//         {
//             if (TimeInState > SwordModel.CurrentAttackData.RecoveryTimeSpan)
//             {
//                 return StateMachine.Get<Idle>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate()
//         {
//         }
//
//         public override void Uninitialize() { }
//     }
// }