using SpaceInvaders.Engine;
using SpaceInvaders.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace SpaceInvaders.Controllers
{
    internal class EnemyBlock : GameObject
    {
        #region Fields

        /// <summary>
        /// Enemy ships living in the block
        /// </summary>
        private readonly HashSet<SpaceShip> enemyShips;

        /// <summary>
        /// Random class to generate random number
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// Current moving direction of the block
        /// </summary>
        private Vector2 direction;

        /// <summary>
        /// Start width of the block
        /// </summary>
        private int baseWidth;

        /// <summary>
        /// Shoot probability of enemies
        /// </summary>
        private double randomShootProbability;

        /// <summary>
        /// Bonus spawn chance on enemy death
        /// </summary>
        private double randomBonusProbability;

        /// <summary>
        /// Size of the block
        /// </summary>
        private Size Size { get; set; }

        /// <summary>
        /// Array of ship images to access it from index
        /// </summary>
        private readonly Bitmap[] images = new Bitmap[]
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

        /// <summary>
        /// Path of the xml level editor to load levels
        /// </summary>
        private const string XmlPath = @"..\..\Resources\LevelEditor.xml";

        #endregion

        #region Construtors

        /// <summary>
        /// Constructor to spawn enemy ships in block shape on the top left corner depending on the level
        /// </summary>
        /// <param name="position"></param>
        public EnemyBlock(Vector2 position) : base(position, 0, Side.Enemy)
        {
            enemyShips = new HashSet<SpaceShip>();
            direction = Vector2.Right;

            LoadLevel();
        }

        #endregion

        #region InHerited Methods

        /// <summary>
        /// Test the collision with the missile on each enemy and randomly spawn a bonus after a death
        /// </summary>
        /// <param name="m"></param>
        public override void Collision(SimpleObject m)
        {
            foreach (var enemy in enemyShips)
                enemy.Collision(m);

            enemyShips.RemoveWhere(ship =>
            {
                var dead = !ship.IsAlive();

                if (dead)
                {
                    Score.AddPoint(ship.BaseLives * 100);
                    if (random.NextDouble() <= randomBonusProbability)
                    {
                        var bonus = new Bonus(50, ship.Position);
                        Game.GameInstance.AddNewGameObject(bonus);
                    }
                }

                return dead;
            });
        }

        /// <summary>
        /// Draw each SpaceShip in the _enemyShips HashSet
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="graphics"></param>
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach (var enemy in enemyShips)
                enemy.Draw(gameInstance, graphics);
            graphics.DrawRectangle(new Pen(Color.Red), (float)Position.X, (float)Position.Y, Size.Width, Size.Height);
        }

        /// <summary>
        /// EnemyBlock is alive if the size of the enemy ships is above 0. If false the game is won
        /// </summary>
        /// <returns>Is there any ship left ?</returns>
        public override bool IsAlive() => enemyShips.Count > 0;

        /// <summary>
        /// Called every frame to update each enemies position, shoot randomly and update the size of the block
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT)
        {
            if (Position.X + direction.X < 0 || Position.X + Size.Width + direction.X > gameInstance.GameSize.Width)
                ChangeDirection(gameInstance);

            foreach (var enemy in enemyShips)
            {
                enemy.Move(direction, speedPixelPerSecond, deltaT);
                if (random.NextDouble() <= randomShootProbability * deltaT)
                    enemy.Shoot(gameInstance, Vector2.Down);
            }

            Position.X = (int)enemyShips.Min(ship => ship.Position.X);
            Position.Y = (int)enemyShips.Min(ship => ship.Position.Y);
            UpdateSize();
        }

        #endregion

        #region Classic Methods

        /// <summary>
        /// Load the stats and probabilities from the level data
        /// </summary>
        /// <param name="level"></param>
        private void LoadLevelData(XmlNode level)
        {
            XmlNode stats = level["stats"];
            baseWidth = Convert.ToInt32(stats["width"]?.InnerText);
            Size = new Size(baseWidth, 0);
            speedPixelPerSecond = Convert.ToDouble(stats["speed"]?.InnerText);

            XmlNode probabilities = level["probabilities"];
            var provider = new NumberFormatInfo
            {
                NumberGroupSeparator = "."
            };
            randomShootProbability = Convert.ToDouble(probabilities["shoot"]?.InnerText, provider);
            randomBonusProbability = Convert.ToDouble(probabilities["bonus"]?.InnerText, provider);
        }

        /// <summary>
        /// Spawn each lines based on the level data
        /// </summary>
        /// <param name="level"></param>
        private void SpawnLines(XmlNode level)
        {
            foreach (XmlNode item in level["lines"])
            {
                var id = Convert.ToInt32(item["id"]?.InnerText);
                var num = Convert.ToInt32(item["num"]?.InnerText);
                var lives = Convert.ToInt32(item["lives"]?.InnerText);
                AddLine(num, lives, images[id]);
            }
        }

        /// <summary>
        /// Load a level from the xml file
        /// </summary>
        private void LoadLevel()
        {
            var doc = new XmlDocument();
            doc.Load(XmlPath);

            XmlNode levels = doc["levels"];
            if (levels == null || !levels.HasChildNodes) return;
            var level = levels.ChildNodes[Score.Level % levels.ChildNodes.Count];
            LoadLevelData(level);
            SpawnLines(level);
        }

        /// <summary>
        /// Add a line to the block
        /// </summary>
        /// <param name="nbShips"></param>
        /// <param name="nbLives"></param>
        /// <param name="shipImage"></param>
        private void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            var y = Size.Height;
            var split = baseWidth / nbShips;
            for (var i = 0; i < nbShips; i++)
            {
                var position = new Vector2(split * (i + .5f) - shipImage.Width * .5f, y) + Position;
                var enemy = new SpaceShip(speedPixelPerSecond, position, nbLives, shipImage, Side);
                enemyShips.Add(enemy);
            }

            Size += new Size(0, shipImage.Height);
        }

        /// <summary>
        /// Update the size of the block based on the ships position
        /// </summary>
        private void UpdateSize()
        {
            var maxX = (int)enemyShips.Max(ship => ship.Position.X + ship.Image.Width);
            var maxY = (int)enemyShips.Max(ship => ship.Position.Y + ship.Image.Height);

            Size = new Size(maxX - (int)Position.X, maxY - (int)Position.Y);
        }

        /// <summary>
        /// Change the direction from one side to the other and move down the block
        /// </summary>
        /// <param name="gameInstance"></param>
        private void ChangeDirection(Game gameInstance)
        {
            direction *= -1;
            speedPixelPerSecond += 10;
            foreach (var enemy in enemyShips)
                enemy.Move(Vector2.Down, 10);

            randomShootProbability += .01;

            if (Position.Y >= gameInstance.PlayerShip.Position.Y)
                gameInstance.PlayerShip.Die();
        }

        #endregion
    }
}