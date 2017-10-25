using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


public class Player
{
    public Wall Walled
    {
        get
        {
            return walled;
        }
    }
    private Wall walled;

    public Rectangle Collider
    {
        get
        {
            return collider;
        }
    }
    private Rectangle collider;

    public Vector2 TrueCenter
    {
        get
        {
            return new Vector2(position.X + colliderOffset.X + (colliderSize.X / 2), position.Y + colliderOffset.Y + (colliderSize.Y / 2));
        }
    }

    public float Mass
    {
        get
        {
            return mass;
        }
    }
    private float mass = 1f;

    public string Name
    {
        get
        {
            return name;
        }
    }
    private string name = "jamie";

    private Wall prevWalled;
    Texture2D tex;
    Texture2D shieldTex;

    public Vector2 baseVelocity;

    public Rectangle displayRect;
    public Rectangle shieldSourceRect = new Rectangle(0, 0, 128, 128);
    public Rectangle animationSourceRect = new Rectangle(0, 0, 128, 128);
    public Rectangle testCollider;
    public Point colliderOffset;
    public Point colliderSize;
    public Vector2 shieldOffset;
    public Print animationFrame;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 maxVelocity;
    public float terminalVelocity;

    public Vector2 hurtVelocity;

    public float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float energy;
    public float maxEnergy;

    public float invulnerable;
    public float invulnerableOnTouch;
    public float invulnerableMaxTime;

    public bool shielded;
    public bool energyed;                                                                       //just for testing
    public float energyUsableMargin;

    public bool attackHit;  //used to determine if the camera should swing or shake after an attack

    public float shieldRechargeRate;
    public float energyRechargeRate;
    public float healthRechargeRate;
    public float potionRechargeRate;
    public float potionRechargeTime;
    public int maxNumberOfPotions;
    public float baseHealing;
    int potions;
    float potionTime;

    public float shieldHealingPercentage;

    public float shockwaveEffectiveDistance;
    public int shockwaveEffectiveDistanceSquared;
    public float shockwaveStunTime;

    public Vector2 shockwaveKnockback;

    public float speed;
    public float airSpeed;
    public float attackMovementSpeed;

    float jumpTime;
    public float jumpTimeMax;
    float timeSinceLastFrame;
    float idleTime;
    public float idleTimeMax;
    float specialTime;
    float specialIdleTime;
    int currentFrame;
    int currentShieldFrame;
    public bool combat;

    public float stunTime;

    public int currentAnimation;
    int previousAnimation;
    public float[] animationSpeed;
    public int[] animationFrames;
    public bool animationNoLoop;
    int nextAnimation = -1;

    public float shieldAnimationSpeed;
    bool shieldDepleted;
    float shieldtimeSinceLastFrame;

    //float enableInput;

    public float depth;
    public float shieldDepth;

    float superShockwave;

    public float superShockwaveHoldtime;
    public float walljumpHoldtime;

    public Point input;
    public Point prevInput;
    public bool isRunning;

    bool frameInput;
    public bool inputEnabled;                                                                          //use this to turn player control on/off

    int climbablePixels;

    //public Vector2 currentLocation;
    public Direction direction;
    public Direction attackDirection;

    public Location location;
    public Activity activity;
    public Activity previousActivity;

    public float rollTime;
    float rollSpeed;
    float rollTimeMax;

    public Attacking attacking;
    public Attacking prevAttacking;
    public float attackDamage;
    public float attack1Damage;
    public float attack2Damage;
    public int attackID;
    public int lastAttackID;
    int attackIDtracker;
    int attackMovementFrames;
    int attackAnimation = 0;
    int prevAttackAnimation = -1;

    public int attackColliderWidth;
    public int attackColliderHeight;

    public float wallJumpTimer;

    public bool attackImmediately;
    public bool interruptAttack;

    public float debugspeed;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    public List<Enchant> enchantList;

    Vector2 amountToMove;
    Vector2 negAmountToMove;
    Vector2 testPos;

    private bool collidedContains;

    public Collided collided;

    public Vector2 heading;

    public Rectangle attackCollider;

    public float collisionCheckDistanceSqr;

    private bool collision;
    private bool noclip;

