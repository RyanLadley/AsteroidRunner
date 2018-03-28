using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects.Primitives
{
    public class RectanglePrimative
    {

        private static Texture2D _texture;

        public static void LoadTexture(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Rectangle");
        }

        public static void Draw(SpriteBatch spriteBatch, Rectangle rectangle)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, rectangle, Color.White);
            spriteBatch.End();

        }
        
    }
}
