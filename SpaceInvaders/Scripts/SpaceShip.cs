using System;
using System.Drawing;
using System.Windows.Media;

namespace SpaceInvaders
{
    internal class SpaceShip : SimpleObject
    {
        #region Fields
        private Missile _missile;
        private readonly MediaPlayer _shootPlayer;
        #endregion

        #region Constructors

        /// <summary>
        /// Simple constructor
        /// </summary>
        /// <param name="speed">start speed</param>
        /// <param name="position">start position</param>
        /// <param name="lives">start lives</param>
        /// <param name="image">start image</param>
        /// <param name="side"></param>
        public SpaceShip(double speed, Vector2 position, int lives, Bitmap image, Side side) : base(speed, position, lives, image, side)
        {
            _shootPlayer = Sound.Shoot;
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
            if (_missile != null && _missile.IsAlive()) return;
            var position = Position + new Vector2(Image.Width * .5f);
            _missile = new Missile(500, position, 1, direction, Side);
            gameInstance.AddNewGameObject(_missile);
            _shootPlayer.Stop();
            _shootPlayer.Play();
        }
        #endregion
    }
}
