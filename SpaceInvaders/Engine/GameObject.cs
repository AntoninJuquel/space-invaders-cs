using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the generic abstact base class for any entity in the game
    /// </summary>
    public enum Side
    {
        Ally, Enemy, Neutral
    }
    abstract class GameObject
    {
        public Side Side
        {
            get;
            private set;
        }
        /// <summary>
        /// Position de l'objet
        /// </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary>
        /// Vitesse de deplacement de l'objet
        /// </summary>
        protected double speedPixelPerSecond;

        public GameObject(Vector2 position, double speed, Side side)
        {
            Position = position;
            speedPixelPerSecond = speed;
            Side = side;
        }

        /// <summary>
        /// Update the state of a game objet
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time ellapsed in seconds since last call to Update</param>
        public abstract void Update(Game gameInstance, double deltaT);

        /// <summary>
        /// Render the game object
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="graphics">graphic object where to perform rendering</param>
        public abstract void Draw(Game gameInstance, Graphics graphics);

        /// <summary>
        /// Determines if object is alive. If false, the object will be removed automatically.
        /// </summary>
        /// <returns>Am I alive ?</returns>
        public abstract bool IsAlive();

        public abstract void Collision(SimpleObject simpleObject);

        #region Protected Methods
        public void Move(Vector2 direction, double speed, double delatT)
        {
            Position += direction * (speed * delatT);
        }
        #endregion
    }
}
