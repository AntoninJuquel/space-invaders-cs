using System;
using System.Diagnostics;
using System.Drawing;

namespace SpaceInvaders
{
    internal class Bunker : SimpleObject
    {
        #region Constructors

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected override void OnCollision(SimpleObject m)
        {
            m.Lives -= IntersectsPixel(m as Missile);
        }

        #endregion

        #region Classic Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missile"></param>
        /// <returns></returns>
        private int IntersectsPixel(Missile missile)
        {
            var startX = missile.Position.X - Position.X;
            var startY = missile.Position.Y - Position.Y;
            var count = 0;

            for (int y = (int)startY; y < (int)startY + missile.Image.Height; y++)
            {
                if(y < 0 || y >=Image.Height) continue;
                for (int x = (int)startX; x < (int)startX + missile.Image.Width; x++)
                {
                    if (x < 0 || x >= Image.Width) continue;
                    if (Image.GetPixel(x, y).A == 0) continue;

                    Image.SetPixel(x, y, Color.Transparent);
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}