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

    public enum Animation
    {
        Idle = 0,
        Charging = 1,
        Teleport = 2,
        Cast = 3,
        Dying,
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
        { return (aiEnabled && bossState > (BossState)0); }
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

    public int casting;
    Vector2 leftHand, rightHand;

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

    Vector2 origin;

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

    float castingTime;

    int lazerindex = 0;
    List<Lazor> lazers = new List<Lazor>();
    Texture2D[] lazertextures = new Texture2D[5];

    // distance squared at which a cooldown ticks more quickly 
    float lazersFastTick;
    float novaFastTick = 62500;
    float boltsFastTick = 10000;
    float teleportFastTick = 2500;

    public Rectangle bossArena;
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
    float[] castTime = new float[9];
    public float[] timer = new float[9];
    public float[] prevTimer = new float[9];
    public Vector2[][] castingHands = new Vector2[5][];
    public Vector2[] previousHands = new Vector2[2];
    public List<SpellEffect> hands = new List<SpellEffect>();
    Texture2D[] LazorHands;

    public WizardGuy(Texture2D t, int? iPos, float enemyHealth, float enemyDamage, Vector2[] TeleportPoints, Rectangle? BossArena, float drawDepth)
    {
        //                                          left hand          right hand
        castingHands[0] = new Vector2[] { new Vector2(-34, -2), new Vector2(29,  7) };
        castingHands[1] = new Vector2[] { new Vector2(-35,  5), new Vector2(35, -4) };
        castingHands[2] = new Vector2[] { new Vector2(-35, 11), new Vector2(33, -1) };
        castingHands[3] = new Vector2[] { new Vector2(-33,  1), new Vector2(29,  9) };
        castingHands[4] = new Vector2[] { new Vector2(-34, -5), new Vector2(27, 11) };


        castTime[0] = 1; // idle (so unused, I guess?)
        castTime[1] = 1; // teleport
        castTime[2] = 1; // nova (fireballs)
        castTime[3] = 1; // bolt
        castTime[4] = float.MaxValue; // lazers
        castTime[5] = 1; // lazer delay 
        castTime[6] = 1; // fireball delay 
        castTime[7] = 1; // bolt delay 
        castTime[8] = 1; // heal (phase 3 only)

        timer[0] = cooldown[0] = 01f; // idle
        timer[1] = cooldown[1] = 17f; // teleport
        timer[2] = cooldown[2] = 05f; // nova (fireballs)
        timer[3] = cooldown[3] = 08f; // bolt
        timer[4] = cooldown[4] = 08f; // lazers
      /*timer[5]*/ cooldown[5] = .3f; // lazer delay 
      /*timer[6]*/ cooldown[6] = .2f; // fireball delay 
      /*timer[7]*/ cooldown[7] = 01f; // bolt delay 
        timer[8] = cooldown[8] = 15f; // heal (phase 3 only)

        timer[5] = castTime[4];
        timer[6] = castTime[2];
        timer[7] = castTime[3];

        Fireball.fireballtex = Irbis.Irbis.LoadTexture("fireball");
        LazorHands = new Texture2D[] { Irbis.Irbis.LoadTexture("angular1"), Irbis.Irbis.LoadTexture("angular2"), Irbis.Irbis.LoadTexture("angular3") };


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
        collider.Size = standardCollider.Size = new Point(32, 56);
        standardCollider.Location = new Point(-16, -28);
        origin = new Vector2(47, 53) - standardCollider.Location.ToVector2();

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
        animationSpeed[2] = 0.0375f;
        animationFrames[0] = 6; // idle
        animationFrames[1] = 4; // charging
        animationFrames[2] = 6; // teleport
        animationFrames[3] = 3; // casting
        animationFrames[4] = 0; // hurt
        animationFrames[5] = 0;
        animationFrames[6] = 0;
        animationFrames[7] = 0;
        animationFrames[8] = 0;
        animationFrames[9] = 0;


        animationSourceRect = new Rectangle(128 * currentFrame, 128 * currentAnimation, 128, 128);

        if (BossArena != null)
        { bossArena = (Rectangle)BossArena; }
        else
        {
            int topmostBottom = Irbis.Irbis.squareList[0].Collider.Bottom;
            int leftmostRight = Irbis.Irbis.squareList[0].Collider.Right;
            int rightmostLeft = Irbis.Irbis.squareList[0].Collider.Left;
            int bottommosttop = Irbis.Irbis.squareList[0].Collider.Top;

            Irbis.Irbis.WriteLine("squareList.Count:" + Irbis.Irbis.squareList.Count);
            foreach (Square s in Irbis.Irbis.squareList)
            {
                if (s.Collider != Rectangle.Empty)
                {
                    Irbis.Irbis.WriteLine("s.Collider:" + s.Collider);
                    if (s.Collider.Bottom < topmostBottom)
                    { topmostBottom = s.Collider.Bottom; }
                    if (s.Collider.Right < leftmostRight)
                    { leftmostRight = s.Collider.Right; }
                    if (s.Collider.Left > rightmostLeft)
                    { rightmostLeft = s.Collider.Left; }
                    if (s.Collider.Top > bottommosttop)
                    { bottommosttop = s.Collider.Top; }
                }
            }
            bossArena = new Rectangle(leftmostRight, topmostBottom, (rightmostLeft - leftmostRight), (bottommosttop - topmostBottom));
            Irbis.Irbis.WriteLine("bossArena:" + bossArena);
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
        prevTimer = timer;
        input = Point.Zero;
        frameInput = false;

        if (Irbis.Irbis.GetMouseState.LeftButton == ButtonState.Pressed && collider.Contains(Irbis.Irbis.WorldSpaceMouseLocation))
        { position = new Vector2(Irbis.Irbis.WorldSpaceMouseLocation.X - standardCollider.X - (collider.Width/2), Irbis.Irbis.WorldSpaceMouseLocation.Y - standardCollider.Y - (collider.Height/2)); }

        if (castingTime > 0)
        { castingTime -= Irbis.Irbis.DeltaTime; }

        switch (bossState)
        {
            case BossState.Spawn:       // 0
                stepTime += Irbis.Irbis.DeltaTime;
                TrueCenter = Irbis.Irbis.SmootherStep(spawnPoint, initialPoint, (stepTime / stepTimeMax));
                if (stepTime >= stepTimeMax)
                { bossState++; castingTime = 5; }
                break;
            case BossState.Entrance:    // 1
                if (Irbis.Irbis.currentLevel == "c1b2") // just makes sure that there are actually platforms to move
                {
                    Irbis.Irbis.CameraShake(1, 7);
                    float newYpos = Irbis.Irbis.SmoothStep(Irbis.Irbis.squareList[1].InitialPosition.Y - 150,
                        Irbis.Irbis.squareList[1].InitialPosition.Y, castingTime / 5);
                    health = Irbis.Irbis.SmoothStep(maxHealth, 0, castingTime / 5);
                    float newrotation = Irbis.Irbis.SmoothStep(MathHelper.PiOver2, 0, castingTime / 5);

                    Irbis.Irbis.squareList[1].Position = new Vector2(Irbis.Irbis.squareList[1].InitialPosition.X, newYpos);
                    Irbis.Irbis.squareList[2].Position = new Vector2(Irbis.Irbis.squareList[2].InitialPosition.X, newYpos);
                    Irbis.Irbis.squareList[3].Position = new Vector2(Irbis.Irbis.squareList[3].InitialPosition.X, newYpos);

                    if (Irbis.Irbis.debug > 4)
                    {
                        Irbis.Irbis.shadowShapes[2] = new Shape(Irbis.Irbis.squareList[1].Collider, true);
                        Irbis.Irbis.shadowShapes[3] = new Shape(Irbis.Irbis.squareList[2].Collider, true);
                        Irbis.Irbis.shadowShapes[4] = new Shape(Irbis.Irbis.squareList[3].Collider, true);
                    }

                    Irbis.Irbis.squareList[4].rotation = newrotation;
                    Irbis.Irbis.squareList[5].rotation = -newrotation;
                    Irbis.Irbis.grassList[7].rotation = newrotation;
                    Irbis.Irbis.grassList[8].rotation = -newrotation;

                    Irbis.Irbis.grassList[1].area.Y = (int)newYpos - 1;
                    Irbis.Irbis.grassList[3].area.Y = (int)newYpos-1;
                    Irbis.Irbis.grassList[5].area.Y = (int)newYpos-1;

                    if (castingTime <= 0)
                    {
                        bossState++;
                        health = maxHealth;
                        Irbis.Irbis.CameraShake(0.5f, 5);
                    }
                }
                else // if it's not the right level, then just move on
                {
                    bossState++;
                    goto case BossState.Engage;
                }
                break;
            case BossState.Engage:      // 2
                bossState++;
                SetAnimation(Animation.Teleport, true);
                goto case BossState.Combat;
            case BossState.Combat:      // 3
                if (Irbis.Irbis.jamie != null)
                {
                    playerDistanceSQR = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider, TrueCenter);

                    if (aiEnabled && timer[0] <= 0 && stunned <= 0)
                    {/*
                        if (casting != 0) // teleport
                        {
                            timer[1] -= Irbis.Irbis.DeltaTime;
                            if (timer[1] <= 0)
                            { Cast(1); }
                        }

                        if (timer[2] > 0) // nova
                        { timer[2] -= Irbis.Irbis.DeltaTime; }
                        else if (timer[6] > 0) // fireball delay
                        { timer[6] -= Irbis.Irbis.DeltaTime; }

                        if (timer[3] > 0) // bolts
                        { timer[3] -= Irbis.Irbis.DeltaTime; }
                        else if (timer[7] > 0) // bolts delay
                        { timer[7] -= Irbis.Irbis.DeltaTime; }

                        if (casting != 4) // lazers
                        {
                            timer[4] -= Irbis.Irbis.DeltaTime;
                            if (timer[4] <= 0)
                            {
                                Cast(4);
                                timer[5] = castTime[4];
                            }
                        } /**/



                        switch (combatPhase)
                        {
                            case 1: // phase 1
                                Casting(1);
                                if (Casting(2))
                                { timer[6] = castTime[2]; }
                                if (Casting(4))
                                { timer[5] = castTime[4]; }


                                if (casting == 4)
                                {
                                    if (timer[5] <= 0)
                                    {
                                        ShootLazer();
                                        if (lazerindex >= 5)
                                        {
                                            casting = lazerindex = 0;
                                            timer[4] = cooldown[4];
                                            timer[5] = castTime[4];
                                            //ResetCast();
                                        }
                                    }
                                    else
                                    { timer[5] -= Irbis.Irbis.DeltaTime; }
                                }
                                else if (casting == 1)
                                {
                                    SetAnimation(Animation.Teleport, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (casting == 2)
                                {
                                    timer[6] -= Irbis.Irbis.DeltaTime;
                                    if (timer[6] <= 0)
                                    {
                                        Vector2 toJamie = Vector2.Normalize(Irbis.Irbis.jamie.TrueCenter - TrueCenter);
                                        fireballs.Add(new Fireball(collider.Center, 4,
                                            (toJamie * 10f + new Vector2((Irbis.Irbis.RandomFloat - 1), (Irbis.Irbis.RandomFloat - 1))) * 20f, 20));
                                        fireballTTL.Add(30f);
                                        fireballCastingCount++;

                                        if (fireballCastingCount >= 3)
                                        {
                                            timer[2] = cooldown[2];
                                            timer[0] = cooldown[0];
                                            fireballCastingCount = 0;
                                            casting = 0;
                                        }
                                        else if (fireballCastingCount == 1)
                                        { SetAnimation(Animation.Cast, true); }
                                        timer[6] = cooldown[6];
                                    }
                                }
                                else if (casting == 3)
                                { ResetCast(); } // don't do anything during this phase

                                /*if (teleportFastTick > playerDistanceSQR)
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
                                Casting(1);
                                if (Casting(2))
                                { timer[6] = castTime[2]; }
                                if (Casting(3))
                                { timer[7] = castTime[3]; }
                                if (Casting(4))
                                { timer[5] = castTime[4]; }


                                if (casting == 4)
                                {
                                    if (timer[5] <= 0)
                                    {
                                        ShootLazer();
                                        if (lazerindex >= 5)
                                        {
                                            casting = lazerindex = 0;
                                            timer[4] = cooldown[4];
                                            timer[5] = castTime[4];
                                            //ResetCast();
                                        }
                                    }
                                    else
                                    { timer[5] -= Irbis.Irbis.DeltaTime; }
                                }
                                else if (casting == 1)
                                {
                                    SetAnimation(Animation.Teleport, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (casting == 3)
                                {
                                    timer[7] -= Irbis.Irbis.DeltaTime;
                                    if (timer[7] <= 0)
                                    {
                                        ResetCast();
                                        bolts.Add(new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 2, 100, 1, 30, 10, 250));
                                        boltsTTL.Add(30f);
                                        timer[3] = cooldown[3];
                                        timer[0] = cooldown[0];
                                        timer[7] = castTime[3];
                                        SetAnimation(Animation.Cast, true);
                                    }
                                }
                                else if (casting == 2)
                                {
                                    timer[6] -= Irbis.Irbis.DeltaTime;
                                    if (timer[6] <= 0)
                                    {
                                        ResetCast();
                                        firenovas.Add(new Nova(collider.Center, 5, 12, 0, 100, 20));
                                        firenovaTTL.Add(30f);
                                        timer[2] = cooldown[2];
                                        timer[0] = cooldown[0];
                                        timer[6] = castTime[2];
                                        SetAnimation(Animation.Cast, true);
                                    }
                                }

                                /*if (teleportFastTick > playerDistanceSQR)
                                { timer[1] -= Irbis.Irbis.DeltaTime * 3; }
                                else if (boltsFastTick > playerDistanceSQR)
                                { timer[3] -= Irbis.Irbis.DeltaTime * 3; }
                                if (novaFastTick > playerDistanceSQR)
                                { timer[2] -= Irbis.Irbis.DeltaTime * 3; }
                                if (lazersFastTick > playerDistanceSQR)
                                { timer[4] -= Irbis.Irbis.DeltaTime; }/**/

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
                                Casting(1);
                                if (Casting(2))
                                { timer[6] = castTime[2]; }
                                if (Casting(3))
                                { timer[7] = castTime[3]; }
                                if (Casting(4))
                                { timer[5] = castTime[4]; }
                                if (Casting(8))
                                { timer[8] = castTime[8]; }


                                if (casting == 4)
                                {
                                    if (timer[5] <= 0)
                                    { ShootLazer(); }
                                    else
                                    { timer[5] -= Irbis.Irbis.DeltaTime; }
                                }
                                else if (casting == 1)
                                {
                                    SetAnimation(Animation.Teleport, true);
                                    // play teleport animation
                                    // trigger "explosion"
                                    // set position on noloop

                                    timer[1] = cooldown[1];
                                    timer[0] = cooldown[0];
                                }
                                else if (casting == 3)
                                {
                                    timer[7] -= Irbis.Irbis.DeltaTime;
                                    if (timer[7] <= 0)
                                    {
                                        if (boltsCastingCount >= 1)
                                        {
                                            bolts.Add(new ChargedBolts(TrueCenter, 5, (MathHelper.Pi / 10), 1, 100, 1, 40, 10, 250));
                                            boltsTTL.Add(30f);
                                            boltsCastingCount = 0;
                                            timer[3] = cooldown[3];
                                            timer[0] = cooldown[0];
                                            timer[7] = castTime[3];
                                            ResetCast();
                                        }
                                        else
                                        {
                                            bolts.Add(new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 1, 100, 1, 40, 10, 250));
                                            boltsTTL.Add(30f);
                                            boltsCastingCount++;
                                            timer[7] = cooldown[7];
                                        }
                                        SetAnimation(Animation.Cast, true);
                                    }
                                }
                                else if (casting == 2)
                                {
                                    timer[6] -= Irbis.Irbis.DeltaTime;
                                    if (timer[6] <= 0)
                                    {
                                        if (fireballCastingCount >= 1)
                                        {
                                            firenovas.Add(new Nova(collider.Center, 5, 12, MathHelper.Pi / 12, 200, 40));
                                            firenovaTTL.Add(30f);
                                            fireballCastingCount = 0;
                                            timer[2] = cooldown[2];
                                            timer[0] = cooldown[0];
                                            timer[6] = castTime[2];
                                            ResetCast();
                                        }
                                        else
                                        {
                                            firenovas.Add(new Nova(collider.Center, 5, 12, 0, 200, 40));
                                            firenovaTTL.Add(30f);
                                            timer[6] = cooldown[6];
                                            fireballCastingCount++;
                                        }
                                        SetAnimation(Animation.Cast, true);
                                    }
                                }
                                else if (casting == 8)
                                {
                                    timer[8] -= Irbis.Irbis.DeltaTime;
                                    if (timer[8] <= 0)
                                    {
                                        SetAnimation(Animation.Cast, true);
                                        Heal(100);
                                        timer[8] = cooldown[8];
                                        timer[0] = cooldown[0];
                                        ResetCast();
                                    }
                                }

                                if (health > maxHealth * (3f / 5f))
                                { combatPhase = 2; }

                                /*if (teleportFastTick > playerDistanceSQR)
                                { timer[1] -= Irbis.Irbis.DeltaTime; }
                                else if (boltsFastTick > playerDistanceSQR)
                                { timer[3] -= Irbis.Irbis.DeltaTime; }
                                if (novaFastTick > playerDistanceSQR)
                                { timer[2] -= Irbis.Irbis.DeltaTime; }
                                if (lazersFastTick > playerDistanceSQR)
                                { timer[4] -= Irbis.Irbis.DeltaTime; }/**/

                                break;
                        }
                    }
                    else if (timer[0] > 0)
                    { timer[0] -= Irbis.Irbis.DeltaTime; }
                }
                break;
            case BossState.Disengage:   // 4
                Irbis.Irbis.BossVictory();
                bossState = BossState.Death;
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

        if (currentAnimation == 1)
        {
            //leftHand = Irbis.Irbis.Lerp(previousHands[0], castingHands[currentFrame][0], timeSinceLastFrame / animationSpeed[currentAnimation]);
            //rightHand = Irbis.Irbis.Lerp(previousHands[1], castingHands[currentFrame][1], timeSinceLastFrame / animationSpeed[currentAnimation]);
            //leftHand = Irbis.Irbis.Lerp(leftHand, castingHands[currentFrame][0], 25f * Irbis.Irbis.DeltaTime);
            //rightHand = Irbis.Irbis.Lerp(rightHand, castingHands[currentFrame][1], 25f * Irbis.Irbis.DeltaTime);
            leftHand = castingHands[currentFrame][0];
            rightHand = castingHands[currentFrame][1];

            for (int i = hands.Count - 1; i >= 0; i--)
            {
                if (i % 2 == 0)
                { hands[i].Update(leftHand + position, true); }
                else
                { hands[i].Update(rightHand + position, true); }
            }
        }
        else
        {
            for (int i = hands.Count - 1; i >= 0; i--)
            {
                if (i % 2 == 0)
                { hands[i].Update(leftHand + position, false); }
                else
                { hands[i].Update(rightHand + position, false); }
            }
        }


        return true;
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        {
            Update();
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

    public bool Enemy_OnVictory()
    {
        bossState = BossState.Death;
        return true;
    }

    public void Death()
    {
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave -= Enemy_OnPlayerShockwave;
        for (int i = firenovas.Count - 1; i >= 0; i--)
        { firenovas[i].Death(); }
        //Irbis.Irbis.jamie.OnPlayerShockwave -= firenova.Enemy_OnPlayerShockwave;
        Irbis.Irbis.OnVictory -= Enemy_OnVictory;
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
        collider = standardCollider;
        collider.X += (int)Math.Round((double)position.X);
        collider.Y += (int)Math.Round((double)position.Y);
    }

    public void Animate()
    {
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= animationSpeed[currentAnimation])
        {
            if (currentAnimation == 1)
            { previousHands = castingHands[currentFrame]; }

            currentFrame++;
            timeSinceLastFrame -= animationSpeed[currentAnimation];

            if (currentFrame > animationFrames[currentAnimation])
            {
                if (animationNoLoop)
                {
                    switch (currentAnimation)
                    {
                        case 2:
                            int nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length);
                            while (previousTeleport == nextTeleport)
                            { nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length); }
                            previousTeleport = nextTeleport;
                            Explode();
                            TrueCenter = teleportPoints[previousTeleport];
                            goto default;
                        case 3: // Cast - do nothing, use ResetCast();
                            SetAnimation(Animation.Idle, false);
                            break;
                        default:
                            ResetCast();
                            break;
                    }
                }
                else
                { currentFrame = 0; }
            }
        }

        if (previousActivity != activity)
        { SetAnimation(); }

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
                SetAnimation(Animation.Idle, false);
                break;

            default:
                SetAnimation(Animation.Idle, false);
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

    public void SetAnimation(Animation animation, bool noLoop)
    { SetAnimation((int)animation, noLoop); } // I hate this but couldn't find an easy workaround

    private void ShootLazer()
    {
        SetAnimation(Animation.Cast, true);
        switch (lazerindex)
        {
            case 0:
                lazers.Clear();
                lazers.Add(new Lazor(TrueCenter, 15, new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 50));
                break;
            case 1:
                lazers.Add(new Lazor(TrueCenter, 15, Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 50));
                break;
            case 2:
                lazers.Add(new Lazor(TrueCenter, 15, new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 50));
                break;
            case 3:
                lazers.Add(new Lazor(TrueCenter, 15, Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 50));
                break;
            case 4:
                lazers.Add(new Lazor(TrueCenter, 15, new Vector2(-2000, 0), new Texture2D[] { lazertextures[4], Irbis.Irbis.dottex }, 50));
                break;
            case 5:
                lazers.Add(new Lazor(TrueCenter, 15, Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 50));
                break;
            case 6:
                lazers.Add(new Lazor(TrueCenter, 15, new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 50));
                break;
            case 7:
                lazers.Add(new Lazor(TrueCenter, 15, Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 50));
                break;
            case 8:
                lazers.Add(new Lazor(TrueCenter, 15, new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 50));
                casting = lazerindex = 0;
                timer[4] = cooldown[4];
                break;
        }
        timer[5] = cooldown[5];
        lazerindex++;
    }

    public bool Casting(int Spell)
    {
        if (casting  <= 0 && timer[Spell] > 0) // teleport
        {
            timer[Spell] -= Irbis.Irbis.DeltaTime;
            if (timer[Spell] <= 0)
            { Cast(Spell); }
            return true;
        }
        return false;
    }

    public void Cast(int Spell)
    {
        if (casting != Spell)
        {
            casting = Spell;
            SetAnimation((int)Animation.Charging, false);
            previousHands = castingHands[0];

            switch (casting)
            {
                case 4: // lazers
                    hands.Add(new LazorSpellEffect(LazorHands));
                    hands.Add(new LazorSpellEffect(LazorHands));
                    break;
            }

            //Irbis.Irbis.WriteLine("casting:" + casting);
        }
    }

    public void ResetCast()
    {
        SetAnimation(Animation.Idle, false);
        casting = 0;
        hands.Clear();
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
        SetAnimation(Animation.Dying, true);
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
        if (health <= 0)
        { bossState = BossState.Disengage; }
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
                if (currentAnimation == 1)
                {
                    sb.Draw(Irbis.Irbis.nullTex, (leftHand + position) * Irbis.Irbis.screenScale, null, Color.DeepSkyBlue, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.8f);
                    sb.Draw(Irbis.Irbis.nullTex, (rightHand + position) * Irbis.Irbis.screenScale, null, Color.DeepSkyBlue, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.8f);
                }
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
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, origin, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                for (int i = hands.Count - 1; i >= 0; i--)
                { hands[i].Draw(sb); }
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
