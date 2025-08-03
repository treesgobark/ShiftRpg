using ANLG.Utilities.Core;
using ANLG.Utilities.States;
using ProjectLoot.Components.Interfaces;
using ProjectLoot.Contracts;
using ProjectLoot.Controllers.ModularStates;

namespace ProjectLoot.Models.BallModel;

public class BallModel : ModularDelegateState, IMeleeWeaponModel
{
    private readonly StateMachine _states;
    public IEffectsComponent HolderEffects { get; }
    public IMeleeWeaponComponent MeleeWeaponComponent { get; }
    
    public BallModel(IMeleeWeaponComponent meleeWeaponComponent, IEffectsComponent holderEffects, ITimeManager timeManager)
    {
        MeleeWeaponComponent = meleeWeaponComponent;
        HolderEffects        = holderEffects;

        _states = new StateMachine();
        _states.Add(new NotEquipped(_states, this));
        _states.Add(new Idle(_states, this));
    }

    public void Activity()
    {
        _states.DoCurrentStateActivity();
    }

    public string WeaponName => "Ball";
    public bool IsEquipped { get; set; }
}