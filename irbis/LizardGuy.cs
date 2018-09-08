using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class LizardGuy : IEnemy
{
    public enum LizardActivity
    {
        Idle = 0,
        WalkLeft = 1,
        WalkRight = 2,
        Roll = 3,
        Jump = 4,
        Bury = 5,
        Emerge = 6,
        Swipe = 7,
        Tailwhip = 8,
        SwipeSwing = 9,
        TailwhipSwing = 10,
        TurningAround = 11,
        Dying = 12,
        GettingUp = 13,
        Misc = 20,
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

    public Vector2 TrueCenter
    {
        get
        { return new Vector2(position.X + standardCollider.X + (standardCollider.Width / 2f), position.Y + standardCollider.Y + (standardCollider.Height / 2f)); }
    }

    public Vector2 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = value;
        }
    }
    private Vector2 velocity;

    public Wall Walled
    {
        get
        {
            return walled;
        }
    }
    private Wall walled;

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
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    private float maxHealth;

    public float SpeedModifier
    {
        get
        {
            return speedModifier;
        }
        set
        {
            speedModifier = value;
        }
    }
    private float speedModifier;

    public List<Enchant> ActiveEffects
    {
        get
        {
            return activeEffects;
        }
    }
    private List<Enchant> activeEffects;

    public string Name
    {
        get
        {
            return name;
        }
    }
    private string name = "Lizard Guy Boss (rawr)";

    public bool AIenabled
    {
        get
        { return (aiEnabled && bossState > 0); }
        set
        { aiEnabled = value; }
    }
    public bool aiEnabled = true;

    public float Mass
    {
        get
        {
            return mass;
        }
    }
    private float mass = 5f;

    public float ShockwaveMaxEffectDistanceSquared
    {
        get
        {
            return shockwaveMaxEffectDistanceSquared;
        }
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


    private bool collidedContains;
    public float speed;
    public float defaultSpeed;
    int climbablePixels;
    public LizardActivity previousActivity;
    public Point prevInput;
    public int nextAnimation;
    public bool interruptAttack;
    public float idleTime;
    public float attackMovementSpeed;
    public bool attackImmediately;
    public Print animationFrame;
    public float airSpeed;

    Texture2D tex;
    public Rectangle displayRect;
    public Rectangle animationSourceRect;
    Rectangle testCollider;
    public Rectangle standardCollider;
    public float terminalVelocity = 5000f;

    float depth;

    //roll variables
    public float rollCooldownTime = 8;
    public float rollStunTime = 5;
    public float rollPauseTimer;
    public float rollPauseTime = 0.1f;
    public float rollTimeMax;
    public float rollTime;
    public float rollSpeed;
    public float rollLeapTime;
    public float rollLeapInitialYvelocity;
    public float rollLerp = 2;
    public float rollDamage = 65;
    public Vector2 rollKnockback = new Vector2(300, -300);

    //swipe variables
    public float swipeChargeTime = 0.5f;
    public float swipeChargeTimer;
    public float swipeCooldownTime = 1.5f;
    public float swipePauseTime = 0.75f;
    public float swipePauseTimer;
    public Vector2 swipeKnockback = new Vector2(100, -250);
    public float swipeDamage = 25f;
    public int swipeRange = 80;
    public bool swipeHit;

    //tailwhip variables
    public float tailwhipChargeTime = 0.5f;
    public float tailwhipChargeTimer;
    public float tailwhipCooldownTime = 2f;
    public float tailwhipPauseTime = 0.75f;
    public Vector2 tailwhipKnockback = new Vector2(700, -500);
    public float tailwhipDamage = 35f;

    public int meleeActivitiesInARow;
    public int meleeActivityLimit = 3;

    //bury variables
    public float buryDigSpeed = 250f;
    public float buryDigLerp = 5f;
    public float buryChargeTime = 0;
    public float buryCooldownTime = 20f;
    public float buryStrikeWaitTime;
    public float buryStrikeMaxWaitTime = 3;
    public float buryStrikeMinWaitTime = 1;
    public float buryStrikeWaitTimer;
    public   int buryStrikeRadius = 100;
    public  Side buryStrikeSide;
    public float buryStrikeChargeTime = 1f;
    public float buryInitialEmergeChance = 0.1f;
    public float buryEmergeChance = 0.1f;
    public float buryStrikeDamage = 35f;
    public Vector2 buryKnockback = new Vector2(100, -250);
    public Vector2 buryStrikeLocation;
    public Point buryStrikeSize = new Point(100, 50);
    public float buryRumbleFrameTimer;
    public   int buryRumbleFrame;
    public float buryStrikeFrameTimer;
    public   int buryStrikeFrame = 10;

    //wander variables
    float wanderSpeed;
    float wanderLerp = 100f;
    float stunLerp = 5f;

    public float stunned;
    public float wanderTime;
    public float initialWanderTime;
    public float jumpTime;
    public float timeSinceLastFrame;
    int currentFrame;
    public int currentAnimation;
    int previousAnimation;
    public float[] animationSpeed = new float[40];
    public int[] animationFrames = new int[40];
    bool animationNoLoop;
    public Point input;
    public bool frameInput;

    public float shockwaveMaxEffectDistance;
    public float shockwaveEffectiveDistance;
    public float shockwaveStunTime;

    public Vector2 shockwaveKnockback;

    public Direction direction;
    public Location location;
    public LizardActivity activity;

    Vector2 amountToMove;
    Vector2 negAmountToMove;
    Vector2 testPos;

    public int lastHitByAttackID;

    public int combatCheckDistanceSqr;
    public int persueCheckDistanceSqr;
    public bool combat;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 50f;
    public static float movementLerpAir = 5f;

    public Rectangle attackCollider;

    public int attackColliderWidth = 100;
    public int attackColliderHeight = 70;

    public float attackDamage;
    public float attack1Damage;

    public float attackCooldown;
    public float attackCooldownTimer;

    public Rectangle bossArena;

    public Collided collided;

    public int[] state = new int[5];
    public float[] cooldown = new float[6];

    public BossState bossState = (BossState)(-1);

    public LizardGuy(Texture2D t, Vector2? iPos, float enemyHealth, float enemyDamage, float enemySpeed, Rectangle? BossArena, float drawDepth)
    {
        cooldown[0] = 00f; // wander
        cooldown[1] = 05f; // roll
        cooldown[2] = 03f; // swipe
        //cooldown[3] = 00f; // none
        cooldown[4] = 15f; // bury
        cooldown[5] = 0f; // entrance

        collided = new Collided();

        rollTimeMax = 0.25f;
        rollSpeed = 1500f;
        rollTime = 0f;
        idleTime = 0f;
        airSpeed = 0.6f * enemySpeed;
        attackMovementSpeed = 0.3f * enemySpeed;
        attackImmediately = false;
        interruptAttack = false;
        climbablePixels = 3;

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.001f);

        tex = t;

        depth = drawDepth;

        climbablePixels = 3;

        direction = Direction.Forward;
        location = Location.Air;
        wanderSpeed = (1f / 5f) * enemySpeed;
        wanderTime = 1f;
        initialWanderTime = 0f;

        speed = defaultSpeed = enemySpeed;
        animationNoLoop = false;
        standardCollider.Location = new Point(44, 26);
        collider.Size = trueCollider.Size = standardCollider.Size = new Point(60, 100);

        position.X -= standardCollider.X;
        position.Y -= standardCollider.Y;

        maxHealth = health = enemyHealth;
        lastHitByAttackID = -1;

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

        shockwaveMaxEffectDistance = Irbis.Irbis.jamie.shockwaveEffectiveDistance;
        shockwaveMaxEffectDistanceSquared = shockwaveMaxEffectDistance * shockwaveMaxEffectDistance;
        shockwaveEffectiveDistance = 200;
        shockwaveStunTime = Irbis.Irbis.jamie.shockwaveStunTime;
        shockwaveKnockback = Irbis.Irbis.jamie.shockwaveKnockback;

        displayRect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;
        animationSpeed[0] = 0.1f;
        for (int i = 1; i < 40; i++)
        { animationSpeed[i] = animationSpeed[0]; }

        animationFrames[0] = 0;
        animationFrames[1] = 0;
        animationFrames[2] = 0;
        animationFrames[3] = 0;
        animationFrames[4] = 0;
        animationFrames[5] = 0;
        animationFrames[6] = 2;
        animationFrames[7] = 2;
        animationFrames[8] = 2;
        animationFrames[9] = 2;
        animationFrames[10] = 0;
        animationFrames[11] = 0;
        animationFrames[12] = 0;
        animationFrames[13] = 0;
        animationFrames[14] = 0;
        animationFrames[15] = 0;
        animationFrames[16] = 0;
        animationFrames[17] = 0;
        animationFrames[18] = 0;
        animationFrames[19] = 0;
        animationFrames[20] = 0;
        animationFrames[21] = 0;
        animationFrames[22] = 0;
        animationFrames[23] = 0;
        animationFrames[24] = 0;
        animationFrames[25] = 0;
        animationFrames[26] = 1;
        animationFrames[27] = 1;
        animationFrames[28] = 5;
        animationFrames[29] = 5;
        animationFrames[30] = 4;
        animationFrames[31] = 4;
        animationFrames[32] = 0;
        animationFrames[33] = 0;
        animationFrames[34] = 0;
        animationFrames[35] = 0;
        animationFrames[36] = 0;
        animationFrames[37] = 0;
        animationFrames[38] = 3; // bury swipe
        animationFrames[39] = 2; // rumblies

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

        if (iPos == null)
        { position = new Vector2(bossArena.X - standardCollider.X - standardCollider.Width / 2, bossArena.Y - standardCollider.Y - standardCollider.Height); }
        else
        { position = (Vector2)iPos; }

        if ((bossArena.Height - standardCollider.Height) > 300)
        { rollLeapInitialYvelocity = -(float)Math.Sqrt(2 * Irbis.Irbis.gravity * mass * 300); }
        else
        { rollLeapInitialYvelocity = -(float)Math.Sqrt(2 * Irbis.Irbis.gravity * mass * (bossArena.Height - standardCollider.Height)); }
        rollLeapTime = -rollLeapInitialYvelocity / (Irbis.Irbis.gravity * mass);
        Irbis.Irbis.WriteLine("rollLeapInitialYvelocity:" + rollLeapInitialYvelocity + " rollLeapTime:" + rollLeapTime);

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave += Enemy_OnPlayerShockwave;
    }

    public bool Update()
    {
        prevInput = input;
        input = Point.Zero;
        frameInput = false;
        switch (bossState)
        {
            case BossState.Spawn:
                velocity.X = bossArena.Width / ((float)Math.Sqrt(bossArena.Height * 2f / Irbis.Irbis.gravity));
                velocity.Y = Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime;
                direction = Direction.Right;
                SetAnimation(0, false);
                bossState = BossState.Entrance;
                Irbis.Irbis.WriteLine(this.ToString());
                break;
            case BossState.Entrance:
                velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime;
                health = Irbis.Irbis.Clamp(maxHealth * (((float)trueCollider.Bottom - (float)bossArena.Top) / (float)bossArena.Height), 0, maxHealth);
                
                if (trueCollider.Bottom >= bossArena.Bottom)
                {
                    Irbis.Irbis.CameraShake(0.5f, 15f);
                    Collision(Irbis.Irbis.collisionObjects);
                    velocity = Vector2.Zero;
                    cooldown[5] = 0f;
                    health = maxHealth;
                    bossState = BossState.Engage;
                    Irbis.Irbis.WriteLine(this.ToString());
                }
                break;
            case BossState.Engage:
                cooldown[5] += Irbis.Irbis.DeltaTime;
                velocity = Vector2.Zero;
                if (cooldown[5] >= 3f)
                {
                    cooldown[5] = 0;
                    bossState = BossState.Combat;
                    Irbis.Irbis.WriteLine(this.ToString());
                }
                break; 
            case BossState.Combat:
                if (Irbis.Irbis.jamie != null)
                {
                    if (aiEnabled && !ActivelyAttacking && cooldown[0] <= 0 && stunned <= 0 && walled.Bottom > 0)
                    {
                        Direction playerDirection = Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider);
                        if (meleeActivitiesInARow >= meleeActivityLimit)
                        {
                            Irbis.Irbis.WriteLine(name + " melee activity limit reached.");
                            if (Irbis.Irbis.RandomBool)
                            { TriggerBuryAttack(); }
                            else
                            {
                                if (Irbis.Irbis.Directions(bossArena, trueCollider) == Direction.Right)
                                { state[1] = 2; /*state = 2 to roll right*/ }
                                else
                                { state[1] = 1; /*state = 1 to roll left*/ }
                            }

                            meleeActivitiesInARow = 0;
                        }
                        else if (playerDirection != direction && playerDirection != Direction.Forward)
                        { // turn around
                            Irbis.Irbis.WriteLine(name + " not facing player. turning around...");
                            direction = playerDirection;
                            activity = LizardActivity.TurningAround;
                            meleeActivitiesInARow++;
                        }
                        else if (Irbis.Irbis.jamie.Collider.Center.X < bossArena.Center.X && trueCollider.Center.X < bossArena.Center.X && Irbis.Irbis.jamie.stunTime <= 0.5f && cooldown[1] <= 0)
                        { // roll left
                          //player is on left side of screen
                          //leap to right side of screen, roll to the left
                            Irbis.Irbis.WriteLine(name + " roll attack: roll left");
                            state[1] = 1;   //state = 1 to roll left
                            meleeActivitiesInARow = 0;
                        }
                        else if (Irbis.Irbis.jamie.Collider.Center.X > bossArena.Center.X && trueCollider.Center.X > bossArena.Center.X && Irbis.Irbis.jamie.stunTime <= 0.5f && cooldown[1] <= 0)
                        { // roll right
                          //player is on right side of screen
                          //leap to left side of screen, roll to the right
                            Irbis.Irbis.WriteLine(name + " roll attack: roll right");
                            state[1] = 2;   //state = 2 to roll right
                            meleeActivitiesInARow = 0;
                        }
                        else if ((cooldown[4] <= 0 && Irbis.Irbis.XDistance(trueCollider, Irbis.Irbis.jamie.Collider) > swipeRange))
                        { // bury
                          //Irbis.Irbis.WriteLine(name + " bury");
                            TriggerBuryAttack();
                            meleeActivitiesInARow = 0;
                        }
                        else if (cooldown[2] <= 0 && Irbis.Irbis.XDistance(trueCollider, Irbis.Irbis.jamie.Collider) <= swipeRange)
                        { // swipe/tailwhip
                            meleeActivitiesInARow++;
                            if (direction == playerDirection)
                            {
                                // use rng to determine if it's a swipe or tailwhip
                                if (Irbis.Irbis.RandomInt(3) > 0)
                                {
                                    Irbis.Irbis.WriteLine(name + " swipe");
                                    if (direction == Direction.Left)
                                    { state[2] = 1; }
                                    else
                                    { state[2] = 2; }
                                    activity = LizardActivity.Swipe;
                                }
                                else
                                {
                                    Irbis.Irbis.WriteLine(name + " tailwhip");
                                    if (direction == Direction.Left)
                                    { state[3] = 1; }
                                    else
                                    { state[3] = 2; }
                                    activity = LizardActivity.Tailwhip;
                                }
                            }
                            else
                            {
                                Irbis.Irbis.WriteLine(name + " swipe/tailwhip failed, facing wrong direction. turning around... (ERROR)");
                                cooldown[2] = 0.5f;
                                if (direction == Direction.Left)
                                { direction = Direction.Right; }
                                else
                                { direction = Direction.Left; }
                                Stun(0.5f);
                                activity = LizardActivity.TurningAround;
                            }
                        }
                        else
                        { // wander around, maybe yell a bit, just don't look like a doofus
                            Wander((Irbis.Irbis.RandomInt(3) + 1));
                        }
                    }
                }

                if (Irbis.Irbis.IsTouching(trueCollider, Irbis.Irbis.jamie.Collider))
                {
                    if (stunned <= 0)
                    {
                        collider = Rectangle.Empty;
                        if (state[1] >= 9 && Irbis.Irbis.jamie.HurtOnTouch(rollDamage / 2, Irbis.Irbis.Directions(Irbis.Irbis.jamie.Collider.Center, TrueCollider.Center)))
                        { OnTouch(trueCollider, rollKnockback); }
                        else if (state[1] > 0 && Irbis.Irbis.jamie.HurtOnTouch(rollDamage, Irbis.Irbis.Directions(Irbis.Irbis.jamie.Collider.Center, TrueCollider.Center)))
                        { OnTouch(trueCollider, rollKnockback); }
                        else if (Irbis.Irbis.jamie.HurtOnTouch(10f, Irbis.Irbis.Directions(Irbis.Irbis.jamie.Collider.Center, TrueCollider.Center)))
                        { OnTouch(trueCollider, swipeKnockback); }
                    }
                    else if (Irbis.Irbis.jamie.collided.Horizontal || Irbis.Irbis.jamie.collided.Vertical)
                    {
                        collider = Rectangle.Empty;
                        OnTouch(trueCollider, swipeKnockback);
                    }
                }
                else if (collider == Rectangle.Empty && Irbis.Irbis.jamie.invulnerableOnTouch <= 0)
                { collider = trueCollider; }

                if (health <= 0 || position.Y > 5000f)
                { Irbis.Irbis.KillEnemy(this); }
                break;
            case BossState.Disengage:
                break;
            case BossState.Death:
                break;
        }


        Movement();
        CalculateMovement();
        Animate();
        if (state[4] <= 0 && bossState > (BossState)1)
        { Collision(Irbis.Irbis.collisionObjects); }
        return true;
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        { Update(); }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            { Irbis.Irbis.doneEvent.Set(); }
        }
    }

    public void StartUp(object dump)
    {
        Irbis.Irbis.WriteLine(name + " starting up...");
        bossState = BossState.Spawn;
        Irbis.Irbis.WriteLine(this.ToString());
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
        float DistanceSQR = Irbis.Irbis.DistanceSquared(trueCollider, Origin);
        if (DistanceSQR <= Range * Range)
        {
            float Distance = (float)Math.Sqrt(DistanceSQR);
            if (Power > 1.5f)
            {
                if (state[1] >= 7)
                {
                    state[1] = 100;
                    stunned = ((Range - Distance) / Range) * shockwaveStunTime * Power;
                    Irbis.Irbis.WriteLine("roll interrupted! stunned for:" + stunned);
                    activity = LizardActivity.Dying;
                }
                else if (ActivelyAttacking)
                {
                    if (state[2] > 0 || state[3] > 0)
                    {
                        if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
                        { velocity += new Vector2(-shockwaveKnockback.X, shockwaveKnockback.Y) * (Range - Distance) * Power * (1 / mass); }
                        else
                        { velocity += shockwaveKnockback * (Range - Distance) * Power * (1 / mass); }
                    }
                    stunned = 1.5f;
                    Irbis.Irbis.WriteLine("attack slowed! stunned for:" + stunned);
                }
                else
                {
                    if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
                    { velocity += new Vector2(-shockwaveKnockback.X, shockwaveKnockback.Y) * (Range - Distance) * Power * (1 / mass); }
                    else
                    { velocity = shockwaveKnockback * (Range - Distance) * Power * (1 / mass); }
                    stunned = ((Range - Distance) / Range) * shockwaveStunTime * Power;
                    activity = LizardActivity.Dying;
                    Irbis.Irbis.WriteLine("attack slowed! stunned for:" + stunned);
                }
            }
            else
            {
                if (state[1] >= 7)
                {
                    state[1] += 2;
                    if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
                    { velocity.X += -shockwaveKnockback.X * (Range - Distance) * Power * (1 / mass); }
                    else
                    { velocity.X += shockwaveKnockback.X * (Range - Distance) * Power * (1 / mass); }
                    Irbis.Irbis.WriteLine("roll slowed! stunned for:" + stunned);
                }
                else if (ActivelyAttacking)
                {
                    if (state[2] > 0 || state[3] > 0)
                    {
                        if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
                        { velocity += new Vector2(-shockwaveKnockback.X, shockwaveKnockback.Y) * (Range - Distance) * Power * (1 / mass); }
                        else
                        { velocity += shockwaveKnockback * (Range - Distance) * Power * (1 / mass); }
                    }
                    stunned = 0.5f;
                    Irbis.Irbis.WriteLine("attack slowed! stunned for:" + stunned);
                }
                else
                {
                    if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
                    { velocity += new Vector2(-shockwaveKnockback.X, shockwaveKnockback.Y) * (Range - Distance) * Power * (1 / mass); }
                    else
                    { velocity += shockwaveKnockback * (Range - Distance) * Power * (1 / mass); }
                    stunned = ((Range - Distance) / Range) * shockwaveStunTime * Power;
                    activity = LizardActivity.Dying;
                    Irbis.Irbis.WriteLine("attack slowed! stunned for:" + stunned);
                }
            }
        }
        Irbis.Irbis.WriteLine(name + " done.\n");
        return true;
    }

    public void Death()
    {
        Irbis.Irbis.WriteLine(this.ToString());
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave -= Enemy_OnPlayerShockwave;
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

    public void PlayerAttackCollision(float Damage)
    {
        Hurt(Damage);

        foreach (Enchant enchant in Irbis.Irbis.jamie.enchantList)
        { enchant.AddEffect(this); }
    }

    public void Movement()
    {
        if (stunned > 0)
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, stunLerp * Irbis.Irbis.DeltaTime);
            stunned -= Irbis.Irbis.DeltaTime;
            if (stunned <= 0)
            {
                stunned = 0;
                if (!ActivelyAttacking)
                { GetUp(); }
            }
        }
        else if (ActivelyAttacking)
        {
            if (state[1] > 0)
            { RollAttack(); }
            if (state[2] > 0)
            { SwipeAttack(); }
            if (state[3] > 0)
            { TailwhipAttack(); }
            if (state[4] > 0)
            { BuryAttack(); }
        }
        else if (cooldown[0] > 0 && walled.Bottom > 0 && activity != LizardActivity.TurningAround && activity != LizardActivity.GettingUp)
        {
            Direction playerDirection = Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider);
            if (playerDirection != direction && playerDirection != Direction.Forward && activity != LizardActivity.WalkLeft && activity != LizardActivity.WalkRight)
            { // turn around
                Irbis.Irbis.WriteLine(name + " not facing player. turning around...");
                direction = playerDirection;
                activity = LizardActivity.TurningAround;
                meleeActivitiesInARow++;
            }
            else if (activity != LizardActivity.TurningAround)
            {
                Wander();
                cooldown[0] -= Irbis.Irbis.DeltaTime;

                if (velocity.X > 0.1f)
                { activity = LizardActivity.WalkRight; }
                else if (velocity.X < -0.1f)
                { activity = LizardActivity.WalkLeft; }
                else
                { activity = LizardActivity.Idle; }
            }
        }

        if (cooldown[1] > 0)
        { cooldown[1] -= Irbis.Irbis.DeltaTime; }
        if (cooldown[2] > 0)
        { cooldown[2] -= Irbis.Irbis.DeltaTime; }
        if (cooldown[3] > 0)
        { cooldown[3] -= Irbis.Irbis.DeltaTime; }
        if (cooldown[4] > 0)
        { cooldown[4] -= Irbis.Irbis.DeltaTime; }

        if (walled.Top > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }

        position += velocity * Irbis.Irbis.DeltaTime;
    }

    public void Wander()
    {
        if (cooldown[0] > (initialWanderTime / 1.5f))
        { velocity.X = Irbis.Irbis.Lerp(velocity.X, speed, wanderLerp * Irbis.Irbis.DeltaTime); }
        else
        { velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, wanderLerp * Irbis.Irbis.DeltaTime); }
    }

    public bool Wander(float WanderTime)
    {
        if (walled.Left > 0)
        { speed = wanderSpeed; }
        else if (walled.Right > 0)
        { speed = -wanderSpeed; }
        else
        {
            if (Irbis.Irbis.RandomBool)
            { speed = wanderSpeed; }
            else
            { speed = -wanderSpeed; }
        }
        initialWanderTime = cooldown[0] = WanderTime;
        return true;
    }

    public void RollAttack()
    { // Roll == state[1] && cooldown[1]
        switch (state[1])
        {
            case 1: //leap right
                velocity = new Vector2((Irbis.Irbis.UnidirectionalDistance(trueCollider.Right, bossArena.Right) / (rollLeapTime * 2)), rollLeapInitialYvelocity);
                Irbis.Irbis.WriteLine("leap right velocity:" + velocity);
                position.Y -= 1;
                state[1] = 3;
                Irbis.Irbis.CameraShake(0.1f, 3f);
                direction = Direction.Left;
                break;
            case 2: //leap left
                velocity = new Vector2(-(Irbis.Irbis.UnidirectionalDistance(trueCollider.Left, bossArena.Left) / (rollLeapTime * 2)), rollLeapInitialYvelocity);
                Irbis.Irbis.WriteLine("leap left velocity:" + velocity);
                position.Y -= 1;
                state[1] = 4;
                Irbis.Irbis.CameraShake(0.1f, 3f);
                activity = LizardActivity.Roll;
                break;
            case 3: //in the air, waiting to hit the ground
                activity = LizardActivity.Roll;
                if (walled.Bottom > 0)
                {
                    state[1] = 5;
                    rollPauseTimer = 0f;
                    Irbis.Irbis.WriteLine(name + " has hit the ground, now pausing before rolling");
                    Irbis.Irbis.CameraShake(1f, 1.5f);
                }
                break;
            case 4: //in the air, waiting to hit the ground
                direction = Direction.Right;
                if (walled.Bottom > 0)
                {
                    state[1] = 6;
                    rollPauseTimer = 0f;
                    Irbis.Irbis.WriteLine(name + " has hit the ground, now pausing before rolling");
                    Irbis.Irbis.CameraShake(1f, 1.5f);
                }
                break;
            case 5: //hit the ground, waiting to roll
                rollPauseTimer += Irbis.Irbis.DeltaTime;
                if (rollPauseTimer >= rollPauseTime)
                {
                    state[1] = 7;
                    Irbis.Irbis.WriteLine(name + " done pausing, now rolling left");
                }
                break;
            case 6: //hit the ground, waiting to roll
                rollPauseTimer += Irbis.Irbis.DeltaTime;
                if (rollPauseTimer >= rollPauseTime)
                {
                    state[1] = 8;
                    Irbis.Irbis.WriteLine(name + " done pausing, now rolling right");
                }
                break;
            case 7: //roll left
                if (walled.Left <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, -rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                { goto case 11; }
                break;
            case 8: //roll right
                if (walled.Right <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                { goto case 11; }
                break;
            case 9: //roll left
                if (walled.Left <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, -rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                { goto case 11; }
                break;
            case 10: //roll right
                if (walled.Right <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                { goto case 11; }
                break;
            case 11: //stunned
                Irbis.Irbis.WriteLine(name + " hit wall, stunned");
                Irbis.Irbis.CameraShake(0.15f, 15f);
                Stun(rollStunTime);
                activity = LizardActivity.Dying;
                state[1] = 100;
                break;
            default:
                if (stunned <= 0)
                {
                    Irbis.Irbis.WriteLine(name + " recovered");
                    cooldown[1] = rollCooldownTime;
                    Wander(2);
                    state[1] = 0;
                    GetUp();
                }
                break;
        }
    }

    public void SwipeAttack()
    { // swipe == state[2] && cooldown[2]
      // swipe and tailwhip share a cooldown
        switch (state[2])
        {
            case 1: //charge/ready to swipe left
                swipeChargeTimer += Irbis.Irbis.DeltaTime;
                if (swipeChargeTimer >= swipeChargeTime)
                { state[2] = 3; }
                break;
            case 2: //charge/ready to swipe right
                swipeChargeTimer += Irbis.Irbis.DeltaTime;
                if (swipeChargeTimer >= swipeChargeTime)
                { state[2] = 4; }
                break;
            case 3:
                Irbis.Irbis.WriteLine("swipe left");
                swipeChargeTimer = 0;
                activity = LizardActivity.SwipeSwing;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-swipeKnockback.X, swipeKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(swipeDamage, false, Direction.Left);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                    swipeHit = true;
                    Irbis.Irbis.WriteLine("pausing for " + swipePauseTime + " seconds");
                }
                else
                { swipeHit = false; }
                state[2] = 5;
                break;
            case 4:
                Irbis.Irbis.WriteLine("swipe right");
                swipeChargeTimer = 0;
                activity = LizardActivity.SwipeSwing;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = swipeKnockback;
                    Irbis.Irbis.jamie.Hurt(swipeDamage, false, Direction.Right);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                    swipeHit = true;
                    Irbis.Irbis.WriteLine("pausing for " + swipePauseTime + " seconds");
                }
                else
                { swipeHit = false; }
                state[2] = 6;
                break;
            case 5:
                if (currentAnimation == 0 || currentAnimation == 1)
                {
                    if (swipeHit)
                    {
                        state[2] = 7;
                        activity = LizardActivity.Swipe;
                    }
                    else
                    {
                        state[2] = 0;
                        activity = LizardActivity.Idle;
                    }
                    SetAnimation();
                }
                break;
            case 6:
                if (currentAnimation == 0 || currentAnimation == 1)
                {
                    if (swipeHit)
                    {
                        state[2] = 7;
                        activity = LizardActivity.Swipe;
                    }
                    else
                    {
                        state[2] = 0;
                        activity = LizardActivity.Idle;
                    }
                    SetAnimation();
                }
                break;
            case 7:
                swipePauseTimer += Irbis.Irbis.DeltaTime;
                if (swipePauseTimer >= swipePauseTime)
                {
                    state[2] = 9;
                    swipePauseTimer = 0;
                }
                break;
            case 8:
                swipePauseTimer += Irbis.Irbis.DeltaTime;
                if (swipePauseTimer >= swipePauseTime)
                {
                    state[2] = 10;
                    swipePauseTimer = 0;
                }
                break;
            case 9:
                Irbis.Irbis.WriteLine("swipe left again");
                activity = LizardActivity.SwipeSwing;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-swipeKnockback.X, swipeKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(swipeDamage, false, Direction.Left);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                state[2] = 11;
                break;
            case 10:
                Irbis.Irbis.WriteLine("swipe right again");
                activity = LizardActivity.SwipeSwing;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = swipeKnockback;
                    Irbis.Irbis.jamie.Hurt(swipeDamage, false, Direction.Right);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                state[2] = 11;
                break;
            case 11:
                if (currentAnimation == 0 || currentAnimation == 1)
                {
                    state[2] = 12;
                    activity = LizardActivity.Idle;
                    SetAnimation();
                }
                break;
            case 12:
                cooldown[2] = swipeCooldownTime;
                Wander(1);
                state[2] = 0;
                break;
        }
    }

    public void TailwhipAttack()
    { //tailwhip == state[3] && cooldown[2]
      //swipe and tailwhip share a cooldown
        switch (state[3])
        {
            case 1: //charge/ready to tailwhip left
                tailwhipChargeTimer += Irbis.Irbis.DeltaTime;
                if (tailwhipChargeTimer >= tailwhipChargeTime)
                {
                    activity = LizardActivity.TailwhipSwing;
                    state[3] = 3;
                }
                break;
            case 2: //charge/ready to tailwhip right
                tailwhipChargeTimer += Irbis.Irbis.DeltaTime;
                if (tailwhipChargeTimer >= tailwhipChargeTime)
                {
                    activity = LizardActivity.TailwhipSwing;
                    state[3] = 4;
                }
                break;
            case 3:
                Irbis.Irbis.WriteLine("tailwhip left");
                tailwhipChargeTimer = 0;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-tailwhipKnockback.X, tailwhipKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(tailwhipDamage, true, Direction.Left);
                    Irbis.Irbis.CameraShake(0.1f, 10f);
                }
                state[3] = 5;
                break;
            case 4:
                Irbis.Irbis.WriteLine("tailwhip right");
                tailwhipChargeTimer = 0;
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = tailwhipKnockback;
                    Irbis.Irbis.jamie.Hurt(tailwhipDamage, true, Direction.Right);
                    Irbis.Irbis.CameraShake(0.1f, 10f);
                }
                state[3] = 5;
                break;
            case 5:
                if (currentAnimation == 0 || currentAnimation == 1)
                {
                    state[3] = 6;
                    activity = LizardActivity.Idle;
                    SetAnimation();
                }
                break;
            case 6:
                cooldown[2] = tailwhipCooldownTime;
                Wander(1);
                state[3] = 0;
                break;
        }
    }

    public void BuryAttack()
    {
        //bury == state[4]
        switch (state[4])
        {
            case 1: // dig
                if (currentAnimation == 30 || currentAnimation == 31)
                {
                    SetAnimation(36, false);    //empty animation
                    state[4] = 2;
                    velocity.Y = 0;
                    position.Y = bossArena.Bottom;
                }
                break;
            case 2: // random wait time
                buryStrikeWaitTime = (Irbis.Irbis.RandomFloat * (buryStrikeMaxWaitTime - buryStrikeMinWaitTime)) + buryStrikeMinWaitTime;
                buryStrikeWaitTimer = 0;
                state[4] = 3;
                break;
            case 3: // wait
                buryStrikeWaitTimer += Irbis.Irbis.DeltaTime;
                if (buryStrikeWaitTimer >= buryStrikeWaitTime)
                { state[4] = 4; }
                break;
            case 4: // begin strike phase (pick a random spot within distance of the player)
                // determine which side to strike from
                buryStrikeSide = Irbis.Irbis.SideClosest(bossArena, Irbis.Irbis.jamie.Collider);
                switch (buryStrikeSide)
                {
                    case Side.Bottom:
                        buryStrikeLocation.Y = bossArena.Bottom;
                        buryStrikeLocation.X = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.X;
                        attackCollider = new Rectangle((int)buryStrikeLocation.X - (buryStrikeSize.X / 2), (int)buryStrikeLocation.Y - buryStrikeSize.Y, buryStrikeSize.X, buryStrikeSize.Y);
                        break;
                    case Side.Left:
                        buryStrikeLocation.X = bossArena.Left;
                        buryStrikeLocation.Y = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.Y;
                        attackCollider = new Rectangle((int)buryStrikeLocation.X, (int)buryStrikeLocation.Y - (buryStrikeSize.X / 2), buryStrikeSize.Y, buryStrikeSize.X);
                        break;
                    case Side.Right:
                        buryStrikeLocation.X = bossArena.Right;
                        buryStrikeLocation.Y = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.Y;
                        attackCollider = new Rectangle((int)buryStrikeLocation.X - buryStrikeSize.Y, (int)buryStrikeLocation.Y - (buryStrikeSize.X / 2), buryStrikeSize.Y, buryStrikeSize.X);
                        break;
                    case Side.Top:
                        buryStrikeLocation.Y = bossArena.Top;
                        buryStrikeLocation.X = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.X;
                        attackCollider = new Rectangle((int)buryStrikeLocation.X - (buryStrikeSize.X / 2), (int)buryStrikeLocation.Y, buryStrikeSize.X, buryStrikeSize.Y);
                        break;
                }
                state[4] = 5;
                buryStrikeWaitTimer = 0;
                break;
            case 5: // prepare strike, play rumble animation or something
                buryStrikeWaitTimer += Irbis.Irbis.DeltaTime;
                buryRumbleFrameTimer += Irbis.Irbis.DeltaTime;
                if (buryRumbleFrameTimer >= animationSpeed[39])
                {
                    buryRumbleFrame++;
                    buryRumbleFrameTimer -= animationSpeed[39];
                    if (buryRumbleFrame > animationFrames[39])
                    { buryRumbleFrame = 0; }
                }
                if (buryStrikeWaitTimer >= buryStrikeChargeTime)
                {
                    buryStrikeFrameTimer = buryStrikeFrame = 0;
                    state[4] = 6;
                }
                break;
            case 6: // actually strike, determine whether or not to emerge
                if (attackCollider.Intersects(Irbis.Irbis.jamie.Collider))
                {
                    OnTouch(attackCollider, buryKnockback);
                    Irbis.Irbis.jamie.Hurt(buryStrikeDamage, true, Direction.Left);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                if (buryStrikeFrame > animationFrames[38])
                {
                    if (Irbis.Irbis.RandomFloat < buryEmergeChance)
                    {
                        position.X = (Irbis.Irbis.jamie.Collider.Center.X - (standardCollider.Width / 2)) - standardCollider.X;
                        position.Y = bossArena.Bottom - standardCollider.Height - standardCollider.Y;
                        state[4] = 7;
                        activity = LizardActivity.Emerge;
                    }
                    else
                    {
                        state[4] = 2;
                        buryEmergeChance *= 2;
                    }
                }
                break;
            case 7: // emerge?
                //velocity.Y = Irbis.Irbis.Lerp(velocity.Y, -buryDigSpeed, buryDigLerp * Irbis.Irbis.DeltaTime);
                if (currentAnimation == 0 || currentAnimation == 1)
                { state[4] = 8; }
                break;
            default: // wanderpause
                Wander(1);
                activity = LizardActivity.Idle;
                state[4] = 0;
                cooldown[4] = buryCooldownTime;
                break;
        }
    }

    public void TriggerBuryAttack()
    {
        Irbis.Irbis.WriteLine(name + " bury");
        collided = new Collided();
        walled = Wall.Zero; //clear collision to allow boss to pass through floor
        buryEmergeChance = buryInitialEmergeChance;
        state[4] = 1;
        activity = LizardActivity.Bury;
    }

    public void CalculateMovement()
    {
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        //collider = new Rectangle((int)position.X + XstandardCollider.Location, (int)position.Y + YstandardCollider.Location, standardCollider.Width, standardCollider.Height);
        trueCollider.X = (int)Math.Round((decimal)position.X) + standardCollider.X;
        trueCollider.Y = (int)Math.Round((decimal)position.Y) + standardCollider.Y;
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
                    case 6:
                        goto case 7;
                    case 7:
                        SetAnimation(8, false);
                        break;
                    case 12:
                        goto case 13;
                    case 13:
                        SetAnimation(6, true);
                        break;
                    case 14:
                        goto case 15;
                    case 15:
                        SetAnimation(10, false);
                        break;
                    case 16:
                        goto case 17;
                    case 17:
                        SetAnimation(0, false);
                        activity = LizardActivity.Misc;
                        break;
                    case 20:
                        goto case 21;
                    case 21:
                        SetAnimation(0, false);
                        break;
                    case 24:
                        goto case 25;
                    case 25:
                        SetAnimation(0, false);
                        break;
                    case 26:
                        goto case 27;
                    case 27:
                        SetAnimation(0, false);
                        activity = LizardActivity.Misc;
                        break;
                    case 28:
                        goto case 29;
                    case 29:
                        SetAnimation(30, false);
                        break;
                    case 30:
                        goto case 31;
                    case 31:
                        SetAnimation(0, false);
                        break;
                    default:
                        SetAnimation();
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

        previousAnimation = currentAnimation;
        previousActivity = activity;
    }

    public void SetAnimation()
    {
        switch (activity)
        {
            case LizardActivity.Idle:
                SetAnimation(0, false);
                break;
            case LizardActivity.WalkLeft:
                SetAnimation(2, false);
                break;
            case LizardActivity.WalkRight:
                SetAnimation(4, false);
                break;
            case LizardActivity.Roll:
                SetAnimation(12, true);
                break;
            case LizardActivity.Bury:
                SetAnimation(28, true);
                break;
            case LizardActivity.Emerge:
                SetAnimation(30, true);
                break;
            case LizardActivity.Swipe:
                SetAnimation(18, false);
                break;
            case LizardActivity.Tailwhip:
                SetAnimation(22, false);
                break;
            case LizardActivity.SwipeSwing:
                SetAnimation(20, true);
                break;
            case LizardActivity.TailwhipSwing:
                SetAnimation(24, true);
                break;
            case LizardActivity.TurningAround:
                SetAnimation(16, true);
                break;
            case LizardActivity.Dying:
                Dying();
                break;
            case LizardActivity.GettingUp:
                GetUp();
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

    public void GetUp()
    {
        activity = LizardActivity.GettingUp;
        previousActivity = activity;
        SetAnimation(26, true);
        standardCollider.Location = new Point(44, 26);
        trueCollider.Size = collider.Size = new Point(60, 100);
    }

    public void Dying()
    {
        activity = LizardActivity.Dying;
        previousActivity = activity;
        trueCollider.Size = collider.Size = new Point(60, 32);
        standardCollider.Location = new Point(44, 93);
        SetAnimation(14, true);
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = standardCollider.Width;
        testCollider.Height = standardCollider.Height;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && s.Collider != trueCollider && Irbis.Irbis.DistanceSquared(trueCollider, s.Collider) <= 0 && s.GetType() != typeof(Player))
            {
                collidedContains = collided.Contains(s);
                if (Irbis.Irbis.IsTouching(trueCollider, s.Collider, Side.Bottom))                              //DOWN
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Bottom);
                        walled.Bottom++;
                        if (negAmountToMove.Y > s.Collider.Top - trueCollider.Bottom && (velocity.Y * Irbis.Irbis.DeltaTime) >= -(s.Collider.Top - trueCollider.Bottom))
                        {
                            negAmountToMove.Y = s.Collider.Top - trueCollider.Bottom;
                        }
                    }
                    else if (negAmountToMove.Y > s.Collider.Top - trueCollider.Bottom)
                    {
                        negAmountToMove.Y = s.Collider.Top - trueCollider.Bottom;
                    }
                }
                if (Irbis.Irbis.IsTouching(trueCollider, s.Collider, Side.Right))                               //RIGHT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Right);
                        walled.Right++;
                        if (negAmountToMove.X > s.Collider.Left - trueCollider.Right && (velocity.X * Irbis.Irbis.DeltaTime) >= -(s.Collider.Left - trueCollider.Right))
                        {
                            negAmountToMove.X = s.Collider.Left - trueCollider.Right;
                        }
                    }
                    else if (negAmountToMove.X > s.Collider.Left - trueCollider.Right)
                    {
                        negAmountToMove.X = s.Collider.Left - trueCollider.Right;
                    }
                }
                if (Irbis.Irbis.IsTouching(trueCollider, s.Collider, Side.Left))                                //LEFT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Left);
                        walled.Left++;
                        if (amountToMove.X < s.Collider.Right - trueCollider.Left && (velocity.X * Irbis.Irbis.DeltaTime) <= -(s.Collider.Right - trueCollider.Left))
                        {
                            amountToMove.X = s.Collider.Right - trueCollider.Left;
                        }
                    }
                    else if (amountToMove.X < s.Collider.Right - trueCollider.Left)
                    {
                        amountToMove.X = s.Collider.Right - trueCollider.Left;
                    }
                }
                if (Irbis.Irbis.IsTouching(trueCollider, s.Collider, Side.Top))                                 //UP
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Top);
                        walled.Top++;
                        if (amountToMove.Y < s.Collider.Bottom - trueCollider.Top && (velocity.Y * Irbis.Irbis.DeltaTime) <= -(s.Collider.Bottom - trueCollider.Top))
                        {
                            amountToMove.Y = s.Collider.Bottom - trueCollider.Top;
                        }
                    }
                    else if (amountToMove.Y < s.Collider.Bottom - trueCollider.Top)
                    {
                        amountToMove.Y = s.Collider.Bottom - trueCollider.Top;
                    }
                }
            }
        }

        if (walled.Left == 1 && input.X < 0)
        {
            int climbamount = (trueCollider.Bottom - collided.leftCollided[0].Collider.Top);
            if ((climbamount) <= climbablePixels)
            {
                position.Y -= climbamount;
                //position.X -= 1;
                amountToMove = negAmountToMove = Vector2.Zero;
                Irbis.Irbis.WriteLine(this + " on ramp, moved " + climbamount + " pixels. Timer:" + Irbis.Irbis.Timer);
            }
        }
        if (walled.Right == 1 && input.X > 0)
        {
            int climbamount = (trueCollider.Bottom - collided.rightCollided[0].Collider.Top);
            if ((climbamount) <= climbablePixels)
            {
                position.Y -= climbamount;
                //position.X += 1;
                amountToMove = negAmountToMove = Vector2.Zero;
                Irbis.Irbis.WriteLine(this + " on ramp, moved " + climbamount + " pixels. Timer:" + Irbis.Irbis.Timer);
            }
        }


        if (amountToMove.X == 0)
        {
            amountToMove.X = negAmountToMove.X;
        }
        else if (negAmountToMove.X != 0 && -negAmountToMove.X < amountToMove.X)
        {
            amountToMove.X = negAmountToMove.X;
        }

        if (amountToMove.Y == 0)
        {
            amountToMove.Y = negAmountToMove.Y;
        }
        else if (negAmountToMove.Y != 0 && -negAmountToMove.Y < amountToMove.Y)
        {
            amountToMove.Y = negAmountToMove.Y;
        }

        bool Y = false;
        bool X = false;
        if (Math.Abs(amountToMove.Y) <= Math.Abs(amountToMove.X) && amountToMove.Y != 0)
        {
            testPos.Y = (int)Math.Round((double)position.Y);
            testPos.X = position.X;
            testPos.Y += amountToMove.Y;
            Y = true;
        }
        else if (amountToMove.X != 0)
        {
            testPos.X = (int)Math.Round((double)position.X);
            testPos.Y = position.Y;
            testPos.X += amountToMove.X;
            X = true;
        }

        bool pass = true;
        testCollider.X = (int)testPos.X + standardCollider.X;
        testCollider.Y = (int)testPos.Y + standardCollider.Y;

        pass = !(collided.Intersects(testCollider));

        if (pass)
        {
            if (Y)
            {
                amountToMove.X = 0;
            }
            else if (X)
            {
                amountToMove.Y = 0;
            }
        }
        else
        {
            pass = true;
            if (Y)
            {
                testPos.X = (int)Math.Round((double)position.X);
                testPos.Y = position.Y;
                testPos.X += amountToMove.X;
                testCollider.X = (int)testPos.X + standardCollider.X;
                testCollider.Y = (int)testPos.Y + standardCollider.Y;

                pass = !(collided.Intersects(testCollider));

                if (pass)
                {
                    amountToMove.Y = 0;
                }
            }
            else if (X)
            {
                testPos.Y = (int)Math.Round((double)position.Y);
                testPos.X = position.X;
                testPos.Y += amountToMove.Y;
                testCollider.X = (int)testPos.X + standardCollider.X;
                testCollider.Y = (int)testPos.Y + standardCollider.Y;

                pass = !(collided.Intersects(testCollider));

                if (pass)
                {
                    amountToMove.X = 0;
                }
            }
        }

        position += amountToMove;
        CalculateMovement();

        for (int i = collided.bottomCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.bottomCollided[i].Collider, Side.Bottom))
            {
                collided.bottomCollided.RemoveAt(i);
                walled.Bottom--;
            }
        }
        for (int i = collided.rightCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.rightCollided[i].Collider, Side.Right))
            {
                collided.rightCollided.RemoveAt(i);
                walled.Right--;
            }
        }
        for (int i = collided.leftCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.leftCollided[i].Collider, Side.Left))
            {
                collided.leftCollided.RemoveAt(i);
                walled.Left--;
            }
        }
        for (int i = collided.topCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.topCollided[i].Collider, Side.Top))
            {
                collided.topCollided.RemoveAt(i);
                walled.Top--;
            }
        }

        if ((walled.Top > 0 && velocity.Y < 0) || (walled.Bottom > 0 && velocity.Y > 0))
        {
            velocity.Y = 0;
            position.Y = (int)Math.Round((double)position.Y);
        }
        if ((walled.Left > 0 && velocity.X < 0) || (walled.Right > 0 && velocity.X > 0))
        {
            velocity.X = 0;
            position.X = (int)Math.Round((double)position.X);
        }

        if (walled.Bottom <= 0 && jumpTime <= 0)
        { velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime; }
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

    public void Stun(float duration)
    {
        if (stunned < duration)
        { stunned = duration; }
    }

    public override string ToString()
    { return name + " bossState:" + bossState + "(" + (int)bossState + ") position:" + position + " velocity:" + velocity + " health:" + health; }

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
                { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, true); }
                goto case 2;
            case 2:
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, ((position + (standardCollider.Location - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto case 1;
            case 1:
                goto default;
            default:
                if (bossState > 0)
                {
                    if (state[4] == 5)
                    {
                        switch (buryStrikeSide)
                        {
                            case Side.Bottom:
                                sb.Draw(tex, (buryStrikeLocation - new Vector2(64, 128)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryRumbleFrame * 128, 4992), new Point(128)), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Left:
                                sb.Draw(tex, (buryStrikeLocation - new Vector2(-128, 64)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryRumbleFrame * 128, 4992), new Point(128)), Color.White, (float)(Math.PI / 2), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Right:
                                sb.Draw(tex, (buryStrikeLocation + new Vector2(-128, 64)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryRumbleFrame * 128, 4992), new Point(128)), Color.White, (float)((3 * Math.PI) / 2), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Top:
                                sb.Draw(tex, (buryStrikeLocation + new Vector2(64, 128)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryRumbleFrame * 128, 4992), new Point(128)), Color.White, (float)(Math.PI), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                        }
                    }
                    else if (buryStrikeFrame <= animationFrames[38])
                    {
                        buryStrikeFrameTimer += Irbis.Irbis.DeltaTime;
                        if (buryStrikeFrameTimer >= animationSpeed[38])
                        {
                            buryStrikeFrame++;
                            buryStrikeFrameTimer -= animationSpeed[38];
                        }
                        switch (buryStrikeSide)
                        {
                            case Side.Bottom:
                                sb.Draw(tex, (buryStrikeLocation - new Vector2(64, 128)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryStrikeFrame * 128, 4864), new Point(128)), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Left:
                                sb.Draw(tex, (buryStrikeLocation - new Vector2(-128, 64)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryStrikeFrame * 128, 4864), new Point(128)), Color.White, (float)(Math.PI / 2), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Right:
                                sb.Draw(tex, (buryStrikeLocation + new Vector2(-128, 64)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryStrikeFrame * 128, 4864), new Point(128)), Color.White, (float)((3 * Math.PI) / 2), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                            case Side.Top:
                                sb.Draw(tex, (buryStrikeLocation + new Vector2(64, 128)) * Irbis.Irbis.screenScale, new Rectangle(new Point(buryStrikeFrame * 128, 4864), new Point(128)), Color.White, (float)(Math.PI), Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                                break;
                        }
                    }

                    sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                }
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    { }
}
