using SpaceInvaders.Engine;
using SpaceInvaders.Managers;
using System;
using System.Drawing;
using System.Windows.Media;

namespace SpaceInvaders.Controllers
{
    internal class SpaceShip : SimpleObject
    {
        #region Fields

        private Missile missile;
        private readonly MediaPlayer shootPlayer = Sound.Shoot;
        private double missileSpeed = 500;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to spawn a spaceship at a given position, his side will determine which missile to ignore
        /// </summary>
        /// <param name="speed">start speed</param>
        /// <param name="position">start position</param>
        /// <param name="lives">start lives</param>
        /// <param name="image">start image</param>
        /// <param name="side"></param>
        public SpaceShip(double speed, Vector2 position, int lives, Bitmap image, Side side) : base(speed, position, lives, image, side)
        {

        }

        #endregion

        #region Inherited Methods

        public override void Update(Game gameInstance, double deltaT)
        {
        }

        /// <summary>
        /// On collision remove lives to both of the missile and the spaceship, the weaker will die
        /// </summary>
        /// <param name="simpleObject">simple object who called for collision</param>
        protected override void OnCollision(SimpleObject simpleObject)
        {
            if (simpleObject is Missile)
            {
                var min = Math.Min(simpleObject.Lives, Lives);
                RemoveLives(min);
                simpleObject.RemoveLives(min);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shoot a missile at spaceship position with a given direction to move along with, skip if a missile from this spaceship is already on the screen
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="direction"></param>
        public void Shoot(Game gameInstance, Vector2 direction)
        {
            if (missile != null && missile.IsAlive()) return;
            var position = Position + new Vector2(Image.Width * .5f);
            missile = new Missile(missileSpeed, position, 1, direction, Side);
            gameInstance.AddNewGameObject(missile);
            shootPlayer.Stop();
            shootPlayer.Play();
        }

        public void AddMissileSpeed(int amount)
        {
            missileSpeed += amount;
        }

        #endregion
    }
}