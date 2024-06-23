using GumCoreShared.FlatRedBall.Embedded;
using ProjectLoot.Components;
using ProjectLoot.Effects;
using ProjectLoot.InputDevices;

namespace ProjectLoot.Entities;

public abstract partial class Enemy
{
    public EnemyInputDevice EnemyInputDevice { get; protected set; }
    
    public EffectsComponent Effects { get; private set; }

    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        Effects = new EffectsComponent { Team = Team.Enemy };
        
        PositionedObjectGueWrapper? hudParent = gumAttachmentWrappers[0];
        hudParent.ParentRotationChangesRotation = false;
        
        HealthBarRuntimeInstance.Reset();
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