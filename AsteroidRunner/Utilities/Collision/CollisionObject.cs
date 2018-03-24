using AsteroidRunner.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Collision
{
    class CollisionObject
    {
        public IGameObject GameObject { get; set; }
        public Color[,] TextureColors { get; set; }
        public Matrix Matrix { get; set; }

    }
}
