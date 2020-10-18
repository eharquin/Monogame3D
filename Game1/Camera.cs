using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Camera
    {
        public const float CamHeightOffset = 80f;

        public const float FarPlane = 2000;
        public Vector3 Position, Target;
        public Matrix View, Projection, ViewProjection;
        public Vector3 Up;
        private float _currentAngle;
        private float _angleVelocity;
        private float _radius = 100.0f;
        private Vector3 _unitDirection;

        private Input _input;

        // CONSTRUCTOR
        public Camera(GraphicsDevice graphicsDevice, Vector3 UpDirection, Input input)
        {
            Up = UpDirection;
            Position = new Vector3(20, -30, -50);
            Target = Vector3.Zero;
            View = Matrix.CreateLookAt(Position, Target, Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, graphicsDevice.Viewport.AspectRatio, 1.0f, FarPlane);
            ViewProjection = View * Projection;
            _input = input;
            _unitDirection = View.Forward;
            _unitDirection.Normalize();
        }

        // MOVE CAMERA
        public void MoveCamera(Vector3 move)
        {
            Position += move;
            View = Matrix.CreateLookAt(Position, Target, Up);
            ViewProjection = View * Projection;
        }

        // UPDATE TARGET
        public void UpdateTarget(Vector3 newTarget)
        {
            Target = newTarget; Target.Y -= 10; // temporary adjustement hardcoded
            View = Matrix.CreateLookAt(Position, Target, Up);
            ViewProjection = View * Projection;
        }

        //UPDATE PLAYER CAMERA
        public void UpdatePlayerCam()
        {
            #region TEMPORARY_ADDITIONAL_CAMERA_CONTROL
            if (_input.ShiftDown) Position.Y += 5;
            if (_input.KeyDown(Keys.Space)) Position.Y -= 5;
            if (_input.KeyDown(Keys.Z)) Position.X += 5;
            if (_input.KeyDown(Keys.S)) Position.X -= 5;
            if (_input.KeyDown(Keys.Q)) Position.Z += 5;
            if (_input.KeyDown(Keys.D)) Position.Z -= 5;
            #endregion

            UpdateTarget(Vector3.Zero);
        }
    }
}
