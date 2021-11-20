using System.Xml;

namespace SpaceInvaders.Managers
{
    internal static class Score
    {
        #region Fields

        /// <summary>
        /// Total points earned killing enemies
        /// </summary>
        private static int Point { get; set; }

        /// <summary>
        /// Current level
        /// </summary>
        public static int Level { get; private set; }

        /// <summary>
        /// Path of the xml scoreboard to save score
        /// </summary>
        private const string XmlPath = @"..\..\Resources\Scoreboard.xml";

        #endregion

        #region Private Methods

        /// <summary>
        /// Reset the level and the point and save before that
        /// </summary>
        private static void Reset()
        {
            Save();

            Point = 0;
            Level = 0;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saving score to the xml file only if the score is above one of them
        /// </summary>
        public static void Save()
        {
            var doc = new XmlDocument();
            doc.Load(XmlPath);

            XmlNode scores = doc["scores"];
            if (scores == null) return;

            foreach (XmlNode node in scores.ChildNodes)
            {
                if (System.Convert.ToInt32(node.InnerText) >= Point) continue;
                node.InnerText = Point.ToString();
                break;
            }

            doc.Save(XmlPath);
        }

        /// <summary>
        /// Add a certain amount to the points
        /// </summary>
        /// <param name="value"></param>
        public static void AddPoint(int value)
        {
            Point += value;
        }

        /// <summary>
        /// Update the current level depending if we have to reset it or increment it
        /// </summary>
        /// <param name="reset">Did I lost ?</param>
        public static void UpdateLevel(bool reset)
        {
            if (reset)
                Reset();
            else
                Level++;
        }

        /// <summary>
        /// Override ToString() method to return a formated string
        /// </summary>
        /// <returns>A formatted string with the level above the points</returns>
        public new static string ToString() => string.Concat("Level : ", Level, "\n", Point.ToString("000000"));

        public static string ScoreBoard()
        {
            var scoreboard = "";

            var doc = new XmlDocument();
            doc.Load(XmlPath);

            XmlNode scores = doc["scores"];
            if (scores == null) return scoreboard;

            foreach (XmlNode node in scores.ChildNodes)
            {
                scoreboard += string.Concat(System.Convert.ToInt32(node.InnerText).ToString("000000"), "\n");
            }

            return scoreboard;
        }

        #endregion
    }
}