using System.Drawing;

namespace SpaceInvaders.Engine
{
    internal abstract class SimpleObject : GameObject
    {
        #region Fields

        /// <summary>
        /// Lives number of a simple object
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// Object image
        /// </summary>
        public Bitmap Image { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to spawn a simple object at a given position
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="position"></param>
        /// <param name="lives"></param>
        /// <param name="image"></param>
        /// <param name="side"></param>
        protected SimpleObject(double speed, Vector2 position, int lives, Bitmap image, Side side) : base(position,
            speed, side)
        {
            Lives = lives;
            Image = image;
        }

        #endregion

        #region Inherited Methods

        /// <summary>
        /// Draw the object image at his position with a correct size
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="graphics"></param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float) Position.X, (float) Position.Y, Image.Width, Image.Height);
        }

        /// <summary>
        /// Return if the object has enough lives to continue living
        /// </summary>
        /// <returns>Are my lives above 0 ?</returns>
        public override bool IsAlive() => Lives > 0;

        /// <summary>
        /// Calculate collisions by calculating height and width based on the image of the two simple object
        /// </summary>
        /// <param name="simpleObject">other</param>
        public override void Collision(SimpleObject simpleObject)
        {
            if (simpleObject.Side == Side) return;

            var x1 = Position.X;
            var y1 = Position.Y;
            var lx1 = Image.Width;
            var ly1 = Image.Height;

            var x2 = simpleObject.Position.X;
            var y2 = simpleObject.Position.Y;
            var lx2 = simpleObject.Image.Width;
            var ly2 = simpleObject.Image.Height;

            var collision = !(x1 > x2 + lx2 || x2 > x1 + lx1 || y1 > y2 + ly2 || y2 > y1 + ly1);

            if (collision) OnCollision(simpleObject);
        }

        /// <summary>
        /// Called when a collision with a simple object consequences depend on the current object type
        /// </summary>
        /// <param name="simpleObject">other</param>
        protected abstract void OnCollision(SimpleObject simpleObject);

        #endregion
    }
}