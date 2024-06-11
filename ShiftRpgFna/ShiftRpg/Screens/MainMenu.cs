using System;
using FlatRedBall;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screens;
using ShiftRpg.GumRuntimes;

namespace ShiftRpg.Screens
{
    public partial class MainMenu
    {
        void CustomInitialize()
        {
            var controller = InputManager.Xbox360GamePads[0];
            if (controller.IsConnected)
            {
                GuiManager.GamePadsForUiControl.Add(controller);
                Forms.MainMenuInstance.PlayButton.IsFocused = true;
            }

            GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Main;
            Forms.MainMenuInstance.PlayButton.Click += (_, _) => ScreenManager.MoveToScreen("TestLevel");
            Forms.MainMenuInstance.OptionsButton.Click += OnClickOptions;
            Forms.OptionsInstance.BackButton.Click += OnClickBack;
            Forms.MainMenuInstance.QuitButton.Click += (_, _) => FlatRedBallServices.Game.Exit();
        }

        private void OnClickBack(object o, EventArgs eventArgs)
        {
            GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Main;
            if (InputManager.Xbox360GamePads[0].IsConnected)
            {
                Forms.MainMenuInstance.PlayButton.IsFocused = true;
            }
        }

        private void OnClickOptions(object o, EventArgs eventArgs)
        {
            GumScreen.CurrentScreenState = MainMenuGumRuntime.Screen.Options;
            if (InputManager.Xbox360GamePads[0].IsConnected)
            {
                Forms.OptionsInstance.BackButton.IsFocused = true;
            }
        }

        void CustomActivity(bool firstTimeCalled)
        {


        }

        void CustomDestroy()
        {


        }

        static void CustomLoadStaticContent(string contentManagerName)
        {


        }

    }
}
