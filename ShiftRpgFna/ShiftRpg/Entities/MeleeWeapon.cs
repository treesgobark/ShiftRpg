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
    public PolygonSave PolygonSave { get; } = new();
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
        TargetHitEffects              = EffectBundle.Empty;
        HolderHitEffects              = EffectBundle.Empty;
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

    private void CustomDestroy() { }

    private static void CustomLoadStaticContent(string contentManagerName) { }

    public Team Team { get; set; }
    public SourceTag Source { get; set; } = SourceTag.Melee;
    
    // Implement IMeleeWeapon

    public IWeaponHolder Holder { get; set; }
    public IEffectBundle TargetHitEffects { get; set; }
    public IEffectBundle HolderHitEffects { get; set; }

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
}