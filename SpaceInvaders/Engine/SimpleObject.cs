using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        #region Fields
        /// <summary>
        /// Nombre de vie de l'objet
        /// </summary>
        public int Lives
        {
            get;
            set;
        }
        /// <summary>
        /// Image représentant l'objet
        /// </summary>
        public Bitmap Image
        {
            get;
            protected set;
        }
        #endregion

        #region Constructors
        public SimpleObject(double speed, Vector2 position, int lives, Bitmap image, Side side) : base(position, speed, side)
        {
            Lives = lives;
            Image = image;
        }
        #endregion

        #region Inherited Methods
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive() => Lives > 0;

        public override void Collision(SimpleObject simpleObject)
        {
            if (simpleObject.Side == Side) return;

            double x1 = Position.x;
            double y1 = Position.y;
            double lx1 = Image.Width;
            double ly1 = Image.Height;

            double x2 = simpleObject.Position.x;
            double y2 = simpleObject.Position.y;
            double lx2 = simpleObject.Image.Width;
            double ly2 = simpleObject.Image.Height;

            bool collision = !((x1 > x2 + lx2) || (x2 > x1 + lx1) || (y1 > y2 + ly2) || (y2 > y1 + ly1));

            if (collision) OnCollision(simpleObject);
        }

        protected abstract void OnCollision(SimpleObject simpleObject);
        #endregion
    }
}
