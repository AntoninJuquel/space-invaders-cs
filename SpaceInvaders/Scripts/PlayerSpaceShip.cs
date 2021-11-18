using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    internal class PlayerSpaceShip : SpaceShip
    {
        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="position"></param>
        /// <param name="lives"></param>
        public PlayerSpaceShip(double speed, Vector2 position, int lives) : base(speed, position, lives,
            SpaceInvaders.Properties.Resources.ship3, Side.Ally)
        {
        }
        #endregion

        #region Inherited Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT)
        {
            HandleMovements(gameInstance, deltaT);
            HandleShoot(gameInstance);
        }
        
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private void HandleMovements(Game gameInstance, double deltaT)
        {
            if (gameInstance.KeyPressed.Contains(Keys.Left) && Position.X > 0)
                Move(Vector2.Left, SpeedPixelPerSecond, deltaT);
            if (gameInstance.KeyPressed.Contains(Keys.Right) && Position.X + Image.Width < gameInstance.GameSize.Width)
                Move(Vector2.Right, SpeedPixelPerSecond, deltaT);
        }
        
        /// <summary>
        /// 
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