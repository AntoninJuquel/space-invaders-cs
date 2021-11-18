using System.Drawing;

namespace SpaceInvaders
{
    /// <summary>
    /// This is the generic abstract base class for any entity in the game
    /// </summary>
    public enum Side
    {
        Ally,
        Enemy,
        Neutral
    }

    internal abstract class GameObject
    {
        protected Side Side { get; private set; }

        /// <summary>
        /// Object position
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Object movement speed
        /// </summary>
        protected double SpeedPixelPerSecond;

        protected GameObject(Vector2 position, double speed, Side side)
        {
            Position = position;
            SpeedPixelPerSecond = speed;
            Side = side;
        }

        /// <summary>
        /// Update the state of a game objet
        /// </summary>
        /// <param name="gameInstance">instance of the current game</param>
        /// <param name="deltaT">time elapsed in seconds since last call to Update</param>
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

        public void Move(Vector2 direction, double speed, double deltaT)
        {
            Position += direction * (speed * deltaT);
        }

        #endregion
    }
}