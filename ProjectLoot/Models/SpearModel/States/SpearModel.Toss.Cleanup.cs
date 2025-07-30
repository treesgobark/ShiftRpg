using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Controllers;

namespace ProjectLoot.Models.SpearModel;

partial class SpearModel
{
    private class TossCleanup : IState
    {
        private readonly Toss _tossState;

        public TossCleanup(Toss tossState)
        {
            _tossState = tossState;
        }

        public void OnActivate()
        {
            _tossState.Hitbox?.Destroy();
        }

        public void Update()
        {
        }

        public IState? EvaluateExitConditions()
        {
            return EmptyState.Instance;
        }

        public void BeforeDeactivate()
        {
        }
    }
}