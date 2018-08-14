using Irbis;
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


public class Player : ICollisionObject
{
    public Wall Walled
    {
        get
        { return walled; }
    }
    private Wall walled;

    public Rectangle Collider
    {
        get
        { return collider; }
    }
    private Rectangle collider;
    public Vector2 origin = new Vector2(64,87);

    public Vector2 TrueCenter
    {
        get
        { return position; }//new Vector2(position.X + standardCollider.X + (standardCollider.Width / 2f), position.Y + standardCollider.Y + (standardCollider.Height / 2f)); }
    }
    public Vector2 BottomCenter // used for grass
    {
        get
        { return new Vector2(position.X + standardCollider.X + (standardCollider.Width / 2f), position.Y + standardCollider.Y + (standardCollider.Height)); }
    }
    public Vector2 Velocity
    {
        get
        { return velocity; }
    }

    Rectangle CorrectCollider
    {
        get
        {
            if (rollTime > 0)
            { return rollCollider; }
            else
            { return standardCollider; }
        }
    }

    public float Health
    {
        get
        { return health; }
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

    public bool Jumpable
    {
        get
        { return jumpable > 0; }
    }

    public int CurrentFrame
    {
        get
        { return currentFrame; }
    }

    private Wall prevWalled;
    Texture2D tex;
    Texture2D shieldTex;

    public Rectangle playerLight;
    public Rectangle shieldLight;

    public Vector2 baseVelocity;

    public Rectangle shieldSourceRect = new Rectangle(0, 0, 128, 128);
    public Rectangle animationSourceRect = new Rectangle(0, 0, 128, 128);
    public Rectangle testCollider;
    public Rectangle standardCollider;
    public Rectangle rollCollider;
    public Vector2 shieldOffset;
    public Vector2 playerLightOffset;
    public Vector2 shieldLightOffset;
    public Print animationFrame;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 maxVelocity;
    public Vector2 resistance = Vector2.Zero;
    public float terminalVelocity;

    public Vector2 hurtVelocity;
    public Vector2 deathVelocity = new Vector2(300, -300);
    public Vector2 deathResistance = new Vector2(10, 10);
    public float deathBounce = .666f;

    float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float energy;
    public float maxEnergy;

    public float lightBrightness = 1f;
    public float lightSize = 1f;

    public float invulnerable;
    public float invulnerableOnTouch;
    public float invulnerableMaxTime;

    public bool shielded;
    public bool energyed;  // just for testing
    public float energyUsableMargin;

    public bool attackHit;  // used to determine if the camera should swing or shake after an attack
    public bool buddha;

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
    public float shockwaveEffectiveDistanceSquared;
    public float shockwaveStunTime;

    public Vector2 shockwaveKnockback;

    public float speed;
    public float airSpeed;
    public float attackMovementSpeed;

    public float jumpTime;
    public float jumpTimeMax;
    float timeSinceLastFrame;
    public float idleTime;
    public float idleTimeMax;
    public float specialTime;
    float specialIdleTime = 5f;
    int currentFrame;
    int currentShieldFrame;
    public bool combat;

    public float stunTime;
    public float floatTime;
    float zappyTime;

    public int currentAnimation;
    int prevAnimation;
    public float[] animationSpeed = new float[47];
    public int[] animationFrames = new int[47];
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

    public Vector2 input;
    public Vector2 prevInput;
    public bool isRunning;

    bool frameInput;
    public bool inputEnabled = true; // use this to turn player control on/off

    int climbablePixels = 2;
    public float fallingVelocity = 60f;     // this prevents the falling animation from playing before hitting this vertical velocity. Useful for running down slopes.
                                            // use ~35 for one pixel slopes, or ~60 for two pixels.

    public bool fallableSquare;             // is there a square directly below me within /climbablePixels/ ?

    //public Vector2 currentLocation;
    public Direction direction;
    public Direction prevDirection;

    public Location location;
    public Activity activity;
    public Activity prevActivity;
    public bool activityChanged;

    public float rollTime = 0f;
    float rollSpeed = 500f;
    float rollTimeMax = 0.35f;
    float rollCooldown;
    float rollCooldownMax = 0.75f;

    public Attacking attacking;
    public Attacking prevAttacking;
    public float currentAttackDamage;
    public Vector2 attackDamage;
    public float critChance;
    public Vector2 critMultiplier;
    public Vector2 slamDamage;
    int attackMovementFrames = 1;
    int attackAnimation = 0;
    int prevAttackAnimation = -1;
    public int attackFrame = 1;
    bool attacked;

    float healCooldown;
    float healCooldownMax = 5f;

    public Point attackColliderSize;
    public int slamColliderWidth = 60;
    public int slamColliderHeight = 50;

    public float wallJumpTimer;
    public float jumpable;
    public float jumpableMax = 0.05f;   // coyote time

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

    private bool collision = true;
    private bool noclip;

    private Color shieldedColor = new Color(255, 240, 209);
    private Color normalColor = Color.White;
    private Color renderColor;

    ChargedBolt zappy;

    public event Irbis.Irbis.AttackEventDelegate OnPlayerAttack;
    public event Irbis.Irbis.ShockwaveEventDelegate OnPlayerShockwave;



    public Player(Texture2D PlayerTex, Texture2D ShieldTex, PlayerSettings playerSettings, float drawDepth)
    {
        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.01f);

        depth = drawDepth;
        shieldDepth = drawDepth + 0.001f;

        tex = PlayerTex;
        shieldTex = ShieldTex;

        Load(playerSettings);



        airSpeed = 0.6f * speed;
        attackMovementSpeed = 0.5f * speed;

        position.X -= standardCollider.X;
        position.Y -= standardCollider.Y;

        hurtVelocity = new Vector2(50f, -100f);
        collided = new Collided();
        enchantList = new List<Enchant>();
        walled = Wall.Zero;
        baseVelocity = heading = Vector2.Zero;

        zappy = new ChargedBolt(Point.Zero, (standardCollider.Width + standardCollider.Height) / 4, 10, 300, Color.LightCyan, Color.LightCyan);

        shockwaveEffectiveDistanceSquared = shockwaveEffectiveDistance * shockwaveEffectiveDistance;

        PlayerEventsReset();
    }

