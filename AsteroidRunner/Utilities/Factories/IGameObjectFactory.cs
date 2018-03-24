using AsteroidRunner.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Factories
{
    public interface IGameObjectFactory
    {
        List<IGameObject> RetrieveObjects();
    }
}
