using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.FlatRedBall.Controllers;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ShiftRpg.Controllers.DefaultSword;

namespace ShiftRpg.Entities
{
    public partial class DefaultSword : IHasControllers<DefaultSword, DefaultSwordController>
    {
        public ControllerCollection<DefaultSword, DefaultSwordController> Controllers => DefaultSwordControllers;
        public DefaultSwordControllerCollection DefaultSwordControllers { get; set; }
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            DefaultSwordControllers = new DefaultSwordControllerCollection();
            Controllers.Add(new DefaultSwordController(this));
            Controllers.Add(new Active(this));
            Controllers.InitializeStartingController<DefaultSwordController>();
        }

        private void CustomActivity()
        {
            Controllers.DoCurrentControllerActivity();
        }

        private void CustomDestroy()
        {
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
        }

        public override void BeginAttack() => DefaultSwordControllers.CurrentController.BeginAttack();
        public override void EndAttack() => DefaultSwordControllers.CurrentController.EndAttack();
    }
}
