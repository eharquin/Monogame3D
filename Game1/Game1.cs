using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game1
{
    public class Game1 : Game
    {
        // DISPLAY
        public const int Width = 1024, Height = 768; // Target format
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        public static int ScreenWidth, ScreenHeight;
        private Camera _camera;

        // INPUT
        private Input _input;

        // RENDERTARGETS & TEXTURES
        private RenderTarget2D _mainTarget;
        private Texture2D _testTex;

        // RECTANGLES 
        private Rectangle _desktopRect;
        private Rectangle _screenRect;

        // 3D
        private Basic3DObjects _basic3DObjects;

        // CONSTRUCTOR
        public Game1()
        {
            int desktopWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10;
            int desktopHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 10;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = desktopWidth,
                PreferredBackBufferHeight = desktopHeight,
                IsFullScreen = false,
                PreferredDepthStencilFormat = DepthFormat.None,
                GraphicsProfile = GraphicsProfile.HiDef // allow 4Megs of indices at once
            };
            IsMouseVisible = true;
            Window.IsBorderless = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // DISPLAY
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _mainTarget = new RenderTarget2D(GraphicsDevice, Width, Height, false, pp.BackBufferFormat, DepthFormat.Depth24);
            ScreenWidth = _mainTarget.Width;
            ScreenHeight = _mainTarget.Height;
            _desktopRect = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
            _screenRect = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

            // INPUT
            _input = new Input(pp, _mainTarget);

            // INIT 3D
            _camera = new Camera(GraphicsDevice, Vector3.Down, _input);
            _basic3DObjects = new Basic3DObjects(GraphicsDevice, _camera.Up, Content);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteFont = Content.Load<SpriteFont>("Font");
            _testTex = Content.Load<Texture2D>("BeachBallTexture");

            _basic3DObjects.AddFloor(200, 200, Vector3.Zero, Vector3.Zero, "BeachBallTexture", null);
            _basic3DObjects.AddCube(50, 50, 50, Vector3.Zero, Vector3.Zero, "BeachBallTexture", null);
            _basic3DObjects.Objects[1].Position = new Vector3(30, -40, -30);

        }

        protected override void Update(GameTime gameTime)
        {
            _input.Update();
            if (_input.BackDown || _input.KeyDown(Keys.Escape))
                Exit();

            _camera.MoveCamera(new Vector3(_input.GamePadState.ThumbSticks.Left.X, _input.GamePadState.ThumbSticks.Right.Y, _input.GamePadState.ThumbSticks.Right.X));
            _camera.UpdatePlayerCam();
            if (_input.KeyDown(Keys.Up)) _basic3DObjects.Objects[0].Position.Z++;
            if (_input.KeyDown(Keys.Down)) _basic3DObjects.Objects[0].Position.Z--;
            if (_input.KeyDown(Keys.Left)) _basic3DObjects.Objects[0].Position.X--;
            if (_input.KeyDown(Keys.Right)) _basic3DObjects.Objects[0].Position.X++;
            if (_input.KeyDown(Keys.PageUp)) _basic3DObjects.Objects[0].Position.Y--;
            if (_input.KeyDown(Keys.PageDown)) _basic3DObjects.Objects[0].Position.Y++;
            _basic3DObjects.Objects[1].Rotation.Y += 0.3f;                       // rotate just for fun
            _basic3DObjects.Objects[1].UpdateTransform();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_mainTarget);

            Set3DStates();
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);

            // RENDER SCENE OBJECTS
            _basic3DObjects.Draw(_camera);

            // DRAW MAINTARGET TO BACKBUFFER
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
            _spriteBatch.Draw(_mainTarget, _desktopRect, Color.White);

            // PRINT CAMERA COORDONATE
            _spriteBatch.DrawString(_spriteFont, "X : "+_camera.Position.X.ToString(), new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Y : "+ _camera.Position.Y.ToString(), new Vector2(0, 20), Color.White);
            _spriteBatch.DrawString(_spriteFont, "Z : "+_camera.Position.Z.ToString(), new Vector2(0, 40), Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void Set3DStates()
        {
            GraphicsDevice.BlendState = BlendState.NonPremultiplied;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            if (GraphicsDevice.RasterizerState.CullMode == CullMode.None)
            {
                RasterizerState rs = new RasterizerState { CullMode = CullMode.CullCounterClockwiseFace };
                GraphicsDevice.RasterizerState = rs; //device state change requires new instances of RasterizerState...
            }
        }
    }
}
