using System;
using System.Collections.Generic;
using System.Text;
using ANLG.Utilities.Core.StaticUtilities;
using ANLG.Utilities.FlatRedBall.NonStaticUtilities;
using FlatRedBall;
using FlatRedBall.Input;
using FlatRedBall.Instructions;
using FlatRedBall.AI.Pathfinding;
using FlatRedBall.Graphics.Animation;
using FlatRedBall.Graphics.Particle;
using FlatRedBall.Math.Geometry;
using Microsoft.Xna.Framework;

namespace ProjectLoot.Entities
{
    public partial class Door
    {
        private static float TextureDifferential => 2;
        
        private static float RaisedLeftTexturePixel => 144;
        private static float RaisedRightTexturePixel => 160;
        private static float RaisedTopTexturePixel => 64;
        private static float RaisedBottomTexturePixel => 80;
        private float RaisedEnd1Y => AxisAlignedRectangleInstance.Height / 2f - 8;
        private float RaisedEnd2Y => -AxisAlignedRectangleInstance.Height / 2f + 24;
        private float RaisedMiddleY => 0;
        private float RaisedWallHeight => 16;
        private float RaisedWallY => -AxisAlignedRectangleInstance.Height / 2f + 8;
        
        private static float LoweredLeftTexturePixel => RaisedLeftTexturePixel + TextureDifferential;
        private static float LoweredRightTexturePixel => RaisedRightTexturePixel - TextureDifferential;
        private static float LoweredTopTexturePixel => RaisedTopTexturePixel + TextureDifferential;
        private static float LoweredBottomTexturePixel => RaisedBottomTexturePixel - TextureDifferential;
        private float LoweredEnd1Y => AxisAlignedRectangleInstance.Height  / 2f - 24;
        private float LoweredEnd2Y => -AxisAlignedRectangleInstance.Height / 2f + 8;
        private float LoweredMiddleY => -16;
        private float LoweredWallHeight => 0;
        private float LoweredWallY => -AxisAlignedRectangleInstance.Height / 2f;
        
        private float CurrentLeftTexturePixel => float.Lerp(RaisedLeftTexturePixel, LoweredLeftTexturePixel, NormalizedTime);
        private float CurrentRightTexturePixel => float.Lerp(RaisedRightTexturePixel, LoweredRightTexturePixel, NormalizedTime);
        private float CurrentTopTexturePixel => float.Lerp(RaisedTopTexturePixel, LoweredTopTexturePixel, NormalizedTime);
        private float CurrentBottomTexturePixel => float.Lerp(RaisedBottomTexturePixel, LoweredBottomTexturePixel, NormalizedTime);
        private float CurrentEnd1Y => float.Lerp(RaisedEnd1Y, LoweredEnd1Y, NormalizedTime);
        private float CurrentEnd2Y => float.Lerp(RaisedEnd2Y, LoweredEnd2Y, NormalizedTime);
        private float CurrentMiddleY => float.Lerp(RaisedMiddleY, LoweredMiddleY, NormalizedTime);
        private float CurrentWallHeight => float.Lerp(RaisedWallHeight, LoweredWallHeight, NormalizedTime);
        private float CurrentWallY => float.Lerp(RaisedWallY, LoweredWallY, NormalizedTime);
        
        private float NormalizedTime => ((float)Math.Sin(FrbTimeManager.Instance.TotalGameTime.TotalSeconds * 3) + 1) / 2f;
        
        /// <summary>
        /// Initialization logic which is executed only one time for this Entity (unless the Entity is pooled).
        /// This method is called when the Entity is added to managers. Entities which are instantiated but not
        /// added to managers will not have this method called.
        /// </summary>
        private void CustomInitialize()
        {
            
        }

        private void CustomActivity()
        {
            Animate();
        }

        private void CustomDestroy()
        {
            
        }

        private static void CustomLoadStaticContent(string contentManagerName)
        {
            
        }

        partial void CustomActivityEditMode()
        {
            Animate();
        }

        private void Animate()
        {
            EndSprite1.LeftTexturePixel   = CurrentLeftTexturePixel;
            EndSprite1.RightTexturePixel  = CurrentRightTexturePixel;
            EndSprite1.TopTexturePixel    = CurrentTopTexturePixel;
            EndSprite1.BottomTexturePixel = CurrentBottomTexturePixel;
            EndSprite1.Width              = 16;
            EndSprite1.Height             = 16;
            EndSprite1.RelativeY          = CurrentEnd1Y;
            
            EndSprite2.LeftTexturePixel   = CurrentLeftTexturePixel;
            EndSprite2.RightTexturePixel  = CurrentRightTexturePixel;
            EndSprite2.TopTexturePixel    = CurrentTopTexturePixel;
            EndSprite2.BottomTexturePixel = CurrentBottomTexturePixel;
            EndSprite2.Width              = 16;
            EndSprite2.Height             = 16;
            EndSprite2.RelativeY          = CurrentEnd2Y;
            
            MiddleSprite.TopTexturePixel    = CurrentTopTexturePixel;
            MiddleSprite.BottomTexturePixel = CurrentBottomTexturePixel;
            MiddleSprite.RelativeY          = CurrentMiddleY;
            MiddleSprite.Width              = float.Clamp(AxisAlignedRectangleInstance.Height - 32, 0, float.MaxValue);
            // MiddleSprite.Width              = 16;
            // MiddleSprite.Height             = 16;

            WallSprite.Height    = CurrentWallHeight;
            WallSprite.RelativeY = CurrentWallY;

            ForegroundWallSprite.RelativeY = RaisedWallY;
        }
    }
}
