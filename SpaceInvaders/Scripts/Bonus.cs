using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Bonus : SimpleObject
    {
        public Bonus(Vector2 position) : base(50, position, 1, Properties.Resources.bonus, Side.Ally)
        {
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            Move(Vector2.Down, speedPixelPerSecond, deltaT);
            Collision(gameInstance.PlayerShip);
        }

        public override void Collision(SimpleObject simpleObject)
        {
            if (simpleObject is Missile) return;

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

        protected override void OnCollision(SimpleObject simpleObject)
        {
            Lives = 0;
        }
    }
}
