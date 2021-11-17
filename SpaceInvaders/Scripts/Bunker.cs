using System.Drawing;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {

        #region Constructors
        public Bunker(Vector2 position) : base(0, position, 1, new Bitmap(Properties.Resources.bunker), Side.Neutral)
        {

        }
        #endregion
        
        #region Inherited Methods
        public override void Update(Game gameInstance, double deltaT)
        {

        }

        protected override void OnCollision(SimpleObject m)
        {
            m.Lives -= IntersectsPixel(m.Position, m.Image);
        }
        #endregion

        #region Classic Methods
        private int IntersectsPixel(Vector2 missilePosition, Bitmap missileImage)
        {
            int startX = (int)(missilePosition.x - Position.x);
            var imageDimension = missileImage.Width * missileImage.Height;
            var toDestroy = imageDimension;
            for (int x = startX; x < startX + 2; x++)
            {
                if (x > Image.Width-1 || x < 0) continue;
                for (int y = Image.Height - 1; y >= 0; y--)
                {
                    var pixel = Image.GetPixel(x, y);
                    if (pixel.A == 0) continue;
                    Image.SetPixel(x, y, Color.Transparent);
                    toDestroy--;
                    if (toDestroy == 0) break;
                }
            }
            return imageDimension - toDestroy;
        }
        #endregion
    }
}
