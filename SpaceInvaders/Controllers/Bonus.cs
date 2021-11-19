using SpaceInvaders.Engine;

namespace SpaceInvaders.Controllers
{
    internal class Bonus : SimpleObject
    {
        public Bonus(Vector2 position) : base(50, position, 1, Properties.Resources.bonus, Side.Ally)
        {
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            Move(Vector2.Down, SpeedPixelPerSecond, deltaT);
            Collision(gameInstance.PlayerShip);
        }

        public override void Collision(SimpleObject simpleObject)
        {
            if (simpleObject is Missile) return;

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

        protected override void OnCollision(SimpleObject simpleObject)
        {
            Lives = 0;
        }
    }
}