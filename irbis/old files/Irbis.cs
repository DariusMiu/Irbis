using System;
using System.IO;
using System.Text;
using Microsoft.Win32;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace Irbis
{
    public enum Difficulty
    {
        Easy = 1,
        Medium = 2,
        Hard = 3,
        Insane = 4,
    }
    public enum Side
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3,
    }
    public enum Direction
    {
        Forward = 0,
        Left = 1,
        Right = 2,
    }
    public enum Location
    {
        Ground = 0,
        Air = 1,
        Water = 2,
    }
    public enum Activity
    {
        Idle = 0,
        Running = 1,
        Jumping = 2,
        Rolling = 3,
        Falling = 4,
        Landing = 5,
        Attacking = 6,
        Slamming = 7,
        Shockwave = 8,
        Zapped = 9,
        Dying = 21,
        Dead = 22,
    }
    public enum Attacking
    {
        No = 0,
        Attack1 = 1,
        Attack2 = 2,
        Slam = 3,
        Shockwave = 4,
        AirShockwave = 5,
    }
    public enum AI
    {
        Wander = 0,
        Patrol = 1,
        Seek = 2,
        Combat = 3,
        Stunned = 4,
        Persue = 5,
    }
    public enum EnchantType
    {
        Bleed = 0,          // long duration, low damage                 stacks:                             upgrade: longer duration, more damage/second
        Fire = 1,           // short duration, high damage               stacks:                             upgrade: longer duration, more damage/second
        Frost = 2,          // long duration, slows                      stacks:                             upgrade: longer duration
        Knockback = 3,      // knocks enemies back short distance        stacks:                             upgrade: knocks back further
        Poison = 4,         // long duration, high damage, limited use   stacks:                             upgrade: more uses
        Sharpness = 5,      // flat damage increase (no duration)        stacks:                             upgrade: more damage
        Stun = 6,           // short duration, full stun                 stacks:                             upgrade: longer duration
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
    public enum BossState
    {
        Spawn = 0,
        Entrance = 1,
        Engage = 2,
        Combat = 3,
        Disengage = 4,
        Death = 5,
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
        { get; }
        Vector2 Velocity
        { get; }
    }
    public interface IEnemy : ICollisionObject
    {
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
        string Name
        {
            get;
        }
        bool AIenabled
        {
            get;
            set;
        }
        float Mass
        {
            get;
        }
        float StunTime
        {
            get;
        }
        bool Update();
        void ThreadPoolCallback(Object threadContext);
        bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack, Vector2 Damage);
        bool Enemy_OnPlayerShockwave(Vector2 Origin, float Range, Attacking Attack, float Power);
        void AddEffect(Enchant effect);
        void UpgradeEffect(int index, float duration);
        void Hurt(float damage);
        void Stun(float duration);
        void Draw(SpriteBatch sb);
        void Light(SpriteBatch sb, bool UseColor);
        void Death();
    }



    public class Irbis : Game
    {                                                                                               // version info
        /// version number key (two types): 
        /// release number . software stage (pre/alpha/beta) . build/version . build iteration
        /// release number . content patch number . software stage . build iteration
        public const string versionNo = "0.2.1.3";
        public const string versionID = "beta";
        public const string versionTy = "debug";
        /// Different version types: 
        /// debug
        /// release candidate
        /// release

                                                                                                    // debug
        public static int debug = 0;
        public static bool Crash = true;
        private static Print debuginfo;
        private static Print topright;
        private static SmartFramerate smartFPS;
        private static SmoothFramerate smoothFPS;
        private static int smoothUpdate;
        private static string smoothDisplay;
        private static string medianDisplay;
        private static string minDisplay;
        private static string maxDisplay;
        public static bool framebyframe;
        public static bool nextframe;
        private static TotalMeanFramerate meanFPS;
        private static double minFPS;
        private static double minFPStime;
        private static double maxFPS;
        private static double maxFPStime;
        private static bool recordFPS;
        private static int framedropfactor = 10;

                                                                                                    // console
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
        private static object writelock = new object();

                                                                                                    // properties
        public static float DeltaTime
        {
            get
            { return deltaTime; }
        }
        private static float deltaTime;
        public static float RealTime
        {
            get
            { return realTime; }
        }
        private static float realTime;
        public static double Timer
        {
            get
            {
                return timer;
            }
        }
        private static double timer;
        private static double elapsedTime;
        private static float timeScale = 1f;


        public static KeyboardState GetKeyboardState
        {
            get
            {
                return keyboardState;
            }
        }
        public static KeyboardState GetPreviousKeyboardState
        {
            get
            {
                return previousKeyboardState;
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
        public static GamePadState GetGamePadState
        {
            get
            {
                return gamePadState;
            }
        }
        public static GamePadState GetPreviousGamePadState
        {
            get
            {
                return previousGamePadState;
            }
        }
        public static bool GetPauseKey
        {
            get
            {
                return (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.IsButtonDown(Buttons.Start));
            }
        }
        public static bool GetBackKey
        {
            get
            {
                return (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.IsButtonDown(Buttons.Back));
            }
        }
        public static bool GetUseKey
        {
            get
            {
                return (keyboardState.IsKeyDown(useKey) || keyboardState.IsKeyDown(altUseKey) || gamePadState.IsButtonDown(GPuseKey));
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
                return (keyboardState.IsKeyDown(attackKey) || keyboardState.IsKeyDown(altAttackKey) || gamePadState.IsButtonDown(GPattackKey));
            }
        }
        public static bool GetShockwaveKey
        {
            get
            {
                return (keyboardState.IsKeyDown(shockwaveKey) || keyboardState.IsKeyDown(altShockwaveKey) || gamePadState.IsButtonDown(GPshockwaveKey));
            }
        }
        public static bool GetShieldKey
        {
            get
            {
                return (keyboardState.IsKeyDown(shieldKey) || keyboardState.IsKeyDown(altShieldKey) || gamePadState.IsButtonDown(GPshieldKey));
            }
        }
        public static bool GetJumpKey
        {
            get
            {
                return (keyboardState.IsKeyDown(jumpKey) || keyboardState.IsKeyDown(altJumpKey) || gamePadState.IsButtonDown(GPjumpKey));
            }
        }
        public static bool GetUpKey
        {
            get
            {
                return (keyboardState.IsKeyDown(upKey) || keyboardState.IsKeyDown(altUpKey) || gamePadState.IsButtonDown(GPupKey));
            }
        }
        public static bool GetDownKey
        {
            get
            {
                return (keyboardState.IsKeyDown(downKey) || keyboardState.IsKeyDown(altDownKey) || gamePadState.IsButtonDown(GPdownKey));
            }
        }
        public static bool GetLeftKey
        {
            get
            {
                return (keyboardState.IsKeyDown(leftKey) || keyboardState.IsKeyDown(altLeftKey) || gamePadState.IsButtonDown(GPleftKey)
                     /*|| gamePadState.ThumbSticks.Left.X < -analogCutoff*/);
            }
        }
        public static bool GetRightKey
        {
            get
            {
                return (keyboardState.IsKeyDown(rightKey) || keyboardState.IsKeyDown(altRightKey) || gamePadState.IsButtonDown(GPrightKey)
                    /*|| gamePadState.ThumbSticks.Left.X > analogCutoff*/);
            }
        }
        public static float GetLeftStickX
        {
            get
            { return gamePadState.ThumbSticks.Left.X; }
        }
        public static float GetLeftStickY
        {
            get
            { return gamePadState.ThumbSticks.Left.Y; }
        }
        public static float GetRightStickX
        {
            get
            { return gamePadState.ThumbSticks.Right.X; }
        }
        public static float GetRightStickY
        {
            get
            { return gamePadState.ThumbSticks.Right.Y; }
        }
        public static bool GetPotionKey
        {
            get
            {
                return (keyboardState.IsKeyDown(potionKey) || keyboardState.IsKeyDown(altPotionKey) || gamePadState.IsButtonDown(GPpotionKey));
            }
        }
        public static bool GetRollKey
        {
            get
            {
                return (keyboardState.IsKeyDown(rollKey) || keyboardState.IsKeyDown(altRollKey) || gamePadState.IsButtonDown(GProllKey));
            }
        }
        public static bool GetPauseKeyDown
        {
            get
            {
                return (GetKeyDown(Keys.Escape) || GetButtonDown(Buttons.Start));
            }
        }
        public static bool GetBackKeyDown
        {
            get
            {
                return (GetKeyDown(Keys.Escape) || GetButtonDown(Buttons.Back));
            }
        }
        public static bool GetUseKeyDown
        {
            get
            {
                return (GetKeyDown(useKey) || GetKeyDown(altUseKey) || GetButtonDown(GPuseKey));
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
                return (GetKeyDown(attackKey) || GetKeyDown(altAttackKey) || GetButtonDown(GPattackKey));
            }
        }
        public static bool GetShockwaveKeyDown
        {
            get
            {
                return (GetKeyDown(shockwaveKey) || GetKeyDown(altShockwaveKey) || GetButtonDown(GPshockwaveKey));
            }
        }
        public static bool GetShieldKeyDown
        {
            get
            {
                return (GetKeyDown(shieldKey) || GetKeyDown(altShieldKey) || GetButtonDown(GPshieldKey));
            }
        }
        public static bool GetJumpKeyDown
        {
            get
            {
                return (GetKeyDown(jumpKey) || GetKeyDown(altJumpKey) || GetButtonDown(GPjumpKey));
            }
        }
        public static bool GetUpKeyDown
        {
            get
            {
                return (GetKeyDown(upKey) || GetKeyDown(altUpKey) || GetButtonDown(GPupKey));
            }
        }
        public static bool GetDownKeyDown
        {
            get
            {
                return (GetKeyDown(downKey) || GetKeyDown(altDownKey) || GetButtonDown(GPdownKey));
            }
        }
        public static bool GetLeftKeyDown
        {
            get
            {
                return (GetKeyDown(leftKey) || GetKeyDown(altLeftKey) || GetButtonDown(GPleftKey)
                    /*|| (gamePadState.ThumbSticks.Left.X < -analogCutoff && previousGamePadState.ThumbSticks.Left.X >= analogCutoff)*/);
            }
        }
        public static bool GetRightKeyDown
        {
            get
            {
                return (GetKeyDown(rightKey) || GetKeyDown(altRightKey) || GetButtonDown(GPrightKey)
                    /*|| (gamePadState.ThumbSticks.Left.X > analogCutoff && previousGamePadState.ThumbSticks.Left.X <= -analogCutoff)*/);
            }
        }
        public static bool GetPotionKeyDown
        {
            get
            {
                return (GetKeyDown(potionKey) || GetKeyDown(altPotionKey) || GetButtonDown(GPpotionKey));
            }
        }
        public static bool GetRollKeyDown
        {
            get
            {
                return (GetKeyDown(rollKey) || GetKeyDown(altRollKey) || GetButtonDown(GProllKey));
            }
        }
        public static bool GetLeftMouseDown
        {
            get
            {
                return (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released);
            }
        }
        public static bool GetPauseKeyUp
        {
            get
            {
                return (GetKeyUp(Keys.Escape) || GetButtonUp(Buttons.Start));
            }
        }
        public static bool GetBackKeyUp
        {
            get
            {
                return (GetKeyUp(Keys.Escape) || GetButtonUp(Buttons.Back));
            }
        }
        public static bool GetUseKeyUp
        {
            get
            {
                return (GetKeyUp(useKey) || GetKeyUp(altUseKey) || GetButtonUp(GPuseKey));
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
                return (GetKeyUp(attackKey) || GetKeyUp(altAttackKey) || GetButtonUp(GPattackKey));
            }
        }
        public static bool GetShockwaveKeyUp
        {
            get
            {
                return (GetKeyUp(shockwaveKey) || GetKeyUp(altShockwaveKey) || GetButtonUp(GPshockwaveKey));
            }
        }
        public static bool GetShieldKeyUp
        {
            get
            {
                return (GetKeyUp(shieldKey) || GetKeyUp(altShieldKey) || GetButtonUp(GPshieldKey));
            }
        }
        public static bool GetJumpKeyUp
        {
            get
            {
                return (GetKeyUp(jumpKey) || GetKeyUp(altJumpKey) || GetButtonUp(GPjumpKey));
            }
        }
        public static bool GetUpKeyUp
        {
            get
            {
                return (GetKeyUp(upKey) || GetKeyUp(altUpKey) || GetButtonUp(GPupKey));
            }
        }
        public static bool GetDownKeyUp
        {
            get
            {
                return (GetKeyUp(downKey) || GetKeyUp(altDownKey) || GetButtonUp(GPdownKey));
            }
        }
        public static bool GetLeftKeyUp
        {
            get
            {
                return (GetKeyUp(leftKey) || GetKeyUp(altLeftKey) || GetButtonUp(GPleftKey)
                    /*|| (gamePadState.ThumbSticks.Left.X >= -analogCutoff && previousGamePadState.ThumbSticks.Left.X < -analogCutoff)*/);
            }
        }
        public static bool GetRightKeyUp
        {
            get
            {
                return (GetKeyUp(rightKey) || GetKeyUp(altRightKey) || GetButtonUp(GPrightKey)
                    /*|| (gamePadState.ThumbSticks.Left.X <= analogCutoff && previousGamePadState.ThumbSticks.Left.X > analogCutoff)*/);
            }
        }
        public static bool GetPotionKeyUp
        {
            get
            {
                return (GetKeyUp(potionKey) || GetKeyUp(altPotionKey) || GetButtonUp(GPpotionKey));
            }
        }
        public static bool GetRollKeyUp
        {
            get
            {
                return (GetKeyUp(rollKey) || GetKeyUp(altRollKey) || GetButtonUp(GProllKey));
            }
        }
        public static bool RandomBool
        {
            get
            { return (RAND.NextDouble() > 0.5d); }
        }
        /// <summary>
        /// returns random float between 0 and 1
        /// </summary>
        public static float RandomFloat
        {
            get
            { return (float)RAND.NextDouble(); }
        }
        public static Point WorldSpaceMouseLocation
        {
            get
            { return ((mouseState.Position.ToVector2() / screenScale) + (mainCamera / screenScale) - (boundingBox.Center.ToVector2() / screenScale)).ToPoint(); }
        }

        public static Rectangle DefaultBoundingBox
        {
            get
            {
                Rectangle tempRect = Rectangle.Empty;
                tempRect.Width = (int)(resolution.X * (1f / 3f));
                tempRect.Height = (int)(resolution.Y * (1f / 4f));
                tempRect.X = (int)(halfResolution.X) - tempRect.Width / 2;
                tempRect.Y = (int)(resolution.Y * 0.64f) - tempRect.Height / 2;
                return tempRect;
            }
        }
        public static float DefaultScreenScale
        {
            get
            { return resolution.X / 480f; }
        }

        /// <summary>
        /// returns true if there has been input during this frame
        /// </summary>
        public static bool Input
        {
            get
            {
                return (GetKeyboardState != GetPreviousKeyboardState) || (GetGamePadState != GetPreviousGamePadState)
                    || (mouseState.LeftButton != previousMouseState.LeftButton)
                    || (mouseState.MiddleButton != previousMouseState.MiddleButton)
                    || (mouseState.RightButton != previousMouseState.RightButton)
                    || (mouseState.XButton1 != previousMouseState.XButton1)
                    || (mouseState.XButton2 != previousMouseState.XButton2);
            }
        }

                                                                                                    // graphics
        public static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;
        public static RenderTarget2D sceneRenderTarget;
        private static RenderTarget2D lightingRenderTarget;
        private static RenderTarget2D coloredLightingRenderTarget;
        public static float darkness = 0.5f;
        public static bool lightingEnabled = true;

                                                                                                    // save info
        private static string autosave;
        public static SaveFile savefile;
        public static bool isMenuScrollable;
        public static int maxButtonsOnScreen;
        public static int levelListCounter;
        public static string currentLevel;

                                                                                                    // lists
        private static List<Square> backgroundSquareList;
        private static Color backgroundColor = Color.CornflowerBlue;
        public static List<ICollisionObject> collisionObjects;
        public static List<Square> sList;
        public static List<Square> squareList;
        public static List<Button> buttonList;
        public static List<IEnemy> enemyList;
        public static List<Print> printList;    //₯
        public static List<UIElementSlider> sliderList;
        public static string[] levelList;

                                                                                                    // player
        public static Player jamie;
        public static Vector2 initialPos = new Vector2(156, 93);
        public static float analogCutoff = 0.1f;

                                                                                                    // onslaught
        public static bool onslaughtMode;
        public static OnslaughtSpawner onslaughtSpawner;
        private static Print onslaughtDisplay;
        public static int vendingMachineUseDistanceSqr;

                                                                                                    // camera

        private static Vector2 camera;
        private static Vector2 mainCamera;
        private static Vector2 screenSpacePlayerPos;
        private static Matrix background;
        private static Matrix foreground;
        private static Matrix UIground;
        public static Rectangle boundingBox;
        public static Point halfBoundingBox;
        public static Rectangle screenspace;
        public static Rectangle zeroScreenspace;
        public static float screenScale;
        public static int textScale;
        public static Point resolution;
        public static bool cameraLerp;
        public static bool cameraLerpSetting;
        public static float cameraLerpXSpeed;
        public static float cameraLerpYSpeed;
        public static bool cameraShakeSetting;
        private static float cameraShakeDuration;
        private static float cameraShakeMagnitude;
        private static float cameraSwingDuration;
        private static float cameraSwingMaxDuration;
        private static float cameraSwingMagnitude;
        private static float cameraShakeLerpTime;
        private static float cameraShakePercentage;
        private static float cameraShakeLerpTimeMax;
        private static Vector2 cameraShakeTargetLocation;
        private static Vector2 cameraShakePrevLocation;
        public static float swingDuration;
        public static float swingMagnitude;
        static Vector2 cameraSwingHeading;
        public static bool cameraSwingSetting;
        public static Vector2 cameraLockon;
        public static float cameraLockonFactor = 0.1f;
        public static float cameraLockonSpeed = 3;
        public static Vector2 cameraLockonLocation;

                                                                                                    // menu
        public static Menu menu;
        private static Texture2D cf;
        private static Texture2D[] menuTex;
        public static int menuSelection;
        public static bool listenForNewKeybind;
        public static bool resetRequired;
        public static int levelLoaded;
        public static int scene = -1;
        public static bool sceneIsMenu;
        public static VendingMenu vendingMachineMenu;
        public static bool justLeftMenu;
        public static Print patreonPrint;
        public static int lastMenuScene = 0;
        public static bool startup = true;

                                                                                                    // leveleditor
        public static bool levelEditor;
        int selectedBlock;
        int lastEditedBlock;
        Texture2D selectedTexture;
        static List<Texture2D> availableTextures;
        static Rectangle texturePanel;

                                                                                                    // enemy/AI variables
        public static bool AIenabled;
        Texture2D enemy0Tex;
        private Vector2 bossSpawn;
        private string bossName;
        public List<Vector2> enemySpawnPoints;

                                                                                                    // UI
        public static Font font;
        public static Bars bars;
        public static Print timerDisplay;
        public static string timerAccuracy;
        public static float minSqrDetectDistance;
        public static bool displayEnemyHealth;
        public static int vendingMenu;
        public static bool displayUI = true;
        public static Texture2D[] fontLogos = new Texture2D[3];

                                                                                                    // credits/intro vars
        public static bool rollCredits;
        public static float intro = 3f;
        public static bool creditsActive = true;
        public static string credits;
        public static Vector2 creditsCurrentPos;
        public static float creditsOpacity;
        public static Print creditsPrint;
        public static float creditsSpeed;
        public static Texture2D l;
        public static Texture2D n;
        public static Texture2D liquidnitrogen;



                                                                                                    // settings vars
        public static Point halfResolution;
        public static Point tempResolution;
        public static float masterAudioLevel;
        public static float musicLevel;
        public static float soundEffectsLevel;
        public float randomTimer;
        public static int sliderPressed;
        public static bool easyWalljumpMode = true;
        public static bool smartCamera = true;
        public static float cameraJumpCameraCutoff = -50;

                                                                                                    // keyboard/keys
        private static KeyboardState keyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState mouseState;
        private static MouseState previousMouseState;
        private static GamePadState gamePadState;
        private static GamePadState previousGamePadState;
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
        public static Buttons GPattackKey;
        public static Buttons GPshockwaveKey;
        public static Buttons GPshieldKey;
        public static Buttons GPjumpKey;
        public static Buttons GPupKey;
        public static Buttons GPdownKey;
        public static Buttons GPleftKey;
        public static Buttons GPrightKey;
        public static Buttons GProllKey;
        public static Buttons GPpotionKey;
        public static Buttons GPuseKey;


                                                                                                    // threading
        private static int threadCount;
        private static bool useMultithreading = false;
        public static ManualResetEvent doneEvent;
        public static int pendingThreads;
        public static ManualResetEvent loadEvent = new ManualResetEvent(false);
        public static int loadingThreads;
        private static object listLock = new object();
        private static object texLock = new object();
        private static object pngLock = new object();

                                                                                                    // events
        public delegate bool AttackEventDelegate(Rectangle AttackCollider, Attacking Attack, Vector2 Damage);
        public delegate bool ShockwaveEventDelegate(Vector2 Origin, float Range, Attacking Attack, float Power);


                                                                                                    // etc
        public static float gravity;
        private static Random RAND;
        public static Texture2D nullTex;
        public static Texture2D largeNullTex;
        public static Texture2D defaultTex;
        public static Irbis game;
        private static float nextFrameTimer;
        private static BasicEffect basicEffect;
        //private static BasicEffect lighting;
        private static Matrix projection = Matrix.Identity;
        private static Ray[] debugrays = new Ray[50];
        private static Line[] debuglines = new Line[5];
        private static Shape[] debugshapes = new Shape[4];
        private static Shape shadowShape;
        private static List<Vector2> shadows;
        public static TooltipGenerator tooltipGenerator;
        public static List<Song> music;
        public static List<string> musicList;
        public static List<Texture2D> logos;
        public static Rectangle testRectangle = new Rectangle(300, 500, 0, 0);
        public static List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        public static List<Grass> grassList = new List<Grass>();
        public static Texture2D dottex;
        public static Texture2D explosiontex;
        public static Print infoText;
        private static float infoTextTimer;
        private static float infoTextTimerMax = 5f;
        public static Texture2D originTexture;
        public static float deathscreenTimer;
        public static float deathscreenTimerMax = 5f;
        public static float deathBackgroundOpacity = 1;
        public static UIElementSlider loadingBar;
        public static float deathTimeScale;
        //static Torch torch;



        public static BlendState multiplicativeBlend = new BlendState
        {
            AlphaBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,

            ColorBlendFunction = BlendFunction.Add,
            ColorDestinationBlend = Blend.InverseSourceColor,
        };


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

            Console.WriteLine("Number of logical processors:" + threadCount);
            Console.WriteLine(".Net: " + GetDotNet());

            sList = new List<Square>();
            collisionObjects = new List<ICollisionObject>();
            squareList = new List<Square>();
            backgroundSquareList = new List<Square>();
            buttonList = new List<Button>();
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
            /*//lighting.EnableDefaultLighting();
            lighting = new BasicEffect(graphics.GraphicsDevice);
            lighting.LightingEnabled = true;      // red  green  blue
            lighting.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            lighting.DiffuseColor      = new Vector3(0.1f, 0.1f, 0.8f);
            lighting.World = Matrix.Identity;
            lighting.View = Matrix.Identity;/**/

            cameraShakeLerpTimeMax = 0.025f;
            cameraShakeLerpTime = 0f;
            levelLoaded = -1;

            RAND = new Random();
            displayEnemyHealth = false;

            Window.TextInput += TextEntered;
            onTextEntered += HandleInput;
            acceptTextInput = false;
            textInputBuffer = "";

            keyboardState = Keyboard.GetState();
            previousKeyboardState = keyboardState;
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            gamePadState = GamePad.GetState(PlayerIndex.One);
            previousGamePadState = gamePadState;

            //EventInput.EventInput.Initialize(this.Window);
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
            Console.WriteLine("loading content");
            l = Content.Load<Texture2D>("l");
            n = Content.Load<Texture2D>("n");
            liquidnitrogen = Content.Load<Texture2D>("liquidnitrogen");
            defaultTex = Content.Load<Texture2D>("defaultTex");
            nullTex = Content.Load<Texture2D>("nullTex");


            Texture2D fontTex2 = Content.Load<Texture2D>("font2");
            Font font2 = new Font(fontTex2, 8);
            //consoleTex = LoadTexture("console texture");
            consoleTex = null;
            developerConsole = new Print(font2);
            PrintVersion();
            Irbis.WriteLine("Number of logical processors:" + threadCount);
            Irbis.WriteLine(".Net: " + GetDotNet());
            Irbis.WriteLine();

            spriteBatch = new SpriteBatch(GraphicsDevice);
            tooltipGenerator = new TooltipGenerator(game);

            vendingMenu = -1;

            dottex = LoadTexture("lazordot");
            explosiontex = LoadTexture("explosion");

            PlayerSettings playerSettings = new PlayerSettings(true);
            if (File.Exists(@".\content\playerSettings.ini"))
            {
                Console.WriteLine("loading playerSettings.ini...");
                playerSettings.Load(@".\content\playerSettings.ini");
            }
            else
            {
                Console.WriteLine("creating new playerSettings.ini...");
                WriteLine("creating new playerSettings.ini...");
                playerSettings = new PlayerSettings(true);
                Console.WriteLine("saving playerSettings.ini...");
                playerSettings.Save(@".\content\playerSettings.ini");
            }

            Load(playerSettings);

            autosave = ".\\content\\autosave.snep";
            savefile = new SaveFile(true);
            if (File.Exists(autosave))
            {
                Console.WriteLine("loading autosave.snep...");
                savefile.Load(autosave);
            }
            else
            {
                Console.WriteLine("creating new autosave.snep...");
                WriteLine("creating new autosave.snep...");
                savefile = new SaveFile(false);
                Console.WriteLine("saving autosave.snep...");
                savefile.Save(autosave);
            }

            sceneRenderTarget = new RenderTarget2D(GraphicsDevice, resolution.X, resolution.Y);
            lightingRenderTarget = new RenderTarget2D(GraphicsDevice, resolution.X, resolution.Y);
            coloredLightingRenderTarget = new RenderTarget2D(GraphicsDevice, resolution.X, resolution.Y);

            developerConsole.textScale = textScale;
            font = new Font(LoadTexture("font"), playerSettings.characterHeight, playerSettings.characterWidth, false);

            Texture2D playerTex = LoadPNG("player");
            Texture2D shieldTex = LoadTexture("shield");
            originTexture = LoadTexture("originTexture");

            jamie = new Player(playerTex, shieldTex, playerSettings, 0.5f);
            jamie.Respawn(new Vector2(-1000f, -1000f));


            loadingBar = new UIElementSlider(Direction.Left, new Rectangle(0, (int)(resolution.Y - (font.charHeight + 1) * textScale), resolution.X, (int)((font.charHeight + 1) * textScale)), Point.Zero, Direction.Forward, 100, new Color(038, 118, 213), null, null, Color.Black, nullTex, null, null, true, font, false, 0.8f, 0.799f, 0.801f, 0.803f);


            enemySpawnPoints = new List<Vector2>();

            menu = new Menu();
            cf = LoadTexture("cf");

            topright = new Print(resolution.X, font, Color.White, false, new Point(resolution.X, 4 * textScale), Direction.Right, 0.95f);
            debuginfo = new Print(resolution.X, font2, Color.White, true, new Point(3, 3), Direction.Left, 0.95f);
            consoleWriteline = new Print(resolution.X, font2, Color.White, true, new Point(1, 5), Direction.Left, 1f);
            infoText = new Print(resolution.X - 32, font, Color.White, false, new Point(resolution.X - 32, resolution.Y - 26), Direction.Right, 0.5f);
            WriteLine("" + resolution);
            developerConsole.Update(resolution.X);
            developerConsole.scrollDown = false;
            consoleMoveTimer = 0f;
            console = false;

            //topright.Update("℗Patreon.com/Ln2");
            Debug(debug);

            particleSystems = new List<ParticleSystem>();
            //torch = new Torch(Point.Zero);

            //printList.Add(infoText);
            //printList.Add(topright);
            //printList.Add(debuginfo);
            //printList.Add(consoleWriteline);
            //printList.Add(developerConsole);

            smartFPS = new SmartFramerate(5);
            smoothFPS = new SmoothFramerate(100);

            basicEffect.View = Matrix.Identity;
            basicEffect.World = Matrix.Identity;
            basicEffect.Projection = projection;

            for (int i = 0; i < debuglines.Length; i++)
            { debuglines[i] = new Line(new Vector2(300f * i, 300f), new Vector2(100f * i, 500f)); }
            //debugrays[0] = new Ray(halfResolution.ToVector2() / screenScale, Vector2.One);
            for (int i = 0; i < debugrays.Length; i++)
            {
                float myAngleInRadians = (2f * (float)Math.PI) * ((float)i / 50f);
                Vector2 angleVector = new Vector2((float)Math.Cos(myAngleInRadians), -(float)Math.Sin(myAngleInRadians));
                debugrays[i] = new Ray(halfResolution.ToVector2() / screenScale, angleVector);
            }

            vendingMachineUseDistanceSqr = 2500;

            //loading logos
            logos = new List<Texture2D>();
            logos.Add(LoadTexture("ln2"));
            //logos.Add(LoadTexture("monogame"));
            logos.Add(LoadTexture("patreon"));
            patreonPrint = new Print(font.charHeight * 15 * textScale, font, Color.White, false, new Point(((logos.Count) * 72), zeroScreenspace.Height /*- 28*/ - ((textScale * font.charHeight) /*/ 2*/)), Direction.Left, 0.4f);
            patreonPrint.Update("patreon.com/Ln2", true);

            menuTex = new Texture2D[8];
            menuTex[0] = LoadTexture("Main Menu title");
            menuTex[1] = LoadTexture("Options title");
            menuTex[2] = LoadTexture("Keybinds title");
            menuTex[3] = LoadTexture("Camera title");
            menuTex[4] = LoadTexture("Video title");
            menuTex[5] = LoadTexture("Audio title");
            menuTex[6] = LoadTexture("Misc title");
            menuTex[7] = LoadTexture("Level Select title");

            largeNullTex = LoadTexture("largeNullTex");
            selectedTexture = defaultTex = LoadTexture("defaultTex");

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


            Thread menuload = new Thread(new ThreadStart(InitialMenuLoad));
            menuload.Start();
            //InitialMenuLoad();


            LoadMusic();
            PlaySong("bensound-relaxing", true);

            //WriteLine("Type 'help' for a few commands");
            Console.WriteLine("done.");
        }

        private void LoadScene(int SCENE)
        {
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
                    justLeftMenu = sceneIsMenu = false;
                    jamie.inputEnabled = false;
                    break;
                default:    //error
                    Console.WriteLine("Error. Scene ID " + scene + " does not exist.");
                    break;
            }
        }

        public void InitialMenuLoad()
        {
            loadEvent.WaitOne();
            LoadMenu(-1, 0, false);
            sceneIsMenu = true;
            deathBackgroundOpacity = 0;
            displayUI = false;
        }

        public void LoadMenu(int Scene, int startMenuLocation, bool loadSettings)
        {
            //jamie.inputEnabled = false;
            this.IsMouseVisible = true;
            lastMenuScene = scene = Scene;
            if (lastMenuScene < 0)
            { lastMenuScene = 0; }
            sceneIsMenu = true;
            if (!startup)
            {
                sList.Clear();
                printList.Clear();
                infoText.Clear();
                infoTextTimer = 0;
                buttonList.Clear();
                if (levelLoaded > 0)
                { logos = null; }
            }
            if (loadSettings)
            {
                PlayerSettings playerSettings = new PlayerSettings(true);
                playerSettings.Load(@".\content\playerSettings.ini");
                Load(playerSettings);
            }
            //sList.Add(new Square(menuTex[scene], Color.White, Point.Zero, menuTex[scene].Width, menuTex[scene].Height, false, true, true, 0.5f));
            if (resetRequired)
            {
                Print resettt = new Print(resolution.X - 132, font, Color.White, false, new Point(resolution.X - 32, resolution.Y - 26), Direction.Right, 0.5f);
                resettt.Update("Restart the game to apply resolution changes!");
                printList.Add(resettt);
            }


            //this is where all the crazy stuff happens
            menu.Create(scene);

            menuSelection = startMenuLocation;
            startup = false;
        }

        public void LoadLevel(string filename, bool loadUI)
        {
            creditsActive = rollCredits = !loadUI;
            displayUI = loadUI;
            LoadUI();

            Thread loadlevel = new Thread(new ParameterizedThreadStart(LoadLevel));
            loadlevel.Start(filename);
            //LoadLevel(filename);

            if (jamie != null)
            {
                jamie.Respawn(new Vector2(-1000f, -1000f));
                screenSpacePlayerPos = mainCamera = boundingBox.Center.ToVector2();
            }
            else
            { LoadPlayer(); }
        }

        public void LoadLevel(object data)
        {
            string filename = (string)data;
            loadingBar.Value = 0;
            Level thisLevel = new Level(true);
            try
            {
                if (filename.Length >= 4 && filename.Substring(filename.Length - 4, 4).ToLower().Equals(".lvl"))
                { filename = filename.Substring(filename.Length - 4, 4); }

                WriteLine("loading level: " + filename + ".lvl");
                if (!File.Exists(".\\levels\\" + filename + ".lvl"))
                { filename = "c1b1"; }

                thisLevel.Load(".\\levels\\" + filename + ".lvl");
            }
            catch (Exception e)
            {
                WriteLine("Error - level " + filename + " could not be loaded");
                WriteLine("Exception: " + e.Message + "\n");
                DisplayInfoText("Error - level " + filename + " could not be loaded");
                loadingBar.Value = 100;
                return;
            }
            loadingThreads = 3;
            if (!startup)
            {
                displayUI = true;
                loadEvent = new ManualResetEvent(false);
                this.IsMouseVisible = sceneIsMenu = !displayUI;
            }
            deathBackgroundOpacity = 1;
            loadingBar.Value += 1;
            ClearLevel();

            //Thread loadSquares = new Thread(new ParameterizedThreadStart(LoadSquares));
            //loadSquares.Start(thisLevel);
            //Thread loadBackgrounds = new Thread(new ParameterizedThreadStart(LoadBackgrounds));
            //loadBackgrounds.Start(thisLevel);

            LoadSquares(thisLevel);
            LoadBackgrounds(thisLevel);

            if (jamie != null)
            {
                jamie.Respawn(thisLevel.PlayerSpawn);
                screenSpacePlayerPos = mainCamera = boundingBox.Center.ToVector2();
            }
            else
            { LoadPlayer(); }

            scene = 11;
            currentLevel = filename;

            timeScale = 1f;

            initialPos = thisLevel.PlayerSpawn;
            enemySpawnPoints = new List<Vector2>(thisLevel.EnemySpawnPoints);
            loadingBar.Value += 1;
            //enemy0Tex = LoadTexture("enemy0");
            //Texture2D centerTex = LoadTexture("centerTexture");
            //loadingBar.Value += 1;
            collisionObjects.Add(jamie);
            loadingBar.Value += 1;
            if (thisLevel.isOnslaught)
            {
                onslaughtSpawner = new OnslaughtSpawner();

                Point[] vendingMachineLocations = thisLevel.VendingMachineLocations;
                VendingType[] vendingMachineTypes = thisLevel.VendingMachineTypes;
                string[] vendingMachineTextures = thisLevel.VendingMachineTextures;
                for (int i = 0; i < vendingMachineTextures.Length; i++)
                { onslaughtSpawner.vendingMachineList.Add(new VendingMachine(200, vendingMachineTypes[i], new Rectangle(vendingMachineLocations[i], new Point(64, 64)), LoadTexture(vendingMachineTextures[i]), 0.35f)); }

                onslaughtDisplay = new Print(resolution.Y / 2, font, Color.White, true, new Point(2, 7), Direction.Left, 0.6f);
                onslaughtDisplay.Update("Onslaught Wave " + onslaughtSpawner.wave, true);
                onslaughtMode = true;
            }
            else
            {
                onslaughtDisplay = null;
                onslaughtSpawner = null;
                onslaughtMode = false;
            }
            timerDisplay = new Print(resolution.Y / 2, font, Color.White, true, new Point(2, 7), Direction.Left, 0.6f);
            loadingBar.Value += 1;


            darkness = thisLevel.darkness;
            loadingBar.Value += 1;

            particleSystems = new List<ParticleSystem>(thisLevel.ParticleSystems);
            loadingBar.Value += 1;
            Grass.spriteBatch = new SpriteBatch(GraphicsDevice);
            grassList = new List<Grass>(/*thisLevel.Grasses*/);
            backgroundColor = thisLevel.BackgroundColor;
            backgroundColor = Color.CornflowerBlue;
            loadingBar.Value += 1;

            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle(-160, 259, 263, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle( 105, 259, 152, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle( 259, 259, 390, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle( 651, 259, 160, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle( 813, 259, 368, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle(1184, 259, 171, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            grassList.Add(new Grass(0f, 2.0f, 100, 0.5f, new float[] { 0.5f, 0.5f, 1, 0.1f }, -0.5f, -0.1f, new Vector2(3, 26), new Rectangle(1357, 259, 263, 10), LoadTexture("wizardgrass"), new Point(7, 29), null, 2));
            /*particleSystems.Add(new ParticleSystem(Vector2.Zero, new Vector2(-.1f,-.2f), new float[]{2,3,2}, new float[]{1,1,1,1},
                new float[]{2,2,2,2}, 0.75f, 0.8f, new float[]{0,1f,0,0.01f,0.01f,0.01f},new Rectangle(100,15,200,100),
                new Texture2D[]{LoadTexture("godray1")//,LoadTexture("godray2"),LoadTexture("godray3"),LoadTexture("godray4"),LoadTexture("godray5"),LoadTexture("godray6")
                },new Color[]{Color.Transparent,Color.White,Color.White},new Color[]{Color.Transparent,new Color(1f,1f,0.5f,1f),new Color(1f,1f,1f,.5f)},
                new int[]{0,0,0,0}, 5, 0, 2)); /**/

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
            loadingBar.Value += 2;

            //Thread loadBoss = new Thread(new ParameterizedThreadStart(LoadBoss));
            //loadBoss.Start(thisLevel);
            LoadBoss(thisLevel);


            justLeftMenu = true;
            if (loadingThreads <= 0) { loadEvent.Set(); }
            loadEvent.WaitOne();
            loadingBar.Value = 100;
        }

        public void LoadSquares(object data)
        {
            Level thisLevel = (Level)data;

            /*Point[] squareSpawns = thisLevel.SquareSpawnPoints;
            loadingBar.Value += 1;
            Rectangle?[] squareColliders = thisLevel.SquareColliders;
            loadingBar.Value += 1;
            float[] squareDepths;
            squareDepths = thisLevel.squareDepths.ToArray();
            loadingBar.Value += 1;

            float baradd = 57f / thisLevel.squareTextures.Count;
            for (int i = 0; i < thisLevel.squareTextures.Count; i++)
            {
                Square tempSquare;
                if (thisLevel.squareDepths[i] > 0)     // has collider, is displayed (usually debug)
                { tempSquare = new Square(LoadTexture(thisLevel.squareTextures[i]), Color.White, squareSpawns[i], squareColliders[i], Vector2.Zero, screenScale, thisLevel.squareDepths[i]); }
                else                                   // has collider, is not displayed (this is generally used after backgrounds are added)
                { tempSquare = new Square(LoadTexture(thisLevel.squareTextures[i]), Color.White, squareSpawns[i], squareColliders[i], Vector2.Zero, screenScale, null); }
                collisionObjects.Add(tempSquare);
                squareList.Add(tempSquare);
                loadingBar.Value += baradd;
            }/**/


            Square
            tempSquare = new Square(LoadPNG("environment//wizardground"), Color.White, new Point(-160, 260), null, Vector2.Zero, screenScale, 0.15f);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            tempSquare = new Square(LoadPNG("environment//wizardplatform1"), Color.White, new Point(100, 260), new Rectangle(5, 0, 152, 20), Vector2.Zero, screenScale, 0.1f);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            tempSquare = new Square(LoadPNG("environment//wizardplatform2"), Color.White, new Point(642, 260), new Rectangle(9, 0, 160, 20), Vector2.Zero, screenScale, 0.1f);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            tempSquare = new Square(LoadPNG("environment//wizardplatform3"), Color.White, new Point(1179, 260), new Rectangle(5, 0, 171, 20), Vector2.Zero, screenScale, 0.1f);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            tempSquare = new Square(nullTex, Color.Transparent, new Point(-180, 0), new Rectangle(0, 0, 20, 280), Vector2.Zero, screenScale, null);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            tempSquare = new Square(nullTex, Color.Transparent, new Point(1620, 0), new Rectangle(0, 0, 20, 280), Vector2.Zero, screenScale, null);
            collisionObjects.Add(tempSquare);
            squareList.Add(tempSquare);
            /**/

            if (Interlocked.Decrement(ref loadingThreads) <= 0)
            { loadEvent.Set(); }
        }

        public void LoadBackgrounds(object data)
        {
            Level thisLevel = (Level)data;

            Point[] BackgroundSquares = thisLevel.BackgroundSquares;
            loadingBar.Value += 1;

            float baradd = 18f / thisLevel.backgroundTextures.Count;
            for (int i = 0; i < thisLevel.backgroundTextures.Count; i++)                                                                                              //backgrounds
            {
                Texture2D squareTex; Square tempSquare;
                if (thisLevel.backgroundTextures[i] == "centerTexture")
                {
                    squareTex = LoadTexture(thisLevel.backgroundTextures[i]);
                    tempSquare = new Square(squareTex, Color.White, (halfResolution.ToVector2() / screenScale).ToPoint(),
                        null, squareTex.Bounds.Size.ToVector2() / 2, screenScale, thisLevel.backgroundSquareDepths[i]);
                }
                else
                {
                    squareTex = LoadTexture(thisLevel.backgroundTextures[i]);
                    tempSquare = new Square(squareTex, Color.White,
                        //(BackgroundSquares[i].ToVector2() + squareTex.Bounds.Size.ToVector2() / 2).ToPoint(),
                        BackgroundSquares[i],
                        null, squareTex.Bounds.Size.ToVector2() / 2, screenScale, thisLevel.backgroundSquareDepths[i]);
                }
                backgroundSquareList.Add(tempSquare);
                loadingBar.Value += baradd;
            }
            //{
            //    Texture2D squareTex = LoadTexture("cave");
            //    Square tempSquare = new Square(squareTex, new Point(300, squareTex.Bounds.Height / 2), screenScale, true, 1f);
            //    backgroundSquareList.Add(tempSquare);
            //}
            //{
            //    Texture2D squareTex = LoadTexture("sky");
            //    Square tempSquare = new Square(squareTex, new Point(300, squareTex.Bounds.Height / 2), screenScale, true, 0.9f);
            //    backgroundSquareList.Add(tempSquare);
            //}
            //{
            //    Texture2D squareTex = LoadTexture("dirt");
            //    Square tempSquare = new Square(squareTex, new Point(300, squareTex.Bounds.Height / 2), screenScale, true, 0.5f);
            //    backgroundSquareList.Add(tempSquare);
            //}

            if (Interlocked.Decrement(ref loadingThreads) <= 0)
            { loadEvent.Set(); }
        }

        public void LoadBoss(object data)
        {
            Level thisLevel = (Level)data;

            bossSpawn = thisLevel.BossSpawn;
            bossName = thisLevel.bossName;
            loadingBar.Value += 1;

            SummonBoss(bossName, bossSpawn);

            if (Interlocked.Decrement(ref loadingThreads) <= 0)
            { loadEvent.Set(); }
        }

        public void LoadUI()
        {
            bars = new Bars(LoadTexture("bar health"), LoadTexture("bar shield"), LoadTexture("bar energy"), LoadTexture("bar potion fill 1"), LoadTexture("bar enemy fill"), LoadTexture("shieldBar"), LoadTexture("bars"), LoadTexture("bar enemy"), new[] { LoadTexture("bar potion 1"), LoadTexture("bar potion 2"), LoadTexture("bar potion 3") });
            if (jamie == null)
            { LoadPlayer(); }
        }

        public static Texture2D LoadTexture(string TextureFile)
        {
            lock (texLock)
            {
                WriteLine("loading texture: " + TextureFile);
                if (string.IsNullOrWhiteSpace(TextureFile))
                { Write(" failed. (" + TextureFile + ")"); return defaultTex; }
                else if (!File.Exists(@".\content\" + TextureFile + ".xnb"))
                { return LoadPNG(TextureFile); }
                else
                { return game.Content.Load<Texture2D>(TextureFile); }
            }
        }

        public static Texture2D LoadPNG(string TextureFile)
        {
            lock (pngLock)
            {
                WriteLine("loading .png: " + TextureFile);

                if (TextureFile.Length >= 4 && TextureFile.Substring(TextureFile.Length - 4, 4).ToLower().Equals(".png"))
                { TextureFile = TextureFile.Substring(TextureFile.Length - 4, 4); }

                if (string.IsNullOrWhiteSpace(TextureFile) || !File.Exists(@".\content\textures\" + TextureFile + ".png"))
                { Write(" failed. (" + TextureFile + ")"); return defaultTex; }
                else
                {
                    FileStream fileStream = new FileStream(@".\content\textures\" + TextureFile + ".png", FileMode.Open);
                    Texture2D spriteAtlas = Texture2D.FromStream(graphics.GraphicsDevice, fileStream);
                    fileStream.Dispose();
                    spriteAtlas.Name = TextureFile;
                    WriteLine();
                    return spriteAtlas;
                }
            }
        }



        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific Content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager Content here
        }



        protected override void Update(GameTime gameTime)
        {
            pendingThreads = 0;
            doneEvent = new ManualResetEvent(false);
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            elapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            //if (debug > 0)
            //{
            if (useMultithreading)
            { QueueThread(new WaitCallback(DebugUpdate)); }
            else
            { DebugUpdate(null); }
            //}

            smartFPS.Update(elapsedTime);
            smoothFPS.Update(elapsedTime);
            realTime = (float)elapsedTime;
            if (elapsedTime > 0.2d)
            { deltaTime = 0.2f * timeScale; }
            else
            { deltaTime = (float)elapsedTime * timeScale; }

            if (rollCredits) // line 1594
            {
                if (intro <= 0)
                {
                    if (rollCredits && GetPauseKeyDown)
                    { PauseCredits(); }
                    else
                    { Credits(); }
                }
                else
                {
                    if (Input && loadingBar.Value >= 100)
                    { intro = 0; }
                    else if (DeltaTime <= 0.2f)
                    {
                        intro -= DeltaTime;
                        if (loadingBar.Value < 100 && intro < 2)
                        { intro = 2; }
                    }
                    if (intro <= 0)
                    { rollCredits = creditsActive = false; }
                }
            }
            else if (!acceptTextInput && !sceneIsMenu)
            {
                if (loadingBar.Value >= 100)
                    LevelUpdate();
            }
            else
            {
                if (levelEditor)
                {
                    if (useMultithreading)
                    { QueueThread(new WaitCallback(LevelEditor)); }
                    else
                    { LevelEditor(null); }

                    if (GetPauseKeyDown)
                    {
                        LoadMenu(0, 0, false);
                        levelEditor = false;
                    }
                }
                else
                { MenuUpdate(); }
            }

            if (GetKeyDown(Keys.OemTilde))
            { OpenConsole(); }
            if (consoleMoveTimer > 0)
            { MoveConsole(); }
            if (acceptTextInput && console)
            { UpdateConsole(); }
            else
            {
                if (useMultithreading)
                {
                    for (int i = particleSystems.Count - 1; i >= 0; i--)
                    { QueueThread(particleSystems[i].ThreadPoolCallback); }
                    foreach (Grass g in grassList)
                    { QueueThread(g.ThreadPoolCallback); }
                }
                else
                {
                    for (int i = particleSystems.Count - 1; i >= 0; i--)
                    {
                        if (particleSystems[i].Update())
                        { particleSystems.RemoveAt(i); }
                    }
                    for (int i = 0; i < grassList.Count; i++)
                    { grassList[i].Update(); }
                }
            }

            if (infoTextTimer > 0)
            {
                infoTextTimer -= deltaTime;
                if (infoTextTimer <= 0)
                { infoText.Clear(); }
            }

            // just don't even touch this
            if (debug > 4)
            {
                for (int i = 0; i < debugrays.Length; i++)
                { debugrays[i] = new Ray(screenSpacePlayerPos / screenScale, debugrays[i].Direction); }

                shadows.Clear();
                for (int i = 0; i < debugrays.Length; i++)
                {
                    Vector2 tempvector = debugrays[i].Intersect(debugshapes);
                    //if (shadows.Count > i && shadows[i] != tempvector && tempvector != Vector2.Zero)
                    //{
                    //    shadows[i] = tempvector;
                    //}
                    /*else*/
                    if (/*shadows.Count < i &&*/ tempvector != Vector2.Zero)
                    { shadows.Add(tempvector); }
                }

                shadowShape.Vertices = shadows.ToArray();
            }

            previousKeyboardState = keyboardState;
            previousMouseState = mouseState;
            previousGamePadState = gamePadState;
            if (pendingThreads <= 0) { doneEvent.Set(); }
            doneEvent.WaitOne();
            base.Update(gameTime);
        }

        protected void MenuUpdate()
        {
            if (!console)
            { menu.Update(this); }
        }

        protected void LevelUpdate()
        {
            if (debug > 0)
            {
                if (GetKeyDown(Keys.N) && !acceptTextInput)
                {
                    nextFrameTimer = 0f;
                    framebyframe = true; //nex
                }
                else if (framebyframe && GetKey(Keys.N) && !acceptTextInput)
                { nextFrameTimer += DeltaTime; }
                if (keyboardState.IsKeyDown(Keys.G) && !acceptTextInput)
                { framebyframe = false; } //nex
            }

            if (justLeftMenu && keyboardState.GetPressedKeys().Length <= 0)
            { jamie.inputEnabled = true; justLeftMenu = false; }

            if (!framebyframe || GetKeyDown(Keys.N) || nextFrameTimer > 0.1f) //nex
            {
                if (framebyframe)
                { nextFrameTimer -= 0.1f; }
                timer += deltaTime;

                if (useMultithreading)
                {
                    QueueThread(jamie.ThreadPoolCallback);
                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    { QueueThread(enemyList[i].ThreadPoolCallback); }
                    QueueThread(new WaitCallback(UpdateEnemyHealthBar));
                    if (jamie.Health <= 0)
                    { DeathScreen(); }
                    else
                    {
                        QueueThread(new WaitCallback(Camera));
                        if (deathBackgroundOpacity > 0)
                        { deathBackgroundOpacity -= RealTime; }
                    }
                }
                else
                {
                    jamie.Update();
                    if (jamie.Health <= 0)
                    { DeathScreen(); }
                    else
                    {
                        Camera(null);
                        if (deathBackgroundOpacity > 0)
                        { deathBackgroundOpacity -= RealTime; }
                    }

                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    {
                        enemyList[i].Update();
                        if (enemyList[i].Health <= 0 || enemyList[i].Position.Y > 5000f)
                        { KillEnemy(enemyList[i]); }
                    }
                    UpdateEnemyHealthBar(null);
                }

                if (timerDisplay != null) { timerDisplay.Update(TimerText(timer), true); }

                Bars.healthBar.UpdateValue(jamie.Health);
                Bars.shieldBar.UpdateValue(jamie.shield, jamie.shielded);
                Bars.energyBar.UpdateValue(jamie.energy);
            }

            if (onslaughtMode)
            {
                if (onslaughtSpawner.enemiesLeftThisWave > 0 && enemyList.Count < onslaughtSpawner.maxEnemies && onslaughtSpawner.EnemySpawnTimer())
                { SummonGenericEnemy(onslaughtSpawner.enemyHealth, onslaughtSpawner.enemyDamage, onslaughtSpawner.enemySpeed); }
                if (enemyList.Count <= 0 && onslaughtSpawner.enemiesKilled >= onslaughtSpawner.enemiesThisWave)
                { onslaughtSpawner.NextWave(); }
                if (!onslaughtSpawner.waveStarted)
                { onslaughtDisplay.Update("Wave " + onslaughtSpawner.wave + " Start: " + onslaughtSpawner.timeUntilNextSpawn.ToString("00"), true); }
                else
                { onslaughtDisplay.Update("Onslaught Wave " + onslaughtSpawner.wave, true); }

                for (int i = onslaughtSpawner.vendingMachineList.Count - 1; i >= 0; i--)
                {
                    onslaughtSpawner.vendingMachineList[i].Update();
                    if (GetUseKeyDown)
                    {
                        if (DistanceSquared(onslaughtSpawner.vendingMachineList[i].collider.Center, jamie.Collider.Center) <= vendingMachineUseDistanceSqr) //add collider to vending machine
                        {
                            onslaughtSpawner.vendingMachineList[i].LoadMenu();
                            jamie.inputEnabled = !onslaughtSpawner.vendingMachineList[i].drawMenu;
                            if (!jamie.inputEnabled)
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
                    if (jamie.inputEnabled)
                    {
                        onslaughtSpawner.vendingMachineList[vendingMenu].LoadMenu();
                        vendingMenu = -1;
                        IsMouseVisible = false;
                        WriteLine("vending machine menu close");
                    }
                    if (GetLeftKeyDown)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].Update(onslaughtSpawner.vendingMachineList[vendingMenu].selection - 1); }
                    if (GetRightKeyDown)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].Update(onslaughtSpawner.vendingMachineList[vendingMenu].selection + 1); }
                    if (GetUpKeyDown)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].MoveSelectionUp(); }
                    if (GetDownKeyDown)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].MoveSelectionDown(); }
                    if (GetLeftMouseDown)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].OnClick(mouseState.Position); }

                    if (mouseState != previousMouseState)
                    { onslaughtSpawner.vendingMachineList[vendingMenu].Update(mouseState.Position); }

                    if (Use())
                    { onslaughtSpawner.vendingMachineList[vendingMenu].Purchase(onslaughtSpawner.vendingMachineList[vendingMenu].selection); }
                }
            }

            if (GetPauseKeyDown)
            {
                if (vendingMenu >= 0)
                {
                    onslaughtSpawner.vendingMachineList[vendingMenu].LoadMenu();
                    jamie.inputEnabled = true;
                    vendingMenu = -1;
                    IsMouseVisible = false;
                    WriteLine("vending machine menu close");
                }
                else
                {
                    framebyframe = false;
                    LoadMenu(0, 0, false);
                    jamie.inputEnabled = false;
                }
            }

            //this will be removed, don't bother multithreading
            if (!acceptTextInput)
            {
                if (keyboardState.IsKeyDown(Keys.K) && !previousKeyboardState.IsKeyDown(Keys.K))
                {
                    randomTimer = 0f;
                    SummonGenericEnemy();
                }
                if (keyboardState.IsKeyDown(Keys.K))
                {
                    randomTimer += DeltaTime;
                    if (randomTimer > 0.5f)
                    { SummonGenericEnemy(); }
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

        private void Camera(Object threadContext)
        {
            try
            {
                if (cameraShakeDuration > 0)
                { CameraShake(); }
                else if (cameraShakeLerpTime > 0)
                { ReturnCamera(); }

                if (smartCamera)
                {
                    if (jamie.direction == Direction.Left)
                    {
                        mainCamera.X = Lerp(mainCamera.X, (jamie.TrueCenter.X * screenScale) - halfBoundingBox.X,
                            (cameraLerpXSpeed + (cameraLerpXSpeed * (-jamie.Velocity.X / jamie.speed) * 0.5f)) * DeltaTime);
                    }
                    else //if (jamie.direction == Direction.Right)
                    {
                        mainCamera.X = Lerp(mainCamera.X, (jamie.TrueCenter.X * screenScale) + halfBoundingBox.X,
                            (cameraLerpXSpeed + (cameraLerpXSpeed * (jamie.Velocity.X / jamie.speed) * 0.5f)) * DeltaTime);
                    }

                    if (GetDownKey && jamie.attacking != Attacking.Slam)
                    { mainCamera.Y = Lerp(mainCamera.Y, (jamie.TrueCenter.Y * screenScale) + halfBoundingBox.Y, cameraLerpYSpeed * DeltaTime); }
                    else if (GetUpKey)
                    { mainCamera.Y = Lerp(mainCamera.Y, (jamie.TrueCenter.Y * screenScale) - halfBoundingBox.Y, cameraLerpYSpeed * DeltaTime); }
                    else
                    {
                        if (boundingBox.Bottom > screenSpacePlayerPos.Y - cameraLerpYSpeed * screenScale)
                        { mainCamera.Y = Lerp(mainCamera.Y, jamie.TrueCenter.Y * screenScale, cameraLerpYSpeed * DeltaTime); }
                        else
                        {
                            mainCamera.Y = Lerp(mainCamera.Y, jamie.TrueCenter.Y * screenScale,
                                ((screenSpacePlayerPos.Y - boundingBox.Bottom) / screenScale) * DeltaTime);
                        }
                    }

                    if (jamie.combat)
                    { cameraLockon = Lerp(cameraLockon, (cameraLockonLocation - mainCamera / screenScale) * cameraLockonFactor, cameraLockonSpeed * DeltaTime); }
                    else
                    { cameraLockon = Lerp(cameraLockon, Vector2.Zero, cameraLockonSpeed * DeltaTime); }

                    background.M31 = foreground.M41 = boundingBox.Center.X - (mainCamera.X + camera.X + cameraLockon.X);
                    background.M32 = foreground.M42 = boundingBox.Center.Y - (mainCamera.Y + camera.Y + cameraLockon.Y);
                }
                else
                {
                    if (cameraLerpSetting)
                    {
                        if (boundingBox.Right <= screenSpacePlayerPos.X)
                        { mainCamera.X += Lerp(boundingBox.Right, screenSpacePlayerPos.X, cameraLerpXSpeed * DeltaTime) - boundingBox.Right; }
                        else if (boundingBox.Left >= screenSpacePlayerPos.X)
                        { mainCamera.X += Lerp(boundingBox.Left, screenSpacePlayerPos.X, cameraLerpXSpeed * DeltaTime) - boundingBox.Left; }
                        if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                        { mainCamera.Y += Lerp(boundingBox.Bottom, screenSpacePlayerPos.Y, cameraLerpYSpeed * DeltaTime) - boundingBox.Bottom; }
                        else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                        { mainCamera.Y += Lerp(boundingBox.Top, screenSpacePlayerPos.Y, cameraLerpYSpeed * DeltaTime) - boundingBox.Top; }
                    }
                    else
                    {
                        if (boundingBox.Right <= screenSpacePlayerPos.X)
                        { mainCamera.X += screenSpacePlayerPos.X - boundingBox.Right; }
                        else if (boundingBox.Left >= screenSpacePlayerPos.X)
                        { mainCamera.X += screenSpacePlayerPos.X - boundingBox.Left; }
                        if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                        { mainCamera.Y += screenSpacePlayerPos.Y - boundingBox.Bottom; }
                        else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                        { mainCamera.Y += screenSpacePlayerPos.Y - boundingBox.Top; }
                    }

                    background.M31 = foreground.M41 = halfResolution.X - (mainCamera.X + camera.X);
                    background.M32 = foreground.M42 = halfResolution.Y - (mainCamera.Y + camera.Y);
                }

                screenSpacePlayerPos = (jamie.TrueCenter * screenScale) + boundingBox.Center.ToVector2() - mainCamera;
            }
            finally
            {
                if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
                { Irbis.doneEvent.Set(); }
            }
        }

        public static void CenterCamera()
        {
            //screenSpacePlayerPos = mainCamera = boundingBox.Center.ToVector2();

            if (jamie.direction == Direction.Left)
            { mainCamera.X = (jamie.TrueCenter.X * screenScale) - halfBoundingBox.X; }
            else //if (jamie.direction == Direction.Right)
            { mainCamera.X = (jamie.TrueCenter.X * screenScale) + halfBoundingBox.X; }
            mainCamera.Y = jamie.TrueCenter.Y * screenScale;
        }

        /*
        public static void CameraSwing(float duration, float magnitude, Vector2 heading)
        {
            if (cameraSwingSetting)
            {
                cameraSwingDuration = cameraSwingMaxDuration = duration;
                cameraSwingHeading = heading;
                cameraSwingHeading.Normalize();
                cameraSwingMagnitude = magnitude * screenScale;
            }
        }

        /// <summary>
        /// returns how long the camera should take returning to it's original position. 
        /// returns -1 if swinging is not done.
        /// </summary>
        private static float CameraSwing()
        {
            //cameraSwingDuration -= DeltaTime;

            //camera.X = (Lerp(camera.X, camera.X + (cameraSwingHeading.X * cameraSwingMagnitude), 15f * DeltaTime));
            //camera.Y = (Lerp(camera.Y, camera.Y + (cameraSwingHeading.Y * cameraSwingMagnitude), 15f * DeltaTime)); //cameraLerpSpeed

            //if (cameraSwingDuration <= 0)
            //{
            //    return cameraSwingMaxDuration * 2;
            //}
            return -1;
        }
        /**/

        public static void CameraShake(float duration, float magnitude)
        {
            if (cameraShakeSetting)
            {
                cameraShakeDuration = duration;
                cameraShakePrevLocation = Vector2.Zero;
                cameraShakeMagnitude = magnitude * screenScale;
            }
        }

        private static void CameraShake()
        {
            if (cameraShakeLerpTime <= 0)
            {
                cameraShakePrevLocation = cameraShakeTargetLocation;
                cameraShakeTargetLocation = RandomPoint(cameraShakeMagnitude);
                if (debug > 0)
                {
                    WriteLine("       mainCamera:" + mainCamera +
                            "\ncameraShakeTarget:" + cameraShakeTargetLocation + '\n');
                }
                cameraShakeLerpTime = cameraShakeLerpTimeMax;
                cameraShakeDuration -= cameraShakeLerpTime;
                if (cameraShakeDuration <= 0)
                { return; }
            }
            cameraShakeLerpTime -= DeltaTime;
            cameraShakePercentage = 1 - (cameraShakeLerpTime / cameraShakeLerpTimeMax);
            camera = SmoothStep(cameraShakePrevLocation, cameraShakeTargetLocation, cameraShakePercentage);
        }

        private static void ReturnCamera()
        {
            cameraShakeLerpTime -= DeltaTime;
            cameraShakePercentage = 1 - (cameraShakeLerpTime / cameraShakeLerpTimeMax);
            camera = SmoothStep(cameraShakePrevLocation, Vector2.Zero, cameraShakePercentage);
        }

        private void LoadPlayer()
        {
            PlayerSettings playerSettings = new PlayerSettings(true);
            if (File.Exists(@".\content\playerSettings.ini"))
            {
                Console.WriteLine("loading playerSettings.ini...");
                playerSettings.Load(@".\content\playerSettings.ini");
            }
            else
            {
                Console.WriteLine("creating new playerSettings.ini...");
                WriteLine("creating new playerSettings.ini...");
                playerSettings = new PlayerSettings(true);
                Console.WriteLine("saving playerSettings.ini...");
                playerSettings.Save(@".\content\playerSettings.ini");
                Console.WriteLine("loading playerSettings.ini...");
                playerSettings.Load(@".\content\playerSettings.ini");
            }

            Load(playerSettings);

            Texture2D playerTex = LoadPNG("player");
            Texture2D shieldTex = LoadTexture("shield");

            jamie = new Player(playerTex, shieldTex, playerSettings, 0.5f);
        }

        private static bool LoadMusic()
        {
            //string[] tempMusicList = Directory.GetFiles(".\\music");

            music = new List<Song>();
            musicList = new List<string>();
            music.Add(game.Content.Load<Song>("music/bensound-betterdays"));
            musicList.Add("bensound-betterdays");

            music.Add(game.Content.Load<Song>("music/bensound-epic"));
            musicList.Add("bensound-epic");

            music.Add(game.Content.Load<Song>("music/bensound-instinct"));
            musicList.Add("bensound-instinct");

            music.Add(game.Content.Load<Song>("music/bensound-memories"));
            musicList.Add("bensound-memories");

            music.Add(game.Content.Load<Song>("music/bensound-relaxing"));
            musicList.Add("bensound-relaxing");

            music.Add(game.Content.Load<Song>("music/bensound-slowmotion"));
            musicList.Add("bensound-slowmotion");

            return true;
        }

        public static bool PlaySong(string songName, bool repeat)
        {
            int songIndex = musicList.IndexOf(songName);
            if (songIndex >= 0)
            {
                MediaPlayer.Volume = (masterAudioLevel * musicLevel) / 10000f;
                MediaPlayer.Play(music[songIndex]);
                MediaPlayer.IsRepeating = repeat;
                //MediaPlayer.Stop();
                return true;
            }
            else
            { MediaPlayer.Stop(); }
            return false;
        }

        public static void DisplayInfoText(string text)
        {
            infoText.Update(text, true);
            infoTextTimer = infoTextTimerMax;
        }

        public static void DisplayInfoText(string text, float howlong)
        {
            infoText.Update(text, true);
            infoTextTimer = howlong;
        }

        private void DebugUpdate(Object threadContext)
        {
            try
            {
                if (!console && consoleMoveTimer <= 0 && (RealTime) < (elapsedTime / framedropfactor))
                {
                    WriteLine("recording framedrop (1/" + framedropfactor +
                        ")\nold fps:" + (1 / RealTime) + " new fps:" + (1 / elapsedTime) + " timer: " + Timer);
                }
                if (recordFPS)
                {
                    meanFPS.Update(elapsedTime);
                    if (maxFPS < (1 / elapsedTime))
                    {
                        maxFPS = (1 / elapsedTime);
                        maxFPStime = Timer;
                    }
                    if (minFPS > (1 / elapsedTime))
                    {
                        minFPS = (1 / elapsedTime);
                        minFPStime = Timer;
                    }
                }
                switch (Irbis.debug)
                {
                    case 5:
                        goto case 4;
                    case 4:
                        goto case 3;
                    case 3:
                        PrintDebugInfo();
                        if (jamie != null)
                        {
                            try
                            {
                                foreach (Square s in squareList)
                                {
                                    if (jamie.collided.Contains(s))
                                    { s.color = Color.Cyan; }
                                    else
                                    { s.color = Color.White; }
                                }
                            }
                            catch (Exception e)
                            {
                                WriteLine("exception caught during debug:" + e.Message + "\nStackTrace:\n" + e.StackTrace);
                                DisplayInfoText("exception caught during debug:" + e.Message);
                            }
                        }
                        break;
                    case 2:
                        goto case 1;
                    case 1:
                        debuginfo.Update("\n\nᴥ" + smartFPS.Framerate.ToString("0000"), true);
                        break;
                    default:
                        //disable on release
                        break;
                }
            }
            finally
            {
                if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
                { Irbis.doneEvent.Set(); }
            }
        }

        public void PrintDebugInfo()
        {
            debuginfo.Update("      DEBUG MODE. " + versionID.ToUpper() + " v" + versionNo, true);
            debuginfo.Update("\n DeltaTime:" + DeltaTime);
            debuginfo.Update("\n   raw FPS:" + (1 / DeltaTime).ToString("0000.0"));
            debuginfo.Update("\n  smartFPS:" + smartFPS.Framerate.ToString("0000.0"));
            smoothUpdate++;
            if (smoothUpdate >= smoothFPS.Samples)
            {
                smoothUpdate = 0;
                smoothDisplay = smoothFPS.Framerate.ToString("0000.0");
                smoothFPS = new SmoothFramerate((int)smoothFPS.Framerate);
                medianDisplay = ((minFPS + maxFPS) / 2d).ToString("0000.0");
                minDisplay = minFPS.ToString("0000.0");
                maxDisplay = maxFPS.ToString("0000.0");
                maxFPS = double.MinValue;
                minFPS = double.MaxValue;
            }
            debuginfo.Update("\n   meanFPS:" + smoothDisplay);
            debuginfo.Update("\n medianFPS:" + medianDisplay);
            debuginfo.Update("\n    minFPS:" + minDisplay);
            debuginfo.Update("\n    maxFPS:" + maxDisplay);
            if (intro > 0)
            { debuginfo.Update("\n\nintro:" + intro); }
            debuginfo.Update("\n\nmouse location:" + GetMouseState.Position);
            debuginfo.Update("\n\nworld space mouse location:" + WorldSpaceMouseLocation);
            debuginfo.Update("\ntimer:" + TimerText(timer));
            if (jamie != null)
            {
                debuginfo.Update("\n     input:" + jamie.input + "  isRunning:" + jamie.isRunning);
                debuginfo.Update("\n prevInput:" + jamie.prevInput);
                debuginfo.Update("\n\nwallJumpTimer:" + jamie.wallJumpTimer);
                debuginfo.Update("\njumpable:" + jamie.jumpable);
                debuginfo.Update("\njumpTime:" + jamie.jumpTime);
                debuginfo.Update("\n\n  player info");
                debuginfo.Update("\nHealth:" + jamie.Health + "\nShield:" + jamie.shield + "\nEnergy:" + jamie.energy);
                debuginfo.Update("\n    pos:" + jamie.position);
                debuginfo.Update("\nscreenSpacePlayerPos:" + screenSpacePlayerPos);
                debuginfo.Update("\n    vel:" + jamie.velocity);
                debuginfo.Update("\nbaseVel:" + jamie.baseVelocity);
                debuginfo.Update("\n    col:" + jamie.Collider);
                debuginfo.Update("\n center:" + jamie.Collider.Center);
                debuginfo.Update("\nmaxspeed:" + jamie.debugspeed);
                debuginfo.Update("\ninvulner:" + jamie.invulnerable);
                debuginfo.Update("\nShielded:" + jamie.shielded);
                debuginfo.Update("\ncolliders:" + collisionObjects.Count);
                debuginfo.Update("\n collided:" + jamie.collided.Count);
                debuginfo.Update("\n   walled:" + jamie.Walled);
                debuginfo.Update("\nattacking:" + jamie.attacking);
                debuginfo.Update("\nprevattacking:" + jamie.prevAttacking);
                debuginfo.Update("\nactivity:" + jamie.activity);
                debuginfo.Update("\nprevActivity:" + jamie.prevActivity);
                debuginfo.Update("\ndirection:" + jamie.direction);
                debuginfo.Update("\nactivityChanged:" + jamie.activityChanged);
                debuginfo.Update("\nidleTime:" + jamie.idleTime);
                debuginfo.Update("\nspecialTime:" + jamie.specialTime);
                debuginfo.Update("\nfallableSquare:" + jamie.fallableSquare);
                debuginfo.Update("\n          animation:" + jamie.currentAnimation);
                debuginfo.Update("\nanimationSourceRect:" + jamie.animationSourceRect);
                debuginfo.Update("\n    animationNoLoop:" + jamie.animationNoLoop);
            }
            debuginfo.Update("\nThumbSticks.Left:" + GetGamePadState.ThumbSticks.Left);
            debuginfo.Update("\n\ncurrentLevel:" + currentLevel);
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
            if (enemyList.Count > 0)
            {
                debuginfo.Update("\n\nTotal of " + enemyList.Count + " enemies");
                debuginfo.Update("\nEnemy Info:");

                float avghp = 0;
                for (int i = enemyList.Count - 1; i >= 0; i--)
                { avghp += enemyList[i].Health; }
                avghp = avghp / enemyList.Count;
                debuginfo.Update("\n  avg health: " + avghp);

                if (enemyList[0].GetType() == typeof(LizardGuy))
                {
                    //debuginfo.Update("\nXDistance:" + XDistance(Irbis.jamie.Collider, ((LizardGuy)enemyList[0]).Collider));
                    //debuginfo.Update("\nYDistance:" + YDistance(Irbis.jamie.Collider, ((LizardGuy)enemyList[0]).Collider));
                    //debuginfo.Update("\n Distance:" + Distance(Irbis.jamie.Collider, ((LizardGuy)enemyList[0]).Collider));
                    //debuginfo.Update("\n Arena side closest:" + SideClosest(((LizardGuy)enemyList[0]).bossArena, Irbis.jamie.Collider));
                    debuginfo.Update("\n\n   Lizard Activity:" + ((LizardGuy)enemyList[0]).activity);
                    debuginfo.Update("\n  melee activities:" + ((LizardGuy)enemyList[0]).meleeActivitiesInARow);
                    //debuginfo.Update("\n  proper direction:" + Directions(((LizardGuy)enemyList[0]).TrueCollider, jamie.Collider));
                    //debuginfo.Update("\n         Direction:" + ((LizardGuy)enemyList[0]).direction);
                    //debuginfo.Update("\n         Animation:" + ((LizardGuy)enemyList[0]).currentAnimation);
                    //debuginfo.Update("\n             speed:" + ((LizardGuy)enemyList[0]).animationSpeed[((LizardGuy)enemyList[0]).currentAnimation]);
                    //debuginfo.Update("\n\n ActivelyAttacking:" + ((LizardGuy)enemyList[0]).ActivelyAttacking);
                    debuginfo.Update("\n     ActiveAttacks:" + ((LizardGuy)enemyList[0]).ActiveAttacks);
                    //debuginfo.Update("\ntimeSinceLastFrame:" + ((LizardGuy)enemyList[0]).timeSinceLastFrame);
                    //debuginfo.Update("\n            frames:" + ((LizardGuy)enemyList[0]).animationFrames[((LizardGuy)enemyList[0]).currentAnimation]);
                    debuginfo.Update("\n          velocity:" + ((LizardGuy)enemyList[0]).Velocity);
                    debuginfo.Update("\n         direction:" + ((LizardGuy)enemyList[0]).direction);
                    //debuginfo.Update("\n              stun:" + ((LizardGuy)enemyList[0]).stunned);
                    debuginfo.Update("\n              roll:" + ((LizardGuy)enemyList[0]).state[1] + " cooldown:" + ((LizardGuy)enemyList[0]).cooldown[1]);
                    debuginfo.Update("\n             swipe:" + ((LizardGuy)enemyList[0]).state[2] + " cooldown:" + ((LizardGuy)enemyList[0]).cooldown[2]);
                    debuginfo.Update("\n              bury:" + ((LizardGuy)enemyList[0]).state[4] + " cooldown:" + ((LizardGuy)enemyList[0]).cooldown[4]);
                    debuginfo.Update("\n            wander:" + ((LizardGuy)enemyList[0]).state[0] + " cooldown:" + ((LizardGuy)enemyList[0]).cooldown[0]);
                    debuginfo.Update("\n");
                }
                else if (enemyList[0].GetType() == typeof(WizardGuy))
                {
                    debuginfo.Update("\n\nWizard state:" + ((WizardGuy)enemyList[0]).bossState);
                    debuginfo.Update("\n      Activity:" + ((WizardGuy)enemyList[0]).activity);
                    debuginfo.Update("\n         phase:" + ((WizardGuy)enemyList[0]).combatPhase);
                    debuginfo.Update("\n     Animation:" + ((WizardGuy)enemyList[0]).currentAnimation);
                    debuginfo.Update("\n      Position:" + ((WizardGuy)enemyList[0]).Position);
                    debuginfo.Update("\n   speed:" + ((WizardGuy)enemyList[0]).animationSpeed[((WizardGuy)enemyList[0]).currentAnimation]);
                    debuginfo.Update("\n    idle:" + ((WizardGuy)enemyList[0]).timer[0]);
                    debuginfo.Update("\nteleport:" + ((WizardGuy)enemyList[0]).timer[1]);
                    debuginfo.Update("\n    nova:" + ((WizardGuy)enemyList[0]).timer[2]);
                    debuginfo.Update("\nfireball:" + ((WizardGuy)enemyList[0]).timer[6]);
                    debuginfo.Update("\n    bolt:" + ((WizardGuy)enemyList[0]).timer[3]);
                    debuginfo.Update("\n  lazers:" + ((WizardGuy)enemyList[0]).timer[4]);
                    debuginfo.Update("\n    heal:" + ((WizardGuy)enemyList[0]).timer[8]);



                }


                for (int i = 0; i < enemyList.Count; i++)
                { debuginfo.Update("\n enemy: " + enemyList[i].Name + "\nhealth: " + enemyList[i].Health + "\n  stun: " + enemyList[i].StunTime); }
            }
            //debuginfo.Update("\nDistance  :" + Distance(testRectangle, Irbis.jamie.Collider));
            //debuginfo.Update("\nDistance  :" + Distance(Irbis.jamie.Collider, testRectangle.Center));
            //debuginfo.Update("\nDistanceTL:" + Distance(testRectangle.Center, new Point(jamie.Collider.Left, jamie.Collider.Top)));
            //debuginfo.Update("\nDistanceTR:" + Distance(testRectangle.Center, new Point(jamie.Collider.Right, jamie.Collider.Top)));
            //debuginfo.Update("\nDistanceBL:" + Distance(testRectangle.Center, new Point(jamie.Collider.Left, jamie.Collider.Bottom)));
            //debuginfo.Update("\nDistanceBR:" + Distance(testRectangle.Center, new Point(jamie.Collider.Right, jamie.Collider.Bottom)));
            //debuginfo.Update("\ntestRectangle.Location:" + testRectangle.Location + " testRectangle.Center:" + testRectangle.Center);
            //background.M31 = foreground.M41 = halfResolution.X - (mainCamera.X + camera.X);
            //background.M32 = foreground.M42 = halfResolution.Y - (mainCamera.Y + camera.Y);

            debuginfo.Update("\nmainCamera:" + mainCamera);
            debuginfo.Update("\nBG.M31:" + background.M31 + " FG.M41:" + foreground.M41);
            debuginfo.Update("\nBG.M32:" + background.M32 + " FG.M42:" + foreground.M42);
            debuginfo.Update("\n    Camera:" + camera);
            debuginfo.Update("\n    Lockon:" + cameraLockon);

        }

        public void Debug(int rank)
        {
            debug = rank;
            if (debug > 0)
            {
                this.IsMouseVisible = recordFPS = true;
                meanFPS = new TotalMeanFramerate(true);
                maxFPS = double.MinValue;
                minFPS = double.MaxValue;
                debuginfo.align = Direction.Left;
                debuginfo.origin = new Point(3, 3);
            }
            else
            {
                //debuginfo.Update(versionID + " v" + versionNo, true);
                debuginfo.Clear();
                this.IsMouseVisible = recordFPS = false;
                foreach (Square s in squareList)
                { s.color = Color.White; }
                //debuginfo.align = Direction.Right;
                //debuginfo.origin = new Point((resolution.X - 3), 3);
            }
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

        private static void UpdateEnemyHealthBar(Object threadContext)
        {
            try
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
                            thisEnemysSqrDistance = DistanceSquared(jamie.Collider, e.Collider);
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
                        displayEnemyHealth = jamie.combat = true;
                        jamie.Combat();
                        Bars.enemyHealthBar.maxValue = closest.MaxHealth;
                        Bars.enemyHealthBar.UpdateValue(closest.Health);
                        Bars.name.Update(closest.Name, true);
                        cameraLockonLocation = closest.Position;
                    }
                    else
                    { displayEnemyHealth = jamie.combat = false; }
                }
                else
                { displayEnemyHealth = jamie.combat = false; }

            }
            finally
            {
                if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
                { Irbis.doneEvent.Set(); }
            }
        }

        public void EnableLevelEditor()
        {
            if (levelEditor)
            {
                LoadMenu(0, 0, false);
                levelEditor = false;
                availableTextures.Clear();
                sliderList.Clear();
            }
            else
            {
                ClearUI();
                levelEditor = sceneIsMenu = true;
                enemyList.Clear();
                texturePanel = new Rectangle((int)((float)resolution.X * 0.8f), 0, (int)((float)resolution.X * 0.2f), resolution.Y);
                buttonList.Add(new Button(new Rectangle(texturePanel.X + 10, 10, 30 * textScale, 16 * textScale), Direction.Forward, "01", ">01",
                    new Color(223, 227, 236), nullTex, font, Color.Magenta, false, true, 0.9f));
                sliderList.Add(new UIElementSlider(Direction.Left, new Rectangle(texturePanel.X + (40 * textScale), 10, texturePanel.Width - 50 * textScale, 20 * textScale),
                    Point.Zero, Direction.Left, 1, new Color(166, 030, 030), Color.White, Color.White, Color.Red, nullTex, null, null, true, null, false, 0.9f, 0.899f, 0.901f, 0.902f));
                sliderList[0].UpdateValue(0.5f);
                string[] texlist = Directory.GetFiles(".\\content\\textures\\environment");
                availableTextures = new List<Texture2D>();
                availableTextures.Add(defaultTex);
                foreach (string s in texlist)
                { availableTextures.Add(LoadTexture(s.Substring(31))); }
            }
        }

        public void LevelEditor(Object threadContext)
        {
            try
            {
                //debug = 0;
                this.IsMouseVisible = sceneIsMenu = true;
                //PrintDebugInfo();
                if (!acceptTextInput)
                {
                    if (GetKey(Keys.LeftShift))
                    {
                        if (keyboardState.IsKeyDown(upKey))
                        { mainCamera.Y -= 10f * DeltaTime; }
                        if (keyboardState.IsKeyDown(downKey))
                        { mainCamera.Y += 10f * DeltaTime; }
                        if (keyboardState.IsKeyDown(leftKey))
                        { mainCamera.X -= 10f * DeltaTime; }
                        if (keyboardState.IsKeyDown(rightKey))
                        { mainCamera.X += 10f * DeltaTime; }
                    }
                    else
                    {
                        if (keyboardState.IsKeyDown(upKey))
                        { mainCamera.Y -= 1000f * DeltaTime; }
                        if (keyboardState.IsKeyDown(downKey))
                        { mainCamera.Y += 1000f * DeltaTime; }
                        if (keyboardState.IsKeyDown(leftKey))
                        { mainCamera.X -= 1000f * DeltaTime; }
                        if (keyboardState.IsKeyDown(rightKey))
                        { mainCamera.X += 1000f * DeltaTime; }
                    }
                }

                //background.M31 = foreground.M41 = halfResolution.X - (mainCamera.X + camera.X);
                //background.M32 = foreground.M42 = halfResolution.Y - (mainCamera.Y + camera.Y);
                screenspace.X = (int)(mainCamera.X - halfResolution.X);
                screenspace.Y = (int)(mainCamera.Y - halfResolution.Y);

                if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed)
                {
                    int destroyBlock = -1;

                    for (int i = 0; i < squareList.Count; i++)
                    {
                        if (new Rectangle(squareList[i].Position.ToPoint(), squareList[i].texture.Bounds.Size).Contains(WorldSpaceMouseLocation))
                        { destroyBlock = i; }
                    }
                    if (destroyBlock >= 0)
                    {
                        WriteLine("destroying block " + destroyBlock + " at " + WorldSpaceMouseLocation);
                        squareList.RemoveAt(destroyBlock);
                    }
                    else
                    {
                        WriteLine("spawning square with " + selectedTexture.Name + " at " + WorldSpaceMouseLocation);
                        squareList.Add(new Square(selectedTexture, Color.White, WorldSpaceMouseLocation, null, Vector2.Zero, screenScale, 0.3f));
                        selectedBlock = squareList.Count - 1;
                    }
                }

                while (acceptTextInput && selectedBlock >= 0 && textInputBuffer.Length > 0)
                {
                    if ((char.IsDigit(textInputBuffer[0])) && buttonList[0].buttonStatement.Length < 8) //-
                    {
                        buttonList[0].Update(textInputBuffer[0].ToString(), false);
                    }
                    textInputBuffer = textInputBuffer.Substring(1);
                    if (GetEnterKey || GetPauseKey)
                    {
                        acceptTextInput = false;
                        if (buttonList[0].buttonStatement.Length <= 0)
                        {
                            buttonList[0].buttonStatement = buttonList[0].originalStatement;
                        }
                        else
                        {
                            float floatResult;
                            if (float.TryParse(buttonList[0].buttonStatement, out floatResult))
                            {
                                sList[selectedBlock].depth = floatResult;
                            }
                            else
                            {
                                buttonList[0].buttonStatement = buttonList[0].originalStatement;
                            }
                            buttonList[0].originalStatement = sList[selectedBlock].depth.ToString("0.00");
                            buttonList[0].highlightStatement = ">" + sList[selectedBlock].depth.ToString("0.00");
                            sliderList[0].UpdateValue(sList[selectedBlock].depth);
                            //update sound effects volume
                        }
                    }
                }

                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (previousMouseState.LeftButton != ButtonState.Pressed)
                    {
                        if (buttonList[0].Contains(mouseState))
                        {
                            acceptTextInput = true;
                            menuSelection = 0;
                        }
                        else if (sliderList[0].Contains(mouseState))
                        { sliderPressed = 0; }
                        else
                        {
                            lastEditedBlock = -1;
                            for (int i = 0; i < squareList.Count; i++)
                            {
                                if (new Rectangle(squareList[i].Position.ToPoint(), squareList[i].texture.Bounds.Size).Contains(WorldSpaceMouseLocation))
                                { lastEditedBlock = i; }
                            }
                            if (lastEditedBlock >= 0)
                            {
                                WriteLine("moving block " + lastEditedBlock);
                                selectedBlock = lastEditedBlock;
                            }
                        }
                    }
                    if (sliderPressed >= 0 && selectedBlock >= 0)
                    {
                        sList[selectedBlock].depth = ((float)(GetMouseState.X - sliderList[0].bounds.Left) / (float)(sliderList[0].bounds.Width));
                        buttonList[0].originalStatement = sList[selectedBlock].depth.ToString("0.00");
                        buttonList[0].highlightStatement = ">" + sList[selectedBlock].depth.ToString("0.00");
                        sliderList[0].UpdateValue(sList[selectedBlock].depth);
                    }
                }
                else
                { sliderPressed = -1; }

                if (mouseState.LeftButton == ButtonState.Pressed && lastEditedBlock >= 0)
                { squareList[lastEditedBlock].Position = WorldSpaceMouseLocation.ToVector2(); }
            }
            finally
            {
                if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
                { Irbis.doneEvent.Set(); }
            }
        }

        public static Level ConvertOldLevelFileToNew(string fileName)
        {
            //string[] leeevelList = Directory.GetFiles(".\\levels");
            //Console.WriteLine("length pre format check: " + leeevelList.Length);

            //if (true)
            //{
            //    List<string> tempLevelList = new List<string>();

            //    foreach (string s in leeevelList)
            //    {
            //        tempLevelList.Add(s);
            //    }

            //    for (int i = 0; i < tempLevelList.Count; i++)
            //    {
            //        if (tempLevelList[i].StartsWith(".\\levels\\") && tempLevelList[i].EndsWith(".lvl"))
            //        {
            //            //tempLevelList[i] = tempLevelList[i].Substring(9);
            //            //tempLevelList[i] = tempLevelList[i].Remove(tempLevelList[i].Length - 4);
            //        }
            //        else
            //        {
            //            tempLevelList.RemoveAt(i);
            //        }
            //    }

            //    leeevelList = tempLevelList.ToArray();
            //}

            //Console.WriteLine("length post format check: " + leeevelList.Length);


            //foreach (string s in leeevelList)
            //{
            Level lvl = new Level(true);
            //Console.WriteLine("attempting load on: " + fileName);
            //lvl.Load(fileName);
            //Console.WriteLine(lvl.ToString());
            //OldLevel.Save(OldLevel.LevelConverter(lvl), fileName);

            OldLevel testlvl = new OldLevel(true);
            testlvl.Load(fileName);
            testlvl.Load(fileName);
            //testlvl.Debug();
            //}


            return lvl;
        }

        public void SaveLevel(string levelname)
        {
            Level thisLevel = new Level(true);
            //thisLevel.squareList = squareList;

            List<Point> squareSpawns = new List<Point>();
            List<Rectangle?> squareColliders = new List<Rectangle?>();
            List<Vector2> squareOrigins = new List<Vector2>();
            List<string> squareTextures = new List<string>();
            List<float> squareDepths = new List<float>();
            for (int i = 0; i < squareList.Count; i++)
            {
                squareSpawns.Add(squareList[i].InitialPosition);
                squareColliders.Add(squareList[i].InitialCollider);
                squareOrigins.Add(squareList[i].Origin);
                squareDepths.Add(squareList[i].depth);
                squareTextures.Add(squareList[i].texture.Name);
            }

            List<Point> BackgroundSquares = new List<Point>();
            List<string> backgroundTextures = new List<string>();
            List<float> backgroundSquareDepths = new List<float>();
            for (int i = 0; i < backgroundSquareList.Count; i++)
            {
                BackgroundSquares.Add(backgroundSquareList[i].InitialPosition);
                backgroundTextures.Add(backgroundSquareList[i].texture.Name);
                backgroundSquareDepths.Add(backgroundSquareList[i].depth);
            }
            thisLevel.BackgroundColor = backgroundColor;

            thisLevel.SquareSpawnPoints = squareSpawns.ToArray();
            thisLevel.SquareColliders = squareColliders.ToArray();
            thisLevel.squareDepths = squareDepths;
            thisLevel.squareTextures = squareTextures;
            thisLevel.BackgroundSquares = BackgroundSquares.ToArray();
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
            thisLevel.bossName = bossName;
            thisLevel.EnemySpawnPoints = enemySpawnPoints.ToArray();

            thisLevel.ParticleSystems = particleSystems.ToArray();
            thisLevel.darkness = darkness;
            thisLevel.Grasses = grassList.ToArray();


            thisLevel.Save(".\\levels\\" + levelname + ".lvl");
            WriteLine(".\\levels\\" + levelname + ".lvl saved");
            Console.WriteLine(".\\levels\\" + levelname + ".lvl saved");
        }

        public void ClearLevel()
        {
            printList.Clear();
            infoText.Clear();
            infoTextTimer = 0;
            sList.Clear();
            buttonList.Clear();
            //printList.Add(topright);
            //printList.Add(debuginfo);
            //printList.Add(consoleWriteline);
            //printList.Add(developerConsole);
            //printList.Add(infoText);
            //eList.Clear();
            enemyList.Clear();
            squareList.Clear();
            collisionObjects.Clear();
            backgroundSquareList.Clear();
            if (jamie != null)
            { jamie.PlayerEventsReset(); }

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
            printList.Clear();
            buttonList.Clear();
            infoText.Clear();
            infoTextTimer = 0;
            sList.Clear();

            bars = null;
        }

        public static Texture2D[] LoadEnchantIcons()
        {
            List<Texture2D> texlist = new List<Texture2D>();

            texlist.Add(LoadTexture("blood enchant icon"));
            texlist.Add(LoadTexture("fire enchant icon"));
            texlist.Add(LoadTexture("frost enchant icon"));
            texlist.Add(LoadTexture("knockback enchant icon"));
            texlist.Add(LoadTexture("poison enchant icon"));
            texlist.Add(LoadTexture("sharpness enchant icon"));
            texlist.Add(LoadTexture("stun enchant icon"));

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

        /// <summary>
        /// returns a random number between 0 and the passed value (not including the value)
        /// </summary>
        public static int RandomInt(int maxValue)
        { return RAND.Next(maxValue); }

        /// <summary>
        /// returns a random number between min and max
        /// </summary>
        public static int RandomInt(int minValue, int maxValue)
        { return RAND.Next(minValue, maxValue); }

        /// <summary>
        /// returns a random number between X and Y
        /// </summary>
        public static int RandomInt(Point Range)
        { return RAND.Next(Range.X, Range.Y); }

        /// <summary>
        /// returns a random number between min and max
        /// </summary>
        public static float RandomFloatInRange(float minValue, float maxValue)
        { return RandomFloat * (maxValue - minValue) + minValue; }

        /// <summary>
        /// returns a random number between X and Y
        /// </summary>
        public static float RandomFloatInRange(Vector2 Range)
        { return RandomFloat * (Range.Y - Range.X) + Range.X; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Probability">Probability of returning true (values over 1 always return true, values under 0 always return false)</param>
        /// <returns>a random bool based on probability</returns>
        public static bool RandomBoolUsingProbability(float Probability)
        { return (RandomFloat <= Probability); }

        /// <summary>
        /// a random point within a circle of size Radius
        /// </summary>
        /// <param name="Radius">determines the size of the circle</param>
        /// <returns>a random point within a circle</returns>
        public static Vector2 RandomPoint(float Radius)
        {
            float t = 2 * MathHelper.Pi * RandomFloat;
            float u = RandomFloat + RandomFloat;
            if (u > 1)
            { Radius = Radius * (2 - u); }
            else
            { Radius = Radius * u; }
            return new Vector2((float)(Radius * Math.Cos(t)), (float)(Radius * Math.Sin(t)));
        }

        public static void AddPlayerEnchant(EnchantType enchant)
        {
            int hasEnchant = -1;
            for (int i = 0; i < jamie.enchantList.Count; i++)
            {
                if (jamie.enchantList[i].enchantType == enchant)
                {
                    hasEnchant = i;
                }
            }
            if (hasEnchant >= 0)
            {
                jamie.enchantList[hasEnchant].Upgrade();
                WriteLine(jamie.enchantList[hasEnchant].enchantType + " upgraded");
            }
            else
            {
                jamie.enchantList.Add(new Enchant(enchant, 1));
                WriteLine(enchant + " added");
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
                case Side.Bottom:
                    if (rect1.Right > rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom >= rect2.Top && rect1.Left < rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.Right:
                    if (rect1.Right >= rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top && rect1.Left < rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.Left:
                    if (rect1.Right > rect2.Left && rect1.Top < rect2.Bottom && rect1.Bottom > rect2.Top && rect1.Left <= rect2.Right)
                    {
                        return true;
                    }
                    break;
                case Side.Top:
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
        }

        public static float Distance(Point p1, Point p2)
        { return (float)Math.Sqrt(DistanceSquared(p1, p2)); }

        public static float DistanceSquared(Vector2 v1, Vector2 v2)
        {
            float tempX = (v2.X - v1.X);
            float tempY = (v2.Y - v1.Y);
            return (tempX * tempX) + (tempY * tempY);
        }

        public static float Distance(Vector2 v1, Vector2 v2)
        { return (float)Math.Sqrt(DistanceSquared(v1, v2)); }

        public static float Distance(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (float)Math.Sqrt(DistanceSquared(rectangle1, rectangle2));
        }

        public static float DistanceSquared(Rectangle rectangle1, Rectangle rectangle2)
        {
            Point p1 = Point.Zero;
            Point p2 = Point.Zero;

            if (rectangle1.Left > rectangle2.Right)
            {//rectangle1 is to the right of rectangle2
                p1.X = rectangle1.Left;
                p2.X = rectangle2.Right;
            }
            else if (rectangle2.Left > rectangle1.Right)
            {//rectangle2 is to the right of rectangle1
                p1.X = rectangle1.Right;
                p2.X = rectangle2.Left;
            }
            //else
            //{//X values overlap
            //    //leave as zero
            //}

            if (rectangle1.Bottom < rectangle2.Top)
            {//rectangle1 is above rectangle2
                p1.Y = rectangle1.Bottom;
                p2.Y = rectangle2.Top;
            }
            else if (rectangle2.Bottom < rectangle1.Top)
            {//rectangle1 is below rectangle2
                p1.Y = rectangle1.Top;
                p2.Y = rectangle2.Bottom;
            }
            //else
            //{//Y values overlap
            //    //leave as zero
            //}

            return DistanceSquared(p1, p2);
        }

        public static float DistanceSquared(Rectangle rectangle, Point point)
        {
            Point p1 = Point.Zero;

            if (rectangle.Left > point.X)
            {//rectangle1 is to the right of rectangle2
                p1.X = rectangle.Left;
            }
            else if (rectangle.Right < point.X)
            {//rectangle2 is to the right of rectangle1
                p1.X = rectangle.Right;
            }
            else
            {//point is inside rectangle
                point.X = 0;
            }

            if (rectangle.Bottom < point.Y)
            {//rectangle1 is above rectangle2
                p1.Y = rectangle.Bottom;
            }
            else if (rectangle.Top > point.Y)
            {//rectangle1 is below rectangle2
                p1.Y = rectangle.Top;
            }
            else
            {//point is inside rectangle
                point.Y = 0;
            }

            return DistanceSquared(p1, point);
        }

        public static float DistanceSquared(Rectangle rectangle, Vector2 vector)
        {
            Vector2 v1 = Vector2.Zero;

            if (rectangle.Left > vector.X)
            {//rectangle1 is to the right of rectangle2
                v1.X = rectangle.Left;
            }
            else if (rectangle.Right < vector.X)
            {//rectangle2 is to the right of rectangle1
                v1.X = rectangle.Right;
            }
            else
            {//point is inside rectangle
                vector.X = 0;
            }

            if (rectangle.Bottom < vector.Y)
            {//rectangle1 is above rectangle2
                v1.Y = rectangle.Bottom;
            }
            else if (rectangle.Top > vector.Y)
            {//rectangle1 is below rectangle2
                v1.Y = rectangle.Top;
            }
            else
            {//point is inside rectangle
                vector.Y = 0;
            }

            return DistanceSquared(v1, vector);
        }

        public static float Distance(Rectangle rectangle, Point point)
        {
            return (float)Math.Sqrt(DistanceSquared(rectangle, point));
        }

        public static float Distance(Rectangle rectangle, Vector2 vector)
        {
            return (float)Math.Sqrt(DistanceSquared(rectangle, vector));
        }

        public static float UnidirectionalDistance(float float1, float float2)
        {
            return Math.Abs(float1 - float2);
        }

        public static int UnidirectionalDistance(int int1, int int2)
        {
            return Math.Abs(int1 - int2);
        }

        /// <summary>
        /// set X to true to test horizontal distance, or set X to false to test vertical distance
        /// </summary>
        public static int UnidirectionalDistance(Rectangle rectangle1, Rectangle rectangle2, bool X)
        {
            if (X)
            {
                return XDistance(rectangle1, rectangle2);
            }
            else
            {
                return YDistance(rectangle1, rectangle2);
            }
        }

        public static int XDistance(Rectangle rectangle1, Rectangle rectangle2)
        {
            int Shortest = UnidirectionalDistance(rectangle1.Left, rectangle2.Right);
            int temp;
            temp = UnidirectionalDistance(rectangle2.Left, rectangle1.Right);
            if (temp < Shortest)
            { Shortest = temp; }
            temp = UnidirectionalDistance(rectangle2.Left, rectangle1.Left);
            if (temp < Shortest)
            { Shortest = temp; }
            temp = UnidirectionalDistance(rectangle2.Right, rectangle1.Right);
            if (temp < Shortest)
            { Shortest = temp; }
            return Shortest;
        }

        public static int YDistance(Rectangle rectangle1, Rectangle rectangle2)
        {
            //if (rectangle1.Bottom < rectangle2.Top)
            //{//rectangle1 is above rectangle2
            //    return UnidirectionalDistance(rectangle1.Bottom, rectangle2.Top);
            //}
            //else if (rectangle2.Bottom < rectangle1.Top)
            //{//rectangle1 is below rectangle2
            //    return UnidirectionalDistance(rectangle2.Bottom, rectangle1.Top);
            //}
            //else
            //{//Y values overlap
            //    return 0;
            //}
            int Shortest = UnidirectionalDistance(rectangle1.Top, rectangle2.Bottom);
            int temp;
            temp = UnidirectionalDistance(rectangle2.Top, rectangle1.Bottom);
            if (temp < Shortest)
            { Shortest = temp; }
            temp = UnidirectionalDistance(rectangle2.Top, rectangle1.Top);
            if (temp < Shortest)
            { Shortest = temp; }
            temp = UnidirectionalDistance(rectangle2.Bottom, rectangle1.Bottom);
            if (temp < Shortest)
            { Shortest = temp; }
            return Shortest;
        }

        ///<summary>
        ///returns rectangle1's side that is closest to rectangle2
        ///</summary>
        public static Side SideClosest(Rectangle rectangle1, Rectangle rectangle2)
        {
            Side Closest = 0;
            int ClosestDistance = int.MaxValue;
            int temp;
            temp = UnidirectionalDistance(rectangle1.Bottom, rectangle2.Bottom);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Bottom;
            }
            temp = UnidirectionalDistance(rectangle1.Bottom, rectangle2.Top);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Bottom;
            }
            temp = UnidirectionalDistance(rectangle1.Left, rectangle2.Left);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Left;
            }
            temp = UnidirectionalDistance(rectangle1.Left, rectangle2.Right);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Left;
            }
            temp = UnidirectionalDistance(rectangle1.Right, rectangle2.Right);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Right;
            }
            temp = UnidirectionalDistance(rectangle1.Right, rectangle2.Left);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Right;
            }
            temp = UnidirectionalDistance(rectangle1.Top, rectangle2.Top);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Top;
            }
            temp = UnidirectionalDistance(rectangle1.Top, rectangle2.Bottom);
            if (temp < ClosestDistance)
            {
                ClosestDistance = temp;
                Closest = Side.Top;
            }


            return Closest;
        }

        /// <summary>
        /// returns, relative to rectangle1, in which direction rectangle2 is.
        /// returns Direction.Forward if there is overlap.
        /// </summary>
        public static Direction Directions(Rectangle Rectangle1, Rectangle Rectangle2)
        {
            return Directions(Rectangle1.Center, Rectangle2.Center);
        }

        /// <summary>
        /// returns, relative to Point1, in which direction Point2 is.
        /// returns Direction.Forward if Point1.X == Point2.X.
        /// </summary>
        public static Direction Directions(Point Point1, Point Point2)
        {
            if (Point1.X > Point2.X)
            {// Point2 is to the left of Point1
                return Direction.Left;
            }
            else if (Point2.X > Point1.X)
            {// Point2 is to the right of Point1
                return Direction.Right;
            }
            else
            {// values are the same
                return Direction.Forward;
            }
        }

        /// <summary>
        /// returns, relative to v1, in which direction v2 is.
        /// returns Direction.Forward if v1.X == v2.X.
        /// </summary>
        public static Direction Directions(Vector2 v1, Vector2 v2)
        {
            if (v1.X > v2.X)
            {// Point2 is to the left of Point1
                return Direction.Left;
            }
            else if (v2.X > v1.X)
            {// Point2 is to the right of Point1
                return Direction.Right;
            }
            else
            {// values are the same
                return Direction.Forward;
            }
        }

        /// <summary>
        /// returns, relative to v1, in which direction Point2 is.
        /// returns Direction.Forward if v1.X == Point2.X.
        /// </summary>
        public static Direction Directions(Vector2 v1, Point Point1)
        {
            if (v1.X > Point1.X)
            {// Point2 is to the left of Point1
                return Direction.Left;
            }
            else if (Point1.X > v1.X)
            {// Point2 is to the right of Point1
                return Direction.Right;
            }
            else
            {// values are the same
                return Direction.Forward;
            }
        }

        private static bool GetKey(Keys key)
        {
            return (keyboardState.IsKeyDown(key));
        }

        private static bool GetKeyDown(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key));
        }

        private static bool GetKeyUp(Keys key)
        {
            return (keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key));
        }

        private static bool GetButton(Buttons button)
        {
            return (gamePadState.IsButtonDown(button));
        }

        private static bool GetButtonDown(Buttons button)
        {
            return (gamePadState.IsButtonDown(button) && previousGamePadState.IsButtonUp(button));
        }

        private static bool GetButtonUp(Buttons button)
        {
            return (gamePadState.IsButtonUp(button) && previousGamePadState.IsButtonDown(button));
        }

        public string TimerText(double timer)
        {
            return ((int)(timer / 60)).ToString("00") + ":" + (timer % 60).ToString(timerAccuracy);
        }

        public void FizzBuzz(int loops)
        {
            bool fizz;
            bool buzz;

            for (int i = 1; i <= loops; i++)
            {
                fizz = (i % 3 == 0);
                buzz = (i % 5 == 0);

                Console.WriteLine();
                if (fizz || buzz)
                {
                    if (fizz)
                    { Console.Write("fizz"); }
                    if (buzz)
                    { Console.Write("buzz"); }
                }
                else
                { Console.Write(i); }
            }
        }

        public static void PlayerDeath()
        {
            deathTimeScale = timeScale;
            //timeScale = timeScale / 2f;
            deathscreenTimer = 0;
            if (onslaughtMode)
            {
                if (onslaughtSpawner.wave > savefile.bestOnslaughtWave)
                {
                    savefile.bestOnslaughtWave = onslaughtSpawner.wave;
                    savefile.bestOnslaughtWaveLevel = currentLevel;
                }
            }
            //else { }
            int loselisttimerindex = savefile.loseList.IndexOf(currentLevel);
            if (loselisttimerindex >= 0)
            {
                if (timer > savefile.timerLoseList[loselisttimerindex])
                { savefile.timerLoseList[loselisttimerindex] = timer; }
            }
            else
            {
                savefile.loseList.Add(currentLevel);
                savefile.timerLoseList.Add(timer);
            }

            WriteLine("PlayerDeath");
            savefile.Save(autosave);

            //if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
            //{ Irbis.doneEvent.Set(); }
            //doneEvent.WaitOne();
            //game.LoadLevel(currentLevel, true);
            //Irbis.pendingThreads = 0;
            //jamie.Respawn(initialPos);
        }

        public void DeathScreen()
        {
            if (deathscreenTimer < deathscreenTimerMax)
            {
                deathscreenTimer += RealTime;
                deathBackgroundOpacity = (deathscreenTimer - 2) / 3f;

                timeScale = (deathTimeScale * (1 + deathBackgroundOpacity)) / 2;

                if (cameraShakeDuration > 0)
                { CameraShake(); }
                else if (cameraShakeLerpTime > 0)
                { ReturnCamera(); }
                if (smartCamera)
                {
                    mainCamera.X = Lerp(mainCamera.X, jamie.TrueCenter.X * screenScale, cameraLerpXSpeed * DeltaTime);
                    mainCamera.Y = Lerp(mainCamera.Y, jamie.TrueCenter.Y * screenScale, cameraLerpYSpeed * DeltaTime);

                    cameraLockon = Lerp(cameraLockon, Vector2.Zero, cameraLockonSpeed * DeltaTime);

                    background.M31 = foreground.M41 = boundingBox.Center.X - (mainCamera.X + camera.X + cameraLockon.X);
                    background.M32 = foreground.M42 = boundingBox.Center.Y - (mainCamera.Y + camera.Y + cameraLockon.Y);
                }
                else
                {
                    if (cameraLerpSetting)
                    {
                        if (boundingBox.Right <= screenSpacePlayerPos.X)
                        { mainCamera.X += Lerp(boundingBox.Right, screenSpacePlayerPos.X, cameraLerpXSpeed * DeltaTime) - boundingBox.Right; }
                        else if (boundingBox.Left >= screenSpacePlayerPos.X)
                        { mainCamera.X += Lerp(boundingBox.Left, screenSpacePlayerPos.X, cameraLerpXSpeed * DeltaTime) - boundingBox.Left; }
                        if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                        { mainCamera.Y += Lerp(boundingBox.Bottom, screenSpacePlayerPos.Y, cameraLerpYSpeed * DeltaTime) - boundingBox.Bottom; }
                        else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                        { mainCamera.Y += Lerp(boundingBox.Top, screenSpacePlayerPos.Y, cameraLerpYSpeed * DeltaTime) - boundingBox.Top; }
                    }
                    else
                    {
                        if (boundingBox.Right <= screenSpacePlayerPos.X)
                        { mainCamera.X += screenSpacePlayerPos.X - boundingBox.Right; }
                        else if (boundingBox.Left >= screenSpacePlayerPos.X)
                        { mainCamera.X += screenSpacePlayerPos.X - boundingBox.Left; }
                        if (boundingBox.Bottom <= screenSpacePlayerPos.Y)
                        { mainCamera.Y += screenSpacePlayerPos.Y - boundingBox.Bottom; }
                        else if (boundingBox.Top >= screenSpacePlayerPos.Y)
                        { mainCamera.Y += screenSpacePlayerPos.Y - boundingBox.Top; }
                    }

                    background.M31 = foreground.M41 = halfResolution.X - (mainCamera.X + camera.X);
                    background.M32 = foreground.M42 = halfResolution.Y - (mainCamera.Y + camera.Y);
                }
            }
            else
            {
                //if (Interlocked.Decrement(ref Irbis.pendingThreads) <= 0)
                //{ Irbis.doneEvent.Set(); }
                //doneEvent.WaitOne();
                jamie.Respawn(initialPos);
                CameraShake(0, 0);
                enemyList.Clear();
                if (onslaughtMode)
                { onslaughtSpawner = new OnslaughtSpawner(); }
                else
                { SummonBoss(bossName, bossSpawn); }
                CenterCamera();
                deathBackgroundOpacity = 1;
                timeScale = deathTimeScale;
                jamie.inputEnabled = true;
            }
        }

        public void SummonGenericEnemy()
        {
            if (enemySpawnPoints.Count > 0)
            {
                int spawnpoint = (int)(RAND.NextDouble() * enemySpawnPoints.Count);
                Enemy tempEnemy = new Enemy("Enemy " + (enemyList.Count + 1), enemy0Tex, enemySpawnPoints[spawnpoint], 100f, 10f, 200f, 0.4f);
                //eList.Add(tempEnemy);
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
            if (enemySpawnPoints.Count > 0)
            {
                int spawnpoint = (int)(RAND.NextDouble() * enemySpawnPoints.Count);
                Enemy tempEnemy = new Enemy("Enemy " + (enemyList.Count + 1), enemy0Tex, enemySpawnPoints[spawnpoint], health, damage, speed, 0.4f);
                //eList.Add(tempEnemy);
                enemyList.Add(tempEnemy);
                //collisionObjects.Add(tempEnemy);
                WriteLine("enemy spawned at " + enemySpawnPoints[spawnpoint] + ". health:" + health + " damage:" + damage + " speed:" + speed + ". timer:" + Timer);
            }
            else
            {
                WriteLine("Error, no spawn points");
            }
        }

        public void SummonBoss(string Boss, Vector2? Location)
        {
            if (!string.IsNullOrEmpty(Boss))
            {
                Irbis.WriteLine("Summoning boss name:" + Boss + " at location:" + Location);
                switch (Boss.Trim().ToLower())
                {
                    case "lizard":
                        {
                            loadingBar.Value += 1;
                            LizardGuy tempLizardGuy = new LizardGuy(LoadTexture("Lizard"), Location, 999, 50, 500, null, 0.45f);
                            loadingBar.Value += 8;
                            enemyList.Add(tempLizardGuy);
                            collisionObjects.Add(tempLizardGuy);
                            loadingBar.Value += 1;
                        }
                        break;
                    case "wizard":
                        {
                            loadingBar.Value += 1;
                            WizardGuy tempWizardGuy;
                            //if (Location != null)
                            //{ tempWizardGuy = new WizardGuy(LoadTexture("wizard"), null, 500, 50, new Vector2[] { (Vector2)Location }, null, 0.45f); }
                            //else
                            { tempWizardGuy = new WizardGuy(LoadTexture("wizard"), 2, 400, 50, null, null, 0.45f); }
                            loadingBar.Value += 8;
                            enemyList.Add(tempWizardGuy);
                            //collisionObjects.Add(tempWizardGuy);
                            loadingBar.Value += 1;
                        }
                        break;
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void KillEnemy(IEnemy killMe)
        {
            WriteLine("removing enemy. index:" + enemyList.IndexOf(killMe));

            enemyList[enemyList.IndexOf(killMe)].Death();

            if (enemyList.Contains(killMe))
            {
                enemyList.Remove(killMe);
                WriteLine("successfully removed from enemyList.");
            }
            if (collisionObjects.Contains(killMe))
            {
                if (jamie.collided.Contains(killMe))
                { jamie.RemoveCollision(killMe); }
                collisionObjects.Remove(killMe);
                WriteLine("successfully removed from collisionObjects.");
            }
            if (onslaughtMode) { onslaughtSpawner.EnemyKilled(); }
            WriteLine("done.");
        }

        public void Load(PlayerSettings settings)
        {
            attackKey = settings.attackKey;
            altAttackKey = settings.altAttackKey;
            shockwaveKey = settings.shockwaveKey;
            altShockwaveKey = settings.altShockwaveKey;
            shieldKey = settings.shieldKey;
            altShieldKey = settings.altShieldKey;
            jumpKey = settings.jumpKey;
            altJumpKey = settings.altJumpKey;
            upKey = settings.upKey;
            altUpKey = settings.altUpKey;
            downKey = settings.downKey;
            altDownKey = settings.altDownKey;
            leftKey = settings.leftKey;
            altLeftKey = settings.altLeftKey;
            rightKey = settings.rightKey;
            altRightKey = settings.altRightKey;
            rollKey = settings.rollKey;
            altRollKey = settings.altRollKey;
            potionKey = settings.potionKey;
            altPotionKey = settings.altPotionKey;
            useKey = settings.useKey;
            altUseKey = settings.altUseKey;

            GPattackKey = settings.GPattackKey;
            GPshockwaveKey = settings.GPshockwaveKey;
            GPshieldKey = settings.GPshieldKey;
            GPjumpKey = settings.GPjumpKey;
            GPupKey = settings.GPupKey;
            GPdownKey = settings.GPdownKey;
            GPleftKey = settings.GPleftKey;
            GPrightKey = settings.GPrightKey;
            GProllKey = settings.GProllKey;
            GPpotionKey = settings.GPpotionKey;
            GPuseKey = settings.GPuseKey;

            analogCutoff = settings.analogCutoff;

            cameraLerp = cameraLerpSetting = settings.cameraLerpSetting;
            cameraLerpXSpeed = settings.cameraLerpXSpeed;
            cameraLerpYSpeed = settings.cameraLerpYSpeed;

            easyWalljumpMode = settings.easyWalljumpMode;
            lightingEnabled = settings.lighting;

            //debug = playerSettings.debug;
            minSqrDetectDistance = settings.minSqrDetectDistance;
            cameraShakeSetting = settings.cameraShakeSetting;
            cameraSwingSetting = settings.cameraSwingSetting;
            swingDuration = settings.swingDuration;
            swingMagnitude = settings.swingMagnitude;
            timerAccuracy = settings.timerAccuracy;
            graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = settings.vSync;
            tempResolution = settings.resolution;
            masterAudioLevel = settings.masterAudioLevel;
            musicLevel = settings.musicLevel;
            soundEffectsLevel = settings.soundEffectsLevel;

            if (!resetRequired)
            {
                tempResolution = settings.resolution;

                if (tempResolution != Point.Zero)
                { SetResolution(tempResolution); }
                else
                {
                    tempResolution = new Point(graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
                    SetResolution(tempResolution);
                }
                graphics.IsFullScreen = settings.fullscreen;
            }

            SetScreenScale(settings.screenScale);

            projection.M11 = screenScale / (resolution.X);
            projection.M22 = screenScale / (resolution.Y);
            //projection = Matrix.CreateOrthographicOffCenter(0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height, 0, 0, 1);

            graphics.SynchronizeWithVerticalRetrace = IsFixedTimeStep = settings.vSync;
            graphics.ApplyChanges();

            boundingBox = settings.boundingBox;
            if (boundingBox == Rectangle.Empty)
            {
                boundingBox.Width = (int)(resolution.X * (1f / 3f));
                boundingBox.Height = (int)(resolution.Y * (1f / 4f));
                boundingBox.X = (int)(halfResolution.X) - boundingBox.Width / 2;
                boundingBox.Y = (int)(resolution.Y * 0.64f) - boundingBox.Height / 2;
            }
            halfBoundingBox = new Point(boundingBox.Width / 2, boundingBox.Height / 2);

            if (jamie != null)
            { jamie.Load(settings); }
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
            return (GetShockwaveKeyDown || GetAttackKeyDown || GetEnterKeyDown);
        }

        public static float Clamp(float x, float lowerlimit, float upperlimit)
        {
            if (x < lowerlimit)
            { x = lowerlimit; }
            if (x > upperlimit)
            { x = upperlimit; }
            return x;
        }

        public static float Lerp(float initial, float target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            return initial + (target - initial) * amount;
        }

        public static float LerpNoClamp(float initial, float target, float amount)
        { return initial + (target - initial) * amount; }

        public static Vector2 LerpNoClamp(Vector2 initial, Vector2 target, float amount)
        { return initial + (target - initial) * amount; }

        public static Vector2 Lerp(Vector2 initial, Vector2 target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            return initial + (target - initial) * amount;
        }

        public static float SmoothStep(float initial, float target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            amount = amount * amount * (3 - 2 * amount); // 3x^2 - 2x^3
            return initial + (target - initial) * amount;
        }

        public static Vector2 SmoothStep(Vector2 initial, Vector2 target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            amount = amount * amount * (3 - 2 * amount); // 3x^2 - 2x^3
            return initial + (target - initial) * amount;
        }

        public static float SmootherStep(float initial, float target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            // 6x^5 - 15x^4 + 10x^3
            amount = amount * amount * amount * (amount * (amount * 6 - 15) + 10);
            return initial + (target - initial) * amount;
        }

        public static Vector2 SmootherStep(Vector2 initial, Vector2 target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            // 6x^5 - 15x^4 + 10x^3
            amount = amount * amount * amount * (amount * (amount * 6 - 15) + 10);
            return initial + (target - initial) * amount;
        }

        public static float SmoothestStep(float initial, float target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            // -20x^7 + 70x^6 - 84x^5 + 35x^4
            amount = amount * amount * amount * amount * (amount * (amount * (amount * (-20) + 70) - 84) + 35);
            return initial + (target - initial) * amount;
        }

        public static Vector2 SmoothestStep(Vector2 initial, Vector2 target, float amount)
        {
            amount = Clamp(amount, 0.0f, 1.0f);
            // -20x^7 + 70x^6 - 84x^5 + 35x^4
            amount = amount * amount * amount * amount * (amount * (amount * (amount * (-20) + 70) - 84) + 35);
            return initial + (target - initial) * amount;
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
                        { encounteredMapChar = true; }
                        else
                        { return false; }
                    }
                }
                if (!encounteredMapChar)
                { return false; }
            }
            else
            { return false; }
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

        public static string GetLevelName(string level)
        {
            switch (level.ToLower())
            {
                case "c1b1":
                    return "Caves";
                case "c1b2":
                    return "Plains";
                case "c1b3":
                    return "Slope";
                case "c1b4":
                    return "Peak";
                case "c1b5":
                    return "Keep";
                case "c1o0":
                    return "Onslaught Test";
            }
            return level;
        }

        private void OpenConsole()
        {
            textInputBuffer = string.Empty;
            acceptTextInput = console = !console;
            if (consoleTex == null)
            {
                Texture2D tempConsoleTex = LoadTexture("console texture");
                float tempScale = 0f;
                Vector2 tempLocation = Vector2.Zero;
                if (((float)consoleRect.Width / (float)tempConsoleTex.Bounds.Width) > ((float)consoleRect.Height / (float)tempConsoleTex.Bounds.Height))
                {
                    tempScale = (float)consoleRect.Width / (float)tempConsoleTex.Bounds.Width;
                }
                else
                {
                    tempScale = (float)consoleRect.Height / (float)tempConsoleTex.Bounds.Height;
                }
                tempLocation.X = -((((float)tempConsoleTex.Bounds.Width * tempScale) - screenspace.Width) / 2f);

                RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, consoleRect.Width, consoleRect.Height); ;
                GraphicsDevice.SetRenderTarget(renderTarget);
                GraphicsDevice.Clear(Color.Transparent);
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, Matrix.Identity);
                spriteBatch.Draw(tempConsoleTex, tempLocation, null, Color.White, 0f, Vector2.Zero, tempScale, SpriteEffects.None, 0.5f);
                spriteBatch.End();
                GraphicsDevice.SetRenderTarget(null);
                consoleTex = (Texture2D)renderTarget;

            }
            if (jamie != null)
            {
                //jamie.inputEnabled = !console;
                //framebyframe = console;
            }
            consoleLine = developerConsole.lines + 1;
            consoleMoveTimer = 1 - consoleMoveTimer;
        }

        private void MoveConsole()
        {
            consoleMoveTimer -= (float)elapsedTime;
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

        private void UpdateConsole()
        {
            consoleWriteline.Update(textInputBuffer, true);
            if ((GetKey(Keys.LeftControl) || GetKey(Keys.RightControl)) && GetKeyDown(Keys.V) /*&& System.Windows.Forms.Clipboard.ContainsText()*/)
            {
                textInputBuffer += GetClipboardText();
            }
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
            if (GetKey(Keys.Up) || GetKey(Keys.Down))
            {
                consoleLineChangeTimer += RealTime;
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


            if (GetKeyDown(Keys.Enter))
            {
                consoleWriteline.Update(string.Empty, true);
                ConsoleParser(textInputBuffer);
                textInputBuffer = string.Empty;
            }

            if (GetPauseKeyDown)
            { OpenConsole(); }
        }

        public static string GetClipboardText()
        {
            try
            {
                string clipboardData = null;
                Exception threadEx = null;
                Thread staThread = new Thread(
                    delegate ()
                    {
                        try
                        { clipboardData = System.Windows.Forms.Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text); }
                        catch (Exception ex)
                        { threadEx = ex; }
                    });
                staThread.SetApartmentState(ApartmentState.STA);
                staThread.Start();
                staThread.Join();
                return clipboardData;
            }
            catch (Exception e)
            { return e.Message; }
        }

        public static void ExportConsole()
        {
            if (meanFPS != null)
            {
                Irbis.WriteLine("meanFPS: " + meanFPS.Framerate);
                Irbis.WriteLine(" minFPS: " + minFPS);
                Irbis.WriteLine("at time: " + minFPStime);
                Irbis.WriteLine(" maxFPS: " + maxFPS);
                Irbis.WriteLine("at time: " + maxFPStime);
                Irbis.WriteLine(" median: " + ((minFPS + maxFPS) / 2));
            }

            ExportString(developerConsole.Konsole);
        }

        private void CleanConsole()
        {
            while (developerConsole.lines > 10000)
            {
                developerConsole.Clear();
            }
        }

        public static void ExportString(string stringtoexport)
        {
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

        public static void WriteLine(string line)
        {
            lock (writelock)
            {
                developerConsole.WriteLine(line);
                consoleLine = developerConsole.lines + 1;
            }
        }

        public static void WriteLine()
        {
            lock (writelock)
            {
                developerConsole.WriteLine();
                consoleLine = developerConsole.lines + 1;
            }
        }

        public static void Write(string line)
        {
            lock (writelock)
            { developerConsole.Write(line); }
        }

        private static string GetDotNet()
        {
            using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\"))
            {
                int releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                if (releaseKey >= 393273)
                {
                    return "4.6 RC or later";
                }
                if ((releaseKey >= 379893))
                {
                    return "4.5.2 or later";
                }
                if ((releaseKey >= 378675))
                {
                    return "4.5.1 or later";
                }
                if ((releaseKey >= 378389))
                {
                    return "4.5 or later";
                }
                // This line should never execute. A non-null release key should mean 
                // that 4.5 or later is installed. 
                return "No 4.5 or later version detected";
            }
        }

        public static void SetScreenScale(float newScale)
        {
            if (newScale > 0)
            { screenScale = newScale; }
            else
            { screenScale = DefaultScreenScale; }      //this decides the screen scale on load

            if ((int)(screenScale / 2) == (screenScale / 2))
            { textScale = (int)(screenScale / 2); }
            else
            { textScale = (int)(screenScale / 2) + 1; }

            //change the location of everything
            if (bars != null)
            { bars = new Bars(LoadTexture("bar health"), LoadTexture("bar shield"), LoadTexture("bar energy"), LoadTexture("bar potion fill 1"), LoadTexture("bar enemy fill"), LoadTexture("shieldBar"), LoadTexture("bars"), LoadTexture("bar enemy"), new[] { LoadTexture("bar potion 1"), LoadTexture("bar potion 2"), LoadTexture("bar potion 3") }); }

            if (loadingBar != null)
            { loadingBar = new UIElementSlider(Direction.Left, new Rectangle(0, (int)(resolution.Y - (font.charHeight + 1) * textScale), resolution.X, (int)((font.charHeight + 1) * textScale)), Point.Zero, Direction.Forward, 100, new Color(038, 118, 213), null, null, Color.Black, nullTex, null, null, true, font, false, 0.8f, 0.799f, 0.801f, 0.803f); }

            Texture2D temptex = Irbis.LoadTexture("patreon-character");
            fontLogos[0] = ResizeTexture(temptex, (12f * textScale) / temptex.Width, false);
            temptex = Irbis.LoadTexture("twitter-character");
            fontLogos[1] = ResizeTexture(temptex, (12f * textScale) / temptex.Width, false);
            temptex = Irbis.LoadTexture("furaffinity-character");
            fontLogos[2] = ResizeTexture(temptex, (12f * textScale) / temptex.Width, false);

            maxButtonsOnScreen = (int)(resolution.Y / (25f * 2 * textScale));

            foreach (Square s in squareList)
            { s.scale = screenScale; }

            foreach (Square s in backgroundSquareList)
            { s.scale = screenScale; }
        }

        public static void RollCredits()
        {
            sceneIsMenu = creditsActive = rollCredits = true;
            creditsSpeed = 100f;
            creditsOpacity = 0;
            creditsCurrentPos = new Vector2(resolution.X * (9f/10f), resolution.Y + font.charHeight*2);
            credits = // ᵵ = twitter logo  ℗ = patreon logo  ᵮ = furaffinity
@"Game Design by Liquid Nitrogen    



Programming & Design
---
J. ""Darius"" Miu


Art Design
---
Pulex
ᵵLordPulex


Animation
---
Ivan Romero Cuenca
ᵵKrampusParty


Music
---
""Relaxing""
Bensound.com


Without your love and support, I'd never have gotten this far
---
Snyx, my boyfriend
ᵵSnyxTheSnynx


℗Patrons
---
kieran
ᵵhomphs

Pulex
ᵮPulex

Ventain
ᵵVentain

Serephita
ᵵserephita

Mirage
ᵵmiirage

Nelly
ᵮnelly

RykerHusky
ᵵRykerHusky



---
Thank you all so much";
/*

RykerHusky
ᵵRykerHusky
*/

            creditsPrint = new Print(resolution.X, font, Color.White, false, Point.Zero, Direction.Right, 0.009f);
            creditsPrint.Write(credits);
        }

        public void Credits()
        {
            creditsCurrentPos.Y -= creditsSpeed * DeltaTime;
            if (creditsCurrentPos.Y <= -(creditsPrint.lines * creditsPrint.characterHeight * textScale))
            {
                creditsOpacity -= DeltaTime;
                if (creditsOpacity < 0)
                { creditsActive = rollCredits = false; LoadMenu(0, 0, false); }
            }
            else if (creditsOpacity < 1f)
            { creditsOpacity += DeltaTime; }
            creditsSpeed += (mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue);
        }

        public void PauseCredits()
        {
            rollCredits = !rollCredits;
            if (rollCredits)
            {
                printList.Clear();
                buttonList.Clear();
                infoText.Clear();
                infoTextTimer = 0;
            }
            else
            { LoadMenu(0, 3, false); }
        }

        public string Help()
        {
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
quit  ---------------  exits                     fps  --------------------  shows framerate
";

            string returnhelp = string.Empty;
            foreach (char c in help)
            {
                if (!c.Equals('\u000D'))
                { returnhelp += c; }
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
            Crash = false;
            Exit();
        }

        public void PrintVersion()
        {
            WriteLine("    Project: Irbis (" + versionTy + ")\n    " + versionID + " v" + versionNo);
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
                { return new Point(Xresult, Yresult); }
                else
                { Console.WriteLine("error: Point could not be parsed"); }
            }
            else
            { Console.WriteLine("error: Point could not be parsed"); }
            return Point.Zero;
        }

        private static Vector2 Vector2Parser(string value)
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
            if (float.TryParse(Xval, out float Xresult))
            {
                if (float.TryParse(Yval, out float Yresult))
                { return new Vector2(Xresult, Yresult); }
                else
                { Console.WriteLine("error: Point could not be parsed"); }
            }
            else
            { Console.WriteLine("error: Point could not be parsed"); }
            return Vector2.Zero;
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
            line.Trim();
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
                    { statement += c; }
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
                        { stage = -1; }
                        if (stage > 0)
                        { value += c; }
                    }
                    if (stage == 2)
                    {
                        if (c.Equals('\u003d'))
                        { stage = 3; }
                        else
                        { /* do nothing */ }
                    }
                    if (stage == 1)
                    {
                        if (c.Equals('\u005d'))
                        { stage = 2; }
                        else
                        { extra += c; }
                    }
                    if (stage == 0)
                    {
                        if (c.Equals('\u003d'))
                        { stage = 3; }
                        else
                        {
                            if (c.Equals('\u005b'))
                            { stage = 1; }
                            else
                            { variable += c; }
                        }
                    }
                }

                switch (variable.ToLower())
                {
                    case "speed":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.speed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "jumptimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.jumpTimeMax = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "idletimemax":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.idleTimeMax = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "maxhealth":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.maxHealth = floatResult;
                            Bars.healthBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "maxshield":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.maxShield = floatResult;
                            Bars.shieldBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "maxenergy":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.maxEnergy = floatResult;
                            Bars.energyBar.maxValue = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "superShockwaveHoldtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.superShockwaveHoldtime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "shockwaveeffectivedistance":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.shockwaveEffectiveDistance = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "shockwavestuntime":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.shockwaveStunTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "invulnerablemaxtime":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.invulnerableMaxTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "shieldrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.shieldRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "energyrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.energyRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "healthrechargerate":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.healthRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "energyusablemargin":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.energyUsableMargin = floatResult;
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
                            jamie.shieldAnimationSpeed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "shieldhealingpercentage":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.shieldHealingPercentage = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "cameralerpxspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            cameraLerpXSpeed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "cameralerpyspeed":
                        if (float.TryParse(value, out floatResult))
                        {
                            cameraLerpYSpeed = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "terminalvelocity":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.terminalVelocity = floatResult;
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
                            jamie.potionRechargeRate = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "potionrechargetime":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.potionRechargeTime = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "darkness":
                        if (float.TryParse(value, out floatResult))
                        { darkness = floatResult; }
                        else
                        { WriteLine("darkness current value: " + darkness); }
                        break;
                    case "lightness":
                        if (float.TryParse(value, out floatResult))
                        { darkness = 1 - floatResult; }
                        else
                        { WriteLine("darkness current value: " + darkness); }
                        break;
                    case "gravity":
                        if (float.TryParse(value, out floatResult))
                        { gravity = floatResult; }
                        else
                        { WriteLine("gravity current value: " + gravity); }
                        break;
                    case "lightbrightness":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.lightBrightness = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "lightsize":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.lightSize = floatResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;






                    case "collideroffset":                                                         //place new floats above
                        jamie.standardCollider.Location = PointParser(value);
                        if (jamie.standardCollider.Location == new Point(-0112, -0112))
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "colliderwidth":
                        if (int.TryParse(value, out intResult))
                        {
                            jamie.standardCollider.Width = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "colliderheight":
                        if (int.TryParse(value, out intResult))
                        {
                            jamie.standardCollider.Height = intResult;
                        }
                        else
                        {
                            WriteLine("error: variable \"" + variable + "\" could not be parsed");
                        }
                        break;
                    case "attackcollidersize":
                        jamie.attackColliderSize = PointParser(value);
                        if (jamie.attackColliderSize == new Point(-0112, -0112))
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
                            jamie.maxNumberOfPotions = intResult;
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
                            cameraLerpSetting = boolResult;
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
                            WriteLine("Use this command to summon X number of enemies: \"summonenemies=X\"");
                        }
                        break;
                    case "summonboss":
                        SummonBoss(value, null);
                        //WriteLine("debug: " + debug);
                        break;
                    case "notarget":
                        AIenabled = !AIenabled;
                        foreach (IEnemy e in enemyList)
                        {
                            e.AIenabled = AIenabled;
                        }
                        break;
                    case "noclip":
                        jamie.Noclip();
                        break;
                    case "killall":
                        //eList.Clear();
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
                            WriteLine("squareList[" + i + "] position:" + squareList[i].Position + " collider:" + squareList[i].Collider + " texture:" + squareList[i].texture + " depth:" + squareList[i].depth);
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
                                WriteLine("spawning block with defaultTex texture at " + new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))));
                                Square tempSquare = new Square(defaultTex, Color.White, new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))), null, Vector2.Zero, screenScale, 0.3f);
                                squareList.Add(tempSquare);
                            }
                            else
                            {
                                WriteLine("spawning block with" + value + " texture at " + new Point((int)(mainCamera.X % 32), (int)(mainCamera.Y % 32)));
                                Square tempSquare = new Square(defaultTex, Color.White, new Point((int)(mainCamera.X % 32), (int)(mainCamera.Y % 32)), null, Vector2.Zero, screenScale, 0.3f);
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
                            if (Enum.TryParse(value, out enchantResult) && jamie != null)
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
                            if (Enum.TryParse(value, out enchantResult) && jamie != null)
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
                        jamie.enchantList.Clear();
                        WriteLine("removed all enchants");
                        break;
                    case "enchants":
                        if (jamie.enchantList.Count <= 0)
                        {
                            WriteLine("no enchants");
                        }
                        foreach (Enchant e in jamie.enchantList)
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
                        jamie.ClearCollision();
                        break;
                    case "fps":
                        WriteLine("smart fps: " + smartFPS.Framerate + ", raw fps: " + (1 / DeltaTime));
                        if (recordFPS)
                        {
                            WriteLine("meanfps:" + meanFPS.Framerate);
                            WriteLine(" maxfps:" + maxFPS + " at time:" + maxFPStime);
                            WriteLine(" minfps:" + minFPS + " at time:" + minFPStime);
                        }
                        if (debug <= 0)
                        { debug = 1; }
                        break;
                    case "framerate":
                        goto case "fps";
                    case "timer":
                        WriteLine("timer: " + Timer);
                        break;
                    case "debuglevel":
                        if (true)
                        {
                            Level thislevel = new Level();
                            thislevel.Load(".\\levels\\" + currentLevel + ".lvl");
                            WriteLine(thislevel.ToString());
                        }
                        break;
                    case "moveme":
                        goto case "move";
                    case "move":
                        if (PointParser(value) != Point.Zero)
                        {
                            Point tempPoint = PointParser(value);
                            jamie.position.X += tempPoint.X;
                            jamie.position.Y += tempPoint.X;

                            WriteLine("moved player to " + jamie.position);
                        }
                        break;
                    case "testparticles":
                        particleSystems.Add(new ParticleSystem(new Vector2(0, -10), new Vector2(0, 25), new float[] { 0.2f, 1f, 0.2f },
                        new float[] { 0.1f, 0.1f, 0.05f, 0f }, new float[] { }, 0.01f, new float[] { 0.6f }, new float[] { 10, 25, 0, 0, 0.05f, 0.1f },
                        new Rectangle(PointParser(value), new Point(3, 5)), new Texture2D[] { Irbis.LoadTexture("torchflame") },
                        new Color[] {Color.White,Color.White,Color.Black},new Color[] {Color.White}, new int[] {1,1,3,1}, 0.1f, 0f,1));
                        break;
                    case "particles":
                        for (int i = 0; i < particleSystems.Count; i++)
                        {
                            Irbis.WriteLine("ParticleSystem[" + i + "].spawnArea:" + particleSystems[i].spawnArea);
                        }
                        break;
                    case "grass":
                        for (int i = 0; i < grassList.Count; i++)
                        {
                            Irbis.WriteLine("grass[" + i + "].area:" + grassList[i].area + " depth:" + grassList[i].depth);
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
                                WriteLine(tempPoint.Y + " random ints between 0 and " + tempPoint.X + ": ");
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
                    case "randombool":
                        if (string.IsNullOrWhiteSpace(value))
                        { WriteLine("RandomBool: " + RandomBool); }
                        else
                        {
                            if (Vector2Parser(value) != Vector2.Zero)
                            {
                                Vector2 tempVector = Vector2Parser(value);
                                WriteLine((int)tempVector.Y + " random bools using probability:" + tempVector.X);
                                int truecount = 0;
                                int falsecount = 0;
                                for (int i = 0; i < tempVector.Y; i++)
                                {
                                    bool tempBool = RandomBoolUsingProbability(tempVector.X);
                                    if (tempBool)
                                    { truecount++; }
                                    else
                                    { falsecount++; }
                                    WriteLine("RandomBool[" + i + "](prob:" + tempVector.X + "): " + tempBool);
                                }
                                WriteLine(" true: " + truecount);
                                WriteLine("false: " + falsecount);
                            }
                            else
                            {
                                if (float.TryParse(value, out floatResult))
                                { WriteLine("RandomBool(prob:" + floatResult + "): " + RandomBoolUsingProbability(floatResult)); }
                                else
                                { WriteLine("RandomBool: " + RandomBool); }
                            }
                        }
                        break;
                    case "spawnvending":
                        if (true)
                        {
                            if (string.IsNullOrWhiteSpace(value))
                            {
                                WriteLine("spawning enchant vending machine at " + new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))));
                                VendingMachine tempvend = new VendingMachine(200, VendingType.Enchant, new Rectangle(new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))), new Point(64, 64)), LoadTexture("enchant vending machine"), 0.35f);
                                onslaughtSpawner.vendingMachineList.Add(tempvend);

                            }
                            else
                            {
                                VendingType vendtype;
                                if (Enum.TryParse(value, out vendtype))
                                {
                                    WriteLine("spawning " + value + " vending machine at " + new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))));
                                    VendingMachine tempvend = new VendingMachine(200, vendtype, new Rectangle(new Point((int)(mainCamera.X - (mainCamera.X % 32)), (int)(mainCamera.Y - (mainCamera.Y % 32))), new Point(64, 64)), LoadTexture("enchant vending machine"), 0.35f);
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
                    case "vendings":
                        for (int i = 0; i < onslaughtSpawner.vendingMachineList.Count; i++)
                        {
                            WriteLine("VendingMachine[" + i + "]: " + onslaughtSpawner.vendingMachineList[i]);
                        }
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
                        break;
                    case "removevendingmachine":
                        goto case "removevending";
                    case "player":
                        WriteLine(jamie.ToString());
                        break;
                    case "debugonslaught":
                        if (onslaughtSpawner != null)
                        {
                            WriteLine(onslaughtSpawner.ToString());
                        }
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
                        if (jamie.invulnerable == float.PositiveInfinity)
                        { jamie.invulnerable = 0f; WriteLine("godmode off"); }
                        else
                        { jamie.invulnerable = float.PositiveInfinity; WriteLine("godmode on"); }
                        break;
                    case "buddha":
                        jamie.buddha = !jamie.buddha;
                        WriteLine("buddha mode:" + jamie.buddha);
                        break;
                    case "addpoints":
                        if (int.TryParse(value, out intResult))
                        {
                            if (onslaughtSpawner != null)
                            {
                                onslaughtSpawner.Points += (uint)intResult;
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
                        break;
                    case "projection":
                        WriteLine("basicEffect.Projection:" + MatrixPrinter(projection));
                        break;
                    case "debuglines":
                        WriteLine("lines: " + debuglines.Length);
                        for (int i = 0; i < debuglines.Length; i++)
                        {
                            WriteLine("line[" + i + "]:" + debuglines[i].ToString());
                        }
                        break;
                    case "debugrays":
                        WriteLine("rays: " + debugrays.Length);
                        for (int i = 0; i < debugrays.Length; i++)
                        {
                            WriteLine("ray[" + i + "]:" + debugrays[i].ToString());
                        }
                        break;
                    case "debugshapes":
                        WriteLine("shapes: " + debugshapes.Length);
                        for (int i = 0; i < debugshapes.Length; i++)
                        {
                            WriteLine("shape[" + i + "]:\n" + debugshapes[i].Debug(true));
                        }
                        break;
                    case "mt":
                        goto case "multithreading";
                    case "multithread":
                        goto case "multithreading";
                    case "multithreading":
                        useMultithreading = !useMultithreading;
                        WriteLine("multithreading:" + useMultithreading);
                        break;
                    case "forcetriangulate":
                        for (int i = 0; i < debugshapes.Length; i++)
                        {
                            WriteLine("triangulating debugshapes[" + i + "]...");
                            if (debugshapes[i].Triangulate())
                            { Write(" done."); }
                            else
                            { Write(" failed."); }
                        }
                        break;
                    case "shadowshape":
                        WriteLine(shadowShape.ToString());
                        break;
                    case "fizzbuzz":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            FizzBuzz(100);
                        }
                        else
                        {
                            if (int.TryParse(value, out intResult))
                            {
                                FizzBuzz(intResult);
                            }
                            else
                            {
                                WriteLine("error: rank \"" + value + "\" could not be parsed");
                            }
                        }
                        break;
                    case "timescale":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("Sets the timescale");
                        }
                        else
                        {
                            if (float.TryParse(value, out floatResult))
                            {
                                timeScale = floatResult;
                            }
                            else
                            {
                                WriteLine("error: \"" + value + "\" could not be parsed");
                            }
                        }
                        WriteLine("Current timescale: " + timeScale);
                        break;
                    case "camerashake":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            WriteLine("Causes the camera to shake for (magnitude, duration)");
                        }
                        else
                        {
                            Point tempppoint = PointParser(value);
                            CameraShake(tempppoint.Y, tempppoint.X);
                            WriteLine("Shaking the camera at " + tempppoint.X + " magnitude for " + tempppoint.Y + " seconds");
                        }
                        break;
                    case "ons":
                        goto case "onslaught";
                    case "onslaught":
                        onslaughtMode = !onslaughtMode;
                        WriteLine("onslaught mode: " + onslaughtMode);
                        break;
                    case "displayui":
                        displayUI = !displayUI;
                        WriteLine("displayUI: " + displayUI);
                        break;
                    case "enemies":
                        WriteLine("enemyList currently contains " + enemyList.Count + " entities.\n" +
                            "use \"print enemies\" to print a list");
                        break;
                    case "printenemies":
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            WriteLine("enemyList[" + i + "]:" + enemyList[i] + " Position:" + enemyList[i].Position + " Health:" + enemyList[i].Health + " StunTime:" + enemyList[i].StunTime);
                        }
                        break;
                    case "stunme":
                        if (float.TryParse(value, out floatResult))
                        {
                            jamie.Stun(floatResult);
                            WriteLine("player stunned for \"" + value + "\" seconds");
                        }
                        else
                        {
                            WriteLine("error: \"" + value + "\" could not be parsed");
                        }
                        break;
                    case "<3":
                        WriteLine("℗patreon.com/Ln2");
                        break;
                    case "lighting":
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            lightingEnabled = !lightingEnabled;
                            WriteLine("lightingEnabled:" + lightingEnabled);
                        }
                        else
                        { goto case "lightness"; }
                        break;
                    case "exception":
                        goto case "crash";
                    case "crash":
                        if (string.IsNullOrWhiteSpace(value))
                        { throw new Exception("test exception"); }
                        else
                        { throw new Exception(value); }







                    case "debug":                                                                     //place new bools above
                        if (string.IsNullOrWhiteSpace(value))
                        {
                            if (debug > 0)
                            { Debug(0); }
                            else
                            { Debug(3); }
                        }
                        else
                        {
                            if (int.TryParse(value, out intResult))
                            { Debug(intResult); }
                            else
                            { WriteLine("error: rank \"" + value + "\" could not be parsed"); }
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
                                { developerConsole.Write("mow"); }

                                if (intResult > 25)
                                { WriteLine("It's probably pretty cluttered here now... why don't you -clearlog-?"); }
                            }
                            else
                            { WriteLine("Yep, yep, I'm a snep!"); }
                        }
                        else
                        { WriteLine("Yep, yep, I'm a snep!"); }
                        break;
                    case "credits":
                        RollCredits();
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
                        { WriteLine("error: no command with name: \"" + variable + "\""); }
                        else
                        { WriteLine("error: no command with name: \"" + variable + "[" + extra + "]\""); }
                        break;
                }
            }
        }

        public static Texture2D ResizeTexture(Texture2D TextureToResize, float Scale, bool PointClamp)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(game.GraphicsDevice, (int)(TextureToResize.Bounds.Size.X * Scale + 0.5f), (int)(TextureToResize.Bounds.Size.Y * Scale + 0.5f));
            game.GraphicsDevice.SetRenderTarget(renderTarget);
            game.GraphicsDevice.Clear(Color.Transparent);
            if (PointClamp)
            { spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity); }
            else
            { spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity); }
            spriteBatch.Draw(TextureToResize, renderTarget.Bounds, Color.White);
            spriteBatch.End();
            game.GraphicsDevice.SetRenderTarget(null);

            return (Texture2D)renderTarget;
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
            if (deathBackgroundOpacity < 1)
            {
                try
                {
                    for (int i = 0; i < grassList.Count; i++)
                    { grassList[i].PreDraw(); }

                    GraphicsDevice.SetRenderTarget(sceneRenderTarget);
                    GraphicsDevice.Clear(backgroundColor);
                    //spritebatch for BACKGROUNDS
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, background);
                    for (int i = 0; i < backgroundSquareList.Count; i++)
                    { backgroundSquareList[i].Draw(spriteBatch); }
                    spriteBatch.End();

                    //standard spritebatch, draw level and player and enemies here
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, foreground);
                    for (int i = 0; i < squareList.Count; i++)
                    { squareList[i].Draw(spriteBatch); }
                    for (int i = enemyList.Count - 1; i >= 0; i--)
                    { if (enemyList[i] != null) { enemyList[i].Draw(spriteBatch); } }

                    for (int i = particleSystems.Count - 1; i >= 0; i--)
                    { particleSystems[i].Draw(spriteBatch); }
                    for (int i = 0; i < grassList.Count; i++)
                    { grassList[i].Draw(spriteBatch); }
                    //torch.Draw(spriteBatch);
                    if (jamie != null) { jamie.Draw(spriteBatch); }
                    if (debug > 1)
                    {
                        RectangleBorder.Draw(spriteBatch, new Rectangle(Point.Zero, screenspace.Size), Color.Magenta, false);
                        //if (levelEditor)
                        //{
                        //    Texture2D squareTex = LoadTexture("originTexture");
                        //    foreach (Vector2 p in enemySpawnPoints)
                        //    {
                        //        Square tempSquare = new Square(squareTex, p.ToPoint(), screenScale, false, 0.9f);
                        //        tempSquare.Draw(spriteBatch);
                        //    }
                        //    Square tempSquare2 = new Square(squareTex, worldSpaceMouseLocation, screenScale, false, 0.9f);
                        //    tempSquare2.Draw(spriteBatch);
                        //}
                    }
                    if (onslaughtSpawner != null)
                    {
                        for (int i = 0; i < onslaughtSpawner.vendingMachineList.Count; i++)
                        { onslaughtSpawner.vendingMachineList[i].Draw(spriteBatch); }
                    }
                    spriteBatch.End();
                    if (lightingEnabled)
                    {
                        GraphicsDevice.SetRenderTarget(lightingRenderTarget);
                        GraphicsDevice.Clear(new Color(0f, 0f, 0f, darkness));
                        // spritebatch for LIGHTING
                        spriteBatch.Begin(blendState: multiplicativeBlend, transformMatrix: foreground);
                        for (int i = particleSystems.Count - 1; i >= 0; i--)
                        { particleSystems[i].Light(spriteBatch, false); }
                        //torch.Light(spriteBatch, false);
                        for (int i = enemyList.Count - 1; i >= 0; i--)
                        { enemyList[i].Light(spriteBatch, false); }
                        if (jamie != null) { jamie.Light(spriteBatch, false); }
                        spriteBatch.End();


                        GraphicsDevice.SetRenderTarget(coloredLightingRenderTarget);
                        GraphicsDevice.Clear(Color.Transparent);
                        //spritebatch for COLORED LIGHTING
                        spriteBatch.Begin(blendState: BlendState.Additive, transformMatrix: foreground);
                        for (int i = particleSystems.Count - 1; i >= 0; i--)
                        { particleSystems[i].Light(spriteBatch, true); }
                        //torch.Light(spriteBatch, true);
                        if (jamie != null) { jamie.Light(spriteBatch, true); }
                        spriteBatch.End();


                        // don't mess with this stuff
                        GraphicsDevice.SetRenderTarget(null);
                        GraphicsDevice.Clear(Color.Black);
                        spriteBatch.Begin(blendState: BlendState.AlphaBlend);
                        spriteBatch.Draw(sceneRenderTarget, zeroScreenspace, Color.White);
                        spriteBatch.Draw(lightingRenderTarget, zeroScreenspace, Color.White);
                        GraphicsDevice.BlendState = BlendState.Additive;
                        spriteBatch.Draw(coloredLightingRenderTarget, zeroScreenspace, Color.White);
                        GraphicsDevice.BlendState = BlendState.AlphaBlend;
                        spriteBatch.End();
                    }
                    else
                    {
                        GraphicsDevice.SetRenderTarget(null);
                        GraphicsDevice.Clear(Color.Transparent);
                        spriteBatch.Begin();
                        spriteBatch.Draw(sceneRenderTarget, zeroScreenspace, Color.White);
                        spriteBatch.End();
                    }
                }
                catch (Exception e)
                {
                    WriteLine("exception caught during draw:" + e.Message + "\nStackTrace:\n" + e.StackTrace);
                    DisplayInfoText("exception caught during draw:" + e.Message);
                }
            }

            if (debug > 4)
            {
                basicEffect.Projection = projection;
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
            if (displayUI)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                if (recordFPS)
                { spriteBatch.Draw(nullTex, new Rectangle(3, 4, 60, 14), null, Color.Black, 0f, Vector2.Zero, SpriteEffects.None, 1f); } // steam fps
                if (vendingMachineMenu != null)
                { vendingMachineMenu.Draw(spriteBatch); }
                else
                { if (bars != null) { bars.Draw(spriteBatch); } }
                if (onslaughtMode)
                { onslaughtDisplay.Draw(spriteBatch); }
                else
                { timerDisplay.Draw(spriteBatch); }
                if (!sceneIsMenu)
                {
                    debuginfo.Draw(spriteBatch);
                    //topright.Draw(spriteBatch);
                }

                //deathscreen blackout
                spriteBatch.Draw(nullTex, zeroScreenspace, null, Color.Black * (deathBackgroundOpacity), 0f, Vector2.Zero, SpriteEffects.None, 0.7f);

                if (loadingBar.Value < 100)
                { loadingBar.Draw(spriteBatch); }

                if (debug > 1)
                { RectangleBorder.Draw(spriteBatch, boundingBox, Color.Magenta, false); }
                spriteBatch.End();
            }
            // --------------------------------------------------------------------------------------------------------------------------------

            if (sceneIsMenu /*&& displayUI*/)        // FOR MENU DRAWING
            {
                if (!levelEditor && (rollCredits || intro > 0))
                {
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                    spriteBatch.Draw(menuTex[lastMenuScene], Vector2.Zero, null, Color.White, 0f, Vector2.Zero, (70f / menuTex[lastMenuScene].Height) * screenScale, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }
                
                // TODO: Add your drawing code here    --------------------------------------------------------------------------------------------
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);

                if (!rollCredits || intro > 0)
                {
                    if (!levelEditor)
                    {
                        if (levelLoaded > 0 || (creditsActive && intro <= 0))
                        { // everything in this if is displayed after a game has already been started
                          //darken bg screen
                            spriteBatch.Draw(cf, Vector2.Zero, zeroScreenspace, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);
                        }
                        else if (scene <= 0)
                        { // everything in this if is displayed ONLY on the main menu before a game has been started
                            if (logos != null)
                            {
                                if (intro > 0)
                                {
                                    float rotation = SmoothestStep(-10, 10, intro / 3.7f);
                                    if (intro > 2)
                                    {
                                        spriteBatch.Draw(liquidnitrogen, halfResolution.ToVector2(),
                                            null, Color.White, 0f, liquidnitrogen.Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.65f);
                                        spriteBatch.Draw(l, halfResolution.ToVector2() - new Vector2((liquidnitrogen.Width - l.Width) / 2f, 0), null, Color.White, 0f, l.Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                        spriteBatch.Draw(n, halfResolution.ToVector2() + new Vector2((liquidnitrogen.Width - n.Width) / 2f, 0), null, Color.White, 0f, n.Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                    }
                                    else if (intro > 1.85f)
                                    {
                                        float amount = Lerp(0, 1, (intro - 1.85f) / 0.15f);
                                        Rectangle saucerect = new Rectangle((int)((1 - amount) * (float)liquidnitrogen.Width / 2f - 4.5f), 0, (int)((float)liquidnitrogen.Width * amount + 10), liquidnitrogen.Height);
                                        spriteBatch.Draw(liquidnitrogen, halfResolution.ToVector2(), saucerect,
                                            Color.White, 0f, saucerect.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.65f);
                                        spriteBatch.Draw(l, halfResolution.ToVector2() - new Vector2(((liquidnitrogen.Width - l.Width) * amount) / 2f, 0), null, Color.White, 0f, l.Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                        spriteBatch.Draw(n, halfResolution.ToVector2() + new Vector2(((liquidnitrogen.Width - n.Width) * amount) / 2f, 0), null, Color.White, 0f, n.Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                        if (intro <= 1.85f)
                                        { l = n = liquidnitrogen = null; }
                                    }
                                    else if (intro > 1.23f)
                                    { // (2/5) * (1.85 - 0.3) = 0.62
                                        float amountY = SmoothestStep(1, -1, (intro - 1.23f) / 1.24f);
                                        float amountX = Lerp(1, 0, (intro - 1.23f) / 0.62f);
                                        //halfResolution.ToVector2() - new Vector2(halfResolution.X * amountX / 3f, (resolution.Y / 8f) * amountY)
                                        Vector2 pos = new Vector2(Lerp(halfResolution.X, resolution.X / 3.5f, amountX), Lerp(halfResolution.Y, resolution.Y * 3 / 10f, amountY));
                                        spriteBatch.Draw(logos[0], pos,
                                            null, Color.White, rotation * MathHelper.Pi, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                    }
                                    else if (intro > 0.3f)
                                    { // (3/5) * (1.85 - 0.3) = 0.93
                                        float amountY = SmoothestStep(2, 0, (intro - 0.3f) / 1.86f + 0.5f);
                                        float amountX = Lerp(1, -1, (intro - 0.3f) / 1.86f);
                                        Vector2 pos = new Vector2(Lerp(resolution.X / 3.5f, 37, amountX), Lerp(resolution.Y * 3 / 10f, resolution.Y - 37, amountY));
                                        spriteBatch.Draw(logos[0], pos,
                                            null, Color.White, rotation * MathHelper.Pi, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                    }
                                    else if (intro > 0.15f)
                                    {
                                        float amount = SmoothestStep(0.5f, -0.5f, (intro - 0.15f) / 0.3f);
                                        spriteBatch.Draw(logos[0], new Vector2(37, resolution.Y - 37) + new Vector2(-logos[0].Width / 3 * amount, logos[0].Height * amount),
                                            null, Color.White, rotation * MathHelper.Pi, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                    }
                                    else
                                    {
                                        float amount = SmoothestStep(0, 0.5f, (intro) / 0.15f);
                                        spriteBatch.Draw(logos[0], new Vector2(37, resolution.Y - 37) + new Vector2(-logos[0].Width / 3 * amount, logos[0].Height * amount),
                                            null, Color.White, rotation * MathHelper.Pi, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f);
                                    }
                                    if (intro <= 2 && loadingBar.Value < 100)
                                    { loadingBar.Draw(spriteBatch); }
                                }
                                else
                                { spriteBatch.Draw(logos[0], new Vector2(37, resolution.Y - 37), null, Color.White, 0f, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 0.7f); }
                                for (int i = 1; i < logos.Count /*add -1 to disable patreon logo*/; i++)
                                { spriteBatch.Draw(logos[i], new Vector2((i * 72) + 5, zeroScreenspace.Height - 69), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.45f); }
                                patreonPrint.Draw(spriteBatch);
                            }
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(nullTex, texturePanel, null, new Color(31, 29, 37, 205), 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
                        int texwidth = (int)((float)resolution.X * 0.1f);
                        for (int i = 0; i < availableTextures.Count; i += 2)
                        {
                            spriteBatch.Draw(availableTextures[i], new Rectangle(texturePanel.X, (int)(texwidth * (i / 2f) + (30 * textScale)), texwidth, texwidth), null, new Color(255, 255, 255, 205), 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
                            if (mouseState.LeftButton == ButtonState.Pressed && (new Rectangle(texturePanel.X, (int)(texwidth * (i / 2f) + (30 * textScale)), texwidth, texwidth)).Contains(mouseState.Position))
                            { selectedTexture = availableTextures[i]; }
                        }
                        for (int i = 1; i < availableTextures.Count; i += 2)
                        {
                            spriteBatch.Draw(availableTextures[i], new Rectangle(texturePanel.X + texwidth, (int)(texwidth * ((i - 1) / 2f) + (30 * textScale)), texwidth, texwidth), null, new Color(255, 255, 255, 205), 0f, Vector2.Zero, SpriteEffects.None, 0.95f);
                            if (mouseState.LeftButton == ButtonState.Pressed && (new Rectangle(texturePanel.X + texwidth, (int)(texwidth * ((i - 1) / 2f) + (30 * textScale)), texwidth, texwidth)).Contains(mouseState.Position))
                            { selectedTexture = availableTextures[i]; }
                        }
                    }

                    // debug stuff
                    debuginfo.Draw(spriteBatch);
                    topright.Draw(spriteBatch);
                    infoText.Draw(spriteBatch);

                    if (lastMenuScene == 3 && menuSelection >= 0 && menuSelection <= 3)
                    { RectangleBorder.Draw(spriteBatch, boundingBox, Color.Magenta, false); }

                    for (int i = 0; i < sliderList.Count; i++)
                    { sliderList[i].Draw(spriteBatch); }

                    for (int i = 0; i < buttonList.Count; i++)
                    { buttonList[i].Draw(spriteBatch); }

                    for (int i = printList.Count - 1; i >= 0; i--)
                    { printList[i].Draw(spriteBatch); }

                    for (int i = 0; i < sList.Count; i++)
                    { sList[i].Draw(spriteBatch); }
                }
                if (creditsActive)
                {
                    if (intro <= 0)
                    {
                        spriteBatch.Draw(nullTex, zeroScreenspace, null, Color.Black * creditsOpacity, 0f, Vector2.Zero, SpriteEffects.None, 0.005f);
                        creditsPrint.Draw(spriteBatch, creditsCurrentPos.ToPoint());
                    }
                    else
                    {
                        spriteBatch.Draw(cf, Vector2.Zero, zeroScreenspace, Color.White * intro, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.51f);
                        spriteBatch.Draw(nullTex, zeroScreenspace, null, Color.Black * (intro-1), 0f, Vector2.Zero, SpriteEffects.None, 0.52f);
                        //spriteBatch.Draw(logos[0], halfResolution.ToVector2(), null, Color.White * intro, 0f, logos[0].Bounds.Size.ToVector2() / 2, 1f, SpriteEffects.None, 1f);
                    }
                }
                spriteBatch.End();
                // --------------------------------------------------------------------------------------------------------------------------------

                if (!levelEditor && !rollCredits && intro <= 0)
                {
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                    spriteBatch.Draw(menuTex[lastMenuScene], Vector2.Zero, null, Color.White, 0f, Vector2.Zero, (70f / menuTex[lastMenuScene].Height) * screenScale, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                }
            }

            if (console || consoleMoveTimer > 0)
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, /*Effect*/ null, UIground);
                spriteBatch.Draw(consoleTex, consoleRect, null, consoleRectColor, 0f, Vector2.Zero, SpriteEffects.None, 0.999f);
                consoleWriteline.Draw(spriteBatch);
                developerConsole.Draw(spriteBatch);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
