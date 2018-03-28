using AsteroidRunner.GameObjects;
using AsteroidRunner.GameObjects.Asteroids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Factories
{
    public class AsteroidFactory : IGameObjectFactory
    {
        private Texture2D _asteroidTextureRound;
        private Texture2D _asteroidTextureSharp;
        private Texture2D _asteroidTextureSmall;

        private List<Asteroid> _newAsteroids;
        private static AsteroidFactory _instance;

        private Timer _buildTimer;
        private int _currentTimerInterval;

        private AsteroidFactory(ContentManager content)
        {
            _asteroidTextureRound = content.Load<Texture2D>("Sprites/Asteroid2");
            _asteroidTextureSharp = content.Load<Texture2D>("Sprites/Asteroid");
            _asteroidTextureSmall = content.Load<Texture2D>("Sprites/SmallAsteroid");
            _newAsteroids = new List<Asteroid>();
        }

        public static AsteroidFactory Initiate(ContentManager content)
        {
            if (_instance == null)
                _instance = new AsteroidFactory(content);
            return _instance;
        }

        public static AsteroidFactory GetInstance()
        {
            return _instance;
        }
        
        //// <summary>
        /// Adds a new asteroid at the every milisecond interval 
        /// </summary>
        /// <param name="miliseconds"></param>
        public void BeginIntervaledProduction(int miliseconds)
        {
            _currentTimerInterval = miliseconds;
            _buildTimer = new Timer(
                (timer) => PreformIntervalProduciton(), null, miliseconds, miliseconds);
        }

        public void StopIntervaledProduction()
        {
            _buildTimer = null;
        }

        private void PreformIntervalProduciton()
        {
            AddAsteroid(Color.White);

            _currentTimerInterval -= 10;
            _buildTimer.Change(_currentTimerInterval, _currentTimerInterval);
        }


        /// <summary>
        /// Bulk adds the number of asteroids specified
        /// </summary>
        /// <param name="amount"></param>
        public void AddAsteroids(int amount)
        {
            for (var i = 0; i < amount; i++)
                AddAsteroid(Color.White);
        }


        public void AddAsteroid(Color color)
        {

            var random = new Random(Guid.NewGuid().GetHashCode());
            var asteroidType = (AsteroidTypes)random.Next(0, 2);
            var direction = _randomFloat(random, 0, 6.28318530718f); //between 0 and 2 pi
            var scale = 1f;
            var location = _getRandomSideLocation(random);
            var speed = _randomFloat(random, 0.5f, 4);
            var rotationSpeed = _randomFloat(random, -0.004f, 0.004f);

            Texture2D texture;
            Rectangle textureRectangle;

            switch (asteroidType)
            {
                case AsteroidTypes.Round:
                    texture = _asteroidTextureRound;
                    textureRectangle = new Rectangle(0, 0, 157, 147);
                    break;

                case AsteroidTypes.Sharp:
                default:
                    texture = _asteroidTextureSharp;
                    textureRectangle = new Rectangle(0, 0, 132, 157);
                    break;
            }

            _newAsteroids.Add(new Asteroid(texture, textureRectangle, direction, location, color, speed, rotationSpeed, scale, 2,true));
        }

        /// <summary>
        /// Bulk adds the number of asteroids specified
        /// </summary>
        /// <param name="amount"></param>
        public void AddSmallAsteroids(int amount, Color color, Vector2? location)
        {
            for (var i = 0; i < amount; i++)
                AddSmallAsteroid(color, location);
        }

        public void AddSmallAsteroid(Color color, Vector2? location = null)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var asteroidType = (AsteroidTypes)random.Next(0, 2);
            var direction = _randomFloat(random, 0, 6.28318530718f); //between 0 and 2 pi
            var scale = 1f;
            var speed = _randomFloat(random, 1f, 4.5f);
            var rotationSpeed = _randomFloat(random, -0.008f, 0.008f);

            if(!location.HasValue)
                location = _getRandomSideLocation(random);

            var textureRectangle = new Rectangle(0, 0, 90, 64);

            _newAsteroids.Add(new Asteroid(_asteroidTextureSmall, textureRectangle, direction, location.Value, color, speed, rotationSpeed, scale, 1, false));
        }


        private Vector2 _getRandomSideLocation(Random random)
        {
            var width = 1980;
            var height = 1140;
            
            var side = (ObjectSide)random.Next(0, 4);
            switch (side)
            {
                case ObjectSide.Left:
                    return new Vector2(0, random.Next(0, height));

                case ObjectSide.Right:
                    return new Vector2(width, random.Next(0, height));

                case ObjectSide.Top:
                    return new Vector2(random.Next(0, width), 0);

                case ObjectSide.Bottom:
                default:
                    return new Vector2(random.Next(0, width), height);
            }
        }

        private float _randomFloat(Random random, float minValue, float maxValue)
        {
            return (float) random.NextDouble() * (maxValue - minValue) + minValue;
        }

        /// <summary>
        /// Returns list of newly created projectiles.
        /// </summary>
        /// <returns></returns>
        public List<IGameObject> RetrieveObjects()
        {
            var gameObjects = _newAsteroids.ToList<IGameObject>();
            _newAsteroids.Clear();
            return gameObjects;
        }

    }
}
