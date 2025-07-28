using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models;

partial class SwordModel
{
    private class SpinWindup : ModularState
    {
        public SpinWindup(ITimeManager timeManager, IReadonlyStateMachine states, IMeleeWeaponInputDevice inputDevice)
        {
            DurationModule chargeDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(0.5)));
            DurationModule timeoutDuration = AddModule(new DurationModule(timeManager, TimeSpan.FromSeconds(2)));
            AddModule(new SpinWindupModule(chargeDuration, states, inputDevice));
            AddModule(new DurationExitModule<EmptyState>(timeoutDuration, states));
            AddModule(new LoggingModule(" " + nameof(SpinWindup)));
        }
    }

    private class SpinWindupModule : IState
    {
        private readonly IDurationModule _chargeDuration;
        private readonly IReadonlyStateMachine _states;
        private readonly IMeleeWeaponInputDevice _inputDevice;

        public SpinWindupModule(IDurationModule chargeDuration, IReadonlyStateMachine states, IMeleeWeaponInputDevice inputDevice)
        {
            _chargeDuration = chargeDuration;
            _states         = states;
            _inputDevice    = inputDevice;
        }
        
        public void OnActivate() { }

        public void CustomActivity() { }

        public IState? EvaluateExitConditions()
        {
            if (_inputDevice.HeavyAttack.WasJustReleased)
            {
                if (_chargeDuration.HasDurationCompleted)
                {
                    return _states.Get<SpinActive>();
                }

                return EmptyState.Instance;
            }

            return null;
        }

        public void BeforeDeactivate() { }
    }
}