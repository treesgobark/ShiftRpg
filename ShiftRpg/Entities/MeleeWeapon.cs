using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.FlatRedBall.Controllers;
using ANLG.Utilities.FlatRedBall.Extensions;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Content.Polygon;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;
using ShiftRpg.Contracts;
using ShiftRpg.Controllers.MeleeWeapon;
using ShiftRpg.DataTypes;
using Point = FlatRedBall.Math.Geometry.Point;

namespace ShiftRpg.Entities;

public abstract partial class MeleeWeapon : IMeleeWeapon, IHasControllers<MeleeWeapon, MeleeWeaponController>
{
    public ControllerCollection<MeleeWeapon, MeleeWeaponController> Controllers { get; protected set; }
    public AttackData CurrentAttackData { get; set; }
    
    public Player Owner { get; set; }

    private readonly PolygonSave _polygonSave = new();
    
    /// <summary>
    /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
    /// This method is called when the Entity is added to managers. Entities which are instantiated but not
    /// added to managers will not have this method called.
    /// </summary>
    private void CustomInitialize()
    {
        var data = GlobalContent.AttackData[AttackData.DefaultSwordSlash];
        _polygonSave.Points =
        [
            new Point(data.HitboxOffsetX, data.HitboxOffsetY + data.HitboxSizeY / 2),
            new Point(data.HitboxOffsetX + data.HitboxSizeX, data.HitboxOffsetY + data.HitboxSizeY / 2),
            new Point(data.HitboxOffsetX + data.HitboxSizeX, data.HitboxOffsetY - data.HitboxSizeY / 2),
            new Point(data.HitboxOffsetX, data.HitboxOffsetY - data.HitboxSizeY / 2),
            new Point(data.HitboxOffsetX, data.HitboxOffsetY + data.HitboxSizeY / 2),
        ];
        _polygonSave.MapShapeRelative(PolygonInstance);
    }

    private void CustomActivity()
    {


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