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
    }

    public interface ICollisionObject
    {
        Rectangle collider
        {
            get;
            set;
        }
    }

    public class Irbis : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D renderTarget;

        public bool debug;
        string versionNo;
        string versionID;

        string filename;
        string savefile;

        public KeyboardState keyboardState;
        public KeyboardState previousKeyboardState;

        public MouseState mouseState;
        public MouseState previousMouseState;

        public Vector2 initialPos;
        //Vector2 eInitialPos;

        Square backgroundSquare;
        List<Square> backgroundSquareList;
        public List<ICollisionObject> collisionObjects;
        public List<Square> sList;
        public List<Square> squareList;
        List<Button> buttonList;
        public List<Enemy> eList;
        public List<Print> printList;
        public List<UIElementSlider> sliderList;

        int scene;
        bool sceneIsMenu;

        Texture2D fontTex;
        //Texture2D fontTex2;
        public Font font;

        public bool cameraLerp;
        public float cameraLerpSpeed;
        public bool cameraShakeSetting;
        float cameraShakeDuration;
        float cameraShakeMagnitude;
        //float cameraTimePerShake;

        float cameraSwingDuration;
        float cameraSwingMaxDuration;
        float cameraSwingMagnitude;
        public float swingDuration;
        public float swingMagnitude;
        Vector2 cameraSwingHeading;
        public bool cameraSwingSetting;
        bool cameraSwing;


        int smoothFPSframecounter;
        double[] smoothFPS;

        double timer;
        //bool timerCount;
        public string timerAccuracy;

        public float minSqrDetectDistance;
        bool displayEnemyHealth;

        bool framebyframe;
        bool nextframe;
        //bool pressed;

        int menuSelection;

        public static float gravity;
        public Random RAND;
        public Print debuginfo;
        public Print developerConsole;
        public Print developerConsole1;
        public Print timerDisplay;

        public Texture2D nullTex;
        Texture2D enemy0Tex;
        Texture2D[] menuTex;

        public Player geralt;

        public UIElementSlider healthBar;
        public UIElementSlider shieldBar;
        public UIElementSlider energyBar;
        public UIElementDiscreteSlider potionBar;
        public UIElementSlider enemyHealthBar;

        Vector2 camera;
        Vector2 mainCamera;
        Vector2 screenSpacePlayerPos;

        Matrix background;
        Matrix foreground;
        Matrix UIground;

        public Rectangle boundingBox;

        RectangleBorder screenspace;
        RectangleBorder boundingBoxBorder;

        //public Boss RAWR;                                                                                                                                             //don't mind this

        EventHandler<TextInputEventArgs> onTextEntered;
        bool acceptTextInput;
        string textInputBuffer;

        //public PlayerSettings playerSettings;
        bool listenForNewKeybind;
        bool resetRequired;
        int levelLoaded;

        public bool AIenabled;

        public int screenScale;
        public Point resolution;
        public Point tempResolution;

        public float masterAudioLevel;
        public float musicLevel;
        public float soundEffectsLevel;

        public float randomTimer;
        int sliderPressed;

        public Keys attackKey;
        public Keys altAttackKey;

        public Keys shockwaveKey;
        public Keys altShockwaveKey;

        public Keys shieldKey;
        public Keys altShieldKey;

        public Keys jumpKey;
        public Keys altJumpKey;

        public Keys upKey;
        public Keys altUpKey;

        public Keys downKey;
        public Keys altDownKey;

        public Keys leftKey;
        public Keys altLeftKey;

        public Keys rightKey;
        public Keys altRightKey;

        public Keys potionKey;
        public Keys altPotionKey;

        public Keys rollKey;
        public Keys altRollKey;

        public Irbis()
        {
            versionNo = "0.0.9.0";
            versionID = "pre-alpha?";
            //debug = false;
            Console.WriteLine("    Project: Irbis (debug)\n    " + versionID + " v" + versionNo);

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
            buttonList = new List<Button>();
            eList = new List<Enemy>();
            printList = new List<Print>();
            sliderList = new List<UIElementSlider>();

            AIenabled = true;
            framebyframe = false;
            nextframe = true;
            //pressed = false;
            randomTimer = 0f;
            base.Initialize();
            smoothFPS = new double[600];
            smoothFPSframecounter = 1;

            levelLoaded = 0;

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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your Content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            filename = ".\\content\\playerSettings.ini";
            savefile = ".\\content\\autosave.snep";

            PlayerSettings playerSettings;
            if (File.Exists(filename))
            {
                playerSettings = Load(filename);
            }
            else
            {
                Console.WriteLine("creating new playerSettings.ini...");
                playerSettings = new PlayerSettings(true);
                PlayerSettings.Save(playerSettings, filename);
                playerSettings = Load(filename);
            }

            SaveFile autosave = new SaveFile();
            if (File.Exists(savefile))
            {
                autosave.Load(savefile);
            }
            else
            {
                Console.WriteLine("creating new autosave.snep...");
                autosave = new SaveFile();
                autosave.Save(savefile);
            }


            Texture2D playerTex = Content.Load<Texture2D>("player");
            Texture2D shieldTex = Content.Load<Texture2D>("shield");

            geralt = new Player(playerTex, shieldTex, playerSettings, this);
            geralt.Respawn(new Vector2(-1000f, -1000f));

            spriteBatch = new SpriteBatch(GraphicsDevice);

            menuTex = new Texture2D[7];
            menuTex[0] = Content.Load<Texture2D>("Main Menu title");
            menuTex[1] = Content.Load<Texture2D>("Options title");
            menuTex[2] = Content.Load<Texture2D>("Keybinds title");
            menuTex[3] = Content.Load<Texture2D>("Camera title");
            menuTex[4] = Content.Load<Texture2D>("Video title");
            menuTex[5] = Content.Load<Texture2D>("Audio title");
            menuTex[6] = Content.Load<Texture2D>("Misc title");

            nullTex = Content.Load<Texture2D>("nullTex");

            fontTex = Content.Load<Texture2D>("font");
            Texture2D fontTex2 = Content.Load<Texture2D>("font2");

            font = new Font(fontTex, playerSettings.characterHeight, playerSettings.characterWidth, false);

            Font font2 = new Font(fontTex2, 8, playerSettings.characterWidth, true);
            debuginfo = new Print(800, font2, Color.White, true, 1f);
            developerConsole = new Print(500, font2, Color.White, true, new Point(resolution.X, 5), Direction.right, 1f);
            developerConsole1 = new Print(500, font2, Color.White, true, new Point(resolution.X, 14), Direction.right, 1f);
            printList.Add(debuginfo);
            printList.Add(developerConsole);
            printList.Add(developerConsole1);


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
                case 11:     //test level
                    LoadLevel(scene, false);
                    sceneIsMenu = false;
                    break;
                default:    //error
                    Console.WriteLine("Error. Scene ID " + scene + " does not exist.");
                    break;
            }

            LoadMenu(0, 0, false);
            sceneIsMenu = true;

            // TODO: use this.Content to load your game Content here

        }

        protected void LoadMenu(int menu, int startMenuLocation, bool loadSettings)
        {
            //geralt.inputEnabled = false;
            this.IsMouseVisible = true;
            scene = menu;
            sceneIsMenu = true;
            sList.Clear();
            printList.Clear();
            buttonList.Clear();
            //eList.Clear();
            if (loadSettings)
            {
                PlayerSettings playerSettings = Load(filename);
            }
            foreach (Print p in printList)
            {
                p.Clear();
            }
            Point tempLP;
            Point tempDP;
            sList.Add(new Square(menuTex[scene], Color.White, Point.Zero, 449, 140, false, true, true, 1f));

            if (resetRequired)
            {
                Print resettt = new Print(resolution.X - 132, font, Color.White, false, new Point(resolution.X - 32, resolution.Y - 26), Direction.right, 1f);
                resettt.Update("Restart the game to apply resolution changes!");
                printList.Add(resettt);
            }

            switch (scene)
            {
                case 0:     //main menu
                    tempLP = new Point(500, 280);       //distance from the bottom right corner of the screen
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 000), 500, 51), Direction.left, "New game", ">NEW GAME", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 050), 500, 51), Direction.left, "Continue", ">CONTINUE", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 100), 500, 51), Direction.left, "Options", ">OPTIONS", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 150), 500, 51), Direction.left, "Exit();", ">EXIT();", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    sceneIsMenu = true;
                    break;
                case 1:     //options menu
                    tempLP = new Point(500, 280);       //distance from the bottom right corner of the screen
                    if (!resetRequired)
                    {
                        Print op11t = new Print(resolution.X - 32, font, Color.White, false, new Point(resolution.X - 32, resolution.Y - 26), Direction.right, 1f);
                        op11t.Update("For even more options and details, view the playerSettings.ini file");
                        printList.Add(op11t);
                    }
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y + 025), 500, 51), Direction.left, "Controls", ">Controls", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 025), 500, 51), Direction.left, "Camera", ">Camera", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 075), 500, 51), Direction.left, "Video", ">Video", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 125), 500, 51), Direction.left, "Audio", ">Audio", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - tempLP.X, resolution.Y - (tempLP.Y - 175), 500, 51), Direction.left, "Misc", ">Misc", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bBack", "<\u001bBack", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    sceneIsMenu = true;
                    break;
                case 2:     //options - controls
                    tempLP = new Point(400, 314);       //distance from the bottom right corner of the screen
                    tempDP = new Point(100, 24);
                    listenForNewKeybind = false;
                    //playerSettings = Load(".\\Content\\playerSettings.ini");

                    for (int i = 0; i < 9; i++)
                    {
                        if (i < 10)
                        {
                            sList.Add(new Square(nullTex, new Color(223, 227, 236), new Point(resolution.X - (tempLP.X - 41), resolution.Y - (tempLP.Y - 3 - (24*i))), 280, 1, false, true, true, 1f));
                            sList.Add(new Square(nullTex, new Color(27, 28, 32), new Point(resolution.X - (tempLP.X - 42), resolution.Y - (tempLP.Y - 4 - (24 * i))), 280, 1, false, true, true, 1f));
                        }
                    }
                    Print op201t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 145), resolution.Y - (tempLP.Y + 48)), Direction.forward, 1f);
                    op201t.Update("Key");
                    printList.Add(op201t);
                    Print op202t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 245), resolution.Y - (tempLP.Y + 48)), Direction.forward, 1f);
                    op202t.Update("Alt.");
                    printList.Add(op202t);

                    Print op21t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y + 8)), Direction.right, 1f);
                    op21t.Update("Attack");
                    printList.Add(op21t);
                    Print op22t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 16)), Direction.right, 1f);
                    op22t.Update("Jump");
                    printList.Add(op22t);
                    Print op23t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 40)), Direction.right, 1f);
                    op23t.Update("Roll");
                    printList.Add(op23t);
                    Print op24t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 64)), Direction.right, 1f);
                    op24t.Update("Potion");
                    printList.Add(op24t);
                    Print op25t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 88)), Direction.right, 1f);
                    op25t.Update("Shield");
                    printList.Add(op25t);
                    Print op26t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 112)), Direction.right, 1f);
                    op26t.Update("Shockwave");
                    printList.Add(op26t);
                    Print op27t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 136)), Direction.right, 1f);
                    op27t.Update("Up");
                    printList.Add(op27t);
                    Print op28t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 160)), Direction.right, 1f);
                    op28t.Update("Down");
                    printList.Add(op28t);
                    Print op29t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 184)), Direction.right, 1f);
                    op29t.Update("Left");
                    printList.Add(op29t);
                    Print op210t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 030), resolution.Y - (tempLP.Y - 208)), Direction.right, 1f);
                    op210t.Update("Right");
                    printList.Add(op210t);
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y + 20), tempDP.X, tempDP.Y), Direction.forward, attackKey.ToString(), ">" + attackKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 4), tempDP.X, tempDP.Y), Direction.forward, jumpKey.ToString(), ">" + jumpKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 28), tempDP.X, tempDP.Y), Direction.forward, rollKey.ToString(), ">" + rollKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 52), tempDP.X, tempDP.Y), Direction.forward, potionKey.ToString(), ">" + potionKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 76), tempDP.X, tempDP.Y), Direction.forward, shieldKey.ToString(), ">" + shieldKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 100), tempDP.X, tempDP.Y), Direction.forward, shockwaveKey.ToString(), ">" + shockwaveKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 124), tempDP.X, tempDP.Y), Direction.forward, upKey.ToString(), ">" + upKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 148), tempDP.X, tempDP.Y), Direction.forward, downKey.ToString(), ">" + downKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 172), tempDP.X, tempDP.Y), Direction.forward, leftKey.ToString(), ">" + leftKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 196), tempDP.X, tempDP.Y), Direction.forward, rightKey.ToString(), ">" + rightKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y + 20), tempDP.X, tempDP.Y), Direction.forward, altAttackKey.ToString(), ">" + altAttackKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 4), tempDP.X, tempDP.Y), Direction.forward, altJumpKey.ToString(), ">" + altJumpKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 28), tempDP.X, tempDP.Y), Direction.forward, altRollKey.ToString(), ">" + altRollKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 52), tempDP.X, tempDP.Y), Direction.forward, altPotionKey.ToString(), ">" + altPotionKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 76), tempDP.X, tempDP.Y), Direction.forward, altShieldKey.ToString(), ">" + altShieldKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 100), tempDP.X, tempDP.Y), Direction.forward, altShockwaveKey.ToString(), ">" + altShockwaveKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 124), tempDP.X, tempDP.Y), Direction.forward, altUpKey.ToString(), ">" + altUpKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 148), tempDP.X, tempDP.Y), Direction.forward, altDownKey.ToString(), ">" + altDownKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 172), tempDP.X, tempDP.Y), Direction.forward, altLeftKey.ToString(), ">" + altLeftKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 195), resolution.Y - (tempLP.Y - 196), tempDP.X, tempDP.Y), Direction.forward, altRightKey.ToString(), ">" + altRightKey.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(100, resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    sceneIsMenu = true;
                    break;
                case 3:     //options - camera
                    boundingBoxBorder = new RectangleBorder(boundingBox, Color.Magenta, 0.8f);
                    Print BB = new Print(boundingBox.Width, font, Color.Magenta, false, new Point(boundingBox.Center.X, boundingBox.Center.Y), Direction.forward, 1f);
                    BB.Update("Bounding Box");
                    printList.Add(BB);

                    listenForNewKeybind = false;
                    //playerSettings = Load(".\\Content\\playerSettings.ini");
                    tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                    tempDP = new Point(75, 24);
                    Print op31t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y + 000)), Direction.forward, 1f);
                    op31t.Update("Bounding Box (anchor)");
                    printList.Add(op31t);
                    Print op31at = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 50), resolution.Y - (tempLP.Y - 18)), Direction.forward, 1f);
                    op31at.Update("X:");
                    printList.Add(op31at);
                    Print op31bt = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 12), resolution.Y - (tempLP.Y - 18)), Direction.forward, 1f);
                    op31bt.Update("Y:");
                    printList.Add(op31bt);
                    Print op32t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 48)), Direction.forward, 1f);
                    op32t.Update("Bounding Box (Width/Height)");
                    printList.Add(op32t);
                    Print op32at = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 50), resolution.Y - (tempLP.Y - 66)), Direction.forward, 1f);
                    op32at.Update("X:");
                    printList.Add(op32at);
                    Print op32bt = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 12), resolution.Y - (tempLP.Y - 66)), Direction.forward, 1f);
                    op32bt.Update("Y:");
                    printList.Add(op32bt);
                    Print op33t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 96)), Direction.forward, 1f);
                    op33t.Update("Camera Lerp");
                    printList.Add(op33t);
                    Print op34t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 144)), Direction.forward, 1f);
                    op34t.Update("Lerp Speed");
                    printList.Add(op34t);
                    Print op35t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 192)), Direction.forward, 1f);
                    op35t.Update("Camera Shake");
                    printList.Add(op35t);



                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 43), resolution.Y - (tempLP.Y - 10), 40, 16), Direction.forward, boundingBox.Center.X.ToString(), ">" + boundingBox.Center.X.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 19), resolution.Y - (tempLP.Y - 10), 40, 16), Direction.forward, boundingBox.Center.Y.ToString(), ">" + boundingBox.Center.Y.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 43), resolution.Y - (tempLP.Y - 58), 40, 16), Direction.forward, boundingBox.Width.ToString(), ">" + boundingBox.Width.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 19), resolution.Y - (tempLP.Y - 58), 40, 16), Direction.forward, boundingBox.Height.ToString(), ">" + boundingBox.Height.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 25), resolution.Y - (tempLP.Y - 106), 50, 16), Direction.forward, cameraLerp.ToString(), ">" + cameraLerp.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 25), resolution.Y - (tempLP.Y - 154), 50, 16), Direction.forward, cameraLerpSpeed.ToString(), ">" + cameraLerpSpeed.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 25), resolution.Y - (tempLP.Y - 202), 50, 16), Direction.forward, cameraShakeSetting.ToString(), ">" + cameraShakeSetting.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(100, resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));

                    sceneIsMenu = true;
                    break;
                case 4:     //options - video
                    listenForNewKeybind = false;
                    //playerSettings = Load(".\\Content\\playerSettings.ini");
                    tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                    tempDP = new Point(75, 24);
                    Print op41t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 48)), Direction.forward, 1f);
                    op41t.Update("Display");
                    printList.Add(op41t);
                    //Print op41at = new Print(100, font, Color.White, false, new Vector2(resolution.X - (tempLP.X + 50), resolution.Y - (tempLP.Y - 62)), Direction.forward, 1f);
                    //op41at.Update("X:");
                    //printList.Add(op41at);
                    //Print op41bt = new Print(100, font, Color.White, false, new Vector2(resolution.X - (tempLP.X - 50), resolution.Y - (tempLP.Y - 62)), Direction.forward, 1f);
                    //op41bt.Update("Y:");
                    //printList.Add(op41bt);
                    Print op42t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 96)), Direction.forward, 1f);
                    op42t.Update("Window Scale");
                    printList.Add(op42t);
                    Print op42at = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 95), resolution.Y - (tempLP.Y - 114)), Direction.forward, 1f);
                    op42at.Update("x");
                    printList.Add(op42at);
                    //Print op42bt = new Print(100, font, Color.White, false, new Vector2(resolution.X - (tempLP.X - 12), resolution.Y - (tempLP.Y - 112)), Direction.forward, 1f);
                    //op42bt.Update("2x");
                    //printList.Add(op42bt);
                    Print op43t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 144)), Direction.forward, 1f);
                    op43t.Update("Resolution");
                    printList.Add(op43t);
                    Print op43at = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 52), resolution.Y - (tempLP.Y - 162)), Direction.forward, 1f);
                    op43at.Update("X:");
                    printList.Add(op43at);
                    Print op43bt = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X - 11), resolution.Y - (tempLP.Y - 162)), Direction.forward, 1f);
                    op43bt.Update("Y:");
                    printList.Add(op43bt);
                    Print op46t = new Print(100, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 192)), Direction.forward, 1f);
                    op46t.Update("vSync");
                    printList.Add(op46t);

                    sList.Add(new Square(nullTex, new Color(223, 227, 236), new Point(resolution.X - (tempLP.X + 83), resolution.Y - (tempLP.Y - 73)), 65, 1, false, true, true, 1f));
                    sList.Add(new Square(nullTex, new Color(027, 028, 032), new Point(resolution.X - (tempLP.X + 82), resolution.Y - (tempLP.Y - 74)), 65, 1, false, true, true, 1f));

                    sList.Add(new Square(nullTex, new Color(223, 227, 236), new Point(resolution.X - (tempLP.X - 25), resolution.Y - (tempLP.Y - 73)), 69, 1, false, true, true, 1f));
                    sList.Add(new Square(nullTex, new Color(027, 028, 032), new Point(resolution.X - (tempLP.X - 26), resolution.Y - (tempLP.Y - 74)), 69, 1, false, true, true, 1f));

                    //sList.Add(new Square(nullTex, new Color(223, 227, 236), new Vector2(resolution.X - (tempLP.X + 83), resolution.Y - (tempLP.Y - 128)), 12, 1, false, true, true, 1f));
                    //sList.Add(new Square(nullTex, new Color(027, 028, 032), new Vector2(resolution.X - (tempLP.X + 82), resolution.Y - (tempLP.Y - 129)), 12, 1, false, true, true, 1f));

                    if (graphics.IsFullScreen)
                    {
                        sList[1].drawTex = sList[2].drawTex = false;
                        sList[3].drawTex = sList[4].drawTex = true;
                    }
                    else
                    {
                        sList[1].drawTex = sList[2].drawTex = true;
                        sList[3].drawTex = sList[4].drawTex = false;
                    }

                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 90), resolution.Y - (tempLP.Y - 58), 80, 16), Direction.forward, "windowed", ">windowed<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 20), resolution.Y - (tempLP.Y - 58), 80, 16), Direction.forward, "fullscreen", ">fullscreen<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 90), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "1x", ">1x<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 60), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "2x", ">2x<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 30), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "3x", ">3x<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 00), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "4x", ">4x<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 30), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "5x", ">5x<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 60), resolution.Y - (tempLP.Y - 106), 30, 16), Direction.forward, "", ">  <", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));

                    //buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 20), resolution.Y - (tempLP.Y - 110), 50, 20), Direction.forward, cameraLerp.ToString(), ">" + cameraLerp.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    //buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 20), resolution.Y - (tempLP.Y - 158), 50, 20), Direction.forward, cameraLerpSpeed.ToString(), ">" + cameraLerpSpeed.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 45), resolution.Y - (tempLP.Y - 154), 40, 16), Direction.forward, tempResolution.X.ToString(), ">" + tempResolution.X.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X - 18), resolution.Y - (tempLP.Y - 154), 40, 16), Direction.forward, tempResolution.Y.ToString(), ">" + tempResolution.Y.ToString() + "<", new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));

                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 25), resolution.Y - (tempLP.Y - 200), 50, 20), Direction.forward, IsFixedTimeStep.ToString(), ">" + IsFixedTimeStep.ToString() + "<", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(100, resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));

                    sceneIsMenu = true;
                    break;
                case 5:     //options - audio
                            //Console.WriteLine("Coming soon!\nHit escape to go back");
                    listenForNewKeybind = false;
                    //playerSettings = Load(".\\Content\\playerSettings.ini");
                    tempLP = new Point(250, 314);       //distance from the bottom right corner of the screen
                    tempDP = new Point(75, 24);
                    Print op52t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 48)), Direction.forward, 1f);
                    op52t.Update("Master");
                    printList.Add(op52t);
                    Print op53t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 96)), Direction.forward, 1f);
                    op53t.Update("Music");
                    printList.Add(op53t);
                    Print op54t = new Print(200, font, Color.White, false, new Point(resolution.X - (tempLP.X + 000), resolution.Y - (tempLP.Y - 144)), Direction.forward, 1f);
                    op54t.Update("Sound Effects");
                    printList.Add(op54t);

                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 170), resolution.Y - (tempLP.Y - 060), 40, 16), Direction.forward, masterAudioLevel.ToString("0"), ">" + masterAudioLevel.ToString("0"), new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    sliderList.Add(new UIElementSlider(Direction.left, new Point(resolution.X - (tempLP.X + 125), resolution.Y - (tempLP.Y - 058)), 250, 20, 100, Color.Red, new Color(166, 030, 030), nullTex, nullTex, nullTex, font, false, false, 0.905f, 0.95f, 0.9f));
                    sliderList[0].Update(masterAudioLevel);
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 170), resolution.Y - (tempLP.Y - 108), 40, 16), Direction.forward, musicLevel.ToString("0"), ">" + musicLevel.ToString("0"), new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    sliderList.Add(new UIElementSlider(Direction.left, new Point(resolution.X - (tempLP.X + 125), resolution.Y - (tempLP.Y - 106)), 250, 20, 100, Color.Red, new Color(255, 170, 000), nullTex, nullTex, nullTex, font, false, false, 0.905f, 0.95f, 0.9f));
                    sliderList[1].Update(musicLevel);
                    buttonList.Add(new Button(new Rectangle(resolution.X - (tempLP.X + 170), resolution.Y - (tempLP.Y - 156), 40, 16), Direction.forward, soundEffectsLevel.ToString("0"), ">" + soundEffectsLevel.ToString("0"), new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 1f));
                    sliderList.Add(new UIElementSlider(Direction.left, new Point(resolution.X - (tempLP.X + 125), resolution.Y - (tempLP.Y - 154)), 250, 20, 100, Color.Red, new Color(000, 234, 255), nullTex, nullTex, nullTex, font, false, false, 0.905f, 0.95f, 0.9f));
                    sliderList[2].Update(soundEffectsLevel);

                    sceneIsMenu = true;
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(100, resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    break;
                case 6:     //options - misc
                    Console.WriteLine("Coming soon!\nHit escape to go back");

                    //playerSettings = Load(".\\Content\\playerSettings.ini");
                    //Print op51t = new Print(100, font, Color.White, false, new Vector2(400, 200), Direction.right, 1f);
                    //op51t.Update("vSync");
                    //printList.Add(op51t);
                    //Button op51 = new Button(new Rectangle(400, 200, 200, 12), Direction.left, "Controls", ">Controls", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f);
                    //buttonList.Add(op51);
                    //Button op52 = new Button(new Rectangle(400, 213, 200, 12), Direction.left, "Camera", ">Camera", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f);
                    //buttonList.Add(op52);
                    //Button op53 = new Button(new Rectangle(400, 225, 200, 12), Direction.left, "Video", ">Video", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f);
                    //buttonList.Add(op53);
                    //Button op54 = new Button(new Rectangle(400, 237, 200, 12), Direction.left, "Audio", ">Audio", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f);
                    //buttonList.Add(op54);
                    sceneIsMenu = true;
                    buttonList.Add(new Button(new Rectangle(32, resolution.Y - 32, 50, 20), Direction.left, Side.left, "\u001bSave", "<\u001bSave", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    buttonList.Add(new Button(new Rectangle(100, resolution.Y - 32, 100, 20), Direction.left, Side.left, "\u001bCancel", "<\u001bCancel", Color.Magenta, nullTex, font, Color.Magenta, false, false, 1f));
                    break;
                default:
                    Console.WriteLine("Error. Scene ID " + scene + " is not in LoadMenu list");
                    break;
            }
            menuSelection = startMenuLocation;
        }

        protected void LoadLevel(int level, bool loadUI)
        {
            this.IsMouseVisible = false;
            scene = level;
            sceneIsMenu = false;
            sList.Clear();
            printList.Clear();
            buttonList.Clear();
            eList.Clear();
            printList.Add(debuginfo);
            printList.Add(developerConsole);
            printList.Add(developerConsole1);

            if (levelLoaded == 0) //THIS MEANS WE ARE LOADING A LEVEL FOR THE TITLESCREEN
            {
                levelLoaded = -1;
            }
            else
            {
                squareList.Clear();
                collisionObjects.Clear();
                backgroundSquareList.Clear();
                levelLoaded = scene;
            }

            switch (level)
            {
                case 11:
                    if (true)
                    {
                        enemy0Tex = Content.Load<Texture2D>("enemy0");
                        Texture2D shieldBarTex = Content.Load<Texture2D>("shieldBar");
                        Texture2D centerTex = Content.Load<Texture2D>("centerTexture");
                        Texture2D defaultSquareTex = Content.Load<Texture2D>("defaultTex");

                        timerDisplay = new Print(resolution.Y, font, Color.White, true, new Point(2, 7), Direction.left, 1f);

                        if (loadUI)
                        {
                            healthBar = new UIElementSlider(Direction.left, new Point(32, 32), 250, 20, geralt.maxHealth, Color.Red, new Color(166, 030, 030), nullTex, nullTex, shieldBarTex, font, false, 0.905f, 0.95f, 0.9f);
                            shieldBar = new UIElementSlider(Direction.left, new Point(32, 51), 150, 20, geralt.maxShield, Color.Red, new Color(255, 170, 000), nullTex, nullTex, shieldBarTex, font, false, 0.901f);
                            energyBar = new UIElementSlider(Direction.left, new Point(32, 70), 100, 20, geralt.maxEnergy, Color.Red, new Color(000, 234, 255), nullTex, nullTex, shieldBarTex, font, false, 0.9f);
                            potionBar = new UIElementDiscreteSlider(Direction.left, new Rectangle(184, 54, 96, 15), nullTex, nullTex, nullTex, Color.DarkSlateGray, Color.DarkRed, Color.DarkSlateBlue, geralt.maxNumberOfPotions, 3, 0.9f);
                            enemyHealthBar = new UIElementSlider(Direction.right, new Point(resolution.X - 32, 32), 500, 20, 100, Color.Red, new Color(166, 030, 030), nullTex, nullTex, shieldBarTex, font, false, 0.9f);
                        }
                        geralt.Respawn(initialPos);

                        timer = 0;
                        //timerCount = true;

                        screenspace = new RectangleBorder(new Rectangle(Point.Zero, resolution), Color.Magenta, 0f);
                        boundingBoxBorder = new RectangleBorder(boundingBox, Color.Magenta, 0.8f);

                        backgroundSquareList = new List<Square>();
                        for (int i = 0; i < 20; i++)                                                                                              //backgrounds
                        {
                            backgroundSquare = new Square(centerTex, Color.White, new Point((resolution.X - centerTex.Width) / 2, (resolution.Y - centerTex.Height) / 2), centerTex.Width, centerTex.Height, false, true, true, 0.05f * i);
                            backgroundSquareList.Add(backgroundSquare);
                        }


                        for (int i = 0; i < 5; i++)
                        {
                            Square tempSquare = new Square(defaultSquareTex, new Point(2 + i, 8 - (i * 5)), 0.3f);
                            squareList.Add(tempSquare);
                            collisionObjects.Add(tempSquare);
                        }

                        //for (int i = 0; i < 64; i++)        //slope
                        //{
                        //    Vector2 tempSquarePos = new Vector2(32 + (2 * i), 352 + (i));
                        //    Vector2 tempSquareColPos = new Vector2(16 + (2 * i), 352 + (i));
                        //    Square tempSquare = new Square(defaultSquareTex, tempSquarePos, tempSquareColPos, 32, 32, 32, 32, true, 0.3f);
                        //    sList.Add(tempSquare);
                        //}
                        for (int i = 0; i < 58; i++)                                                             //floor
                        {
                            Square tempSquare = new Square(defaultSquareTex, new Point(i - 8, 13), 0.3f);
                            squareList.Add(tempSquare);
                            collisionObjects.Add(tempSquare);
                        }
                        for (int i = 0; i < 31; i++)                                                             //left wall
                        {
                            Point tempSquarePos = new Point(0, -i + 8);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 0; i < 35; i++)                                                             //left wall part 2
                        {
                            Point tempSquarePos = new Point(-1, -i - 22);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 0; i < 69; i++)                                                             //left left wall
                        {
                            Point tempSquarePos = new Point(-8, -i + 12);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            Point tempSquarePos = new Point(49 + i * 2, 13 + i);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                            tempSquarePos = new Point(50 + i * 2, 13 + i);
                            tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 0; i < 10; i++)
                        {
                            Point tempSquarePos = new Point(68 + i, 22);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 0; i < 1; i++)
                        {
                            Point tempSquarePos = new Point(77, 21 - i);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                        for (int i = 1; i <= 10; i++)
                        {
                            Point tempSquarePos = new Point(10 * i - 2, -17);
                            Square tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                            tempSquarePos = new Point(10 * i - 3, -17);
                            tempSquare = new Square(defaultSquareTex, tempSquarePos, 0.3f);
                            collisionObjects.Add(tempSquare);
                            squareList.Add(tempSquare);
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Error. Scene ID " + scene + " is not a level.");
                    break;
            }
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



            if (!sceneIsMenu)
            {
                LevelUpdate(gameTime);
                if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||*/
                    (keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape)) ||
                    (keyboardState.IsKeyDown(Keys.Pause) && !previousKeyboardState.IsKeyDown(Keys.Pause)))
                {
                    framebyframe = false;
                    LoadMenu(0, 0, false);
                }
            }
            else
            {
                MenuUpdate(gameTime);
            }

            if (keyboardState.IsKeyDown(Keys.OemTilde) && !previousKeyboardState.IsKeyDown(Keys.OemTilde))
            {
                Debug();
            }

            if ((keyboardState.IsKeyDown(Keys.Tab) && !previousKeyboardState.IsKeyDown(Keys.Tab)) && debug)
            {
                acceptTextInput = true;
                geralt.inputEnabled = false;
                framebyframe = true;
            }

            if (debug && acceptTextInput)
            {
                developerConsole.Update(textInputBuffer, true);
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    framebyframe = acceptTextInput = false;
                    geralt.inputEnabled = true;
                    developerConsole.Update(string.Empty, true);
                    consoleParser(textInputBuffer);
                    textInputBuffer = string.Empty;
                }
            }

            if (developerConsole1.lines > 0)
            {
                developerConsole1.timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (developerConsole1.timer >= 3f / developerConsole1.lines)
                {
                    developerConsole1.DeleteLine();
                }
            }

            if (!framebyframe)
            {
                previousKeyboardState = keyboardState;
            }

            base.Update(gameTime);
        }

        protected void MenuUpdate(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mouseState = new MouseState(mouseState.X / screenScale, mouseState.Y / screenScale, mouseState.ScrollWheelValue, mouseState.LeftButton, mouseState.MiddleButton, mouseState.RightButton, mouseState.XButton1, mouseState.XButton2);
            debuginfo.Update("\n\nᴥ" + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0000")/* + "\n" + versionNo*/, true);

            switch (scene)
            {
                case 0:     //main menu
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                        {
                            menuSelection = i;
                            //buttonList[i].Update(buttonList[i].highlightStatement.ToUpper());
                        }
                    }
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        if (i == menuSelection)
                        {
                            buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                        }
                        else
                        {
                            buttonList[i].Update(buttonList[i].originalStatement);
                        }
                    }
                    if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) || (keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) ||
                        (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)) || (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                    {
                        menuSelection++;
                    }
                    if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) || (keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) ||
                        (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)) || (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                    {
                        menuSelection--;
                    }
                    if (menuSelection >= buttonList.Count)
                    {
                        menuSelection = 0;
                    }
                    if (menuSelection < 0)
                    {
                        menuSelection = buttonList.Count - 1;
                    }
                    //THIS DECIDES WHAT EACH BUTTON DOES
                    if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                    {
                        switch (menuSelection)
                        {
                            case 0:                         //0 == New Game
                                buttonList.Clear();
                                camera.X = resolution.X / 2;
                                camera.Y = resolution.Y / 2;
                                Console.WriteLine("    New Game");
                                levelLoaded = 11;
                                LoadLevel(11, true);
                                break;
                            case 1:                         //1 == Continue
                                buttonList.Clear();
                                Console.WriteLine("    Continue");
                                if (levelLoaded > 0)        ///THIS MEANS A /TRUE/ LEVEL (one not loaded exclusively for the titlescreen) HAS ALREADY BEEN LOADED
                                {
                                    sceneIsMenu = false;
                                }
                                else
                                {
                                    goto case 0;
                                }
                                break;
                            case 2:                         //2 == Options
                                buttonList.Clear();
                                Console.WriteLine("    Options");
                                LoadMenu(1, 0, false);
                                break;
                            case 3:                         //3 == Exit
                                buttonList.Clear();
                                Console.WriteLine("    Exit();");
                                Exit();
                                break;
                            default:
                                Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                break;
                        }
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        if (levelLoaded > 0)
                        {
                            buttonList.Clear();
                            Console.WriteLine("    Continue");
                            sceneIsMenu = false;
                        }
                        else
                        {
                            Exit();
                        }
                    }
                    break;
                case 1:     //options
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                        {
                            menuSelection = i;
                            //buttonList[i].Update(buttonList[i].highlightStatement.ToUpper());
                        }
                    }
                    for (int i = 0; i < buttonList.Count; i++)
                    {
                        if (i == menuSelection)
                        {
                            buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                        }
                        else
                        {
                            buttonList[i].Update(buttonList[i].originalStatement);
                        }
                    }
                    if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) || (keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) ||
                        (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)) || (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                    {
                        menuSelection++;
                    }
                    if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) || (keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) ||
                        (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)) || (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                    {
                        menuSelection--;
                    }
                    if (menuSelection >= buttonList.Count)
                    {
                        menuSelection = 0;
                    }
                    if (menuSelection < 0)
                    {
                        menuSelection = buttonList.Count - 1;
                    }
                    //THIS DECIDES WHAT EACH BUTTON DOES
                    if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                    {
                        switch (menuSelection)
                        {
                            case 0:                         //0 == Controls
                                buttonList.Clear();
                                Console.WriteLine("    Controls");
                                LoadMenu(2, 0, false);
                                break;
                            case 1:                         //1 == Camera
                                buttonList.Clear();
                                Console.WriteLine("    Camera");
                                LoadMenu(3, 0, false);
                                break;
                            case 2:                         //2 == Video
                                buttonList.Clear();
                                Console.WriteLine("    Video");
                                LoadMenu(4, 0, false);
                                break;
                            case 3:                         //3 == Audio
                                buttonList.Clear();
                                Console.WriteLine("    Audio");
                                LoadMenu(5, 0, false);
                                break;
                            case 4:                         //4 == Misc
                                buttonList.Clear();
                                Console.WriteLine("    Misc");
                                LoadMenu(6, 0, false);
                                break;

                            case 5:                         //5 == Back
                                LoadMenu(0, 2, false);
                                break;

                            default:
                                Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                break;
                        }
                    }
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        LoadMenu(0, 2, false);
                    }
                    break;
                case 2:     //options - controls
                    if (listenForNewKeybind)
                    {
                        buttonList[menuSelection].Update("_");
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            listenForNewKeybind = false;
                        }
                        else if (keyboardState.GetPressedKeys().Length > 0 && previousKeyboardState.GetPressedKeys().Length <=0)
                        {
                            switch (menuSelection)
                            {
                                case 0:                         //0 == Attack
                                    attackKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 1:                         //1 == Jump
                                    jumpKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 2:                         //2 == Roll
                                    rollKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 3:                         //3 == Potion
                                    potionKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 4:                         //4 == Shield
                                    shieldKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 5:                         //5 == Shockwave
                                    shockwaveKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 6:                         //6 == Up
                                    upKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 7:                         //7 == Down
                                    downKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 8:                         //8 == Left
                                    leftKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 9:                         //9 == Right
                                    rightKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                // ALTS FROM HERE DOWN
                                case 10:                         //0 == Attack
                                    altAttackKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 11:                         //1 == Jump
                                    altJumpKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 12:                         //2 == Roll
                                    altRollKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 13:                         //3 == Potion
                                    altPotionKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 14:                         //4 == Shield
                                    altShieldKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 15:                         //5 == Shockwave
                                    altShockwaveKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 16:                         //6 == Up
                                    altUpKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 17:                         //7 == Down
                                    altDownKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 18:                         //8 == Left
                                    altLeftKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                case 19:                         //9 == Right
                                    altRightKey = keyboardState.GetPressedKeys()[0];
                                    break;
                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }
                            buttonList[menuSelection].originalStatement = keyboardState.GetPressedKeys()[0].ToString();
                            buttonList[menuSelection].highlightStatement = ">" + buttonList[menuSelection].originalStatement + "<";

                            listenForNewKeybind = false;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                            {
                                menuSelection = i;
                                //buttonList[i].Update(buttonList[i].highlightStatement.ToUpper());
                            }
                            if (i == menuSelection)
                            {
                                buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                            }
                            else
                            {
                                buttonList[i].Update(buttonList[i].originalStatement);
                            }
                        }
                        if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) || 
                            (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)))
                        {
                            if (menuSelection == 9)
                            {
                                menuSelection += 10;
                            }
                            menuSelection++;
                        }
                        if ((keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) || (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                        {
                            if (menuSelection < 10)
                            {
                                menuSelection += 10;
                            }
                            else if (menuSelection < 20)
                            {
                                menuSelection -= 10;
                            }
                            else
                            {
                                menuSelection++;
                            }
                        }

                        if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) || 
                            (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)))
                        {
                            menuSelection--;
                        }
                        if ((keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) || (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                        {
                            if (menuSelection < 10)
                            {
                                menuSelection += 10;
                            }
                            else if (menuSelection < 20)
                            {
                                menuSelection -= 10;
                            }
                            else
                            {
                                menuSelection--;
                            }
                        }

                        if (menuSelection >= buttonList.Count)
                        {
                            menuSelection = 0;
                        }
                        if (menuSelection < 0)
                        {
                            menuSelection = buttonList.Count - 1;
                        }
                        //THIS DECIDES WHAT EACH BUTTON DOES
                        if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                        {
                            switch (menuSelection)
                            {
                                case 20: //save
                                    PlayerSettings.Save(this, filename);
                                    LoadMenu(1, 0, false);
                                    break;
                                case 21: //cancel
                                    LoadMenu(1, 0, false);
                                    break;
                                default:
                                    listenForNewKeybind = true;
                                    break;
                            }
                        }


                        //acceptTextInput = true;
                        //buttonList[0].Update(textInputBuffer, false);

                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            PlayerSettings.Save(this, filename);
                            LoadMenu(1, 0, false);
                        }
                    }
                    break;
                case 3:     //options - camera
                    if (listenForNewKeybind)
                    {
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            listenForNewKeybind = false;
                            buttonList[menuSelection].Update(buttonList[menuSelection].originalStatement, true);
                        }
                        if (menuSelection == 5)
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if ((char.IsDigit(textInputBuffer[0])) && buttonList[menuSelection].buttonStatement.Length < 8) //-
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }
                        else
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if (char.IsDigit(textInputBuffer[0]) && buttonList[menuSelection].buttonStatement.Length < 8)
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter)))
                        {
                            acceptTextInput = false;
                            listenForNewKeybind = false;
                            switch (menuSelection)
                            {
                                case 0:                         //0 == Anchor X
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        boundingBox.X = int.Parse(buttonList[menuSelection].buttonStatement) - (boundingBox.Width / 2);
                                        buttonList[menuSelection].originalStatement = boundingBox.Center.X.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + boundingBox.Center.X.ToString() + "<";
                                    }
                                    break;
                                case 1:                         //1 == Anchor Y
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        boundingBox.Y = int.Parse(buttonList[menuSelection].buttonStatement) - (boundingBox.Height / 2);
                                        buttonList[menuSelection].originalStatement = boundingBox.Center.Y.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + boundingBox.Center.Y.ToString() + "<";
                                    }
                                    break;
                                case 2:                         //2 == Width
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        Point oldCenter = boundingBox.Center;
                                        boundingBox.Width = int.Parse(buttonList[menuSelection].buttonStatement);
                                        boundingBox.X = oldCenter.X - (boundingBox.Width / 2);
                                        buttonList[menuSelection].originalStatement = boundingBox.Width.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + boundingBox.Width.ToString() + "<";
                                    }

                                    break;
                                case 3:                         //3 == Height
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        Point oldCenter = boundingBox.Center;
                                        boundingBox.Height = int.Parse(buttonList[menuSelection].buttonStatement);
                                        boundingBox.Y = oldCenter.Y - (boundingBox.Height / 2);
                                        buttonList[menuSelection].originalStatement = boundingBox.Height.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + boundingBox.Height.ToString() + "<";
                                    }

                                    break;
                                //case 4:                         //4 == Camera Lerp

                                //    break;
                                case 5:                         //5 == Lerp Speed
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        cameraLerpSpeed = float.Parse(buttonList[menuSelection].buttonStatement);
                                        buttonList[menuSelection].originalStatement = cameraLerpSpeed.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + cameraLerpSpeed.ToString() + "<";
                                    }

                                    break;
                                //case 6:                         //6 == Camera Shake

                                //    break;
                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }
                            boundingBoxBorder.Update(boundingBox, Color.Magenta);
                            printList[0].Update(new Point(boundingBox.Center.X, boundingBox.Center.Y));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                            {
                                menuSelection = i;
                                //buttonList[i].Update(buttonList[i].highlightStatement.ToUpper());
                            }
                        }
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (i == menuSelection)
                            {
                                buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                            }
                            else
                            {
                                buttonList[i].Update(buttonList[i].originalStatement);
                            }
                        }
                        if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) || 
                            (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)))
                        {
                            if (menuSelection >= 0 && menuSelection <= 2)
                            {
                                menuSelection += 2;
                            }
                            else
                            {
                                menuSelection++;
                            }
                        }
                        if ((keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) || (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                        {
                            menuSelection++;
                        }

                        if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) || 
                            (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)))
                        {
                            if (menuSelection >= 1 && menuSelection <= 3)
                            {
                                menuSelection -= 2;
                            }
                            else
                            {
                                menuSelection--;
                            }
                        }
                        if ((keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) || (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                        {
                            menuSelection--;
                        }

                        if (menuSelection >= buttonList.Count)
                        {
                            menuSelection = 0;
                        }
                        if (menuSelection < 0)
                        {
                            menuSelection = buttonList.Count - 1;
                        }
                        //THIS DECIDES WHAT EACH BUTTON DOES
                        if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                        {
                            switch (menuSelection)
                            {
                                case 0:                         //0 == Anchor X
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 1:                         //1 == Anchor Y
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 2:                         //2 == Width
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 3:                         //3 == Height
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 4:                         //4 == Camera Lerp
                                    cameraLerp = !cameraLerp;
                                    buttonList[menuSelection].originalStatement = cameraLerp.ToString();
                                    buttonList[menuSelection].highlightStatement = ">" + cameraLerp.ToString() + "<";
                                    break;
                                case 5:                         //5 == Lerp Speed
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 6:                         //6 == Camera Shake
                                    cameraShakeSetting = !cameraShakeSetting;
                                    buttonList[menuSelection].originalStatement = cameraShakeSetting.ToString();
                                    buttonList[menuSelection].highlightStatement = ">" + cameraShakeSetting.ToString() + "<";
                                    break;
                                case 7: //save
                                    PlayerSettings.Save(this, filename);
                                    LoadMenu(1, 1, false);
                                    break;
                                case 8: //cancel
                                    LoadMenu(1, 1, false);
                                    break;

                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }


                        }

                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            PlayerSettings.Save(this, filename);
                            LoadMenu(1, 1, false);
                        }
                    }
                    break;
                case 4:     //options - video
                    if (listenForNewKeybind)
                    {
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            listenForNewKeybind = false;
                            buttonList[menuSelection].Update(buttonList[menuSelection].originalStatement, true);
                        }
                        if (menuSelection == 5)
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if ((char.IsDigit(textInputBuffer[0])) && buttonList[menuSelection].buttonStatement.Length < 8)
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }
                        else
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if (char.IsDigit(textInputBuffer[0]) && buttonList[menuSelection].buttonStatement.Length < 8)
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter)))
                        {
                            acceptTextInput = false;
                            listenForNewKeybind = false;
                            switch (menuSelection)
                            {
                                case 7:                         //7 == Screen Scale
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        if (int.Parse(buttonList[menuSelection].buttonStatement) > 100)
                                        {
                                            screenScale = 100;
                                        }
                                        else
                                        {
                                            screenScale = int.Parse(buttonList[menuSelection].buttonStatement);
                                        }
                                        buttonList[menuSelection].originalStatement = screenScale.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + screenScale.ToString() + "<";
                                        if (!resetRequired)
                                        {
                                            graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                            graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                            graphics.ApplyChanges();
                                        }
                                    }
                                    break;
                                case 8:                         //8 == Resolution.X
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        tempResolution.X = int.Parse(buttonList[menuSelection].buttonStatement);
                                        buttonList[menuSelection].originalStatement = tempResolution.X.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + tempResolution.X.ToString() + "<";
                                        PlayerSettings.Save(this, filename);
                                        resetRequired = true;
                                        LoadMenu(4, 8, false);
                                    }
                                    break;
                                case 9:                         //7 == Resolution.Y
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        tempResolution.Y = int.Parse(buttonList[menuSelection].buttonStatement);
                                        buttonList[menuSelection].originalStatement = tempResolution.Y.ToString();
                                        buttonList[menuSelection].highlightStatement = ">" + tempResolution.Y.ToString() + "<";
                                        PlayerSettings.Save(this, filename);
                                        resetRequired = true;
                                        LoadMenu(4, 9, false);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            PlayerSettings.Save(this, filename);
                            LoadMenu(1, 2, false);
                        }
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                            {
                                menuSelection = i;
                                //buttonList[i].Update(buttonList[i].highlightStatement.ToUpper());
                            }
                        }
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            if (i == menuSelection)
                            {
                                buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                            }
                            else
                            {
                                buttonList[i].Update(buttonList[i].originalStatement);
                            }
                        }
                        if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) ||
                            (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)))
                        {
                            if (menuSelection == 0)//menuSelection <= 1)
                            {
                                menuSelection = 3;
                            }
                            else if (menuSelection == 1)
                            {
                                menuSelection = 6;
                            }
                            else if (menuSelection >= 2 && menuSelection <= 4)
                            {
                                menuSelection = 8;
                            }
                            else if (menuSelection >= 5 && menuSelection <= 7)
                            {
                                menuSelection = 9;
                            }
                            else if (menuSelection >= 8 && menuSelection <= 9)
                            {
                                menuSelection = 10;
                            }
                            else
                            {
                                menuSelection++;
                            }
                        }
                        if ((keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) || (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                        {
                            menuSelection++;
                        }

                        if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) ||
                            (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)))
                        {
                            if (menuSelection >= 2 && menuSelection <= 4)
                            {
                                menuSelection = 0;
                            }
                            else if (menuSelection >= 5 && menuSelection <= 7)
                            {
                                menuSelection = 1;
                            }
                            else if (menuSelection == 8)
                            {
                                menuSelection = 3;
                            }
                            else if (menuSelection == 9)
                            {
                                menuSelection = 6;
                            }
                            else
                            {
                                menuSelection--;
                            }
                        }
                        if ((keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) || (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                        {
                            menuSelection--;
                        }

                        if (menuSelection >= buttonList.Count)
                        {
                            menuSelection = 0;
                        }
                        if (menuSelection < 0)
                        {
                            menuSelection = buttonList.Count - 1;
                        }
                        //THIS DECIDES WHAT EACH BUTTON DOES
                        if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                        {
                            switch (menuSelection)
                            {
                                case 0:                         //0 == windowed
                                    graphics.IsFullScreen = false;
                                    buttonList[0].buttonStatement = buttonList[0].highlightStatement;
                                    buttonList[1].buttonStatement = buttonList[1].originalStatement;
                                    sList[1].drawTex = sList[2].drawTex = true;
                                    sList[3].drawTex = sList[4].drawTex = false;
                                    break;
                                case 1:                         //1 == fullscreen
                                    //playerSettings.fullscreen = graphics.IsFullScreen = true;
                                    buttonList[0].buttonStatement = buttonList[0].originalStatement;
                                    buttonList[1].buttonStatement = buttonList[1].highlightStatement;
                                    sList[1].drawTex = sList[2].drawTex = false;
                                    sList[3].drawTex = sList[4].drawTex = true;
                                    break;
                                case 2:                         //2 == 1x
                                    screenScale = 1;
                                    if (!resetRequired)
                                    {
                                        graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                        graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                    }
                                    graphics.ApplyChanges();
                                    break;
                                case 3:                         //3 == 2x
                                    screenScale = 2;
                                    if (!resetRequired)
                                    {
                                        graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                        graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                    }
                                    graphics.ApplyChanges();
                                    break;
                                case 4:                         //4 == 3x
                                    screenScale = 3;
                                    if (!resetRequired)
                                    {
                                        graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                        graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                    }
                                    graphics.ApplyChanges();
                                    break;
                                case 5:                         //5 == 4x
                                    screenScale = 4;
                                    if (!resetRequired)
                                    {
                                        graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                        graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                    }
                                    graphics.ApplyChanges();
                                    break;
                                case 6:                         //6 == 5x
                                    screenScale = 5;
                                    if (!resetRequired)
                                    {
                                        graphics.PreferredBackBufferHeight = resolution.Y * screenScale;
                                        graphics.PreferredBackBufferWidth = resolution.X * screenScale;
                                    }
                                    graphics.ApplyChanges();
                                    break;
                                case 7:                         //7 == ___x
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 8:                         //8 == resolution.X
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 9:                         //9 == resolution.Y
                                    acceptTextInput = true;
                                    listenForNewKeybind = true;
                                    buttonList[menuSelection].Update("", true);
                                    break;
                                case 10:                         //10 == vSync
                                    graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = !IsFixedTimeStep;
                                    buttonList[menuSelection].originalStatement = IsFixedTimeStep.ToString();
                                    buttonList[menuSelection].highlightStatement = ">" + IsFixedTimeStep.ToString() + "<";
                                    break;
                                case 11:                         //Save
                                    PlayerSettings.Save(this, filename);
                                    LoadMenu(1, 2, false);
                                    break;
                                case 12:                         //Cancel
                                    LoadMenu(1, 2, false);
                                    break;

                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }
                        }
                    }
                    break;
                case 5:     //options - audio
                    if (listenForNewKeybind)
                    {
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            listenForNewKeybind = false;
                            buttonList[menuSelection].Update(buttonList[menuSelection].originalStatement, true);
                        }
                        if (menuSelection == 5)
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if ((char.IsDigit(textInputBuffer[0]) || textInputBuffer[0].Equals('\u002e')) && buttonList[menuSelection].buttonStatement.Length < 8)
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }
                        else
                        {
                            while (textInputBuffer.Length > 0)
                            {
                                if ((char.IsDigit(textInputBuffer[0]) || textInputBuffer[0].Equals('\u002e')) && buttonList[menuSelection].buttonStatement.Length < 8)
                                {
                                    buttonList[menuSelection].Update(textInputBuffer[0].ToString(), false);
                                }
                                textInputBuffer = textInputBuffer.Substring(1);
                            }
                        }

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter)))
                        {
                            acceptTextInput = false;
                            listenForNewKeybind = false;
                            float floatResult;
                            switch (menuSelection)
                            {
                                case 0:                         //0 == master audio
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        if (float.TryParse(buttonList[menuSelection].buttonStatement, out floatResult))
                                        {
                                            if (floatResult > 100f)
                                            {
                                                masterAudioLevel = 100f;
                                            }
                                            else
                                            {
                                                masterAudioLevel = floatResult;
                                            }
                                        }
                                        else
                                        {
                                            buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                        }
                                        buttonList[menuSelection].originalStatement = masterAudioLevel.ToString("0");
                                        buttonList[menuSelection].highlightStatement = ">" + masterAudioLevel.ToString("0");
                                        sliderList[menuSelection].Update(masterAudioLevel);
                                    }
                                    break;
                                case 1:                         //1 == music
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        if (float.TryParse(buttonList[menuSelection].buttonStatement, out floatResult))
                                        {
                                            if (floatResult > 100f)
                                            {
                                                musicLevel = 100f;
                                            }
                                            else
                                            {
                                                musicLevel = floatResult;
                                            }
                                        }
                                        else
                                        {
                                            buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                        }
                                        buttonList[menuSelection].originalStatement = musicLevel.ToString("0");
                                        buttonList[menuSelection].highlightStatement = ">" + musicLevel.ToString("0");
                                        sliderList[menuSelection].Update(musicLevel);
                                    }
                                    break;
                                case 2:                         //2 == sound effects
                                    if (buttonList[menuSelection].buttonStatement.Length <= 0)
                                    {
                                        buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                    }
                                    else
                                    {
                                        if (float.TryParse(buttonList[menuSelection].buttonStatement, out floatResult))
                                        {
                                            if (floatResult > 100f)
                                            {
                                                soundEffectsLevel = 100f;
                                            }
                                            else
                                            {
                                                soundEffectsLevel = floatResult;
                                            }
                                        }
                                        else
                                        {
                                            buttonList[menuSelection].buttonStatement = buttonList[menuSelection].originalStatement;
                                        }
                                        buttonList[menuSelection].originalStatement = soundEffectsLevel.ToString("0");
                                        buttonList[menuSelection].highlightStatement = ">" + soundEffectsLevel.ToString("0");
                                        sliderList[menuSelection].Update(soundEffectsLevel);
                                    }
                                    break;
                                default:
                                    Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                        {
                            sliderList.Clear();
                            PlayerSettings.Save(this, filename);
                            LoadMenu(1, 3, false);
                        }
                        for (int i = 0; i < sliderList.Count; i++)
                        {
                            if (sliderList[i].Pressed(mouseState) && sliderPressed < 0)
                            {
                                sliderPressed = i;
                            }
                        }

                        if (mouseState.LeftButton != ButtonState.Pressed)
                        {
                            sliderPressed = -1;
                        }



                        


                        switch (sliderPressed)
                        {
                            case 0:
                                masterAudioLevel = ((float)(mouseState.X - sliderList[menuSelection].bounds.Left) / (float)(sliderList[menuSelection].bounds.Width)) * 100;
                                if (masterAudioLevel >= 100f)
                                {
                                    masterAudioLevel = 100f;
                                }
                                else if (masterAudioLevel <= 0f)
                                {
                                    masterAudioLevel = 0f;
                                }
                                buttonList[sliderPressed].originalStatement = buttonList[sliderPressed].buttonStatement = masterAudioLevel.ToString("0.0");
                                buttonList[sliderPressed].highlightStatement = ">" + masterAudioLevel.ToString("0");
                                buttonList[sliderPressed].Update(buttonList[sliderPressed].buttonStatement);
                                sliderList[sliderPressed].Update(masterAudioLevel);
                                break;
                            case 1:
                                musicLevel = ((float)(mouseState.X - sliderList[menuSelection].bounds.Left) / (float)(sliderList[menuSelection].bounds.Width)) * 100;
                                if (musicLevel >= 100f)
                                {
                                    musicLevel = 100f;
                                }
                                else if (musicLevel <= 0f)
                                {
                                    musicLevel = 0f;
                                }
                                buttonList[sliderPressed].originalStatement = buttonList[sliderPressed].buttonStatement = musicLevel.ToString("0.0");
                                buttonList[sliderPressed].highlightStatement = ">" + musicLevel.ToString("0.0");
                                buttonList[sliderPressed].Update(buttonList[sliderPressed].buttonStatement);
                                sliderList[sliderPressed].Update(musicLevel);
                                break;
                            case 2:
                                soundEffectsLevel = ((float)(mouseState.X - sliderList[menuSelection].bounds.Left) / (float)(sliderList[menuSelection].bounds.Width)) * 100;
                                if (soundEffectsLevel >= 100f)
                                {
                                    soundEffectsLevel = 100f;
                                }
                                else if (soundEffectsLevel <= 0f)
                                {
                                    soundEffectsLevel = 0f;
                                }
                                buttonList[sliderPressed].originalStatement = buttonList[sliderPressed].buttonStatement = soundEffectsLevel.ToString("0.0");
                                buttonList[sliderPressed].highlightStatement = ">" + soundEffectsLevel.ToString("0.0");
                                buttonList[sliderPressed].Update(buttonList[sliderPressed].buttonStatement);
                                sliderList[sliderPressed].Update(soundEffectsLevel);
                                break;
                            default:
                                for (int i = 0; i < buttonList.Count; i++)
                                {
                                    if (buttonList[i].Contains(mouseState) && mouseState != previousMouseState)
                                    {
                                        menuSelection = i;
                                    }
                                    if (i < sliderList.Count)
                                    {
                                        if (sliderList[i].Contains(mouseState) && mouseState != previousMouseState)
                                        {
                                            menuSelection = i;
                                        }
                                    }
                                }
                                if ((keyboardState.IsKeyDown(downKey) && !previousKeyboardState.IsKeyDown(downKey)) ||
                                    (keyboardState.IsKeyDown(altDownKey) && !previousKeyboardState.IsKeyDown(altDownKey)))
                                {
                                    menuSelection++;
                                }
                                if ((keyboardState.IsKeyDown(rightKey) && !previousKeyboardState.IsKeyDown(rightKey)) ||
                                    (keyboardState.IsKeyDown(altRightKey) && !previousKeyboardState.IsKeyDown(altRightKey)))
                                {
                                    switch (menuSelection)
                                    {
                                        case 0:
                                            masterAudioLevel += 5;
                                            if (masterAudioLevel >= 100f)
                                            {
                                                masterAudioLevel = 100f;
                                            }
                                            buttonList[menuSelection].originalStatement = masterAudioLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + masterAudioLevel.ToString("0");
                                            sliderList[menuSelection].Update(masterAudioLevel);
                                            break;
                                        case 1:
                                            musicLevel += 5;
                                            if (musicLevel >= 100f)
                                            {
                                                musicLevel = 100f;
                                            }
                                            buttonList[menuSelection].originalStatement = musicLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + musicLevel.ToString("0");
                                            sliderList[menuSelection].Update(musicLevel);
                                            break;
                                        case 2:
                                            soundEffectsLevel += 5;
                                            if (soundEffectsLevel >= 100f)
                                            {
                                                soundEffectsLevel = 100f;
                                            }
                                            buttonList[menuSelection].originalStatement = soundEffectsLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + soundEffectsLevel.ToString("0");
                                            sliderList[menuSelection].Update(soundEffectsLevel);
                                            break;
                                    }
                                }

                                if ((keyboardState.IsKeyDown(upKey) && !previousKeyboardState.IsKeyDown(upKey)) ||
                                    (keyboardState.IsKeyDown(altUpKey) && !previousKeyboardState.IsKeyDown(altUpKey)))
                                {
                                    menuSelection--;
                                }
                                if ((keyboardState.IsKeyDown(leftKey) && !previousKeyboardState.IsKeyDown(leftKey)) ||
                                    (keyboardState.IsKeyDown(altLeftKey) && !previousKeyboardState.IsKeyDown(altLeftKey)))
                                {
                                    switch (menuSelection)
                                    {
                                        case 0:
                                            masterAudioLevel -= 5;
                                            if (masterAudioLevel <= 0f)
                                            {
                                                masterAudioLevel = 0f;
                                            }
                                            buttonList[menuSelection].originalStatement = masterAudioLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + masterAudioLevel.ToString("0");
                                            sliderList[menuSelection].Update(masterAudioLevel);
                                            break;
                                        case 1:
                                            musicLevel -= 5;
                                            if (musicLevel <= 0f)
                                            {
                                                musicLevel = 0f;
                                            }
                                            buttonList[menuSelection].originalStatement = musicLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + musicLevel.ToString("0");
                                            sliderList[menuSelection].Update(musicLevel);
                                            break;
                                        case 2:
                                            soundEffectsLevel -= 5;
                                            if (soundEffectsLevel <= 0f)
                                            {
                                                soundEffectsLevel = 0f;
                                            }
                                            buttonList[menuSelection].originalStatement = soundEffectsLevel.ToString("0");
                                            buttonList[menuSelection].highlightStatement = ">" + soundEffectsLevel.ToString("0");
                                            sliderList[menuSelection].Update(soundEffectsLevel);
                                            break;
                                    }
                                }
                                if (menuSelection >= buttonList.Count)
                                {
                                    menuSelection = 0;
                                }
                                if (menuSelection < 0)
                                {
                                    menuSelection = buttonList.Count - 1;
                                }

                                for (int i = 0; i < buttonList.Count; i++)
                                {
                                    if (i == menuSelection)
                                    {
                                        buttonList[menuSelection].Update(buttonList[menuSelection].highlightStatement.ToUpper());
                                    }
                                    else
                                    {
                                        buttonList[i].Update(buttonList[i].originalStatement);
                                    }
                                }

                                //THIS DECIDES WHAT EACH BUTTON DOES
                                if (buttonList[menuSelection].Pressed(mouseState, previousMouseState) || Use())
                                {
                                    switch (menuSelection)
                                    {
                                        case 0:                         //7 == ___x
                                            acceptTextInput = true;
                                            listenForNewKeybind = true;
                                            buttonList[menuSelection].Update("", true);
                                            break;
                                        case 1:                         //8 == resolution.X
                                            acceptTextInput = true;
                                            listenForNewKeybind = true;
                                            buttonList[menuSelection].Update("", true);
                                            break;
                                        case 2:                         //9 == resolution.Y
                                            acceptTextInput = true;
                                            listenForNewKeybind = true;
                                            buttonList[menuSelection].Update("", true);
                                            break;
                                        case 3:                         //Save
                                            sliderList.Clear();
                                            PlayerSettings.Save(this, filename);
                                            LoadMenu(1, 3, false);
                                            break;
                                        case 4:                         //Cancel
                                            sliderList.Clear();
                                            LoadMenu(1, 3, false);
                                            break;

                                        default:
                                            Console.WriteLine("Error. Menu item " + menuSelection + " does not exist.");
                                            break;
                                    }
                                }
                                break;
                        }

                    }
                    break;
                case 6:     //options - misc
                    if (/*GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || */keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
                    {
                        LoadMenu(1, 4, false);
                    }
                    break;
                default:
                    Console.WriteLine("Error. Scene ID " + scene + " is not in MenuUpdate list");
                    break;
            }
            previousMouseState = mouseState;
        }

        protected void LevelUpdate(GameTime gameTime)
        {            
            if (debug)
            {
                debuginfo.Update("      DEBUG MODE. " + versionID.ToUpper() + " v" + versionNo, true);
                if (smoothFPSframecounter >= smoothFPS.Length) { smoothFPSframecounter = 0; }
                smoothFPS[smoothFPSframecounter] = gameTime.ElapsedGameTime.TotalSeconds;
                smoothFPSframecounter++;
                double tempSmoothFPS = 0;
                for (int i = smoothFPS.Length - 1; i >= 0; i--)
                {
                    tempSmoothFPS += smoothFPS[i];
                }
                debuginfo.Update("\n      FPS:" + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0000.0"));
                debuginfo.Update("\nsmoothFPS:" + (smoothFPS.Length / tempSmoothFPS).ToString("0000.0"));
                debuginfo.Update("\n    input:" + geralt.input + "  isRunning:" + geralt.isRunning);
                debuginfo.Update("\nprevInput:" + geralt.prevInput);
                debuginfo.Update("\nwalledInputChange:" + geralt.walledInputChange);
                debuginfo.Update("\n rolltime:" + geralt.rollTime);
                debuginfo.Update("\nFrame-by-frame mode:" + framebyframe);
                debuginfo.Update("\n\n  player info");
                debuginfo.Update("\nHealth:" + geralt.health + "\nShield:" + geralt.shield + "\nEnergy:" + geralt.energy);
                debuginfo.Update("\n  Xpos:" + geralt.pos.X + "\n  Ypos:" + geralt.pos.Y);
                debuginfo.Update("\n  Xvel:" + geralt.velocity.X + "\n  Yvel:" + geralt.velocity.Y);
                debuginfo.Update("\ninvulner:" + geralt.invulnerableTime);
                debuginfo.Update("\nShielded:" + geralt.shielded);
                debuginfo.Update("\ncolliders:" + collisionObjects.Count);
                debuginfo.Update("\n collided:" + geralt.collided.Count);
                debuginfo.Update("\n botWall:" + geralt.bottomWalled + "\n topWall:" + geralt.topWalled + "\nleftWall:" + geralt.leftWalled + "\nriteWall:" + geralt.rightWalled);
                debuginfo.Update("\nactivity:" + geralt.activity);
                debuginfo.Update("\nattackin:" + geralt.attacking);
                debuginfo.Update("\nattackID:" + geralt.attackID);
                //debuginfo.Update("\ncamera\n{M11:" + camera.M11.ToString("000") + ", M12:" + camera.M12.ToString("000") + ", M13:" + camera.M13.ToString("000") + ", M14:" + camera.M14.ToString("000") + "}");
                //debuginfo.Update("\n{M21:" + camera.M21.ToString("000") + ", M22:" + camera.M22.ToString("000") + ", M23:" + camera.M23.ToString("000") + ", M24:" + camera.M24.ToString("000") + "}");
                debuginfo.Update("\n\ncamera:\n{M31:" + background.M31.ToString("000.00") + ", M32:" + background.M32.ToString("000.00") + ", M33:" + background.M33.ToString("000.00") + ", M34:" + background.M34.ToString("000.00") + "}");
                debuginfo.Update("\n{M41:" + foreground.M41.ToString("000.00") + ", M42:" + foreground.M42.ToString("000.00") + ", M43:" + foreground.M43.ToString("000.00") + ", M44:" + foreground.M44.ToString("000.00") + "}");
                //debuginfo.Update("\n01234567890ABCDEFGHIJKLMNOPQRSTUVWQYZabcdefghijklmnopqrstuvwxyz!@#$%^&*()_+><~" + '\u001b' + "{}|.`,:;/@'[\\]\"ᴥ");
                debuginfo.Update("\n\nTotal of " + eList.Count + " enemies");
                debuginfo.Update("\nEnemy Info:");

                float avghp = 0;
                for (int i = eList.Count - 1; i >= 0; i--)
                {
                    avghp += eList[i].health;
                }
                avghp = avghp / eList.Count;
                debuginfo.Update("\n  avg health: " + avghp);
                for (int i = 0; i < eList.Count; i++)
                {
                    debuginfo.Update("\n  enemy " + i);
                    debuginfo.Update("\n  health: " + eList[i].health);
                    debuginfo.Update("\n  Xpos:" + eList[i].pos.X + "\n  Ypos:" + eList[i].pos.Y);
                    debuginfo.Update("\n  Xvel:" + eList[i].velocity.X + "\n  Yvel:" + eList[i].velocity.Y);
                    if (i > 3)
                    {
                        i = eList.Count;
                    }
                }


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
                debuginfo.Update("\n\nᴥ" + (1 / gameTime.ElapsedGameTime.TotalSeconds).ToString("0000")/* + "\n" + versionNo*/, true);
            }

            if ((keyboardState.IsKeyDown(Keys.R) && !acceptTextInput) || geralt.health <= 0)                            //RESPAWN
            {
                geralt.Respawn(initialPos);
                eList.Clear();
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
                //geralt.Input(gameTime);
                geralt.Update(gameTime);

                Camera(gameTime);

                timer += gameTime.ElapsedGameTime.TotalSeconds;
                timerDisplay.Update(TimerText(timer), true);
                //PlayerCollision(geralt, sList);



                for (int i = 0; i < eList.Count; i++)
                {
                    eList[i].Update(gameTime);
                    //eList[i].health += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (eList[i].health <= 0 || eList[i].pos.Y > 1000)
                    {
                        eList.Remove(eList[i]);
                        collisionObjects.Remove(eList[i]);
                        i--;
                    }
                }

                healthBar.Update(gameTime, false, geralt.health);
                if (geralt.shielded)
                {
                    shieldBar.Update(gameTime, true, geralt.shield);
                }
                else
                {
                    shieldBar.Update(gameTime, false, geralt.shield);
                }
                energyBar.Update(gameTime, geralt.energy);

                if (true)
                {
                    if (eList.Count > 0)
                    {
                        Enemy closest = eList[0];

                        float closestSqrDistance = float.MaxValue;
                        float thisEnemysSqrDistance = 0f;
                        foreach (Enemy e in eList)
                        {
                            thisEnemysSqrDistance = Vector2.DistanceSquared(geralt.pos, e.pos);
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
                            enemyHealthBar.Update(gameTime, closest.health);
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

            if (keyboardState.IsKeyDown(Keys.K) && !previousKeyboardState.IsKeyDown(Keys.K) && !acceptTextInput)
            {
                randomTimer = 0f;
                Enemy tempEnemy = new Enemy(enemy0Tex, new Vector2(20f, 5f), this);
                eList.Add(tempEnemy);
                collisionObjects.Add(tempEnemy);
            }

            if (keyboardState.IsKeyDown(Keys.K) && !acceptTextInput)
            {
                randomTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (randomTimer > 0.5f)
                {
                    Enemy tempEnemy = new Enemy(enemy0Tex, new Vector2(20f, 5f), this);
                    eList.Add(tempEnemy);
                    collisionObjects.Add(tempEnemy);
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
                default:
                    textInputBuffer += e.Character;
                    break;
            }
        }

        public void Camera(GameTime gameTime)
        {
            screenSpacePlayerPos.X = geralt.collider.Center.X + (int)foreground.M41;
            screenSpacePlayerPos.Y = geralt.collider.Center.Y + (int)foreground.M42;

            //mainCamera is used for returning the camera to where it "should" be
            //camera is what is displayed on-screen
            if (cameraLerp)
            {
                if (boundingBox.Right <= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X = (Lerp(camera.X + boundingBox.Right, camera.X + (screenSpacePlayerPos.X), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) - boundingBox.Right);
                }
                else if (boundingBox.Left >= screenSpacePlayerPos.X)
                {
                    mainCamera.X = camera.X = (Lerp(camera.X + boundingBox.Left, camera.X + (screenSpacePlayerPos.X), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) - boundingBox.Left);
                }
                if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y = (Lerp(camera.Y + boundingBox.Bottom, camera.Y + (screenSpacePlayerPos.Y), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) - boundingBox.Bottom);
                }
                else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                {
                    mainCamera.Y = camera.Y = (Lerp(camera.Y + boundingBox.Top, camera.Y + (screenSpacePlayerPos.Y), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) - boundingBox.Top);
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

            background.M31 = foreground.M41 = resolution.X / 2 - camera.X;        //optimize
            background.M32 = foreground.M42 = resolution.Y / 2 - camera.Y;

            if (cameraShakeSetting && cameraShakeDuration > 0) { CameraShake(gameTime); }
            if (cameraSwingSetting && cameraSwingDuration > 0) { CameraSwing(gameTime); }
        }

        public void CameraSwing(GameTime gameTime)
        {
            cameraSwingDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (cameraSwingDuration <= 0 && cameraSwing)
            {
                cameraSwing = false;
                cameraSwingDuration = cameraSwingMaxDuration * 2;
            }

            if (cameraSwing)
            {
                //background.M31 = foreground.M41 = Lerp(background.M31, background.M31 + (cameraSwingHeading.X * cameraSwingMagnitude), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
                camera.X = (Lerp(camera.X, camera.X + (cameraSwingHeading.X * cameraSwingMagnitude), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                camera.Y = (Lerp(camera.Y, camera.Y + (cameraSwingHeading.Y * cameraSwingMagnitude), cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            else
            {
                camera.X = (Lerp(camera.X, mainCamera.X, cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));
                camera.Y = (Lerp(camera.Y, mainCamera.Y, cameraLerpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds));
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

        public void CameraShake(GameTime gameTime)
        {
            cameraShakeDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            //cameraTimePerShake += (float)gameTime.ElapsedGameTime.TotalSeconds;

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

        public Vector2 BackUp(Rectangle playerCollider, Rectangle squareCollider, Vector2 pos, Vector2 previousPos)
        {
            Vector2 tempPos = pos;
            if (Vector2.Distance(pos, previousPos) < 1)
            {
                return previousPos;
            }

            playerCollider.X = (int)pos.X;
            playerCollider.Y = (int)pos.Y;

            while (squareCollider.Intersects(playerCollider))
            {
                tempPos.Y = pos.Y + (-1);
                playerCollider.X = (int)tempPos.X;
                playerCollider.Y = (int)tempPos.Y;
            }

            

            //if (!playerCollider.Intersects(squareCollider))
            //{
            //    return pos;
            //}
            //else
            //{
            //    tempPos = pos + Vector2.Normalize(previousPos - pos);
            //}

            

            //return BackUp(playerCollider, squareCollider, tempPos, previousPos);

            return tempPos;
        }

        public bool IsTouching(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Right >= rect2.Left && rect1.Top <= rect2.Bottom && rect1.Bottom >= rect2.Top && rect1.Left <= rect2.Right)
            {
                return true;
            }
            return false;
        }

        public bool IsTouching(Rectangle rect1, Rectangle rect2, Side side)
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

        public float DistanceSquared(Point p1, Point p2)
        {
            int tempX = (p2.X - p1.X);
            int tempY = (p2.Y - p1.Y);
            return (tempX * tempX) + (tempY * tempY);
            //return ((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y));
        }

        public void Debug()
        {
            debug = this.IsMouseVisible = !debug;
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

        public bool GetKey(Keys key)
        {
            if (keyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public bool GetKeyDown(Keys key)
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

        public bool Use()
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

        public float Lerp(float value1, float value2, float amount)
        {
            if (amount > 1)
            {
                return value2;
            }
            return value1 + (value2 - value1) * amount;
        }

        public void consoleParser(string line)
        {
            if (debug) { developerConsole1.WriteLine("     line: " + line); }
            line.Trim();
            //line = line.ToLower();
            //if (debug) { Console.WriteLine(" trimline: " + line); }
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

                if (debug)
                {
                    if (string.IsNullOrWhiteSpace(extra))
                    {
                        developerConsole1.WriteLine("statement: " + statement);
                        developerConsole1.WriteLine(" variable: " + variable);
                        developerConsole1.WriteLine("    value: " + value);
                        developerConsole1.WriteLine("  restate: " + variable + "=" + value);
                    }
                    else
                    {
                        developerConsole1.WriteLine("statement: " + statement);
                        developerConsole1.WriteLine(" variable: " + variable);
                        developerConsole1.WriteLine("    extra: " + extra);
                        developerConsole1.WriteLine("    value: " + value);
                        developerConsole1.WriteLine("  restate: " + variable + "[" + extra + "]=" + value);
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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "attack2damage":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.attack2Damage = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "speed":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.speed = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "jumptimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.jumpTimeMax = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "idletimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.idleTimeMax = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "superShockwaveHoldtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.superShockwaveHoldtime = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavemaxeffectdistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveMaxEffectDistance = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwaveeffectivedistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveEffectiveDistance = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavestuntime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shockwaveStunTime = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "invulnerablemaxtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.invulnerableMaxTime = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldRechargeRate = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "energyrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.energyRechargeRate = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "healthrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.healthRechargeRate = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "energyusablemargin":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.energyUsableMargin = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "minsqrdetectdistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            minSqrDetectDistance = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;

                    case "shieldanimationspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldAnimationSpeed = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldhealingpercentage":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.shieldHealingPercentage = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "cameralerpspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            cameraLerpSpeed = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "terminalvelocity":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.terminalVelocity = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "masteraudiolevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            masterAudioLevel = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "musiclevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            musicLevel = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "soundeffectslevel":
                        if (float.TryParse(value, out floatResult))
                        {
                            soundEffectsLevel = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.potionRechargeRate = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionrechargetime":
                        if (float.TryParse(value, out floatResult))
                        {
                            geralt.potionRechargeTime = floatResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "ycollideroffset":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.YcolliderOffset = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "colliderwidth":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.colliderWidth = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "colliderheight":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.colliderHeight = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "attackcolliderwidth":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.attackColliderWidth = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "attackcolliderheight":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.attackColliderHeight = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "screenscale":
                        if (int.TryParse(value, out intResult))
                        {
                            screenScale = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "maxnumberofpotions":
                        if (int.TryParse(value, out intResult))
                        {
                            geralt.maxNumberOfPotions = intResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;





                    case "attackkey":                                                               //place new ints above
                        if (Enum.TryParse(value, out keyResult))
                        {
                            attackKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altattackkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altAttackKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shockwavekey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            shockwaveKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altshockwavekey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altShockwaveKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "shieldkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            shieldKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altshieldkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altShieldKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "jumpkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            jumpKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altjumpkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altJumpKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "upkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            upKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altupkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altUpKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "downkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            downKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altdownkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altDownKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "leftkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            leftKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altleftkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altLeftKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "rightkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            rightKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altrightkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altRightKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "rollkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            rollKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altrollkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altRollKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "potionkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            potionKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "altpotionkey":
                        if (Enum.TryParse(value, out keyResult))
                        {
                            altPotionKey = keyResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }

                        break;





                    case "cameralerp":                                                                //place new "etc variable" (like vectors and rectangles) above
                        if (bool.TryParse(value, out boolResult))
                        {
                            cameraLerp = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "camerashakesetting":
                        if (bool.TryParse(value, out boolResult))
                        {
                            cameraShakeSetting = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "fullscreen":
                        if (bool.TryParse(value, out boolResult))
                        {
                            graphics.IsFullScreen = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "verticalretrace":
                        if (bool.TryParse(value, out boolResult))
                        {
                            graphics.SynchronizeWithVerticalRetrace = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "Isfixedtimestep":
                        if (bool.TryParse(value, out boolResult))
                        {
                            IsFixedTimeStep = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

                        }
                        break;
                    case "summonenemy":
                        eList.Add(new Enemy(enemy0Tex, new Vector2(20f, 5f), this));
                        break;
                    case "summonenemies":
                        if (int.TryParse(value, out intResult))
                        {
                            for (int i = intResult; i > 0; i--)
                            {
                                eList.Add(new Enemy(enemy0Tex, new Vector2(20f, 5f), this));
                            }
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");

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
                        break;







                    case "debug":                                                                     //place new bools above
                        if (bool.TryParse(value, out boolResult))
                        {
                            debug = boolResult;
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(extra))
                        {
                            developerConsole1.WriteLine("error: no variable with name: \"" + variable + "\"");
                        }
                        else
                        {
                            developerConsole1.WriteLine("error: no variable with name: \"" + variable + "[" + extra + "]\"");
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
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, background);
            //spritebatch for BACKGROUNDS
            if (debug)
            {
                foreach (Square b in backgroundSquareList)
                {
                    b.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
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
            if (debug) { screenspace.Draw(spriteBatch, nullTex); }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
            //spritebatch for static elements (like UI and menus, etc)
            if (healthBar != null) { healthBar.Draw(spriteBatch); }
            if (shieldBar != null) { shieldBar.Draw(spriteBatch); }
            if (energyBar != null) { energyBar.Draw(spriteBatch); }
            if (potionBar != null) { potionBar.Draw(spriteBatch); }
            if (displayEnemyHealth) { enemyHealthBar.Draw(spriteBatch); }
            if (timerDisplay != null) { timerDisplay.Draw(spriteBatch); }

            if (debug)
            {
                developerConsole.Draw(spriteBatch);
                developerConsole1.Draw(spriteBatch);
            }
            debuginfo.Draw(spriteBatch);
            if (debug)
            {
                foreach (Button b in buttonList)
                {
                    b.Draw(spriteBatch);
                }
            }
            if (debug) { boundingBoxBorder.Draw(spriteBatch, nullTex); }
            spriteBatch.End();
            // --------------------------------------------------------------------------------------------------------------------------------

            if (sceneIsMenu)        //FOR MENU DRAWING
            {
                // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
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
