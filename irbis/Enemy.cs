using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



public class Enemy : IEnemy
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

    public string Name
    {
        get
        {
            return name;
        }
    }
    private string name = "enemy " + (Irbis.Irbis.RandomInt(100)).ToString("00");

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
    private float mass = 0.9f;

    public float StunTime
    {
        get
        {
            return stunned;
        }
    }

    private bool collidedContains;
    public float speed;
    public float defaultSpeed;
    int climbablePixels;

    Texture2D tex;
    public Rectangle displayRect;
    public Rectangle animationSourceRect;
    Rectangle testCollider;
    public int XcolliderOffset;
    public int YcolliderOffset;
    public int colliderWidth;
    public int colliderHeight;
    public Print animationFrame;


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

    public Enemy(string enemyName, Texture2D t, Vector2 iPos, float enemyHealth, float enemyDamage, float enemySpeed, float drawDepth)
	{
        tex = t;
        attackPlayerLock = new object();
        collidedLock = new object();
        AIenabled = true;

        name = enemyName;

        depth = drawDepth;

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.001f);

        climbablePixels = 3;

        position = iPos;
        direction = Direction.Forward;
        location = Location.Air;
        activity = Activity.Idle;
        AIactivity = AI.Wander;
        wanderSpeed = (2f/3f) * enemySpeed;
        wanderTime = 0f;
        previouslyWandered = false;

        speed = defaultSpeed = enemySpeed;
        jumpTime = 0;
        jumpTimeMax = 0.06f;
        animationNoLoop = false;
        XcolliderOffset = 22;
        YcolliderOffset = 19;
        colliderWidth = 19;
        colliderHeight = 43;

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

        shockwaveStunTime = Irbis.Irbis.jamie.shockwaveStunTime;
        shockwaveKnockback = Irbis.Irbis.jamie.shockwaveKnockback;


        displayRect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 64, 64);
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

        animationSourceRect = new Rectangle(64 * currentFrame, 64 * currentAnimation, 64, 64);

        collided = new List<ICollisionObject>();
        sideCollided = new List<Side>();

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave += Enemy_OnPlayerShockwave;
    }

    public bool Update()
    {
        if (stunned <= 0)
        {
            if (jumpTime > 0)
            {
                velocity.Y = -250f;
                jumpTime -= Irbis.Irbis.DeltaTime;
                if (jumpTime <= 0) { jumpTime = 0; }
            }
            distanceToPlayerSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider.Center, this.Collider.Center);
            if (distanceToPlayerSqr <= combatCheckDistanceSqr)
            {
                AIactivity = AI.Combat;
            }
            else if (distanceToPlayerSqr <= persueCheckDistanceSqr)
            {
                AIactivity = AI.Persue;
            }
            else
            {
                AIactivity = AI.Wander;
            }
            attackCooldownTimer -= Irbis.Irbis.DeltaTime;

            if (AIenabled)
            {
                switch (AIactivity)
                {
                    case AI.Combat:
                        Combat(Irbis.Irbis.jamie);
                        break;
                    case AI.Persue:
                        Persue(Irbis.Irbis.jamie);
                        break;
                    case AI.Wander:
                        Wander();
                        break;
                    default:
                        Wander();
                        break;
                }
            }
            //PlayerCollision(game.jamie, this);
        }
        else
        {
            stunned -= Irbis.Irbis.DeltaTime;
            if (walled.Bottom > 0 && Math.Abs(velocity.X) > 0)
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpBuildup * Irbis.Irbis.DeltaTime);
            }
        }

        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i].ApplyEffect(this))
            { activeEffects.RemoveAt(i); }
        }

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

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack)
    {
        Irbis.Irbis.WriteLine(name + " response:\nattackCollider:" + AttackCollider + " this.collider:" + collider);
        if (AttackCollider.Intersects(collider))
        {
            PlayerAttackCollision();
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
        float DistanceSQR = Irbis.Irbis.DistanceSquared(collider, Origin);
        if (DistanceSQR <= RangeSquared)
        {
            float Distance = (float)Math.Sqrt(DistanceSQR);
            Stun(((Range - Distance) / Range) * shockwaveStunTime * Power);
            if (Irbis.Irbis.Directions(Origin, collider.Center) == Direction.Left)
            { velocity = new Vector2(-shockwaveKnockback.X, shockwaveKnockback.Y)  * (Range - Distance) * Power * (1 / mass); }
            else
            { velocity = shockwaveKnockback * (Range - Distance) * Power * (1 / mass); }
            Irbis.Irbis.WriteLine("postcalc velocity:" + velocity);
        }
        Irbis.Irbis.WriteLine(name + " done.\n");
        return true;
    }

    public void Death()
    {
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave -= Enemy_OnPlayerShockwave;
    }

    public void Respawn(Vector2 initialPos)
    {
        position = initialPos * 32;
        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;
        velocity = Vector2.Zero;
        health = maxHealth;
        CalculateMovement();
    }

    public bool Wander()
    {
        if (wanderTime > 0)
        {
            wanderTime -= Irbis.Irbis.DeltaTime;
        }
        else
        {
            
            if (previouslyWandered)
            {
                input = Point.Zero;
                previouslyWandered = false;
            }
            else
            {
                if (walled.Right > 0 && walled.Right <= 1)
                {
                    if (walled.Right > 0)
                    {
                        input.X++;
                        direction = Direction.Right;
                        jumpTime = jumpTimeMax;
                    }
                    else
                    {
                        input.X--;
                        direction = Direction.Left;
                    }
                }
                else if (walled.Left > 0 && walled.Left <= 1)
                {
                    if (walled.Left > 0)
                    {
                        input.X--;
                        direction = Direction.Left;
                        jumpTime = jumpTimeMax;
                    }
                    else
                    {
                        input.X++;
                        direction = Direction.Right;
                    }
                }
                else
                {
                    if (Irbis.Irbis.RandomFloat > 0.5)
                    {
                        input.X--;
                        direction = Direction.Left;
                    }
                    else
                    {
                        input.X++;
                        direction = Direction.Right;
                    }
                }

                previouslyWandered = true;
            }
            
            wanderTime = Irbis.Irbis.RandomFloat + 0.5f;
        }

        if (walled.Bottom > 0)                                                               //movement
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * wanderSpeed * speedModifier, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        }
        else if (velocity.X == 0)
        {
            velocity.X = input.X * 0.1f * speed;
        }
        return true;
    }

    public bool Persue(Player player)
    {
        input = Point.Zero;
        player.heading = (player.Collider.Center - collider.Center).ToVector2();
        //player.heading.Normalize();

        if (player.heading.X > 0)
        {
            if (walled.Right <= 0)
            {
                input.X++;
                direction = Direction.Right;
            }
            else if (walled.Bottom > 0)
            {
                input.X++;
                direction = Direction.Right;
                jumpTime = jumpTimeMax;
            }
        }
        else if (player.heading.X < 0)
        {
            if (walled.Left <= 0)
            {
                input.X--;
                direction = Direction.Left;
            }
            else if (walled.Bottom > 0)
            {
                input.X--;
                direction = Direction.Left;
                jumpTime = jumpTimeMax;
            }
        }

        if (walled.Bottom > 0)                                                               //movement
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed * speedModifier, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        }
        else if (velocity.X == 0)
        {
            velocity.X = input.X * 0.1f * speed;
        }
        return true;
    }

    public bool Combat(Player player)
    {
        velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        jumpTime = 0;

        if (attackCooldownTimer <= 0)
        {
            attackID = attackIDtracker++;
            attackCooldownTimer = attackCooldown;
            attacking = Attacking.Attack1;
        }
        
        if (attacking != Attacking.No)
        {
            Hitbox();
            if (attackCollider.Intersects(player.Collider))
            {
                player.Hurt(attackDamage, true);
            }
        }
        else
        {
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
            attackID = 0;
        }
        return true;
    }

    public void Hitbox()
    {
        //change attackCollider based on if (attacking) and current animation
        if (lastAttackID != attackID)
        {
            lastAttackID = attackID;
            attackDamage = attack1Damage;

            if (direction == Direction.Left)
            {
                attackCollider.X = collider.Center.X - attackColliderWidth;
                attackCollider.Y = collider.Center.Y - attackColliderHeight / 2;
                attackCollider.Width = attackColliderWidth;
                attackCollider.Height = attackColliderHeight;
            }
            else
            {
                attackCollider.X = collider.Center.X;
                attackCollider.Y = collider.Center.Y - attackColliderHeight / 2;
                attackCollider.Width = attackColliderWidth;
                attackCollider.Height = attackColliderHeight;
            }
        }
        else if (attackCollider != Rectangle.Empty)
        {
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
            attackCooldownTimer = attackCooldown;
        }
    }

    public void Movement()
    {

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
            //velocity.Y = -speed;
        }
        if (walled.Top > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }
        if (walled.Bottom <= 0 && jumpTime <= 0)
        {
            velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime;
        }

        //previousPos = position;
        position += velocity * mass * Irbis.Irbis.DeltaTime;
    }

    public void CalculateMovement()
    {
        displayRect.X = (int)Position.X;
        displayRect.Y = (int)Position.Y;
        collider.X = (int)Position.X + XcolliderOffset;
        collider.Y = (int)Position.Y + YcolliderOffset;
        collider.Width = colliderWidth;
        collider.Height = colliderHeight;
    }

    public void Animate()
    {                                                        //animator
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (timeSinceLastFrame >= animationSpeed[currentAnimation])
        {
            currentFrame++;
            timeSinceLastFrame -= animationSpeed[currentAnimation];
        }

        if (currentFrame > animationFrames[currentAnimation])
        {
            currentFrame = 0;
            if (animationNoLoop)
            {
                animationNoLoop = false;
                switch (currentAnimation)
                {
                    case 1:
                        currentAnimation = 0;
                        break;
                    case 2:
                        currentAnimation = 0;
                        break;
                    default:
                        //do nothing
                        break;
                }
            }
            if (attacking != Attacking.No)
            {
                attacking = Attacking.No;
            }
        }

        if (attacking != Attacking.No)
        {
            activity = Activity.Attacking;
        }
        else
        {

            //if (input.X != 0 || input.Y != 0)
            //{
            //    if (!inputDown)
            //    {
            //        currentFrame = 0;
            //    }

            //    inputDown = true;
            //}
            //else
            //{
            //    inputDown = false;
            //}

            if (input != Point.Zero)
            {
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
        }
        switch (activity)
        {
            case Activity.Idle:
                currentAnimation = 3;
                break;
            case Activity.Running:
                currentAnimation = 5;
                break;
            case Activity.Jumping:
                currentAnimation = 5;                                                           //normally 7
                break;
            case Activity.Rolling:
                currentAnimation = 5;
                break;
            case Activity.Falling:
                currentAnimation = 5;
                break;
            case Activity.Landing:
                currentAnimation = 5;
                break;
            case Activity.Attacking:
                //Random RAND = new Random();                 //current attack animations are 7 and 9
                //USE GAME.RAND!

                currentAnimation = 7;
                break;
            default:
                currentAnimation = 5;                                                           //run
                break;
        }
        if (direction == Direction.Right)
        {
            currentAnimation++;
        }

        if (previousAnimation != currentAnimation)
        {
            timeSinceLastFrame = 0;
            currentFrame = 0;
        }




        //animationSourceRect = new Rectangle(128 * currentFrame, 128 * currentAnimation, 128, 128);

        animationSourceRect.X = 64 * currentFrame;
        animationSourceRect.Y = 64 * currentAnimation;

        previousAnimation = currentAnimation;
    }

    public void PlayerCollision(Player player, Enemy enemy)
    {
        lock (attackPlayerLock)
        {
            if (player.invulnerable <= 0)
            {
                if (enemy.collider != Rectangle.Empty && player.Collider.Intersects(enemy.collider))
                {
                    if (!player.shielded)
                    {
                        if (player.velocity.X >= 0)
                        {
                            player.velocity.X = -player.hurtVelocity.X;
                            player.velocity.Y = player.hurtVelocity.Y;
                        }
                        else
                        {
                            player.velocity = player.hurtVelocity;
                        }
                    }
                    player.Hurt(20, true);
                    player.invulnerable = player.invulnerableMaxTime;
                }
            }
        }
    }

    public void PlayerAttackCollision()
    {
        Irbis.Irbis.jamie.attackHit = true;
        Hurt(Irbis.Irbis.jamie.attackDamage);
        Stun(0.75f);
        float distanceSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider.Center, Collider.Center);

        if (Irbis.Irbis.jamie.direction == Direction.Left)
        {
            velocity.Y = -25f;
            velocity.X = -0f;
            if (distanceSqr < 1000)
            { velocity.X -= 150; }
            else
            { velocity.X -= (22500 / distanceSqr); }
        }
        else
        {
            velocity.Y = -25f;
            velocity.X = 0f;
            if (distanceSqr < 1000)
            { velocity.X += 150; }
            else
            { velocity.X += (22500 / distanceSqr); }
        }

        foreach (Enchant enchant in Irbis.Irbis.jamie.enchantList)
        { enchant.AddEffect(this); }
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = colliderWidth;
        testCollider.Height = colliderHeight;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && s.Collider != this.collider && Irbis.Irbis.DistanceSquared(collider, s.Collider) <= 0)
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
            velocity.Y +=  -25f * strength;
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
                goto case 2;
            case 2:
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, (position * Irbis.Irbis.screenScale).ToPoint());
                if (attackCollider != Rectangle.Empty) { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, depth + 0.001f); }
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto case 1;
            case 1:
                goto default;
            default:
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    { }
}