    private Color shieldedColor = new Color(255, 240, 209);
    private Color normalColor = Color.White;
    private Color renderColor;

    public event Irbis.Irbis.AttackEventDelegate OnPlayerAttack;
    public event Irbis.Irbis.ShockwaveEventDelegate OnPlayerShockwave;



    public Player(Texture2D t, Texture2D t3, PlayerSettings playerSettings, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Player"); }
        displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        
        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.01f);
        currentFrame = 0;
        currentAnimation = 0;

        Load(playerSettings);

        baseVelocity = Vector2.Zero;

        stunTime = 0f;
        isRunning = false;

        walled = Wall.Zero;

        depth = drawDepth;
        shieldDepth = 0.51f;

        heading = Vector2.Zero;
        inputEnabled = true;
        frameInput = false;
        tex = t;
        shieldTex = t3;
        direction = Direction.Right;
        location = Location.Air;
        activity = Activity.Idle;
        prevAttacking = attacking = Attacking.No;

        attackID = attackIDtracker = 0;
        lastAttackID = -1;

        climbablePixels = 3;

        attackImmediately = false;
        interruptAttack = false;

        airSpeed = 0.6f * speed;
        attackMovementSpeed = 0.3f * speed;
        jumpTime = 0;
        idleTime = 0f;
        specialIdleTime = 5f;
        animationNoLoop = false;

        position.X -= colliderOffset.X;
        position.Y -= colliderOffset.Y;

        hurtVelocity = new Vector2(50f, -100f);
        invulnerable = 0f;

        shieldDepleted = false;
        shielded = false;
        energyed = false;

        potionTime = 0f;

        attackMovementFrames = 1;

        collided = new Collided();

        enchantList = new List<Enchant>();

        rollTimeMax = 0.25f;
        rollSpeed = 1500f;
        rollTime = 0f;

        shieldtimeSinceLastFrame = 0f;

        collision = true;
        noclip = false;

        shockwaveEffectiveDistanceSquared = (int)(shockwaveEffectiveDistance * shockwaveEffectiveDistance);

