using AsteroidRunner.GameObjects;
using AsteroidRunner.GameObjects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Factories
{
    public class ProjectileFactory : IGameObjectFactory
    {
        private Texture2D _projectileTexture;
        private List<Projectile> _newProjectiles;
        private static ProjectileFactory _instance;

        private ProjectileFactory(ContentManager content)
        {
            _projectileTexture = content.Load<Texture2D>("Sprites/Projectiles");
            _newProjectiles = new List<Projectile>();
        }

        public static ProjectileFactory Initiate(ContentManager content)
        { 
            if (_instance == null)
                _instance = new ProjectileFactory(content);
            return _instance;
        }

        public static ProjectileFactory GetInstance()
        {
            return _instance;
        }

        public void AddStandardProjectile(Vector2 location, float direction, Color color)
        {
            _newProjectiles.Add(new Projectile(_projectileTexture, new Rectangle(0, 0, 8, 30), direction, location, color, 1));
        }

        /// <summary>
        /// Returns list of newly created projectiles.
        /// </summary>
        /// <returns></returns>
        public List<IGameObject> RetrieveObjects()
        {
            var gameObjects =  _newProjectiles.ToList<IGameObject>();
            _newProjectiles.Clear();
            return gameObjects;
        }

        

    }
}
