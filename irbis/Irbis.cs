using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

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
        Bleed = 0,          //long duration, low damage                 stacks:                             upgrade: longer duration, more damage/second
        Fire = 1,           //short duration, high damage               stacks:                             upgrade: longer duration, more damage/second
        Frost = 2,          //long duration, slows                      stacks:                             upgrade: longer duration
        Knockback = 3,      //knocks enemies back short distance        stacks:                             upgrade: knocks back further
        Poison = 4,         //long duration, high damage, limited use   stacks:                             upgrade: more uses
        Sharpness = 5,      //flat damage increase (no duration)        stacks:                             upgrade: more damage
        Stun = 6,           //short duration, full stun                 stacks:                             upgrade: longer duration
    }
    public enum VendingType
    {
        Enchant = 0,
        Health = 1,
        Energy = 2,
        Shield = 3,
        Potion = 4,
        Life = 5,
    }

    public interface IDrawableObject : System.IComparable
    {
        float Depth
        {
            get;
        }
        void Draw(SpriteBatch sb);
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
        Vector2 Position
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
        void Knockback(Direction knockbackDirection, float strength);
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
        Vector2 Position
        {
            get;
            set;
        }

    }

    public class Irbis : Game
    {                                                                                               //version info
        /// version number key (two types): 
        /// release number . software stage (pre/alpha/beta) . build/version . build iteration
        /// release number . content patch number . software stage . build iteration
        static string versionNo = "0.1.3.3";
        static string versionID = "alpha";
        static string versionTy = "debug";
        /// Different version types: 
        /// debug
        /// release candidate
        /// release

                                                                                                    //debug
        public static int debug = 1;
        private static Print debuginfo;
        private static SmartFramerate smartFPS;
        public static bool framebyframe;
        public static bool nextframe;
        private static TotalMeanFramerate meanFPS;
        private static double minFPS;
        private static double minFPStime;
        private static double maxFPS;
        private static double maxFPStime;
        private static bool recordFPS;
        private static int framedropfactor = 3;

                                                                                                    //console
        EventHandler<TextInputEventArgs> onTextEntered;
        public static bool acceptTextInput;
        public static string textInputBuffer;
        private static Print consoleWriteline;
        private static Print developerConsole;
        public static bool console;
        private static int consoleLine;
        private static float consoleLineChangeTimer;
        private static float consoleMoveTimer;
        private static Rectangle consoleRect;
        private static Color consoleRectColor = new Color(31, 29, 37, 255);
        private static Texture2D consoleTex;

                                                                                                    //properties
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
        
        public static KeyboardState GetKeyboardState
        {
            get
            {
                return keyboardState;
            }
        }
        public static MouseState GetMouseState
        {
            get
            {
                return mouseState;
            }
        }
        public static MouseState GetPreviousMouseState
        {
            get
            {
                return previousMouseState;
            }
        }
        public static KeyboardState GetPreviousKeyboardState
        {
            get
            {
                return previousKeyboardState;
            }
        }
        public static bool GetEscapeKey
        {
            get
            {
                return (keyboardState.IsKeyDown(Keys.Escape));
            }
        }
        public static bool GetUseKey
        {
            get
            {
                return (keyboardState.IsKeyDown(useKey) || keyboardState.IsKeyDown(altUseKey));
            }
        }
        public static bool GetEnterKey
        {
            get
            {
                return (keyboardState.IsKeyDown(Keys.Enter));
            }
        }
        public static bool GetAttackKey
        {
            get
            {
                return (keyboardState.IsKeyDown(attackKey) || keyboardState.IsKeyDown(altAttackKey));
            }
        }
        public static bool GetShockwaveKey
        {
            get
            {
                return (keyboardState.IsKeyDown(shockwaveKey) || keyboardState.IsKeyDown(altShockwaveKey));
            }
        }
        public static bool GetShieldKey
        {
            get
            {
                return (keyboardState.IsKeyDown(shieldKey) || keyboardState.IsKeyDown(altShieldKey));
            }
        }
        public static bool GetJumpKey
        {
            get
            {
                return (keyboardState.IsKeyDown(jumpKey) || keyboardState.IsKeyDown(altJumpKey));
            }
        }
        public static bool GetUpKey
        {
            get
            {
                return (keyboardState.IsKeyDown(upKey) || keyboardState.IsKeyDown(altUpKey));
            }
        }
        public static bool GetDownKey
        {
            get
            {
                return (keyboardState.IsKeyDown(downKey) || keyboardState.IsKeyDown(altDownKey));
            }
        }
        public static bool GetLeftKey
        {
            get
            {
                return (keyboardState.IsKeyDown(leftKey) || keyboardState.IsKeyDown(altLeftKey));
            }
        }
        public static bool GetRightKey
        {
            get
            {
                return (keyboardState.IsKeyDown(rightKey) || keyboardState.IsKeyDown(altRightKey));
            }
        }
        public static bool GetPotionKey
        {
            get
            {
                return (keyboardState.IsKeyDown(potionKey) || keyboardState.IsKeyDown(altPotionKey));
            }
        }
        public static bool GetRollKey
        {
            get
            {
                return (keyboardState.IsKeyDown(rollKey) || keyboardState.IsKeyDown(altRollKey));
            }
        }
        public static bool GetEscapeKeyDown
        {
            get
            {
                return (GetKeyDown(Keys.Escape));
            }
        }
        public static bool GetUseKeyDown
        {
            get
            {
                return (GetKeyDown(useKey) || GetKeyDown(altUseKey));
            }
        }
        public static bool GetEnterKeyDown
        {
            get
            {
                return (GetKeyDown(Keys.Enter));
            }
        }
        public static bool GetAttackKeyDown
        {
            get
            {
                return (GetKeyDown(attackKey) || GetKeyDown(altAttackKey));
            }
        }
        public static bool GetShockwaveKeyDown
        {
            get
            {
                return (GetKeyDown(shockwaveKey) || GetKeyDown(altShockwaveKey));
            }
        }
        public static bool GetShieldKeyDown
        {
            get
            {
                return (GetKeyDown(shieldKey) || GetKeyDown(altShieldKey));
            }
        }
        public static bool GetJumpKeyDown
        {
            get
            {
                return (GetKeyDown(jumpKey) || GetKeyDown(altJumpKey));
            }
        }
        public static bool GetUpKeyDown
        {
            get
            {
                return (GetKeyDown(upKey) || GetKeyDown(altUpKey));
            }
        }
        public static bool GetDownKeyDown
        {
            get
            {
                return (GetKeyDown(downKey) || GetKeyDown(altDownKey));
            }
        }
        public static bool GetLeftKeyDown
        {
            get
            {
                return (GetKeyDown(leftKey) || GetKeyDown(altLeftKey));
            }
        }
        public static bool GetRightKeyDown
        {
            get
            {
                return (GetKeyDown(rightKey) || GetKeyDown(altRightKey));
            }
        }
        public static bool GetPotionKeyDown
        {
            get
            {
                return (GetKeyDown(potionKey) || GetKeyDown(altPotionKey));
            }
        }
        public static bool GetRollKeyDown
        {
            get
            {
                return (GetKeyDown(rollKey) || GetKeyDown(altRollKey));
            }
        }
        public static bool GetLeftMouseDown
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released);
            }
        }
        public static bool GetEscapeKeyUp
        {
            get
            {
                return (GetKeyUp(Keys.Escape));
            }
        }
        public static bool GetUseKeyUp
        {
            get
            {
                return (GetKeyUp(useKey) || GetKeyUp(altUseKey));
            }
        }
        public static bool GetEnterKeyUp
        {
            get
            {
                return (GetKeyUp(Keys.Enter));
            }
        }
        public static bool GetAttackKeyUp
        {
            get
            {
                return (GetKeyUp(attackKey) || GetKeyUp(altAttackKey));
            }
        }
        public static bool GetShockwaveKeyUp
        {
            get
            {
                return (GetKeyUp(shockwaveKey) || GetKeyUp(altShockwaveKey));
            }
        }
        public static bool GetShieldKeyUp
        {
            get
            {
                return (GetKeyUp(shieldKey) || GetKeyUp(altShieldKey));
            }
        }
        public static bool GetJumpKeyUp
        {
            get
            {
                return (GetKeyUp(jumpKey) || GetKeyUp(altJumpKey));
            }
        }
        public static bool GetUpKeyUp
        {
            get
            {
                return (GetKeyUp(upKey) || GetKeyUp(altUpKey));
            }
        }
        public static bool GetDownKeyUp
        {
            get
            {
                return (GetKeyUp(downKey) || GetKeyUp(altDownKey));
            }
        }
        public static bool GetLeftKeyUp
        {
            get
            {
                return (GetKeyUp(leftKey) || GetKeyUp(altLeftKey));
            }
        }
        public static bool GetRightKeyUp
        {
            get
            {
                return (GetKeyUp(rightKey) || GetKeyUp(altRightKey));
            }
        }
        public static bool GetPotionKeyUp
        {
            get
            {
                return (GetKeyUp(potionKey) || GetKeyUp(altPotionKey));
            }
        }
        public static bool GetRollKeyUp
        {
            get
            {
                return (GetKeyUp(rollKey) || GetKeyUp(altRollKey));
            }
        }


                                                                                                    //graphics
        public static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

                                                                                                    //save info
        private static string autosave;
        public static SaveFile savefile;
        public static bool isMenuScrollable;
        public static int maxButtonsOnScreen;
        public static int levelListCounter;
        public static string currentLevel;

                                                                                                    //lists
        private static List<Square> backgroundSquareList;
        public static List<ICollisionObject> collisionObjects;
        public static List<Square> sList;
        public static List<Square> squareList;
        public static List<Button> buttonList;
        public static List<Enemy> eList;
        public static List<IEnemy> enemyList;
        public static List<Print> printList;
        public static List<UIElementSlider> sliderList;
        public static string[] levelList;

                                                                                                    //player
        public static Player geralt;
        public static Vector2 initialPos;

                                                                                                    //onslaught
        public static bool onslaughtMode;
        public static OnslaughtSpawner onslaughtSpawner;
        private static Print onslaughtDisplay;
        public static int vendingMachineUseDistanceSqr;

                                                                                                    //camera
        private static Vector2 camera;
        private static Vector2 trueCamera;
        private static Vector2 mainCamera;
        private static Vector2 screenSpacePlayerPos;
        private static Matrix background;
        private static Matrix foreground;
        private static Matrix UIground;
        public static Rectangle boundingBox;
        public static Rectangle screenspace;
        public static Rectangle zeroScreenspace;
        public static float screenScale;
        public static int textScale;
        public static Point resolution;
        public static bool cameraLerp;
        public static float cameraLerpSpeed;
        public static bool cameraShakeSetting;
        private static float cameraShakeDuration;
        private static float cameraShakeMagnitude;
        private static float cameraSwingDuration;
        private static float cameraSwingMaxDuration;
        private static float cameraSwingMagnitude;
        private static float cameraShakeLerpTime;
        private static float cameraShakeLerpTimeMax;
        private static Vector2 cameraShakeTargetLocation;
        public static float swingDuration;
        public static float swingMagnitude;
        static Vector2 cameraSwingHeading;
        public static bool cameraSwingSetting;
        static bool cameraSwing;


                                                                                                    //menu
        public static Menu menu;
        private static Texture2D[] menuTex;
        public static int menuSelection;
        public static bool listenForNewKeybind;
        public static bool resetRequired;
        public static int levelLoaded;
        public static int scene;
        public static bool sceneIsMenu;
        public static bool levelEditor;
        int selectedBlock;
        public static VendingMenu vendingMachineMenu;

                                                                                                    //enemy/AI variables
        public static bool AIenabled;
        Texture2D enemy0Tex;
        public Vector2 bossSpawn;
        public List<Vector2> enemySpawnPoints;

                                                                                                    //UI
        public static Font font;
        public static UIElementSlider healthBar;
        public static UIElementSlider shieldBar;
        public static UIElementSlider energyBar;
        public static UIElementDiscreteSlider potionBar;
        public static UIElementSlider enemyHealthBar;
        public static Print timerDisplay;
        public static string timerAccuracy;
        public static float minSqrDetectDistance;
        public static bool displayEnemyHealth;
        public static SpriteFont spriteFont;
        public static SpriteFont spriteFont2;
        public static int vendingMenu;

                                                                                                    //settings vars
        public static Point halfResolution;
        public static Point tempResolution;

        public static float masterAudioLevel;
        public static float musicLevel;
        public static float soundEffectsLevel;

        public float randomTimer;
        public static int sliderPressed;

                                                                                                    //keyboard/keys
        private static KeyboardState keyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState mouseState;
        private static MouseState previousMouseState;
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
        public static Keys useKey;
        public static Keys altUseKey;

                                                                                                    //threading
        private static int threadCount;
        private static bool useMultithreading = false;
        public static ManualResetEvent doneEvent;
        public static int pendingThreads;
        private static object listLock = new object();

                                                                                                    //etc
        public static float gravity;
        private static Random RAND;
        private static int lastRandomInt;
        public static Texture2D nullTex;
        public static Texture2D largeNullTex;
        public static Texture2D defaultTex;
        public static Game game;
        public static BinaryTree<float> testTree;
        private static float nextFrameTimer;
        private static BasicEffect basicEffect;
        private static Matrix projection = Matrix.Identity;
        private static Ray[] debugrays = new Ray[50];
        private static Line[] debuglines = new Line[5];
        private static Shape[] debugshapes = new Shape[4];
        private static Shape shadowShape;
        private static List<Vector2> shadows;
        public static TooltipGenerator tooltipGenerator;
        Point worldSpaceMouseLocation;

        public Irbis()
        {
            //Never remove these console debug lines
            Console.WriteLine("    Project: Irbis (" + versionTy + ")");
            Console.WriteLine("    " + versionID + " v" + versionNo);

            game = this;

            this.IsMouseVisible = false;

            sceneIsMenu = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;

            resetRequired = false;
            IsFixedTimeStep = false;

            Content.RootDirectory = ".\\content";
            gravity = 1125f;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related Content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Console.WriteLine("initializing");
            // TODO: Add your initialization logic here
            threadCount = Environment.ProcessorCount;

            sList = new List<Square>();
            collisionObjects = new List<ICollisionObject>();
            squareList = new List<Square>();
            backgroundSquareList = new List<Square>();
            buttonList = new List<Button>();
            eList = new List<Enemy>();
            enemyList = new List<IEnemy>();
            printList = new List<Print>();
            sliderList = new List<UIElementSlider>();
            doneEvent = new ManualResetEvent(false);

            AIenabled = true;
            nextframe = true;
            //pressed = false;
            randomTimer = 0f;

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.TextureEnabled = false;
            basicEffect.VertexColorEnabled = true;// = Color.Red;

            cameraShakeLerpTimeMax = 0.025f;
            cameraShakeLerpTime = 0f;
            levelLoaded = -1;

            RAND = new Random();
            displayEnemyHealth = false;

            Window.TextInput += TextEntered;
            onTextEntered += HandleInput;
            acceptTextInput = false;
            textInputBuffer = "";

            //EventInput.EventInput.Initialize(this.Window);
            camera = screenSpacePlayerPos = mainCamera = Vector2.Zero;
            foreground = Matrix.Identity;
            background = Matrix.Identity;
            UIground = Matrix.Identity;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your Content.
        /// </summary>
        protected override void LoadContent()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LoadContent"); }
            Console.WriteLine("loading content");
            Texture2D fontTex2 = Content.Load<Texture2D>("font2");
            Font font2 = new Font(fontTex2, 8);
            consoleTex = Content.Load<Texture2D>("console texture");
            developerConsole = new Print(font2);
            PrintVersion();
            Irbis.WriteLine("Number of logical processors:" + threadCount);
            WriteLine();

            tooltipGenerator = new TooltipGenerator(game);

            testTree = new BinaryTree<float>();
            vendingMenu = -1;

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

            savefile = new SaveFile(true);
            if (File.Exists(autosave))
            {
                savefile.Load(autosave);
            }
            else
            {
                Console.WriteLine("creating new autosave.snep...");
                WriteLine("creating new autosave.snep...");
                savefile = new SaveFile(false);
                savefile.Save(autosave);
            }

            developerConsole.textScale = textScale;

            Texture2D playerTex = Content.Load<Texture2D>("player");
            Texture2D shieldTex = Content.Load<Texture2D>("shield");

            geralt = new Player(playerTex, shieldTex, playerSettings, 0.5f);
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
            largeNullTex = Content.Load<Texture2D>("largeNullTex");
            defaultTex = Content.Load<Texture2D>("defaultTex");

            enemySpawnPoints = new List<Vector2>();

            Texture2D fontTex = Content.Load<Texture2D>("font");

            font = new Font(fontTex, playerSettings.characterHeight, playerSettings.characterWidth, false);
            spriteFont2 = Content.Load<SpriteFont>("console font");

            menu = new Menu();

            debuginfo = new Print(resolution.X, font2, Color.White, true, new Point(1, 3), Direction.left, 0.95f);
            consoleWriteline = new Print(resolution.X, font2, Color.White, true, new Point(1, 5), Direction.left, 1f);
            developerConsole.Update(resolution.X);
            developerConsole.scrollDown = false;
            consoleMoveTimer = 0f;
            console = false;

            printList.Add(debuginfo);
            printList.Add(consoleWriteline);
            printList.Add(developerConsole);

            smartFPS = new SmartFramerate(5);

            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = projection;
            
            for (int i = 0; i < debuglines.Length; i++)
            {
                debuglines[i] = new Line(new Vector2(300f * i, 300f), new Vector2(100f * i, 500f));
            }
            //debugrays[0] = new Ray(halfResolution.ToVector2() / screenScale, Vector2.One);
            for (int i = 0; i < debugrays.Length; i++)
            {
                float myAngleInRadians = (2f * (float)Math.PI) * ((float)i / 50f);
                Vector2 angleVector = new Vector2(
                    (float)Math.Cos(myAngleInRadians),
                    -(float)Math.Sin(myAngleInRadians));
                debugrays[i] = new Ray(halfResolution.ToVector2() / screenScale, angleVector);
            }

            vendingMachineUseDistanceSqr = 2500;

            Vector2[] debugshapevertices = new Vector2[3];
            debugshapevertices[0] = new Vector2(100f, 200f);
            debugshapevertices[1] = new Vector2(300f, 200f);
            debugshapevertices[2] = new Vector2(400f, 400f);
            debugshapes[0] = new Shape(debugshapevertices);

            debugshapevertices = new Vector2[4];
            debugshapevertices[0] = new Vector2(900f, 700f);
            debugshapevertices[1] = new Vector2(600f, 500f);
            debugshapevertices[2] = new Vector2(600f, 450f);
            debugshapevertices[3] = new Vector2(500f, 600f);
            debugshapes[1] = new Shape(debugshapevertices);

            debugshapevertices = new Vector2[5];
            debugshapevertices[0] = new Vector2(900f, 100f);
            debugshapevertices[1] = new Vector2(850f, 450f);
            debugshapevertices[2] = new Vector2(700f, 500f);
            debugshapevertices[3] = new Vector2(800f, 200f);
            debugshapevertices[4] = new Vector2(750f, 100f);
            debugshapes[2] = new Shape(debugshapevertices);

            debugshapes[3] = new Shape(zeroScreenspace);

            shadows = new List<Vector2>();
            WriteLine("creating shadowShape");
            shadowShape = new Shape(shadows.ToArray());

            //scenes 0-10 are reserved for menus

            LoadScene(11);   //master scene ID
                             //LOAD THIS FROM SAVE FILE FOR BACKGROUND

            LoadMenu(0, 0, false);
            sceneIsMenu = true;

            //WriteLine("Type 'help' for a few commands");
            Console.WriteLine("done.");
        }

        private void LoadScene(int SCENE)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LoadScene"); }
            scene = SCENE;
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
        }

        public void LoadMenu(int Scene, int startMenuLocation, bool loadSettings)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LoadMenu"); }
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
            //sList.Add(new Square(menuTex[scene], Color.White, Point.Zero, menuTex[scene].Width, menuTex[scene].Height, false, true, true, 0.5f));
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LoadLevel"); }
            this.IsMouseVisible = false;
            scene = 11;
            sceneIsMenu = false;
            currentLevel = filename;

            ClearLevel();

            Level thisLevel = new Level(true);
            thisLevel.Load(".\\levels\\" + filename + ".lvl");

            List<Point> squareSpawns = thisLevel.SquareSpawnPoints;

            List<Point> BackgroundSquares = thisLevel.BackgroundSquares;

            onslaughtMode = thisLevel.isOnslaught;
            initialPos = thisLevel.PlayerSpawn;
            //foreach (Vector2 v in thisLevel.EnemySpawnPoints)
            //{
            //    enemySpawnPoints.Add(v / 2f);
            //}
            enemySpawnPoints = thisLevel.EnemySpawnPoints;
            bossSpawn = thisLevel.BossSpawn;

            enemy0Tex = Content.Load<Texture2D>("enemy0");
            Texture2D shieldBarTex = Content.Load<Texture2D>("shieldBar");
            Texture2D centerTex = Content.Load<Texture2D>("centerTexture");

            if (loadUI)
            {
                healthBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale)), 125, 10, geralt.maxHealth, Color.Red, new Color(166, 030, 030), nullTex, nullTex, nullTex, font, false, 0.505f, 0.55f, 0.5f);
                shieldBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale) + 10), 75, 10, geralt.maxShield, Color.Red, new Color(255, 170, 000), nullTex, nullTex, shieldBarTex, font, false, 0.5f);
                energyBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale) + 20), 50, 10, geralt.maxEnergy, Color.Red, new Color(000, 234, 255), nullTex, nullTex, nullTex, font, false, 0.5f);
                potionBar = new UIElementDiscreteSlider(Direction.left, new Rectangle((int)(32 / screenScale) + 76, (int)(32 / screenScale) + 11, 48, 8), nullTex, nullTex, nullTex, Color.DarkSlateGray, Color.DarkRed, Color.DarkSlateBlue, geralt.maxNumberOfPotions, 3, 0.5f);
                enemyHealthBar = new UIElementSlider(Direction.right, new Point((int)((resolution.X / screenScale) - (32 / screenScale)), (int)(32 / screenScale)), 250, 10, 100, Color.Red, new Color(166, 030, 030), nullTex, nullTex, nullTex, font, false, 0.5f);

                if (geralt != null)
                {
                    geralt.Respawn(initialPos);
                    //lighting.Lights.Add(geralt.light);
                }
            }
            else
            {
                if (geralt != null)
                {
                    geralt.Respawn(new Vector2(-1000f, -1000f));
                }
            }

            if (onslaughtMode)
            {
                onslaughtSpawner = new OnslaughtSpawner();

                Point[] vendingMachineLocations = thisLevel.VendingMachineLocations;
                VendingType[] vendingMachineTypes = thisLevel.VendingMachineTypes;
                string[] vendingMachineTextures = thisLevel.VendingMachineTextures;
                for (int i = 0; i < vendingMachineTextures.Length; i++)
                {
                    onslaughtSpawner.vendingMachineList.Add(new VendingMachine(200, vendingMachineTypes[i], new Rectangle(vendingMachineLocations[i], new Point(64, 64)), Content.Load<Texture2D>(vendingMachineTextures[i]), 0.35f));
                }

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

            for (int i = 0; i < thisLevel.squareTextures.Count; i++)
            {
                Texture2D squareTex = Content.Load<Texture2D>(thisLevel.squareTextures[i]);
                Square tempSquare = new Square(squareTex, squareSpawns[i], true, thisLevel.squareDepth);
                collisionObjects.Add(tempSquare);
                squareList.Add(tempSquare);
            }

            for (int i = 0; i < thisLevel.backgroundSquareDepths.Count; i++)                                                                                              //backgrounds
            {
                Texture2D squareTex = Content.Load<Texture2D>(thisLevel.backgroundTextures[i]);
                Square tempSquare = new Square(squareTex, Color.White, BackgroundSquares[i], squareTex.Width, squareTex.Height, false, true, true, thisLevel.backgroundSquareDepths[i]);
                backgroundSquareList.Add(tempSquare);
            }

            camera.X = resolution.X / 2;
            camera.Y = resolution.Y / 2;

            if (levelLoaded < 0) //THIS MEANS WE ARE LOADING A LEVEL FOR THE TITLESCREEN
            {
                levelLoaded = 0;
                WriteLine("for a list of commands, enter: help");
            }
            else
            {
                levelLoaded = scene;
                savefile.lastPlayedLevel = currentLevel;
                savefile.Save(autosave);
                WriteLine("for a list of all squares, enter: squareList");
            }
        }

        

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific Content.
        /// </summary>
        protected override void UnloadContent()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.UnloadContent"); }
            // TODO: Unload any non ContentManager Content here
        }



        protected override void Update(GameTime gameTime)
        {
            pendingThreads = 0;
            doneEvent = new ManualResetEvent(false);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            if (debug > 0)
            {
                if (!console && (gameTime.ElapsedGameTime.TotalSeconds) > (framedropfactor * DeltaTime))
                {
                    WriteLine("recording framedrop(1/" + framedropfactor + ")\nold fps: " + (1 / DeltaTime) + ", new fps: " + (1 / gameTime.ElapsedGameTime.TotalSeconds) + "\ntimer: " + Timer);
                }
                if (recordFPS)
                {
                    meanFPS.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    if (maxFPS < (1d / gameTime.ElapsedGameTime.TotalSeconds))
                    {
                        maxFPS = (1d / gameTime.ElapsedGameTime.TotalSeconds);
                        maxFPStime = Timer;
                    }
                    if (minFPS > (1d / gameTime.ElapsedGameTime.TotalSeconds))
                    {
                        minFPS = (1d / gameTime.ElapsedGameTime.TotalSeconds);
                        minFPStime = Timer;
                    }
                }

                if (debug > 1)
                {
                    if (debug > 2)
                    {
                        PrintDebugInfo();

                        if (geralt != null)
                        {
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
                    }
                    else
                    {
                        debuginfo.Update("\n\n\nᴥ" + smartFPS.Framerate.ToString("0000"), true);
                    }
                }
                else
                {
                    debuginfo.Update("\n\n\nᴥ" + smartFPS.Framerate.ToString("0000"), true);
                }
            }
            else
            {
                debuginfo.Update("\n\n\nᴥ" + smartFPS.Framerate.ToString("0000"), true);
            }
            smartFPS.Update(gameTime.ElapsedGameTime.TotalSeconds);
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!sceneIsMenu && !acceptTextInput)
            {
                LevelUpdate(gameTime);
            }
            else
            {
                mouseState = Mouse.GetState();
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

            if (debug > 3)
            {
                //if (mouseState.LeftButton == ButtonState.Pressed)
                //{
                //    debugrays[0].Origin = (mouseState.Position.ToVector2() / screenScale);
                //}
                //if (mouseState.RightButton == ButtonState.Pressed)
                //{
                //    debugrays[0].Direction = (mouseState.Position.ToVector2() - halfResolution.ToVector2()) - (new Vector2(debugrays[0].Origin.X * screenScale, -debugrays[0].Origin.Y * screenScale));
                //}

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    for (int i = 0; i < debugrays.Length; i++)
                    {
                        debugrays[i] = new Ray(mouseState.Position.ToVector2() / screenScale, debugrays[i].Direction);
                    }
                }
                //if (mouseState != previousMouseState)
                //{
                //    for (int i = 0; i < debugrays.Length; i++)
                //    {
                //        debugrays[i] = new Ray(halfResolution.ToVector2() / screenScale, mouseState.Position.ToVector2() - (halfResolution.ToVector2()));
                //    }
                //}


                //if (!console)
                //{
                //    if (GetKey(Keys.Y))
                //    {
                //        debugline.Origin += new Vector2(0.01f, 0); 
                //    }
                //    if (GetKey(Keys.H))
                //    {
                //        debugline.Origin -= new Vector2(0.01f, 0);
                //    }
                //    if (GetKey(Keys.U))
                //    {
                //        debugline.Origin += new Vector2(0, 0.01f);
                //    }
                //    if (GetKey(Keys.J))
                //    {
                //        debugline.Origin -= new Vector2(0, 0.01f);
                //    }
                //    if (GetKey(Keys.I))
                //    {
                //        projection.M41 += 0.001f;
                //    }
                //    if (GetKey(Keys.K))
                //    {
                //        projection.M41 -= 0.001f;
                //    }
                //    if (GetKey(Keys.O))
                //    {
                //        projection.M42 += 0.001f;
                //    }
                //    if (GetKey(Keys.L))
                //    {
                //        projection.M42 -= 0.001f;
                //    }
                //}


                shadows.Clear();
                for (int i = 0; i < debugrays.Length; i++)
                {
                    Vector2 tempvector = debugrays[i].Intersect(debugshapes);
                    //if (shadows.Count > i && shadows[i] != tempvector && tempvector != Vector2.Zero)
                    //{
                    //    shadows[i] = tempvector;
                    //}
                    /*else*/ if (/*shadows.Count < i &&*/ tempvector != Vector2.Zero)
                    {
                        shadows.Add(tempvector);
                    }
                }

                shadowShape.Vertices = shadows.ToArray();
            }

            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
            if (pendingThreads <= 0) { doneEvent.Set(); }
            doneEvent.WaitOne();
            base.Update(gameTime);
        }

        protected void MenuUpdate()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.MenuUpdate"); }
            if (!console)
            {
                menu.Update(this);
            }
        }

        protected void LevelUpdate(GameTime gameTime)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LevelUpdate"); }
            if ((keyboardState.IsKeyDown(Keys.T) && !acceptTextInput) || geralt.health <= 0)                            //RESPAWN
            {
                geralt.Respawn(initialPos);

                foreach (Enemy e in eList)
                {
                    collisionObjects.Remove(e);
                }
                eList.Clear();
                enemyList.Clear();
            }

            if (GetKeyDown(Keys.N) && !acceptTextInput)
            {
                nextFrameTimer = 0f;
                framebyframe = true; //nex
            }
            else if (framebyframe && GetKey(Keys.N) && !acceptTextInput)
            {
                nextFrameTimer += DeltaTime;
            }
            if (keyboardState.IsKeyDown(Keys.G) && !acceptTextInput)
            {
                framebyframe = false; //nex
            }
            if (!framebyframe || GetKeyDown(Keys.N) || nextFrameTimer > 0.1f) //nex
            {
                if (framebyframe)
                {
                    nextFrameTimer -= 0.1f;
                }
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                geralt.Update();

                Camera();

                if (timerDisplay != null) { timerDisplay.Update(TimerText(timer), true); }
                healthBar.UpdateValue(false, geralt.health);
                if (geralt.shielded)
                {
                    shieldBar.UpdateValue(true, geralt.shield);
                }
                else
                {
                    shieldBar.UpdateValue(false, geralt.shield);
                }
                energyBar.UpdateValue(geralt.energy);

                if (useMultithreading)
                {
                    for (int i = 0; i < eList.Count; i++)
                    {
                        QueueThread(eList[i].ThreadPoolCallback);
                    }
                }
                else
                {
                    for (int i = 0; i < eList.Count; i++)
                    {
                        eList[i].Update();
                        if (eList[i].Health <= 0 || eList[i].Position.Y > 5000f)
                        {
                            KillEnemy(eList[i]);
                            i--;
                        }
                    }
                }

                if (true)
                {
                    if (enemyList.Count > 0)
                    {
                        IEnemy closest = enemyList[0];

                        float closestSqrDistance = float.MaxValue;
                        float thisEnemysSqrDistance = 0f;
                        try
                        {
                            foreach (IEnemy e in enemyList)
                            {
                                thisEnemysSqrDistance = DistanceSquared(geralt.Collider.Center, e.Collider.Center);
                                if (thisEnemysSqrDistance < closestSqrDistance)
                                {
                                    closestSqrDistance = thisEnemysSqrDistance;
                                    closest = e;
                                }
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            WriteLine("caught: InvalidOperationException");
                            Console.WriteLine("caught: InvalidOperationException");
                            //just continue as normal
                        }
                        if (closestSqrDistance <= minSqrDetectDistance)
                        {
                            displayEnemyHealth = geralt.combat = true;
                            geralt.Combat();
                            enemyHealthBar.maxValue = closest.MaxHealth;
                            enemyHealthBar.UpdateValue(closest.Health);
                        }
                        else
                        { displayEnemyHealth = geralt.combat = false; }
                    }
                    else
                    { displayEnemyHealth = geralt.combat = false; }
                }
            }

            if (onslaughtMode)
            {
                if (onslaughtSpawner.enemiesLeftThisWave > 0 && eList.Count < onslaughtSpawner.maxEnemies && onslaughtSpawner.EnemySpawnTimer())
                {
                    SummonGenericEnemy(onslaughtSpawner.enemyHealth, onslaughtSpawner.enemyDamage, onslaughtSpawner.enemySpeed);
                }
                if (eList.Count <= 0 && onslaughtSpawner.enemiesKilled >= onslaughtSpawner.enemiesThisWave)
                {
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

                if (GetUseKeyDown)
                {
                    for (int i = 0; i < onslaughtSpawner.vendingMachineList.Count; i++)
                    {
                        if (DistanceSquared(onslaughtSpawner.vendingMachineList[i].collider.Center, geralt.Collider.Center) <= vendingMachineUseDistanceSqr) //add collider to vending machine
                        {
                            onslaughtSpawner.vendingMachineList[i].LoadMenu();
                            geralt.inputEnabled = !onslaughtSpawner.vendingMachineList[i].drawMenu;
                            if (!geralt.inputEnabled)
                            {
                                vendingMenu = i;
                                IsMouseVisible = true;
                                WriteLine("vending machine menu open");
                            }
                            else
                            {
                                vendingMenu = -1;
                                IsMouseVisible = false;
                                WriteLine("vending machine menu close");
                            }
                        }
                    }
                }

                if (vendingMenu >= 0)
                {
                    if (geralt.inputEnabled)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].LoadMenu();
                        vendingMenu = -1;
                        IsMouseVisible = false;
                        WriteLine("vending machine menu close");
                    }
                    if (GetLeftKeyDown)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].Update(onslaughtSpawner.vendingMachineList[vendingMenu].selection - 1);
                    }
                    if (GetRightKeyDown)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].Update(onslaughtSpawner.vendingMachineList[vendingMenu].selection + 1);
                    }
                    if (GetUpKeyDown)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].MoveSelectionUp();
                    }
                    if (GetDownKeyDown)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].MoveSelectionDown();
                    }
                    if (GetLeftMouseDown)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].OnClick(mouseState.Position);
                    }

                    if (mouseState != previousMouseState)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].Update(mouseState.Position);
                    }

                    if (Use())
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].Purchase(onslaughtSpawner.vendingMachineList[vendingMenu].selection);
                    }
                }
            }

            if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/
                (GetKeyDown(Keys.Escape) || (GetKeyDown(Keys.Pause))))
            {
                if (vendingMenu >= 0)
                {
                    onslaughtSpawner.vendingMachineList[vendingMenu].LoadMenu();
                    geralt.inputEnabled = true;
                    vendingMenu = -1;
                    IsMouseVisible = false;
                    WriteLine("vending machine menu close");
                }
                else
                {
                    framebyframe = false;
                    LoadMenu(0, 0, false);
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



        private static void QueueThread(WaitCallback callBack)
        {
            ThreadPool.QueueUserWorkItem(callBack);
            Interlocked.Increment(ref pendingThreads);
        }

        private static void QueueThreadNoWait(WaitCallback callBack)
        {
            ThreadPool.QueueUserWorkItem(callBack);
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

        private void Camera()
        {            
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Camera"); }
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

            trueCamera.X = halfResolution.X - camera.X;
            trueCamera.Y = halfResolution.Y - camera.Y;

            screenSpacePlayerPos.X = ((geralt.TrueCenter.X * screenScale) + halfResolution.X - mainCamera.X);
            screenSpacePlayerPos.Y = ((geralt.TrueCenter.Y * screenScale) + halfResolution.Y - mainCamera.Y);

            screenspace.X = (int)(-trueCamera.X);
            screenspace.Y = (int)(-trueCamera.Y);

            background.M31 = foreground.M41 = trueCamera.X;
            background.M32 = foreground.M42 = trueCamera.Y;

            if (cameraShakeSetting && cameraShakeDuration > 0) { CameraShake(); }
            if (cameraSwingSetting && cameraSwingDuration > 0) { CameraSwing(); }

            //lighting.Transform = foreground;
            //geralt.light.Position = screenSpacePlayerPos;
        }

        private static void CameraSwing()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.CameraSwing"); }
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

        public static void CameraSwing(float duration, float magnitude, Vector2 heading)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.CameraSwing"); }
            cameraSwingDuration = cameraSwingMaxDuration = duration;
            cameraSwingHeading = heading;
            cameraSwingHeading.Normalize();
            cameraSwingMagnitude = magnitude * screenScale;
            cameraSwing = true;
        }

        private static void CameraShake()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.CameraShake"); }
            cameraShakeDuration -= DeltaTime;
            cameraShakeLerpTime -= DeltaTime;
            if (cameraShakeLerpTime <= 0)
            {
                cameraShakeTargetLocation.X = (halfResolution.X - mainCamera.X) + ((((float)RAND.NextDouble() * 2) - 1) * cameraShakeMagnitude);       //X
                cameraShakeTargetLocation.Y = (halfResolution.Y - mainCamera.Y) + ((((float)RAND.NextDouble() * 2) - 1) * cameraShakeMagnitude);       //Y
                if (debug > 0)
                {
                    WriteLine("               background:{X:" + background.M31 + " Y:" + background.M32 + "}\ncameraShakeTargetLocation:" + cameraShakeTargetLocation);
                    WriteLine();
                }
                cameraShakeLerpTime = cameraShakeLerpTimeMax;
            }
            background.M31 = foreground.M41 = Lerp(background.M31, cameraShakeTargetLocation.X, (cameraShakeLerpTimeMax - cameraShakeLerpTime) / cameraShakeLerpTimeMax);       //X
            background.M32 = foreground.M42 = Lerp(background.M32, cameraShakeTargetLocation.Y, (cameraShakeLerpTimeMax - cameraShakeLerpTime) / cameraShakeLerpTimeMax);       //Y
        }

        public static void CameraShake(float duration, float magnitude)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.CameraShake"); }
            //cameraTimePerShake = 0f;
            if (duration > cameraShakeDuration)
            {
                cameraShakeDuration = duration;
            }
            cameraShakeLerpTime = 0;
            cameraShakeMagnitude = magnitude * screenScale;
        }

        public static void RemoveFromPrintList(string printStatement)
        {
            for (int i = 0; i < printList.Count; i++)
            {
                if (printList[i].statement.Equals(printStatement))
                {
                    printList.RemoveAt(i);
                    return;
                }
            }
        }

        public void EnableLevelEditor()
        {
            ClearUI();
            levelEditor = sceneIsMenu = true;
            geralt = null;

        }

        public void LevelEditor()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.LevelEditor"); }
            debug = 3;
            this.IsMouseVisible = sceneIsMenu = true;
            //PrintDebugInfo();
            if (!acceptTextInput)
            {
                if (GetKey(Keys.LeftShift))
                {
                    if (keyboardState.IsKeyDown(upKey))
                    {
                        camera.Y -= 10f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(downKey))
                    {
                        camera.Y += 10f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(leftKey))
                    {
                        camera.X -= 10f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(rightKey))
                    {
                        camera.X += 10f * DeltaTime;
                    }
                }
                else
                {
                    if (keyboardState.IsKeyDown(upKey))
                    {
                        camera.Y -= 1000f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(downKey))
                    {
                        camera.Y += 1000f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(leftKey))
                    {
                        camera.X -= 1000f * DeltaTime;
                    }
                    if (keyboardState.IsKeyDown(rightKey))
                    {
                        camera.X += 1000f * DeltaTime;
                    }
                }
            }

            background.M31 = foreground.M41 = halfResolution.X - camera.X;
            background.M32 = foreground.M42 = halfResolution.Y - camera.Y;
            screenspace.X = (int)(camera.X - halfResolution.X);
            screenspace.Y = (int)(camera.Y - halfResolution.Y);

            worldSpaceMouseLocation = (mouseState.Position.ToVector2() / screenScale).ToPoint() + (camera / screenScale).ToPoint() - (halfResolution.ToVector2() / screenScale).ToPoint();
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
                squareList[selectedBlock].Position = worldSpaceMouseLocation.ToVector2();
            }
        }

        public bool ConvertOldLevelFilesToNew()
        {
            string[] leeevelList = Directory.GetFiles(".\\levels");
            maxButtonsOnScreen = resolution.Y / 25;
            Console.WriteLine("length pre format check: " + leeevelList.Length);

            if (true)
            {
                List<string> tempLevelList = new List<string>();

                foreach (string s in leeevelList)
                {
                    tempLevelList.Add(s);
                }

                for (int i = 0; i < tempLevelList.Count; i++)
                {
                    if (tempLevelList[i].StartsWith(".\\levels\\") && tempLevelList[i].EndsWith(".lvl"))
                    {
                        //tempLevelList[i] = tempLevelList[i].Substring(9);
                        //tempLevelList[i] = tempLevelList[i].Remove(tempLevelList[i].Length - 4);
                    }
                    else
                    {
                        tempLevelList.RemoveAt(i);
                    }
                }

                leeevelList = tempLevelList.ToArray();
            }

            Console.WriteLine("length post format check: " + leeevelList.Length);


            foreach (string s in leeevelList)
            {
                Level lvl = new Level(true);
                Console.WriteLine("attempting load on: " + s);
                lvl.Load(s);
                Console.WriteLine(lvl.ToString());
                OldLevel.Save(OldLevel.LevelConverter(lvl), s);
                OldLevel testlvl = new OldLevel(true);
                testlvl.Load(s);
                testlvl.Debug(true);
            }


            return true;
        }

        public void SaveLevel(string levelname)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.SaveLevel"); }
            Level thisLevel = new Level(true);
            //thisLevel.squareList = squareList;

            List<Point> squareSpawns = new List<Point>();
            List<string> squareTextures = new List<string>();
            for (int i = 0; i < squareList.Count; i++)
            {
                squareSpawns.Add(squareList[i].Position.ToPoint());
                squareTextures.Add(squareList[i].tex.Name);
            }

            List<Point> BackgroundSquares = new List<Point>();
            List<string> backgroundTextures = new List<string>();
            List<float> backgroundSquareDepths = new List<float>();
            for (int i = 0; i < backgroundSquareList.Count; i++)
            {
                BackgroundSquares.Add(backgroundSquareList[i].Position.ToPoint());
                backgroundTextures.Add(backgroundSquareList[i].tex.Name);
                backgroundSquareDepths.Add(backgroundSquareList[i].depth);
            }

            thisLevel.SquareSpawnPoints = squareSpawns;
            thisLevel.squareTextures = squareTextures;
            thisLevel.BackgroundSquares = BackgroundSquares;
            thisLevel.backgroundTextures = backgroundTextures;
            thisLevel.backgroundSquareDepths = backgroundSquareDepths;
            thisLevel.levelName = currentLevel;

            if (onslaughtSpawner != null)
            {
                List<Point> vendingMachineLocations = new List<Point>();
                List<VendingType> vendingMachineTypes = new List<VendingType>();
                List<string> vendingMachineTextures = new List<string>();

                foreach (VendingMachine v in onslaughtSpawner.vendingMachineList)
                {
                    vendingMachineTextures.Add(v.vendingTex.ToString());
                    vendingMachineLocations.Add(v.displayLocation.ToPoint());
                    vendingMachineTypes.Add(v.type);
                }

                thisLevel.VendingMachineLocations = vendingMachineLocations.ToArray();
                thisLevel.VendingMachineTextures = vendingMachineTextures.ToArray();
                thisLevel.VendingMachineTypes = vendingMachineTypes.ToArray();

                WriteLine("vendingMachineLocations count: " + vendingMachineLocations.Count);
                WriteLine("vendingMachineTextures count: " + vendingMachineTextures.Count);
                WriteLine("vendingMachineTypes count: " + vendingMachineTypes.Count);
            }
            else
            {
                thisLevel.VendingMachineLocations = new Point[0];
                thisLevel.VendingMachineTextures = new string[0];
                thisLevel.VendingMachineTypes = new VendingType[0];
            }

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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.ClearLevel"); }
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

            if (recordFPS)
            {
                meanFPS = new TotalMeanFramerate(true);
                maxFPS = double.MinValue;
                maxFPStime = double.NaN;
                minFPS = double.MaxValue;
                minFPStime = double.NaN;
            }

            timer = 0;

            screenspace = new Rectangle(Point.Zero, resolution);
        }

        public void ClearUI()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.ClearUI"); }
            sList.Clear();
            printList.Clear();
            buttonList.Clear();

            healthBar = null;
            shieldBar = null;
            energyBar = null;
            potionBar = null;
            enemyHealthBar = null;
        }

        public void PrintDebugInfo()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.PrintDebugInfo"); }
            debuginfo.Update("      DEBUG MODE. " + versionID.ToUpper() + " v" + versionNo, true);
            debuginfo.Update("\n DeltaTime:" + DeltaTime);
            debuginfo.Update("\n       FPS:" + (1 / DeltaTime).ToString("0000.0"));
            debuginfo.Update("\n  smartFPS:" + smartFPS.Framerate.ToString("0000.0"));
            if (meanFPS != null)
            {
                debuginfo.Update("\n   meanFPS:" + meanFPS.Framerate.ToString("0000.0"));
                debuginfo.Update("\n    minFPS:" + minFPS.ToString("0000.0"));
                debuginfo.Update("\n    maxFPS:" + maxFPS.ToString("0000.0"));
            }
            debuginfo.Update("\n     timer:" + TimerText(timer));
            debuginfo.Update("\nnextFrameTimer:" + nextFrameTimer);
            if (geralt != null)
            {
                debuginfo.Update("\n     input:" + geralt.input + "  isRunning:" + geralt.isRunning);
                debuginfo.Update("\n prevInput:" + geralt.prevInput);
                debuginfo.Update("\n\nwallJumpTimer:" + geralt.wallJumpTimer);
                debuginfo.Update("\n\n  player info");
                debuginfo.Update("\nHealth:" + geralt.health + "\nShield:" + geralt.shield + "\nEnergy:" + geralt.energy);
                debuginfo.Update("\n  Xpos:" + geralt.position.X + "\n  Ypos:" + geralt.position.Y);
                debuginfo.Update("\n  Xvel:" + geralt.velocity.X + "\n  Yvel:" + geralt.velocity.Y);
                debuginfo.Update("\n   col:" + geralt.Collider);
                debuginfo.Update("\nmaxspeed:" + geralt.debugspeed);
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
            if (framebyframe)
            {
                debuginfo.Update("\nFrame-by-frame mode:" + framebyframe);
            }
            //debuginfo.Update("\n01234567890ABCDEFGHIJKLMNOPQRSTUVWQYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_+><~" + '\u001b' + "{}|.`,:;/@'[\\]\"ᴥ");  //every character in print class
            if (eList.Count > 0)
            { 
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
                    debuginfo.Update("\n  Xpos:" + eList[i].Position.X + "\n  Ypos:" + eList[i].Position.Y);
                    debuginfo.Update("\n  Xvel:" + eList[i].velocity.X + "\n  Yvel:" + eList[i].velocity.Y);
                    if (i > 3)
                    {
                        i = eList.Count;
                    }
                }
            }
            debuginfo.Update("\nCamera: " + camera);
        }

        public static Texture2D[] LoadEnchantIcons()
        {
            List <Texture2D> texlist= new List<Texture2D>();

            texlist.Add(game.Content.Load<Texture2D>("blood enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("fire enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("frost enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("knockback enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("poison enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("sharpness enchant icon"));
            texlist.Add(game.Content.Load<Texture2D>("stun enchant icon"));

            return texlist.ToArray();
        }

        public static string[] LoadEnchantDescriptions()
        {
            string[] returnstrings = new string[7];

            returnstrings[0] = "long duration, low damage";                     //Bleed = 0,          //long duration, low damage                 stacks:                             upgrade: longer duration, more damage/second
            returnstrings[1] = "short duration, high damage";                   //Fire = 1,           //short duration, high damage               stacks:                             upgrade: longer duration, more damage/second
            returnstrings[2] = "long duration, slows";                          //Frost = 2,          //long duration, slows                      stacks:                             upgrade: longer duration
            returnstrings[3] = "knocks enemies back short distance";            //Knockback = 3,      //knocks enemies back short distance        stacks:                             upgrade: knocks back further
            returnstrings[4] = "long duration, high damage, limited use";       //Poison = 4,         //long duration, high damage, limited use   stacks:                             upgrade: more uses
            returnstrings[5] = "flat damage increase (no duration)";            //Sharpness = 5,      //flat damage increase (no duration)        stacks:                             upgrade: more damage
            returnstrings[6] = "short duration, full stun";                     //Stun = 6,           //short duration, full stun                 stacks:                             upgrade: longer duration

            return returnstrings;
        }

        public static float RandomFloat()
        {
            return (float)RAND.NextDouble();
        }

        public static int RandomInt(int maxValue)
        {
            maxValue++;
            int randomInt = RAND.Next(maxValue);
            if (maxValue <= 1) { return randomInt; }
            while (randomInt == lastRandomInt)
            {
                randomInt = RAND.Next(maxValue);
            }
            lastRandomInt = randomInt;
            return randomInt;
        }

        public static void AddPlayerEnchant(EnchantType enchant)
        {
            int hasEnchant = -1;
            for (int i = 0; i < geralt.enchantList.Count; i++)
            {
                if (geralt.enchantList[i].enchantType == enchant)
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
                geralt.enchantList.Add(new Enchant(enchant, 1));
                WriteLine(enchant + " added");
            }
        }

        public static bool IsTouching(Rectangle rect1, Rectangle rect2)
        {
            ////if (debug > 4) { methodLogger.AppendLine("Irbis.IsTouching"); }
            if (rect1.Right >= rect2.Left && rect1.Top <= rect2.Bottom && rect1.Bottom >= rect2.Top && rect1.Left <= rect2.Right)
            {
                return true;
            }
            return false;
        }

        public static bool IsTouching(Rectangle rect1, Rectangle rect2, Side side)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.IsTouching"); }
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
            ////if (debug > 4) { methodLogger.AppendLine("Irbis.DistanceSquared"); }
            int tempX = (p2.X - p1.X);
            int tempY = (p2.Y - p1.Y);
            return (tempX * tempX) + (tempY * tempY);
            //return ((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        private static bool GetKey(Keys key)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.GetKey"); }
            if (keyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        private static bool GetKeyDown(Keys key)
        {
            ////if (debug > 4) { methodLogger.AppendLine("Irbis.GetKeyDown"); }
            if (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        private static bool GetKeyUp(Keys key)
        {
            ////if (debug > 4) { methodLogger.AppendLine("Irbis.GetKeyDown"); }
            if (keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public string TimerText(double timer)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.TimerText"); }
            return ((int)(timer/60)).ToString("00") + ":" + (timer % 60).ToString(timerAccuracy);
        }

        public static void PlayerDeath()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.PlayerDeath"); }
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

            WriteLine("PlayerDeath");
            savefile.Save(autosave);
        }

        public void SummonGenericEnemy()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.SummonGenericEnemy"); }
            if (enemySpawnPoints.Count > 0)
            {
                int spawnpoint = (int)(RAND.NextDouble() * enemySpawnPoints.Count);
                Enemy tempEnemy = new Enemy(enemy0Tex, enemySpawnPoints[spawnpoint], 100f, 10f, 200f, 0.4f);
                eList.Add(tempEnemy);
                enemyList.Add(tempEnemy);
                //collisionObjects.Add(tempEnemy);
                WriteLine("enemy spawned at " + enemySpawnPoints[spawnpoint] + ". health:100 damage:10 speed:300. timer:" + Timer);
            }
            else
            {
                Console.WriteLine("Error, no spawn points");
            }
        }

        public void SummonGenericEnemy(float health, float damage, float speed)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.SummonGenericEnemy"); }
            if (enemySpawnPoints.Count > 0)
            {
                int spawnpoint = (int)(RAND.NextDouble() * enemySpawnPoints.Count);
                Enemy tempEnemy = new Enemy(enemy0Tex, enemySpawnPoints[spawnpoint], health, damage, speed, 0.4f);
                eList.Add(tempEnemy);
                enemyList.Add(tempEnemy);
                //collisionObjects.Add(tempEnemy);
                WriteLine("enemy spawned at " + enemySpawnPoints[spawnpoint] + ". health:" + health + " damage:" + damage + " speed:" + speed + ". timer:" + Timer);
            }
            else
            {
                WriteLine("Error, no spawn points");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void KillEnemy(Enemy killMe)
        {
            WriteLine("removing enemy. index:" + eList.IndexOf(killMe));
            //collisionObjects.Remove(eList[i]);
            enemyList.Remove(killMe);
            eList.Remove(killMe);
            if (onslaughtMode) { onslaughtSpawner.EnemyKilled(); }
            WriteLine("success.");
        }

        public PlayerSettings Load(string filename)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Load"); }
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
            useKey = playerSettings.useKey;
            altUseKey = playerSettings.altUseKey;

            cameraLerp = playerSettings.cameraLerp;
            cameraLerpSpeed = playerSettings.cameraLerpSpeed;
        
            //debug = playerSettings.debug;
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
                tempResolution = playerSettings.resolution;

                if (tempResolution != Point.Zero)
                {
                    SetResolution(tempResolution);
                }
                else
                {
                    tempResolution = new Point(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
                    SetResolution(tempResolution);
                }
                graphics.IsFullScreen = playerSettings.fullscreen;
            }

            SetScreenScale(playerSettings.screenScale);

            if ((int)(screenScale / 2) == (screenScale / 2))
            {
                textScale = (int)(screenScale / 2);
            }
            else
            {
                textScale = (int)(screenScale / 2) + 1;
            }

            projection.M11 = 4f / (resolution.X);
            projection.M22 = 4f / (resolution.Y);

            //halfPixelOffset.M41 = -(resolution.X / (screenScale * 2));
            //halfPixelOffset.M42 = (resolution.Y / (screenScale * 2));
            //halfPixelOffset.M44 = (resolution.X / (screenScale * 2));
            //halfPixelOffset.M22 = (resolution.X / (float)resolution.Y);

            graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = playerSettings.vSync;
            graphics.ApplyChanges();

            boundingBox = playerSettings.boundingBox;
            if (boundingBox == Rectangle.Empty)
            {
                boundingBox = new Rectangle((resolution.ToVector2() * 0.3f).ToPoint(), (resolution.ToVector2() * 0.4f).ToPoint());
            }

            if (geralt != null)
            {
                geralt.Load(playerSettings);
            }

            return playerSettings;
        }

        public static void SetResolution(Point newResolution)
        {
            tempResolution = resolution = newResolution;
            halfResolution.X = resolution.X / 2;
            halfResolution.Y = resolution.Y / 2;
            consoleRect = new Rectangle(0, -halfResolution.Y, resolution.X, halfResolution.Y);
            zeroScreenspace = new Rectangle(Point.Zero, resolution);
            if (consoleWriteline != null)
            {
                developerConsole.Update(resolution.X);
                consoleWriteline.Update(resolution.X);
            }
            graphics.PreferredBackBufferHeight = resolution.Y;
            graphics.PreferredBackBufferWidth = resolution.X;
            game.GraphicsDevice.SetRenderTarget(null);
            //graphics.ApplyChanges();
        }

        public static bool Use()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Use"); }
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Lerp"); }
            if (amount > 1)
            {
                return value2;
            }
            return value1 + (value2 - value1) * amount;
        }

        public static bool IsDefaultLevelFormat(string level)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.IsDefaultLevelFormat"); }
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.GetLevelChapterAndMap"); }
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.OpenConsole"); }
            textInputBuffer = string.Empty;
            acceptTextInput = console = !console;
            if (geralt != null)
            {
                geralt.inputEnabled = !console;
                //framebyframe = console;
            }
            consoleLine = developerConsole.lines + 1;
            consoleMoveTimer = 1 - consoleMoveTimer;
        }

        public void MoveConsole()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.MoveConsole"); }
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
            consoleWriteline.Update(new Point(1, consoleRect.Bottom - (int)(10 * screenScale)));
            developerConsole.Update(new Point(1, consoleRect.Bottom - (int)(20 * screenScale)));
        }

        public void UpdateConsole()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.UpdateConsole"); }
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


            if ((keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter)))
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.ExportConsole"); }
            if (meanFPS != null)
            {
                Irbis.WriteLine("meanFPS: " + meanFPS.Framerate);
                Irbis.WriteLine(" minFPS: " + minFPS);
                Irbis.WriteLine("at time: " + minFPStime);
                Irbis.WriteLine(" maxFPS: " + maxFPS);
                Irbis.WriteLine("at time: " + maxFPStime);
                Irbis.WriteLine(" median: " + ((minFPS + maxFPS) / 2));
            }

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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.CleanConsole"); }
            while (developerConsole.lines > 10000)
            {
                developerConsole.Clear();
            }
        }

        public void ExportString(string stringtoexport)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.ExportString"); }
            string timenow = (DateTime.Now).ToShortDateString() + "." + (DateTime.Now).ToString("HH:mm:ss.fff");
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

            File.WriteAllText(nameoffile, stringtoexport);
        }

        public void Debug(int rank)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Debug"); }
            debug = rank;
            if (debug > 0)
            {
                this.IsMouseVisible = recordFPS = true;
                meanFPS = new TotalMeanFramerate(true);
                maxFPS = double.MinValue;
                minFPS = double.MaxValue;
            }
            else
            {
                this.IsMouseVisible = recordFPS = false;
                debuginfo.Clear();
                foreach (Square s in squareList)
                {
                    s.color = Color.White;
                }
            }
        }

        public static void WriteLine(string line)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.WriteLine"); }
            developerConsole.WriteLine(line);
            consoleLine = developerConsole.lines + 1;
        }

        public static void WriteLine()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.WriteLine"); }
            developerConsole.WriteLine();
            consoleLine = developerConsole.lines + 1;
        }

        public static void Write(string line)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Write"); }
            developerConsole.Write(line);
        }

        public static void SetScreenScale(float newScale)
        {
            if (newScale > 0)
            {
                screenScale = newScale;
            }
            else if (screenScale <= 0)
            {
                screenScale = resolution.X / 480f;      //this decides the screen scale on load         960/480
            }
            //change the location of everything
            if (enemyHealthBar != null)
            {
                enemyHealthBar = new UIElementSlider(Direction.right, new Point((int)((resolution.X / screenScale) - (32 / screenScale)), (int)(32 / screenScale)), 250, 10, 100, Color.Red, new Color(166, 030, 030), nullTex, nullTex, nullTex, font, false, 0.5f);
            }
            if (healthBar != null)
            {
                healthBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale)), 125, 10, geralt.maxHealth, Color.Red, new Color(166, 030, 030), nullTex, nullTex, nullTex, font, false, 0.505f, 0.55f, 0.5f);
            }
            if (shieldBar != null)
            {
                shieldBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale) + 10), 75, 10, geralt.maxShield, Color.Red, new Color(255, 170, 000), nullTex, nullTex, shieldBar.overlayTex, font, false, 0.5f);
            }
            if (energyBar != null)
            {
                energyBar = new UIElementSlider(Direction.left, new Point((int)(32 / screenScale), (int)(32 / screenScale) + 20), 50, 10, geralt.maxEnergy, Color.Red, new Color(000, 234, 255), nullTex, nullTex, nullTex, font, false, 0.5f);
            }
            if (potionBar != null)
            {
                potionBar = new UIElementDiscreteSlider(Direction.left, new Rectangle((int)(32 / screenScale) + 76, (int)(32 / screenScale) + 11, 48, 8), nullTex, nullTex, nullTex, Color.DarkSlateGray, Color.DarkRed, Color.DarkSlateBlue, geralt.maxNumberOfPotions, 3, 0.5f);
            }
            if ((int)(screenScale / 2) == (screenScale / 2))
            {
                textScale = (int)(screenScale / 2);
            }
            else
            {
                textScale = (int)(screenScale / 2) + 1;
            }
        }

        public string Credits()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Credits"); }
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
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Help"); }
            string help =
