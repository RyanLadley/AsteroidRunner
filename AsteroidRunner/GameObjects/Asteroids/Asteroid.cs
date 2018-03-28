using AsteroidRunner.Utilities.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects.Asteroids
{
    public class Asteroid : GameObject
    {
        public override GameObjectType Type => GameObjectType.Asteroid;


        private bool _addSmallOnExpire;
        private int _maxHits;
        private int hitCount = 0;

        public Asteroid(Texture2D texture, Rectangle textureRectange, float direction, Vector2 startLaction, Color color, float speed, float rotationSpeed,  float scale, int maxHits, bool addSmallOnExpire)
            : base(texture, textureRectange, direction, startLaction, color, speed, rotationSpeed, scale)
        {
            _maxHits = maxHits;
            _addSmallOnExpire = addSmallOnExpire;
        }

        public override void ProcessCollision(IGameObject collidedObject)
        {
            switch (collidedObject.Type)
            {
                case GameObjectType.Projectile:
                    _processProjectileHit();
                    break;

                case GameObjectType.Asteroid:
                    //_processProjectileHit();
                    break;
            }

        }

        private void _processProjectileHit()
        {
            hitCount++;
            if (hitCount >= _maxHits)
            {
                _expired = true;
                if(_addSmallOnExpire)
                    AsteroidFactory.GetInstance().AddSmallAsteroids(3, _color, _location);
            }
            else
                _textureRectangle.X += _textureRectangle.Width;

                 
        }
    }
}
