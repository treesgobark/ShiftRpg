using System.Collections.Generic;
using System.ComponentModel;
using FlatRedBall;
using FlatRedBall.Graphics;
using Microsoft.Xna.Framework;
using ProjectLoot.DataTypes;
using ProjectLoot.ViewModels;
using RenderingLibrary;
using IUpdateable = ProjectLoot.Contracts.IUpdateable;

namespace ProjectLoot.Models;

public class MagazineDisplay : IDestroyable, IUpdateable
{
    private int            _currentCount;
    private Vector3        _position;
    private float          _spacing;
    private IGunViewModel? _bindingContext;
    private GunClass       _gunClass;
    private bool           _isVisible;
    private List<ICartridgeDisplay> CartridgeDisplays { get; } = [];
    private List<ICartridgeDisplay> SpentCartridgeDisplays { get; } = [];
    private Func<ICartridgeDisplay> CartridgeDisplayFactory { get; }

    public MagazineDisplay(Func<ICartridgeDisplay> cartridgeDisplayFactory, Vector3 position, int capacity, float spacing, int currentCount = -1)
    {
        CartridgeDisplayFactory = cartridgeDisplayFactory;
        Capacity = capacity;
        CurrentCount = currentCount >= 0 ? currentCount : capacity;
        Spacing = spacing;
        Position = position;
        
        UpdateVisiblity();
    }

    public IGunViewModel? BindingContext
    {
        get => _bindingContext;
        set
        {
            if (_bindingContext is not null)
            {
                _bindingContext.PropertyChanged -= ReactToPropertyChanged;
                _bindingContext.GunFired        -= EjectCartridge;
            }

            _bindingContext = value;
            
            if (_bindingContext is not null)
            {
                _bindingContext.PropertyChanged += ReactToPropertyChanged;
                _bindingContext.GunFired        += EjectCartridge;
            }
        }
    }

