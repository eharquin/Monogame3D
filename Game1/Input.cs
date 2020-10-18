using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Input
    {
        // KEYBOARD STUFF
        public KeyboardState KeyboardState, OldKeybordState;
        public bool ShiftDown, ControlDown, AltDown, ShiftPress, ControlPress, AltPress;
        public bool OldShiftDown, OldControlDown, OldAltDown;

        // MOUSE STUFF
        public MouseState MouseState, OldMouseState;
        public bool LeftClick, MiddleClick, RightClick, LeftDown, MiddleDown, RightDown;
        public int MouseX, MouseY;
        public Vector2 MouseVector;
        public Point MousePoint;
        public float ScreenScaleX, ScreenScaleY; // used to scale desktop resolution mouse coordinates to match position in MainTarget (resolution of game)

        // GAMEPAD STUFF
        public GamePadState GamePadState, OldGamePadState;
        public bool ADown, BDown, XDown, YDown, RDown, LDown, StartDown, BackDown, LeftStickDown, RightStickDown;
        public bool APress, BPress, XPress, YPress, RPress, LPress, StartPress, BackPress, LeftStickPress, RightStickPress;

        // CONSTRUCTOR
        public Input(PresentationParameters pp, RenderTarget2D target)
        {
            ScreenScaleX = 1.0f / ((float)pp.BackBufferWidth / (float)target.Width);
            ScreenScaleY = 1.0f / ((float)pp.BackBufferHeight / (float)target.Height);
        }
        
        public bool KeyPress(Keys k)
        { 
            if (KeyboardState.IsKeyDown(k) && OldKeybordState.IsKeyUp(k)) return true;
            else return false;
        }
        public bool KeyDown(Keys k)
        { 
            if (KeyboardState.IsKeyDown(k)) return true;
            else return false;
        }
        public bool ButtonPress(Buttons button)
        {
            if (GamePadState.IsButtonDown(button) && OldGamePadState.IsButtonUp(button)) return true;
            else return false;
        }

        // UPDATE
        public void Update()
        {
            // KEYBOARD STUFF
            OldKeybordState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            OldAltDown = AltDown; OldShiftDown = ShiftDown; OldControlDown = ControlDown;
            ShiftDown = ShiftPress = ControlDown = ControlPress = AltDown = AltPress = false;

            if (KeyboardState.IsKeyDown(Keys.LeftShift) || KeyboardState.IsKeyDown(Keys.RightShift)) ShiftDown = true;
            if (KeyboardState.IsKeyDown(Keys.LeftControl) || KeyboardState.IsKeyDown(Keys.RightControl)) ControlDown = true;
            if (KeyboardState.IsKeyDown(Keys.LeftAlt) || KeyboardState.IsKeyDown(Keys.RightAlt)) AltDown = true;
            if ((ShiftDown) && (!OldShiftDown)) ShiftPress = true;
            if ((ControlDown) && (!OldControlDown)) ControlPress = true;
            if ((AltDown) && (!OldAltDown)) AltPress = true;

            // MOUSE STUFF
            OldMouseState = MouseState;
            MouseState = Mouse.GetState();

            MouseVector = new Vector2((float)MouseState.Position.X * ScreenScaleX, (float)MouseState.Position.Y * ScreenScaleY);
            MouseX = (int)MouseVector.X; MouseY = (int)MouseVector.Y; MousePoint = new Point(MouseX, MouseY);

            LeftClick = MiddleClick = RightClick = LeftDown = RightDown = MiddleDown = false;

            if (MouseState.LeftButton == ButtonState.Pressed) LeftDown = true;
            if (MouseState.MiddleButton == ButtonState.Pressed) MiddleDown = true;
            if (MouseState.RightButton == ButtonState.Pressed) RightDown = true;
            if ((LeftDown) && (OldMouseState.LeftButton == ButtonState.Released)) LeftClick = true;
            if ((MiddleDown) && (OldMouseState.MiddleButton == ButtonState.Released)) MiddleClick = true;
            if ((RightDown) && (OldMouseState.RightButton == ButtonState.Released)) RightClick = true;

            // GAMEPAD STUFF
            OldGamePadState = GamePadState;
            GamePadState = GamePad.GetState(0);

            ADown = BDown = XDown = YDown = RDown = LDown = StartDown = BackDown = LeftStickDown = RightStickDown = false;
            APress = BPress = XPress = YPress = RPress = LPress = StartPress = BackPress = LeftStickPress = RightStickPress = false;

            if (GamePadState.IsButtonDown(Buttons.A)) { ADown = true; if (OldGamePadState.IsButtonUp(Buttons.A)) APress = true; }
            if (GamePadState.IsButtonDown(Buttons.B)) { BDown = true; if (OldGamePadState.IsButtonUp(Buttons.B)) BPress = true; }
            if (GamePadState.IsButtonDown(Buttons.X)) { XDown = true; if (OldGamePadState.IsButtonUp(Buttons.X)) XPress = true; }
            if (GamePadState.IsButtonDown(Buttons.Y)) { YDown = true; if (OldGamePadState.IsButtonUp(Buttons.Y)) YPress = true; }
            if (GamePadState.IsButtonDown(Buttons.LeftShoulder)) { RDown = true; if (OldGamePadState.IsButtonUp(Buttons.RightShoulder)) RPress = true; }
            if (GamePadState.IsButtonDown(Buttons.RightShoulder)) { LDown = true; if (OldGamePadState.IsButtonUp(Buttons.LeftShoulder)) LPress = true; }
            if (GamePadState.IsButtonDown(Buttons.Back)) { BackDown = true; if (OldGamePadState.IsButtonUp(Buttons.Back)) BackPress = true; }
            if (GamePadState.IsButtonDown(Buttons.Start)) { StartDown = true; if (OldGamePadState.IsButtonUp(Buttons.Start)) StartPress = true; }
            if (GamePadState.IsButtonDown(Buttons.LeftStick)) { LeftStickDown = true; if (OldGamePadState.IsButtonUp(Buttons.LeftStick)) LeftStickPress = true; }
            if (GamePadState.IsButtonDown(Buttons.RightStick)) { RightStickDown = true; if (OldGamePadState.IsButtonUp(Buttons.RightStick)) RightStickPress = true; }
        }
    }
}

