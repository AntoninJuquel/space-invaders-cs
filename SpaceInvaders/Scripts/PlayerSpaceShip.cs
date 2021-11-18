using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceInvaders
{
    class PlayerSpaceShip : SpaceShip
    {

        #region Constructors
        public PlayerSpaceShip(double speed, Vector2 position, int lives) : base(speed, position, lives, SpaceInvaders.Properties.Resources.ship3, Side.Ally)
        {

        }
        #endregion

        #region Inherited Methods
        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.KeyPressed.Contains(Keys.Left) && Position.x > 0)
            {
                Move(Vector2.Left, speedPixelPerSecond, deltaT);
            }
            if (gameInstance.KeyPressed.Contains(Keys.Right) && Position.x + Image.Width < gameInstance.GameSize.Width)
            {
                Move(Vector2.Right, speedPixelPerSecond, deltaT);
            }
            if (gameInstance.KeyPressed.Contains(Keys.Space))
            {
                gameInstance.ReleaseKey(Keys.Space);
                Shoot(gameInstance, Vector2.Up);
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);

            var text = string.Concat(Lives, " LIVES");
            var size = graphics.MeasureString(text, Game.DefaultFont);
            graphics.DrawString(text, Game.DefaultFont, Game.BlackBrush, gameInstance.GameSize.Width - size.Width, 0);
        }
        #endregion
    }
}
