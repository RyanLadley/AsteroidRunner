using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidRunner.GameObjects.Projectiles
{
    public class Projectile : GameObject
    {
        public override GameObjectType Type => GameObjectType.Projectile;
        
        private int _lifespan = 100;
        private int _timeAlive;
        
        public Projectile(Texture2D texture, Rectangle projectileTextureRectange, float direction, Vector2 startLaction, Color color, float scale)
            : base(texture, projectileTextureRectange, direction, startLaction, color, 10 , 0, scale)
        {
            _timeAlive = 0;
        }
        

        public override void Update(KeyboardState keyState)
        {
            base.Update(keyState);

            _timeAlive++;
            if (_timeAlive > _lifespan)
                _expired = true;
        }
    }
}
