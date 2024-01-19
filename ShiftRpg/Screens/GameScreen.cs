using System;
using FlatRedBall;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screens;
using ShiftRpg.GumRuntimes;

namespace ShiftRpg.Screens;

public partial class GameScreen
{

    void CustomInitialize()
    {
        InitializePauseMenu();
    }

    void CustomActivity(bool firstTimeCalled)
    {
        if (Player1.InputDevice.DefaultPauseInput.WasJustPressed)
        {
            var gameScreen = (GameScreen)ScreenManager.CurrentScreen;
            gameScreen.TogglePause();
        }
    }

    void CustomDestroy()
    {


    }

    static void CustomLoadStaticContent(string contentManagerName)
    {


    }

    private void InitializePauseMenu()
    {
        var controller = InputManager.Xbox360GamePads[0];
        if (controller.IsConnected)
        {
            GuiManager.GamePadsForUiControl.Add(controller);
            Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
        }
            
        GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Play;
        Forms.PauseMenuInstance.ResumeButton.Click += (_, _) => Resume();
        Forms.PauseMenuInstance.OptionsButton.Click += OnClickOptions;
        Forms.PauseMenuInstance.ExitToMainButton.Click += (_, _) => ScreenManager.MoveToScreen("MainMenu");
        Forms.PauseMenuInstance.ExitToDesktopButton.Click += (_, _) => FlatRedBallServices.Game.Exit();
        Forms.OptionsInstance.BackButton.Click += OnClickOptionsBack;
    }

    public void Pause()
    {
        PauseThisScreen();
        GameScreenGum.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Pause;
        Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
    }

    private void Resume()
    {
        ScreenManager.CurrentScreen.UnpauseThisScreen();
        GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Play;
    }

    private void TogglePause()
    {
        if (GumScreen.CurrentPauseStateState is GameScreenGumRuntime.PauseState.Play)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void OnClickOptions(object o, EventArgs eventArgs)
    {
        GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Options;
        if (InputManager.Xbox360GamePads[0].IsConnected)
        {
            Forms.OptionsInstance.BackButton.IsFocused = true;
        }
    }

    private void OnClickOptionsBack(object o, EventArgs eventArgs)
    {
        GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Pause;
        if (InputManager.Xbox360GamePads[0].IsConnected)
        {
            Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
        }
    }
}