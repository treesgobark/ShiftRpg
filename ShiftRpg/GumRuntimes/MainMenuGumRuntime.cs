using System;
using System.Collections.Generic;
using System.Linq;
using FlatRedBall;
using FlatRedBall.Screens;

namespace ShiftRpg.GumRuntimes
{
    public partial class MainMenuGumRuntime
    {
        partial void CustomInitialize ()
        {
            CurrentScreenState = Screen.Main;
            MainMenuInstance.PlayButton.Click += _ => ScreenManager.MoveToScreen("TestLevel");
            MainMenuInstance.OptionsButton.Click += _ => CurrentScreenState = Screen.Options;
            OptionsInstance.BackButton.Click += _ => CurrentScreenState = Screen.Main;
            MainMenuInstance.QuitButton.Click += _ => FlatRedBallServices.Game.Exit();
        }
    }
}
