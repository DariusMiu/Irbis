using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Collections.Generic;

namespace Irbis
{
    public enum Side
    {
        top = 0,
        right = 1,
        bottom = 2,
        left = 3,
    }
    public enum Direction
    {
        forward = 0,
        left = 1,
        right = 2,
    }
    public enum Location
    {
        ground = 0,
        air = 1,
        water = 2,
    }
    public enum Activity
    {
        idle = 0,
        running = 1,
        jumping = 2,
        rolling = 3,
        falling = 4,
        landing = 5,
        attacking = 6,
    }
    public enum Attacking
    {
        no = 0,
        attack1 = 1,
        attack2 = 2,

    }
    public enum AI
    {
        wander = 0,
        patrol = 1,
        seek = 2,
        combat = 3,
        stunned = 4,
        persue = 5,
    }
    public enum EnchantType
    {
        bleed = 0,          //long duration, low damage                 stacks:                             upgrade: longer duration, more damage/second
        fire = 1,           //short duration, high damage               stacks:                             upgrade: longer duration, more damage/second
        frost = 2,          //long duration, slows                      stacks:                             upgrade: longer duration
        knockback = 3,      //knocks enemies back short distance        stacks:                             upgrade: knocks back further
        poison = 4,         //long duration, high damage, limited use   stacks:                             upgrade: more uses
        sharpness = 5,      //flat damage increase (no duration)        stacks:                             upgrade: more damage
        stun = 6,           //short duration, full stun                 stacks:                             upgrade: longer duration
    }

    public interface ICollisionObject
    {
        Rectangle Collider
        {
            get;
            set;
        }
    }

    public interface IEnemy
    {
        Rectangle Collider
        {
            get;
            set;
        }
        float MaxHealth
        {
            get;
            set;
        }
        float Health
        {
            get;
            set;
        }
        float SpeedModifier
        {
            get;
            set;
        }
        Vector2 Pos
        {
            get;
            set;
        }
        List<Enchant> ActiveEffects
        {
            get;
        }
        void AddEffect(Enchant effect);
        void UpgradeEffect(int index, float duration);
        void Knockback(Player player, float strength);
        void Hurt(float damage);
        void Stun(float duration);
    }

    public interface IBoss
    {
        Rectangle Collider
        {
            get;
            set;
        }
        bool HurtLeft
        {
            get;
        }
        bool HurtRight
        {
            get;
        }
        bool HurtTop
        {
            get;
        }
        bool HurtBottom
        {
            get;
        }
        float MaxHealth
        {
            get;
            set;
        }
        float Health
        {
            get;
            set;
        }
        Vector2 Pos
        {
            get;
            set;
        }

    }

    //public interface ISwordEnchant
    //{
    //    Enchant enchant
    //    {
    //        get;

    //    }
    //}

    public class Irbis : Game
    {
        /// version number key (two types): 
        /// release number . software stage (pre/alpha/beta) . build/version . build iteration
        /// release number . content patch number . software stage . build iteration
        static string versionNo = "0.1.1.6";
        static string versionID = "alpha";
        static string versionTy = "debug";
        /// Different version types: 
        /// debug
        /// release candidate
        /// release
        
        public static bool debug;
        
        public static float DeltaTime
        {
            get
            {
                return deltaTime;
            }
        }
        private static float deltaTime;

        public static double Timer
        {
            get
            {
                return timer;
            }
        }
        private static double timer;

        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;

        static string autosave;

        public static KeyboardState keyboardState;
        public static KeyboardState previousKeyboardState;

        public static MouseState mouseState;
        public static MouseState previousMouseState;

        public Vector2 initialPos;
        public Vector2 bossSpawn;
        public List<Vector2> enemySpawnPoints;

        static List<Square> backgroundSquareList;
        public static List<ICollisionObject> collisionObjects;
        public static List<Square> sList;
        public static List<Square> squareList;
        public static List<Button> buttonList;
        public static List<Enemy> eList;
        public static List<IEnemy> enemyList;
        public static List<Print> printList;
        public static List<UIElementSlider> sliderList;
        public static string[] levelList;

        public static bool isMenuScrollable;
        public static int maxButtonsOnScreen;
        public static int levelListCounter;
        public static string currentLevel;
        Menu menu;

        public static SaveFile savefile;

        public static int scene;
        public static bool sceneIsMenu;
        public static bool levelEditor;
        int selectedBlock;

        static int consoleLine;
        float consoleLineChangeTimer;
        float consoleMoveTimer;
        Rectangle consoleRect;
        bool console;

        Texture2D fontTex;
        //Texture2D fontTex2;
        public static Font font;

        public static bool cameraLerp;
        public static float cameraLerpSpeed;
        public static bool cameraShakeSetting;
        float cameraShakeDuration;
        float cameraShakeMagnitude;
        //float cameraTimePerShake;

        float cameraSwingDuration;
        float cameraSwingMaxDuration;
        float cameraSwingMagnitude;
        public float swingDuration;
        public float swingMagnitude;
        Vector2 cameraSwingHeading;
        public static bool cameraSwingSetting;
        bool cameraSwing;

        SmartFramerate smartFPS;

        public string timerAccuracy;

        public float minSqrDetectDistance;
        bool displayEnemyHealth;

        bool framebyframe;
        bool nextframe;

        public static int menuSelection;

        public static float gravity;
        public Random RAND;

        Print debuginfo;
        Print consoleWriteline;
        private static Print developerConsole;
        public Print timerDisplay;
        Print onslaughtDisplay;

        public static Texture2D nullTex;
        Texture2D enemy0Tex;
        Texture2D[] menuTex;

        public static Player geralt;

        public UIElementSlider healthBar;
        public UIElementSlider shieldBar;
        public UIElementSlider energyBar;
        public UIElementDiscreteSlider potionBar;
        public UIElementSlider enemyHealthBar;

        public static bool onslaughtMode;
        public static OnslaughtSpawner onslaughtSpawner;
        public int enemiesKilled;


        Vector2 camera;
        Vector2 mainCamera;
        Vector2 screenSpacePlayerPos;

        Matrix background;
        Matrix foreground;
        Matrix UIground;

        public static Rectangle boundingBox;
        public static Rectangle screenspace;
        public static Rectangle zeroScreenspace;

        RectangleBorder startingScreenLocation;
        public static RectangleBorder boundingBoxBorder;

        EventHandler<TextInputEventArgs> onTextEntered;
        public static bool acceptTextInput;
        public static string textInputBuffer;

        public static bool listenForNewKeybind;
        public static bool resetRequired;
        public static int levelLoaded;

        public bool AIenabled;

        public static int screenScale;
        public static Point resolution;
        public static Point halfResolution;
        public static Point tempResolution;

        public static float masterAudioLevel;
        public static float musicLevel;
        public static float soundEffectsLevel;

        public float randomTimer;
        public static int sliderPressed;

        public static Keys attackKey;
        public static Keys altAttackKey;

        public static Keys shockwaveKey;
        public static Keys altShockwaveKey;

        public static Keys shieldKey;
        public static Keys altShieldKey;

        public static Keys jumpKey;
        public static Keys altJumpKey;

        public static Keys upKey;
        public static Keys altUpKey;

        public static Keys downKey;
        public static Keys altDownKey;

        public static Keys leftKey;
        public static Keys altLeftKey;

        public static Keys rightKey;
        public static Keys altRightKey;

        public static Keys potionKey;
        public static Keys altPotionKey;

        public static Keys rollKey;
        public static Keys altRollKey;

