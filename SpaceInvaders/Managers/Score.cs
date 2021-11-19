using SpaceInvaders.Engine;

namespace SpaceInvaders.Managers
{
    internal static class Score
    {
        public static int Value { get; private set; }
        public static int Level { get; private set; }

        public static void AddScore(int value)
        {
            Value += value;
        }
        public static void UpdateLevel()
        {
            if (Game.GameInstance.State == GameState.Lost)
                Reset();
            else
                Level++;
        }
        private static void Reset()
        {
            Value = 0;
            Level = 0;
        }
        public static new string ToString() => string.Concat(Value.ToString("000000"), "\n", "Level : ", Level);
    }
}
