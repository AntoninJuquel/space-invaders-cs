using System.Linq;

namespace SpaceInvaders
{
    internal class Missile : SimpleObject
    {
        #region Fields

        /// <summary>
        /// Direction the missile move along
        /// </summary>
        private readonly Vector2 _moveDirection;

        #endregion

        #region Constructors

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="speed">Speed of the missile</param>
        /// <param name="position">Start position of the missile</param>
        /// <param name="lives">Lives of the missiles</param>
        /// <param name="direction">Direction of the missile</param>
        /// <param name="side">Side of the missile</param>
        public Missile(double speed, Vector2 position, int lives, Vector2 direction, Side side) : base(speed, position,
            lives, Properties.Resources.shoot1, side)
        {
            _moveDirection = direction;
        }

        #endregion

        #region Inherited Methods

        public override void Update(Game gameInstance, double deltaT)
        {
            Move(_moveDirection, SpeedPixelPerSecond, deltaT);
            if (Position.Y < 0 || gameInstance.GameSize.Height < Position.Y) Lives = 0;

            foreach (var gameObject in gameInstance.GameObjects.Where(gameObject => gameObject != this))
                gameObject.Collision(this);
        }

        protected override void OnCollision(SimpleObject m)
        {
            m.Lives = Lives = 0;
        }

        #endregion
    }
}