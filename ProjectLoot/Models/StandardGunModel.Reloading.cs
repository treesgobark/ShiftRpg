using ANLG.Utilities.Core.NonStaticUtilities;
using ANLG.Utilities.Core.States;
using ProjectLoot.Components;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Controllers;
using ProjectLoot.ViewModels;

namespace ProjectLoot.Models;

public partial class StandardGunModel
{
    private class Reloading : TimedState
    {
        private StandardGunModel GunModel { get; }

        public Reloading(IReadonlyStateMachine stateMachine, ITimeManager  timeManager, StandardGunModel gunModel)
            : base(stateMachine, timeManager)
        {
            GunModel  = gunModel;
        }

        public override void Initialize() { }

        protected override void AfterTimedStateActivate() { }
    
        public override IState? EvaluateExitConditions()
        {
            if (TimeInState > GunModel.GunData.ReloadTimeSpan)
            {
                GunModel.CurrentRoundsInMagazine           = GunModel.GunData.MagazineSize;
                GunModel.GunViewModel.CurrentMagazineCount = GunModel.GunData.MagazineSize;
                return StateMachine.Get<Ready>();
            }
    
            return null;
        }

        protected override void AfterTimedStateActivity()
        {
        }
    
        public override void BeforeDeactivate()
        {
        }

        public override void Uninitialize() { }
    }
}
