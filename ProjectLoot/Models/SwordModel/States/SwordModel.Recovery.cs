// using ANLG.Utilities.NonStaticUtilities;
// using ANLG.Utilities.States;
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
//         public Recovery(IReadonlyStateMachine states, ITimeManager timeManager, SwordModel swordModel, IMeleeWeaponComponent meleeWeaponComponent) : base(states, timeManager)
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
//                 return _states.Get<Idle>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate(IState? nextState)
//         {
//         }
//
//         public override void Uninitialize() { }
//     }
// }