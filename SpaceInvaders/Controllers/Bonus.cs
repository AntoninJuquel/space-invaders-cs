using SpaceInvaders.Engine;
using System;

namespace SpaceInvaders.Controllers
{
    /// <summary>
    /// Enum of the different bonuses
    /// </summary>
    internal enum BonusType
    {
        Lives,
        MultiShot,
        FasterShot
    }

    internal class Bonus : SimpleObject
    {
        #region Fields

        /// <summary>
        /// Type of the bonus will change his effect, currently selected randomly
        /// </summary>
        private readonly BonusType _bonusType;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor to spawn a bonus at a given position and speed and select a random type of bonus
        /// </summary>
        /// <param name="position">Initial position</param>
        /// <param name="speed">Initial fall speed</param>
        public Bonus(int speed, Vector2 position) : base(speed, position, 1, Properties.Resources.bonus, Side.Neutral)
        {
            var values = Enum.GetValues(typeof(BonusType));
            var random = new Random();
            _bonusType = (BonusType) values.GetValue(random.Next(values.Length));
        }

        #endregion

        #region Inherited Methods

        /// <summary>
        /// Update it's position to fall down and detect collision with the player ship
        /// </summary>
        /// <param name="gameInstance"></param>
        /// <param name="deltaT"></param>
        public override void Update(Game gameInstance, double deltaT)
        {
            Move(Vector2.Down, SpeedPixelPerSecond, deltaT);
            Collision(gameInstance.PlayerShip);
        }

        /// <summary>
        /// Skip the collision with the missile and override the subject of the collision with the player ship
        /// </summary>
        /// <param name="simpleObject"></param>
        public override void Collision(SimpleObject simpleObject)
        {
            if (simpleObject is Missile) return;
            base.Collision(simpleObject);
        }

        /// <summary>
        /// When in collision with the player immediately disappear and apply a bonus depending on the randomly selected type
        /// </summary>
        /// <param name="simpleObject"></param>
        protected override void OnCollision(SimpleObject simpleObject)
        {
            Lives = 0;
            switch (_bonusType)
            {
                case BonusType.Lives:
                    simpleObject.Lives++;
                    break;
                case BonusType.MultiShot:
                case BonusType.FasterShot:
                    Game.GameInstance.PlayerShip.MissileSpeed += 250;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}