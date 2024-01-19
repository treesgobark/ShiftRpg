using System;
using System.Collections.Generic;
using System.Linq;
using FlatRedBall;
using FlatRedBall.Gui;
using FlatRedBall.Screens;

namespace ShiftRpg.GumRuntimes
{
    public partial class GameScreenGumRuntime
    {
        partial void CustomInitialize ()
        {
            CurrentPauseStateState = PauseState.Play;
            PauseMenuInstance.ResumeButton.Click += Resume;
            PauseMenuInstance.OptionsButton.Click += _ => CurrentPauseStateState = PauseState.Options;
            PauseMenuInstance.ExitToMainButton.Click += _ => ScreenManager.MoveToScreen("MainMenu");
            PauseMenuInstance.ExitToDesktopButton.Click += _ => FlatRedBallServices.Game.Exit();
            OptionsInstance.BackButton.Click += _ => CurrentPauseStateState = PauseState.Pause;
        }

        private void Resume(IWindow window)
        {
            ScreenManager.CurrentScreen.UnpauseThisScreen();
            CurrentPauseStateState = PauseState.Play;
        }
    }
}
