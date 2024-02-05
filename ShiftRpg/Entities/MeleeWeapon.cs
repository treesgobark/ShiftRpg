using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Debugging;
using FlatRedBall.Input;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.MeleeWeapon;
using ShiftRpg.DataTypes;
using Point = FlatRedBall.Math.Geometry.Point;

namespace ShiftRpg.Entities;

public abstract partial class MeleeWeapon : IMeleeWeapon, IHasControllers<MeleeWeapon, MeleeWeaponController>
{
    public ControllerCollection<MeleeWeapon, MeleeWeaponController> Controllers { get; protected set; }
    public AttackData CurrentAttackData { get; set; }
    public CyclableList<string> AttackList = new(AttackData.OrderedList);
    
    public Player Owner { get; set; }

    public PolygonSave PolygonSave { get; } = new();

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

    public abstract void BeginAttack();
    public abstract void EndAttack();
        
    public void Equip()
    {
        // PolygonInstance.Visible = true;
    }

    public void Unequip()
    {
        // PolygonInstance.Visible = false;
    }
}