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

    public string EnemyName
    {
        get
        {
            return enemyName;
        }
    }
    private string enemyName = "Lizard Guy Boss (rawr)";

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
    public bool aiEnabled;

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

    private bool collidedContains;
    public float speed;
    public float defaultSpeed;
    int climbablePixels;
    public float specialTime;
    public float specialIdleTime;
    public float rollTimeMax;
    public float rollTime;
    public float rollSpeed;
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
    public int XcolliderOffset;
    public int YcolliderOffset;
    public int colliderWidth;
    public int colliderHeight;
    public float terminalVelocity = 5000f;

    float depth;

    public float stunned;
    float wanderSpeed;
    bool previouslyWandered;
    public float wanderTime;
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

    public float collisionCheckDistanceSqr;

    public float distanceToPlayerSqr;
    public int combatCheckDistanceSqr;
    public int persueCheckDistanceSqr;
    public bool combat;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    public Rectangle attackCollider;

    public int attackColliderWidth;
    public int attackColliderHeight;

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

    public List<ICollisionObject> collided;
    List<Side> sideCollided;

    public LizardGuy(Texture2D t, Vector2 iPos, float enemyHealth, float enemyDamage, float enemySpeed, float drawDepth)
    {
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

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, 0.9f);

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
        wanderSpeed = (2f / 3f) * enemySpeed;
        wanderTime = 0f;
        previouslyWandered = false;

        speed = defaultSpeed = enemySpeed;
        jumpTime = 0;
        jumpTimeMax = 0.06f;
        animationNoLoop = false;
        XcolliderOffset = 14;
        YcolliderOffset = 28;
        colliderWidth = 100;
        colliderHeight = 100;

        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;

        maxHealth = health = enemyHealth;
        lastHitByAttackID = -1;

        stunned = 0;
        speedModifier = 1f;

        attackCollider = Rectangle.Empty;

        attackColliderWidth = 30;
        attackColliderHeight = 30;

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

        collided = new List<ICollisionObject>();
        sideCollided = new List<Side>();

        CalculateMovement();
        if (colliderHeight > colliderWidth)
        {
            collisionCheckDistanceSqr = (colliderHeight * colliderHeight);
        }
        else
        {
            collisionCheckDistanceSqr = (colliderWidth * colliderWidth);
        }
    }

    public bool Update()
    {
        prevInput = input;
        prevAttacking = attacking;
        input = Point.Zero;
        frameInput = false;

        if (Irbis.Irbis.GetKeyboardState.IsKeyDown(Keys.P))
        {
            input.X++;
        }
        if (Irbis.Irbis.GetKeyboardState.IsKeyDown(Keys.O))
        {
            input.X--;
        }

        //PlayerAttackCollision();
        Movement();
        CalculateMovement();
        Animate();
        Collision(Irbis.Irbis.collisionObjects);
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

    public void Movement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Movement"); }
        if (attacking != Attacking.No)
        {
            //AttackMovement();
            isRunning = false;
            if (currentFrame <= attackMovementFrames && walled.Bottom > 0)
            {
                if (direction == Direction.Right && (collided.Count > 1 || (collided.Count > 0 && collided[0].Collider.Right >= collider.Right)))
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                }
                else if (direction == Direction.Left && (collided.Count > 1 || (collided.Count > 0 && collided[0].Collider.Left <= collider.Left)))
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, -attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                }
                debugspeed = attackMovementSpeed;
            }
            else if (walled.Bottom <= 0)
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                debugspeed = attackMovementSpeed;
            }
            else
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                debugspeed = 0f;
            }
        }
        else
        {
            if (input.X != 0)
            {
                if (walled.Bottom > 0)                                                                   //movement
                {
                    isRunning = true;
                }
                else if (!isRunning)
                {
                    if ((input.X > 0 && velocity.X < 0) || (input.X < 0 && velocity.X > 0))
                    {
                        velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * airSpeed, movementLerpAir * Irbis.Irbis.DeltaTime);
                    }
                    else
                    {
                        velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * airSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    }
                    debugspeed = airSpeed;
                }

                if (isRunning && input.X == prevInput.X)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    debugspeed = speed;
                }
                else
                {
                    isRunning = false;
                }
                if ((walled.Left > 0 && input.X < 0) || (walled.Right > 0 && input.X > 0))
                {
                    isRunning = false;
                }
            }
            else
            {
                if (walled.Bottom > 0)                                                                   //movement
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                    debugspeed = speed;
                }
                else if (Math.Abs(velocity.X) < airSpeed)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * airSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    debugspeed = airSpeed;
                }
                isRunning = false;
            }
        }

        if (Math.Abs(velocity.X) <= 0.000001f)
        {
            velocity.X = 0;
        }

        if (walled.Right > 0 && velocity.X > 0)
        {
            velocity.X = 0;
        }
        if (walled.Left > 0 && velocity.X < 0)
        {
            velocity.X = 0;
        }

        if (jumpTime > 0)
        {
            velocity.Y = Irbis.Irbis.Lerp(velocity.Y, -speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
            //velocity.Y = -speed;
        }
        if (walled.Top > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }
        if (walled.Bottom <= 0 && jumpTime <= 0)
        {
            if (attacking != Attacking.No)
            {
                velocity.Y += (Irbis.Irbis.gravity / 2) * mass * Irbis.Irbis.DeltaTime;
            }
            else
            {
                velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime;
            }
        }

        if (velocity.X > terminalVelocity)
        {
            velocity.X = terminalVelocity;
        }
        if (velocity.Y > terminalVelocity)
        {
            velocity.Y = terminalVelocity;
        }
        if (velocity.X < -terminalVelocity)
        {
            velocity.X = -terminalVelocity;
        }
        if (velocity.Y < -terminalVelocity)
        {
            velocity.Y = -terminalVelocity;
        }
        position += velocity * Irbis.Irbis.DeltaTime;
    }

    public void CalculateMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("CalculateMovement"); }
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        displayRect.X = (int)position.X;
        displayRect.Y = (int)position.Y;
        //collider = new Rectangle((int)position.X + XcolliderOffset, (int)position.Y + YcolliderOffset, colliderWidth, colliderHeight);
        collider.X = (int)Math.Round((decimal)position.X) + XcolliderOffset;
        collider.Y = (int)Math.Round((decimal)position.Y) + YcolliderOffset;
        collider.Width = colliderWidth;
        collider.Height = colliderHeight;
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
            if (s.Collider != Rectangle.Empty && s.Collider != collider && Irbis.Irbis.DistanceSquared(collider, s.Collider) <= 0)
            {
                collidedContains = collided.Contains(s);
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Bottom))                              //DOWN
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.Bottom);
                        walled.Bottom++;
                        if (negAmountToMove.Y > s.Collider.Top - collider.Bottom && (velocity.Y * Irbis.Irbis.DeltaTime) >= -(s.Collider.Top - collider.Bottom))
                        {
                            negAmountToMove.Y = s.Collider.Top - collider.Bottom;
                        }
                    }
                    else if (negAmountToMove.Y > s.Collider.Top - collider.Bottom)
                    {
                        negAmountToMove.Y = s.Collider.Top - collider.Bottom;
                    }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Right))                               //RIGHT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.Right);
                        walled.Right++;
                        if (negAmountToMove.X > s.Collider.Left - collider.Right && (velocity.X * Irbis.Irbis.DeltaTime) >= -(s.Collider.Left - collider.Right))
                        {
                            negAmountToMove.X = s.Collider.Left - collider.Right;
                        }
                    }
                    else if (negAmountToMove.X > s.Collider.Left - collider.Right)
                    {
                        negAmountToMove.X = s.Collider.Left - collider.Right;
                    }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Left))                                //LEFT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.Left);
                        walled.Left++;
                        if (amountToMove.X < s.Collider.Right - collider.Left && (velocity.X * Irbis.Irbis.DeltaTime) <= -(s.Collider.Right - collider.Left))
                        {
                            amountToMove.X = s.Collider.Right - collider.Left;
                        }
                    }
                    else if (amountToMove.X < s.Collider.Right - collider.Left)
                    {
                        amountToMove.X = s.Collider.Right - collider.Left;
                    }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Top))                                 //UP
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.Top);
                        walled.Top++;
                        if (amountToMove.Y < s.Collider.Bottom - collider.Top && (velocity.Y * Irbis.Irbis.DeltaTime) <= -(s.Collider.Bottom - collider.Top))
                        {
                            amountToMove.Y = s.Collider.Bottom - collider.Top;
                        }
                    }
                    else if (amountToMove.Y < s.Collider.Bottom - collider.Top)
                    {
                        amountToMove.Y = s.Collider.Bottom - collider.Top;
                    }
                }
            }
        }

        if (walled.Left == 1 && input.X < 0)
        {
            int climbamount = (collider.Bottom - collided[sideCollided.IndexOf(Side.Left)].Collider.Top);
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
            int climbamount = (collider.Bottom - collided[sideCollided.IndexOf(Side.Right)].Collider.Top);
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
            testPos.Y = (int)Math.Round((decimal)position.Y);
            testPos.X = position.X;
            testPos.Y += amountToMove.Y;
            Y = true;
        }
        else if (amountToMove.X != 0)
        {
            testPos.X = (int)Math.Round((decimal)position.X);
            testPos.Y = position.Y;
            testPos.X += amountToMove.X;
            X = true;
        }

        bool pass = true;
        testCollider.X = (int)testPos.X + XcolliderOffset;
        testCollider.Y = (int)testPos.Y + YcolliderOffset;

        foreach (ICollisionObject s in collided)
        {
            if (s.Collider.Intersects(testCollider))
            {
                pass = false;
            }
        }

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
                Irbis.Irbis.WriteLine("  position: " + position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + collider.Top + " B:" + collider.Bottom + " L:" + collider.Left + " R:" + collider.Right);
                Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                foreach (ICollisionObject s in collided)
                {
                    Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right);
                }
                Irbis.Irbis.WriteLine("after1--");
            }

            pass = true;
            if (Y)
            {
                testPos.X = (int)Math.Round((decimal)position.X);
                testPos.Y = position.Y;
                testPos.X += amountToMove.X;
                testCollider.X = (int)testPos.X + XcolliderOffset;
                testCollider.Y = (int)testPos.Y + YcolliderOffset;

                foreach (ICollisionObject s in collided)
                {
                    if (s.Collider.Intersects(testCollider))
                    {
                        pass = false;
                    }
                }

                if (pass)
                {
                    amountToMove.Y = 0;
                }
            }
            else if (X)
            {
                testPos.Y = (int)Math.Round((decimal)position.Y);
                testPos.X = position.X;
                testPos.Y += amountToMove.Y;
                testCollider.X = (int)testPos.X + XcolliderOffset;
                testCollider.Y = (int)testPos.Y + YcolliderOffset;

                foreach (ICollisionObject s in collided)
                {
                    if (s.Collider.Intersects(testCollider))
                    {
                        pass = false;
                    }
                }

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
                Irbis.Irbis.WriteLine("  position: " + position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + collider.Top + " B:" + collider.Bottom + " L:" + collider.Left + " R:" + collider.Right);
                Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                foreach (ICollisionObject s in collided)
                {
                    Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right);
                }
                Irbis.Irbis.WriteLine("after2--");
            }
        }

        if (amountToMove != Vector2.Zero)
        {
            Irbis.Irbis.WriteLine("    velocity: " + velocity);
        }

        position += amountToMove;

        CalculateMovement();
        for (int i = 0; i < collided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided[i].Collider, sideCollided[i]))
            {
                switch (sideCollided[i])
                {
                    case Side.Bottom:
                        walled.Bottom--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.Right:
                        walled.Right--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.Left:
                        walled.Left--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.Top:
                        walled.Top--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    default:
                        break;
                }
            }
        }
        if (amountToMove != Vector2.Zero)
        {
            Irbis.Irbis.WriteLine("        pass: " + pass);
            Irbis.Irbis.WriteLine("amountToMove: " + amountToMove);
            Irbis.Irbis.WriteLine("    velocity: " + velocity);
            Irbis.Irbis.WriteLine("  position: " + position);
            Irbis.Irbis.WriteLine("     testPos: " + testPos);
            Irbis.Irbis.WriteLine("   pcollider: T:" + collider.Top + " B:" + collider.Bottom + " L:" + collider.Left + " R:" + collider.Right);
            Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
            foreach (ICollisionObject s in collided)
            {
                Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right);
            }
            Irbis.Irbis.WriteLine("done--");
            Irbis.Irbis.WriteLine();
        }
        if (walled.Top > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            position.Y = (int)Math.Round((decimal)position.Y);
        }
        if (walled.Bottom > 0 && velocity.Y > 0)
        {
            velocity.Y = 0;
            position.Y = (int)Math.Round((decimal)position.Y);
        }
        if (walled.Left > 0 && velocity.X < 0)
        {
            velocity.X = 0;
            position.X = (int)Math.Round((decimal)position.X);
        }
        if (walled.Right > 0 && velocity.X > 0)
        {
            velocity.X = 0;
            position.X = (int)Math.Round((decimal)position.X);
        }
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
        //game.CameraShake(0.075f, 0.05f * damage);
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

    public void Draw(SpriteBatch sb)
    {
        switch (Irbis.Irbis.debug)
        {
            case 5:
                goto case 4;
            case 4:
                goto case 3;
            case 3:
                goto case 2;
            case 2:
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, (position * Irbis.Irbis.screenScale).ToPoint());
                goto case 1;
            case 1:
                if (attackCollider != Rectangle.Empty) { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, 0.9f); }
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto default;
            default:
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }
}
