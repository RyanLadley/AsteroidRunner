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

        public Asteroid(Texture2D texture, Rectangle textureRectange, float direction, Vector2 startLaction, Color color, float speed, float rotationSpeed,  float scale)
            : base(texture, textureRectange, direction, startLaction, color, speed, rotationSpeed, scale)
        {
        }
    }
}
