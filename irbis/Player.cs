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
            return new Vector2(position.X+XcolliderOffset+(colliderWidth/2), position.Y+YcolliderOffset+(colliderHeight/2));
        }
    }

    Texture2D tex;
    Texture2D shieldTex;

    public Rectangle displayRect;
    public Rectangle shieldSourceRect;
    public Rectangle animationSourceRect;
    public Rectangle testCollider;
    public int XcolliderOffset;
    public int YcolliderOffset;
    public int colliderWidth;
    public int colliderHeight;

    public Vector2 position;
    public Vector2 velocity;
    public float terminalVelocity;

    public Vector2 hurtVelocity;

    public float health;
    public float maxHealth;
    public float shield;
    public float maxShield;
    public float energy;
    public float maxEnergy;

    public bool invulnerable;
    public float invulnerableTime; 
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
    public float shockwaveMaxEffectDistance;
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
    int currentFrame;
    int currentShieldFrame;
    public bool combat;

    float stunTime;

    int currentAnimation;
    int previousAnimation;
    public float[] animationSpeed = new float[20];
    public int[] animationFrames = new int[20];
    bool animationNoLoop;

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

    //public int attackXcolliderOffset;
    //public int attackYcolliderOffset;
    public int attackColliderWidth;
    public int attackColliderHeight;

    public float wallJumpTimer;

    public bool attackImmediately;
    public bool interruptAttack;

    public float debugspeed;

    //public bool attackKeyIsDown;
    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    public List<Enchant> enchantList;

    Vector2 amountToMove;
    Vector2 negAmountToMove;
    Vector2 testPos;

    private bool collidedContains;

    public List<ICollisionObject> collided;
    public List<Side> sideCollided;

    public Vector2 heading;

    public Rectangle attackCollider;

    public float collisionCheckDistanceSqr;

    private bool collision;
    private bool noclip;

    public Player(Texture2D t, Texture2D t3, PlayerSettings playerSettings, float drawDepth)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Player"); }
        Load(playerSettings);

        //light = new PointLight();
        //light.Scale = new Vector2(1000f, 1000f);

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
        position = playerSettings.initialPosition;
        direction = Direction.right;
        location = Location.air;
        activity = Activity.idle;
        prevAttacking = attacking = Attacking.no;

        attackID = attackIDtracker = 0;
        lastAttackID = -1;

        climbablePixels = 3;

        attackImmediately = false;
        interruptAttack = false;

        airSpeed = 0.6f * speed;
        attackMovementSpeed = 0.3f * speed;
        jumpTime = 0;
        idleTime = 0f;
        animationNoLoop = false;

        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;

        superShockwave = 0;

        hurtVelocity = new Vector2(50f, -100f);
        invulnerableTime = 0f;
        invulnerable = false;

        shieldDepleted = false;
        shielded = false;
        energyed = false;

        potionTime = 0f;

        attackMovementFrames = 1;

        collided = new List<ICollisionObject>();
        sideCollided = new List<Side>();
        enchantList = new List<Enchant>();

        rollTimeMax = 0.25f;
        rollSpeed = 1500f;
        rollTime = 0f;

        displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        shieldSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;

        shieldtimeSinceLastFrame = 0f;

        collision = true;
        noclip = false;

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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Update"); }
        prevInput = input;
        prevAttacking = attacking;
        input = Point.Zero;
        frameInput = false;

        if (!Irbis.Irbis.GetJumpKey && !Irbis.Irbis.GetLeftKey && !Irbis.Irbis.GetRightKey)
        {
            wallJumpTimer = 0f;
        }
        if (inputEnabled)
        {
            if (Irbis.Irbis.GetLeftKey) //left
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X--;
                    direction = Direction.left;
                }
                wallJumpTimer += Irbis.Irbis.DeltaTime;
            }
            if (Irbis.Irbis.GetRightKey) //right
            {
                if (walled.Bottom > 0 || !walled.Horizontal || wallJumpTimer > walljumpHoldtime)
                {
                    input.X++;
                    direction = Direction.right;
                }
                wallJumpTimer += Irbis.Irbis.DeltaTime;
            }

            if (direction != Direction.forward)
            {
                if (Irbis.Irbis.GetUpKey) //up
                {
                    input.Y++;
                }
                if (Irbis.Irbis.GetDownKey) //down
                {
                    input.Y--;
                }
                if (Irbis.Irbis.GetShieldKey) //shield
                {
                    if (shield <= 0)
                    {
                        shieldDepleted = true;
                        shielded = false;
                    }
                    else if (!shieldDepleted)
                    {
                        shielded = true;
                    }
                }
                else
                {
                    shieldDepleted = shielded = false;
                }
                if (Irbis.Irbis.GetShockwaveKey && energy > maxShield * energyUsableMargin)
                //shockwave key held
                {
                    superShockwave += Irbis.Irbis.DeltaTime;
                }
                else if (superShockwave > 0)
                //activate shockwave
                {
                    Shockwave(this, Irbis.Irbis.eList);
                    interruptAttack = true;
                    superShockwave = 0; //shockwave was just used, reset to zero
                }
                if ((Irbis.Irbis.GetPotionKeyDown) && potions > 0) //potions
                {
                    healthRechargeRate += potionRechargeRate;
                    if (potionTime >= potionRechargeTime / 2)
                    {
                        potionTime += potionRechargeTime / 2;
                    }
                    else
                    {
                        potionTime = potionRechargeTime;
                    }
                    potions--;
                    Irbis.Irbis.potionBar.Update(potions);
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
                            direction = Direction.right;
                            input.X = prevInput.X = 1;
                        }
                        else if (walled.Right > 0)
                        {
                            velocity.X = -speed;
                            position.X = position.X - 1;
                            direction = Direction.left;
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
                        {
                            jumpTime += Irbis.Irbis.DeltaTime;
                        }
                        else if (walled.Bottom > 0)
                        {
                            jumpTime += Irbis.Irbis.DeltaTime;
                            wallJumpTimer = 1f;
                        }
                        wallJumpTimer += Irbis.Irbis.DeltaTime;
                    }
                    else //just jumped, reset to zero
                    {
                        jumpTime = 0;
                    }
                }
                else //just jumped, reset to zero
                {
                    jumpTime = 0;
                }
                if (Irbis.Irbis.GetJumpKeyDown && walled.Bottom <= 0)
                //jump key was just pressed, interrupt an attack (this is here so that attacks in-air are not interrupted)
                {
                    interruptAttack = true;
                }
                if (rollTime <= 0 && walled.Bottom > 0 && (Irbis.Irbis.GetRollKeyDown))
                //roll
                {
                    invulnerable = true;
                    inputEnabled = false;
                    rollTime = rollTimeMax;
                }

                if (Irbis.Irbis.GetAttackKeyDown) //&& if attack is interruptable
                //attack!
                {
                    if (attacking == Attacking.no /*&& walled.Bottom > 0*/)
                    {
                        attacking = Attacking.attack1;
                        attackID = attackIDtracker++;
                        //attackKeyIsDown = true;
                    }
                    else //if (attacking != Attacking.no && walled.Bottom > 0)
                    {
                        attackImmediately = true;
                        //attackKeyIsDown = true;
                    }
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
        {
            frameInput = true;
        }

        if (input != prevInput && input != Point.Zero)
        {
            interruptAttack = true;
        }

        if (interruptAttack)
        {
            attacking = Attacking.no;
            interruptAttack = false;
        }

        if (stunTime > 0)
        {
            stunTime -= Irbis.Irbis.DeltaTime;
            if (stunTime <= 0)
            {
                inputEnabled = true;
                stunTime = 0;
            }
        }

        if (invulnerableTime > 0)
        {
            invulnerableTime -= Irbis.Irbis.DeltaTime;
            if (invulnerableTime <= 0)
            {
                invulnerable = false;
                invulnerableTime = 0;
            }
        }

        Movement();
        CalculateMovement();
        if (collision)
        {
            Collision(Irbis.Irbis.collisionObjects);
        }
        Animate();

        if (attacking != Attacking.no)
        {
            Hitbox();
        }
        else
        {
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
            attackID = 0;
        }

        if (potionTime > 0)
        {
            potionTime -= Irbis.Irbis.DeltaTime;
        }
        else
        {
            potionTime = 0;
            healthRechargeRate = baseHealing;
        }

        if (!shielded)
        {
            if (shield < maxShield)
            {
                shield += shieldRechargeRate * Irbis.Irbis.DeltaTime;
                if (shield >= maxShield)
                {
                    shield = maxShield;
                }
            }
        }

        if (!energyed)
        {
            if (energy < maxEnergy)
            {
                energy += energyRechargeRate * Irbis.Irbis.DeltaTime;
                if (energy >= maxEnergy)
                {
                    energy = maxEnergy;
                }
            }
        }

        if (true)
        {
            if (health < maxHealth)
            {
                health += healthRechargeRate * Irbis.Irbis.DeltaTime;
                if (health >= maxHealth)
                {
                    health = maxHealth;
                }
            }
        }

        //light.Position = new Vector2(position.X + XcolliderOffset + (collider.Width/2), position.Y + YcolliderOffset + (collider.Height / 2));
        return true;
    }

    public void Respawn(Vector2 initialPos)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Respawn"); }
        position = initialPos;
        position.X -= XcolliderOffset;
        position.Y -= YcolliderOffset;
        velocity = Vector2.Zero;
        CalculateMovement();
        health = maxHealth;
        energy = maxEnergy;
        shield = maxShield;
        potions = maxNumberOfPotions;
        if (Irbis.Irbis.potionBar != null) { Irbis.Irbis.potionBar.Update(potions); }
    }

    public void Movement()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Movement"); }
        if (rollTime > 0)
        {
            if (direction == Direction.right)
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
                invulnerable = false;
                rollTime = 0;
            }
        }
        else if (attacking != Attacking.no)
        {
            //AttackMovement();
            isRunning = false;
            if (currentFrame <= attackMovementFrames && walled.Bottom > 0)
            {
                if (direction == Direction.right && (collided.Count > 1 || (collided.Count > 0 && collided[0].Collider.Right >= collider.Right)))
                {
                    velocity.X = Irbis.Irbis.Lerp(velocity.X, attackMovementSpeed, movementLerpBuildup * Irbis.Irbis.DeltaTime);
                }
                else if (direction == Direction.left && (collided.Count > 1 || (collided.Count > 0 && collided[0].Collider.Left <= collider.Left)))
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

        if (noclip)
        {
            velocity.Y = Irbis.Irbis.Lerp(velocity.Y, -speed * input.Y, movementLerpSlowdown * Irbis.Irbis.DeltaTime);
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
        if (walled.Bottom <= 0 && jumpTime <= 0 && !noclip)
        {
            if (attacking != Attacking.no)
            {
                velocity.Y += (Irbis.Irbis.gravity / 2) * Irbis.Irbis.DeltaTime;
            }
            else
            {
                velocity.Y += Irbis.Irbis.gravity * Irbis.Irbis.DeltaTime;
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

    public void WalljumpDebug(int butt)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("WalljumpDebug"); }
        Irbis.Irbis.WriteLine();
        Irbis.Irbis.WriteLine("       walljumped " + butt);
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
                if (attackImmediately)
                {
                    attackImmediately = false;
                    attackID = attackIDtracker++;
                }
                else
                {
                    attacking = Attacking.no;
                }
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
                idleTime = 0;
                if (input.X != 0)
                {
                    if (walled.Bottom > 0)
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
                    if (walled.Bottom <= 0)
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
                if (walled.Bottom <= 0)
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
            if (rollTime > 0)
            {
                activity = Activity.rolling;
            }
        }
        switch (activity)
        {
            case Activity.idle:
                if (direction == Direction.forward)
                {
                    if (idleTime > idleTimeMax && idleTimeMax > 0)
                    {
                        idleTime = 0;
                        currentAnimation = 1;
                        currentFrame = 0;
                        animationNoLoop = true;
                    }
                    else if (!animationNoLoop)
                    {
                        currentAnimation = 0;
                    }
                }
                else
                {
                    if (idleTime > idleTimeMax && idleTimeMax > 0)
                    {
                        idleTime = 0;
                        currentAnimation = 0;
                        currentFrame = 0;
                        direction = Direction.forward;
                    }
                    else
                    {
                        currentAnimation = 3;
                    }
                }
                if (frameInput || combat)
                {
                    idleTime = 0;
                }
                else
                {
                    idleTime += Irbis.Irbis.DeltaTime;
                }

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
        
        animationSourceRect.X = 128 * currentFrame;
        animationSourceRect.Y = 128 * currentAnimation;
        

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
        }
        else
        {
            shieldtimeSinceLastFrame = 0;
        }
        previousAnimation = currentAnimation;
    }

    public void Noclip()
    {
        noclip = !noclip;
        collision = !noclip;
        velocity = Vector2.Zero;
        walled = Wall.Zero;
        collided.Clear();
        sideCollided.Clear();
        if (noclip)
        {
            speed *= 25;
            walled.Bottom = 1;
        }
        else
        {
            speed /= 25;
        }
    }

    public void Collision(List<ICollisionObject> colliderList)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Collision"); }
        amountToMove = negAmountToMove = Vector2.Zero;
        testCollider.Width = colliderWidth;
        testCollider.Height = colliderHeight;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.Collider != Rectangle.Empty && Irbis.Irbis.DistanceSquared(collider.Center, s.Collider.Center) < collisionCheckDistanceSqr /*&& !collided.Contains(s)*/)
            {
                collidedContains = collided.Contains(s);
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.bottom))                              //DOWN
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.bottom);
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
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.right))                               //RIGHT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.right);
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
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.left))                                //LEFT
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.left);
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
                if (Irbis.Irbis.IsTouching(collider, s.Collider, Side.top))                                 //UP
                {
                    if (!collidedContains)
                    {
                        collided.Add(s);
                        sideCollided.Add(Side.top);
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
            int climbamount = (collider.Bottom - collided[sideCollided.IndexOf(Side.left)].Collider.Top);
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
            int climbamount = (collider.Bottom - collided[sideCollided.IndexOf(Side.right)].Collider.Top);
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
                    case Side.bottom:
                        walled.Bottom--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.right:
                        walled.Right--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.left:
                        walled.Left--;
                        collided.RemoveAt(i);
                        sideCollided.RemoveAt(i);
                        i--;
                        break;
                    case Side.top:
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

    public void Hurt(float damage)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Hurt"); }
        if (!invulnerable)
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
            invulnerable = true;
            invulnerableTime = invulnerableMaxTime;
        }
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

    public void Shockwave(Player player, List<Enemy> enemyList)
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

            float distance;
            foreach (Enemy e in enemyList)
            {
                distance = Vector2.Distance(collider.Center.ToVector2(), e.Collider.Center.ToVector2());
                if (distance < e.shockwaveMaxEffectDistance) { distance = e.shockwaveMaxEffectDistance; }
                if (e.Collider != Rectangle.Empty && distance <= e.shockwaveEffectiveDistance)
                {
                    heading = (e.Collider.Center - collider.Center).ToVector2();
                    heading.Normalize();
                    heading.Y += 1f;
                    e.Shockwave(distance, 1, heading);
                }
            }
        }
        else
        {
            energy -= 50;
            Irbis.Irbis.CameraShake(0.15f, 10f);

            float distance;
            foreach (Enemy e in enemyList)
            {
                distance = Vector2.Distance(collider.Center.ToVector2(), e.Collider.Center.ToVector2());
                if (distance < e.shockwaveMaxEffectDistance) { distance = e.shockwaveMaxEffectDistance; }
                if (e.Collider != Rectangle.Empty && distance <= e.shockwaveEffectiveDistance)
                {
                    heading = (e.Collider.Center - collider.Center).ToVector2();
                    heading.Normalize();
                    heading.Y += 1f;
                    e.Shockwave(distance, 2, heading);
                }
            }
        }
        superShockwave = 0;
    }

    public void ClearCollision()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("ClearCollision"); }
        collided.Clear();
        sideCollided.Clear();
        walled = Wall.Zero;
    }

    public void Hitbox()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Hitbox"); }
        //change attackCollider based on if (attacking) and current animation
        if (lastAttackID != attackID)
        {
            lastAttackID = attackID;
            attackHit = false;

            switch (attacking)
            {
                case (Attacking.attack1):
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

                    break;
                //case (Attacking.attack2):

                    //break;
                default:
                    attackCollider = Rectangle.Empty;
                    attackDamage = 0f;
                    break;
            }
        }
        else if (attackCollider != Rectangle.Empty)
        {
            if (attackHit)
            {
                Irbis.Irbis.CameraShake(0.075f, 0.1f * attackDamage);
            }
            else
            {
                if (direction == Direction.left)
                {
                    Irbis.Irbis.CameraSwing(Irbis.Irbis.swingDuration, Irbis.Irbis.swingMagnitude, new Vector2(-5,-Irbis.Irbis.RandomFloat())); //change the Y based on which attack animation is playing
                }
                else
                {
                    Irbis.Irbis.CameraSwing(Irbis.Irbis.swingDuration, Irbis.Irbis.swingMagnitude, new Vector2(5, -Irbis.Irbis.RandomFloat()));
                }
            }
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
        }
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
        position = playerSettings.initialPosition;
        attack1Damage = playerSettings.attack1Damage;
        attack2Damage = playerSettings.attack2Damage;
        speed = playerSettings.speed;
        jumpTimeMax = playerSettings.jumpTimeMax;
        idleTimeMax = playerSettings.idleTimeMax;
        XcolliderOffset = playerSettings.XcolliderOffset;
        YcolliderOffset = playerSettings.YcolliderOffset;
        colliderWidth = playerSettings.colliderWidth;
        colliderHeight = playerSettings.colliderHeight;
        attackColliderWidth = playerSettings.attackColliderWidth;
        attackColliderHeight = playerSettings.attackColliderHeight;
        health = maxHealth = playerSettings.maxHealth;
        shield = maxShield = playerSettings.maxShield;
        energy = maxEnergy = playerSettings.maxEnergy;
        superShockwaveHoldtime = playerSettings.superShockwaveHoldtime;
        walljumpHoldtime = playerSettings.walljumpHoldtime;
        shockwaveMaxEffectDistance = playerSettings.shockwaveMaxEffectDistance;
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
        if (direction == Direction.forward)
        {
            direction = Direction.right;
            idleTime = 0f;
        }
    }

    public override string ToString()
    {
        return
            "Position:" + position +
            " TrueCenter:" + TrueCenter +
            " Center:" + collider.Center +
            " Collider:" + collider +
            " Health:" + health +
            " MaxHealth:" + maxHealth;
    }

    public void Draw(SpriteBatch sb)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Draw"); }
        if (Irbis.Irbis.debug > 1) { RectangleBorder.Draw(sb, collider, Color.Magenta, 0.9f); }
        if (attackCollider != Rectangle.Empty) { RectangleBorder.Draw(sb, attackCollider, Color.Magenta, 0.9f); }
        sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, (Irbis.Irbis.screenScale / 2f), SpriteEffects.None, depth);
        if (shielded) { sb.Draw(shieldTex, (position - new Vector2(32, 32)) * Irbis.Irbis.screenScale, shieldSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth + 0.01f); }
    }
}
