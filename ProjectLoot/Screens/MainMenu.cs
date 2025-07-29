using FlatRedBall;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screens;
using ProjectLoot.GumRuntimes;

namespace ProjectLoot.Screens;

public partial class MainMenu
{
    private void CustomInitialize()
    {
        Xbox360GamePad? controller = InputManager.Xbox360GamePads[0];
        if (controller.IsConnected)
        {
            GuiManager.GamePadsForUiControl.Add(controller);
            Forms.MainMenuInstance.PlayButton.IsFocused = true;
        }

        GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Main;
        
        Forms.MainMenuInstance.PlayButton.Click +=
            (_, _) => GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Play;
        
        Forms.MainMenuInstance.OptionsButton.Click += OnClickOptions;
        Forms.MainMenuInstance.QuitButton.Click    += (_, _) => FlatRedBallServices.Game.Exit();
        Forms.OptionsInstance.BackButton.Click     += OnClickBack;
        Forms.PlayMenuInstance.ArenaButton.Click   += (_, _) => ScreenManager.MoveToScreen(nameof(Arena));
        
        Forms.PlayMenuInstance.TrainingGroundsButton.Click +=
            (_, _) => ScreenManager.MoveToScreen(nameof(ShootingRange));
        
        Forms.PlayMenuInstance.BackButton.Click +=
            (_, _) => GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Main;
    }

    private void OnClickBack(object? o, EventArgs eventArgs)
    {
        GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Main;
        if (InputManager.Xbox360GamePads[0].IsConnected)
        {
            Forms.MainMenuInstance.PlayButton.IsFocused = true;
        }
    }

    private void OnClickOptions(object? o, EventArgs eventArgs)
    {
        GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Options;
        if (InputManager.Xbox360GamePads[0].IsConnected)
        {
            Forms.OptionsInstance.BackButton.IsFocused = true;
        }
    }

    private void CustomActivity(bool firstTimeCalled)
    {
    }

    private void CustomDestroy()
    {
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }
}