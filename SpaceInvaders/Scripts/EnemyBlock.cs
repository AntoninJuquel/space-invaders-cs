using System;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        #region Fields
        private HashSet<SpaceShip> enemmyShips;
        private int baseWidth;
        private Vector2 direction;
        private double randomShootProbability, randomBonusProbability;
        private Random random;
        public Size Size
        {
            get;
            private set;
        }
        public int Count => enemmyShips.Count;
        #endregion

        #region Construtors
        public EnemyBlock(int width, Vector2 position, double speed) : base(position, speed, Side.Enemy)
        {
            baseWidth = width;
            enemmyShips = new HashSet<SpaceShip>();
            direction = Vector2.Right;
            randomShootProbability = .1;
            randomBonusProbability = .1;
            random = new Random();
            AddLine(9, 1, Properties.Resources.ship5);
            AddLine(3, 1, Properties.Resources.ship6);
            AddLine(5, 1, Properties.Resources.ship7);
        }
        #endregion

        #region InHerited Methods
        public override void Collision(SimpleObject m)
        {
            foreach (var enemy in enemmyShips)
                enemy.Collision(m);

            enemmyShips.RemoveWhere(ship =>
            {
                bool dead = !ship.IsAlive();

                if (dead && random.NextDouble() <= randomBonusProbability)
                {
                    var bonus = new Bonus(ship.Position);
                    Game.game.AddNewGameObject(bonus);
                }

                return dead;
            });
            UpdateSize();
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (var enemy in enemmyShips)
                enemy.Draw(gameInstance, graphics);
            graphics.DrawRectangle(new Pen(Color.Red), (float)Position.x, (float)Position.y, Size.Width, Size.Height);
        }

        public override bool IsAlive() => enemmyShips.Count > 0;

        public override void Update(Game gameInstance, double deltaT)
        {
            if (Position.x + direction.x < 0 || Position.x + Size.Width + direction.x > gameInstance.gameSize.Width)
                UpdateDirection(gameInstance);

            Move(direction, speedPixelPerSecond, deltaT);
            foreach (var enemy in enemmyShips)
            {
                enemy.Move(direction, speedPixelPerSecond, deltaT);
                if (random.NextDouble() <= randomShootProbability * deltaT)
                    enemy.Shoot(gameInstance,Vector2.Down);
            }
        }
        #endregion

        #region Classic Methods
        private void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            var y = Size.Height;
            var split = baseWidth / nbShips;
            for (int i = 0; i < nbShips; i++)
            {
                var position = new Vector2(split * (i + .5f) - shipImage.Width * .5f, y) + Position;
                var enemy = new SpaceShip(speedPixelPerSecond, position, nbLives, shipImage, Side);
                enemmyShips.Add(enemy);
            }
            UpdateSize();
        }

        private void UpdateSize()
        {
            Size = new Size();
            Bitmap currentImage = null;
            int lineWidth = 0;
            double position = double.MaxValue;

            foreach (var enemy in enemmyShips)
            {
                if (enemy.Image != currentImage) // new line
                {
                    currentImage = enemy.Image;
                    Size = new Size(Math.Max(Size.Width, lineWidth), Size.Height + currentImage.Height);
                    Position.x = Math.Min(position, enemy.Position.x);
                    position = Position.x;
                }
                lineWidth = enemy.Image.Width + (int)enemy.Position.x - (int)Position.x;
            }
            Size = new Size(Math.Max(Size.Width, lineWidth), Size.Height);
        }

        private void UpdateDirection(Game gameInstance)
        {
            direction *= -1;
            Position += Vector2.Down * 10;
            speedPixelPerSecond += 10;
            foreach (var enemy in enemmyShips)
                enemy.Position += Vector2.Down * 10;

            randomShootProbability += .025;

            if (Position.y >= gameInstance.PlayerShip.Position.y)
                gameInstance.PlayerShip.Lives = 0;
        }
        #endregion
    }
}