        public Irbis()
        {
            //Never remove these console debug lines
            Console.WriteLine("    Project: Irbis (" + versionTy + ")");
            Console.WriteLine("    " + versionID + " v" + versionNo);

            this.IsMouseVisible = false;

            sceneIsMenu = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;



            //GraphicsDevice.
            //graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 800;
            //graphics.ApplyChanges();


            resetRequired = false;
            IsFixedTimeStep = false;


            Content.RootDirectory = ".\\content";
            gravity = 2250f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related Content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            sList = new List<Square>();
            collisionObjects = new List<ICollisionObject>();
            squareList = new List<Square>();
            backgroundSquareList = new List<Square>();
            buttonList = new List<Button>();
            eList = new List<Enemy>();
            enemyList = new List<IEnemy>();
            printList = new List<Print>();
            sliderList = new List<UIElementSlider>();

            AIenabled = true;
            framebyframe = false;
            nextframe = true;
            //pressed = false;
            randomTimer = 0f;
            base.Initialize();

            levelLoaded = 0;

            RAND = new Random();
            displayEnemyHealth = false;

            Window.TextInput += TextEntered;
            onTextEntered += HandleInput;
            acceptTextInput = false;
            textInputBuffer = "";
            enemiesKilled = 0;

            //EventInput.EventInput.Initialize(this.Window);
            camera = screenSpacePlayerPos = mainCamera = Vector2.Zero;
            foreground = Matrix.Identity;
            background = Matrix.Identity;
            UIground = Matrix.Identity;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your Content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D fontTex2 = Content.Load<Texture2D>("font2");
            Font font2 = new Font(fontTex2, 8);
            developerConsole = new Print(font2);
            PrintVersion();
            WriteLine();

            autosave = ".\\content\\autosave.snep";

            PlayerSettings playerSettings;
            if (File.Exists(@".\content\playerSettings.ini"))
            {
                playerSettings = Load(@".\content\playerSettings.ini");
            }
            else
            {
                Console.WriteLine("creating new playerSettings.ini...");
                WriteLine("creating new playerSettings.ini...");
                playerSettings = new PlayerSettings(true);
                PlayerSettings.Save(playerSettings, @".\content\playerSettings.ini");
                playerSettings = Load(@".\content\playerSettings.ini");
            }

            savefile = new SaveFile();
            if (File.Exists(autosave))
            {
                savefile.Load(autosave);
            }
            else
            {
                Console.WriteLine("creating new autosave.snep...");
                WriteLine("creating new autosave.snep...");
                savefile = new SaveFile();
                savefile.Save(autosave);
            }

            Texture2D playerTex = Content.Load<Texture2D>("player");
            Texture2D shieldTex = Content.Load<Texture2D>("shield");

            geralt = new Player(playerTex, shieldTex, playerSettings, this);
            geralt.Respawn(new Vector2(-1000f, -1000f));

            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuTex = new Texture2D[8];
            menuTex[0] = Content.Load<Texture2D>("Main Menu title");
            menuTex[1] = Content.Load<Texture2D>("Options title");
            menuTex[2] = Content.Load<Texture2D>("Keybinds title");
            menuTex[3] = Content.Load<Texture2D>("Camera title");
            menuTex[4] = Content.Load<Texture2D>("Video title");
            menuTex[5] = Content.Load<Texture2D>("Audio title");
            menuTex[6] = Content.Load<Texture2D>("Misc title");
            menuTex[7] = Content.Load<Texture2D>("Level Select title");

            nullTex = Content.Load<Texture2D>("nullTex");

            enemySpawnPoints = new List<Vector2>();

            fontTex = Content.Load<Texture2D>("font");

            font = new Font(fontTex, playerSettings.characterHeight, playerSettings.characterWidth, false);

            menu = new Menu();

            debuginfo = new Print(resolution.X, font2, Color.White, true, new Point(1, 3), Direction.left, 0.8f);
            consoleWriteline = new Print(resolution.X, font2, Color.White, true, new Point(1, 5), Direction.left, 1f);
            developerConsole.Update(resolution.X);
            developerConsole.scrollDown = false;
            consoleMoveTimer = 0f;
            console = false;

            printList.Add(debuginfo);
            printList.Add(consoleWriteline);
            printList.Add(developerConsole);

            //smoothFPS = new SmoothFramerate(100);
            smartFPS = new SmartFramerate(5);

            //scenes 0-10 are reserved for menus

            scene = 11;      //master scene ID
                             //LOAD THIS FROM SAVE FILE FOR BACKGROUND

            switch (scene)
            {
                case 0:     //main menu
                    LoadMenu(0, 0, false);
                    sceneIsMenu = true;
                    break;
                case 1:     //options menu
                    LoadMenu(1, 0, false);
                    sceneIsMenu = true;
                    break;
                case 2:     //options - controls
                    LoadMenu(2, 0, false);
                    sceneIsMenu = true;
                    break;
                case 3:     //options - camera
                    LoadMenu(3, 0, false);
                    sceneIsMenu = true;
                    break;
                case 4:     //options - video
                    LoadMenu(4, 0, false);
                    sceneIsMenu = true;
                    break;
                case 5:     //options - audio
                    LoadMenu(5, 0, false);
                    sceneIsMenu = true;
                    break;
                case 6:     //options - misc
                    LoadMenu(6, 0, false);
                    sceneIsMenu = true;
                    break;
                case 7:     //new game level select
                    LoadMenu(7, 0, false);
                    sceneIsMenu = true;
                    break;
                case 11:     //load level
                    LoadLevel(savefile.lastPlayedLevel, false);
                    sceneIsMenu = false;
                    break;
                default:    //error
                    Console.WriteLine("Error. Scene ID " + scene + " does not exist.");
                    break;
            }

            LoadMenu(0, 0, false);
            sceneIsMenu = true;

            //WriteLine("Type 'help' for a few commands");
        }

        public void LoadMenu(int Scene, int startMenuLocation, bool loadSettings)
        {
            //geralt.inputEnabled = false;
            this.IsMouseVisible = true;
            scene = Scene;
            sceneIsMenu = true;
            sList.Clear();
            printList.Clear();
            buttonList.Clear();
            if (loadSettings)
            {
                Load(@".\content\playerSettings.ini");
            }
            sList.Add(new Square(menuTex[scene], Color.White, Point.Zero, menuTex[scene].Width, menuTex[scene].Height, false, true, true, 0.5f));
            if (resetRequired)
            {
                Print resettt = new Print(resolution.X - 132, font, Color.White, false, new Point(resolution.X - 32, resolution.Y - 26), Direction.right, 0.5f);
                resettt.Update("Restart the game to apply resolution changes!");
                printList.Add(resettt);
            }

            //this is where all the crazy stuff happens
            menu.Create(scene);

            menuSelection = startMenuLocation;
        }

        public void LoadLevel(string filename, bool loadUI)
        {
            this.IsMouseVisible = false;
            scene = 11;
            sceneIsMenu = false;
            currentLevel = filename;

            ClearLevel();

            Level thisLevel = new Level();
            thisLevel.Load(".\\levels\\" + filename + ".lvl");

            List<Point> squareSpawns = thisLevel.SquareSpawnPoints;
            List<string> squareTextures = thisLevel.squareTextures;

            List<Point> BackgroundSquares = thisLevel.BackgroundSquares;
            List<string> backgroundTextures = thisLevel.backgroundTextures;
            List<float> backgroundSquareDepths = thisLevel.backgroundSquareDepths;

            onslaughtMode = thisLevel.isOnslaught;
            initialPos = thisLevel.PlayerSpawn;
            enemySpawnPoints = thisLevel.EnemySpawnPoints;
            bossSpawn = thisLevel.BossSpawn;

            enemy0Tex = Content.Load<Texture2D>("enemy0");
            Texture2D shieldBarTex = Content.Load<Texture2D>("shieldBar");
            Texture2D centerTex = Content.Load<Texture2D>("centerTexture");

            if (loadUI)
            {
                healthBar = new UIElementSlider(Direction.left, new Point(32, 32), 250, 20, geralt.maxHealth, Color.Red, new Color(166, 030, 030), nullTex, nullTex, shieldBarTex, font, false, 0.505f, 0.55f, 0.5f);
                shieldBar = new UIElementSlider(Direction.left, new Point(32, 51), 150, 20, geralt.maxShield, Color.Red, new Color(255, 170, 000), nullTex, nullTex, shieldBarTex, font, false, 0.501f);
                energyBar = new UIElementSlider(Direction.left, new Point(32, 70), 100, 20, geralt.maxEnergy, Color.Red, new Color(000, 234, 255), nullTex, nullTex, shieldBarTex, font, false, 0.5f);
                potionBar = new UIElementDiscreteSlider(Direction.left, new Rectangle(184, 54, 96, 15), nullTex, nullTex, nullTex, Color.DarkSlateGray, Color.DarkRed, Color.DarkSlateBlue, geralt.maxNumberOfPotions, 3, 0.5f);
                enemyHealthBar = new UIElementSlider(Direction.right, new Point(resolution.X - 32, 32), 500, 20, 100, Color.Red, new Color(166, 030, 030), nullTex, nullTex, shieldBarTex, font, false, 0.5f);

                if (onslaughtMode)
                {
                    onslaughtSpawner = new OnslaughtSpawner(1);
                    onslaughtDisplay = new Print(resolution.Y / 2, font, Color.White, true, new Point(2, 7), Direction.left, 0.6f);
                    onslaughtDisplay.Update("Onslaught Wave " + onslaughtSpawner.wave, true);
                    timerDisplay = null;
                }
                else
                {
                    timerDisplay = new Print(resolution.Y / 2, font, Color.White, true, new Point(2, 7), Direction.left, 0.6f);
                    onslaughtDisplay = null;
                    onslaughtSpawner = null;
                }

                geralt.Respawn(initialPos);
            }
            else
            {
                geralt.Respawn(new Vector2(-1000f, -1000f));
            }

            for (int i = 0; i < squareTextures.Count; i++)
            {
                Texture2D squareTex = Content.Load<Texture2D>(squareTextures[i]);
                Square tempSquare = new Square(squareTex, squareSpawns[i], true, thisLevel.squareDepth);
                collisionObjects.Add(tempSquare);
                squareList.Add(tempSquare);
            }

            for (int i = 0; i < backgroundSquareDepths.Count; i++)                                                                                              //backgrounds
            {
                Texture2D squareTex = Content.Load<Texture2D>(backgroundTextures[i]);
                Square tempSquare = new Square(squareTex, Color.White, BackgroundSquares[i], squareTex.Width, squareTex.Height, false, true, true, backgroundSquareDepths[i]);
                backgroundSquareList.Add(tempSquare);
            }

            camera.X = resolution.X / 2;
            camera.Y = resolution.Y / 2;
            levelLoaded = scene;

            savefile.lastPlayedLevel = currentLevel;

            WriteLine("for a list of all squares, type squareList");
        }

        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific Content.
        /// </summary>
        protected override void UnloadContent()                                                                                                                         //unload Content
        {
            // TODO: Unload any non ContentManager Content here
        }



