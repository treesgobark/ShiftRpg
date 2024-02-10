using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Debugging;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.MeleeWeapon;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;
using ShiftRpg.InputDevices;
using Point = FlatRedBall.Math.Geometry.Point;

namespace ShiftRpg.Entities;

public abstract partial class MeleeWeapon : IMeleeWeapon, IHasControllers<MeleeWeapon, MeleeWeaponController>
{
    public AttackData CurrentAttackData { get; set; }
    public readonly CyclableList<string> AttackList = new(AttackData.OrderedList);
    public Player Owner { get; set; }
    public PolygonSave PolygonSave { get; } = new();
    public bool IsAttacking { get; protected set; }
    public IMeleeWeaponInputDevice InputDevice { get; set; }

    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        PolygonSave.Points =
        [
            new Point(0, 0),
            new Point(0, 0),
            new Point(0, 0),
            new Point(0, 0),
            new Point(0, 0),
        ];
        PolygonSave.MapShapeRelative(PolygonInstance);
        AttackList.CycleToPreviousItem();
        CurrentAttackData = GlobalContent.AttackData[AttackList.CycleToNextItem()];
    }

    private void CustomActivity()
    {
        if (InputManager.Mouse.ScrollWheelChange > 0)
        {
            CurrentAttackData = GlobalContent.AttackData[AttackList.CycleToNextItem()];
            Debugger.CommandLineWrite($"Switched to {CurrentAttackData.Name}");
        }
        else if (InputManager.Mouse.ScrollWheelChange < 0)
        {
            CurrentAttackData = GlobalContent.AttackData[AttackList.CycleToPreviousItem()];
            Debugger.CommandLineWrite($"Switched to {CurrentAttackData.Name}");
        }
    }

    private void CustomDestroy()
    {


    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {


    }
    
    public Team Team { get; set; }
    public SourceTag Source { get; set; } = SourceTag.Melee;

    // Implement IHasControllers
    
    public ControllerCollection<MeleeWeapon, MeleeWeaponController> Controllers { get; protected set; }
    
    // Implement IMeleeWeapon

    public Action<IReadOnlyList<IEffect>> ApplyHolderEffects { get; set; }
    public Action<IReadOnlyList<IEffect>> ModifyTargetEffects { get; set; }
    public IReadOnlyList<IEffect> TargetHitEffects { get; set; } = new List<IEffect>();
    public IReadOnlyList<IEffect> HolderHitEffects { get; set; } = new List<IEffect>();

    public void Equip(IMeleeWeaponInputDevice inputDevice)
    {
        InputDevice = inputDevice;
    }

    public void Unequip()
    {
        InputDevice = ZeroMeleeWeaponInputDevice.Instance;
    }
}