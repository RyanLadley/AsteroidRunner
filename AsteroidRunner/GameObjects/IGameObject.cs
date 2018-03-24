using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects
{
    public interface IGameObject
    {
        GameObjectType Type { get;}
        Vector2 Location{ get;}
        float Scale { get; }
        float Direction { get; }
        Texture2D Texture { get; }
        Rectangle HitBox { get; }
        bool IsExpired { get; }
        Vector2 Origin { get; }
        Vector2 Dimensions { get; }

        void Draw(SpriteBatch spriteBatch);
        void Update(KeyboardState keyState);
        void SetLocation(int? x = null, int? y = null);
        Color[,] TextureColors();
        void ProcessCollision(IGameObject collidedObject);
    }
}
