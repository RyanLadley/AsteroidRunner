using AsteroidRunner.GameObjects.Asteroids;
using AsteroidRunner.Utilities.Factories;
using AsteroidRunner.Utilities.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects.Ships
{
    public class Ship : GameObject
    {
        public override GameObjectType Type => GameObjectType.Ship;

        private float _thrustForce = 0.05f;
        private float _rotationalForce = 0.002f;
        private float _maxSpeed = 7;
        private float _maxRotation = 0.2f;

        private bool _isThrusting;
        private bool _isRightThrust;
        private bool _isLeftThrust;

        
        private long _lastFired;
        private int _fireInterval = 5000000; //2 per Second (1 every 0.5 seconds)
        
        
        private Rectangle _thrustTextureRectangle;
        private Rectangle _horizontalThrustTextureRectangle;
       


        public Ship(Texture2D texture, Rectangle shipTextureRectangle, Rectangle thrustTextureRectange, Rectangle horizontalThrustTextureRectangle, float scale)
             : base(texture, shipTextureRectangle, 0.4f, new Vector2(950, 500), Color.Green, 0.1f, 0.002f, scale)
        {

            _thrustTextureRectangle = thrustTextureRectange;
            _horizontalThrustTextureRectangle = horizontalThrustTextureRectangle;
        }

        public override void Update(KeyboardState keyState)
        {
            _handleKeyPress(keyState);

            base.Update(keyState);
        }

        public override void SetLocation(int? x = null, int? y = null)
        {
            if (x.HasValue)
                _location.X = x.Value;

            if (y.HasValue)
                _location.Y = y.Value;
        }

        /// <summary>
        /// Move our sprite based on arrow keys being pressed:
        /// </summary>
        private void _handleKeyPress(KeyboardState keyState)
        {

            if (keyState.IsKeyDown(Keys.D))
            {
                //If _isRightThrust is false, we are initiating the thrust, so play sound
                if (!_isRightThrust)
                    SoundEffectEngine.GetInstance().LoopAirThrust();
                _isRightThrust = true;
                _appyRotation(true);
            }
            //If _isRightThrust is true, but A is up, we are stopping our thrust. 
            if (_isRightThrust && keyState.IsKeyUp(Keys.D))
            {
                SoundEffectEngine.GetInstance().StopAirThrust();
                _isRightThrust = false;
            }


            if (keyState.IsKeyDown(Keys.A))
            {
                //If _isLeftThrust is false, we are initiating the thrust, so play sound
                if (!_isLeftThrust)
                    SoundEffectEngine.GetInstance().LoopAirThrust();
                
                _isLeftThrust = true;
                _appyRotation(false);
            }

            //If _isRightThrust is true, but A is up, we are stopping our thrust. 
            if (_isLeftThrust && keyState.IsKeyUp(Keys.A))
            {
                SoundEffectEngine.GetInstance().StopAirThrust();
                _isLeftThrust = false;
            }


            if (keyState.IsKeyDown(Keys.W))
            {
                if (!_isThrusting)
                    SoundEffectEngine.GetInstance().LoopRocket();

                _isThrusting = true;
                _applyThrust();
            }

            if (_isThrusting && keyState.IsKeyUp(Keys.W))
            {
                SoundEffectEngine.GetInstance().StopRocket();
                _isThrusting = false;
            }

            if (keyState.IsKeyDown(Keys.Space))
                _fireProjectile();
        }

        private void _fireProjectile()
        {
            if((DateTime.Now.Ticks - _fireInterval) > _lastFired)
            {
                var projectileFactory = ProjectileFactory.GetInstance();
                projectileFactory.AddStandardProjectile(_location, _direction, Color.Red);
                SoundEffectEngine.GetInstance().PlayLaserShot();
                _lastFired = DateTime.Now.Ticks;
            }
        }


        public void _applyThrust()
        {
            _speed.X += (float)(_thrustForce * Math.Sin(_direction));
            _speed.Y += (float)(_thrustForce * Math.Cos(_direction));

            //If the speed goes above the max, normalize it for it's direction at the max speed
            if (_speed.Length() > _maxSpeed)
                _speed = Vector2.Normalize(_speed) * _maxSpeed;
        }

        public void _appyRotation(bool right)
        {
            if(right)
                _rotationSpeed += _rotationalForce;
            else
                _rotationSpeed -= _rotationalForce;

            if (_rotationSpeed > _maxRotation)
                _rotationSpeed = _maxRotation;
            else if (_rotationSpeed < -1 * _maxRotation)
                _rotationSpeed = -1 * _maxRotation;
        }


        public override void ProcessCollision(IGameObject collidedObject)
        {
            switch (collidedObject.Type)
            {
                case GameObjectType.Asteroid:
                    _processAsteroidCollision((Asteroid)collidedObject);
                    break;
            }
        }

        private void _processAsteroidCollision(Asteroid asteroid)
        {
            var soundEngine = SoundEffectEngine.GetInstance();
            soundEngine.StopAirThrust();
            soundEngine.StopAirThrust(); //Can have two air thrusts going
            soundEngine.StopRocket();

            _expired = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();

            if (_isThrusting)
            {
                var thrustRotationOrigin = new Vector2(_origin.X / 4.5f, _origin.Y * -1.05f);
                spriteBatch.Draw(_texture, _location, _thrustTextureRectangle, Color.Blue, _direction, thrustRotationOrigin, _drawScale, SpriteEffects.None, 0);
            }

            if (_isLeftThrust)
            {
                var leftThrustOrigin = new Vector2(_origin.X * -1f, _origin.Y / -6f);
                spriteBatch.Draw(_texture, _location, _horizontalThrustTextureRectangle, Color.White, _direction - 1.5708f, leftThrustOrigin, _drawScale, SpriteEffects.None, 0);
            }

            if (_isRightThrust)
            {
                var rightThrustOrigin = new Vector2(_origin.X * 1.33f, _origin.Y/-5.6f);
                spriteBatch.Draw(_texture, _location, _horizontalThrustTextureRectangle, Color.White, _direction + 1.5708f, rightThrustOrigin, _drawScale, SpriteEffects.None, 0);
            }

            spriteBatch.End();
        }
        
    }
}
