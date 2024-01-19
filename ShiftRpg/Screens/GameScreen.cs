using FlatRedBall;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screens;
using ShiftRpg.GumRuntimes;

namespace ShiftRpg.Screens
{
    public partial class GameScreen
    {

        void CustomInitialize()
        {
            var controller = InputManager.Xbox360GamePads[0];
            if (controller.IsConnected)
            {
                GuiManager.GamePadsForUiControl.Add(controller);
                Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
            }
            
            GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Play;
            Forms.PauseMenuInstance.ResumeButton.Click += (_, _) => Resume();
            Forms.PauseMenuInstance.OptionsButton.Click += (_, _) =>
            {
                GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Options;
                Forms.OptionsInstance.BackButton.IsFocused = true;
            };
            Forms.PauseMenuInstance.ExitToMainButton.Click += (_, _) => ScreenManager.MoveToScreen("MainMenu");
            Forms.PauseMenuInstance.ExitToDesktopButton.Click += (_, _) => FlatRedBallServices.Game.Exit();
            Forms.OptionsInstance.BackButton.Click += (_, _) =>
            {
                GumScreen.CurrentPauseStateState = GameScreenGumRuntime.PauseState.Pause;
                Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
            };
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

    }
}
