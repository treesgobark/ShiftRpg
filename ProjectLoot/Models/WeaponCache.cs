using System.Diagnostics;
using ANLG.Utilities.FlatRedBall.StaticUtilities;
using ProjectLoot.Contracts;

namespace ProjectLoot.Models;

public class WeaponCache<TWeapon, TInput> : IWeaponCache<TWeapon, TInput>
    where TWeapon : class, IWeapon<TInput>
{
    private const int MaximumWeaponCount = 3;
    
    public TInput InputDevice { get; private set; }

    public WeaponCache(TInput inputDevice)
    {
        InputDevice = inputDevice;
    }
    
    private int CurrentIndex { get; set; }
    private TWeapon?[] WeaponArray { get; } = new TWeapon?[MaximumWeaponCount];
    public TWeapon? CurrentWeapon => WeaponArray[CurrentIndex];
    public int Count { get; private set; }
    public int MaxWeapons => MaximumWeaponCount;

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            if (_isActive)
            {
                CurrentWeapon?.Equip();
            }
            else
            {
                CurrentWeapon?.Unequip();
            }
        }
    }

    public TWeapon? CycleToNextWeapon() => CycleWeapon(true);
    public TWeapon? CycleToPreviousWeapon() => CycleWeapon(false);

    private TWeapon? CycleWeapon(bool isForward)
    {
        if (Count == 1)
        {
            return CurrentWeapon;
        }

        if (Count == 0)
        {
            return null;
        }
        
        CurrentWeapon.Unequip();

        for (int i = 1; i < MaxWeapons; i++)
        {
            int index = (CurrentIndex + (isForward ? i : -i)).Regulate(MaxWeapons);
            var item  = WeaponArray[index];
            
            if (item is not null)
            {
                SelectWeapon(index);
                return CurrentWeapon;
            }
        }

        throw new UnreachableException("Can't find another weapon to cycle to");
    }

    private void SelectWeapon(int index)
    {
        if (index == CurrentIndex)
        {
            return;
        }
        
        CurrentWeapon?.Unequip();
        CurrentIndex = index;
        CurrentWeapon?.Equip();
    }

    public TWeapon Add(TWeapon weapon)
    {
        if (Count >= MaxWeapons)
        {
            throw new InvalidOperationException("Can't add a weapon above the maximum number");
        }

        Count++;

        for (var i = 0; i < MaxWeapons; i++)
        {
            ref TWeapon? item = ref WeaponArray[i];
            if (item is not null) continue;
            
            item = weapon;
            break;
        }

        weapon.InputDevice = InputDevice;

        return weapon;
    }

    public void RemoveWeapon(int index)
    {
        if (index < 0 || index >= MaxWeapons)
        {
            throw new IndexOutOfRangeException();
        }
        
        WeaponArray[index] = default;
    }

    public void ReplaceWeapon(int index, TWeapon weapon)
    {
        if (index < 0 || index >= MaxWeapons)
        {
            throw new IndexOutOfRangeException();
        }

        if (WeaponArray[index] is null)
        {
            Count++;
        }
        
        WeaponArray[index] = weapon;
    }

    public TWeapon? GetWeaponAt(int index) => WeaponArray[index];
    
    public void Destroy()
    {
        foreach (TWeapon? weapon in WeaponArray)
        {
            weapon?.Destroy();
        }
    }

    public void Activity()
    {
        if (IsActive)
        {
            CurrentWeapon?.Activity();
        }
    }
}