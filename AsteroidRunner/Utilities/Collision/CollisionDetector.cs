using AsteroidRunner.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Collision
{
    public class CollisionDetector
    {

        public void Detect(List<IGameObject> gameObjects)
        {
            
            for(var i = 0; i < gameObjects.Count-1; i++)
            { 
                for (var j = i+1; j < gameObjects.Count; j++)
                {
                    //Check if the objects hit boxes collide
                    if (gameObjects[i].HitBox.Intersects(gameObjects[j].HitBox)){

                        //if they do, check to see if there textures collide
                        var objectA = _getCollisionObject(gameObjects[i]);
                        var objectB = _getCollisionObject(gameObjects[j]);

                        var collisionLocation = _texturesCollide(objectA, objectB);
                        if (collisionLocation.HasValue)
                        {
                            gameObjects[i].ProcessCollision(gameObjects[j]);
                            gameObjects[j].ProcessCollision(gameObjects[i]);
                        }
                    }
                }
            }
        }
        
        private CollisionObject _getCollisionObject(IGameObject gameObject)
        {
            return new CollisionObject()
            {
                GameObject = gameObject, //Helps with debugging to have this here
                TextureColors = gameObject.TextureColors(),
                Matrix = _getObjectMatrix(gameObject)
            };
        }
        

        //http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Coll_Detection_Matrices.php
        private Matrix _getObjectMatrix(IGameObject gameObject)
        {
            var location = gameObject.Location;
            var dimensions = gameObject.Dimensions;
            var rotation = gameObject.Direction;
            var scale = gameObject.Scale;
            
            return  Matrix.CreateTranslation(-gameObject.Origin.X, -gameObject.Origin.Y, 0) *
                    Matrix.CreateRotationZ(rotation) *
                    Matrix.CreateScale(scale) *
                    Matrix.CreateTranslation(location.X, location.Y, 0);
        }


        private Vector2? _texturesCollide(CollisionObject objectA, CollisionObject objectB)
        {
            var normalizedMatrix = objectA.Matrix * Matrix.Invert(objectB.Matrix);

            var widthA = objectA.TextureColors.GetLength(0);
            var heightA = objectA.TextureColors.GetLength(1);

            var widthB = objectB.TextureColors.GetLength(0);
            var heightB= objectB.TextureColors.GetLength(1);
            
            for (var x = 0; x < widthA;  x++)
            {
                for (var y = 0; y < heightA; y++)
                {
                    var position = new Vector2(x, y);
                    var transPosition = Vector2.Transform(position, normalizedMatrix);

                    var transX = (int)transPosition.X;
                    var transY = (int)transPosition.Y;
                    
                    //Check to make sure the matricies colide at this pixel, and that both have an alpha of 0
                    if ((transX >= 0) && (transX < widthB) &&
                        (transY >= 0) && (transY < heightB) &&
                        (objectA.TextureColors[x, y].A > 0) &&
                        (objectB.TextureColors[transX, transY].A > 0))
                        //Screen Position of collision
                        return Vector2.Transform(position, objectA.Matrix); 
                }
            }

            return null;
        }


    }
}