        PlayerEventsReset();
    }
    
    public bool Update()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Update"); }
        prevInput = input;
        prevAttacking = attacking;
        prevWalled = walled;
        input = Point.Zero;
        frameInput = false;

        if (!Irbis.Irbis.GetJumpKey && !Irbis.Irbis.GetLeftKey && !Irbis.Irbis.GetRightKey)
        { wallJumpTimer = 0f; }
        if (inputEnabled)
        {
            if (Irbis.Irbis.GetLeftKey) //left
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X--;
                    direction = Direction.Left;
                }
                wallJumpTimer += Irbis.Irbis.DeltaTime;
            }
            if (Irbis.Irbis.GetRightKey) //right
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X++;
                    direction = Direction.Right;
                }
                wallJumpTimer += Irbis.Irbis.DeltaTime;
            }

            if (direction != Direction.Forward)
            {
                if (Irbis.Irbis.GetUpKey) //up
                { input.Y++; }
                if (Irbis.Irbis.GetDownKey) //down
                { input.Y--; }
                if (Irbis.Irbis.GetShieldKey) //shield
                {
                    if (shield <= 0)
                    {
                        shieldDepleted = true;
                        shielded = false;
                    }
                    else if (!shieldDepleted)
                    { shielded = true; }
                }
                else
                { shieldDepleted = shielded = false; }
                if (Irbis.Irbis.GetShockwaveKey && energy > maxShield * energyUsableMargin)
                //shockwave key held
                { superShockwave += Irbis.Irbis.DeltaTime; }
                else if (superShockwave > 0)
                //activate shockwave
                {
                    Shockwave(this, Irbis.Irbis.enemyList);
                    interruptAttack = true;
                    superShockwave = 0; //shockwave was just used, reset to zero
                }
                if ((Irbis.Irbis.GetPotionKeyDown) && potions > 0) //potions
                {
                    healthRechargeRate += potionRechargeRate;
                    if (potionTime >= potionRechargeTime / 2)
                    { potionTime += potionRechargeTime / 2; }
                    else
                    { potionTime = potionRechargeTime; }
                    potions--;
                    Bars.potionBar.Update(potions);
                }
                if (Irbis.Irbis.GetJumpKey)
                {
                    if (wallJumpTimer <= walljumpHoldtime && walled.Bottom <= 0 && walled.Horizontal && (Irbis.Irbis.GetRightKey || Irbis.Irbis.GetLeftKey))
                    //horizontal input
                    {
                        if (walled.Left > 0)
                        {
                            velocity.X = speed;
                            position.X = position.X + 1;
                            direction = Direction.Right;
                            input.X = prevInput.X = 1;
                        }
                        else if (walled.Right > 0)
                        {
                            velocity.X = -speed;
                            position.X = position.X - 1;
                            direction = Direction.Left;
                            input.X = prevInput.X = -1;
                        }
                        wallJumpTimer = 1f;
                        jumpTime += Irbis.Irbis.DeltaTime;
                        isRunning = true;
                    }
                    else if (walled.Top <= 0 && jumpTime < jumpTimeMax)
                    //normal jump
                    {
                        if (jumpTime > 0)
                        { jumpTime += Irbis.Irbis.DeltaTime; }
                        else if (walled.Bottom > 0)
                        {
                            jumpTime += Irbis.Irbis.DeltaTime;
                            wallJumpTimer = 1f;
                        }
                        wallJumpTimer += Irbis.Irbis.DeltaTime;
                    }
                    else //just jumped, reset to zero
                    { jumpTime = 0; }
                }
                else //just jumped, reset to zero
                { jumpTime = 0; }
                if (Irbis.Irbis.GetJumpKeyDown && walled.Bottom <= 0)
                { interruptAttack = true; /*jump key was just pressed, interrupt an attack(this is here so that attacks in-air are not interrupted)*/}
                if (rollTime <= 0 && walled.Bottom > 0 && (Irbis.Irbis.GetRollKeyDown))
                { // roll
                    inputEnabled = false;
                    rollTime = rollTimeMax;
                }

                if (Irbis.Irbis.GetAttackKeyDown) //&& if attack is interruptable
                { // attack!
                    if (attacking == Attacking.No /*&& walled.Bottom > 0*/)
                    { Attack(Attacking.Attack1); }
                    else //if (attacking != Attacking.No && walled.Bottom > 0)
                    { attackImmediately = true; }
                }
            }
            else //in case the player goes idle while jumping/shielding/shockwaving (somehow)
            {
                shielded = false;
                jumpTime = 0;
                superShockwave = 0;
            }
        }

        if (Irbis.Irbis.GetKeyboardState != Irbis.Irbis.GetPreviousKeyboardState)
        { frameInput = true; }

        if (input != prevInput && input != Point.Zero)
        { interruptAttack = true; }

        if (interruptAttack)
        {
            attacking = Attacking.No;
            attackCollider = Rectangle.Empty;
            interruptAttack = false;
        }


        if (invulnerable > 0)
        { invulnerable -= Irbis.Irbis.DeltaTime; }
        if (invulnerableOnTouch > 0)
        { invulnerableOnTouch -= Irbis.Irbis.DeltaTime; }
        if (stunTime > 0)
        {   stunTime -= Irbis.Irbis.DeltaTime;
            if (stunTime <= 0)
            {   inputEnabled = true;
                stunTime = 0; } }
        if (!shielded)
        {   if (shield < maxShield)
            {   shield += shieldRechargeRate * Irbis.Irbis.DeltaTime;
                if (shield >= maxShield)
                { shield = maxShield; } } }
        if (energy < maxEnergy)
        {   energy += energyRechargeRate * Irbis.Irbis.DeltaTime;
            if (energy >= maxEnergy)
            { energy = maxEnergy; } }
        if (potionTime > 0)
        {   potionTime -= Irbis.Irbis.DeltaTime;
            if (potionTime <= 0)
            { healthRechargeRate = baseHealing; } }
        if (health < maxHealth)
        {   health += healthRechargeRate * Irbis.Irbis.DeltaTime;
            if (health >= maxHealth)
            { health = maxHealth; } }

        Movement();
        CalculateMovement();
        if (collision)
        { Collision(Irbis.Irbis.collisionObjects); }
        Animate();

        return true;
    }

    private bool Player_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack)
    {
        Irbis.Irbis.WriteLine(name + " attack:" + Attack + "\n");
        return true;
    }

    private bool Player_OnPlayerShockwave(Point Origin, int RangeSquared, int Range, float Power)
    {
        Irbis.Irbis.WriteLine(name + " origin:" + Origin + " rangeSquared:" + RangeSquared + " power:" + Power + "\n");
        return true;
    }

    public void PlayerEventsReset()
    {
        OnPlayerAttack = Player_OnPlayerAttack;
        OnPlayerShockwave = Player_OnPlayerShockwave;
    }

    public void Respawn(Vector2 initialPos)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Respawn"); }
        position = initialPos;
        position.X -= colliderOffset.X;
        position.Y -= colliderOffset.Y;
        velocity = Vector2.Zero;
        CalculateMovement();
        health = maxHealth;
        energy = maxEnergy;
        shield = maxShield;
        potions = maxNumberOfPotions;
        if (Bars.potionBar != null) { Bars.potionBar.Update(potions); }
    }

    public void Movement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Movement"); }

        if (noclip)
        {
            if (Irbis.Irbis.GetRollKey)
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * ( speed * 0.1f), movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, input.Y * (-speed * 0.1f), movementLerpSlowdown * Irbis.Irbis.DeltaTime);
            }
            else
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X *  speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, input.Y * -speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
            }
        }
        else
        {
            if (rollTime > 0)
            {
                if (direction == Direction.Right)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, rollSpeed, movementLerpAir * Irbis.Irbis.DeltaTime);
                }
                else
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, -rollSpeed, movementLerpAir * Irbis.Irbis.DeltaTime);
                }
                debugspeed = rollSpeed;

                rollTime -= Irbis.Irbis.DeltaTime;
                if (rollTime <= 0)
                {
                    inputEnabled = true;
                    rollTime = 0;
                }
            }
            else if (attacking != Attacking.No)
            {
                //AttackMovement();
                isRunning = false;
                if (currentFrame <= attackMovementFrames && walled.Bottom > 0)
                {
                    if (direction == Direction.Right)
                    {
                        velocity.X = Irbis.Irbis.Lerp(velocity.X, attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    }
                    else if (direction == Direction.Left)
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

                if (walled.Bottom > 0 && (walled.Left + walled.Right + walled.Top <= 0) && ((direction == Direction.Left && collided.LeftmostBottomCollision > collider.Left) || (direction == Direction.Right && collided.RightmostBottomCollision < collider.Right)))
                {
                    velocity.X = 0;
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

            ///if (Math.Abs(velocity.X) <= 0.000001f)
            ///{
            ///    velocity.X = 0;
            ///}
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

            if (walled.Bottom > 0)
            { baseVelocity = collided.bottomCollided[0].Velocity; }
            else if (walled.Bottom <= 0)
            {
                if (walled.Left > 0 && walled.Right <= 0)
                { baseVelocity = collided.leftCollided[0].Velocity; }
                else if (walled.Right > 0 && walled.Left <= 0)
                { baseVelocity = collided.rightCollided[0].Velocity; }
            }
            if (walled.Total <= 0)
            { baseVelocity = Vector2.Lerp(baseVelocity, Vector2.Zero, 2.0f * Irbis.Irbis.DeltaTime); }
        }


        position += (baseVelocity + velocity) * Irbis.Irbis.DeltaTime;
    }

    public void CalculateMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("CalculateMovement"); }
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        displayRect.X = (int)position.X;
        displayRect.Y = (int)position.Y;
        //collider = new Rectangle((int)position.X + colliderOffset.X, (int)position.Y + colliderOffset.Y, colliderWidth, colliderHeight);
        collider.X = (int)Math.Round((double)position.X) + colliderOffset.X;
        collider.Y = (int)Math.Round((double)position.Y) + colliderOffset.Y;
        collider.Size = colliderSize;
    }

    public void Animate()
    {                                                        //animator
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
                        if (jumpTime > 0)
                        {
                            activity = Activity.Jumping;
                            //jumping
                        }
                        else if (velocity.Y > 0 || activity != Activity.Jumping)
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
                        if (jumpTime > 0)
                        {
                            activity = Activity.Jumping;
                            //jumping
                        }
                        else if (velocity.Y > 0 || activity != Activity.Jumping)
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
                    if (jumpTime > 0)
                    {
                        activity = Activity.Jumping;
                        //jumping
                    }
                    else if (velocity.Y > 0 || activity != Activity.Jumping)
                    {
                        activity = Activity.Falling;
                        //falling
                    }
                }
                else if (prevWalled.Bottom <= 0)
                {
                    activity = Activity.Landing;
                }
                else if (activity != Activity.Landing)
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
                    Attack(Attacking.Attack1);
                }
                else
                {
                    attacking = Attacking.No;
                    attackCollider = Rectangle.Empty;
                }
            }
            if (animationNoLoop)
            {
                switch (currentAnimation)
                {
                    case 0:
                        SetAnimation(1, false);
                        break;
                    case 15:
                        goto case 16;
                    case 16:
                        SetAnimation(3, false);
                        activity = Activity.Idle;
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

        if (previousAnimation != currentAnimation)
        {
            timeSinceLastFrame = 0;
            currentFrame = 0;
        }

        animationSourceRect.X = animationSourceRect.Width * currentFrame;
        animationSourceRect.Y = animationSourceRect.Height * currentAnimation;


        //abilities
        if (shielded)
        {
            shieldtimeSinceLastFrame += Irbis.Irbis.DeltaTime;
            if (shieldtimeSinceLastFrame >= shieldAnimationSpeed)
            {
                shieldtimeSinceLastFrame -= shieldAnimationSpeed;
                currentShieldFrame++;
            }
            if (currentShieldFrame * 128 >= shieldTex.Width)
            {
                currentShieldFrame = 0;
            }
            //shieldSourceRect = new Rectangle(currentShieldFrame * 128, 0, 128, 128);
            shieldSourceRect.X = currentShieldFrame * 128;
            renderColor = shieldedColor;
        }
        else
        {
            shieldtimeSinceLastFrame = 0;
            renderColor = normalColor;
        }
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
                    { SetAnimation(1, false); }
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
                        switch (Irbis.Irbis.RandomInt(2))
                        {
                            case 1:
                                SetAnimation(7, true);
                                break;
                            default:
                                SetAnimation(5, true);
                                break;
                        }
                    }
                    else
                    { SetAnimation(3, false); }
                }
                if (frameInput || combat)
                { idleTime = 0; }
                //else
                //{ /*idleTime += Irbis.Irbis.DeltaTime;*/ }

                break;
            case Activity.Running:
                SetAnimation(9, false);
                break;
            case Activity.Jumping:
                SetAnimation(11, false);
                break;
            case Activity.Falling:
                SetAnimation(13, false);
                break;
            case Activity.Landing:
                SetAnimation(15, true);
                break;
            case Activity.Rolling:
                SetAnimation(17, false);
                break;
            case Activity.Attacking:
                while (attackAnimation == prevAttackAnimation)
                { attackAnimation = Irbis.Irbis.RandomInt(3); }
                switch (attackAnimation)
                {
                    case 1:
                        SetAnimation(19, true);
                        break;
                    case 2:
                        SetAnimation(21, true);
                        break;
                    default:
                        SetAnimation(17, true);
                        break;
                }
                prevAttackAnimation = attackAnimation;
                break;
            default:
                SetAnimation(9, false);                                                           //run
                break;
        }

        if (nextAnimation >= 0)
        { SetAnimation(nextAnimation, false); }
    }

    public void SetAnimation(int animation, bool noLoop)
    {
        currentAnimation = animation;
        currentFrame = 0;
        nextAnimation = -1;
        animationNoLoop = noLoop;
        if (direction == Direction.Right)
        { currentAnimation++; }
    }

    public void WalljumpDebug(int variableYo)
    {
        Irbis.Irbis.WriteLine();
        Irbis.Irbis.WriteLine("       walljumped " + variableYo);
        Irbis.Irbis.WriteLine("         velocity: " + velocity);
        Irbis.Irbis.WriteLine("        direction: " + direction);
        Irbis.Irbis.WriteLine("wallJumpTimer: " + wallJumpTimer);
        Irbis.Irbis.WriteLine("            input: " + input);
        Irbis.Irbis.WriteLine("        prevInput: " + prevInput);
        Irbis.Irbis.WriteLine("           Walled: " + Walled);
        Irbis.Irbis.WriteLine("         position: " + position);
        Irbis.Irbis.WriteLine("         left key: " + Irbis.Irbis.GetLeftKey);
        Irbis.Irbis.WriteLine("        right key: " + Irbis.Irbis.GetRightKey);
    }

    public void Noclip()
    {
        noclip = !noclip;
        collision = !noclip;
        velocity = Vector2.Zero;
        walled = Wall.Zero;
        collided = new Collided();
        if (noclip)
        { speed *= 25; }
        else
        { speed /= 25; }
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = colliderSize.X;
        testCollider.Height = colliderSize.Y;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && s.Collider != this.collider && Irbis.Irbis.DistanceSquared(collider, s.Collider) <= 0)
            {
                collidedContains = collided.Contains(s);
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Bottom))                              //DOWN
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Bottom);
                        walled.Bottom++;
                        if (negAmountToMove.Y > s.Collider.Top - collider.Bottom && (velocity.Y * Irbis.Irbis.DeltaTime) >= -(s.Collider.Top - collider.Bottom))
                        { negAmountToMove.Y = s.Collider.Top - collider.Bottom; }
                    }
                    else if (negAmountToMove.Y > s.Collider.Top - collider.Bottom)
                    { negAmountToMove.Y = s.Collider.Top - collider.Bottom; }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Right))                               //RIGHT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Right);
                        walled.Right++;
                        if (negAmountToMove.X > s.Collider.Left - collider.Right && (velocity.X * Irbis.Irbis.DeltaTime) >= -(s.Collider.Left - collider.Right))
                        { negAmountToMove.X = s.Collider.Left - collider.Right; }
                    }
                    else if (negAmountToMove.X > s.Collider.Left - collider.Right)
                    { negAmountToMove.X = s.Collider.Left - collider.Right; }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Left))                                //LEFT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Left);
                        walled.Left++;
                        if (amountToMove.X < s.Collider.Right - collider.Left && (velocity.X * Irbis.Irbis.DeltaTime) <= -(s.Collider.Right - collider.Left))
                        { amountToMove.X = s.Collider.Right - collider.Left; }
                    }
                    else if (amountToMove.X < s.Collider.Right - collider.Left)
                    { amountToMove.X = s.Collider.Right - collider.Left; }
                }
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.Top))                                 //UP
                {
                    if (!collidedContains)
                    {
                        collided.Add(s, Side.Top);
                        walled.Top++;
                        if (amountToMove.Y < s.Collider.Bottom - collider.Top && (velocity.Y * Irbis.Irbis.DeltaTime) <= -(s.Collider.Bottom - collider.Top))
                        { amountToMove.Y = s.Collider.Bottom - collider.Top; }
                    }
                    else if (amountToMove.Y < s.Collider.Bottom - collider.Top)
                    { amountToMove.Y = s.Collider.Bottom - collider.Top; }
                }
            }
        }

        if (walled.Left == 1 && input.X < 0)
        {
            int climbamount = (collider.Bottom - collided.leftCollided[0].Collider.Top);
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
            int climbamount = (collider.Bottom - collided.rightCollided[0].Collider.Top);
            if ((climbamount) <= climbablePixels)
            {
                position.Y -= climbamount;
                //position.X += 1;
                amountToMove = negAmountToMove = Vector2.Zero;
                Irbis.Irbis.WriteLine(this + " on ramp, moved " + climbamount + " pixels. Timer:" + Irbis.Irbis.Timer);
            }
        }


        if (amountToMove.X == 0)
        { amountToMove.X = negAmountToMove.X; }
        else if (negAmountToMove.X != 0 && -negAmountToMove.X < amountToMove.X)
        { amountToMove.X = negAmountToMove.X; }

        if (amountToMove.Y == 0)
        { amountToMove.Y = negAmountToMove.Y; }
        else if (negAmountToMove.Y != 0 && -negAmountToMove.Y < amountToMove.Y)
        { amountToMove.Y = negAmountToMove.Y; }

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
            { amountToMove.X = 0; }
            else if (X)
            { amountToMove.Y = 0; }
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
                { amountToMove.Y = 0; }
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
                { amountToMove.X = 0; }
            }
        }

        if (amountToMove != Vector2.Zero)
        { Irbis.Irbis.WriteLine("    amountToMove: " + amountToMove); }

        position += amountToMove;
        CalculateMovement();

        for (int i = 0; i < collided.bottomCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.bottomCollided[i].Collider, Side.Bottom))
            {
                collided.bottomCollided.RemoveAt(i);
                walled.Bottom--;
                i--;
            }
        }
        for (int i = 0; i < collided.rightCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.rightCollided[i].Collider, Side.Right))
            {
                collided.rightCollided.RemoveAt(i);
                walled.Right--;
                i--;
            }
        }
        for (int i = 0; i < collided.leftCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.leftCollided[i].Collider, Side.Left))
            {
                collided.leftCollided.RemoveAt(i);
                walled.Left--;
                i--;
            }
        }
        for (int i = 0; i < collided.topCollided.Count; i++)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.topCollided[i].Collider, Side.Top))
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
        {
            if (attacking != Attacking.No)
            { velocity.Y += (Irbis.Irbis.gravity / 2) * mass * Irbis.Irbis.DeltaTime; }
            else
            { velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime; }
        }
    }

    public bool RemoveCollision(ICollisionObject collisionObject)
    {
        if (collided.Remove(collisionObject, Side.Bottom))
        { walled.Bottom--; }
        if (collided.Remove(collisionObject, Side.Right ))
        { walled.Right--; }
        if (collided.Remove(collisionObject, Side.Left  ))
        { walled.Left--; }
        if (collided.Remove(collisionObject, Side.Top   ))
        { walled.Top--; }
        return true;
    }

    /// <summary>
    /// returns true if the player took damage
    /// </summary>
    public bool Hurt(float damage)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Hurt"); }
        if (invulnerable <= 0)
        {
            if (shielded)
            {
                if (damage > shield)
                {
                    Heal(shield * shieldHealingPercentage);
                    damage -= shield;
                    shield = 0;
                    health -= damage;
                }
                else
                {
                    Heal(damage * shieldHealingPercentage);
                    shield -= damage;
                }
            }
            else
            {
                health -= damage;
            }
            invulnerable = invulnerableMaxTime;
            return true;
        }
        return false;
    }

    /// <summary>
    /// returns true if knockback should trigger
    /// </summary>
    public bool HurtOnTouch(float damage)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Hurt"); }
        if (invulnerableOnTouch <= 0 && invulnerable <= 0)
        {
            invulnerableOnTouch = 1;
            if (shielded)
            {
                if (damage > shield)
                {
                    Heal(shield * shieldHealingPercentage);
                    damage -= shield;
                    shield = 0;
                    health -= damage;
                    return true;
                }
                else
                {
                    Heal(damage * shieldHealingPercentage);
                    shield -= damage;
                    return false;
                }
            }
            else
            {
                health -= damage;
                return true;
            }
        }
        return false;
    }

    public void Heal(float amount)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Heal"); }
        if (health + amount > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += amount;
        }
    }

    public void Shockwave(Player player, List<IEnemy> enemyList)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Shockwave"); }
        //energyed = true;
        //activate shockwave animation
        //stun for duration of animation
        Stun(0.25f);        //the time it takes to "cast" shockwave
        if (superShockwave < superShockwaveHoldtime)
        {
            energy -= 30;
            Irbis.Irbis.CameraShake(0.1f, 5f);
            OnPlayerShockwave(collider.Center, shockwaveEffectiveDistanceSquared, (int)shockwaveEffectiveDistance, 1);
        }
        else
        {
            energy -= 50;
            Irbis.Irbis.CameraShake(0.15f, 10f);
            OnPlayerShockwave(collider.Center, shockwaveEffectiveDistanceSquared, (int)shockwaveEffectiveDistance, 2);
        }
        superShockwave = 0;
    }

    public void ClearCollision()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("ClearCollision"); }
        collided = new Collided();
        walled = Wall.Zero;
    }

    public void Attack(Attacking attack)
    {
        //change attackCollider based on if (attacking) and current animation
        attacking = attack;
        switch (attack)
        {
            case (Attacking.Attack1):
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
                OnPlayerAttack(attackCollider, Attacking.Attack1);
                break;
            //case (Attacking.Attack2):
            //    break;
            default:
                //attackCollider = Rectangle.Empty;
                attackDamage = 0f;
                break;
        }
        //else if (attackCollider != Rectangle.Empty)
        //{
        //    if (attackHit)
        //    {
        //        Irbis.Irbis.CameraShake(0.075f, 0.1f * attackDamage);
        //    }
        //    else
        //    {
        //        if (direction == Direction.Left)
        //        {
        //            Irbis.Irbis.CameraSwing(Irbis.Irbis.swingDuration, Irbis.Irbis.swingMagnitude, new Vector2(-5, -Irbis.Irbis.RandomFloat)); //change the Y based on which attack animation is playing
        //        }
        //        else
        //        {
        //            Irbis.Irbis.CameraSwing(Irbis.Irbis.swingDuration, Irbis.Irbis.swingMagnitude, new Vector2(5, -Irbis.Irbis.RandomFloat));
        //        }
        //    }
        //    attackCollider = Rectangle.Empty;
        //    attackDamage = 0f;
        //}
        //else
        //{
        //    attackCollider = Rectangle.Empty;
        //    attackDamage = 0f;
        //}
    }

    public void AttackMovement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("AttackMovement"); }

    }

    public void Stun(float duration)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Stun"); }
        stunTime += duration;
        inputEnabled = false;
    }

    public void Load(PlayerSettings playerSettings)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Load"); }
        attack1Damage = playerSettings.attack1Damage;
        attack2Damage = playerSettings.attack2Damage;
        speed = playerSettings.speed;
        jumpTimeMax = playerSettings.jumpTimeMax;
        idleTimeMax = playerSettings.idleTimeMax;
        colliderOffset = playerSettings.colliderOffset;
        colliderSize = playerSettings.colliderSize;
        shieldOffset = new Vector2((colliderOffset.X + (colliderSize.X / 2f)) - (shieldSourceRect.Width / 2f), (colliderOffset.Y + (colliderSize.Y / 2f)) - (shieldSourceRect.Height / 2f));
        attackColliderWidth = playerSettings.attackColliderWidth;
        attackColliderHeight = playerSettings.attackColliderHeight;
        health = maxHealth = playerSettings.maxHealth;
        shield = maxShield = playerSettings.maxShield;
        energy = maxEnergy = playerSettings.maxEnergy;
        superShockwaveHoldtime = playerSettings.superShockwaveHoldtime;
        walljumpHoldtime = playerSettings.walljumpHoldtime;
        shockwaveEffectiveDistance = playerSettings.shockwaveEffectiveDistance;
        shockwaveStunTime = playerSettings.shockwaveStunTime;
        shockwaveKnockback = playerSettings.shockwaveKnockback;
        invulnerableMaxTime = playerSettings.invulnerableMaxTime;
        shieldRechargeRate = playerSettings.shieldRechargeRate;                    //2f //4f
        energyRechargeRate = playerSettings.energyRechargeRate;                    //5f //10f
        baseHealing = healthRechargeRate = playerSettings.healthRechargeRate;
        potionRechargeRate = playerSettings.potionRechargeRate;
        potions = maxNumberOfPotions = playerSettings.maxNumberOfPotions;
        potionRechargeTime = playerSettings.potionRechargeTime;
        shieldHealingPercentage = playerSettings.shieldHealingPercentage;
        energyUsableMargin = playerSettings.energyUsableMargin;
        terminalVelocity = playerSettings.terminalVelocity;
        animationSpeed = playerSettings.animationSpeed;
        shieldAnimationSpeed = playerSettings.shieldAnimationSpeed;
        animationFrames = playerSettings.animationFrames;
    }

    public void Combat()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Combat"); }
        combat = true;
        if (direction == Direction.Forward)
        {
            direction = Direction.Right;
            idleTime = 0f;
        }
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
                goto case 1;
            case 1:
                if (attackCollider != Rectangle.Empty) { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, true); }
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, ((position + (colliderOffset - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                goto default;
            default:
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, renderColor, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                if (shielded)
                { sb.Draw(shieldTex, (position + shieldOffset) * Irbis.Irbis.screenScale, shieldSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth + 0.001f); }
                break;
        }
    }
}
