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

        Dying = 19,
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
    private string name = "Wizard Guy (bweam)";

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
    private float mass = 0.5f;

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
    public WizardActivity previousActivity;
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
    public Point colliderOffset;
    public Point colliderSize;
    public float terminalVelocity = 5000f;

    float depth;

    //attack variables

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
    public WizardActivity activity;

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

    private object attackPlayerLock;
    private object collidedLock;

    public Rectangle bossArena;

    public Collided collided;

    public int[] state = new int[5];
    public float[] cooldown = new float[5];


    public WizardGuy(Texture2D t, Vector2 iPos, float enemyHealth, float enemyDamage, float enemySpeed, Rectangle? BossArena, float drawDepth)
    {
        cooldown[0] = 00f; // wander
        cooldown[1] = 05f; // roll
        cooldown[2] = 03f; // swipe
                           //cooldown[3] = 00f; // none
        cooldown[4] = 15f; // bury

        collided = new Collided();

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
        wanderSpeed = (1f / 5f) * enemySpeed;
        wanderTime = 1f;
        initialWanderTime = 0f;

        speed = defaultSpeed = enemySpeed;
        jumpTime = 0;
        animationNoLoop = false;
        colliderOffset = new Point(44, 26);
        collider.Size = trueCollider.Size = colliderSize = new Point(60, 100);

        position.X -= colliderOffset.X;
        position.Y -= colliderOffset.Y;

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
        {
            animationSpeed[i] = animationSpeed[0];
        }

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

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave += Enemy_OnPlayerShockwave;
    }

    public bool Update()
    {
        prevInput = input;
        input = Point.Zero;
        frameInput = false;
        /*
        if (Irbis.Irbis.jamie != null)
        {
            if (aiEnabled && !ActivelyAttacking && cooldown[0] <= 0 && stunned <= 0 && walled.Bottom > 0)
            {
                Direction playerDirection = Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider);
                /* attack logic
                if (meleeActivitiesInARow >= meleeActivityLimit)
                {
                }
                else if ()
                {
                }
                else
                { // wander around, maybe yell a bit, just don't look like a doofus
                    Wander((Irbis.Irbis.RandomInt(3) + 1));
                }/
            }
        }

        if (Irbis.Irbis.IsTouching(trueCollider, Irbis.Irbis.jamie.Collider))
        {
            /*if (stunned <= 0)
            {
                collider = Rectangle.Empty;
                if (state[1] >= 9 && Irbis.Irbis.jamie.HurtOnTouch(rollDamage / 2))
                {
                    OnTouch(trueCollider, rollKnockback);
                }
                else if (state[1] > 0 && Irbis.Irbis.jamie.HurtOnTouch(rollDamage))
                {
                    OnTouch(trueCollider, rollKnockback);
                }
                else if (Irbis.Irbis.jamie.HurtOnTouch(10f))
                {
                    OnTouch(trueCollider, swipeKnockback);
                }
            }
            else if (Irbis.Irbis.jamie.collided.Horizontal || Irbis.Irbis.jamie.collided.Vertical)
            {
                collider = Rectangle.Empty;
                OnTouch(trueCollider, swipeKnockback);
            }/
        }
        else if (collider == Rectangle.Empty && Irbis.Irbis.jamie.invulnerableOnTouch <= 0)
        { collider = trueCollider; }

        */

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
            { Irbis.Irbis.KillEnemy(this); }
        }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            { Irbis.Irbis.doneEvent.Set(); }
        }
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack)
    {
        Irbis.Irbis.WriteLine(name + " response:\nattackCollider:" + AttackCollider + " this.collider:" + trueCollider);
        if (AttackCollider.Intersects(trueCollider))
        {
            PlayerAttackCollision(Attack);
            Irbis.Irbis.WriteLine("hit. health remaining:" + health);
        }
        else
        { Irbis.Irbis.WriteLine("miss. health remaining:" + health); }
        Irbis.Irbis.WriteLine(name + " done.\n");
        return true;
    }

    public bool Enemy_OnPlayerShockwave(Point Origin, int RangeSquared, int Range, float Power)
    {
        Irbis.Irbis.WriteLine(name + " Enemy_OnPlayerShockwave triggered");
        float DistanceSQR = Irbis.Irbis.DistanceSquared(trueCollider, Origin);
        if (DistanceSQR <= RangeSquared)
        {
            float Distance = (float)Math.Sqrt(DistanceSQR);
            if (Power > 1.5f)
            {
                if (state[1] >= 7)
                {
                    state[1] = 100;
                    stunned = ((Range - Distance) / Range) * shockwaveStunTime * Power;
                    Irbis.Irbis.WriteLine("roll interrupted! stunned for:" + stunned);
                    activity = WizardActivity.Dying;
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
                    activity = WizardActivity.Dying;
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
                    activity = WizardActivity.Dying;
                    Irbis.Irbis.WriteLine("attack slowed! stunned for:" + stunned);
                }
            }
        }
        Irbis.Irbis.WriteLine(name + " done.\n");
        return true;
    }

    public void PlayerAttackCollision(Attacking Attack)
    {
        Hurt(Irbis.Irbis.jamie.attackDamage);

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
                { /*GetUp();*/ }
            }
        }
        else if (ActivelyAttacking)
        {
            /*
            if (state[1] > 0)
            { RollAttack(); }
            if (state[2] > 0)
            { SwipeAttack(); }
            if (state[3] > 0)
            { TailwhipAttack(); }
            if (state[4] > 0)
            { BuryAttack(); }
            */
        }
        else if (cooldown[0] > 0 && walled.Bottom > 0 /*&& activity != WizardActivity.TurningAround && activity != WizardActivity.GettingUp*/)
        {
            Direction playerDirection = Irbis.Irbis.Directions(trueCollider, Irbis.Irbis.jamie.Collider);
            /*if (playerDirection != direction && playerDirection != Direction.Forward && activity != WizardActivity.WalkLeft && activity != WizardActivity.WalkRight)
            { // turn around
                Irbis.Irbis.WriteLine(name + " not facing player. turning around...");
                direction = playerDirection;
                activity = WizardActivity.TurningAround;
                //meleeActivitiesInARow++;
            }
            else if (activity != WizardActivity.TurningAround)
            {
                Wander();
                cooldown[0] -= Irbis.Irbis.DeltaTime;

                if (velocity.X > 0.1f)
                { activity = WizardActivity.WalkRight; }
                else if (velocity.X < -0.1f)
                { activity = WizardActivity.WalkLeft; }
                else
                { activity = WizardActivity.Idle; }
            }*/
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

    public void CalculateMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("CalculateMovement"); }
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        //collider = new Rectangle((int)position.X + XcolliderOffset, (int)position.Y + YcolliderOffset, colliderSize.X, colliderSize.Y);
        trueCollider.X = (int)Math.Round((decimal)position.X) + colliderOffset.X;
        trueCollider.Y = (int)Math.Round((decimal)position.Y) + colliderOffset.Y;
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
                        activity = WizardActivity.Misc;
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
                        activity = WizardActivity.Misc;
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

    public void Dying()
    {
        activity = WizardActivity.Dying;
        previousActivity = activity;
        trueCollider.Size = collider.Size = new Point(60, 32);
        colliderOffset = new Point(44, 93);
        SetAnimation(14, true);
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = colliderSize.X;
        testCollider.Height = colliderSize.Y;

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
        }

        position += amountToMove;
        CalculateMovement();

        //if (amountToMove != Vector2.Zero)
        //{
        //    Irbis.Irbis.WriteLine("        pass: " + pass);
        //    Irbis.Irbis.WriteLine("amountToMove: " + amountToMove);
        //    Irbis.Irbis.WriteLine("    velocity: " + velocity);
        //    Irbis.Irbis.WriteLine("    position: " + position);
        //    Irbis.Irbis.WriteLine("     testPos: " + testPos);
        //    Irbis.Irbis.WriteLine("   pcollider: T:" + collider.Top + " B:" + collider.Bottom + " L:" + collider.Left + " R:" + collider.Right);
        //    Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
        //}

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
                animationFrame.Draw(sb, ((position + (colliderOffset - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto case 1;
            case 1:
                goto default;
            default:

                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

    public void Light(SpriteBatch sb)
    { }
}
