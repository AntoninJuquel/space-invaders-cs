using System;
using System.Drawing;
using System.Windows.Media;

namespace SpaceInvaders
{
    class SpaceShip : SimpleObject
    {
        #region Fields
        private Missile missile;
        private MediaPlayer shoot;
        #endregion

        #region Constructors
        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="speed">start speed</param>
        /// <param name="position">start position</param>
        /// <param name="lives">start lives</param>
        /// <param name="image">start image</param>
        public SpaceShip(double speed, Vector2 position, int lives, Bitmap image, Side side) : base(speed, position, lives, image, side)
        {
            shoot = Sound.shoot;
        }
        #endregion

        #region Inherited Methods
        public override void Update(Game gameInstance, double deltaT)
        {
            
        }

        protected override void OnCollision(SimpleObject m)
        {
            var min = Math.Min(m.Lives, Lives);
            Lives -= min;
            m.Lives -= min;
        }
        #endregion

        #region Methods
        public void Shoot(Game gameInstance,Vector2 direction)
        {
            if (missile != null && missile.IsAlive()) return;
            var position = Position + new Vector2(Image.Width * .5f, 0);
            missile = new Missile(500, position, 1, direction, Side);
            gameInstance.AddNewGameObject(missile);
            shoot.Stop();
            shoot.Play();
        }
        #endregion
    }
}
