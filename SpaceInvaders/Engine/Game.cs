using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    class Game
    {
        #region Static Fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        public static System.Drawing.Brush blackBrush = new SolidBrush(System.Drawing.Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        public static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        #endregion

        #region GameObjects Management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }
        #endregion

        #region Game Technical Elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        /// <summary>
        /// GameState enum
        /// </summary>
        private enum GameState
        {
            Play, Pause, Win, Lost
        }

        /// <summary>
        /// Current state of the game
        /// </summary>
        private GameState state;

        /// <summary>
        /// Theme sound of the game
        /// </summary>
        private MediaPlayer theme;
        #endregion

        #region Game Physical Elements
        /// <summary>
        /// Player spaceship
        /// </summary>
        public PlayerSpaceShip PlayerShip { get; private set; }

        private EnemyBlock enemies;
        #endregion

        #region Constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
            theme = Sound.theme;
            NewGame();
        }
        #endregion

        #region Live Game Methods
        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            if (state == GameState.Pause)
                g.DrawString("PAUSED", defaultFont, blackBrush, 0, 0);
            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(this, g);
        }

        /// <summary>
        /// Update game
        /// </summary>
        /// /// <param name="deltaT">Elapsed time since last frame</param>
        public void Update(double deltaT)
        {
            switch (state)
            {
                case GameState.Play:
                    if (keyPressed.Contains(Keys.P))
                    {
                        ReleaseKey(Keys.P);
                        state = GameState.Pause;
                    }
                    if (enemies.Count == 0)
                        state = GameState.Win;
                    if (!PlayerShip.IsAlive())
                        state = GameState.Lost;

                    break;
                case GameState.Pause:
                    if (keyPressed.Contains(Keys.P))
                    {
                        ReleaseKey(Keys.P);
                        state = GameState.Play;
                    }
                    return;
                case GameState.Win:
                    if (keyPressed.Contains(Keys.Space))
                    {
                        ReleaseKey(Keys.Space);
                        NewGame();
                    }
                    return;
                case GameState.Lost:
                    if (keyPressed.Contains(Keys.Space))
                    {
                        ReleaseKey(Keys.Space);
                        NewGame();
                    }
                    return;
            }

            // add new game objects
            gameObjects.UnionWith(pendingNewGameObjects);
            pendingNewGameObjects.Clear();

            // update each game object
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(this, deltaT);
            }

            // remove dead objects
            gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
        }
        #endregion

        #region Methods
        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }
        
        /// <summary>
        /// Start a new game
        /// </summary>
        private void NewGame()
        {
            gameObjects = new HashSet<GameObject>();

            var position = new Vector2(gameSize.Width * .5f, gameSize.Height * .9f);
            PlayerShip = new PlayerSpaceShip(100, position, 3);
            AddNewGameObject(PlayerShip);

            var split = gameSize.Width / 3;
            var bunkerWidth = Properties.Resources.bunker.Width;
            for (int i = 0; i < 3; i++)
            {
                position = new Vector2(split * (i + .5f) - bunkerWidth * .5f, gameSize.Height * .75);
                GameObject bunker = new Bunker(position);
                AddNewGameObject(bunker);
            }

            position = new Vector2(0, 0);
            enemies = new EnemyBlock(position);
            AddNewGameObject(enemies);
            
            theme.Stop();
            theme.Play();

            state = GameState.Play;
        }
        #endregion

    }
}
