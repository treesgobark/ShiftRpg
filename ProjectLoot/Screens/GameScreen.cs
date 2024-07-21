using FlatRedBall;
using FlatRedBall.Graphics;
using FlatRedBall.Gui;
using FlatRedBall.Input;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;
using ProjectLoot.Components;
using ProjectLoot.Entities;
using ProjectLoot.GumRuntimes;
using ProjectLoot.GumRuntimes.VirtualController;
using ProjectLoot.Models;

namespace ProjectLoot.Screens;

public partial class GameScreen
{
    private MagazineDisplay MagazineDisplay { get; set; }

    protected bool GameOver { get; set; }

    private void CustomInitialize()
    {
        SpriteManager.OrderedSortType                    = SortType.ZSecondaryParentY;
        CameraControllingEntityInstance.CameraOffset     = new Vector3(0, 0, 0);
        GumScreen.HealthBarPlayerInstance.BindingContext = Player1.HealthComponent;
        GumScreen.HealthBarPlayerInstance.SetBinding(nameof(HealthBarPlayerRuntime.ProgressPerCent),
                                                     nameof(HealthComponent.HealthPercentage));
        InitializePauseMenu();

        MagazineDisplay = new MagazineDisplay(CartridgeDisplayFactory, new Vector3(16, -32, 0), 3, 11)
        {
            BindingContext = Player1.GunComponent
        };
    }

    private static CartridgeDisplayRuntime CartridgeDisplayFactory()
    {
        var cartridge = new CartridgeDisplayRuntime();
        cartridge.AddToManagers();
        return cartridge;
    }

    private void CustomActivity(bool firstTimeCalled)
    {
        // Debugger.CommandLineWrite(GuiManager.Cursor.WindowOver);

        // DisplayEnemyInputs();
        DisplayPlayerInputs();

        foreach (Enemy? enemy in EnemyList)
        {
            enemy?.EnemyInputDevice?.SetTarget(Player1);

            if (enemy is Dot dot)
            {
                dot.Target = Player1;
            }
        }

        // foreach(AxisAlignedRectangle? rect in SolidCollision.Rectangles)
        // {
        //     rect.RepositionDirections = RepositionDirections.All;
        // }

        // foreach(var rect in SolidCollision.Rectangles)
        // {
        //     GlueControl.Editing.EditorVisuals.DrawRepositionDirections(rect);
        // }

        if (Player1.InputDevice.DefaultPauseInput.WasJustPressed)
        {
            TogglePause();
        }

        if (PlayerList.Count == 0 && !GameOver)
        {
            GameOver = true;
            Pause();
        }

        if (!IsPaused)
        {
            MagazineDisplay.Activity();
        }
    }

    private void DisplayEnemyInputs()
    {
        Enemy? rangedEnemy = EnemyList.FirstOrDefault(e => e is DefaultRangedEnemy);
        if (rangedEnemy is { EnemyInputDevice: not null })
        {
            GumScreen.VirtualControllerDisplayInstance.Input2DIndicatorInstance.SetPosition(
                rangedEnemy.EnemyInputDevice.Movement, 4f);

            GumScreen.VirtualControllerDisplayInstance.AttackIndicatorInstance.CurrentIsPressedState =
                rangedEnemy.EnemyInputDevice.Attack.IsDown
                    ? AttackIndicatorRuntime.IsPressed.Pressed
                    : AttackIndicatorRuntime.IsPressed.NotPressed;

            GumScreen.VirtualControllerDisplayInstance.GuardIndicatorInstance.CurrentIsPressedState =
                rangedEnemy.EnemyInputDevice.Guard.IsDown
                    ? GuardIndicatorRuntime.IsPressed.Pressed
                    : GuardIndicatorRuntime.IsPressed.NotPressed;

            GumScreen.VirtualControllerDisplayInstance.DashIndicatorInstance.CurrentIsPressedState =
                rangedEnemy.EnemyInputDevice.Dash.IsDown
                    ? DashIndicatorRuntime.IsPressed.Pressed
                    : DashIndicatorRuntime.IsPressed.NotPressed;
        }
    }

    private void DisplayPlayerInputs()
    {
        GumScreen.VirtualControllerDisplayInstance.Input2DIndicatorInstance.SetPosition(
            Player1.GameplayInputDevice.Movement, 4f);

        GumScreen.VirtualControllerDisplayInstance.AttackIndicatorInstance.CurrentIsPressedState =
            Player1.GameplayInputDevice.Attack.IsDown
                ? AttackIndicatorRuntime.IsPressed.Pressed
                : AttackIndicatorRuntime.IsPressed.NotPressed;

        GumScreen.VirtualControllerDisplayInstance.GuardIndicatorInstance.CurrentIsPressedState =
            Player1.GameplayInputDevice.Guard.IsDown
                ? GuardIndicatorRuntime.IsPressed.Pressed
                : GuardIndicatorRuntime.IsPressed.NotPressed;

        GumScreen.VirtualControllerDisplayInstance.DashIndicatorInstance.CurrentIsPressedState =
            Player1.GameplayInputDevice.Dash.IsDown
                ? DashIndicatorRuntime.IsPressed.Pressed
                : DashIndicatorRuntime.IsPressed.NotPressed;
    }

    private void CustomDestroy()
    {
        MagazineDisplay.Destroy();
    }

    private static void CustomLoadStaticContent(string contentManagerName)
    {
    }

    private void InitializePauseMenu()
    {
        Xbox360GamePad? controller = InputManager.Xbox360GamePads[0];
        if (controller.IsConnected)
        {
            GuiManager.GamePadsForUiControl.Add(controller);
            Forms.PauseMenuInstance.ResumeButton.IsFocused = true;
        }

        GumScreen.CurrentPauseStateState                  =  GameScreenGumRuntime.PauseState.Play;
        Forms.PauseMenuInstance.ResumeButton.Click        += (_, _) => Resume();
        Forms.PauseMenuInstance.OptionsButton.Click       += OnClickOptions;
        Forms.PauseMenuInstance.ExitToMainButton.Click    += (_, _) => ScreenManager.MoveToScreen("MainMenu");
        Forms.PauseMenuInstance.ExitToDesktopButton.Click += (_, _) => FlatRedBallServices.Game.Exit();
        Forms.OptionsInstance.BackButton.Click            += OnClickOptionsBack;
    }

    private void Pause()
    {
        PauseThisScreen();
        GameScreenGum.CurrentPauseStateState           = GameScreenGumRuntime.PauseState.Pause;
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