using FlatRedBall;
using FlatRedBall.Screens;
using Microsoft.Xna.Framework;

namespace ProjectLoot
{
    public partial class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        
        partial void GeneratedInitializeEarly();
        partial void GeneratedInitialize();
        partial void GeneratedUpdate(GameTime gameTime);
        partial void GeneratedDrawEarly(GameTime gameTime);
        partial void GeneratedDraw(GameTime gameTime);

        public Game1() : base()
        {
            graphics = new GraphicsDeviceManager(this);

#if  ANDROID || IOS
            graphics.IsFullScreen = true;
#elif WINDOWS || DESKTOP_GL
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
#endif

            IsFixedTimeStep                         = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime                       = TimeSpan.FromMilliseconds(1000f / 120f);
            
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            #if IOS
            var bounds = UIKit.UIScreen.MainScreen.Bounds;
            var nativeScale = UIKit.UIScreen.MainScreen.Scale;
            var screenWidth = (int)(bounds.Width * nativeScale);
            var screenHeight = (int)(bounds.Height * nativeScale);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            #endif
        
            GeneratedInitializeEarly();

            FlatRedBallServices.InitializeFlatRedBall(this, graphics);

            GeneratedInitialize();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            FlatRedBallServices.Update(gameTime);

            ScreenManager.Activity();

            GeneratedUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GeneratedDrawEarly(gameTime);

            FlatRedBallServices.Draw();

            GeneratedDraw(gameTime);

            base.Draw(gameTime);
        }
    }
}
