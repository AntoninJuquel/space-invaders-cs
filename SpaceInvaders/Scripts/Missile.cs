using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        #region Fields
        /// <summary>
        /// Direction que va suivre le missile
        /// </summary>
        private Vector2 moveDirection;
        #endregion

        #region Constructors
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="speed">start speed</param>
        /// <param name="position">start position</param>
        /// <param name="lives">start lives</param>
        /// <param name="image">start image</param>
        public Missile(double speed, Vector2 position, int lives, Vector2 direction, Side side) : base(speed, position, lives, Properties.Resources.shoot1, side)
        {
            moveDirection = direction;
        }
        #endregion

        #region Inherited Methods
        public override void Update(Game gameInstance, double deltaT)
        {
            Move(moveDirection, speedPixelPerSecond, deltaT);
            if (Position.y < 0 || gameInstance.gameSize.Height < Position.y) Lives = 0;

            foreach (var gameObject in gameInstance.gameObjects)
            {
                if (gameObject == this) continue;
                gameObject.Collision(this);
            }
        }

        protected override void OnCollision(SimpleObject m)
        {
            m.Lives = Lives = 0;
        }
        #endregion
    }
}
