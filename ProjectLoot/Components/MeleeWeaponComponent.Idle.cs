// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
// using ProjectLoot.Controllers;
//
// namespace ProjectLoot.Components;
//
// partial class MeleeWeaponComponent
// {
//     protected class Idle : ParentedTimedState<MeleeWeaponComponent>
//     {
//         public Idle(MeleeWeaponComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager)
//             : base(parent, stateMachine, timeManager)
//         {
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
//             if (Parent.InputDevice.Attack.WasJustPressed)
//             {
//                 return StateMachine.Get<Startup>();
//             }
//
//             return null;
//         }
//
//         public override void BeforeDeactivate() { }
//
//         public override void Uninitialize() { }
//     }
// }
