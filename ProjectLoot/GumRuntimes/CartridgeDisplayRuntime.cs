using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLoot.DataTypes;
using ProjectLoot.Models;

namespace ProjectLoot.GumRuntimes
{
    public partial class CartridgeDisplayRuntime : ICartridgeDisplay
    {
        private GunClass   _gunClass;
        private SpentState _spentState;

        public GunClass GunClass
        {
            get => _gunClass;
            set
            {
                _gunClass = value;
                CurrentCartridgeTypeState = value switch
                {
                    GunClass.Handgun => CartridgeType.Handgun,
                    GunClass.Rifle   => CartridgeType.Rifle,
                    GunClass.Shotgun => CartridgeType.Shotgun,
                    _                => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
            }
        }

        public SpentState SpentState
        {
            get => _spentState;
            set
            {
                _spentState = value;
                CurrentIsSpentState = value switch
                {
                    SpentState.NotSpent => IsSpent.NotSpent,
                    SpentState.Empty    => IsSpent.Empty,
                    SpentState.Spent    => IsSpent.Spent,
                    _                   => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
            }
        }

        public float XPosition { get => X; set => X = value; }
        public float YPosition { get => Y; set => Y = value; }
        public float ZPosition { get => Z; set => Z = value; }
        
        public float XVelocity { get; set; }
        public float YVelocity { get; set; }
        
        public float YAcceleration { get; set; }
        
        public float ZRotation { get => Rotation; set => Rotation = value; }
        public float ZRotationVelocity { get; set; }
        
        public float DestructionCountdown { get; set; }
        public bool IsVisible { get => Visible; set => Visible = value; }

        partial void CustomInitialize () 
        {
        }
    }
}
