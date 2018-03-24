using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.GameObjects.Primitives
{
    public class Triangle
    {
        private BasicEffect _basicEffect;
        private List<VertexPositionColor> _initialTriange;

        private List<VertexPositionColor> _currentTriange;
        private GraphicsDevice _graphicsDevice;

        private Vector2 _location;

        public Triangle(GraphicsDevice graphicsDevice, Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            _graphicsDevice = graphicsDevice;

            _basicEffect = new BasicEffect(graphicsDevice)
            {
                World = Matrix.CreateOrthographicOffCenter(
                    0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, 0, 0, 1),

                LightingEnabled = false,
                VertexColorEnabled = true
            };

            _initialTriange = new List<VertexPositionColor>
             {
                new VertexPositionColor(new Vector3(pointA.X, pointA.Y, 0), Color.Green),
                new VertexPositionColor(new Vector3(pointB.X, pointB.Y, 0), Color.Red),
                new VertexPositionColor(new Vector3(pointC.X, pointC.Y, 0), Color.Blue)
            };

            _currentTriange = new List<VertexPositionColor>();

            foreach (var vertex in _initialTriange)
                _currentTriange.Add(new VertexPositionColor(new Vector3(vertex.Position.X, vertex.Position.Y, 0), vertex.Color));
        }
        
        public void Update(KeyboardState keyState)
        {
            if (keyState.IsKeyDown(Keys.Right))
                _location.X += 10;
            if (keyState.IsKeyDown(Keys.Left))
                _location.X -= 10;
            if (keyState.IsKeyDown(Keys.Up))
                _location.Y -= 10;
            if (keyState.IsKeyDown(Keys.Down))
                _location.Y += 10;
        }

        public void Draw()
        {

            for (var i = 0; i < _currentTriange.Count; i++)
            {
                _currentTriange[i] = new VertexPositionColor(
                    new Vector3(
                        _initialTriange[i].Position.X + _location.X,
                        _initialTriange[i].Position.Y + _location.Y, 
                        0),
                    _initialTriange[i].Color);

            }

            _basicEffect.CurrentTechnique.Passes[0].Apply();
            _graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _currentTriange.ToArray(), 0, 1);
        }

    }
}