@"enter commands in the form: command=value or command=(optional value)
List of commands:

debug=(rank)  -------  enables debugmode         leveleditor  ------------  enter level editor
version  ------------  print current version      savelevel=(name)  ------  save the current level
summonenemy=(hp)  ---  summons one enemy          newlevel=(name)  -------  creates empty level
summonenemies=(X)  --  summon -X- enemies         spawnblock=(texture)  --  spawns block at screen center
notarget  -----------  disables all AI            squarelist  ------------  lists ALL squares
killall  ------------  kill all enemies           spawnpoints  -----------  list enemy spawn points
skiptowave=wave  ----  if onslaught, skip to wave  removespawn=index  ----  remove spawn point
loadlevel=name  -----  if exists, load level       removespawns=startIndex,endIndex
killme  -------------  kills the player            removeallspawns  ------  deletes all spawns
printsave  ----------  print autosave info         addspawn=X,Y  ---------  adds spawn at position X, Y
credits  ------------  display credits           Enchants  ---------------  list active enchants
export  -------------  export log to file         addenchant=Type  -------  add enchant to active enchants
help  ---------------  displays this msg           (bleed, fire, frost, knockback, poison, sharpness, stun)
move=X,Y  -----------  move player X,Y pixels      add the same enchant multiple times to upgrade its strength
exit  ---------------  quits                      disenchant  ------------  remove all enchants
quit  ---------------  exits
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

        public string Invocation()
        {
            string invocation =
@"Don't call it a comb-back; I'll have hair for years.
I’M SCARED.
I’m scared that my abilities are gone.
I’m scared that I’m going to fuck this up.
And I’m scared of you.
I don't want to start, but I will.
This is an invocation for anyone who hasn't begun, who's stuck in a terrible place between zero and one.
Let me realize that my past failures at follow-through are no indication of my future performance. They're
just healthy little fires that are going to warm up my ass.
If my F.I.L.D.I. is strong, let me keep him in a velvet box until I really, really need him. If my F.I.L.D.I.
is weak, let me feed him oranges and not let him gorge himself on ego and arrogance.
Let me not hit up my Facebook like it's a crack pipe, keep the browser closed.
If I catch myself wearing a too-too (too fat, too late, too old) let me shake it off like a donkey would
shake off something it doesn't like.
And when I get that feeling in my stomach, you know the feeling when all of a sudden you get a ball of energy
and it shoots down into your legs and up into your arms and tells you to get up and stand up and go to the
refrigerator and get a cheese sandwich? That's my cheese monster talking. And my cheese monster will never be
satisfied by cheddar, only the cheese of accomplishment.
Let me think about the people who I care about the most, and how when they fail or disappoint me... I still
love them, I still give them chances, and I still see the best in them. Let me extend that generosity to myself.
Let me find and use metaphors to help me understand the world around me and give me the strength to get rid of
them when it's apparent they no longer work.
Let me thank the parts of me that I don't understand or are outside of my rational control like my creativity
and my courage. And let me remember that my courage is a wild dog. It won't just come when I call it, I have to
chase it down and hold on as tight as I can.
Let me not be so vain to think that I'm the sole author of my victories and a victim of my defeats.
Let me remember that the unintended meaning that people project onto what I do is neither my fault or something
I can take credit for.
Perfectionism may look good in his shiny shoes but he's a little bit of an asshole and no one invites him to
their pool parties.
Let me remember that the impact of criticism is often not the intent of the critic, but when the intent is evil,
that's what the block button's for.
And when I eat my critique, let me be able to separate out the good advice from the bitter herbs.
(There are few people who won't be disarmed by a genuine smile.)
(A big impact on a few can be worth more than a small impact.)
Let me not think of my work only as a stepping stone to something else, and if it is, let me become fascinated
with the shape of the stone.
Let me take the idea that has gotten me this far and put it to bed. What I am about to do will not be that, but
it will be something.
There is no need to sharpen my pencils anymore, my pencils are sharp enough.
Even the dull ones will make a mark.
Warts and all, let's start this shit up.
And god let me enjoy this. Life isn't just a sequence of waiting for things to be done.

Thank you, Ze Frank, for the inspiration.";

            string returninvocation = string.Empty;
            foreach (char c in invocation)
            {
                if (!c.Equals('\u000D'))
                {
                    returninvocation += c;
                }
            }

            return returninvocation;
        }

        public void Quit()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Quit"); }
            if (debug > 0)
            {
                ExportConsole();
            }
            Exit();
        }

        public void PrintVersion()
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.PrintVersion"); }
            WriteLine("    Project: Irbis (" + versionTy + ")\n    " + versionID + " v" + versionNo);
        }

        private static Point PointParser(string value)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.PointParser"); }
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

        private static string MatrixPrinter(Matrix matrix)
        {
            string matrixString = string.Empty;
            matrixString = 
                ("\n{M11:" + projection.M11 + " M12:" + projection.M12 + " M13:" + projection.M13 + " M14:" + projection.M14 + "} " + " {" + projection.M11 + ":" + projection.M12 + ":" + projection.M13 + ":" + projection.M14 + "}"
                + "\n{M21:" + projection.M21 + " M22:" + projection.M22 + " M23:" + projection.M23 + " M24:" + projection.M24 + "} " + " {" + projection.M21 + ":" + projection.M22 + ":" + projection.M23 + ":" + projection.M24 + "}"
                + "\n{M31:" + projection.M31 + " M32:" + projection.M32 + " M33:" + projection.M33 + " M34:" + projection.M34 + "} " + " {" + projection.M31 + ":" + projection.M32 + ":" + projection.M33 + ":" + projection.M34 + "}"
                + "\n{M41:" + projection.M41 + " M42:" + projection.M42 + " M43:" + projection.M43 + " M44:" + projection.M44 + "} " + " {" + projection.M41 + ":" + projection.M42 + ":" + projection.M43 + ":" + projection.M44 + "}"
                );
            return matrixString;
        }

        public void ConsoleParser(string line)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.ConsoleParser"); }
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
                        if (float.TryParse(value, out floatResult))
                        {
                            SetScreenScale(floatResult);
                            WriteLine("screenscale:" + screenScale);
                        }
                        else
                        {
                            WriteLine("this command changes the screenscale\ncurrent screenscale:" + screenScale);
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
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            SummonGenericEnemy();
                        }
                        else
                        {
                            if (int.TryParse(value, out intResult))
                            {
                                SummonGenericEnemy(100f * (intResult / 100f), 10f, 300f);
                            }
                            else
                            {
                                WriteLine("error: variable \"" + variable + "\" could not be parsed");
                            }
                        }
                        //WriteLine("debug: " + debug);
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
                    case "noclip":
                        geralt.Noclip();
                        break;
                    case "killall":
                        eList.Clear();
                        enemyList.Clear();
                        break;
                    case "savelevel":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("saving level as " + currentLevel);
                            SaveLevel(currentLevel);
                        }
                        else
                        {
                            WriteLine("saving level as " + value);
                            SaveLevel(value);
                            LoadLevel(value, true);
                        }
                        WriteLine("done.");
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
                    case "loadlevel":
                        if (File.Exists(".\\levels\\" + value + ".lvl"))
                        {
                            WriteLine("loading level: " + value);
                            LoadLevel(value, !levelEditor);
                            if (levelEditor) { sceneIsMenu = true; }
                            WriteLine("done.");
                        }
                        else
                        {
                            //???
                        }
                        break;
                    case "newlevel":
                        if (levelEditor && !string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("creating new level: " + value);
                            Level thisLevel = new Level(true);
                            SaveLevel(value);
                            LoadLevel(value, true);
                        }
                        break;
                    case "leveleditor":
                        EnableLevelEditor();
                        WriteLine("levelEditor: " + levelEditor);
                        break;
                    case "clearui":
                        ClearUI();
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
                    case "addspawn":
                        if (PointParser(value) != Point.Zero)
                        {
                            Point tempPoint = PointParser(value);
                            enemySpawnPoints.Add(tempPoint.ToVector2());
                            WriteLine("added spawn point at " + tempPoint);
                        }
                        break;
                    case "removespawn":
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
                    case "removespawns":
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
                    case "removeallspawns":
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
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("addenchant=Type  -------  add enchant to active enchants");
                            WriteLine("(bleed, fire, frost, knockback, poison, sharpness, stun)");
                            WriteLine("add the same enchant multiple times to upgrade its strength");
                        }
                        else
                        {
                            if (Enum.TryParse(value, out enchantResult) && geralt != null)
                            {
                                AddPlayerEnchant(enchantResult);
                            }
                            else
                            {
                                WriteLine("error: enchant \"" + value + "\" could not be parsed");
                            }
                        }
                        break;
                    case "purchaseenchant":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("purchase enchant=Type  -------  add enchant to active enchants");
                            WriteLine("(bleed, fire, frost, knockback, poison, sharpness, stun)");
                            WriteLine("purchase the same enchant multiple times to upgrade its strength");
                        }
                        else
                        {
                            if (Enum.TryParse(value, out enchantResult) && geralt != null)
                            {
                                int vendingmachine = -1;
                                for (int i = 0; i < onslaughtSpawner.vendingMachineList.Count; i++)
                                {
                                    if (onslaughtSpawner.vendingMachineList[i].type == VendingType.Enchant)
                                    {
                                        vendingmachine = i;
                                    }
                                }
                                if (vendingmachine >= 0)
                                {
                                    onslaughtSpawner.vendingMachineList[vendingmachine].Purchase((int)enchantResult);
                                    WriteLine("purchased " + enchantResult);
                                }
                                else
                                {
                                    WriteLine("vendingmachine=" + vendingmachine);
                                    WriteLine("failed");
                                }
                            }
                            else
                            {
                                WriteLine("error: enchant \"" + value + "\" could not be parsed");
                            }
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
                        PlayerDeath();
                        break;
                    case "printsave":
                        savefile.Print(autosave);
                        break;
                    case "unstuck":
                        geralt.ClearCollision();
                        break;
                    case "fps":
                        WriteLine("smart fps: " + smartFPS.Framerate + ", raw fps: " + (1 / DeltaTime));
                        if (recordFPS)
                        {
                            WriteLine("meanfps:" + meanFPS.Framerate);
                            WriteLine(" maxfps:" + maxFPS + " at time:" + maxFPStime);
                            WriteLine(" minfps:" + minFPS + " at time:" + minFPStime);
                        }
                        WriteLine();
                        break;
                    case "framerate":
                        goto case "fps";
                    case "timer":
                        WriteLine("timer: " + Timer);
                        WriteLine();
                        break;
                    case "debuglevel":
                        if (true)
                        {
                            Level thislevel = new Level();
                            thislevel.Load(".\\levels\\" + currentLevel + ".lvl");
                            WriteLine(thislevel.ToString());
                        }
                        WriteLine();
                        break;
                    case "moveme":
                        goto case "move";
                    case "move":
                        if (PointParser(value) != Point.Zero)
                        {
                            Point tempPoint = PointParser(value);
                            geralt.position.X += tempPoint.X;
                            geralt.position.Y += tempPoint.X;

                            WriteLine("moved player to " + geralt.position);
                        }
                        break;
                    case "random":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("random int between 0 and 100: " + RandomInt(100));
                        }
                        else
                        {
                            if (PointParser(value) != Point.Zero)
                            {
                                Point tempPoint = PointParser(value);
                                int currentInt;
                                float mean = 0;
                                float median = 0;
                                int min = int.MaxValue;
                                int max = int.MinValue;
                                WriteLine(tempPoint.Y + " random ints: ");
                                WriteLine();
                                for (int i = 0; i < tempPoint.Y; i++)
                                {
                                    currentInt = RandomInt(tempPoint.X);
                                    mean += currentInt;
                                    if (currentInt > max) { max = currentInt; }
                                    if (currentInt < min) { min = currentInt; }

                                    Write(currentInt + " ");
                                }
                                median = ((min + max) / 2f);
                                mean = mean / tempPoint.Y;
                                WriteLine("   min: " + min);
                                WriteLine("   max: " + max);
                                WriteLine("  mean: " + mean);
                                WriteLine("median: " + median);
                                WriteLine();
                            }
                            else
                            {
                                if (int.TryParse(value, out intResult))
                                {
                                    WriteLine("random int between 0 and " + intResult + ": " + RandomInt(intResult));
                                }
                                else
                                {
                                    WriteLine("random int between 0 and 100: " + RandomInt(100));
                                }
                            }
                        }
                        break;
                    case "spawnvending":
                        if (true)
                        {
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                WriteLine("spawning enchant vending machine at " + new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))));
                                VendingMachine tempvend = new VendingMachine(200, VendingType.Enchant, new Rectangle(new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))), new Point(64, 64)), Content.Load<Texture2D>("enchant vending machine"), 0.35f);
                                onslaughtSpawner.vendingMachineList.Add(tempvend);

                            }
                            else
                            {
                                VendingType vendtype;
                                if (Enum.TryParse(value, out vendtype))
                                {
                                    WriteLine("spawning " + value + " vending machine at " + new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))));
                                    VendingMachine tempvend = new VendingMachine(200, vendtype, new Rectangle(new Point((int)(camera.X - (camera.X % 32)), (int)(camera.Y - (camera.Y % 32))), new Point(64, 64)), Content.Load<Texture2D>("enchant vending machine"), 0.35f);
                                    onslaughtSpawner.vendingMachineList.Add(tempvend);
                                }
                                else
                                {
                                    WriteLine("error: vending machine \"" + value + "\" could not be parsed");
                                }
                            }
                        }
                        break;
                    case "spawnvendingmachine":
                        goto case "spawnvending";
                    case "debugvending":
                        goto case "debugonslaught";
                    case "debugvendingmachine":
                        goto case "debugvending";
                    case "invocation":
                        WriteLine(Invocation());
                        break;
                    case "addtotree":
                        if (float.TryParse(value, out floatResult))
                        {
                            testTree.Add(floatResult);
                            WriteLine("added " + floatResult);
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "filltree":
                        if (int.TryParse(value, out intResult))
                        {
                            WriteLine("filling...");
                            for (int i = 0; i < intResult; i++)
                            {
                                float randomfloat = RandomFloat() * 100f;
                                Write(randomfloat + " ");
                                testTree.Add(randomfloat);
                            }
                            WriteLine();
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "printtree":
                        WriteLine();
                        foreach (float f in testTree)
                        {
                            Write(f + " ");
                        }
                        WriteLine();
                        break;
                    case "printtreeleftmost":
                        WriteLine("" + testTree.GetLeftmost(testTree));
                        WriteLine();
                        break;
                    case "debugtree":
                        if (testTree != null)
                        {
                            WriteLine(" root: " + testTree);
                            if (testTree.Left == null)
                            {
                                WriteLine(" left: null");
                            }
                            else
                            {
                                WriteLine(" left: " + testTree.Left);
                            }
                            if (testTree.Right != null)
                            {
                                WriteLine("right: " + testTree.Right);
                            }
                            else
                            {
                                WriteLine("right: null");
                            }
                        }
                        else
                        {
                            WriteLine(" root: null");
                        }
                        break;
                    case "vendings":
                        for (int i = 0; i < onslaughtSpawner.vendingMachineList.Count; i++)
                        {
                            WriteLine("VendingMachine[" + i + "]: " + onslaughtSpawner.vendingMachineList[i]);
                        }
                        WriteLine();
                        break;
                    case "vendingmachines":
                        goto case "vendings";
                    case "removevending":
                        if (int.TryParse(value, out intResult))
                        {
                            onslaughtSpawner.vendingMachineList.RemoveAt(intResult);
                            WriteLine("removed vending machine [" + intResult + "]");
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        WriteLine();
                        break;
                    case "removevendingmachine":
                        goto case "removevending";
                    case "player":
                        WriteLine(geralt.ToString());
                        WriteLine();
                        break;
                    case "debugonslaught":
                        if (onslaughtSpawner != null)
                        {
                            WriteLine(onslaughtSpawner.ToString());
                        }
                        WriteLine();
                        break;
                    case "recordfps":
                        meanFPS = new TotalMeanFramerate(true);
                        maxFPS = double.MinValue;
                        minFPS = double.MaxValue;
                        recordFPS = true;
                        break;
                    case "recordframerate":
                        goto case "recordfps";
                    case "god":
                        geralt.invulnerableTime = float.MaxValue;
                        geralt.invulnerable = true;
                        WriteLine("godmode on");
                        break;
                    case "addpoints":
                        if (int.TryParse(value, out intResult))
                        {
                            if (onslaughtSpawner != null)
                            {
                                onslaughtSpawner.Points += intResult;
                                WriteLine("added " + intResult + " points");
                            }
                        }
                        else
                        {
                            WriteLine("use this command to add points to your score");
                        }
                        break;
                    case "resolution":
                        WriteLine("resolution:" + resolution);
                        WriteLine();
                        break;
                    case "projection":
                        WriteLine("basicEffect.Projection:" + MatrixPrinter(projection));
                        WriteLine();
                        break;
                    case "debuglines":
                        WriteLine("lines: " + debuglines.Length);
                        for (int i = 0; i < debuglines.Length; i++)
                        {
                            WriteLine("line[" + i + "]:" + debuglines[i].ToString());
                        }
                        WriteLine();
                        break;
                    case "debugrays":
                        WriteLine("rays: " + debugrays.Length);
                        for (int i = 0; i < debugrays.Length; i++)
                        {
                            WriteLine("ray[" + i + "]:" + debugrays[i].ToString());
                        }
                        WriteLine();
                        break;
                    case "debugshapes":
                        WriteLine("shapes: " + debugshapes.Length);
                        for (int i = 0; i < debugshapes.Length; i++)
                        {
                            WriteLine("shape[" + i + "]:\n" + debugshapes[i].Debug(true));
                            WriteLine();
                        }
                        break;
                    case "multithreading":
                        useMultithreading = !useMultithreading;
                        WriteLine("multithreading:" + useMultithreading);
                        WriteLine();
                        break;
                    case "multithread":
                        goto case "multithreading";
                    case "forcetriangulate":
                        for (int i = 0; i < debugshapes.Length; i++)
                        {
                            WriteLine("triangulating debugshapes[" + i + "]...");
                            if (debugshapes[i].Triangulate())
                            { Write(" done."); }
                            else
                            { Write(" failed."); }
                        }
                        WriteLine();
                        break;
                    case "shadowshape":
                        WriteLine(shadowShape.ToString());
                        WriteLine();
                        break;






                    case "debug":                                                                     //place new bools above
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            if (debug > 0)
                            {
                                Debug(0);
                            }
                            else
                            {
                                Debug(3);
                            }
                        }
                        else
                        {
                            if (int.TryParse(value, out intResult))
                            {
                                Debug(intResult);
                            }
                            else
                            {
                                WriteLine("error: rank \"" + value + "\" could not be parsed");
                            }
                        }
                        WriteLine("debug: " + debug);
                        break;
                    case "exit":
                        Quit();
                        break;
                    case "quit":
                        Quit();
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
                        //if (string.IsNullOrWhiteSpace(extra))
                        //{
                        //    WriteLine("statement: " + statement);
                        //    WriteLine("  command: " + variable);
                        //    WriteLine("    value: " + value);
                        //}
                        //else
                        //{
                        //    WriteLine("statement: " + statement);
                        //    WriteLine("  command: " + variable);
                        //    WriteLine("    extra: " + extra);
                        //    WriteLine("    value: " + value);
                        //}

                        if (string.IsNullOrWhiteSpace(extra))
                        {
                            WriteLine("error: no command with name: \"" + variable + "\"");
                        }
                        else
                        {
                            WriteLine("error: no command with name: \"" + variable + "[" + extra + "]\"");
                        }
                        break;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //if (debug > 4) { methodLogger.AppendLine("Irbis.Draw"); }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
            
            //spritebatch for BACKGROUNDS
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, background);
            foreach (Square b in backgroundSquareList)
            {
                b.Draw(spriteBatch);
            }
            spriteBatch.End();

            //standard spritebatch, draw level and player and enemies here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, foreground);
            foreach (Square s in squareList)
            {
                s.Draw(spriteBatch);
            }
            foreach (Enemy e in eList)
            {
                if (e != null)
                {
                    e.Draw(spriteBatch);
                }
            }
            if (geralt != null) { geralt.Draw(spriteBatch); }
            if (debug > 1)
            {
                RectangleBorder.Draw(spriteBatch, new Rectangle(Point.Zero, screenspace.Size), Color.Magenta, false);
                if (levelEditor)
                {
                    Texture2D squareTex = Content.Load<Texture2D>("originTexture");
                    foreach (Vector2 p in enemySpawnPoints)
                    {
                        Square tempSquare = new Square(squareTex, p.ToPoint(), true, 0.9f);
                        tempSquare.Draw(spriteBatch);
                    }
                    Square tempSquare2 = new Square(squareTex, worldSpaceMouseLocation, true, 0.9f);
                    tempSquare2.Draw(spriteBatch);

                }
            }
            if (onslaughtSpawner != null)
            {
                foreach (VendingMachine v in onslaughtSpawner.vendingMachineList)
                {
                    v.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            if (debug > 4)
            {
                foreach (EffectPass effectPass in basicEffect.CurrentTechnique.Passes)
                {
                    effectPass.Apply();
                    shadowShape.Draw();
                    //foreach (Shape s in debugshapes)
                    //{
                    //    s.Draw();
                    //    //s.DrawLines();
                    //}
                    //foreach (Ray r in debugrays)
                    //{
                    //    r.Draw(r.Intersect(debugshapes));
                    //}
                }
            }

            //spritebatch for static elements (like UI, etc)
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
            if (vendingMachineMenu != null)
            {
                vendingMachineMenu.Draw(spriteBatch);
            }
            else
            {
                if (healthBar != null) { healthBar.Draw(spriteBatch); }
                if (shieldBar != null) { shieldBar.Draw(spriteBatch); }
                if (energyBar != null) { energyBar.Draw(spriteBatch); }
                if (potionBar != null) { potionBar.Draw(spriteBatch); }
                if (displayEnemyHealth) { enemyHealthBar.Draw(spriteBatch); }
            }
            if (timerDisplay != null) { timerDisplay.Draw(spriteBatch); }
            if (onslaughtDisplay != null)
            {
                onslaughtDisplay.Draw(spriteBatch);
            }
            if (!sceneIsMenu)
            {
                debuginfo.Draw(spriteBatch);
            }

            if (debug > 1)
            {
                RectangleBorder.Draw(spriteBatch, boundingBox, Color.Magenta, false);
            }
            

            if ((console || consoleMoveTimer > 0) && !sceneIsMenu)
            {
                spriteBatch.Draw(consoleTex, consoleRect, consoleTex.Bounds, consoleRectColor, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                consoleWriteline.Draw(spriteBatch);
                developerConsole.Draw(spriteBatch);
            }
            spriteBatch.End();
            // --------------------------------------------------------------------------------------------------------------------------------

            if (sceneIsMenu)        //FOR MENU DRAWING
            {
                // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
                if (!levelEditor)
                {
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                    spriteBatch.Draw(menuTex[scene], Vector2.Zero, null, Color.White, 0f, Vector2.Zero, (70f / menuTex[scene].Height) * screenScale, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                if (console || consoleMoveTimer > 0)
                {
                    spriteBatch.Draw(consoleTex, consoleRect, consoleTex.Bounds, consoleRectColor, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                    consoleWriteline.Draw(spriteBatch);
                    developerConsole.Draw(spriteBatch);
                }
                debuginfo.Draw(spriteBatch);

                //if (levelLoaded > 0 && !levelEditor)
                //{
                //    //darken bg screen
                //    spriteBatch.Draw(nullTex, zeroScreenspace, null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                //}

                if (scene == 3 && menuSelection >= 0 && menuSelection <= 3)
                {
                    RectangleBorder.Draw(spriteBatch, boundingBox, Color.Magenta, false);
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

            base.Draw(gameTime);
        }
    }
}
