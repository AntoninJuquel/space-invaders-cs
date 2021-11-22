using SpaceInvaders.Engine;
using System.Drawing;

namespace SpaceInvaders.Controllers
{
    internal class Bunker : SimpleObject
    {
        #region Constructors

        /// <summary>
        /// Simple constructor to place the bunker at a certain position
        /// </summary>
        /// <param name="position"></param>
        public Bunker(Vector2 position) : base(0, position, 1, new Bitmap(Properties.Resources.bunker), Side.Neutral)
        {

        }

        #endregion

        #region Inherited Methods

        public override void Update(Game gameInstance, double deltaT)
        {
        }

        /// <summary>
        /// When on collision with a missile decrease the missile lives depending on the pixel in collision
        /// </summary>
        /// <param name="m"></param>
        protected override void OnCollision(SimpleObject simpleObject)
        {
            if (simpleObject is Missile)
                simpleObject.RemoveLives(IntersectsPixel(simpleObject));
        }

        #endregion

        #region Classic Methods

        /// <summary>
        /// Calculate the number of pixels in collision by converting the missile's pixels position into local position and update the pixels color
        /// </summary>
        /// <param name="missile"></param>
        /// <returns>Number of pixels hit</returns>
        private int IntersectsPixel(SimpleObject simpleObject)
        {
            var startX = simpleObject.Position.X - Position.X;
            var startY = simpleObject.Position.Y - Position.Y;
            var count = 0;

            for (var y = (int)startY; y < (int)startY + simpleObject.Image.Height; y++)
            {
                if (y < 0 || y >= Image.Height) continue;
                for (var x = (int)startX; x < (int)startX + simpleObject.Image.Width; x++)
                {
                    if (x < 0 || x >= Image.Width || Image.GetPixel(x, y).A == 0) continue;

                    Image.SetPixel(x, y, Color.Transparent);
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}