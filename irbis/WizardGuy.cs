using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class WizardGuy : IEnemy
{
    public enum WizardActivity
    {
        Idle = 0,
        Teleport = 1,
        Nova = 2,
        Bolt = 3,
        Lazers = 4,
        Shockwave = 5,

        Dying = 19,
        Misc = 20,
    }

    public Vector2 TrueCenter
    {
        get
        { return position; }
        set
        { position = value; }
    }
    public Rectangle Collider
    {
        get
        {
            return collider;
        }
        set
        {
            collider = value;
        }
    }
    private Rectangle collider;
    public Rectangle TrueCollider
    {
        get
        {
            return trueCollider;
        }
    }
    private Rectangle trueCollider;

    public Vector2 Position
    {
        get
        { return position; }
        set
        { position = value; }
    }
    private Vector2 position;

    public Vector2 Velocity
    {
        get
        { return velocity; }
        set
        { velocity = value; }
    }
    private Vector2 velocity;

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    private float health;

    public float MaxHealth
    {
        get
        { return maxHealth; }
        set
        { maxHealth = value; }
    }
    private float maxHealth;

    public float SpeedModifier
    {
        get
        { return speedModifier; }
        set
        { speedModifier = value; }
    }
    private float speedModifier;

    public List<Enchant> ActiveEffects
    {
        get
        { return activeEffects; }
    }
    private List<Enchant> activeEffects;

    public string Name
    {
        get
        { return name; }
    }
    private string name = "Wizard Guy (bweam)";

    public bool AIenabled
    {
        get
        { return aiEnabled; }
        set
        { aiEnabled = value; }
    }
    public bool aiEnabled = true;

    public float Mass
    {
        get
        { return mass; }
    }
    private float mass = 0.5f;

    public float ShockwaveMaxEffectDistanceSquared
    {
        get
        { return shockwaveMaxEffectDistanceSquared; }
    }
    private float shockwaveMaxEffectDistanceSquared;

    public float StunTime
    {
        get
        {
            return stunned;
        }
    }

    public bool ActivelyAttacking
    {
        get
        {
            return state[1] > 0 || state[2] > 0 || state[3] > 0 || state[4] > 0;
        }
    }

    public string ActiveAttacks
    {
        get
        {
            string returnString = string.Empty;
            if (state[1] > 0)
            { returnString += " roll:" + state[1]; }
            if (state[2] > 0)
            { returnString += " swipe:" + state[2]; }
            if (state[3] > 0)
            { returnString += " tailwhip:" + state[3]; }
            if (state[4] > 0)
            { returnString += " bury:" + state[4]; }
            if (cooldown[0] > 0)
            { returnString += " wander:" + state[0]; }
            returnString.TrimStart();
            return returnString;
        }
    }

    Vector2 Origin;

    public float speed;
    public float defaultSpeed;
    public WizardActivity previousActivity;
    public Point prevInput;
    public int nextAnimation;
    public float idleTime;
    public Print animationFrame;

    Texture2D tex;
    Texture2D fireballtex;
    public Rectangle displayRect;
    public Rectangle animationSourceRect;
    public Rectangle standardCollider;

    public float terminalVelocity = 5000f;

    float depth;

    public float stunned;
    public float wanderTime;
    public float initialWanderTime;
    public float timeSinceLastFrame;
    int currentFrame;
    public int currentAnimation;
    int previousAnimation;
    public float[] animationSpeed = new float[10];
    public int[] animationFrames = new int[10];
    bool animationNoLoop;
    public Point input;
    public bool frameInput;

    public float shockwaveStunTime;
    public Vector2 shockwaveKnockback;

    public Direction direction;
    public Location location;
    public WizardActivity activity;

    public int combatCheckDistanceSqr;
    public int persueCheckDistanceSqr;
    public bool combat;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 50f;

    public Rectangle attackCollider;
    public int attackColliderWidth = 100;
    public int attackColliderHeight = 70;

    public float attackDamage;
    public float attack1Damage;

    public float attackCooldown;
    public float attackCooldownTimer;

    Vector2[] teleportPoints;
    int previousTeleport;

    public BossState bossState = 0;
    public int combatPhase = 1;
    Vector2 spawnPoint;
    Vector2 initialPoint;
    float stepTime;
    float stepTimeMax = 5f;

    int lazerindex = 0;
    List<Lazor> lazers = new List<Lazor>();
    Texture2D[] lazertextures = new Texture2D[5];

    // distance squared at which a cooldown ticks more quickly 
    float lazersFastTick;
    float novaFastTick = 62500;
    float boltsFastTick = 10000;
    float teleportFastTick = 2500;

    Rectangle bossArena;
    List<Nova> firenovas = new List<Nova>();
    List<float> firenovaTTL = new List<float>();
    List<Fireball> fireballs = new List<Fireball>();
    List<float> fireballTTL = new List<float>();
    List<ChargedBolts> bolts = new List<ChargedBolts>();
    List<float> boltsTTL = new List<float>();

    int fireballCastingCount;
    int boltsCastingCount;

    Vector2 explosionLocation;
    float explosionDamage = 50;
    float explosionRadius = 64;
    float explosionRadiusSQR = 64 * 64;
    bool exploding;
    float timeSinceLastExplosionFrame;
    float explosionAnimationSpeed = 0.05f;
    int currentExplosionFrame;
    float explosionVelocity = 300;
    Rectangle explosionSourceRect = new Rectangle(Point.Zero, new Point(Irbis.Irbis.explosiontex.Height));

    float playerDistanceSQR;

    public int[] state = new int[5];
    float[] cooldown = new float[9];
    public float[] timer = new float[9];

    public WizardGuy(Texture2D t, int? iPos, float enemyHealth, float enemyDamage, Vector2[] TeleportPoints, Rectangle? BossArena, float drawDepth)
    {
        timer[0] = cooldown[0] = 01f; // idle
        timer[1] = cooldown[1] = 17f; // teleport
        timer[2] = cooldown[2] = 05f; // nova
        timer[3] = cooldown[3] = 08f; // bolt
        timer[4] = cooldown[4] = 08f; // lazers
      /*timer[5]*/ cooldown[5] = .3f; // lazer delay 
      /*timer[6]*/ cooldown[6] = .2f; // fireball delay 
      /*timer[7]*/ cooldown[7] = 01f; // nova delay 
        timer[8] = cooldown[8] = 15f; // heal (phase 3 only)



        Fireball.fireballtex = Irbis.Irbis.LoadTexture("fireball");

        idleTime = 0f;

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.001f);

        tex = t;
        AIenabled = true;

        depth = drawDepth;

        direction = Direction.Forward;
        location = Location.Air;
        wanderTime = 1f;
        initialWanderTime = 0f;

        animationNoLoop = false;
        collider.Size = trueCollider.Size = standardCollider.Size = new Point(32, 56);
        standardCollider.Location = new Point((int)-(standardCollider.Width / 2f), (int)-(standardCollider.Height / 2f));
        Origin = new Vector2(47, 37) - standardCollider.Location.ToVector2();

        maxHealth = health = enemyHealth;

        stunned = 0;
        speedModifier = 1f;

        attackCollider = Rectangle.Empty;

        attackDamage = 0f;
        attack1Damage = enemyDamage;

        attackCooldown = 2f;        //how quickly enemies can attack
        attackCooldownTimer = 3f;

        combatCheckDistanceSqr = attackColliderWidth * attackColliderWidth;
        persueCheckDistanceSqr = 40000;
        combat = false;

        activeEffects = new List<Enchant>();

        lazertextures[0] = Irbis.Irbis.LoadTexture("lazor0");
        lazertextures[1] = Irbis.Irbis.LoadTexture("lazor45");
        lazertextures[2] = Irbis.Irbis.LoadTexture("lazor90");
        lazertextures[3] = Irbis.Irbis.LoadTexture("lazor135");
        lazertextures[4] = Irbis.Irbis.LoadTexture("lazor180");

        shockwaveStunTime = Irbis.Irbis.jamie.shockwaveStunTime;
        shockwaveKnockback = Irbis.Irbis.jamie.shockwaveKnockback;

        displayRect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;
        animationSpeed[0] = 0.1f;
        for (int i = 1; i < animationSpeed.Length; i++)
        { animationSpeed[i] = animationSpeed[0]; }
         animationSpeed[4] = 0.0375f;
        animationFrames[0] = 0; // idle
        animationFrames[1] = 0; // charging
        animationFrames[2] = 0; // casting
        animationFrames[3] = 0; // hurt
        animationFrames[4] = 4; // teleport


        animationSourceRect = new Rectangle(128 * currentFrame, 128 * currentAnimation, 128, 128);

        if (BossArena != null)
        { bossArena = (Rectangle)BossArena; }
        else
        {
            int topmostBottom = Irbis.Irbis.squareList[0].Collider.Bottom;
            int leftmostRight = Irbis.Irbis.squareList[0].Collider.Right;
            int rightmostLeft = Irbis.Irbis.squareList[0].Collider.Left;
            int bottommosttop = Irbis.Irbis.squareList[0].Collider.Top;

            foreach (Square s in Irbis.Irbis.squareList)
            {
                if (s.Collider.Bottom < topmostBottom)
                { topmostBottom = s.Collider.Bottom; }
                if (s.Collider.Right < leftmostRight)
                { leftmostRight = s.Collider.Right; }
                if (s.Collider.Left > rightmostLeft)
                { rightmostLeft = s.Collider.Left; }
                if (s.Collider.Top > bottommosttop)
                { bottommosttop = s.Collider.Top; }
            }
            bossArena = new Rectangle(leftmostRight, topmostBottom, (rightmostLeft - leftmostRight), (bottommosttop - topmostBottom));
        }

        if (TeleportPoints == null || TeleportPoints.Length <= 1)
        {
            teleportPoints = new Vector2[5];
            teleportPoints[0] = new Vector2(bossArena.X + bossArena.Width * 0.1f, bossArena.Center.Y);
            teleportPoints[1] = new Vector2(bossArena.X + bossArena.Width * 0.3f, bossArena.Center.Y);
            teleportPoints[2] = new Vector2(bossArena.X + bossArena.Width * 0.5f, bossArena.Center.Y);
            teleportPoints[3] = new Vector2(bossArena.X + bossArena.Width * 0.7f, bossArena.Center.Y);
            teleportPoints[4] = new Vector2(bossArena.X + bossArena.Width * 0.9f, bossArena.Center.Y);
        }
        else
        { teleportPoints = TeleportPoints; }

        if (iPos == null)
        {
            if (TeleportPoints != null && TeleportPoints.Length == 1)
            {
                previousTeleport = -1;
                initialPoint = TeleportPoints[0];
                TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
            }
            else
            {
                previousTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length);
                initialPoint = teleportPoints[previousTeleport];
                TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
            }
        }
        else
        {
            previousTeleport = (int)iPos;
            initialPoint = teleportPoints[previousTeleport];
            TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
        }

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave += Enemy_OnPlayerShockwave;
    }

    public bool Update()
    {
        prevInput = input;
        input = Point.Zero;
        frameInput = false;

        if (Irbis.Irbis.GetMouseState.LeftButton == ButtonState.Pressed && collider.Contains(Irbis.Irbis.WorldSpaceMouseLocation))
        { position = new Vector2(Irbis.Irbis.WorldSpaceMouseLocation.X - standardCollider.X - (collider.Width/2), Irbis.Irbis.WorldSpaceMouseLocation.Y - standardCollider.Y - (collider.Height/2)); }


        switch (bossState)
        {
            case BossState.Spawn:       // 0
                stepTime += Irbis.Irbis.DeltaTime;
                TrueCenter = Irbis.Irbis.SmootherStep(spawnPoint, initialPoint, (stepTime / stepTimeMax));
                if (stepTime >= stepTimeMax)
                { bossState++; SetAnimation(4, true); }
                break;
            case BossState.Entrance:    // 1
                bossState++;
                break;
            case BossState.Engage:      // 2
                bossState++;
                break;
            case BossState.Combat:      // 3
                if (Irbis.Irbis.jamie != null)
                {
                    playerDistanceSQR = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider, TrueCenter);

                    if (aiEnabled && timer[0] <= 0 && stunned <= 0)
                    {
                        switch (combatPhase)
                        {
                            case 1: // phase 1
                                if (timer[4] <= 0)
                                {
                                    if (timer[5] <= 0)
                                    {
                                        switch (lazerindex)
                                        {
                                            case 0:
                                                lazers.Clear();
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y - 15), new Point(30)), new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 1:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 2:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Center.X - 15, collider.Center.Y + 15), new Point(30)), new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 3:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 4:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y - 15), new Point(30)), new Vector2(-2000, 0), new Texture2D[] { lazertextures[4], Irbis.Irbis.dottex }, 20));
                                                timer[4] = cooldown[4];
                                                timer[0] = cooldown[0];
                                                lazerindex = 0;
                                                break;
                                        }
                                    }
                                }
                                else if (timer[1] <= 0)
                                {
                                    SetAnimation(4, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (timer[2] <= 0)
                                {
                                    if (timer[6] <= 0)
                                    {
                                        Vector2 toJamie = Vector2.Normalize(Irbis.Irbis.jamie.TrueCenter - TrueCenter);
                                        fireballs.Add(new Fireball(new Rectangle(new Point(collider.Center.X - 5, collider.Center.Y - 5), new Point(10)),
                                            (toJamie * 10f + new Vector2((Irbis.Irbis.RandomFloat - 1),(Irbis.Irbis.RandomFloat - 1))) * 20f, 20));
                                        fireballTTL.Add(30f);
                                        fireballCastingCount++;

                                        if (fireballCastingCount >= 3)
                                        {
                                            timer[2] = cooldown[2];
                                            timer[0] = cooldown[0];
                                            fireballCastingCount = 0;
                                        }
                                        else
                                        { timer[6] = cooldown[6]; }
                                    }
                                }
                                /* else if (timer[3] <= 0)
                                {
                                    bolts = new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 2, 100, 1, 30);
                                    timer[3] = cooldown[3];
                                    timer[0] = cooldown[0];
                                }/**/

                                if (teleportFastTick > playerDistanceSQR)
                                { timer[1] -= Irbis.Irbis.DeltaTime * 3; }
                                else if (boltsFastTick > playerDistanceSQR)
                                { timer[3] -= Irbis.Irbis.DeltaTime * 3; }
                                /*else if (novaFastTick > playerDistanceSQR)
                                { timer[2] -= Irbis.Irbis.DeltaTime * 3; }
                                else if (lazersFastTick > playerDistanceSQR)
                                { timer[4] -= Irbis.Irbis.DeltaTime * 3; }/**/


                                if (health <= maxHealth * (4f / 5f))
                                {
                                    combatPhase = 2;
                                    timer[2] = cooldown[2] = 08f; // nova
                                    timer[4] = cooldown[4] = 12f; // lazers
                                }
                                break;
                            case 2: // phase 2
                                if (timer[4] <= 0)
                                {
                                    if (timer[5] <= 0)
                                    {
                                        switch (lazerindex)
                                        {
                                            case 0:
                                                lazers.Clear();
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y - 15), new Point(30)), new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 1:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 2:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Center.X - 15, collider.Center.Y + 15), new Point(30)), new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 3:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 20));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 4:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y - 15), new Point(30)), new Vector2(-2000, 0), new Texture2D[] { lazertextures[4], Irbis.Irbis.dottex }, 20));
                                                timer[4] = cooldown[4];
                                                timer[0] = cooldown[0];
                                                lazerindex = 0;
                                                break;
                                        }
                                    }
                                }
                                else if (timer[1] <= 0)
                                {
                                    SetAnimation(4, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (timer[3] <= 0)
                                {
                                    bolts.Add(new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 2, 100, 1, 30));
                                    boltsTTL.Add(30f);
                                    timer[3] = cooldown[3];
                                    timer[0] = cooldown[0];
                                }
                                else if (timer[2] <= 0)
                                {
                                    firenovas.Add(new Nova(new Rectangle(new Point(collider.Center.X - 5, collider.Center.Y - 5), new Point(10)), 12, 0, 100, 20));
                                    firenovaTTL.Add(30f);
                                    timer[2] = cooldown[2];
                                    timer[0] = cooldown[0];
                                }

                                if (teleportFastTick > playerDistanceSQR)
                                { timer[1] -= Irbis.Irbis.DeltaTime * 3; }
                                else if (boltsFastTick > playerDistanceSQR)
                                { timer[3] -= Irbis.Irbis.DeltaTime * 3; }
                                if (novaFastTick > playerDistanceSQR)
                                { timer[2] -= Irbis.Irbis.DeltaTime * 3; }
                                if (lazersFastTick > playerDistanceSQR)
                                { timer[4] -= Irbis.Irbis.DeltaTime; }

                                if (health <= maxHealth * (3f/ 5f))
                                {
                                    timer[1] = cooldown[1] = .25f; // idle
                                    timer[1] = cooldown[1] = 10f; // teleport
                                    timer[2] = cooldown[2] = 07f; // nova
                                    timer[3] = cooldown[3] = 06f; // bolt
                                    timer[4] = cooldown[4] = 08f; // lazers
                                  /*timer[5]*/ cooldown[5] = .2f; // lazer delay 
                                  /*timer[6]*/ cooldown[6] = .5f; // fireball delay 
                                    
                                    combatPhase = 3;
                                }
                                break;
                            case 3: // phase 3
                                if (timer[4] <= 0)
                                {
                                    if (timer[5] <= 0)
                                    {
                                        switch (lazerindex)
                                        {
                                            case 0:
                                                lazers.Clear();
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y - 15), new Point(30)), new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 1:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 2:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Center.X - 15, collider.Center.Y + 15), new Point(30)), new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 3:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 4:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y - 15), new Point(30)), new Vector2(-2000, 0), new Texture2D[] { lazertextures[4], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 5:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 6:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Center.X - 15, collider.Center.Y + 15), new Point(30)), new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 7:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 50));
                                                timer[5] = cooldown[5];
                                                lazerindex++;
                                                break;
                                            case 8:
                                                lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y - 15), new Point(30)), new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 50));
                                                timer[4] = cooldown[4];
                                                timer[0] = cooldown[0];
                                                lazerindex = 0;
                                                break;
                                        }
                                    }
                                }
                                else if (timer[1] <= 0)
                                {
                                    SetAnimation(4, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (timer[3] <= 0)
                                {
                                    if (timer[7] <= 0)
                                    {
                                        if (boltsCastingCount >= 1)
                                        {
                                            bolts.Add(new ChargedBolts(TrueCenter, 5, (MathHelper.Pi / 10), 1, 100, 1, 40));
                                            boltsTTL.Add(30f);
                                            boltsCastingCount = 0;
                                            timer[3] = cooldown[3];
                                            timer[0] = cooldown[0];
                                        }
                                        else
                                        {
                                            bolts.Add(new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 1, 100, 1, 40));
                                            boltsTTL.Add(30f);
                                            boltsCastingCount++;
                                            timer[7] = cooldown[7];
                                        }
                                    }
                                }
                                else if (timer[2] <= 0)
                                {
                                    if (timer[6] <= 0)
                                    {

                                        if (fireballCastingCount >= 1)
                                        {
                                            firenovas.Add(new Nova(new Rectangle(new Point(collider.Center.X - 5, collider.Center.Y - 5), new Point(10)), 12, MathHelper.Pi / 12, 200, 40));
                                            firenovaTTL.Add(30f);
                                            timer[2] = cooldown[2];
                                            timer[0] = cooldown[0];
                                            fireballCastingCount = 0;
                                        }
                                        else
                                        {
                                            firenovas.Add(new Nova(new Rectangle(new Point(collider.Center.X - 5, collider.Center.Y - 5), new Point(10)), 12, 0, 200, 40));
                                            firenovaTTL.Add(30f);
                                            timer[6] = cooldown[6];
                                            fireballCastingCount++;
                                        }
                                    }
                                }
                                else if (timer[8] <= 0)
                                {
                                    Heal(100);
                                    timer[8] = cooldown[8];
                                    timer[0] = cooldown[0];
                                }

                                if (health > maxHealth * (3f / 5f))
                                { combatPhase = 2; }

                                if (teleportFastTick > playerDistanceSQR)
                                { timer[1] -= Irbis.Irbis.DeltaTime; }
                                else if (boltsFastTick > playerDistanceSQR)
                                { timer[3] -= Irbis.Irbis.DeltaTime; }
                                if (novaFastTick > playerDistanceSQR)
                                { timer[2] -= Irbis.Irbis.DeltaTime; }
                                if (lazersFastTick > playerDistanceSQR)
                                { timer[4] -= Irbis.Irbis.DeltaTime; }

                                if (timer[8] > 0) // heal
                                { timer[8] -= Irbis.Irbis.DeltaTime; }
                                break;
                        }
                        if (timer[1] > 0) // teleport
                        { timer[1] -= Irbis.Irbis.DeltaTime; }
                        if (timer[2] > 0) // nova
                        { timer[2] -= Irbis.Irbis.DeltaTime; }
                        else if (timer[6] > 0) // fireball delay
                        { timer[6] -= Irbis.Irbis.DeltaTime; }
                        if (timer[3] > 0) // bolts
                        { timer[3] -= Irbis.Irbis.DeltaTime; }
                        else if (timer[7] > 0) // bolts delay
                        { timer[7] -= Irbis.Irbis.DeltaTime; }
                        if (timer[4] > 0) // lazers
                        { timer[4] -= Irbis.Irbis.DeltaTime; }
                        else if (timer[5] > 0) // lazer delay
                        { timer[5] -= Irbis.Irbis.DeltaTime; }
                    }
                    else if (timer[0] > 0)
                    { timer[0] -= Irbis.Irbis.DeltaTime; }
                }
                break;
            case BossState.Disengage:   // 4

                break;
            case BossState.Death:       // 5

                break;
        }

        for (int i = firenovas.Count - 1; i >= 0; i--)
        {
            firenovaTTL[i] -= Irbis.Irbis.DeltaTime;
            if (firenovaTTL[i] > 0)
            { firenovas[i].Update(); }
            else
            {
                firenovas[i].Death();
                firenovas.RemoveAt(i);
                firenovaTTL.RemoveAt(i);
            }
        }

        for (int i = fireballs.Count - 1; i >= 0; i--)
        {
            fireballTTL[i] -= Irbis.Irbis.DeltaTime;
            if (fireballTTL[i] > 0)
            { fireballs[i].Update(); }
            else
            {
                fireballs[i].Death();
                fireballs.RemoveAt(i);
                fireballTTL.RemoveAt(i);
            }
        }

        for (int i = bolts.Count - 1; i >= 0; i--)
        {
            boltsTTL[i] -= Irbis.Irbis.DeltaTime;
            if (boltsTTL[i] > 0)
            { bolts[i].Update(); }
            else
            {
                bolts.RemoveAt(i);
                boltsTTL.RemoveAt(i);
            }
        }
        
        for (int i = lazers.Count - 1; i >= 0; i--)
        {
            if (lazers[i].Update())
            { lazers.RemoveAt(i); }
        }


        //Movement();
        CalculateMovement();
        Animate();
        return true;
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        {
            Update();
            if (health <= 0 || position.Y > 5000f)
            { Irbis.Irbis.KillEnemy(this); }
        }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            { Irbis.Irbis.doneEvent.Set(); }
        }
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack, Vector2 Damage)
    {
        if (AttackCollider.Intersects(collider))
        {
            float tempDamage = Irbis.Irbis.RandomFloatInRange(Damage);
            PlayerAttackCollision(tempDamage);
            Irbis.Irbis.WriteLine(name + " hit. health remaining:" + health + " (" + tempDamage + ")");
        }
        return true;
    }

    public bool Enemy_OnPlayerShockwave(Vector2 Origin, float Range, Attacking Attack, float Power)
    {
        Irbis.Irbis.WriteLine(name + " Enemy_OnPlayerShockwave triggered");
        return true;
    }

    public void Death()
    {
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave -= Enemy_OnPlayerShockwave;
        for (int i = firenovas.Count - 1; i >= 0; i--)
        { firenovas[i].Death(); }
        //Irbis.Irbis.jamie.OnPlayerShockwave -= firenova.Enemy_OnPlayerShockwave;
    }

    public void PlayerAttackCollision(float Damage)
    {
        Hurt(Damage);

        foreach (Enchant enchant in Irbis.Irbis.jamie.enchantList)
        { enchant.AddEffect(this); }
    }

    public bool OnTouch(Rectangle TouchedCollider, Vector2 Knockback)
    {
        Direction playerDirection = Irbis.Irbis.Directions(TouchedCollider, Irbis.Irbis.jamie.Collider);
        if (playerDirection == Direction.Left)
        {
            Irbis.Irbis.jamie.velocity = new Vector2(-Knockback.X, Knockback.Y);
            Irbis.Irbis.jamie.direction = Direction.Right;
        }
        else if (playerDirection == Direction.Right)
        {
            Irbis.Irbis.jamie.velocity = Knockback;
            Irbis.Irbis.jamie.direction = Direction.Left;
        }
        else
        { Irbis.Irbis.jamie.velocity = new Vector2(0, Knockback.Y); }
        return true;
    }

    public void CalculateMovement()
    {
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        //collider = new Rectangle((int)position.X + XcolliderOffset, (int)position.Y + YcolliderOffset, colliderSize.X, colliderSize.Y);
        trueCollider = standardCollider;
        trueCollider.X += (int)Math.Round((double)position.X);
        trueCollider.Y += (int)Math.Round((double)position.Y);
        //trueCollider.Size = colliderSize;
        if (collider != Rectangle.Empty)
        { collider = trueCollider; }
    }

    public void Animate()
    {
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= animationSpeed[currentAnimation])
        {
            currentFrame++;
            timeSinceLastFrame -= animationSpeed[currentAnimation];
        }

        if (previousActivity != activity)
        { SetAnimation(); }
        else if (currentFrame > animationFrames[currentAnimation])
        {
            if (animationNoLoop)
            {
                switch (currentAnimation)
                {
                    case 4:
                        int nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length);
                        while (previousTeleport == nextTeleport)
                        { nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length); }
                        previousTeleport = nextTeleport;
                        Explode();
                        TrueCenter = teleportPoints[previousTeleport];
                        SetAnimation(0, false);
                        break;
                }
            }
            else
            { currentFrame = 0; }
        }

        if (previousAnimation != currentAnimation)
        {
            timeSinceLastFrame = 0;
            currentFrame = 0;
        }

        animationSourceRect.X = 128 * currentFrame;
        animationSourceRect.Y = 128 * currentAnimation;


        if (exploding)
        {
            timeSinceLastExplosionFrame += Irbis.Irbis.DeltaTime;
            if (timeSinceLastExplosionFrame >= explosionAnimationSpeed)
            {
                timeSinceLastExplosionFrame -= explosionAnimationSpeed;
                currentExplosionFrame++;
            }
            if (currentExplosionFrame * Irbis.Irbis.explosiontex.Height >= Irbis.Irbis.explosiontex.Width)
            {
                currentExplosionFrame = 0;
                timeSinceLastExplosionFrame = 0;
                exploding = false;
            }
            explosionSourceRect.X = currentExplosionFrame * Irbis.Irbis.explosiontex.Height;
        }


        previousAnimation = currentAnimation;
        previousActivity = activity;
    }

    public void SetAnimation()
    {
        switch (activity)
        {
            case WizardActivity.Idle:
                SetAnimation(0, false);
                break;

            default:
                SetAnimation(0, false);
                break;
        }

        if (nextAnimation >= 0)
        { SetAnimation(nextAnimation, false); }

        Irbis.Irbis.WriteLine(name + " animation set. animation:" + currentAnimation + " direction:" + direction);
    }

    public void SetAnimation(int animation, bool noLoop)
    {
        currentAnimation = animation;
        currentFrame = 0;
        timeSinceLastFrame = 0;
        nextAnimation = -1;
        animationNoLoop = noLoop;
        if (direction == Direction.Right)
        { currentAnimation++; }
    }

    protected void Explode()
    {
        exploding = true;
        explosionLocation = TrueCenter;
        if (playerDistanceSQR <= explosionRadiusSQR)
        {
            float Distance = (float)Math.Sqrt(playerDistanceSQR);
            Irbis.Irbis.jamie.Hurt(((explosionRadius - Distance) / explosionRadius) * explosionDamage, true, Irbis.Irbis.Directions(TrueCenter, Irbis.Irbis.jamie.TrueCenter));
            Irbis.Irbis.jamie.velocity += Vector2.Normalize(Irbis.Irbis.jamie.TrueCenter - explosionLocation) * explosionVelocity;
        }
    }

    public void Dying()
    {
        activity = WizardActivity.Dying;
        previousActivity = activity;
        trueCollider.Size = collider.Size = new Point(60, 32);
        standardCollider.Location = new Point(44, 93);
        SetAnimation(14, true);
    }

    public void AddEffect(Enchant effect)
    {
        activeEffects.Add(effect);
    }

    public void UpgradeEffect(int index, float duration)
    {
        if (activeEffects.Count > index)
        {
            activeEffects[index].strength++;
            activeEffects[index].effectDuration = duration;
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        Irbis.Irbis.CameraShake(0.075f, 0.05f * damage);
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        { health = maxHealth; }
    }

    public void Stun(float duration)
    {
        if (stunned < duration)
        { stunned = duration; }
    }

    public override string ToString()
    {
        return name;
    }

    public void Draw(SpriteBatch sb)
    {
        switch (Irbis.Irbis.debug)
        {
            case 5:
                goto case 4;
            case 4:
                goto case 3;
            case 3:
                RectangleBorder.Draw(sb, bossArena, Color.Red, true);
                if (attackCollider != Rectangle.Empty)
                { RectangleBorder.Draw(sb, attackCollider, Color.Red, true); }
                goto case 2;
            case 2:
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, ((position + (standardCollider.Location - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto case 1;
            case 1:
                goto default;
            default:
                for (int i = firenovas.Count - 1; i >= 0; i--)
                { firenovas[i].Draw(sb); }
                for (int i = fireballs.Count - 1; i >= 0; i--)
                { fireballs[i].Draw(sb); }
                for (int i = bolts.Count - 1; i >= 0; i--)
                { bolts[i].Draw(sb); }
                foreach (Lazor l in lazers)
                { l.Draw(sb); }
                if (exploding)
                { sb.Draw(Irbis.Irbis.explosiontex, explosionLocation * Irbis.Irbis.screenScale, explosionSourceRect, Color.SteelBlue, 0f, new Vector2(64f), Irbis.Irbis.screenScale, SpriteEffects.None, depth); }
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Origin, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        for (int i = firenovas.Count - 1; i >= 0; i--)
        { firenovas[i].Light(sb, UseColor); }
        for (int i = fireballs.Count - 1; i >= 0; i--)
        { fireballs[i].Light(sb, UseColor); }
        for (int i = bolts.Count - 1; i >= 0; i--)
        { bolts[i].Light(sb, UseColor); }
        foreach (Lazor l in lazers)
        { l.Light(sb, UseColor); }
    }
}
