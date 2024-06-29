// using ANLG.Utilities.Core.NonStaticUtilities;
// using ANLG.Utilities.Core.States;
// using ProjectLoot.Controllers;
//
// namespace ProjectLoot.Components;
//
// public partial class MeleeWeaponComponent
// {
//     protected class Startup : ParentedTimedState<MeleeWeaponComponent>
//     {
//         public Startup(MeleeWeaponComponent parent, IReadonlyStateMachine stateMachine, ITimeManager timeManager) : base(parent, stateMachine, timeManager) { }
//         
//         public override void Initialize() { }
//
//         protected override void AfterTimedStateActivate()
//         {
//             Parent.Holder.SetInputEnabled(false);
//         }
//
//         protected override void AfterTimedStateActivity()
//         {
//         }
//
//         public override IState? EvaluateExitConditions()
//         {
//             if (TimeInState > Parent.CurrentAttackData.StartupTimeSpan)
//             {
//                 return StateMachine.Get<Active>();
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