using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;

namespace SpaceInvaders
{
    /// <summary>
    /// This class represents the entire game, it implements the singleton pattern
    /// </summary>
    internal class Game
    {
        #region Static Fields

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game GameInstance { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        public static readonly System.Drawing.Brush BlackBrush = new SolidBrush(System.Drawing.Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        public static readonly Font DefaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);

        #endregion

        #region GameObjects Management

        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> GameObjects { get; private set; }

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> _pendingNewGameObjects;

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            _pendingNewGameObjects.Add(gameObject);
        }

        #endregion

        #region Game Technical Elements

        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size GameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public readonly HashSet<Keys> KeyPressed = new HashSet<Keys>();

        /// <summary>
        /// GameState enum
        /// </summary>
        private enum GameState
        {
            Play,
            Pause,
            Win,
            Lost
        }

        /// <summary>
        /// Current state of the game
        /// </summary>
        private GameState _state;

        /// <summary>
        /// Theme sound of the game
        /// </summary>
        private MediaPlayer _themePlayer;

        #endregion

        #region Game Physical Elements

        /// <summary>
        /// Player spaceship
        /// </summary>
        public PlayerSpaceShip PlayerShip { get; private set; }

        /// <summary>
        /// Block of enemies moving on the screen
        /// </summary>
        private EnemyBlock _enemyBlock;

        #endregion

        #region Constructors

        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns>instance of the game</returns>
        public static Game CreateGame(Size gameSize)
        {
            return GameInstance ?? (GameInstance = new Game(gameSize));
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.GameSize = gameSize;
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
            if (_state == GameState.Pause)
                g.DrawString("PAUSED", DefaultFont, BlackBrush, 0, 0);
            foreach (var gameObject in GameObjects)
                gameObject.Draw(this, g);
        }

        /// <summary>
        /// Update game
        /// </summary>
        /// <param name="deltaT">Elapsed time since last frame</param>
        public void Update(double deltaT)
        {
            HandleGameState(out var update);
            if (!update) return;
            // add new game objects
            GameObjects.UnionWith(_pendingNewGameObjects);
            _pendingNewGameObjects.Clear();

            // update each game object
            foreach (var gameObject in GameObjects)
                gameObject.Update(this, deltaT);

            // remove dead objects
            GameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitly retype it or the system auto fires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            KeyPressed.Remove(key);
        }

        /// <summary>
        /// Spawn the player spaceship at the middle of the screen
        /// </summary>
        private void SpawnPlayer()
        {
            var position = new Vector2(GameSize.Width * .5f, GameSize.Height * .9f);
            PlayerShip = new PlayerSpaceShip(100, position, 3);
            AddNewGameObject(PlayerShip);
        }

        /// <summary>
        /// Spawn 3 bunkers
        /// </summary>
        private void SpawnBunkers()
        {
            var split = GameSize.Width / 3;
            var bunkerWidth = Properties.Resources.bunker.Width;
            for (var i = 0; i < 3; i++)
            {
                var position = new Vector2(split * (i + .5f) - bunkerWidth * .5f, GameSize.Height * .75);
                GameObject bunker = new Bunker(position);
                AddNewGameObject(bunker);
            }
        }

        /// <summary>
        /// Spawn the enemy block on the top left corner
        /// </summary>
        private void SpawnEnemyBlock()
        {
            _enemyBlock = new EnemyBlock(new Vector2());
            AddNewGameObject(_enemyBlock);
        }

        /// <summary>
        /// Start a new game
        /// </summary>
        private void NewGame()
        {
            GameObjects = new HashSet<GameObject>();
            _pendingNewGameObjects = new HashSet<GameObject>();

            SpawnPlayer();
            SpawnBunkers();
            SpawnEnemyBlock();

            if (_themePlayer != null)
                _themePlayer.Stop();
            _themePlayer = Sound.Theme;
            _themePlayer.Play();

            _state = GameState.Play;
        }

        /// <summary>
        /// Switch between play pause when P is pressed
        /// </summary>
        /// /// <param name="nextState">next game state when P will be pressed</param>
        private void HandlePlayPause(GameState nextState)
        {
            if (!KeyPressed.Contains(Keys.P)) return;
            ReleaseKey(Keys.P);
            _state = nextState;
            if (nextState == GameState.Pause)
                _themePlayer.Pause();
            else
                _themePlayer.Play();
        }

        /// <summary>
        /// Switch between win and lose depending on the situation
        /// </summary>
        private void HandleWinLoss()
        {
            if (!_enemyBlock.IsAlive())
                _state = GameState.Win;
            else if (!PlayerShip.IsAlive())
                _state = GameState.Lost;
        }

        /// <summary>
        /// Switch game state and handle actions depending on it
        /// </summary>
        private void HandleGameState(out bool update)
        {
            update = false;
            switch (_state)
            {
                case GameState.Play:
                    update = true;
                    HandlePlayPause(GameState.Pause);
                    HandleWinLoss();
                    break;
                case GameState.Pause:
                    HandlePlayPause(GameState.Play);
                    break;
                case GameState.Win:
                case GameState.Lost:
                    if (KeyPressed.Contains(Keys.Space))
                    {
                        ReleaseKey(Keys.Space);
                        NewGame();
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}