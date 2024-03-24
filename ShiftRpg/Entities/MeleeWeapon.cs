using System;
using System.Collections.Generic;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Debugging;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.DataTypes;
using ShiftRpg.Effects;
using ShiftRpg.InputDevices;
using Point = FlatRedBall.Math.Geometry.Point;

namespace ShiftRpg.Entities;

public abstract partial class MeleeWeapon : IMeleeWeapon
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
        CurrentAttackData             = GlobalContent.AttackData[AttackList.CycleToNextItem()];
        ParentRotationChangesRotation = true;
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
    
    public void ShowHitbox(bool isVisible)
    {
        PolygonInstance.Visible = isVisible;
    }

    public void PrepareAttackData(AttackData data, IReadOnlyList<IEffect> holderEffects,
        IReadOnlyList<IEffect> targetHitEffects)
    {
        PolygonSave.Points[0].X = data.HitboxOffsetX;
        PolygonSave.Points[4].X = data.HitboxOffsetX;
        PolygonSave.Points[0].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        PolygonSave.Points[4].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        
        PolygonSave.Points[1].X = data.HitboxOffsetX + data.HitboxSizeX;
        PolygonSave.Points[1].Y = data.HitboxOffsetY + data.HitboxSizeY / 2;
        
        PolygonSave.Points[2].X = data.HitboxOffsetX + data.HitboxSizeX;
        PolygonSave.Points[2].Y = data.HitboxOffsetY - data.HitboxSizeY / 2;
        
        PolygonSave.Points[3].X = data.HitboxOffsetX;
        PolygonSave.Points[3].Y = data.HitboxOffsetY - data.HitboxSizeY / 2;
        
        PolygonSave.MapShapeRelative(PolygonInstance);
    }
}