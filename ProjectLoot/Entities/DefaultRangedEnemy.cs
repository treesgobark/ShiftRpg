using ANLG.Utilities.FlatRedBall.States;
using FlatRedBall.Screens;
using ProjectLoot.Components;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;
using ProjectLoot.Screens;

namespace ProjectLoot.Entities
{
    public partial class DefaultRangedEnemy
    {
        public WeaponsComponent Weapons { get; private set; }

        public StateMachine StateMachine { get; protected set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            var eInput = new RangedEnemyInputDevice(this, 100, 25);
            InitializeTopDownInput(eInput);
            EnemyInputDevice = eInput;
            Weapons = new WeaponsComponent(eInput, Team.Enemy, this);
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            StateMachine = new StateMachine();
            StateMachine.Add(new GunMode(this, StateMachine));
            StateMachine.InitializeStartingState<GunMode>();
        }

        private void CustomActivity()
        {
            StateMachine.DoCurrentStateActivity();
        }

        private void CustomDestroy()
        {
            Weapons.Destroy();
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {


        }
    }
}
