using AsteroidRunner.GameObjects.Asteroids;
using AsteroidRunner.GameObjects.Ships;
using AsteroidRunner.Utilities.Collision;
using AsteroidRunner.Utilities.Factories;
using AsteroidRunner.Utilities.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects
{
    public class GameField
    {
        private static GameField _instance;
        private bool _ready;
        public List<IGameObject> FieldObjects { get; set; }
        private Texture2D _texture;
        private Texture2D _playerTexture;
        private List<IGameObjectFactory> _gameObjectFactories;
        private CollisionDetector _collisionDetector;
        private int _playerIndex;

        private GameField(ContentManager content)
        {
            _ready = false;
            _texture = content.Load<Texture2D>("Sprites/StarField");
            _playerTexture = content.Load<Texture2D>("Sprites/SpaceShip");
            _collisionDetector = new CollisionDetector();

            var projectileFactory = ProjectileFactory.GetInstance();
            var asteroidFactory = AsteroidFactory.GetInstance();
            _gameObjectFactories = new List<IGameObjectFactory>() {
                asteroidFactory,
                projectileFactory
            };

            Reset();

            MusicEngine.GetInstance().PlayMeditation();
        }
        
        public void Reset()
        {
            FieldObjects = new List<IGameObject>();

            var projectileFactory = ProjectileFactory.GetInstance();
            var asteroidFactory = AsteroidFactory.GetInstance();
            asteroidFactory.StopIntervaledProduction();

            var player = new Ship(_playerTexture, new Rectangle(0, 0, 130, 210), new Rectangle(140, 0, 20, 67), new Rectangle(168, 2, 20, 10), 0.5f);
            AddPlayer(player);
            
            asteroidFactory.AddAsteroids(3);
            asteroidFactory.BeginIntervaledProduction(5000);
        }

        public static GameField Initiate(ContentManager content)
        {
            if (_instance == null)
                _instance = new GameField(content);
            return _instance;
        }

        public static GameField GetInstance()
        {
            return _instance;
        }


        public void AddGameObject(IGameObject gameObject)
        {
            FieldObjects.Add(gameObject);
        }

        public void AddPlayer(IGameObject gameObject)
        {
            AddGameObject(gameObject);
            _playerIndex = FieldObjects.IndexOf(gameObject);
        }

        public void Update(KeyboardState keyState)
        {
            for(var i = 0; i<FieldObjects.Count; i++)
            {
                FieldObjects[i].Update(keyState);
                FieldObjects[i] = _checkBoundries(FieldObjects[i]);

                if (FieldObjects[i].IsExpired && i != _playerIndex)
                {
                    FieldObjects.RemoveAt(i);
                    i--;
                }
            }

            if (FieldObjects[_playerIndex].IsExpired)
            {
                Reset();
            }

            foreach (var factory in _gameObjectFactories)
            {
                foreach (var gameObject in factory.RetrieveObjects())
                {
                    AddGameObject(gameObject);
                }
            }

            _collisionDetector.Detect(FieldObjects);
        }

        private IGameObject _checkBoundries(IGameObject gameObject)
        {
            if (gameObject.Location.X < -60)
                gameObject.SetLocation(1980);
            else if ((gameObject.Location.X > 1980))
                gameObject.SetLocation(-60);

            if (gameObject.Location.Y < -60)
                gameObject.SetLocation(null, 1140);
            else if ((gameObject.Location.Y > 1140))
                gameObject.SetLocation(null, -60);

            return gameObject;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, new Rectangle(0, 0, 1900, 1200), Color.White);
            spriteBatch.End();

            foreach (var fieldObject in FieldObjects)
            {
                fieldObject.Draw(spriteBatch);
            }
        }
    }
}