        protected override void Update(GameTime gameTime)                                                                                                               //update
        {
            keyboardState = Keyboard.GetState();
            smartFPS.Update(gameTime.ElapsedGameTime.TotalSeconds);

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (debug)
            {
                PrintDebugInfo();


                foreach (Square s in squareList)
                {
                    if (geralt.collided.Contains(s))
                    {
                        s.color = Color.Cyan;
                    }
                    else
                    {
                        s.color = Color.White;
                    }
                }
            }
            else
            {
                debuginfo.Update("\n\nᴥ" + smartFPS.framerate.ToString("0000"), true);
            }

            if (!sceneIsMenu && !acceptTextInput)
            {
                LevelUpdate(gameTime);
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/
                    (GetKeyDown(Keys.Escape) || (GetKeyDown(Keys.Pause))))
                {
                    framebyframe = false;
                    LoadMenu(0, 0, false);
                }
            }
            else
            {
                mouseState = Mouse.GetState();
                mouseState = new MouseState(mouseState.X / screenScale, mouseState.Y / screenScale, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);

                if (levelEditor)
                {
                    LevelEditor();
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/
                        (GetKeyDown(Keys.Escape) || (GetKeyDown(Keys.Pause))))
                    {
                        LoadMenu(0, 0, false);
                        levelEditor = false;
                    }
                }
                else
                {
                    MenuUpdate();
                }
                previousMouseState = mouseState;
            }

            if (GetKeyDown(Keys.OemTilde))
            {
                if (!sceneIsMenu && false)
                {
                    framebyframe = false;
                    LoadMenu(0, 0, false);
                }
                OpenConsole();
            }

            if (consoleMoveTimer > 0)
            {
                MoveConsole();
            }

            if (acceptTextInput && console)
            {
                UpdateConsole();
            }

            CleanConsole();

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        protected void MenuUpdate()
        {

            if (!console)
            {
                menu.Update(this);
            }
        }

        protected void LevelUpdate(GameTime gameTime)
        {            
            if ((keyboardState.IsKeyDown(Keys.R) && !acceptTextInput) || geralt.health <= 0)                            //RESPAWN
            {
                geralt.Respawn(initialPos);

                foreach (Enemy e in eList)
                {
                    collisionObjects.Remove(e);
                }
                eList.Clear();
                enemyList.Clear();
                enemiesKilled = 0;
                if (debug) { debuginfo.Update("\nReset"); }
            }

            if (keyboardState.IsKeyDown(Keys.N) && !acceptTextInput)
            {
                if (!nextframe)
                {
                    framebyframe = false; //nex
                }
                nextframe = true;
            }
            else
            {
                nextframe = false;
            }
            if (keyboardState.IsKeyDown(Keys.G) && !acceptTextInput)
            {
                framebyframe = false; //nex
            }
            if (!framebyframe) //nex
            {
                geralt.Update();

                Camera();

                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timerDisplay != null) { timerDisplay.Update(TimerText(timer), true); }
                
                //PlayerCollision(geralt, sList);



                for (int i = 0; i < eList.Count; i++)
                {
                    eList[i].Update();
                    if (eList[i].Health <= 0 || eList[i].Pos.Y > 5000)
                    {
                        //collisionObjects.Remove(eList[i]);
                        enemyList.Remove(eList[i]);
                        eList.Remove(eList[i]);
                        enemiesKilled++;
                        i--;
                    }
                }

                healthBar.Update(false, geralt.health);
                if (geralt.shielded)
                {
                    shieldBar.Update(true, geralt.shield);
                }
                else
                {
                    shieldBar.Update(false, geralt.shield);
                }
                energyBar.UpdateValue(geralt.energy);

                if (true)
                {
                    if (enemyList.Count > 0)
                    {
                        IEnemy closest = enemyList[0];

                        float closestSqrDistance = float.MaxValue;
                        float thisEnemysSqrDistance = 0f;
                        foreach (IEnemy e in enemyList)
                        {
                            thisEnemysSqrDistance = DistanceSquared(geralt.collider.Center, e.Collider.Center);
                            if (thisEnemysSqrDistance < closestSqrDistance)
                            {
                                closestSqrDistance = thisEnemysSqrDistance;
                                closest = e;
                            }
                        }
                        if (closestSqrDistance <= minSqrDetectDistance)
                        {
                            displayEnemyHealth = geralt.combat = true;
                            geralt.Combat();
                            enemyHealthBar.maxValue = closest.MaxHealth;
                            enemyHealthBar.UpdateValue(closest.Health);
                        }
                        else
                        {
                            displayEnemyHealth = geralt.combat = false;
                        }
                    }
                    else
                    {
                        displayEnemyHealth = geralt.combat = false;
                    }
                }

                if (nextframe) { framebyframe = true; } //nex
            }

            if (onslaughtMode)
            {
                if (onslaughtSpawner.enemiesLeftThisWave > 0 && eList.Count < onslaughtSpawner.maxEnemies && onslaughtSpawner.enemySpawnTimer())
                {
                    SummonGenericEnemy(onslaughtSpawner.enemyHealth, onslaughtSpawner.enemyDamage, onslaughtSpawner.enemySpeed);
                }
                if (eList.Count <= 0 && enemiesKilled >= onslaughtSpawner.enemiesThisWave)
                {
                    enemiesKilled = 0;
                    onslaughtSpawner.NextWave();
                }
                if (!onslaughtSpawner.waveStarted)
                {
                    onslaughtDisplay.Update("Wave " + onslaughtSpawner.wave + " Start: " + onslaughtSpawner.timeUntilNextSpawn.ToString("00"), true);
                }
                else
                {
                    onslaughtDisplay.Update("Onslaught Wave " + onslaughtSpawner.wave, true);
                }
            }

            if (keyboardState.IsKeyDown(Keys.K) && !previousKeyboardState.IsKeyDown(Keys.K) && !acceptTextInput)
            {
                randomTimer = 0f;
                SummonGenericEnemy();
            }

