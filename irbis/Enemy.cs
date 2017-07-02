using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



public class Enemy : ICollisionObject, IEnemy
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

    public RectangleBorder colliderDrawer;

    //public Rectangle edgeCollider;
    //public Vector2 previousPos;
    public Vector2 velocity;


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
    public int rightWalled;
    public int leftWalled;
    public int topWalled;
    public int bottomWalled;
    //public Vector2 currentLocation;

    public float shockwaveEffectiveDistance;
    public float shockwaveMaxEffectDistance;
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

    public bool AIenabled;

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
        
    public List<ICollisionObject> collided;
    List<Side> sideCollided;
    Irbis.Irbis game;

    public Enemy(Texture2D t, Vector2 iPos, float enemyHealth, float enemyDamage, float enemySpeed, Irbis.Irbis masterGame)
	{
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Enemy"); }
        game = masterGame;

        tex = t;
        colliderDrawer = new RectangleBorder(collider, Color.Magenta, 0.19f);

        AIenabled = true;

        climbablePixels = 3;

        position = iPos;
        direction = Direction.forward;
        location = Location.air;
        activity = Activity.idle;
        AIactivity = AI.wander;
        wanderSpeed = (2f/3f) * enemySpeed;
        wanderTime = 0f;
        previouslyWandered = false;

        speed = defaultSpeed = enemySpeed;
        jumpTime = 0;
        jumpTimeMax = 0.06f;
        animationNoLoop = false;
        XcolliderOffset = 44;
        YcolliderOffset = 38;
        colliderWidth = 38;
        colliderHeight = 86;

        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;

        maxHealth = health = enemyHealth;
        lastHitByAttackID = -1;

        stunned = 0;
        speedModifier = 1f;

        attackCollider = Rectangle.Empty;

        attackColliderWidth = 60;
        attackColliderHeight = 60;

        attackID = attackIDtracker = 0;
        lastAttackID = -1;

        attackDamage = 0f;
        attack1Damage = enemyDamage;

        attackCooldown = 2f;        //how quickly enemies can attack
        attackCooldownTimer = 3f;

        combatCheckDistanceSqr = attackColliderWidth * attackColliderWidth;
        persueCheckDistanceSqr = 160000;
        combat = false;

        activeEffects = new List<Enchant>();

        shockwaveMaxEffectDistance = Irbis.Irbis.geralt.shockwaveMaxEffectDistance;
        shockwaveEffectiveDistance = Irbis.Irbis.geralt.shockwaveEffectiveDistance;
        shockwaveStunTime = Irbis.Irbis.geralt.shockwaveStunTime;
        shockwaveKnockback = Irbis.Irbis.geralt.shockwaveKnockback;


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

        if (colliderHeight > colliderWidth)
        {
            collisionCheckDistanceSqr = (colliderHeight * colliderHeight);
        }
        else
        {
            collisionCheckDistanceSqr = (colliderWidth * colliderWidth);
        }
    }

    public void Update()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Update"); }
        if (stunned <= 0)
        {
            if (jumpTime > 0)
            {
                velocity.Y = -500f;
                jumpTime -= Irbis.Irbis.DeltaTime;
                if (jumpTime <= 0) { jumpTime = 0; }
            }
            distanceToPlayerSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.geralt.collider.Center, this.Collider.Center);
            if (distanceToPlayerSqr <= combatCheckDistanceSqr)
            {
                AIactivity = AI.combat;
            }
            else if (distanceToPlayerSqr <= persueCheckDistanceSqr)
            {
                AIactivity = AI.persue;
            }
            else
            {
                AIactivity = AI.wander;
            }
            attackCooldownTimer -= Irbis.Irbis.DeltaTime;

            if (AIenabled)
            {
                switch (AIactivity)
                {
                    case AI.combat:
                        Combat(Irbis.Irbis.geralt);
                        break;
                    case AI.persue:
                        Persue(Irbis.Irbis.geralt);
                        break;
                    case AI.wander:
                        Wander();
                        break;
                    default:
                        Wander();
                        break;
                }
            }
            //PlayerCollision(game.geralt, this);
        }
        else
        {
            stunned -= Irbis.Irbis.DeltaTime;
            if (bottomWalled > 0 && Math.Abs(velocity.X) > 0)
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpBuildup * Irbis.Irbis.DeltaTime);
            }
            if (stunned <= 0) { stunned = 0; }
        }

        for (int i = 0; i < activeEffects.Count; i ++)
        {
            if (activeEffects[i].ApplyEffect(this))
            {
                activeEffects.RemoveAt(i);
                i--;
            }
        }

        PlayerAttackCollision();
        Movement();
        CalculateMovement();
        Animate();
        Collision(this, Irbis.Irbis.squareList);
    }

    public void Respawn(Vector2 initialPos)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Respawn"); }
        position = initialPos * 32;
        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;
        velocity = Vector2.Zero;
        health = maxHealth;
        CalculateMovement();
    }

    public void Wander()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Wander"); }


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
                if (rightWalled <= 0 && leftWalled <= 0)
                {
                    if (Irbis.Irbis.RandomFloat() > 0.5)
                    {
                        input.X--;
                        direction = Direction.left;
                    }
                    else
                    {
                        input.X++;
                        direction = Direction.right;
                    }
                }
                else if (rightWalled > 0 && rightWalled <= 1)
                {
                    if (rightWalled > 0)
                    {
                        input.X++;
                        direction = Direction.right;
                        jumpTime = jumpTimeMax;
                    }
                    else
                    {
                        input.X--;
                        direction = Direction.left;
                    }
                }
                else if (leftWalled > 0 && leftWalled <= 1)
                {
                    if (leftWalled > 0)
                    {
                        input.X--;
                        direction = Direction.left;
                        jumpTime = jumpTimeMax;
                    }
                    else
                    {
                        input.X++;
                        direction = Direction.right;
                    }
                }
                
                previouslyWandered = true;
            }
            
            wanderTime = Irbis.Irbis.RandomFloat() + 0.5f;
        }


        if (bottomWalled > 0)                                                               //movement
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * wanderSpeed * speedModifier, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        }
        else if (velocity.X == 0)
        {
            velocity.X = input.X * 0.1f * speed;
        }

    }

    public void Persue(Player player)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Persue"); }
        input = Point.Zero;

        player.heading = (player.collider.Center - collider.Center).ToVector2();
        //player.heading.Normalize();

        if (player.heading.X > 0)
        {
            if (rightWalled <= 0)
            {
                input.X++;
                direction = Direction.right;
            }
            else if (bottomWalled > 0)
            {
                input.X++;
                direction = Direction.right;
                jumpTime = jumpTimeMax;
            }
        }
        else if (player.heading.X < 0)
        {
            if (leftWalled <= 0)
            {
                input.X--;
                direction = Direction.left;
            }
            else if (bottomWalled > 0)
            {
                input.X--;
                direction = Direction.left;
                jumpTime = jumpTimeMax;
            }
        }

        if (bottomWalled > 0)                                                               //movement
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed * speedModifier, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        }
        else if (velocity.X == 0)
        {
            velocity.X = input.X * 0.1f * speed;
        }
    }

    public void Combat(Player player)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Combat"); }
        velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpBuildup * Irbis.Irbis.DeltaTime);
        jumpTime = 0;

        if (attackCooldownTimer <= 0)
        {
            attackID = attackIDtracker++;
            attackCooldownTimer = attackCooldown;
            attacking = Attacking.attack1;
        }
        
        if (attacking != Attacking.no)
        {
            Hitbox();
            if (attackCollider.Intersects(player.collider))
            {
                player.Hurt(attackDamage);
            }
        }
        else
        {
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
            attackID = 0;
        }
    }

    public void Hitbox()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Hitbox"); }
        //change attackCollider based on if (attacking) and current animation
        if (lastAttackID != attackID)
        {
            lastAttackID = attackID;
            attackDamage = attack1Damage;

            if (direction == Direction.left)
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Movement"); }

        if (rightWalled > 0 && velocity.X > 0)
        {
            velocity.X = 0;
        }
        if (leftWalled > 0 && velocity.X < 0)
        {
            velocity.X = 0;
        }


        if (jumpTime > 0)
        {
            //velocity.Y = -speed;
        }
        if (topWalled > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }
        if (bottomWalled <= 0 && jumpTime <= 0)
        {
            velocity.Y += Irbis.Irbis.gravity * Irbis.Irbis.DeltaTime;
        }

        //previousPos = position;
        position += velocity * Irbis.Irbis.DeltaTime;
    }

    public void CalculateMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.CalculateMovement"); }
        displayRect.X = (int)Position.X;
        displayRect.Y = (int)Position.Y;
        collider.X = (int)Position.X + XcolliderOffset;
        collider.Y = (int)Position.Y + YcolliderOffset;
        collider.Width = colliderWidth;
        collider.Height = colliderHeight;
    }

    public void Animate()
    {                                                        //animator
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Animate"); }
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
            if (attacking != Attacking.no)
            {
                attacking = Attacking.no;
            }
        }

        if (attacking != Attacking.no)
        {
            activity = Activity.attacking;
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
                    if (bottomWalled > 0)
                    {
                        activity = Activity.running;
                    }
                    else
                    {
                        if (velocity.Y < 0)
                        {
                            activity = Activity.jumping;
                            //jumping
                        }
                        else
                        {
                            activity = Activity.falling;
                            //falling
                        }
                    }
                }
                else
                {
                    if (bottomWalled <= 0)
                    {
                        if (velocity.Y < 0)
                        {
                            activity = Activity.jumping;
                            //jumping
                        }
                        else
                        {
                            activity = Activity.falling;
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
                if (bottomWalled <= 0)
                {
                    if (velocity.Y < 0)
                    {
                        activity = Activity.jumping;
                        //jumping
                    }
                    else
                    {
                        activity = Activity.falling;
                        //falling
                    }
                }
                else
                {
                    activity = Activity.idle;
                }
            }
        }
        switch (activity)
        {
            case Activity.idle:
                currentAnimation = 3;
                break;
            case Activity.running:
                currentAnimation = 5;
                break;
            case Activity.jumping:
                currentAnimation = 5;                                                           //normally 7
                break;
            case Activity.rolling:
                currentAnimation = 5;
                break;
            case Activity.falling:
                currentAnimation = 5;
                break;
            case Activity.landing:
                currentAnimation = 5;
                break;
            case Activity.attacking:
                //Random RAND = new Random();                 //current attack animations are 7 and 9
                //USE GAME.RAND!

                currentAnimation = 7;
                break;
            default:
                currentAnimation = 5;                                                           //run
                break;
        }
        if (direction == Direction.right)
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.PlayerCollision"); }
        if (player.invulnerableTime <= 0)
        {
            if (enemy.collider != Rectangle.Empty && player.collider.Intersects(enemy.collider))
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
                player.Hurt(20);
                player.invulnerableTime = player.invulnerableMaxTime;
            }
        }
    }

    public void PlayerAttackCollision()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.PlayerAttackCollision"); }
        if (Irbis.Irbis.geralt.attackID != lastHitByAttackID)
        {
            if (Irbis.Irbis.geralt.attackCollider != Rectangle.Empty && Irbis.Irbis.geralt.attackCollider.Intersects(collider))
            {
                lastHitByAttackID = Irbis.Irbis.geralt.attackID;
                Irbis.Irbis.geralt.attackHit = true;
                Hurt(Irbis.Irbis.geralt.attackDamage);
                Stun(0.75f);
                float distanceSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.geralt.collider.Center, Collider.Center);

                if (Irbis.Irbis.geralt.direction == Direction.left)
                {
                    velocity.Y = -25f;
                    velocity.X =  -0f;
                    if (distanceSqr < 2000)
                    {
                        velocity.X -= 250;
                    }
                    else
                    {
                        velocity.X -= (500000 / distanceSqr);
                    }
                }
                else
                {
                    velocity.Y = -25f;
                    velocity.X =   0f;
                    if (distanceSqr < 2000)
                    {
                        velocity.X += 250;
                    }
                    else
                    {
                        velocity.X += (500000 / distanceSqr);
                    }
                }

                foreach (Enchant enchant in Irbis.Irbis.geralt.enchantList)
                {
                    enchant.AddEffect(this);
                }
            }
        }
    }

    public void Collision(Enemy player, List<Square> colliderList)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Collision"); }
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = player.colliderWidth;
        testCollider.Height = player.colliderHeight;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && Irbis.Irbis.DistanceSquared(collider.Center, s.Collider.Center) < collisionCheckDistanceSqr && !collided.Contains(s))
            {
                if (Irbis.Irbis.IsTouching(player.collider, s.Collider, Side.bottom))                              //DOWN
                {
                    collided.Add(s);
                    sideCollided.Add(Side.bottom);
                    player.bottomWalled++;
                    if (negAmountToMove.Y > s.Collider.Top - player.collider.Bottom && (player.velocity.Y * Irbis.Irbis.DeltaTime) >= -(s.Collider.Top - player.collider.Bottom))
                    {
                        negAmountToMove.Y = s.Collider.Top - player.collider.Bottom;
                    }
                }
                if (Irbis.Irbis.IsTouching(player.collider, s.Collider, Side.right))                               //RIGHT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.right);
                    player.rightWalled++;
                    if (negAmountToMove.X > s.Collider.Left - player.collider.Right && (player.velocity.X * Irbis.Irbis.DeltaTime) >= -(s.Collider.Left - player.collider.Right))
                    {
                        negAmountToMove.X = s.Collider.Left - player.collider.Right;
                    }
                }
                if (Irbis.Irbis.IsTouching(player.collider, s.Collider, Side.left))                                //LEFT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.left);
                    player.leftWalled++;
                    if (amountToMove.X < s.Collider.Right - player.collider.Left && (player.velocity.X * Irbis.Irbis.DeltaTime) <= -(s.Collider.Right - player.collider.Left))
                    {
                        amountToMove.X = s.Collider.Right - player.collider.Left;
                    }
                }
                if (Irbis.Irbis.IsTouching(player.collider, s.Collider, Side.top))                                 //UP
                {
                    collided.Add(s);
                    sideCollided.Add(Side.top);
                    player.topWalled++;
                    if (amountToMove.Y < s.Collider.Bottom - player.collider.Top && (player.velocity.Y * Irbis.Irbis.DeltaTime) <= -(s.Collider.Bottom - player.collider.Top))
                    {
                        amountToMove.Y = s.Collider.Bottom - player.collider.Top;
                    }
                }
            }
        }

        if (leftWalled == 1 && input.X < 0)
        {
            int climbamount = (player.collider.Bottom - collided[sideCollided.IndexOf(Side.left)].Collider.Top);
            if ((climbamount) <= climbablePixels)
            {
                position.Y -= climbamount;
                //position.X -= 1;
                amountToMove = negAmountToMove = Vector2.Zero;
                Irbis.Irbis.WriteLine("on ramp, moved " + climbamount + " pixels");
            }
        }
        if (rightWalled == 1 && input.X > 0)
        {
            int climbamount = (player.collider.Bottom - collided[sideCollided.IndexOf(Side.right)].Collider.Top);
            if ((climbamount) <= climbablePixels)
            {
                position.Y -= climbamount;
                //position.X += 1;
                amountToMove = negAmountToMove = Vector2.Zero;
                Irbis.Irbis.WriteLine("on ramp, moved " + climbamount + " pixels");
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
            testPos.Y = (int)player.position.Y;
            testPos.X = player.position.X;
            testPos.Y += amountToMove.Y;
            Y = true;
        }
        else if (amountToMove.X != 0)
        {
            testPos.X = (int)player.position.X;
            testPos.Y = player.position.Y;
            testPos.X += amountToMove.X;
            X = true;
        }

        bool pass = true;
        testCollider.X = (int)testPos.X + player.XcolliderOffset;
        testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

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
                Irbis.Irbis.WriteLine("    velocity: " + player.velocity);
                Irbis.Irbis.WriteLine("  player.position: " + player.position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
                Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                foreach (ICollisionObject s in collided)
                {
                    Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right);
                }
                Irbis.Irbis.WriteLine("afte1--");
            }

            pass = true;
            if (Y)
            {
                testPos.X = (int)player.position.X;
                testPos.Y = player.position.Y;
                testPos.X += amountToMove.X;
                testCollider.X = (int)testPos.X + player.XcolliderOffset;
                testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

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
                testPos.Y = (int)player.position.Y;
                testPos.X = player.position.X;
                testPos.Y += amountToMove.Y;
                testCollider.X = (int)testPos.X + player.XcolliderOffset;
                testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

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
                Irbis.Irbis.WriteLine("    velocity: " + player.velocity);
                Irbis.Irbis.WriteLine("  player.position: " + player.position);
                Irbis.Irbis.WriteLine("     testPos: " + testPos);
                Irbis.Irbis.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
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
            Irbis.Irbis.WriteLine("    velocity: " + player.velocity);
        }

        if (amountToMove.X > 0 && player.velocity.X < 0)
        {
            player.velocity.X = 0;
        }
        if (amountToMove.X < 0 && player.velocity.X > 0)
        {
            player.velocity.X = 0;
        }
        if (amountToMove.Y > 0 && player.velocity.Y < 0)
        {
            player.velocity.Y = 0;
        }
        if (amountToMove.Y < 0 && player.velocity.Y > 0)
        {
            player.velocity.Y = 0;
        }

        if (amountToMove.X != 0)
        {
            player.position.X = (int)player.position.X;
        }
        if (amountToMove.Y != 0)
        {
            player.position.Y = (int)player.position.Y;
        }
        player.position += amountToMove;

        player.CalculateMovement();
        for (int i = 0; i < collided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(player.collider, collided[i].Collider, sideCollided[i]))
            {
                switch (sideCollided[i])
                {
                    case Side.bottom:
                        player.bottomWalled--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.right:
                        player.rightWalled--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.left:
                        player.leftWalled--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.top:
                        player.topWalled--;
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
            Irbis.Irbis.WriteLine("    velocity: " + player.velocity);
            Irbis.Irbis.WriteLine("  player.position: " + player.position);
            Irbis.Irbis.WriteLine("     testPos: " + testPos);
            Irbis.Irbis.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
            Irbis.Irbis.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
            foreach (ICollisionObject s in collided)
            {
                Irbis.Irbis.WriteLine("   scollider: T:" + s.Collider.Top + " B:" + s.Collider.Bottom + " L:" + s.Collider.Left + " R:" + s.Collider.Right);
            }
            Irbis.Irbis.WriteLine("done--");
            Irbis.Irbis.WriteLine();
        }

        if (bottomWalled > 0 && player.velocity.Y > 0)
        {
            player.velocity.Y = 0;
        }
    }

    public void AddEffect(Enchant effect)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.AddEffect"); }
        activeEffects.Add(effect);
    }

    public void UpgradeEffect(int index, float duration)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.UpgradeEffect"); }
        if (activeEffects.Count > index)
        {
            activeEffects[index].strength++;
            activeEffects[index].effectDuration = duration;
        }
    }

    public void Knockback(Direction knockbackDirection, float strength)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Knockback"); }
        if (knockbackDirection == Direction.left)
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Hurt"); }
        health -= damage;
        //game.CameraShake(0.075f, 0.05f * damage);
    }

    public void Stun(float duration)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Stun"); }
        stunned += duration;
        AIactivity = AI.stunned;
        attackCooldownTimer += 0.5f;

    }

    public void Shockwave(float distance, float power, Vector2 heading)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Shockwave"); }
        Stun(((shockwaveEffectiveDistance - distance) * 2) / shockwaveStunTime);
        velocity = shockwaveKnockback * heading * (shockwaveEffectiveDistance - distance) * power;
    }

    public void Draw(SpriteBatch sb)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Enemy.Draw"); }
        if (Irbis.Irbis.IsTouching(displayRect, Irbis.Irbis.screenspace))
        {
            sb.Draw(tex, displayRect, animationSourceRect, Color.White);
        }
    }
}