    public bool Update()
    {
        prevInput = input;
        prevAttacking = attacking;
        prevWalled = walled;
        input = Vector2.Zero;
        activityChanged = frameInput = false;

        if (Irbis.Irbis.easyWalljumpMode) // this controls wall jump reset
        {
            if ((velocity.X > 0 && Irbis.Irbis.GetRightKeyUp) || (velocity.X < 0 && Irbis.Irbis.GetLeftKeyUp)
                || Irbis.Irbis.GetJumpKeyUp || (!Irbis.Irbis.GetRightKey && !Irbis.Irbis.GetLeftKey))
            { wallJumpTimer = 0f; }
        }
        else
        {
            if ((velocity.X > 0 && Irbis.Irbis.GetRightKeyUp) || (velocity.X < 0 && Irbis.Irbis.GetLeftKeyUp)
                || Irbis.Irbis.GetJumpKeyUp)
            { wallJumpTimer = 0f; }
        }
        if (inputEnabled)
        {
            float leftStickX = Irbis.Irbis.GetLeftStickX;
            if (leftStickX < -Irbis.Irbis.analogCutoff)
            { input.X -= leftStickX; }
            else if (Irbis.Irbis.GetLeftKey) // left
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X--;
                    direction = Direction.Left;
                }
                else if (walled.Right > 0)
                { wallJumpTimer += Irbis.Irbis.DeltaTime; }
            }

