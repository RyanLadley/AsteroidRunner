using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AsteroidRunner.GameObjects
{
    public class GameObject : IGameObject
    {
        public virtual GameObjectType Type => GameObjectType.GameObject;

        protected Rectangle _textureRectangle;
        protected Texture2D _texture;
        public Texture2D Texture { get { return _texture; } }


        public Rectangle HitBox { get { return new Rectangle((int)Math.Ceiling(_location.X), (int)Math.Ceiling(_location.Y), _hitBoxDimensions, _hitBoxDimensions); } }
        public int _hitBoxDimensions;

        public float Scale { get { return _drawScale; } }
        public Vector2 Location { get { return _location; } }
        public float Direction { get { return _direction; } }
        public Vector2 Origin { get { return _origin; }}
        public Vector2 Dimensions { get { return new Vector2(_width, _height); } }

        protected bool _expired;
        public bool IsExpired { get { return _expired; } }


        protected Color _color;

        protected float _speedMagnitude;
        protected Vector2 _speed;
        protected float _direction;
        protected float _rotationSpeed;

        protected float _width;
        protected float _height;
        protected Vector2 _location;
        protected Vector2 _origin;
        protected float _drawScale;

        public GameObject(Texture2D texture, Rectangle textureRectangle, float direction, Vector2 startLaction, Color color, float speedMagnitude, float rotationSpeed, float scale)
        {
            _texture = texture;
            _textureRectangle = textureRectangle;

            _color = color;

            _location = startLaction;
            _direction = direction;

            _speedMagnitude = speedMagnitude;
            _speed.X = (float)(_speedMagnitude * Math.Sin(_direction));
            _speed.Y = (float)(_speedMagnitude * Math.Cos(_direction));

            _rotationSpeed = rotationSpeed;
            
            _drawScale = scale;
            _origin = new Vector2(_textureRectangle.Width / 2, _textureRectangle.Height / 2);

            _width = _textureRectangle.Width * _drawScale;
            _height = _textureRectangle.Height * _drawScale;

            _hitBoxDimensions = (int)Math.Ceiling(Math.Sqrt(_width * _width + _height * _height));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _location, _textureRectangle, _color, _direction, _origin, _drawScale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public virtual void Update(KeyboardState keyState)
        {
            _location.X += _speed.X;
            _location.Y -= _speed.Y;


            _direction += _rotationSpeed;
        }

        public virtual void SetLocation(int? x = null, int? y = null)
        {
            if (x.HasValue)
                _location.X = x.Value;

            if (y.HasValue)
                _location.Y = y.Value;
        }
        
        public Color[,] TextureColors()
        {
            //Get the colors of the full texture
            var colors1d = new Color[_textureRectangle.Width * _textureRectangle.Height];
            _texture.GetData(0, _textureRectangle, colors1d, 0, colors1d.Length);

            //Convert these colors into a 2d array with only the texture that is presented
            var colors2d = new Color[_textureRectangle.Width, _textureRectangle.Height];
            
            for (var x = 0; x < _textureRectangle.Width; x++)
            {
                for (var y = 0; y < _textureRectangle.Height; y++)
                    colors2d[x, y] = colors1d[x + y * _textureRectangle.Width];
            }

            return colors2d;
        }

        public virtual void ProcessCollision(IGameObject collidedObject)
        {
            return;
        }
    }
}