    private void ReactToPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is not IGunViewModel gunViewModel)
        {
            return;
        }
        
        switch (e.PropertyName)
        {
            case "CurrentMagazineCount":
                CurrentCount = gunViewModel.CurrentMagazineCount;
                break;
            case "MaximumMagazineCount":
                Capacity = gunViewModel.MaximumMagazineCount;
                break;
            case "GunClass":
                GunClass = gunViewModel.GunClass;
                break;
            case "IsEquipped":
                IsVisible = gunViewModel.IsEquipped;
                break;
        }
    }

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            RecalculatePositions();
        }
    }

    public int Capacity
    {
        get => CartridgeDisplays.Count;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Magazine capacity cannot be negative");
            }
            
            while (CartridgeDisplays.Count < value)
            {
                CartridgeDisplays.Add(CartridgeDisplayFactory());
            }
            
            while (CartridgeDisplays.Count > value)
            {
                CartridgeDisplays[^1].Destroy();
                CartridgeDisplays.RemoveAt(CartridgeDisplays.Count - 1);
            }
            
            RecalculateStates();
            RecalculatePositions();
            UpdateVisiblity();
        }
    }

    public int CurrentCount
    {
        get => _currentCount;
        set
        {
            if (value > Capacity)
            {
                _currentCount = Capacity;
            }
            else if (value < 0)
            {
                _currentCount = 0;
            }
            else
            {
                _currentCount = value;
            }
            
            RecalculateStates();
        }
    }

    public float Spacing
    {
        get => _spacing;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentException("Spacing must be positive");
            }
            
            _spacing = value;
            RecalculatePositions();
        }
    }

    public GunClass GunClass
    {
        get => _gunClass;
        set
        {
            if (_gunClass != value)
            {
                Spacing = value switch
                {
                    GunClass.Handgun => 9,
                    GunClass.Rifle   => 6,
                    GunClass.Shotgun => 11,
                    _                => throw new ArgumentOutOfRangeException(nameof(value), value, null)
                };
            }
            
            _gunClass = value;
            foreach (ICartridgeDisplay cartridgeDisplay in CartridgeDisplays)
            {
                cartridgeDisplay.GunClass = GunClass;
            }
            
            RecalculateStates();
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            _isVisible = value;
            UpdateVisiblity();
        }
    }

    public void EjectCartridge(int numberOfCartridges = 1)
    {
        for (int i = 1; i <= numberOfCartridges; i++)
        {
            ICartridgeDisplay cart = CartridgeDisplayFactory();
        
            Vector3 pos = GetPosition(CurrentCount - i);
            cart.XPosition = pos.X;
            cart.YPosition = pos.Y;
            cart.ZPosition = pos.Z;
            cart.GunClass  = GunClass;
            Spend(cart, -64, -128, 256, 1440, 0.2f);
        
            SpentCartridgeDisplays.Add(cart);

            if (BindingContext is not null)
            {
                BindingContext.CurrentMagazineCount = CurrentCount;
            }
        }
    }

    private void Spend(ICartridgeDisplay cartridgeDisplay, float xVelocity, float yVelocity, float gravity, float rotationSpeed, float randomizeTolerance = 0f)
    {
        RandomizeByTolerance(ref xVelocity, randomizeTolerance);
        RandomizeByTolerance(ref yVelocity, randomizeTolerance);
        // RandomizeByTolerance(ref gravity, randomizeTolerance);
        RandomizeByTolerance(ref rotationSpeed, randomizeTolerance);
            
        cartridgeDisplay.XVelocity            = xVelocity;
        cartridgeDisplay.YVelocity            = yVelocity;
        cartridgeDisplay.YAcceleration        = gravity;
        cartridgeDisplay.ZRotationVelocity    = rotationSpeed;
        cartridgeDisplay.DestructionCountdown = 2;
        cartridgeDisplay.SpentState           = SpentState.Spent;
    }

    private static void RandomizeByTolerance(ref float value, float tolerance)
    {
        value = MathHelper.Lerp(value * (1 - tolerance), value * (1 + tolerance), Random.Shared.NextSingle());
    }

    private void RecalculatePositions()
    {
        for (var index = 0; index < CartridgeDisplays.Count; index++)
        {
            ICartridgeDisplay cartridge = CartridgeDisplays[index];
            Vector3 pos = GetPosition(index);
            cartridge.XPosition = pos.X;
            cartridge.YPosition = pos.Y;
            cartridge.ZPosition = pos.Z;
        }
    }

    private void RecalculateStates()
    {
        for (var index = 0; index < CartridgeDisplays.Count; index++)
        {
            ICartridgeDisplay cartridge = CartridgeDisplays[index];
            
            cartridge.GunClass = GunClass;
            
            if (CurrentCount > index)
            {
                cartridge.SpentState = SpentState.NotSpent;
            }
            else
            {
                cartridge.SpentState = SpentState.Empty;
            }
        }
    }

    private void UpdateVisiblity()
    {
        for (var index = 0; index < CartridgeDisplays.Count; index++)
        {
            ICartridgeDisplay cartridge = CartridgeDisplays[index];

            cartridge.IsVisible = IsVisible;
        }
    }
    
    private Vector3 GetPosition(int index)
    {
        return Position.AddX(index * Spacing);
    }

    public void Destroy()
    {
        foreach (var cartridge in CartridgeDisplays)
        {
            cartridge.Destroy();
        }
        
        foreach (var cartridge in SpentCartridgeDisplays)
        {
            cartridge.Destroy();
        }
    }

    public void Activity()
    {
        for (var index = SpentCartridgeDisplays.Count - 1; index >= 0; index--)
        {
            ICartridgeDisplay cartridge = SpentCartridgeDisplays[index];
            cartridge.YVelocity += cartridge.YAcceleration * TimeManager.SecondDifference;
            cartridge.YPosition += cartridge.YVelocity * TimeManager.SecondDifference;

            cartridge.XPosition += cartridge.XVelocity * TimeManager.SecondDifference;

            cartridge.ZRotation += cartridge.ZRotationVelocity * TimeManager.SecondDifference;

            if (cartridge.DestructionCountdown > 0)
            {
                cartridge.DestructionCountdown -= TimeManager.SecondDifference;
                if (cartridge.DestructionCountdown <= 0)
                {
                    cartridge.Destroy();
                    SpentCartridgeDisplays.RemoveAt(index);
                }
            }
        }
    }
}

public interface ICartridgeDisplay : IDestroyable
{
    GunClass GunClass { get; set; }
    SpentState SpentState { get; set; }
    
    float XPosition { get; set; }
    float YPosition { get; set; }
    float ZPosition { get; set; }
    
    float XVelocity { get; set; }
    float YVelocity { get; set; }
    
    float YAcceleration { get; set; }
    
    float ZRotation { get; set; }
    
    float ZRotationVelocity { get; set; }
    float DestructionCountdown { get; set; }
    
    bool IsVisible { get; set; }
}

public enum SpentState
{
    NotSpent,
    Empty,
    Spent,
}