            if (leftStickX > Irbis.Irbis.analogCutoff)
            { input.X += leftStickX; }
            else if (Irbis.Irbis.GetRightKey) //right
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X++;
                    direction = Direction.Right;
                }
                else if (walled.Left > 0)
                { wallJumpTimer += Irbis.Irbis.DeltaTime; }
            }

            if (direction != Direction.Forward)
            {
                //if (Irbis.Irbis.GetUpKey) // up
                //{ input.Y++; }
                if (Irbis.Irbis.GetDownKey) // down
                { input.Y--; }
                if (Irbis.Irbis.GetShieldKey) // shield
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
                if (Irbis.Irbis.GetShockwaveKey) // shockwave key held
                { superShockwave += Irbis.Irbis.DeltaTime; }
                else if (superShockwave > 0)
                // activate shockwave
                { Shockwave(); }
                if ((Irbis.Irbis.GetPotionKeyDown) && potions > 0) // potions
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
                    if (Irbis.Irbis.GetDownKeyDown && walled.Bottom <= 0 && energy >= maxEnergy * energyUsableMargin)
                    { Attack(Attacking.Slam); SetActivity(Activity.Slamming); inputEnabled = false; }
                    else
                    { input.Y++; }
                }
                else // just jumped, reset to zero
                { jumpTime = 0; }
                if (Irbis.Irbis.GetJumpKeyDown)
                {
                    if (walled.Bottom <= 0)
                    {
                        if (Irbis.Irbis.GetDownKey && energy >= maxEnergy * energyUsableMargin)
                        { Attack(Attacking.Slam); SetActivity(Activity.Slamming); inputEnabled = false; jumpTime = 0; }
                        else
                        { interruptAttack = true; } /* jump key was just pressed, interrupt an attack(this is here so that attacks in-air are not interrupted) */
                    }
                    else
                    { wallJumpTimer = 1f; }
                }
                if (rollTime <= 0 && rollCooldown <= 0 && walled.Bottom > 0 && (Irbis.Irbis.GetRollKeyDown))
                { // roll
                    inputEnabled = false;
                    rollTime = rollTimeMax;
                    if (rollTime > invulnerable)
                    { invulnerable = rollTime * 2 / 3; }
                    rollCooldown = rollCooldownMax;
                }

                if (Irbis.Irbis.GetAttackKeyDown) // && if attack is interruptable
                { // attack!
                    if (attacking == Attacking.No /*&& walled.Bottom > 0*/)
                    { Attack(Attacking.Attack1); }
                    else // if (attacking != Attacking.No && walled.Bottom > 0)
                    { attackImmediately = true; }
                }
            }
            else // in case the player goes idle while jumping/shielding/shockwaving (somehow)
            {
                shielded = false;
                jumpTime = 0;
                superShockwave = 0;
            }
        }
        else
        {
            input = Vector2.Zero;
            shielded = false;
            jumpTime = 0;
            superShockwave = 0;
        }
        if (Irbis.Irbis.GetKeyboardState != Irbis.Irbis.GetPreviousKeyboardState)
        { frameInput = true; }

        if (input != prevInput && input != Vector2.Zero)
        { interruptAttack = true; }

        if (attacking != Attacking.Slam)
        {
            if (interruptAttack)
            {
                attacking = Attacking.No;
                attackCollider = Rectangle.Empty;
                interruptAttack = false;
            }
        }
        else if (walled.Bottom > 0) // end slam
        { Slam(); }

        zappy.position = TrueCenter;
        zappy.Update();

        if (invulnerable > 0)
        { invulnerable -= Irbis.Irbis.DeltaTime; }
        if (invulnerableOnTouch > 0)
        { invulnerableOnTouch -= Irbis.Irbis.DeltaTime; }
        if (rollCooldown > 0)
        { rollCooldown -= Irbis.Irbis.DeltaTime; }
        if (stunTime > 0)
        {   stunTime -= Irbis.Irbis.DeltaTime;
            if (stunTime <= 0)
            {   inputEnabled = true;
                stunTime = 0; }
        }
        if (zappyTime > 0)
        { zappyTime -= Irbis.Irbis.DeltaTime; }
        if (!shielded)
        {   if (shield < maxShield)
            {   shield += shieldRechargeRate * Irbis.Irbis.DeltaTime;
                if (shield >= maxShield)
                { shield = maxShield; }
            }
        }
        if (energy < maxEnergy)
        {   energy += energyRechargeRate * Irbis.Irbis.DeltaTime;
            if (energy >= maxEnergy)
            { energy = maxEnergy; }
        }
        if (potionTime > 0)
        {   potionTime -= Irbis.Irbis.DeltaTime;
            if (potionTime <= 0)
            { healthRechargeRate = 0; }
        }

        Heal(healthRechargeRate * Irbis.Irbis.DeltaTime);
        if (healCooldown > 0)
        { healCooldown -= Irbis.Irbis.DeltaTime; }
        else
        { Heal(baseHealing * Irbis.Irbis.DeltaTime); }

        Movement();
        CalculateMovement();
        if (collision)
        { Collision(Irbis.Irbis.collisionObjects); }
        Animate();

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

    private bool Player_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack, Vector2 Damage)
    {
        Irbis.Irbis.WriteLine("\n" + name + " attack:" + Attack + " currentFrame:" + currentFrame + " attackFrame:" + attackFrame);
        return true;
    }

    private bool Player_OnPlayerShockwave(Vector2 Origin, float Range, Attacking Attack, float Power)
    {
        Irbis.Irbis.WriteLine(name + " origin:" + Origin + " Range:" + Range + " power:" + Power + "\n");
        return true;
    }

    public void PlayerEventsReset()
    {
        OnPlayerAttack = Player_OnPlayerAttack;
        OnPlayerShockwave = Player_OnPlayerShockwave;
    }

    public void Respawn(Vector2 initialPos)
    {
        position = initialPos;
        //position.X -= standardCollider.X;
        //position.Y -= standardCollider.Y;
        velocity = Vector2.Zero;
        CalculateMovement();
        walled = Wall.Zero;
        collided = new Collided();
        health = maxHealth;
        energy = maxEnergy;
        shield = maxShield;
        potions = maxNumberOfPotions;
        SetActivity(Activity.Idle);
        direction = Direction.Forward;
        if (Bars.potionBar != null) { Bars.potionBar.Update(potions); }
    }

    public void Movement()
    {
        if (walled.Bottom > 0 || fallableSquare)
        {
            if (jumpable < 0)
            { jumpable += Irbis.Irbis.DeltaTime; }
            else
            { jumpable = jumpableMax; }
        }
        else if (jumpable > -0.05)
        { jumpable -= Irbis.Irbis.DeltaTime; }

        if (noclip)
        {
            if (Irbis.Irbis.GetRollKey)
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, input.Y * -speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
            }
            else
            {
                velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * (speed * 0.1f), movementLerpSlowdown * Irbis.Irbis.DeltaTime);
                velocity.Y = Irbis.Irbis.Lerp(velocity.Y, input.Y * (-speed * 0.1f), movementLerpSlowdown * Irbis.Irbis.DeltaTime);
            }
        }
        else if (attacking == Attacking.Slam && activity == Activity.Slamming)
        {
            velocity.X = Irbis.Irbis.Lerp(velocity.X, 0, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
        }
        else if (rollTime > 0)
        {
            if (direction == Direction.Right)
            { velocity.X = Irbis.Irbis.Lerp(velocity.X, rollSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime); }
            else
            { velocity.X = Irbis.Irbis.Lerp(velocity.X, -rollSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime); }
            debugspeed = rollSpeed;

            rollTime -= Irbis.Irbis.DeltaTime;
            if (rollTime <= 0)
            {
                inputEnabled = true;
                rollTime = 0;
            }
        }
        else if (attacking == Attacking.Attack1)
        {
            //AttackMovement
            isRunning = false;
            if ((fallableSquare || walled.Bottom > 0) && currentFrame <= attackMovementFrames)
            {
                if (direction == Direction.Right)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    input.X = 1;
                }
                else if (direction == Direction.Left)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, -attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    input.X = -1;
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

            if (!fallableSquare && walled.Bottom > 0 && ((direction == Direction.Left && collided.LeftmostBottomCollision > collider.Left) || (direction == Direction.Right && collided.RightmostBottomCollision < collider.Right)))
            { velocity.X = 0; }

            if (input.Y > 0 && walled.Top <= 0 && jumpTime < jumpTimeMax && jumpTime > 0)
            //normal jump
            { jumpTime += Irbis.Irbis.DeltaTime; }
            else //just jumped, reset to zero
            { jumpTime = 0; }
        }
        else
        {
            if (input.X != 0)
            {
                if (walled.Bottom > 0)                                                                   //movement
                { isRunning = true; }
                else if (!isRunning)
                {
                    if ((input.X > 0 && velocity.X < 0) || (input.X < 0 && velocity.X > 0))
                    { velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * airSpeed, movementLerpAir * Irbis.Irbis.DeltaTime); }
                    else
                    { velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * airSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime); }
                    debugspeed = airSpeed;
                }

                if (isRunning && input.X == prevInput.X)
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, input.X * speed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                    debugspeed = speed;
                }
                else
                { isRunning = false; }
                if ((walled.Left > 0 && input.X < 0) || (walled.Right > 0 && input.X > 0))
                { isRunning = false; }
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
            if (input.Y > 0)
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
                    else if (Jumpable)
                    {
                        jumpTime += Irbis.Irbis.DeltaTime;
                        jumpable = 0;
                        //wallJumpTimer = 1f;
                    }
                    //wallJumpTimer += Irbis.Irbis.DeltaTime;
                }
                else //just jumped, reset to zero
                { jumpTime = 0; }
            }
            else //just jumped, reset to zero
            { jumpTime = 0; }
        }

        //if (Math.Abs(velocity.X) <= 0.000001f)
        //{ velocity.X = 0; }
        if (walled.Right > 0 && velocity.X > 0)
        { velocity.X = 0; }
        if (walled.Left > 0 && velocity.X < 0)
        { velocity.X = 0; }


        if (jumpTime > 0)
        { velocity.Y = Irbis.Irbis.Lerp(velocity.Y, -speed, movementLerpSlowdown * Irbis.Irbis.DeltaTime); }
        if (walled.Top > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }

        if (velocity.X > terminalVelocity)
        { velocity.X = terminalVelocity; }
        if (velocity.X < -terminalVelocity)
        { velocity.X = -terminalVelocity; }
        if (velocity.Y > terminalVelocity)
        { velocity.Y = terminalVelocity; }
        if (velocity.Y < -terminalVelocity)
        { velocity.Y = -terminalVelocity; }

        if (walled.Bottom > 0)
        { baseVelocity = collided.bottomCollided[0].Velocity; }
        else //if (walled.Bottom <= 0)
        {
            if (walled.Left > 0 && walled.Right <= 0)
            { baseVelocity = collided.leftCollided[0].Velocity; }
            else if (walled.Right > 0 && walled.Left <= 0)
            { baseVelocity = collided.rightCollided[0].Velocity; }
        }
        if (walled.Total <= 0)
        { baseVelocity = Vector2.Lerp(baseVelocity, Vector2.Zero, 2.0f * Irbis.Irbis.DeltaTime); }


        position += (baseVelocity + velocity) * Irbis.Irbis.DeltaTime;
    }

    public void Dying(Direction AttackDirection)
    {
        if ((int)activity < 20)
        {
            Irbis.Irbis.WriteLine("Player.Dying");
            Irbis.Irbis.PlayerDeath();
            SetActivity(Activity.Dying);
        }
        if (AttackDirection == Direction.Left)
        { direction = Direction.Right; }
        else if (AttackDirection == Direction.Right)
        { direction = Direction.Left; }
        else
        { direction = AttackDirection; }
        velocity.Y = deathVelocity.Y;
        if (direction == Direction.Left)
        { velocity.X = deathVelocity.X; }
        else
        { velocity.X = -deathVelocity.X; }
        healCooldown = healCooldownMax;
        resistance = deathResistance;
        inputEnabled = false;
    }

    public void CalculateMovement()
    {
        // using Math.Round instead of (position._ + 0.5f) due to adding 0.5f rounding in the wrong direction when position is negative
        collider = CorrectCollider;
        collider.X += (int)Math.Round((double)position.X);
        collider.Y += (int)Math.Round((double)position.Y);
    }

    public void Animate()
    {                                                        //animator
        if (activity == Activity.Running)
        { timeSinceLastFrame += Irbis.Irbis.DeltaTime * Math.Abs(input.X); }
        else
        { timeSinceLastFrame += Irbis.Irbis.DeltaTime; }
        if (health > 0)
        {
            if (zappyTime <= 0)
            {

                if (timeSinceLastFrame >= animationSpeed[currentAnimation])
                {
                    currentFrame++;
                    timeSinceLastFrame -= animationSpeed[currentAnimation];
                }

                if (activity == Activity.Idle)
                {
                    idleTime += Irbis.Irbis.DeltaTime;
                    specialTime += Irbis.Irbis.DeltaTime;
                }

                if (attacking == Attacking.No)
                {
                    if (input != Vector2.Zero)
                    {
                        specialTime = idleTime = 0;
                        if (input.X != 0)
                        {
                            if (walled.Bottom > 0)
                            { SetActivity(Activity.Running); }
                            else
                            {
                                if (jumpTime > 0)
                                { SetActivity(Activity.Jumping); } // jumping
                                else if (velocity.Y > 0 || activity != Activity.Jumping)
                                { SetActivity(Activity.Falling); } // falling 
                            }
                        }
                        else
                        {
                            if (walled.Bottom <= 0)
                            {
                                if (jumpTime > 0)
                                { SetActivity(Activity.Jumping); } // jumping
                                else if (velocity.Y > 0 || activity != Activity.Jumping)
                                { SetActivity(Activity.Falling); } // falling 
                            }
                            else if (prevWalled.Bottom <= 0)
                            { SetActivity(Activity.Landing); }
                            else if (activity != Activity.Landing && activity != Activity.Idle)
                            { SetActivity(Activity.Idle); }
                        }
                    }
                    else
                    {
                        if (walled.Bottom <= 0)
                        {
                            if (jumpTime > 0)
                            { SetActivity(Activity.Jumping); } // jumping
                            else if (velocity.Y > 0 || activity != Activity.Jumping)
                            { SetActivity(Activity.Falling); } // falling 
                        }
                        else if (prevWalled.Bottom <= 0)
                        { SetActivity(Activity.Landing); }
                        else if (activity != Activity.Landing && activity != Activity.Idle)
                        { SetActivity(Activity.Idle); }
                    }
                    if (rollTime > 0)
                    { SetActivity(Activity.Rolling); }
                }
                else if (!attacked && attacking != Attacking.Slam && currentFrame >= attackFrame)
                { Hit(); }

                if (currentFrame > animationFrames[currentAnimation])
                {
                    if (attacking != Attacking.No && attacking != Attacking.Slam)
                    {
                        if (attackImmediately)
                        {
                            attackImmediately = false;
                            Attack(Attacking.Attack1);
                            SetAnimation();
                        }
                        else
                        {
                            switch (currentAnimation)
                            {
                                case 17:
                                    goto case 18;
                                case 18:
                                    SetAnimation(29, true);
                                    break;
                                case 19:
                                    goto case 20;
                                case 20:
                                    SetAnimation(31, true);
                                    break;
                                case 21:
                                    goto case 22;
                                case 22:
                                    SetAnimation(33, true);
                                    break;
                            }
                            attacking = Attacking.No;
                            attackCollider = Rectangle.Empty;
                            activity = Activity.Idle;
                            prevActivity = Activity.Attacking;
                        }
                    }
                    else if (direction != Direction.Forward && specialTime >= specialIdleTime)
                    {
                        if (idleTime >= idleTimeMax && idleTimeMax > 0)
                        {
                            idleTime -= idleTimeMax;
                            direction = Direction.Forward;
                            SetAnimation(0, true);
                        }
                        else
                        { SetAnimation(); }
                    }
                    else if (animationNoLoop)
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
                                SetActivity(Activity.Idle);
                                break;
                            case 17:
                                goto case 18;
                            case 18:
                                SetAnimation(29, true);
                                break;
                            case 19:
                                goto case 20;
                            case 20:
                                SetAnimation(31, true);
                                break;
                            case 21:
                                goto case 22;
                            case 22:
                                SetAnimation(33, true);
                                break;
                            case 23:
                                goto case 24;
                            case 24:
                                SetAnimation(25, false);
                                break;
                            case 41:
                                goto case 42;
                            case 42:
                                SetAnimation(45, false);
                                break;
                            default:
                                SetAnimation();
                                break;
                        }
                    }
                    else
                    { currentFrame = 0; }
                }
                else if (activity == Activity.Falling && (velocity.Y - baseVelocity.Y) > fallingVelocity && baseVelocity.Y >= 0)
                { SetAnimation(); }
                else if (activityChanged || direction != prevDirection)
                { SetAnimation(); }
                else if (animationSourceRect.X >= tex.Width) // just in case animation refuses to switch
                { SetAnimation(); }

                if (prevAnimation != currentAnimation)
                {
                    timeSinceLastFrame = 0;
                    currentFrame = 0;
                }
            }
            else
            {
                if (timeSinceLastFrame >= 0.05f)
                {
                    if (currentFrame == 0)
                    { currentFrame = 2; }
                    else
                    { currentFrame = 0; }
                    timeSinceLastFrame -= 0.05f;
                }
            }
        }
        else
        {
            if (timeSinceLastFrame >= animationSpeed[currentAnimation])
            {
                currentFrame++;
                timeSinceLastFrame -= animationSpeed[currentAnimation];
                if (currentFrame > animationFrames[45])
                { currentFrame = 0; }
                if (currentAnimation == 41 || currentAnimation == 42)
                { currentAnimation += 4; }
            }
            if (currentAnimation != 41 && currentAnimation != 42)
            {
                if (fallableSquare || walled.Bottom > 0)
                {
                    if (direction == Direction.Left)
                    { currentAnimation = 43; }
                    else
                    { currentAnimation = 44; }
                }
                else
                {
                    if (direction == Direction.Left)
                    { currentAnimation = 45; }
                    else
                    { currentAnimation = 46; }
                }
            }
        }

        animationSourceRect.X = animationSourceRect.Width * currentFrame;
        animationSourceRect.Y = animationSourceRect.Height * currentAnimation;


        // abilities
        if (shielded)
        {
            shieldtimeSinceLastFrame += Irbis.Irbis.DeltaTime;
            if (shieldtimeSinceLastFrame >= shieldAnimationSpeed)
            {
                shieldtimeSinceLastFrame -= shieldAnimationSpeed;
                currentShieldFrame++;
            }
            if (currentShieldFrame * shieldTex.Height >= shieldTex.Width)
            { currentShieldFrame = 0; }
            //shieldSourceRect = new Rectangle(currentShieldFrame * 128, 0, 128, 128);
            shieldSourceRect.X = currentShieldFrame * shieldTex.Height;
            renderColor = shieldedColor;
        }
        else
        {
            shieldtimeSinceLastFrame = 0;
            renderColor = normalColor;
        }
        prevDirection = direction;
        prevAnimation = currentAnimation;
    }

    public void SetAnimation()
    {
        switch (activity)
        {
            case Activity.Idle:
                if (direction != Direction.Forward && specialTime < specialIdleTime)
                { SetAnimation(3, false); }
                else
                {   //currentAnimation = 3;
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
                if ((direction != prevDirection) || (prevActivity != Activity.Running) || ((velocity.Y - baseVelocity.Y) > fallingVelocity && baseVelocity.Y >= 0))
                { SetAnimation(13, false); }
                break;
            case Activity.Landing:
                // if (currentAnimation == 13 || currentAnimation == 14)
                { SetAnimation(15, true); }
                break;
            case Activity.Rolling:
                SetAnimation(35, false);
                break;
            case Activity.Slamming:
                SetAnimation(23, true);
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
            case Activity.Shockwave:
                SetAnimation(37, true);
                break;
            case Activity.Dying:
                SetAnimation(45, true);
                break;
            case Activity.Dead:
                SetAnimation(43, true);
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
        if ((direction == Direction.Right && currentAnimation != animation+1) || (direction == Direction.Left && currentAnimation != animation) || direction == Direction.Forward)
        {
            currentAnimation = animation;
            currentFrame = 0;
            nextAnimation = -1;
            if (direction == Direction.Right)
            { currentAnimation++; }
        }
        animationNoLoop = noLoop;
    }

    public void SetActivity(Activity activityToSet)
    {
        if (activity != activityToSet)
        {
            prevActivity = activity;
            activity = activityToSet;
            activityChanged = true;
        }
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
        testCollider.Size = collider.Size;
        fallableSquare = false;

        foreach (ICollisionObject s in colliderList)
        {
            if (!fallableSquare)
            { fallableSquare = IsSquareFallable(s.Collider); }

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

        if (walled.Left == 1 && (velocity.X < -0.0001f || input.X < 0))
        {
            int climbamount = (collider.Bottom - collided.leftCollided[0].Collider.Top);
            if (climbamount <= climbablePixels)
            {
                position.Y -= climbamount;
                amountToMove = negAmountToMove = Vector2.Zero;
                floatTime = 0.05f;  //turn off gravity for this long
                Irbis.Irbis.WriteLine(this + " on ramp, moved " + climbamount + " pixels.");
            }
        }
        if (walled.Right == 1 && (velocity.X > 0.0001f || input.X > 0))
        {
            int climbamount = (collider.Bottom - collided.rightCollided[0].Collider.Top);
            if (climbamount <= climbablePixels)
            {
                position.Y -= climbamount;
                amountToMove = negAmountToMove = Vector2.Zero;
                floatTime = 0.05f;  //turn off gravity for this long
                Irbis.Irbis.WriteLine(this + " on ramp, moved " + climbamount + " pixels.");
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
        testCollider.Location = testPos.ToPoint() + CorrectCollider.Location;

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
            if (Y)
            {
                testPos.X = (int)Math.Round((double)position.X);
                testPos.Y = position.Y;
                testPos.X += amountToMove.X;
                testCollider.Location = testPos.ToPoint() + CorrectCollider.Location;

                pass = !(collided.Intersects(testCollider));

                if (pass)
                { amountToMove.Y = 0; }
            }
            else if (X)
            {
                testPos.Y = (int)Math.Round((double)position.Y);
                testPos.X = position.X;
                testPos.Y += amountToMove.Y;
                testCollider.Location = testPos.ToPoint() + CorrectCollider.Location;

                pass = !(collided.Intersects(testCollider));

                if (pass)
                { amountToMove.X = 0; }
            }
        }

        if (amountToMove != Vector2.Zero)
        { Irbis.Irbis.WriteLine("    amountToMove: " + amountToMove); }

        position += amountToMove;
        CalculateMovement();

        for (int i = collided.bottomCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.bottomCollided[i].Collider, Side.Bottom))
            {
                collided.bottomCollided.RemoveAt(i);
                walled.Bottom--;
            }
        }
        for (int i = collided.rightCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.rightCollided[i].Collider, Side.Right))
            {
                collided.rightCollided.RemoveAt(i);
                walled.Right--;
            }
        }
        for (int i = collided.leftCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.leftCollided[i].Collider, Side.Left))
            {
                collided.leftCollided.RemoveAt(i);
                walled.Left--;
            }
        }
        for (int i = collided.topCollided.Count - 1; i >= 0; i--)
        {
            if (!Irbis.Irbis.IsTouching(collider, collided.topCollided[i].Collider, Side.Top))
            {
                collided.topCollided.RemoveAt(i);
                walled.Top--;
            }
        }

        if (health < 0 && walled.Bottom > 0 && velocity.Y > climbablePixels)
        { velocity.Y = -(velocity.Y * deathBounce); }

        if ((walled.Left > 0 && velocity.X < 0) || (walled.Right > 0 && velocity.X > 0))
        {
            velocity.X = 0;
            position.X = (int)Math.Round((double)position.X);
        }

        if ((walled.Top > 0 && velocity.Y < 0) || (walled.Bottom > 0 && velocity.Y > 0))
        {
            velocity.Y = 0;
            position.Y = (int)Math.Round((double)position.Y);
        }

        if (floatTime <= 0 && walled.Bottom <= 0 && jumpTime <= 0 && velocity.Y < terminalVelocity)
        {
            if (activity == Activity.Slamming && attacking == Attacking.Slam)
            { velocity.Y += (Irbis.Irbis.gravity * 5) * mass * Irbis.Irbis.DeltaTime; }
            else if (attacking != Attacking.No && velocity.Y > 0)
            { velocity.Y += (Irbis.Irbis.gravity / 2) * mass * Irbis.Irbis.DeltaTime; }
            else
            { velocity.Y += Irbis.Irbis.gravity * mass * Irbis.Irbis.DeltaTime; }
        }
        else
        { floatTime -= Irbis.Irbis.DeltaTime; }
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
    /// returns true if the square is not touching the collider, is within /climbablePixels/ and is within 1 pixel (left and right) of the collider
    /// </summary>
    public bool IsSquareFallable(Rectangle testrect)
    {
        return (testrect.Left < collider.Right && testrect.Right > collider.Left
            && testrect.Top - collider.Bottom > 0 && testrect.Top - collider.Bottom <= climbablePixels);
    }

    /// <summary>
    /// returns true if the player took damage
    /// </summary>
    public bool Hurt(float Damage, bool MakeInvulnerable, Direction AttackDirection)
    {
        if (invulnerable <= 0)
        {
            if (MakeInvulnerable && invulnerableMaxTime > invulnerable)
            { invulnerable = invulnerableMaxTime; }
            if (shielded)
            {
                if (Damage > shield)
                {
                    Heal(shield * shieldHealingPercentage);
                    Damage -= shield;
                    shield = 0;
                    health -= Damage;
                    if (buddha && health <= 0)
                    { health = float.Epsilon; }
                    SetAnimation(39, true);
                    healCooldown = healCooldownMax;
                    CheckHealth(AttackDirection);
                    return true;
                }
                else
                {
                    Heal(Damage * shieldHealingPercentage);
                    shield -= Damage;
                    return false;
                }
            }
            else
            {
                health -= Damage;
                if (buddha && health <= 0)
                { health = float.Epsilon; }
                SetAnimation(39, true);
                healCooldown = healCooldownMax;
                CheckHealth(AttackDirection);
                return true;
            }
        }
        CheckHealth(AttackDirection);
        return false;
    }

    public void CheckHealth(Direction AttackDirection)
    {
        if (health <= 0)
        { Dying(AttackDirection); }
    }

    /// <summary>
    /// returns true if knockback should trigger
    /// </summary>
    public bool HurtOnTouch(float damage, Direction EnemyDirection)
    {
        if (invulnerableOnTouch <= 0 && invulnerable <= 0)
        {
            invulnerableOnTouch = 1;
            return Hurt(damage, false, EnemyDirection);
        }
        return false;
    }

    public void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        { health = maxHealth; }
    }

    public void Slam()
    {
        if (direction == Direction.Left)
        { attackCollider.X = collider.Center.X - slamColliderWidth; }
        else
        { attackCollider.X = collider.Center.X; }

        attackCollider.Y = collider.Center.Y - slamColliderHeight / 2;
        attackCollider.Width = slamColliderWidth;
        attackCollider.Height = slamColliderHeight;
        OnPlayerAttack(attackCollider, Attacking.Slam, slamDamage);

        //autocast shockwave
        energy -= 30;
        if (energy < 0)
        { energy = 0; }
        Irbis.Irbis.CameraShake(0.1f, 5f);
        OnPlayerShockwave(TrueCenter, shockwaveEffectiveDistance, Attacking.Slam, 1);

        Stun(0.2f);
        activity = Activity.Idle; attacking = Attacking.No;
        SetAnimation(27, true);

        //perform slam (shockwave + damage)
    }

    public void Shockwave()
    {
        // energyed = true;
        // activate shockwave animation
        // stun for duration of animation
        if (energy >= maxEnergy * energyUsableMargin)
        {
            Stun(shockwaveStunTime); // the time it takes to "cast" shockwave
            if (walled.Bottom <= 0)
            { attacking = Attacking.AirShockwave; }
            else
            { attacking = Attacking.Shockwave; }
            
            SetActivity(Activity.Shockwave);
            if (superShockwave >= superShockwaveHoldtime && Jumpable)
            {
                energy -= 50;
                Irbis.Irbis.CameraShake(0.15f, 10f);
                OnPlayerShockwave(TrueCenter, shockwaveEffectiveDistance, attacking, 2);
            }
            else
            {
                energy -= 30;
                Irbis.Irbis.CameraShake(0.10f, 5f);
                OnPlayerShockwave(TrueCenter, shockwaveEffectiveDistance, attacking, 1);
            }
        }
        superShockwave = 0; // shockwave was just pressed, reset to zero
    }

    public void ClearCollision()
    {
        collided = new Collided();
        walled = Wall.Zero;
    }

    public void Attack(Attacking Attack)
    {
        attacked = false;
        attacking = Attack;
        SetActivity(Activity.Attacking);
        currentFrame = 0;
    }

    private void Hit()
    {
        //change attackCollider based on if (attacking) and current animation
        switch (attacking)
        {
            case (Attacking.Attack1):
                if (direction == Direction.Left)
                { attackCollider.X = collider.Center.X - attackColliderSize.X; }
                else
                { attackCollider.X = collider.Center.X; }

                attackCollider.Y = collider.Center.Y - attackColliderSize.Y / 2;
                attackCollider.Width = attackColliderSize.X;
                attackCollider.Height = attackColliderSize.Y;

                if (Irbis.Irbis.RandomBoolUsingProbability(critChance))
                { OnPlayerAttack(attackCollider, Attacking.Attack1, attackDamage * critMultiplier); }
                else
                { OnPlayerAttack(attackCollider, Attacking.Attack1, attackDamage); }
                break;
            //case (Attacking.Attack2):
            //    break;
            default:
                //attackCollider = Rectangle.Empty;
                currentAttackDamage = 0f;
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

        attacked = true;
    }

    public void Stun(float duration)
    {
        if (stunTime < duration)
        { stunTime = duration; }
        inputEnabled = false;
    }

    public void Zap(float damage, float duration)
    {
        if (Hurt(damage, true, direction))
        {
            Stun(duration);
            zappyTime = duration;
            SetAnimation(39, true);
        }
        //SetActivity(Activity.Zapped);
    }

    public void Load(PlayerSettings playerSettings)
    {
        attackDamage = playerSettings.attackDamage;
        critChance = playerSettings.critChance;
        critMultiplier = playerSettings.critMultiplier;
        slamDamage = attackDamage * 1.5f;

        speed = playerSettings.speed;
        jumpTimeMax = playerSettings.jumpTimeMax;
        idleTimeMax = playerSettings.idleTimeMax;
        standardCollider.Size = playerSettings.colliderSize;
        standardCollider.Location = new Point((int)-(standardCollider.Width / 2f), (int)-(standardCollider.Height / 2f));
        Irbis.Irbis.WriteLine("standardCollider:" + standardCollider + "\norigin:" + origin);
        origin = playerSettings.colliderOffset.ToVector2() - standardCollider.Location.ToVector2();
        Irbis.Irbis.WriteLine("colliderOffset:" + playerSettings.colliderOffset + "\norigin:" + origin);
        shieldOffset = new Vector2(shieldTex.Height / 2f);  //((standardCollider.X + (standardCollider.Width / 2f)) - (shieldSourceRect.Width / 2f), (standardCollider.Y + (standardCollider.Height / 2f)) - (shieldSourceRect.Height / 2f));

        shieldLight = playerSettings.shieldLight;
        playerLight = playerSettings.playerLight;

        shieldLightOffset = shieldLight.Size.ToVector2() / 2f;
        playerLightOffset = playerLight.Size.ToVector2() / 2f;

        rollCollider = new Rectangle(standardCollider.X, standardCollider.Y + standardCollider.Height/2, standardCollider.Width, standardCollider.Height/2);
        attackColliderSize = playerSettings.attackColliderSize;
        attackFrame = playerSettings.attackFrame;
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
        baseHealing = playerSettings.healthRechargeRate;
        potionRechargeRate = playerSettings.potionRechargeRate;
        potions = maxNumberOfPotions = playerSettings.maxNumberOfPotions;
        potionRechargeTime = playerSettings.potionRechargeTime;
        shieldHealingPercentage = playerSettings.shieldHealingPercentage;
        energyUsableMargin = playerSettings.energyUsableMargin;
        terminalVelocity = playerSettings.terminalVelocity;
        shieldAnimationSpeed = playerSettings.shieldAnimationSpeed;
        for (int i = 0; i < playerSettings.animationFrames.Length; i++)
        { animationFrames[i] = playerSettings.animationFrames[i]; }
        for (int i = 0; i < animationSpeed.Length; i++)
        { animationSpeed[i] = 0.1f; }
        for (int i = 0; i < playerSettings.animationSpeed.Length; i++)
        { animationSpeed[i] = playerSettings.animationSpeed[i]; }
        animationSpeed[35] = rollTimeMax / (animationFrames[35] + 1);
        animationSpeed[36] = rollTimeMax / (animationFrames[36] + 1);
        animationSpeed[37] = shockwaveStunTime / (animationFrames[37] + 1);
        animationSpeed[38] = shockwaveStunTime / (animationFrames[38] + 1);
    }

    public void Combat()
    {
        combat = true;
        if (direction == Direction.Forward)
        {
            direction = Direction.Right;
            idleTime = 0f;
        }
    }

    public override string ToString()
    { return name; }

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
                if (attackCollider != Rectangle.Empty) { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, true); }
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                animationFrame.Update(currentFrame.ToString(), true);
                animationFrame.Draw(sb, ((position + (standardCollider.Location - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                goto case 1;
            case 1:
                goto default;
            default:
                if (zappyTime > 0)
                { zappy.Draw(sb); }
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, renderColor, 0f, origin, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                if (shielded)
                { sb.Draw(shieldTex, position * Irbis.Irbis.screenScale, shieldSourceRect, Color.White, 0f, shieldOffset, Irbis.Irbis.screenScale, SpriteEffects.None, depth + 0.001f); }
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        if (UseColor)
        {
            if (shielded)
            { sb.Draw(tex, position * Irbis.Irbis.screenScale, shieldLight, Color.White, 0f, shieldLightOffset, Irbis.Irbis.screenScale, SpriteEffects.None, 0); }
        }
        else
        {
            sb.Draw(tex, position * Irbis.Irbis.screenScale, playerLight, Color.Black * lightBrightness, 0f, playerLightOffset, Irbis.Irbis.screenScale /** lightSize*/, SpriteEffects.None, 0);
            /*if (shielded)
            { sb.Draw(tex, (position - new Vector2(64)) * Irbis.Irbis.screenScale, new Rectangle(1920, 3584, 256, 256), Color.Black, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0); }/**/
        }
    }
}
