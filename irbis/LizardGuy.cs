using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Irbis
{
    public enum LizardManAI
    {
          Wander = 0,
            Roll = 1,
           Swipe = 2,
        Tailwhip = 3,
            Bury = 4,
    }

}
class LizardGuy : IEnemy
{
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
    private Rectangle trueCollider;

    public Vector2 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }
    private Vector2 position;

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
        {
            return aiEnabled;
        }
        set
        {
            aiEnabled = value;
        }
    }
    public bool aiEnabled = true;

    public float Mass
    {
        get
        {
            return mass;
        }
    }
    private float mass = 1.5f;

    public float ShockwaveMaxEffectDistanceSquared
    {
        get
        {
            return shockwaveMaxEffectDistanceSquared;
        }
    }
    private float shockwaveMaxEffectDistanceSquared;

    public bool ActivelyAttacking
    {
        get
        {
            return state[1] > 0 || state[2] > 0 || state[3] > 0 || state[4] > 0 || state[5] > 0;
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
    public float specialTime;
    public float specialIdleTime;
    public Activity previousActivity;
    public Point prevInput;
    public int nextAnimation;
    public bool isRunning;
    public bool invulnerable;
    public bool interruptAttack;
    public bool inputEnabled;
    public float idleTimeMax;
    public float idleTime;
    public float debugspeed;
    public float attackMovementSpeed;
    public int attackMovementFrames;
    public bool attackImmediately;
    public Print animationFrame;
    public float airSpeed;

    Texture2D tex;
    public Rectangle displayRect;
    public Rectangle animationSourceRect;
    Rectangle testCollider;
    public Point colliderOffset;
    public int colliderWidth;
    public int colliderHeight;
    public float terminalVelocity = 5000f;

    float depth;

    //roll variables
    public float rollCooldownTime = 8;
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
    public float swipeChargeTime = 1f;
    public float swipeCooldownTime = 3.5f;
    public float swipePauseTime = 0.75f;
    public float swipePauseTimer;
    public Vector2 swipeKnockback = new Vector2(100, -250);
    public float swipeDamage = 25f;
    public int swipeRange = 80;

    //tailwhip variables
    public float tailwhipChargeTime = 1f;
    public float tailwhipCooldownTime = 3.5f;
    public float tailwhipPauseTime = 0.75f;
    public Vector2 tailwhipKnockback = new Vector2(700, -500);
    public float tailwhipDamage = 35f;

    //bury variables
    public float buryDigSpeed = 250f;
    public float buryDigLerp = 5f;
    public float buryChargeTime = 0;
    public float buryCooldownTime = 30f;
    public float buryStrikeWaitTime;
    public float buryStrikeMaxWaitTime = 5;
    public float buryStrikeMinWaitTime = 2;
    public float buryStrikeWaitTimer;
    public   int buryStrikeRadius = 100;
    public Point buryStrikelocation;
    public  Side buryStrikeSide;
    public float buryStrikeChargeTime = 1.5f;
    public float buryStrikeCooldownTime = 5f;
    public float buryInitialEmergeChance = 0.1f;
    public float buryEmergeChance = 0.1f;
    public float buryStrikeDamage = 35f;
    public Vector2 buryKnockback = new Vector2(100, -250);

    public float stunned;
    float wanderSpeed;
    bool previouslyWandered;
    public float wanderTime;
    public float initialWanderTime;
    public float jumpTime;
    float jumpTimeMax;
    float timeSinceLastFrame;
    int currentFrame;
    int currentAnimation;
    int previousAnimation;
    float[] animationSpeed = new float[20];
    int[] animationFrames = new int[20];
    bool animationNoLoop;
    public Point input;
    public bool frameInput;

    public float shockwaveMaxEffectDistance;
    public float shockwaveEffectiveDistance;
    public float shockwaveStunTime;

    public Vector2 shockwaveKnockback;

    public Attacking attacking;
    public Attacking prevAttacking;

    public Direction direction;
    public Location location;
    public Activity activity;
    public AI AIactivity;

    Vector2 amountToMove;
    Vector2 negAmountToMove;
    Vector2 testPos;

    public int lastHitByAttackID;

    public int combatCheckDistanceSqr;
    public int persueCheckDistanceSqr;
    public bool combat;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    public Rectangle attackCollider;
    float attackColliderTimer;

    public int attackColliderWidth = 100;
    public int attackColliderHeight = 70;

    public int attackID;
    public int lastAttackID;
    int attackIDtracker;

    public float attackDamage;
    public float attack1Damage;

    public float attackCooldown;
    public float attackCooldownTimer;

    public float freezeTimer;

    private object attackPlayerLock;
    private object collidedLock;

    public Rectangle bossArena;

    public Collided collided;

    public int[] state = new int[6];
    public float[] cooldown = new float[6];
    


    public LizardGuy(Texture2D t, Vector2 iPos, float enemyHealth, float enemyDamage, float enemySpeed, Rectangle? BossArena, float drawDepth)
    {
        cooldown[4] = 3f; //30 seconds

        collided = new Collided();

        rollTimeMax = 0.25f;
        rollSpeed = 1500f;
        rollTime = 0f;
        jumpTime = 0;
        idleTime = 0f;
        airSpeed = 0.6f * enemySpeed;
        attackMovementSpeed = 0.3f * enemySpeed;
        attackImmediately = false;
        interruptAttack = false;
        climbablePixels = 3;

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.001f);

        tex = t;
        attackPlayerLock = new object();
        collidedLock = new object();
        AIenabled = true;

        depth = drawDepth;

        climbablePixels = 3;

        position = iPos;
        direction = Direction.Forward;
        location = Location.Air;
        activity = Activity.Idle;
        AIactivity = AI.Wander;
        wanderSpeed = (1f / 5f) * enemySpeed;
        wanderTime = 1f;
        initialWanderTime = 0f;
        previouslyWandered = false;

        speed = defaultSpeed = enemySpeed;
        jumpTime = 0;
        jumpTimeMax = 0.06f;
        animationNoLoop = false;
        colliderOffset.X = 14;
        colliderOffset.Y = 28;
        colliderWidth = 100;
        colliderHeight = 100;

        position.X -= colliderOffset.X;
        position.Y -= colliderOffset.Y;

        maxHealth = health = enemyHealth;
        lastHitByAttackID = -1;

        stunned = 0;
        speedModifier = 1f;

        attackCollider = Rectangle.Empty;

        attackID = attackIDtracker = 0;
        lastAttackID = -1;

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
        shockwaveStunTime = 0.25f;
        shockwaveKnockback = new Vector2(2f, 1f);

        displayRect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;
        animationSpeed[0] = 0.1f;
        for (int i = 1; i < 20; i++)
        {
            animationSpeed[i] = animationSpeed[0];
        }

        animationFrames[0] = 2;             //idleforward1
        animationFrames[1] = 4;             //idleforward2
        animationFrames[2] = 2;             //idleforward3
        animationFrames[3] = 2;             //idleleft
        animationFrames[4] = 2;             //idleright
        animationFrames[5] = 2;             //runleft
        animationFrames[6] = 2;             //runright
        animationFrames[7] = 2;             //jumpleft
        animationFrames[8] = 2;             //jumpright
        //animationFrames[9] = 2;             //fallleft
        //animationFrames[10] = 2;            //fallright

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

        if ((bossArena.Height - colliderHeight) > 350)
        {
            rollLeapInitialYvelocity = -(float)Math.Sqrt(2 * Irbis.Irbis.gravity * 350);
        }
        else
        {
            rollLeapInitialYvelocity = -(float)Math.Sqrt(2 * Irbis.Irbis.gravity * (bossArena.Height - colliderHeight));
        }
        rollLeapTime = -rollLeapInitialYvelocity / Irbis.Irbis.gravity;
        Irbis.Irbis.WriteLine("rollLeapInitialYvelocity:" + rollLeapInitialYvelocity + " rollLeapTime:" + rollLeapTime);

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
    }

    public bool Update()
    {
        prevInput = input;
        prevAttacking = attacking;
        input = Point.Zero;
        frameInput = false;

        if (Irbis.Irbis.jamie != null)
        {
            if (aiEnabled && !ActivelyAttacking && cooldown[0] <= 0 && stunned <= 0 && walled.Bottom > 0)
            {
                Direction playerDirection = Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider);
                if (playerDirection != direction && playerDirection != Direction.Forward)
                { // turn around
                    Irbis.Irbis.WriteLine(name + " not facing player. turning around...");
                    direction = playerDirection;
                    cooldown[3] = 0.25f;
                    //play turning around animation
                }
                else if (cooldown[2] <= 0 && Irbis.Irbis.XDistance(trueCollider, Irbis.Irbis.jamie.Collider) <= swipeRange)
                { // swipe/tailwhip
                    if (Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider) == direction)
                    {
                        // use rng to determine if it's a swipe or tailwhip
                        if (Irbis.Irbis.RandomInt(2) > 0)
                        {
                            //Irbis.Irbis.WriteLine(name + " swipe");
                            cooldown[2] = swipeCooldownTime;
                            if (direction == Direction.Left)
                            { state[2] = 1; }
                            else
                            { state[2] = 2; }
                        }
                        else
                        {
                            //Irbis.Irbis.WriteLine(name + " tailwhip");
                            cooldown[2] = tailwhipCooldownTime;
                            if (direction == Direction.Left)
                            { state[3] = 1; }
                            else
                            { state[3] = 2; }
                        }
                    }
                    else
                    {
                        Irbis.Irbis.WriteLine(name + " swipe/tailwhip failed, facing wrong direction. turning around... (ERROR)");
                        cooldown[2] = 0.5f;
                        if (direction == Direction.Right)
                        { direction = Direction.Left; }
                        else if (direction == Direction.Left)
                        { direction = Direction.Right; }
                    }
                }
                else if (Irbis.Irbis.jamie.Collider.Center.X < bossArena.Center.X && trueCollider.Center.X < bossArena.Center.X && Irbis.Irbis.jamie.stunTime <= 0.5f && cooldown[1] <= 0)
                { // roll left
                    //player is on left side of screen
                    //leap to right side of screen, roll to the left
                    //Irbis.Irbis.WriteLine(name + " roll attack: roll left");
                    cooldown[1] = rollCooldownTime;
                    state[1] = 1;   //state = 1 to roll left
                }
                else if (Irbis.Irbis.jamie.Collider.Center.X > bossArena.Center.X && trueCollider.Center.X > bossArena.Center.X && Irbis.Irbis.jamie.stunTime <= 0.5f && cooldown[1] <= 0)
                { // roll right
                    //player is on right side of screen
                    //leap to left side of screen, roll to the right
                    //Irbis.Irbis.WriteLine(name + " roll attack: roll right");
                    cooldown[1] = rollCooldownTime;
                    state[1] = 2;   //state = 2 to roll right
                }
                else if (cooldown[4] <= 0 && Irbis.Irbis.XDistance(trueCollider, Irbis.Irbis.jamie.Collider) > swipeRange)
                { // bury
                    //Irbis.Irbis.WriteLine(name + " bury");
                    collided = new Collided();
                    walled = Wall.Zero; //clear collision to allow boss to pass through floor
                    cooldown[4] = buryCooldownTime;
                    buryEmergeChance = buryInitialEmergeChance;
                    state[4] = 1;
                }
                else
                { // wander around, maybe yell a bit, just don't look like a doofus
                    if (Irbis.Irbis.RandomBool)
                    { speed = wanderSpeed; }
                    else
                    { speed = -wanderSpeed; }
                    initialWanderTime = cooldown[0] = (wanderTime * (Irbis.Irbis.RandomInt(2) + 1));
                }
            }

            //do the thing
            if (cooldown[1] > 0)
            { cooldown[1] -= Irbis.Irbis.DeltaTime; }
            if (cooldown[2] > 0)
            { cooldown[2] -= Irbis.Irbis.DeltaTime; }
            if (cooldown[3] > 0)
            { cooldown[3] -= Irbis.Irbis.DeltaTime; }
            if (cooldown[4] > 0)
            { cooldown[4] -= Irbis.Irbis.DeltaTime; }

            if (cooldown[0] > 0)
            {
                if (walled.Bottom > 0)
                {
                    Wander();
                    cooldown[0] -= Irbis.Irbis.DeltaTime;
                }
            }
            if (stunned > 0)
            { stunned -= Irbis.Irbis.DeltaTime; }

        }
        else
        { cooldown[0] = 1; }

        //if (attackCollider != Rectangle.Empty)
        //{
        //    attackColliderTimer += Irbis.Irbis.DeltaTime;
        //    if (attackColliderTimer >= 10)
        //    {
        //        attackColliderTimer = 0;
        //        attackCollider = Rectangle.Empty;
        //    }
        //}

        if (Irbis.Irbis.IsTouching(trueCollider, Irbis.Irbis.jamie.Collider))
        {
            if (stunned <= 0)
            {
                collider = Rectangle.Empty;
                if (state[1] > 0 && Irbis.Irbis.jamie.HurtOnTouch(rollDamage))
                {
                    OnTouch(trueCollider, rollKnockback);
                }
                else if (Irbis.Irbis.jamie.HurtOnTouch(10f))
                {
                    OnTouch(trueCollider, swipeKnockback);
                }
            }
        }
        else if (collider == Rectangle.Empty && Irbis.Irbis.jamie.invulnerableOnTouch <= 0)
        { collider = trueCollider; }


        Movement();
        CalculateMovement();
        Animate();
        if (state[4] <= 0)
        { Collision(Irbis.Irbis.collisionObjects); }
        return true;
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        {
            Update();
            if (health <= 0 || position.Y > 5000f)
            {
                Irbis.Irbis.KillEnemy(this);
            }
        }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            {
                Irbis.Irbis.doneEvent.Set();
            }
        }
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking attack)
    {
        Irbis.Irbis.WriteLine(name + " response:\nattackCollider:" + AttackCollider + " " + name + " collider:" + collider + " attack:" + attack);
        if (AttackCollider.Intersects(trueCollider))
        {
            PlayerAttackCollision();
            Irbis.Irbis.WriteLine("hit. health remaining:" + health);
        }
        Irbis.Irbis.WriteLine(name + " done.\n");
        return true;
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

    public void PlayerAttackCollision()
    {
        //lastHitByAttackID = Irbis.Irbis.jamie.attackID;
        //Irbis.Irbis.jamie.attackHit = true;
        Hurt(Irbis.Irbis.jamie.attackDamage);
        //Stun(0.75f);

        //float distanceSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider.Center, Collider.Center);
        //if (Irbis.Irbis.jamie.direction == Direction.Left)
        //{
        //    velocity.Y = -25f;
        //    velocity.X = -0f;
        //    if (distanceSqr < 1000)
        //    { velocity.X -= 150; }
        //    else
        //    { velocity.X -= (22500 / distanceSqr); }
        //}
        //else
        //{
        //    velocity.Y = -25f;
        //    velocity.X = 0f;
        //    if (distanceSqr < 1000)
        //    { velocity.X += 150; }
        //    else
        //    { velocity.X += (22500 / distanceSqr); }
        //}

        foreach (Enchant enchant in Irbis.Irbis.jamie.enchantList)
        { enchant.AddEffect(this); }
    }

    public void Movement()
    {
        if (state[1] > 0)
        { RollAttack(); }
        if (state[2] > 0)
        { SwipeAttack(); }
        if (state[3] > 0)
        { TailwhipAttack(); }
        if (state[4] > 0)
        { BuryAttack(); }
        if (state[5] > 0)
        {
            //nothing
        }
        
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
        { velocity.X = Irbis.Irbis.Lerp(velocity.X, speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime); }
        else
        { velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpSlowdown * Irbis.Irbis.DeltaTime); }
    }

    public bool Wander(float WanderTime)
    {
        if (Irbis.Irbis.RandomBool)
        { speed = wanderSpeed; }
        else
        { speed = -wanderSpeed; }
        initialWanderTime = cooldown[0] = (WanderTime);
        return true;
    }

    public void RollAttack()
    {
        //Roll == state[1] && cooldown[1]
        switch (state[1])
        {
            case 1: //leap right
                velocity = new Vector2((Irbis.Irbis.UnidirectionalDistance(trueCollider.Right, bossArena.Right) / rollLeapTime), rollLeapInitialYvelocity);
                Irbis.Irbis.WriteLine("leap right velocity:" + velocity);
                position.Y -= 1;
                state[1] = 3;
                Irbis.Irbis.CameraShake(0.1f, 3f);
                break;
            case 2: //leap left
                velocity = new Vector2(-(Irbis.Irbis.UnidirectionalDistance(trueCollider.Left, bossArena.Left) / rollLeapTime), rollLeapInitialYvelocity);
                Irbis.Irbis.WriteLine("leap left velocity:" + velocity);
                position.Y -= 1;
                state[1] = 4;
                Irbis.Irbis.CameraShake(0.1f, 3f);
                break;
            case 3: //in the air, waiting to hit the ground
                if (walled.Bottom > 0)
                {
                    state[1] += 2;
                    rollPauseTimer = 0f;
                    Irbis.Irbis.WriteLine(name + " has hit the ground, now pausing before rolling");
                    Irbis.Irbis.CameraShake(5f, 1.5f);
                }
                break;
            case 4: //in the air, waiting to hit the ground
                if (walled.Bottom > 0)
                {
                    state[1] += 2;
                    rollPauseTimer = 0f;
                    Irbis.Irbis.WriteLine(name + " has hit the ground, now pausing before rolling");
                    Irbis.Irbis.CameraShake(5f, 1.5f);
                }
                break;
            case 5: //hit the ground, waiting to roll
                rollPauseTimer += Irbis.Irbis.DeltaTime;
                if (rollPauseTimer >= rollPauseTime)
                {
                    state[1] += 2;
                    Irbis.Irbis.WriteLine(name + " done pausing, now rolling left");
                }
                break;
            case 6: //hit the ground, waiting to roll
                rollPauseTimer += Irbis.Irbis.DeltaTime;
                if (rollPauseTimer >= rollPauseTime)
                {
                    state[1] += 2;
                    Irbis.Irbis.WriteLine(name + " done pausing, now rolling right");
                }
                break;
            case 7: //roll left
                if (walled.Left <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, -rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                {
                    state[1] = 9;
                    Irbis.Irbis.WriteLine(name + " hit wall, stunned");
                    Irbis.Irbis.CameraShake(0.15f, 15f);
                    Stun(cooldown[1]);
                }
                break;
            case 8: //roll right
                if (walled.Right <= 0)
                { velocity.X = Irbis.Irbis.Lerp(velocity.X, rollSpeed, rollLerp * Irbis.Irbis.DeltaTime); }
                else
                {
                    state[1] = 9;
                    Irbis.Irbis.WriteLine(name + " hit wall, stunned");
                    Irbis.Irbis.CameraShake(0.15f, 15f);
                    Stun(cooldown[1]);
                }
                break;
            case 9: //stunned
                if (cooldown[1] <= 0)
                {
                    state[1] = 10;
                    Irbis.Irbis.WriteLine(name + " recovered");
                }
                break;
            case 10:
                Wander(2);
                state[1] = 0;
                break;
        }
    }

    public void SwipeAttack()
    {
        //swipe == state[2] && cooldown[2]
        //swipe and tailwhip share a cooldown
        switch (state[2])
        {
            case 1: //charge/ready to swipe left
                if (cooldown[2] <= (swipeCooldownTime - swipeChargeTime))
                { state[2] += 2; }
                break;
            case 2: //charge/ready to swipe right
                if (cooldown[2] <= (swipeCooldownTime - swipeChargeTime))
                { state[2] += 2; }
                break;
            case 3:
                Irbis.Irbis.WriteLine("swipe left");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-swipeKnockback.X, swipeKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(swipeDamage);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                    state[2] += 2;
                    Irbis.Irbis.WriteLine("pausing for " + swipePauseTime + " seconds");
                }
                else
                { state[2] = 0; }
                break;
            case 4:
                Irbis.Irbis.WriteLine("swipe right");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = swipeKnockback;
                    Irbis.Irbis.jamie.Hurt(swipeDamage);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                    state[2] += 2;
                    Irbis.Irbis.WriteLine("pausing for " + swipePauseTime + " seconds");
                }
                else
                { state[2] = 0; }
                break;
            case 5:
                swipePauseTimer += Irbis.Irbis.DeltaTime;
                if (swipePauseTimer >= swipePauseTime)
                {
                    state[2] += 2;
                    swipePauseTimer = 0;
                }
                break;
            case 6:
                swipePauseTimer += Irbis.Irbis.DeltaTime;
                if (swipePauseTimer >= swipePauseTime)
                {
                    state[2] += 2;
                    swipePauseTimer = 0;
                }
                break;
            case 7:
                Irbis.Irbis.WriteLine("swipe left again");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-swipeKnockback.X, swipeKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(swipeDamage);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                state[2] = 9;
                break;
            case 8:
                Irbis.Irbis.WriteLine("swipe right again");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = swipeKnockback;
                    Irbis.Irbis.jamie.Hurt(swipeDamage);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                state[2] = 9;
                break;
            case 9:
                Wander(1);
                state[2] = 0;
                break;
        }
    }

    public void TailwhipAttack()
    {
        //tailwhip == state[3] && cooldown[2]
        //swipe and tailwhip share a cooldown
        switch (state[3])
        {
            case 1: //charge/ready to tailwhip left
                if (cooldown[2] <= (tailwhipCooldownTime - tailwhipChargeTime))
                {
                    state[3] += 2;
                }
                break;
            case 2: //charge/ready to tailwhip right
                if (cooldown[2] <= (tailwhipCooldownTime - tailwhipChargeTime))
                {
                    state[3] += 2;
                }
                break;
            case 3:
                Irbis.Irbis.WriteLine("tailwhip left");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X - attackColliderWidth, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = new Vector2(-tailwhipKnockback.X, tailwhipKnockback.Y);
                    Irbis.Irbis.jamie.Hurt(tailwhipDamage);
                    Irbis.Irbis.CameraShake(0.1f, 10f);
                }
                state[3] = 5;
                break;
            case 4:
                Irbis.Irbis.WriteLine("tailwhip right");
                if (Irbis.Irbis.jamie.Collider.Intersects(new Rectangle(collider.Center.X, collider.Center.Y - attackColliderHeight / 2, attackColliderWidth, attackColliderHeight)))
                {
                    Irbis.Irbis.jamie.velocity = tailwhipKnockback;
                    Irbis.Irbis.jamie.Hurt(tailwhipDamage);
                    Irbis.Irbis.CameraShake(0.1f, 10f);
                }
                state[3] = 5;
                break;
            case 5:
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
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, buryDigSpeed, buryDigLerp * Irbis.Irbis.DeltaTime);
                if (trueCollider.Top >= bossArena.Bottom)
                {
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
                //determine which side to strike from
                buryStrikeSide = Irbis.Irbis.SideClosest(bossArena, Irbis.Irbis.jamie.Collider);
                //buryStrikelocation
                switch (buryStrikeSide)
                {
                    case Side.Bottom:
                        buryStrikelocation.Y = bossArena.Bottom;
                        buryStrikelocation.X = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.X;
                        attackCollider = new Rectangle(buryStrikelocation.X - (attackColliderWidth / 2), buryStrikelocation.Y - attackColliderWidth, attackColliderWidth, attackColliderWidth);
                        break;
                    case Side.Left:
                        buryStrikelocation.X = bossArena.Left;
                        buryStrikelocation.Y = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.Y;
                        attackCollider = new Rectangle(buryStrikelocation.X, buryStrikelocation.Y - (attackColliderWidth / 2), attackColliderWidth, attackColliderWidth);
                        break;
                    case Side.Right:
                        buryStrikelocation.X = bossArena.Right;
                        buryStrikelocation.Y = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.Y;
                        attackCollider = new Rectangle(buryStrikelocation.X - attackColliderWidth, buryStrikelocation.Y - (attackColliderWidth / 2), attackColliderWidth, attackColliderWidth);
                        break;
                    case Side.Top:
                        buryStrikelocation.Y = bossArena.Top;
                        buryStrikelocation.X = (int)((Irbis.Irbis.RandomFloat * buryStrikeRadius * 2) - buryStrikeRadius) + Irbis.Irbis.jamie.Collider.Center.X;
                        attackCollider = new Rectangle(buryStrikelocation.X - (attackColliderWidth / 2), buryStrikelocation.Y, attackColliderWidth, attackColliderWidth);
                        break;
                }

                state[4] = 5;
                buryStrikeWaitTimer = 0;
                break;
            case 5: // prepare strike, play rumble animation or something
                buryStrikeWaitTimer += Irbis.Irbis.DeltaTime;
                if (buryStrikeWaitTimer >= buryStrikeChargeTime)
                { state[4] = 6; }
                break;
            case 6: // actually strike, determine whether or not to emerge
                if (attackCollider.Intersects(Irbis.Irbis.jamie.Collider))
                {
                    OnTouch(attackCollider, buryKnockback);
                    Irbis.Irbis.jamie.Hurt(buryStrikeDamage);
                    Irbis.Irbis.CameraShake(0.1f, 5f);
                }
                if (Irbis.Irbis.RandomFloat < buryEmergeChance)
                {
                    position.X = (attackCollider.Center.X - (colliderWidth / 2)) - colliderOffset.X;
                    state[4] = 7;
                }
                else
                {
                    state[4] = 2;
                    buryEmergeChance *= 2;
                }
                break;
            case 7: // emerge?
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, -buryDigSpeed, buryDigLerp * Irbis.Irbis.DeltaTime);
                if (trueCollider.Bottom <= bossArena.Bottom)
                {
                    state[4] = 8;
                }
                break;
            case 8: // wanderpause
                Wander(1);
                state[4] = 0;
                cooldown[4] = buryCooldownTime;
                break;
            case 9:
                break;
            case 10:
                break;
            case 15:
                break;
        }
    }

    public void CalculateMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("CalculateMovement"); }
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        displayRect.X = (int)position.X;
        displayRect.Y = (int)position.Y;
        //collider = new Rectangle((int)position.X + XcolliderOffset, (int)position.Y + YcolliderOffset, colliderWidth, colliderHeight);
        trueCollider.X = (int)Math.Round((decimal)position.X) + colliderOffset.X;
        trueCollider.Y = (int)Math.Round((decimal)position.Y) + colliderOffset.Y;
        trueCollider.Width = colliderWidth;
        trueCollider.Height = colliderHeight;
        if (collider != Rectangle.Empty)
        { collider = trueCollider; }
    }

    public void Animate()
    {                                                        //animator
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Animate"); }
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= animationSpeed[currentAnimation])
        {
            currentFrame++;
            timeSinceLastFrame -= animationSpeed[currentAnimation];
        }

        if (attacking != Attacking.No)
        {
            activity = Activity.Attacking;
        }
        else
        {
            if (input != Point.Zero)
            {
                idleTime = 0;
                if (input.X != 0)
                {
                    if (walled.Bottom > 0)
                    {
                        activity = Activity.Running;
                    }
                    else
                    {
                        if (velocity.Y < 0)
                        {
                            activity = Activity.Jumping;
                            //jumping
                        }
                        else
                        {
                            activity = Activity.Falling;
                            //falling
                        }
                    }
                }
                else
                {
                    if (walled.Bottom <= 0)
                    {
                        if (velocity.Y < 0)
                        {
                            activity = Activity.Jumping;
                            //jumping
                        }
                        else
                        {
                            activity = Activity.Falling;
                            //falling
                        }
                    }
                    else
                    {
                        //nothing, yet
                    }
                }
            }
            else
            {
                if (walled.Bottom <= 0)
                {
                    if (velocity.Y < 0)
                    {
                        activity = Activity.Jumping;
                        //jumping
                    }
                    else
                    {
                        activity = Activity.Falling;
                        //falling
                    }
                }
                else
                {
                    activity = Activity.Idle;
                }
            }
            if (rollTime > 0)
            {
                activity = Activity.Rolling;
            }
        }

        if (activity == Activity.Idle)
        {
            idleTime += Irbis.Irbis.DeltaTime;
            specialTime += Irbis.Irbis.DeltaTime;
        }

        if (currentFrame > animationFrames[currentAnimation])
        {
            if (attacking != Attacking.No)
            {
                if (attackImmediately)
                {
                    attackImmediately = false;
                    attackID = attackIDtracker++;
                }
                else
                {
                    attacking = Attacking.No;
                }
            }
            if (animationNoLoop)
            {
                switch (currentAnimation)
                {
                    case 0:
                        SetAnimation(1, false);
                        break;
                    default:
                        SetAnimation();
                        break;
                }
            }
            else
            {
                SetAnimation();
            }
        }
        else if (previousActivity != activity)
        {
            SetAnimation();
        }

        if (direction == Direction.Right && currentAnimation % 2 != 0)
        {
            currentAnimation++;
        }

        if (previousAnimation != currentAnimation)
        {
            timeSinceLastFrame = 0;
            currentFrame = 0;
        }




        //animationSourceRect = new Rectangle(128 * currentFrame, 128 * currentAnimation, 128, 128);

        animationSourceRect.X = 128 * currentFrame;
        animationSourceRect.Y = 128 * currentAnimation;


        previousAnimation = currentAnimation;
        previousActivity = activity;
    }

    public void SetAnimation()
    {
        switch (activity)
        {
            case Activity.Idle:
                if (direction == Direction.Forward)
                {
                    if (idleTime >= idleTimeMax && idleTimeMax > 0)
                    {
                        idleTime = 0;
                        SetAnimation(1, false);
                    }
                    else
                    {
                        SetAnimation(1, false);
                    }
                }
                else
                {
                    if (idleTime >= idleTimeMax && idleTimeMax > 0)
                    {
                        idleTime -= idleTimeMax;
                        SetAnimation(0, true);
                        direction = Direction.Forward;
                    }
                    else if (specialTime >= specialIdleTime)
                    {
                        //currentAnimation = 3;
                        specialTime -= specialIdleTime;
                        switch (Irbis.Irbis.RandomInt(1))
                        {
                            case 0:
                                SetAnimation(5, true);
                                break;
                            case 1:
                                SetAnimation(7, true);
                                break;
                        }
                    }
                    else
                    {
                        SetAnimation(3, false);
                    }
                }
                if (frameInput || combat)
                {
                    idleTime = 0;
                }
                else
                {
                    //idleTime += Irbis.Irbis.DeltaTime;
                }

                break;
            case Activity.Running:
                SetAnimation(9, false);
                break;
            case Activity.Jumping:
                SetAnimation(9, false);
                break;
            case Activity.Rolling:
                SetAnimation(9, false);
                break;
            case Activity.Falling:
                SetAnimation(9, false);
                break;
            case Activity.Landing:
                SetAnimation(9, false);
                break;
            case Activity.Attacking:
                //Random RAND = new Random();                 //current attack animations are 11 and 13
                //USE GAME.RAND!

                SetAnimation(11, true);
                break;
            default:
                SetAnimation(9, false);                                                           //run
                break;
        }

        if (nextAnimation >= 0)
        {
            SetAnimation(nextAnimation, false);
        }
    }

    public void SetAnimation(int animation, bool noLoop)
    {
        currentAnimation = animation;
        currentFrame = 0;
        nextAnimation = -1;
        animationNoLoop = noLoop;
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = colliderWidth;
        testCollider.Height = colliderHeight;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && s.Collider != trueCollider && Irbis.Irbis.DistanceSquared(trueCollider, s.Collider) <= 0)
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
        testCollider.X = (int)testPos.X + colliderOffset.X;
        testCollider.Y = (int)testPos.Y + colliderOffset.Y;

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
            if (amountToMove != Vector2.Zero)
            {
                Irbis.Irbis.WriteLine("this: " + this.ToString());
                Irbis.Irbis.WriteLine("        pass: " + pass);
                Irbis.Irbis.WriteLine("amountToMove: " + amountToMove);
                Irbis.Irbis.WriteLine("    velocity: " + velocity);
                Irbis.Irbis.WriteLine("    position: " + position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + trueCollider.Top + " B:" + trueCollider.Bottom + " L:" + trueCollider.Left + " R:" + trueCollider.Right);
                Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                //foreach (ICollisionObject s in collided)
                //{ Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right); }
                Irbis.Irbis.WriteLine("after1--");
            }

            pass = true;
            if (Y)
            {
                testPos.X = (int)Math.Round((double)position.X);
                testPos.Y = position.Y;
                testPos.X += amountToMove.X;
                testCollider.X = (int)testPos.X + colliderOffset.X;
                testCollider.Y = (int)testPos.Y + colliderOffset.Y;

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
                testCollider.X = (int)testPos.X + colliderOffset.X;
                testCollider.Y = (int)testPos.Y + colliderOffset.Y;

                pass = !(collided.Intersects(testCollider));

                if (pass)
                {
                    amountToMove.X = 0;
                }
            }
            if (amountToMove != Vector2.Zero)
            {
                Irbis.Irbis.WriteLine("        pass: " + pass);
                Irbis.Irbis.WriteLine("amountToMove: " + amountToMove);
                Irbis.Irbis.WriteLine("    velocity: " + velocity);
                Irbis.Irbis.WriteLine("    position: " + position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + trueCollider.Top + " B:" + trueCollider.Bottom + " L:" + trueCollider.Left + " R:" + trueCollider.Right);
                Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                //foreach (ICollisionObject s in collided)
                //{ Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right); }
                Irbis.Irbis.WriteLine("after2--");
            }
        }

        if (amountToMove != Vector2.Zero)
        {
            Irbis.Irbis.WriteLine("    velocity: " + velocity);
        }

        position += amountToMove;

        CalculateMovement();
        for (int i = 0; i < collided.bottomCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.bottomCollided[i].Collider, Side.Bottom))
            {
                collided.bottomCollided.RemoveAt(i);
                walled.Bottom--;
                i--;
            }
        }
        for (int i = 0; i < collided.rightCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.rightCollided[i].Collider, Side.Right))
            {
                collided.rightCollided.RemoveAt(i);
                walled.Right--;
                i--;
            }
        }
        for (int i = 0; i < collided.leftCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.leftCollided[i].Collider, Side.Left))
            {
                collided.leftCollided.RemoveAt(i);
                walled.Left--;
                i--;
            }
        }
        for (int i = 0; i < collided.topCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(trueCollider, collided.topCollided[i].Collider, Side.Top))
            {
                collided.topCollided.RemoveAt(i);
                walled.Top--;
                i--;
            }
        }

        if (amountToMove != Vector2.Zero)
        {
            Irbis.Irbis.WriteLine("        pass: " + pass);
            Irbis.Irbis.WriteLine("amountToMove: " + amountToMove);
            Irbis.Irbis.WriteLine("    velocity: " + velocity);
            Irbis.Irbis.WriteLine("    position: " + position);
            Irbis.Irbis.WriteLine("     testPos: " + testPos);
            Irbis.Irbis.WriteLine("   pcollider: T:" + trueCollider.Top + " B:" + trueCollider.Bottom + " L:" + trueCollider.Left + " R:" + trueCollider.Right);
            Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
            //foreach (ICollisionObject s in collided)
            //{ Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right); }
            Irbis.Irbis.WriteLine("done.\n");
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

    public void Knockback(Direction knockbackDirection, float strength)
    {
        if (knockbackDirection == Direction.Left)
        {
            velocity.Y += -25f * strength;
            velocity.X += -200f * strength;
        }
        else
        {
            velocity.Y += -25f * strength;
            velocity.X += 200f * strength;
        }
    }

    public void Hurt(float damage)
    {
        health -= damage;
        Irbis.Irbis.CameraShake(0.075f, 0.05f * damage);
    }

    public void Stun(float duration)
    {
        stunned += duration;
        AIactivity = AI.Stunned;
        attackCooldownTimer += 0.5f;
    }

    public void Shockwave(float distance, float power, Vector2 heading)
    {
        Irbis.Irbis.WriteLine("Lizard Guy shockwave\n precalc velocity:" + velocity);
        Irbis.Irbis.WriteLine("shockwaveKnockback:" + shockwaveKnockback + " heading:" + heading + " shockwaveEffectiveDistance:"
            + shockwaveEffectiveDistance + " distance:" + distance + " power:" + power + " (1 / mass):" + (1 / mass));

        heading.Normalize();
        //Stun(((shockwaveEffectiveDistance - distance) * 2) / shockwaveStunTime);
        velocity += shockwaveKnockback * heading * (shockwaveEffectiveDistance - distance) * power * (1 / mass);

        Irbis.Irbis.WriteLine("postcalc velocity:" + velocity);
        Irbis.Irbis.WriteLine("shockwaveKnockback:" + shockwaveKnockback + " heading:" + heading + " shockwaveEffectiveDistance:"
            + shockwaveEffectiveDistance + " distance:" + distance + " power:" + power + " (1 / mass):" + (1 / mass) + "\n");
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
                animationFrame.Draw(sb, (position * Irbis.Irbis.screenScale).ToPoint());
                goto case 1;
            case 1:
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto default;
            default:
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }
}
