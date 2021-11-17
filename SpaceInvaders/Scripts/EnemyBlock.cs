using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        #region Fields
        private readonly HashSet<SpaceShip> enemmyShips;
        private readonly Random random = new Random();
        private int baseWidth;
        private Vector2 direction;
        private double randomShootProbability, randomBonusProbability;
        public Size Size
        {
            get;
            private set;
        }
        public int Count => enemmyShips.Count;

        private Bitmap[] images = new Bitmap[]
        {
            Properties.Resources.ship1,
            Properties.Resources.ship2,
            Properties.Resources.ship3,
            Properties.Resources.ship4,
            Properties.Resources.ship5,
            Properties.Resources.ship6,
            Properties.Resources.ship7,
            Properties.Resources.ship8,
            Properties.Resources.ship9
        };
        #endregion

        #region Construtors
        public EnemyBlock(Vector2 position) : base(position, 0, Side.Enemy)
        {
            enemmyShips = new HashSet<SpaceShip>();
            direction = Vector2.Right;

            LoadLevel("easy");
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
                ChangeDirection(gameInstance);

            Move(direction, speedPixelPerSecond, deltaT);
            foreach (var enemy in enemmyShips)
            {
                enemy.Move(direction, speedPixelPerSecond, deltaT);
                if (random.NextDouble() <= randomShootProbability * deltaT)
                    enemy.Shoot(gameInstance, Vector2.Down);
            }
        }
        #endregion

        #region Classic Methods
        /// <summary>
        /// Load a level from the xml file
        /// </summary>
        /// <param name="levelName"></param>
        /// <remarks>Leave levelName empty to load a random level</remarks>
        private void LoadLevel(string levelName = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"..\..\Resources\LevelEditor.xml");

            XmlNode levels = doc["levels"];
            if (levels.HasChildNodes)
            {
                XmlNode level = levelName == null ? levels.ChildNodes[random.Next(levels.ChildNodes.Count)] : levels[levelName];

                XmlNode stats = level["stats"];
                baseWidth = Convert.ToInt32(stats["width"].InnerText);
                Size = new Size(baseWidth, 0);
                speedPixelPerSecond = Convert.ToDouble(stats["speed"].InnerText);

                XmlNode probabilities = level["probabilities"];
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberGroupSeparator = ".";
                randomShootProbability = Convert.ToDouble(probabilities["shoot"].InnerText, provider);
                randomBonusProbability = Convert.ToDouble(probabilities["bonus"].InnerText, provider);

                foreach (XmlNode item in level["lines"])
                {
                    int id = Convert.ToInt32(item["id"].InnerText);
                    int num = Convert.ToInt32(item["num"].InnerText);
                    int lives = Convert.ToInt32(item["lives"].InnerText);
                    AddLine(num, lives, images[id]);
                }
            }
        }
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
            Size += new Size(0, shipImage.Height);
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

        private void ChangeDirection(Game gameInstance)
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
