using System.Windows.Forms;
using System.Drawing;
using SpaceInvaders.Engine;

namespace SpaceInvaders.Controllers
{
    internal class PlayerSpaceShip : SpaceShip
    {
        #region Constructors

        /// <summary>
        /// Constructor to spawn a player ship accepting inputs at a given position
        /// </summary>
        /// <param name="speed">Initial speed</param>
        /// <param name="position">Starting position</param>
        /// <param name="lives">Starting lives</param>
        public PlayerSpaceShip(double speed, Vector2 position, int lives) : base(speed, position, lives,
            Properties.Resources.ship3, Side.Ally)
        {
        }

        #endregion

        #region Inherited Methods

        /// <summary>
        /// Read player inputs and handle movements and shooting
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT)
        {
            HandleMovements(gameInstance, deltaT);
            HandleShoot(gameInstance);
        }

        /// <summary>
        /// Draw text lives and player spaceship
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="graphics"></param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);

            var text = string.Concat(Lives, " LIVES");
            var size = graphics.MeasureString(text, Game.DefaultFont);
            graphics.DrawString(text, Game.DefaultFont, Game.BlackBrush, gameInstance.GameSize.Width - size.Width, 0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Move along the X axis depending on the input at a certain speed
        /// </summary>
        private void HandleMovements(Game gameInstance, double deltaT)
        {
            if (gameInstance.KeyPressed.Contains(Keys.Left) && Position.X > 0)
                Move(Vector2.Left, SpeedPixelPerSecond, deltaT);
            if (gameInstance.KeyPressed.Contains(Keys.Right) && Position.X + Image.Width < gameInstance.GameSize.Width)
                Move(Vector2.Right, SpeedPixelPerSecond, deltaT);
        }

        /// <summary>
        /// Call shoot if the player hit space
        /// </summary>
        /// <param name="gameInstance"></param>
        private void HandleShoot(Game gameInstance)
        {
            if (!gameInstance.KeyPressed.Contains(Keys.Space)) return;
            gameInstance.ReleaseKey(Keys.Space);
            Shoot(gameInstance, Vector2.Up);
        }

        #endregion
    }
}