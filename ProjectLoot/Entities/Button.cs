using ANLG.Utilities.Core;
using ProjectLoot.Components;
using ProjectLoot.Effects;
using ProjectLoot.Handlers;

namespace ProjectLoot.Entities
{
    public partial class Button
    {
        public event Action ButtonPushed;
        
        public EffectsComponent Effects { get; private set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            ButtonPushed += () => 
                GlobalContent.TuningFork.Play(0.3f, Random.Shared.NextSingle(-0.2f, 0.2f), 0);
            Effects      =  new EffectsComponent { Team = Team.Enemy };
            Effects.AddHandler<AttackEffect>(new CustomEventHandler<AttackEffect>(Effects, _ => ButtonPushed()));
        }

        private void CustomActivity()
        {
            Effects.Activity();
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }
    }
}