            if (keyboardState.IsKeyDown(Keys.K) && !acceptTextInput)
            {
                randomTimer += DeltaTime;
                if (randomTimer > 0.5f)
                {
                    SummonGenericEnemy();
                }
            }
        }



        private void TextEntered(object sender, TextInputEventArgs e)
        {
            if (onTextEntered != null && acceptTextInput)
            {
                onTextEntered.Invoke(sender, e);
            }
        }

        private void HandleInput(object sender, TextInputEventArgs e)
        {
            
            switch (e.Character)
            {
                case '\u0008':
                    if (textInputBuffer.Length > 0)
                    {
                        textInputBuffer = textInputBuffer.Remove(textInputBuffer.Length - 1);
                    }
                    break;
                case '\u0009':
                    textInputBuffer += "    ";
                    break;
                case '\u000D':
                    //do nothing
                    break;
                default:
                    textInputBuffer += e.Character;
                    break;
            }
        }

        public void Camera()
        {
            screenSpacePlayerPos.X = geralt.collider.Center.X + (int)foreground.M41;
            screenSpacePlayerPos.Y = geralt.collider.Center.Y + (int)foreground.M42;

            //mainCamera is used for returning the camera to where it "should" be
            //camera is what is displayed on-screen
            if (cameraLerp)
            {
                if (boundingBox.Right <= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X = (Lerp(camera.X + boundingBox.Right, camera.X + (screenSpacePlayerPos.X), cameraLerpSpeed * DeltaTime) - boundingBox.Right);
                }
                else if (boundingBox.Left >= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X = (Lerp(camera.X + boundingBox.Left, camera.X + (screenSpacePlayerPos.X), cameraLerpSpeed * DeltaTime) - boundingBox.Left);
                }
                if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y = (Lerp(camera.Y + boundingBox.Bottom, camera.Y + (screenSpacePlayerPos.Y), cameraLerpSpeed * DeltaTime) - boundingBox.Bottom);
                }
                else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y = (Lerp(camera.Y + boundingBox.Top, camera.Y + (screenSpacePlayerPos.Y), cameraLerpSpeed * DeltaTime) - boundingBox.Top);
                }
            }
            else
            {
                if (boundingBox.Right <= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X += screenSpacePlayerPos.X - boundingBox.Right;
                }
                else if (boundingBox.Left >= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X += screenSpacePlayerPos.X - boundingBox.Left;
                }
                if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y += screenSpacePlayerPos.Y - boundingBox.Bottom;
                }
                else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y += screenSpacePlayerPos.Y - boundingBox.Top;
                }
            }
            screenspace.X = (int)(camera.X - halfResolution.X);
            screenspace.Y = (int)(camera.Y - halfResolution.Y);

            background.M31 = foreground.M41 = halfResolution.X - camera.X;
            background.M32 = foreground.M42 = halfResolution.Y - camera.Y;

            if (cameraShakeSetting && cameraShakeDuration > 0) { CameraShake(); }
            if (cameraSwingSetting && cameraSwingDuration > 0) { CameraSwing(); }
        }

        public void CameraSwing()
        {
            cameraSwingDuration -= DeltaTime;
            if (cameraSwingDuration <= 0 && cameraSwing)
            {
                cameraSwing = false;
                cameraSwingDuration = cameraSwingMaxDuration * 2;
            }

            if (cameraSwing)
            {
                //background.M31 = foreground.M41 = Lerp(background.M31, background.M31 + (cameraSwingHeading.X * cameraSwingMagnitude), cameraLerpSpeed * DeltaTime);
                camera.X = (Lerp(camera.X, camera.X + (cameraSwingHeading.X * cameraSwingMagnitude), 15f * DeltaTime));
                camera.Y = (Lerp(camera.Y, camera.Y + (cameraSwingHeading.Y * cameraSwingMagnitude), 15f * DeltaTime));
            }
            else
            {
                camera.X = (Lerp(camera.X, mainCamera.X, cameraLerpSpeed * DeltaTime));
                camera.Y = (Lerp(camera.Y, mainCamera.Y, cameraLerpSpeed * DeltaTime));
            }
        }

        public void CameraSwing(float duration, float magnitude, Vector2 heading)
        {
            cameraSwingDuration = cameraSwingMaxDuration = duration;
            cameraSwingHeading = heading;
            cameraSwingHeading.Normalize();
            cameraSwingMagnitude = magnitude;
            cameraSwing = true;
        }

        public void CameraShake()
        {
            cameraShakeDuration -= DeltaTime;
            //cameraTimePerShake += DeltaTime;

            background.M31 = foreground.M41 = background.M31 + ((((float)RAND.NextDouble() * 2) - 1) * cameraShakeMagnitude);       //X
            background.M32 = foreground.M42 = background.M32 + ((((float)RAND.NextDouble() * 2) - 1) * cameraShakeMagnitude);       //Y
        }

        public void CameraShake(float duration, float magnitude)
        {
            //cameraTimePerShake = 0f;
            if (duration > cameraShakeDuration)
            {
                cameraShakeDuration = duration;
            }
            cameraShakeMagnitude = magnitude;
        }

        public void LevelEditor()
        {
            debug = this.IsMouseVisible = true;
            PrintDebugInfo();
            if (!acceptTextInput)
            {
                if (keyboardState.IsKeyDown(upKey))
                {
                    camera.Y -= 1;
                }
                if (keyboardState.IsKeyDown(downKey))
                {
                    camera.Y += 1;
                }
                if (keyboardState.IsKeyDown(leftKey))
                {
                    camera.X -= 1;
                }
                if (keyboardState.IsKeyDown(rightKey))
                {
                    camera.X += 1;
                }
            }
            if (GetKeyDown(Keys.Enter))
            {
                acceptTextInput = !acceptTextInput;
                consoleWriteline.Update(string.Empty, true);
                ConsoleParser(textInputBuffer);
                textInputBuffer = string.Empty;
            }

            background.M31 = foreground.M41 = halfResolution.X - camera.X;
            background.M32 = foreground.M42 = halfResolution.Y - camera.Y;

            Point worldSpaceMouseLocation = new Point((int)(mouseState.Position.X + camera.X - (halfResolution.X)), (int)(mouseState.Position.Y + camera.Y - (halfResolution.Y)));
            if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
            {
                int destroyBlock = -1;

                for (int i = 0; i < squareList.Count; i++)
                {
                    if (squareList[i].Collider.Contains(worldSpaceMouseLocation))
                    {
                        destroyBlock = i;
                    }
                }
                if (destroyBlock >= 0)
                {

                    WriteLine("destroying block " + destroyBlock + " at "+ worldSpaceMouseLocation);
                    squareList.RemoveAt(destroyBlock);
                }
                else
                {
                    WriteLine("spawning block with defaultTex texture at " + worldSpaceMouseLocation);
                    Texture2D defaultSquareTex = Content.Load<Texture2D>("defaultTex");
                    Square tempSquare = new Square(defaultSquareTex, worldSpaceMouseLocation, true, 0.3f);
                    squareList.Add(tempSquare);
                }
            }
            
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
            {
                selectedBlock = -1;
                for (int i = 0; i < squareList.Count; i++)
                {
                    if (squareList[i].Collider.Contains(worldSpaceMouseLocation))
                    {
                        selectedBlock = i;
                    }
                }
                if (selectedBlock >= 0)
                {
                    WriteLine("moving block " + selectedBlock);
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed && selectedBlock >= 0)
            {
                squareList[selectedBlock].Position = worldSpaceMouseLocation;
            }



        }

        public void SaveLevel(string levelname)
        {
            Level thisLevel = new Level();
            //thisLevel.squareList = squareList;

            List<Point> squareSpawns = new List<Point>();
            List<string> squareTextures = new List<string>();
            for (int i = 0; i < squareList.Count; i++)
            {
                squareSpawns.Add(squareList[i].Position);
                squareTextures.Add(squareList[i].tex.Name);
            }

            List<Point> BackgroundSquares = new List<Point>();
            List<string> backgroundTextures = new List<string>();
            List<float> backgroundSquareDepths = new List<float>();
            for (int i = 0; i < backgroundSquareList.Count; i++)
            {
                BackgroundSquares.Add(backgroundSquareList[i].Position);
                backgroundTextures.Add(backgroundSquareList[i].tex.Name);
                backgroundSquareDepths.Add(backgroundSquareList[i].depth);
            }

            thisLevel.SquareSpawnPoints = squareSpawns;
            thisLevel.squareTextures = squareTextures;
            thisLevel.BackgroundSquares = BackgroundSquares;
            thisLevel.backgroundTextures = backgroundTextures;
            thisLevel.backgroundSquareDepths = backgroundSquareDepths;

            thisLevel.isOnslaught = onslaughtMode;
            thisLevel.PlayerSpawn = initialPos;
            thisLevel.BossSpawn = bossSpawn;
            thisLevel.EnemySpawnPoints = enemySpawnPoints;


            thisLevel.Save(".\\levels\\" + levelname + ".lvl");
            WriteLine(".\\levels\\" + levelname + ".lvl saved");
            Console.WriteLine(".\\levels\\" + levelname + ".lvl saved");
        }

        public void ClearLevel()
        {
            printList.Clear();
            printList.Add(debuginfo);
            printList.Add(consoleWriteline);
            printList.Add(developerConsole);
            sList.Clear();
            buttonList.Clear();
            eList.Clear();
            enemyList.Clear();
            squareList.Clear();
            collisionObjects.Clear();
            backgroundSquareList.Clear();

            if (levelLoaded == 0) //THIS MEANS WE ARE LOADING A LEVEL FOR THE TITLESCREEN
            {
                levelLoaded = -1;
            }
            else
            {
                levelLoaded = scene;
                savefile.lastPlayedLevel = currentLevel;
                savefile.Save(autosave);
            }

            timer = 0;
            enemiesKilled = 0;

            screenspace = new Rectangle(Point.Zero, resolution);
            startingScreenLocation = new RectangleBorder(new Rectangle(Point.Zero, resolution), Color.Magenta, 0f);
            boundingBoxBorder = new RectangleBorder(boundingBox, Color.Magenta, 0.8f);

        }

        public void PrintDebugInfo()
        {
            debuginfo.Update("      DEBUG MODE. " + versionID.ToUpper() + " v" + versionNo, true);
            debuginfo.Update("\n       FPS:" + (1 / DeltaTime).ToString("0000.0"));
            debuginfo.Update("\n  smartFPS:" + smartFPS.framerate.ToString("0000.0"));
            //debuginfo.Update("\n smoothFPS:" + smoothFPS.framerate.ToString("0000.0"));
            if (geralt != null)
            {
                debuginfo.Update("\n     input:" + geralt.input + "  isRunning:" + geralt.isRunning);
                debuginfo.Update("\n prevInput:" + geralt.prevInput);
                debuginfo.Update("\n\nwalledInputChange:" + geralt.walledInputChange);
                debuginfo.Update("\n\n  player info");
                debuginfo.Update("\nHealth:" + geralt.health + "\nShield:" + geralt.shield + "\nEnergy:" + geralt.energy);
                debuginfo.Update("\n  Xpos:" + geralt.pos.X + "\n  Ypos:" + geralt.pos.Y);
                debuginfo.Update("\n  Xvel:" + geralt.velocity.X + "\n  Yvel:" + geralt.velocity.Y);
                debuginfo.Update("\ninvulner:" + geralt.invulnerableTime);
                debuginfo.Update("\nShielded:" + geralt.shielded);
                debuginfo.Update("\ncolliders:" + collisionObjects.Count);
                debuginfo.Update("\n collided:" + geralt.collided.Count);
                debuginfo.Update("\n   walled:" + geralt.Walled);
                debuginfo.Update("\nactivity:" + geralt.activity);
                debuginfo.Update("\nattackin:" + geralt.attacking);
                debuginfo.Update("\nattackID:" + geralt.attackID);
            }
            debuginfo.Update("\ncurrentLevel:" + currentLevel);
            debuginfo.Update("\n onslaught:" + onslaughtMode);
            if (onslaughtMode && onslaughtSpawner != null)
            {
                debuginfo.Update("\n      wave:" + onslaughtSpawner.wave);
                debuginfo.Update("\nspawntimer:" + onslaughtSpawner.timeUntilNextSpawn);
                debuginfo.Update("\n   enemies:" + onslaughtSpawner.enemiesLeftThisWave);
                debuginfo.Update("\nmaxenemies:" + onslaughtSpawner.maxEnemies);
                debuginfo.Update("\n   ehealth:" + onslaughtSpawner.enemyHealth);
                debuginfo.Update("\n   edamage:" + onslaughtSpawner.enemyDamage);
            }
            debuginfo.Update("\nFrame-by-frame mode:" + framebyframe);
            //debuginfo.Update("\n01234567890ABCDEFGHIJKLMNOPQRSTUVWQYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_+><~" + '\u001b' + "{}|.`,:;/@'[\\]\"ᴥ");
            debuginfo.Update("\n\nTotal of " + eList.Count + " enemies");
            debuginfo.Update("\nEnemy Info:");

            float avghp = 0;
            for (int i = eList.Count - 1; i >= 0; i--)
            {
                avghp += eList[i].Health;
            }
            avghp = avghp / eList.Count;
            debuginfo.Update("\n  avg health: " + avghp);
            for (int i = 0; i < eList.Count; i++)
            {
                debuginfo.Update("\n    enemy " + i);
                debuginfo.Update("\n  collided: " + eList[i].collided.Count);
                debuginfo.Update("\n   effects: " + eList[i].ActiveEffects.Count);
                for (int j = 0; j < eList[i].ActiveEffects.Count; j++)
                {
                    debuginfo.Update("\n effect[" + j + "]: " + eList[i].ActiveEffects[j].enchantType + ", str: " + eList[i].ActiveEffects[j].strength);
                }
                debuginfo.Update("\n    health: " + eList[i].Health);
                debuginfo.Update("\n     input: " + eList[i].input);
                debuginfo.Update("\n  jumptime: " + eList[i].jumpTime);
                debuginfo.Update("\n  activity: " + eList[i].AIactivity);
                if (eList[i].AIactivity == AI.persue || eList[i].AIactivity == AI.combat)
                {
                    debuginfo.Update("\nattackCD: " + eList[i].attackCooldownTimer);
                }
                else if (eList[i].AIactivity == AI.wander)
                {
                    debuginfo.Update("\nwandtime: " + eList[i].wanderTime);
                }
                debuginfo.Update("\n  Xpos:" + eList[i].Pos.X + "\n  Ypos:" + eList[i].Pos.Y);
                debuginfo.Update("\n  Xvel:" + eList[i].velocity.X + "\n  Yvel:" + eList[i].velocity.Y);
                if (i > 3)
                {
                    i = eList.Count;
                }
            }
        }

        public static bool IsTouching(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Right >= rect2.Left && rect1.Top <= rect2.Bottom && rect1.Bottom >= rect2.Top && rect1.Left <= rect2.Right)
            {
                return true;
            }
            return false;
        }

        public static bool IsTouching(Rectangle rect1, Rectangle rect2, Side side)
        {
            switch (side)
            {
                case Side.bottom:
                    if (rect1.Right > rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom >= rect2.Top && rect1.Left < rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.right:
                    if (rect1.Right >= rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top && rect1.Left < rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.left:
                    if (rect1.Right > rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top && rect1.Left <= rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.top:
                    if (rect1.Right > rect2.Left && rect1.Top <= rect2.Bottom && rect1.Bottom > rect2.Top && rect1.Left < rect2.Right)
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }

        public static float DistanceSquared(Point p1, Point p2)
        {
            int tempX = (p2.X - p1.X);
            int tempY = (p2.Y - p1.Y);
            return (tempX * tempX) + (tempY * tempY);
            //return ((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public static bool GetKey(Keys key)
        {
            if (keyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public static bool GetKeyDown(Keys key)
        {
            if (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public string TimerText(double timer)
        {
            return ((int)(timer/60)).ToString("00") + ":" + (timer % 60).ToString(timerAccuracy);
        }

        public static void PlayerDeath()
        {
            if (onslaughtMode)
            {
                if (onslaughtSpawner.wave > savefile.bestOnslaughtWave)
                {
                    savefile.bestOnslaughtWave = onslaughtSpawner.wave;
                    savefile.bestOnslaughtWaveLevel = currentLevel;
                }
            }
            else
            {

            }
            int loselisttimerindex = savefile.loseList.IndexOf(currentLevel);
            if (loselisttimerindex >= 0)
            {
                if (timer > savefile.timerLoseList[loselisttimerindex])
                {
                    savefile.timerLoseList[loselisttimerindex] = timer;
                }
            }
            else
            {
                savefile.loseList.Add(currentLevel);
                savefile.timerLoseList.Add(timer);
            }

            savefile.Save(autosave);
        }

        public void SummonGenericEnemy()
        {
            if (enemySpawnPoints.Count > 0)
            {
                Enemy tempEnemy = new Enemy(enemy0Tex, enemySpawnPoints[(int)(RAND.NextDouble() * enemySpawnPoints.Count)], 100f, 10f, 300f, this);
                eList.Add(tempEnemy);
                enemyList.Add(tempEnemy);
                //collisionObjects.Add(tempEnemy);
            }
            else
            {
                Console.WriteLine("Error, no spawn points");
            }
        }

        public void SummonGenericEnemy(float health, float damage, float speed)
        {
            if (enemySpawnPoints.Count > 0)
            {
                Enemy tempEnemy = new Enemy(enemy0Tex, enemySpawnPoints[(int)(RAND.NextDouble() * enemySpawnPoints.Count)], health, damage, speed, this);
                eList.Add(tempEnemy);
                enemyList.Add(tempEnemy);
                //collisionObjects.Add(tempEnemy);
            }
            else
            {
                Console.WriteLine("Error, no spawn points");
            }
        }

        public PlayerSettings Load(string filename)
        {
            PlayerSettings playerSettings = PlayerSettings.Load(filename);

            attackKey = playerSettings.attackKey;
            altAttackKey = playerSettings.altAttackKey;
            shockwaveKey = playerSettings.shockwaveKey;
            altShockwaveKey = playerSettings.altShockwaveKey;
            shieldKey = playerSettings.shieldKey;
            altShieldKey = playerSettings.altShieldKey;
            jumpKey = playerSettings.jumpKey;
            altJumpKey = playerSettings.altJumpKey;
            upKey = playerSettings.upKey;
            altUpKey = playerSettings.altUpKey;
            downKey = playerSettings.downKey;
            altDownKey = playerSettings.altDownKey;
            leftKey = playerSettings.leftKey;
            altLeftKey = playerSettings.altLeftKey;
            rightKey = playerSettings.rightKey;
            altRightKey = playerSettings.altRightKey;
            rollKey = playerSettings.rollKey;
            altRollKey = playerSettings.altRollKey;
            potionKey = playerSettings.potionKey;
            altPotionKey = playerSettings.altPotionKey;

            cameraLerp = playerSettings.cameraLerp;
            cameraLerpSpeed = playerSettings.cameraLerpSpeed;
            boundingBox = playerSettings.boundingBox;
            debug = playerSettings.debug;
            initialPos = playerSettings.initialPosition;
            minSqrDetectDistance = playerSettings.minSqrDetectDistance;
            cameraShakeSetting = playerSettings.cameraShakeSetting;
            cameraSwingSetting = playerSettings.cameraSwingSetting;
            swingDuration = playerSettings.swingDuration;
            swingMagnitude = playerSettings.swingMagnitude;
            timerAccuracy = playerSettings.timerAccuracy;
            graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = playerSettings.vSync;
            tempResolution = playerSettings.resolution;
            masterAudioLevel = playerSettings.masterAudioLevel;
            musicLevel = playerSettings.musicLevel;
            soundEffectsLevel = playerSettings.soundEffectsLevel;

            if (!resetRequired)
            {
                tempResolution = resolution = playerSettings.resolution;
                halfResolution.X = resolution.X / 2;
                halfResolution.Y = resolution.Y / 2;
                consoleRect = new Rectangle(0, -halfResolution.Y, resolution.X, halfResolution.Y);
                zeroScreenspace = new Rectangle(Point.Zero, resolution);
                if (consoleWriteline != null)
                {
                    developerConsole.Update(resolution.X);
                    consoleWriteline.Update(resolution.X);
                }
                screenScale = playerSettings.screenScale;
                graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                graphics.IsFullScreen = playerSettings.fullscreen;
            }

            graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = playerSettings.vSync;
            graphics.ApplyChanges();
            renderTarget = new RenderTarget2D(GraphicsDevice, resolution.X, resolution.Y);

            if (geralt != null)
            {
                geralt.Load(playerSettings);
            }

            return playerSettings;
        }

        public static bool Use()
        {
            if ((keyboardState.IsKeyDown(shockwaveKey) && !previousKeyboardState.IsKeyDown(shockwaveKey)) ||
                (keyboardState.IsKeyDown(altShockwaveKey) && !previousKeyboardState.IsKeyDown(altShockwaveKey)) ||
                (keyboardState.IsKeyDown(attackKey) && !previousKeyboardState.IsKeyDown(attackKey)) ||
                (keyboardState.IsKeyDown(altAttackKey) && !previousKeyboardState.IsKeyDown(altAttackKey)) ||
                (keyboardState.IsKeyDown(jumpKey) && !previousKeyboardState.IsKeyDown(jumpKey)) ||
                (keyboardState.IsKeyDown(altJumpKey) && !previousKeyboardState.IsKeyDown(altJumpKey)))
            {
                return true;
            }
            return false;
        }

        public static float Lerp(float value1, float value2, float amount)
        {
            if (amount > 1)
            {
                return value2;
            }
            return value1 + (value2 - value1) * amount;
        }

        public static bool IsDefaultLevelFormat(string level)
        {
            if (level.Length > 0 && level[0].Equals('c'))
            {
                string temp = level.Substring(1);
                //bool isDefault = true;
                bool encounteredMapChar = false;
                foreach (char c in temp)
                {
                    if (!char.IsDigit(c))
                    {
                        if (!encounteredMapChar && (c.Equals('b') || c.Equals('o')))
                        {
                            encounteredMapChar = true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                if (!encounteredMapChar)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// returns the chapter and map of a default level in the format:
        /// c1o12 returns new Point(1, 12);   c3b4 returns new Point(3, 4);
        /// </summary>
        public static Point GetLevelChapterAndMap(string level)
        {
            if (IsDefaultLevelFormat(level) && level[0].Equals('c'))
            {
                string temp = level.Substring(1);
                string chapter = string.Empty;
                string map = string.Empty;
                //bool isDefault = true;
                bool encounteredMapChar = false;
                foreach (char c in temp)
                {
                    if (!char.IsDigit(c))
                    {
                        if (c.Equals('b') || c.Equals('o'))
                        {
                            encounteredMapChar = true;
                        }
                    }
                    else
                    {
                        if (encounteredMapChar)
                        {
                            map += c;
                        }
                        else
                        {
                            chapter += c;
                        }
                    }
                }

                return new Point(int.Parse(chapter), int.Parse(map));
            }

            return Point.Zero;
        }

        public void OpenConsole()
        {
            textInputBuffer = string.Empty;
            acceptTextInput = console = !console;
            if (geralt != null)
            {
                geralt.inputEnabled = !console;
                //framebyframe = console;
            }
            consoleLine = developerConsole.lines + 1;
            consoleMoveTimer = 1f - consoleMoveTimer;
        }

        public void MoveConsole()
        {
            consoleMoveTimer -= DeltaTime;
            if (console)
            {
                consoleRect.Y = (int)Lerp(-halfResolution.Y, 0, 1 - consoleMoveTimer);
            }
            else
            {
                consoleRect.Y = (int)Lerp(0, -halfResolution.Y, 1 - consoleMoveTimer);
            }
            if (consoleMoveTimer <= 0)
            {
                consoleMoveTimer = 0;
            }
            consoleWriteline.Update(new Point(1, consoleRect.Bottom - 10));
            developerConsole.Update(new Point(1, consoleRect.Bottom - 20));
        }

        public void UpdateConsole()
        {
            consoleWriteline.Update(textInputBuffer, true);
            if (GetKeyDown(Keys.Down) && developerConsole.lines >= consoleLine + 1)
            {
                consoleLineChangeTimer = 0f;
                consoleLine++;
                textInputBuffer = developerConsole.GetLine(consoleLine);
            }
            if (GetKeyDown(Keys.Up) && consoleLine - 1 >= 0)
            {
                consoleLineChangeTimer = 0f;
                consoleLine--;
                textInputBuffer = developerConsole.GetLine(consoleLine);
            }
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Down))
            {
                consoleLineChangeTimer += DeltaTime;
            }
            if (consoleLineChangeTimer >= 0.5f)
            {
                if (keyboardState.IsKeyDown(Keys.Up) && consoleLine - 1 >= 0)
                {
                    consoleLine--;
                    textInputBuffer = developerConsole.GetLine(consoleLine);
                }
                if (keyboardState.IsKeyDown(Keys.Down) && developerConsole.lines >= consoleLine + 1)
                {
                    consoleLine++;
                    textInputBuffer = developerConsole.GetLine(consoleLine);
                }
                consoleLineChangeTimer -= 0.05f;
            }


            if ((keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter)) && !levelEditor)
            {
                consoleWriteline.Update(string.Empty, true);
                ConsoleParser(textInputBuffer);
                textInputBuffer = string.Empty;
            }

            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/
                (GetKeyDown(Keys.Escape) || (GetKeyDown(Keys.Pause))))
            {
                OpenConsole();
            }
        }

        public void ExportConsole()
        {
            string timenow = (DateTime.Now).ToShortDateString() + "." + (DateTime.Now).ToString("HH:mm:ss");
            string nameoffile = ".\\";

            foreach (char c in timenow)
            {
                if (char.IsDigit(c) || c.Equals('.'))
                {
                    nameoffile += c;
                }
            }

            nameoffile += ".txt";

            Console.WriteLine("saving " + nameoffile + "...");
            Irbis.WriteLine("saving " + nameoffile + "...");

            File.WriteAllText(nameoffile, developerConsole.Konsole);
        }

        public void CleanConsole()
        {
            while (developerConsole.lines > 2000)
            {
                developerConsole.Clear();
            }
        }

        public void Debug()
        {
            debug = !debug;
            if (!sceneIsMenu)
            {
                this.IsMouseVisible = debug;
            }
            Console.WriteLine("DEBUG MODE: " + debug);
            if (!debug)
            {
                debuginfo.Clear();
                foreach (Square s in squareList)
                {
                    s.color = Color.White;
                }
            }
        }

        public static void WriteLine(string line)
        {
            developerConsole.WriteLine(line);
            consoleLine = developerConsole.lines + 1;
        }

        public static void WriteLine()
        {
            developerConsole.WriteLine();
            consoleLine = developerConsole.lines + 1;
        }

        public static void Write(string line)
        {
            developerConsole.Write(line);
        }

        public string Credits()
        {
            string credits =
@"  Programming, Game Design, Level Design, Mechanics
                 Jonathan ""Darius"" Miu
                      LN2 Software

                       Art Design:
                          Flea
                      lordpulex.com

                      Play Testers:
                          Flea
                          Reiji

                       Animation:
                        someone
";

            string returncredits = string.Empty;
            foreach (char c in credits)
            {
                if (!c.Equals('\u000D'))
                {
                    returncredits += c;
                }
            }

            return returncredits;
        }

        public string Help()
        {
            string help =
@"enter commands in the form: command=valueToPass or command=(optional value)
List of commands:

debug  ------------  enables debugmode                    leveleditor  -----------------  enter level editor
version  ----------  print game's current version          savelevel=(level name)
summonenemy  ------  summons one enemy                     newlevel=(level name)
summonenemies  ----  summons -value- enemies               spawnblock=(block texture)
notarget  ---------  disables all AI                       squarelist  -----------------  lists ALL squares
killall  ----------  kills all enemies                     spawnpoints  ----------------  list enemy spawn points
skiptowave=wave  --  if onslaught, skip to that wave        removespawnpoint=indexOfSpawn
load=levelToLoad  -  load level with that name (if exists)  removespawnpoints=indexOfFirstSpawn,indexOfLastSpawn
newlevel=(name)  --  creates empty level with that name     removeallspawnpoints  ------  deletes all spawns
killme  -----------  kills the player                       addspawnpoint=X,Y  ---------  adds spawn at X, Y
printsave  --------  print information from autosave      Enchants  --------------------  list active enchants
credits  ----------  display the game's credits            enchant=enchantType  --------  bleed, fire, frost, knockback, poison, sharpness, stun
export  -----------  export console log to a file           add the same enchant multiple times to upgrade its strength
help  -------------  displays this help page               disenchant  -----------------  remove all enchants
exit  -------------  quits                                    
quit  -------------  exits                                    
";

            string returnhelp = string.Empty;
            foreach (char c in help)
            {
                if (!c.Equals('\u000D'))
                {
                    returnhelp += c;
                }
            }

            return returnhelp;
        }

        public void Quit()
        {
            Exit();
        }

        public void PrintVersion()
        {
            WriteLine("    Project: Irbis (" + versionTy + ")");
            WriteLine("    " + versionID + " v" + versionNo);
        }

        private static Point PointParser(string value)
        {
            string Xval = string.Empty;
            string Yval = string.Empty;
            bool encounteredComma = false;

            int j = 0;
            while (value.Length > j)
            {
                if (!encounteredComma)
                {
                    if (value[j] != ',')
                    {
                        Xval += value[j];
                        j++;
                    }
                    else
                    {
                        encounteredComma = true;
                        j++;
                    }
                }
                else
                {
                    Yval += value[j];
                    j++;
                }
            }
            if (int.TryParse(Xval, out int Xresult))
            {
                if (int.TryParse(Yval, out int Yresult))
                {
                    return new Point(Xresult, Yresult);
                }
                else
                {
                    Console.WriteLine("error: Point could not be parsed");
                }
            }
            else
            {
                Console.WriteLine("error: Point could not be parsed");
            }
            return Point.Zero;
        }

        public void ConsoleParser(string line)
        {
            line.Trim();
            WriteLine();
            WriteLine(line);
            line = line.ToLower();

            if (line.Length >= 1 && !line[0].Equals('\u003b'))
            {
                string variable = string.Empty;             //'\u003d'://=
                string extra = string.Empty;
                string value = string.Empty;                //'\u003b'://;
                string statement = string.Empty;

                foreach (char c in line)
                {
                    if (!char.IsWhiteSpace(c))
                    {
                        statement += c;
                    }
                }

                int stage = 0;
                //stage 0  = START
                //stage 1  = encountered [ pulling contents of brackets
                //stage 2  = encountered ] dump info until reaching = (should always be next character)
                //stage 3  = encountered = pulling value
                //stage -1 = encountered ; FULL STOP
                //the below IF statements should be in reverse stage order (highest stage first)

                int intResult;
                float floatResult;
                Keys keyResult;
                EnchantType enchantResult;
                bool boolResult;

                foreach (char c in statement)               //'\u005b':[ //'\u005d':]
                {
                    if (stage == 3)
                    {
                        if (c.Equals('\u003b'))
                        {
                            stage = -1;
                        }
                        if (stage > 0)
                        {
                            value += c;
                        }
                    }
                    if (stage == 2)
                    {
                        if (c.Equals('\u003d'))
                        {
                            stage = 3;
                        }
                        else
                        {
                            //do nothing
                        }
                    }
                    if (stage == 1)
                    {
                        if (c.Equals('\u005d'))
                        {
                            stage = 2;
                        }
                        else
                        {
                            extra += c;
                        }
                    }
                    if (stage == 0)
                    {
                        if (c.Equals('\u003d'))
                        {
                            stage = 3;
                        }
                        else
                        {
                            if (c.Equals('\u005b'))
                            {
                                stage = 1;
                            }
                            else
                            {
                                variable += c;
                            }
                        }
                    }
                }

                switch (variable.ToLower())
                {
                    case "attack1damage":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.attack1Damage = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "attack2damage":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.attack2Damage = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "speed":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.speed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "jumptimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.jumpTimeMax = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "idletimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.idleTimeMax = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "maxhealth":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.maxHealth = floatResult;
                            healthBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "maxshield":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.maxShield = floatResult;
                            shieldBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "maxenergy":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.maxEnergy = floatResult;
                            energyBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "superShockwaveHoldtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.superShockwaveHoldtime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavemaxeffectdistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveMaxEffectDistance = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwaveeffectivedistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveEffectiveDistance = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavestuntime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveStunTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "invulnerablemaxtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.invulnerableMaxTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "energyrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.energyRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "healthrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.healthRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "energyusablemargin":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.energyUsableMargin = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "minsqrdetectdistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            minSqrDetectDistance = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;

                    case "shieldanimationspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldAnimationSpeed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldhealingpercentage":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldHealingPercentage = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "cameralerpspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            cameraLerpSpeed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "terminalvelocity":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.terminalVelocity = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "masteraudiolevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            masterAudioLevel = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "musiclevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            musicLevel = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "soundeffectslevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            soundEffectsLevel = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.potionRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionrechargetime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.potionRechargeTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    //public float potionRechargeRate;
                    //public float potionRechargeTime;





                    case "xcollideroffset":                                                         //place new floats above
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.XcolliderOffset = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "ycollideroffset":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.YcolliderOffset = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "colliderwidth":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.colliderWidth = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "colliderheight":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.colliderHeight = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "attackcolliderwidth":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.attackColliderWidth = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "attackcolliderheight":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.attackColliderHeight = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "screenscale":
                        if (int.TryParse(value, out intResult))
                        {
                            screenScale = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "maxnumberofpotions":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.maxNumberOfPotions = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;





                    case "attackkey":                                                               //place new ints above
                        if (Enum.TryParse(value, out keyResult))
                        {
                            attackKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altattackkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altAttackKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavekey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            shockwaveKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altshockwavekey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altShockwaveKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            shieldKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altshieldkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altShieldKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "jumpkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            jumpKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altjumpkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altJumpKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "upkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            upKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altupkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altUpKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "downkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            downKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altdownkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altDownKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "leftkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            leftKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altleftkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altLeftKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "rightkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            rightKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altrightkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altRightKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "rollkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            rollKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altrollkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altRollKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            potionKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altpotionkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altPotionKey = keyResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;





                    case "timeraccuracy":
                        if (int.TryParse(value, out intResult))
                        {
                            timerAccuracy = "00.";
                            if (intResult > 8)
                            {
                                for (int i = 8; i > 0; i--)
                                {
                                    timerAccuracy += "0";
                                }
                            }
                            else
                            {
                                for (int i = intResult; i > 0; i--)
                                {
                                    timerAccuracy += "0";
                                }
                            }
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }

                        break;





                    case "cameralerp":                                                                //place new "etc variable" (like vectors and rectangles) above
                        if (bool.TryParse(value, out boolResult))
                        {
                            cameraLerp = boolResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "camerashakesetting":
                        if (bool.TryParse(value, out boolResult))
                        {
                            cameraShakeSetting = boolResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "fullscreen":
                        if (bool.TryParse(value, out boolResult))
                        {
                            graphics.IsFullScreen = boolResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "verticalretrace":
                        if (bool.TryParse(value, out boolResult))
                        {
                            graphics.SynchronizeWithVerticalRetrace = boolResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "Isfixedtimestep":
                        if (bool.TryParse(value, out boolResult))
                        {
                            IsFixedTimeStep = boolResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "summonenemy":
                        SummonGenericEnemy();
                        break;
                    case "summonenemies":
                        if (int.TryParse(value, out intResult))
                        {
                            for (int i = intResult; i > 0; i--)
                            {
                                SummonGenericEnemy();
                            }
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "notarget":
                        AIenabled = !AIenabled;
                        foreach (Enemy e in eList)
                        {
                            e.AIenabled = AIenabled;
                        }
                        break;
                    case "killall":
                        eList.Clear();
                        enemyList.Clear();
                        break;
                    case "savelevel":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            SaveLevel(currentLevel);
                            WriteLine("saved as " + currentLevel);
                        }
                        else
                        {
                            SaveLevel(value);
                            LoadLevel(value, true);
                            WriteLine("saved as " + value);
                        }
                        break;
                    case "skiptowave":
                        if (onslaughtMode)
                        {
                            if (int.TryParse(value, out intResult))
                            {
                                onslaughtSpawner.SkipToWave(intResult);
                                WriteLine("wave: " + onslaughtSpawner.wave);
                            }
                            else
                            {
                                WriteLine("error: variable \"" + variable + "\" could not be parsed");

                            }
                        }
                        break;
                    case "load":
                        if (File.Exists(".\\levels\\" + value + ".lvl"))
                        {
                            LoadLevel(value, true);
                            WriteLine("loading level: " + value);
                        }
                        else
                        {
                            //???
                        }
                        break;
                    case "newlevel":
                        if (levelEditor && !string.IsNullOrWhiteSpace(value))
                        {
                            Level thisLevel = new Level();
                            SaveLevel(value);
                            LoadLevel(value, true);
                            WriteLine("creating new level: " + value);
                        }
                        break;
                    case "leveleditor":
                        levelEditor = sceneIsMenu = true;
                        geralt = null;
                        WriteLine("levelEditor: " + levelEditor);
                        break;
                    case "spawnpoints":
                        for (int i = 0; i < enemySpawnPoints.Count; i++)
                        {
                            WriteLine("enemySpawnPoints[" + i + "]: " + enemySpawnPoints[i]);
                        }
                        WriteLine(enemySpawnPoints.Count + " enemy spawn points");
                        break;
                    case "squarelist":
                        for (int i = 0; i < squareList.Count; i++)
                        {
                            WriteLine("squareList[" + i + "] position: " + squareList[i].Position + ", collider: " + squareList[i].Collider);
                        }
                        break;
                    case "addspawnpoint":
                        if (PointParser(value) != Point.Zero)
                        {
                            Point tempPoint = PointParser(value);
                            enemySpawnPoints.Add(tempPoint.ToVector2());
                            WriteLine("added spawn point at " + tempPoint);
                        }
                        break;
                    case "removespawnpoint":
                        if (int.TryParse(value, out intResult))
                        {
                            if (intResult >= 0 && intResult < enemySpawnPoints.Count)
                            {
                                enemySpawnPoints.RemoveAt(intResult);
                                WriteLine("removed spawn point " + intResult);
                            }
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "removespawnpoints":
                        if (PointParser(value) != Point.Zero)
                        {
                            Point tempPoint = PointParser(value);

                            for (int i = tempPoint.X; i <= tempPoint.Y; i++)
                            {
                                enemySpawnPoints.RemoveAt(tempPoint.X);
                            }
                            WriteLine("removed " + ((tempPoint.Y - tempPoint.X) + 1) + " spawn points");
                        }
                        break;
                    case "removeallspawnpoints":
                        enemySpawnPoints.Clear();
                        break;
                    case "spawnblock":
                        if (levelEditor)
                        {
                            SaveLevel(currentLevel);

                            if (string.IsNullOrWhiteSpace(value))
                            {
                                WriteLine("spawning block with defaultTex texture at " + new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))));
                                Texture2D defaultSquareTex = Content.Load<Texture2D>("defaultTex");
                                Square tempSquare = new Square(defaultSquareTex, new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))), true, 0.3f);
                                squareList.Add(tempSquare);
                            }
                            else
                            {
                                WriteLine("spawning block with" + value + " texture at " + new Point((int)(camera.X % 32), (int)(camera.Y % 32)));
                                Texture2D defaultSquareTex = Content.Load<Texture2D>(value);
                                Square tempSquare = new Square(defaultSquareTex, new Point((int)(camera.X % 32), (int)(camera.Y % 32)), false, 0.3f);
                                squareList.Add(tempSquare);
                            }
                        }
                        break;
                    case "addenchant":
                        if (Enum.TryParse(value, out enchantResult) && geralt != null)
                        {
                            int hasEnchant = -1;
                            for (int i = 0; i < geralt.enchantList.Count; i++)
                            {
                                if (geralt.enchantList[i].enchantType == enchantResult)
                                {
                                    hasEnchant = i;
                                }
                            }
                            if (hasEnchant >= 0)
                            {
                                geralt.enchantList[hasEnchant].Upgrade();
                                WriteLine(geralt.enchantList[hasEnchant].enchantType + " upgraded");
                            }
                            else
                            {
                                geralt.enchantList.Add(new Enchant(enchantResult, 1));
                                WriteLine(enchantResult + " added");
                            }
                        }
                        else
                        {
                            WriteLine("error: enchant \"" + value + "\" could not be parsed");

                        }
                        break;
                    case "disenchant":
                        geralt.enchantList.Clear();
                        WriteLine("removed all enchants");
                        break;
                    case "enchants":
                        if (geralt.enchantList.Count <= 0)
                        {
                            WriteLine("no enchants");
                        }
                        foreach (Enchant e in geralt.enchantList)
                        {
                            WriteLine("Enchant: " + e.enchantType + ", str: " + e.strength + ", val: " + e.effectValue + ", dur: " + e.effectDuration + ", maxStack: " + e.maxStack);
                        }
                        break;
                    case "lines":
                        WriteLine(developerConsole.lines.ToString());
                        break;
                    case "clearlog":
                        developerConsole.Clear();
                        break;
                    case "version":
                        PrintVersion();
                        break;
                    case "killme":
                        WriteLine("but why?");
                        WriteLine("to actually kill yourself, enter 'just do it'");
                        break;
                    case "justdoit":
                        PlayerDeath();
                        break;
                    case "printsave":
                        savefile.Print();
                        break;






                    case "debug":                                                                     //place new bools above
                        Debug();
                        WriteLine("debug: " + debug);
                        break;
                    case "exit":
                        Exit();
                        break;
                    case "quit":
                        Exit();
                        break;
                    case "mow":
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            if (int.TryParse(value, out intResult))
                            {
                                WriteLine("");
                                for (int i = 0; i < intResult; i++)
                                {
                                    developerConsole.Write("mow");
                                }

                                if (intResult > 25)
                                {
                                    WriteLine("It's probably pretty cluttered here now... why don't you -clearlog-?");
                                }
                            }
                            else
                            {
                                WriteLine("Yep, yep, I'm a snep!");
                            }
                        }
                        else
                        {
                            WriteLine("Yep, yep, I'm a snep!");
                        }

                        break;
                    case "credits":
                        WriteLine(Credits());
                        break;
                    case "export":
                        ExportConsole();
                        break;
                    case "exportconsole":
                        ExportConsole();
                        break;
                    case "help":
                        WriteLine(Help());
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(extra))
                        {
                            WriteLine("statement: " + statement);
                            WriteLine(" variable: " + variable);
                            WriteLine("    value: " + value);
                        }
                        else
                        {
                            WriteLine("statement: " + statement);
                            WriteLine(" variable: " + variable);
                            WriteLine("    extra: " + extra);
                            WriteLine("    value: " + value);
                        }

                        if (string.IsNullOrWhiteSpace(extra))
                        {
                            WriteLine("error: no variable with name: \"" + variable + "\"");
                        }
                        else
                        {
                            WriteLine("error: no variable with name: \"" + variable + "[" + extra + "]\"");
                        }
                        break;
                }
            }
        }

        protected override void Draw(GameTime gameTime)                                                                         //DRAW
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //if (!sceneIsMenu)

            // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
            
            //spritebatch for BACKGROUNDS
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, background);
            if (debug)
            {
                foreach (Square b in backgroundSquareList)
                {
                    b.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            //standard spritebatch, draw level and player and enemies here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, foreground);
            foreach (Enemy e in eList)
            {
                e.Draw(spriteBatch);
            }
            foreach (Square s in squareList)
            {
                s.Draw(spriteBatch);
            }
            if (geralt != null) { geralt.Draw(spriteBatch); }
            if (debug)
            {
                startingScreenLocation.Draw(spriteBatch, nullTex);
                if (levelEditor)
                {
                    Texture2D squareTex = Content.Load<Texture2D>("originTexture");
                    foreach (Vector2 p in enemySpawnPoints)
                    {
                        Square tempSquare = new Square(squareTex, p.ToPoint(), true, 0.9f);
                        tempSquare.Draw(spriteBatch);
                    }
                }

            }
            spriteBatch.End();

            //spritebatch for static elements (like UI and menus, etc)
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
            if (healthBar != null) { healthBar.Draw(spriteBatch); }
            if (shieldBar != null) { shieldBar.Draw(spriteBatch); }
            if (energyBar != null) { energyBar.Draw(spriteBatch); }
            if (potionBar != null) { potionBar.Draw(spriteBatch); }
            if (displayEnemyHealth) { enemyHealthBar.Draw(spriteBatch); }
            if (timerDisplay != null) { timerDisplay.Draw(spriteBatch); }
            if (onslaughtDisplay != null) { onslaughtDisplay.Draw(spriteBatch); }

            debuginfo.Draw(spriteBatch);

            if (debug)
            {
                boundingBoxBorder.Draw(spriteBatch, nullTex);
            }

            if ((console || consoleMoveTimer > 0) && !sceneIsMenu)
            {
                spriteBatch.Draw(nullTex, consoleRect, consoleRect, new Color(Color.DarkSlateBlue, 1f), 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
                consoleWriteline.Draw(spriteBatch);
                developerConsole.Draw(spriteBatch);
            }
            spriteBatch.End();
            // --------------------------------------------------------------------------------------------------------------------------------

            if (sceneIsMenu)        //FOR MENU DRAWING
            {
                // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                if (console || consoleMoveTimer > 0)
                {
                    spriteBatch.Draw(nullTex, consoleRect, consoleRect, new Color(Color.DarkSlateBlue, 1f), 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
                    consoleWriteline.Draw(spriteBatch);
                    developerConsole.Draw(spriteBatch);
                }

                if (levelLoaded > 0)
                {
                    //darken bg screen
                    spriteBatch.Draw(nullTex, new Rectangle(Point.Zero, resolution), new Color(Color.Black, 0.25f));
                }

                if (scene == 3 && menuSelection >= 0 && menuSelection <= 3)
                {
                    boundingBoxBorder.Draw(spriteBatch, nullTex);
                }

                if (scene == 5)
                {
                    foreach (UIElementSlider s in sliderList)
                    {
                        s.Draw(spriteBatch);
                    }
                }

                foreach (Button b in buttonList)
                {
                    b.Draw(spriteBatch);
                }
                for (int i = 0; i < printList.Count; i++)
                {
                    if (scene == 3)
                    {
                        if (i == 0)
                        {
                            if (menuSelection >= 0 && menuSelection <= 3)
                            {
                                printList[i].Draw(spriteBatch);
                            }
                        }
                        else
                        {
                            printList[i].Draw(spriteBatch);
                        }
                    }
                    else
                    {
                        printList[i].Draw(spriteBatch);
                    }
                }
                foreach (Square s in sList)
                {
                    s.Draw(spriteBatch);
                }


                spriteBatch.End();
                // --------------------------------------------------------------------------------------------------------------------------------
            }

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, Matrix.Identity);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
