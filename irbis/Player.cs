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
    Texture2D tex;
    Texture2D shieldTex;

    float deltaTime;

    public Rectangle displayRect;
    public Rectangle shieldSourceRect;
    public Rectangle animationSourceRect;
    public Rectangle collider;
    public Rectangle testCollider;
    public RectangleBorder colliderDrawer;
    public RectangleBorder attackColliderDrawer;
    public int XcolliderOffset;
    public int YcolliderOffset;
    public int colliderWidth;
    public int colliderHeight;

    //public Rectangle edgeCollider;
    public Vector2 pos;
    //public Vector2 previousPos;
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
    public int rightWalled;
    public int leftWalled;
    public int topWalled;
    public int bottomWalled;
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

    public float walledInputChange;

    public bool attackImmediately;
    public bool interruptAttack;

    //public bool attackKeyIsDown;
    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    Vector2 amountToMove;
    Vector2 negAmountToMove;
    Vector2 testPos;

    public List<ICollisionObject> collided;
    public List<Side> sideCollided;
    Irbis.Irbis game;
    //GameTime gameTime;

    public Vector2 heading;

    public Rectangle attackCollider;

    public float collisionCheckDistanceSqr;

    public Player(Texture2D t, Texture2D t3, PlayerSettings playerSettings, Irbis.Irbis masterGame)
    {
        game = masterGame;
        Load(playerSettings);

        stunTime = 0f;
        //enableInput = 0f;
        isRunning = false;

        depth = 0.5f;
        shieldDepth = 0.51f;

        heading = Vector2.Zero;
        inputEnabled = true;
        frameInput = false;
        tex = t;
        shieldTex = t3;
        pos = playerSettings.initialPosition;
        direction = Direction.right;
        location = Location.air;
        activity = Activity.idle;
        prevAttacking = attacking = Attacking.no;

        attackID = attackIDtracker = 0;
        lastAttackID = -1;

        attackImmediately = false;
        interruptAttack = false;

        airSpeed = 0.6f * speed;
        attackMovementSpeed = 0.3f * speed;
        jumpTime = 0;
        idleTime = 0f;
        animationNoLoop = false;

        pos.X -= XcolliderOffset;
        pos.Y -= YcolliderOffset;

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

        rollTimeMax = 0.25f;
        rollSpeed = 1500f;
        rollTime = 0f;

        displayRect = new Rectangle((int)pos.X, (int)pos.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        shieldSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;

        shieldtimeSinceLastFrame = 0f;

        colliderDrawer = new RectangleBorder(collider, Color.Magenta, 0.19f);
        attackColliderDrawer = new RectangleBorder(attackCollider, Color.Magenta, 0.195f);

        if (colliderHeight > colliderWidth)
        {
            collisionCheckDistanceSqr = (colliderHeight * colliderHeight);
        }
        else
        {
            collisionCheckDistanceSqr = (colliderWidth * colliderWidth);
        }
    }

    public void Update(GameTime GT)
    {
        //gameTime = GT;
        deltaTime = (float)GT.ElapsedGameTime.TotalSeconds;
        prevInput = input;
        prevAttacking = attacking;
        input = Point.Zero;
        frameInput = false;

        if (inputEnabled)
        {
            if (game.keyboardState.IsKeyDown(game.leftKey) || game.keyboardState.IsKeyDown(game.altLeftKey))
                //left
            {
                if ((leftWalled > 0 || rightWalled > 0) && bottomWalled <= 0 && -walledInputChange < walljumpHoldtime)
                {
                    walledInputChange -= deltaTime;
                }
                else
                {
                    input.X--;
                    direction = Direction.left;
                }

                if ((input.X != prevInput.X || input.X == 0) && walledInputChange > 0)
                {
                    walledInputChange = 0;
                    input.X = prevInput.X;
                }
            }
            if (game.keyboardState.IsKeyDown(game.rightKey) || game.keyboardState.IsKeyDown(game.altRightKey))
                //right
            {
                if ((leftWalled > 0 || rightWalled > 0) && bottomWalled <= 0 && walledInputChange < walljumpHoldtime)
                {
                    walledInputChange += deltaTime;
                }
                else
                {
                    input.X++;
                    direction = Direction.right;
                }

                if ((input.X != prevInput.X || input.X == 0) && walledInputChange < 0)
                {
                    walledInputChange = 0;
                    input.X = prevInput.X;
                }
            }
        }
        if (direction != Direction.forward)
        {
            if (game.keyboardState.IsKeyDown(game.upKey) || game.keyboardState.IsKeyDown(game.altUpKey))
                //up
            {
                input.Y++;
            }
            if (game.keyboardState.IsKeyDown(game.downKey) || game.keyboardState.IsKeyDown(game.altDownKey))
                //down
            {
                input.Y--;
            }
            if (game.keyboardState.IsKeyDown(game.shieldKey) || game.keyboardState.IsKeyDown(game.altShieldKey))
                //shield
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
                //interruptAttack = true;
            }
            else
            {
                shieldDepleted = false;
                shielded = false;
            }
            if (game.keyboardState.IsKeyDown(game.shockwaveKey) || game.keyboardState.IsKeyDown(game.altShockwaveKey))
                //shockwave key held
            {
                superShockwave += deltaTime;
                //interruptAttack = true;
            }
            else if (superShockwave > 0 && energy > maxShield * energyUsableMargin)
                //activate shockwave
            {
                Shockwave(this, game.eList);
                interruptAttack = true;
            }
            else
                //shockwave was just used, reset to zero
            {
                superShockwave = 0;
            }
            if (((game.keyboardState.IsKeyDown(game.potionKey) && !game.previousKeyboardState.IsKeyDown(game.potionKey)) || (game.keyboardState.IsKeyDown(game.altPotionKey) && !game.previousKeyboardState.IsKeyDown(game.altPotionKey))) && potions > 0)
                //potions
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
                game.potionBar.Update(potions);
            }
            if ((bottomWalled > 0 || jumpTime != 0) && topWalled <= 0 && (game.keyboardState.IsKeyDown(game.jumpKey) || game.keyboardState.IsKeyDown(game.altJumpKey)) && jumpTime < jumpTimeMax)
                //normal jump
            {
                jumpTime += deltaTime;
            }
            else if ((leftWalled > 0 || rightWalled > 0) && topWalled <= 0 && ((game.keyboardState.IsKeyDown(game.jumpKey) && !game.previousKeyboardState.IsKeyDown(game.jumpKey)) || (game.keyboardState.IsKeyDown(game.altJumpKey) && !game.previousKeyboardState.IsKeyDown(game.altJumpKey))))
                //walljump
            {
                if (Math.Abs(walledInputChange) <= walljumpHoldtime &&
                    (game.keyboardState.IsKeyDown(game.rightKey) || game.keyboardState.IsKeyDown(game.altRightKey) ||
                    game.keyboardState.IsKeyDown(game.leftKey) || game.keyboardState.IsKeyDown(game.altLeftKey)))
                    //horizontal input
                {
                    if (leftWalled > 0)
                    {
                        velocity.X = speed;
                        direction = Direction.right;
                        input.X = prevInput.X = 1;
                    }
                    else if (rightWalled > 0)
                    {
                        velocity.X = -speed;
                        direction = Direction.left;
                        input.X = prevInput.X = -1;
                    }

                    jumpTime += deltaTime;
                    isRunning = true;
                }
            }
            else
                //just jumped, reset to zero
            {
                jumpTime = 0;
            }
            if ((game.keyboardState.IsKeyDown(game.jumpKey) && !game.previousKeyboardState.IsKeyDown(game.jumpKey)) || (game.keyboardState.IsKeyDown(game.altJumpKey) && !game.previousKeyboardState.IsKeyDown(game.altJumpKey)))
                //jump key was just pressed, interrupt an attack (this is here so that attacks in-air are not interrupted)
            {
                interruptAttack = true;
            }
            if (rollTime <= 0 && bottomWalled > 0 && (game.GetKeyDown(game.rollKey) || game.GetKeyDown(game.altRollKey)))
                //roll
            {
                invulnerable = true;
                inputEnabled = false;
                rollTime = rollTimeMax;
            }

            if ((game.keyboardState.IsKeyDown(game.attackKey) && !game.previousKeyboardState.IsKeyDown(game.attackKey)) || (game.keyboardState.IsKeyDown(game.altAttackKey) && !game.previousKeyboardState.IsKeyDown(game.altAttackKey))) //&& if attack is interruptable
                                                                                                                                                                                                                                          //attack!
            {
                if (attacking == Attacking.no /*&& bottomWalled > 0*/)
                {
                    attacking = Attacking.attack1;
                    attackID = attackIDtracker++;
                    //attackKeyIsDown = true;
                }
                else //if (attacking != Attacking.no && bottomWalled > 0)
                {
                    attackImmediately = true;
                    //attackKeyIsDown = true;
                }
            }
        }
        else
            //in case the player goes idle while jumping/shielding/shockwaving (somehow)
        {
            shielded = false;
            jumpTime = 0;
            superShockwave = 0;
        }
        
        if (game.keyboardState != game.previousKeyboardState)
        {
            frameInput = true;
        }

        if (input != prevInput && input != Point.Zero)
        {
            interruptAttack = true;
        }

        if (!(leftWalled > 0 || rightWalled > 0))
        {
            walledInputChange = 0f;
        }

        if (interruptAttack)
        {
            attacking = Attacking.no;
            interruptAttack = false;
        }

        if (stunTime > 0)
        {
            stunTime -= deltaTime;
            if (stunTime <= 0)
            {
                inputEnabled = true;
                stunTime = 0;
            }
        }

        if (invulnerableTime > 0)
        {
            invulnerableTime -= deltaTime;
            if (invulnerableTime <= 0)
            {
                invulnerable = false;
                invulnerableTime = 0;
            }
        }

        Movement();
        CalculateMovement();
        Collision(this, game.collisionObjects);
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
            potionTime -= deltaTime;
        }
        else
        {
            potionTime = 0;
            healthRechargeRate = baseHealing;
        }

        if (!shielded)
        {
            if (shield >= maxShield)
            {
                shield = maxShield;
            }
            else
            {
                shield += shieldRechargeRate * deltaTime;
            }
        }
        if (!energyed)
        {
            if (energy >= maxEnergy)
            {
                energy = maxEnergy;
            }
            else
            {
                energy += energyRechargeRate * deltaTime;
            }
        }

        if (true)
        {
            if (health >= maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += healthRechargeRate * deltaTime;
            }
        }

        if (game.debug)
        {
            colliderDrawer.Update(collider, Color.Magenta);
        }
            attackColliderDrawer.Update(attackCollider, Color.Magenta);
        
    }

    public void Respawn(Vector2 initialPos)
    {
        pos = initialPos;
        pos.X -= XcolliderOffset;
        pos.Y -= YcolliderOffset;
        velocity = Vector2.Zero;
        CalculateMovement();
        health = maxHealth;
        energy = maxEnergy;
        shield = maxShield;
        potions = maxNumberOfPotions;
        if (game.potionBar != null) { game.potionBar.Update(potions); }
    }

    public void Movement()
    {
        if (rollTime > 0)
        {
            if (direction == Direction.right)
            {
                velocity.X = game.Lerp(velocity.X, rollSpeed, movementLerpAir * deltaTime);
            }
            else
            {
                velocity.X = game.Lerp(velocity.X, -rollSpeed, movementLerpAir * deltaTime);
            }

            rollTime -= deltaTime;
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
            if (currentFrame <= attackMovementFrames && bottomWalled > 0)
            {
                if (direction == Direction.right && (collided.Count > 1 || collided[0].collider.Right >= collider.Right))
                {
                    velocity.X = game.Lerp(velocity.X, attackMovementSpeed, movementLerpBuildup * deltaTime);
                }
                else if (direction == Direction.left && (collided.Count > 1 || collided[0].collider.Left <= collider.Left))
                {
                    velocity.X = game.Lerp(velocity.X, -attackMovementSpeed, movementLerpBuildup * deltaTime);
                }
            }
            else if (bottomWalled <= 0)
            {
                velocity.X = game.Lerp(velocity.X, input.X * attackMovementSpeed, movementLerpBuildup * deltaTime);
            }
            else
            {
                velocity.X = game.Lerp(velocity.X, 0, movementLerpSlowdown * deltaTime);
            }
        }
        else
        {
            if (input.X != 0)
            {
                if (bottomWalled > 0)                                                                   //movement
                {
                    isRunning = true;
                }
                else if (!isRunning)
                {
                    if ((input.X > 0 && velocity.X < 0) || (input.X < 0 && velocity.X > 0))
                    {
                        velocity.X = game.Lerp(velocity.X, input.X * airSpeed, movementLerpAir * deltaTime);
                    }
                    else
                    {
                        velocity.X = game.Lerp(velocity.X, input.X * airSpeed, movementLerpBuildup * deltaTime);
                    }
                }

                if (isRunning && input.X == prevInput.X)
                {
                    velocity.X = game.Lerp(velocity.X, input.X * speed, movementLerpBuildup * deltaTime);
                }
                else
                {
                    isRunning = false;
                }
                if ((leftWalled > 0 && input.X < 0) || (rightWalled > 0 && input.X > 0))
                {
                    isRunning = false;
                }
            }
            else
            {
                if (bottomWalled > 0)                                                                   //movement
                {
                    velocity.X = game.Lerp(velocity.X, input.X * speed, movementLerpSlowdown * deltaTime);
                }
                else if (Math.Abs(velocity.X) < airSpeed)
                {
                    velocity.X = game.Lerp(velocity.X, input.X * airSpeed, movementLerpBuildup * deltaTime);
                }
                isRunning = false;
            }
        }

        if (Math.Abs(velocity.X) <= 0.000001f)
        {
            velocity.X = 0;
        }

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
            velocity.Y = game.Lerp(velocity.Y, -speed, movementLerpSlowdown * deltaTime);
            //velocity.Y = -speed;
        }
        if (topWalled > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }
        if (bottomWalled <= 0 && jumpTime <= 0)
        {
            if (attacking != Attacking.no)
            {
                velocity.Y += (Irbis.Irbis.gravity / 2) * deltaTime;
            }
            else
            {
                velocity.Y += Irbis.Irbis.gravity * deltaTime;
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
        pos += velocity * deltaTime;

    }

    public void CalculateMovement()
    {
        //displayRect = new Rectangle((int)pos.X, (int)pos.Y, 128, 128);
        displayRect.X = (int)pos.X;
        displayRect.Y = (int)pos.Y;
        //collider = new Rectangle((int)pos.X + XcolliderOffset, (int)pos.Y + YcolliderOffset, colliderWidth, colliderHeight);
        collider.X = (int)pos.X + XcolliderOffset;
        collider.Y = (int)pos.Y + YcolliderOffset;
        collider.Width = colliderWidth;
        collider.Height = colliderHeight;
    }

    public void Animate()
    {                                                        //animator
        timeSinceLastFrame += deltaTime;
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
                if (direction == Direction.forward)
                {
                    if (idleTime > idleTimeMax)
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
                    if (idleTime > idleTimeMax)
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
                    idleTime += deltaTime;
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
            shieldtimeSinceLastFrame += deltaTime;
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

    public void Collision(Player player, List<ICollisionObject> colliderList)
    {
        amountToMove = Vector2.Zero;
        negAmountToMove = Vector2.Zero;
        //testPos = player.pos;
        testCollider.Width = player.colliderWidth;
        testCollider.Height = player.colliderHeight;

        foreach (ICollisionObject s in colliderList)
        {
            if (s.collider != Rectangle.Empty && game.DistanceSquared(collider.Center, s.collider.Center) < collisionCheckDistanceSqr && !collided.Contains(s))
            {
                if (game.IsTouching(player.collider, s.collider, Side.bottom))                              //DOWN
                {
                    collided.Add(s);
                    sideCollided.Add(Side.bottom);
                    player.bottomWalled++;
                    if (negAmountToMove.Y > s.collider.Top - player.collider.Bottom && (player.velocity.Y * deltaTime) >= -(s.collider.Top - player.collider.Bottom))
                    {
                        negAmountToMove.Y = s.collider.Top - player.collider.Bottom;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.right))                               //RIGHT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.right);
                    player.rightWalled++;
                    if (negAmountToMove.X > s.collider.Left - player.collider.Right && (player.velocity.X * deltaTime) >= -(s.collider.Left - player.collider.Right))
                    {
                        negAmountToMove.X = s.collider.Left - player.collider.Right;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.left))                                //LEFT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.left);
                    player.leftWalled++;
                    if (amountToMove.X < s.collider.Right - player.collider.Left && (player.velocity.X * deltaTime) <= -(s.collider.Right - player.collider.Left))
                    {
                        amountToMove.X = s.collider.Right - player.collider.Left;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.top))                                 //UP
                {
                    collided.Add(s);
                    sideCollided.Add(Side.top);
                    player.topWalled++;
                    if (amountToMove.Y < s.collider.Bottom - player.collider.Top && (player.velocity.Y * deltaTime) <= -(s.collider.Bottom - player.collider.Top))
                    {
                        amountToMove.Y = s.collider.Bottom - player.collider.Top;
                    }
                }
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
            testPos.Y = (int)player.pos.Y;
            testPos.X = player.pos.X;
            testPos.Y += amountToMove.Y;
            Y = true;
        }
        else if (amountToMove.X != 0)
        {
            testPos.X = (int)player.pos.X;
            testPos.Y = player.pos.Y;
            testPos.X += amountToMove.X;
            X = true;
        }

        bool pass = true;
        testCollider.X = (int)testPos.X + player.XcolliderOffset;
        testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

        foreach (ICollisionObject s in collided)
        {
            if (s.collider.Intersects(testCollider))
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
            if (game.debug && amountToMove != Vector2.Zero)
            {
                Console.WriteLine("        pass: " + pass);
                Console.WriteLine("amountToMove: " + amountToMove);
                Console.WriteLine("    velocity: " + player.velocity);
                Console.WriteLine("  player.pos: " + player.pos);
                Console.WriteLine("     testPos: " + testPos);
                Console.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
                Console.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                foreach (ICollisionObject s in collided)
                {
                    Console.WriteLine("   scollider: T:" + s.collider.Top + " B:" + s.collider.Bottom + " L:" + s.collider.Left + " R:" + s.collider.Right);
                }
                Console.WriteLine("after--");
            }

            pass = true;
            if (Y)
            {
                testPos.X = (int)player.pos.X;
                testPos.Y = player.pos.Y;
                testPos.X += amountToMove.X;
                testCollider.X = (int)testPos.X + player.XcolliderOffset;
                testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

                foreach (ICollisionObject s in collided)
                {
                    if (s.collider.Intersects(testCollider))
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
                testPos.Y = (int)player.pos.Y;
                testPos.X = player.pos.X;
                testPos.Y += amountToMove.Y;
                testCollider.X = (int)testPos.X + player.XcolliderOffset;
                testCollider.Y = (int)testPos.Y + player.YcolliderOffset;

                foreach (ICollisionObject s in collided)
                {
                    if (s.collider.Intersects(testCollider))
                    {
                        pass = false;
                    }
                }

                if (pass)
                {
                    amountToMove.X = 0;
                }
            }
            if (game.debug && amountToMove != Vector2.Zero)
            {
                Console.WriteLine("        pass: " + pass);
                Console.WriteLine("amountToMove: " + amountToMove);
                Console.WriteLine("    velocity: " + player.velocity);
                Console.WriteLine("  player.pos: " + player.pos);
                Console.WriteLine("     testPos: " + testPos);
                Console.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
                Console.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
                foreach (ICollisionObject s in collided)
                {
                    Console.WriteLine("   scollider: T:" + s.collider.Top + " B:" + s.collider.Bottom + " L:" + s.collider.Left + " R:" + s.collider.Right);
                }
                Console.WriteLine("after--");
            }
        }

        if (game.debug && amountToMove != Vector2.Zero)
        {
            Console.WriteLine("    velocity: " + player.velocity);
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
        //if (amountToMove != Vector2.Zero)
        //{
        //    if (Math.Abs(amountToMove.Y) >= Math.Abs(amountToMove.X))
        //    {
        //        amountToMove.X = 0;
        //        player.velocity.Y = 0;
        //    }
        //    else
        //    {
        //        amountToMove.Y = 0;
        //        player.velocity.X = 0;
        //    }
        //}

        
        if (amountToMove.X != 0)
        {
            player.pos.X = (int)player.pos.X;
        }
        if (amountToMove.Y != 0)
        {
            player.pos.Y = (int)player.pos.Y;
        }
        player.pos += amountToMove;


        player.CalculateMovement();
        for (int i = 0; i < collided.Count; i++)
        {
            if (!game.IsTouching(player.collider, collided[i].collider, sideCollided[i]))
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
        if (game.debug && amountToMove != Vector2.Zero)
        {
            Console.WriteLine("        pass: " + pass);
            Console.WriteLine("amountToMove: " + amountToMove);
            Console.WriteLine("    velocity: " + player.velocity);
            Console.WriteLine("  player.pos: " + player.pos);
            Console.WriteLine("     testPos: " + testPos);
            Console.WriteLine("   pcollider: T:" + player.collider.Top + " B:" + player.collider.Bottom + " L:" + player.collider.Left + " R:" + player.collider.Right);
            Console.WriteLine("   tcollider: T:" + testCollider.Top + " B:" + testCollider.Bottom + " L:" + testCollider.Left + " R:" + testCollider.Right);
            foreach (ICollisionObject s in collided)
            {
                Console.WriteLine("   scollider: T:" + s.collider.Top + " B:" + s.collider.Bottom + " L:" + s.collider.Left + " R:" + s.collider.Right);
            }
            Console.WriteLine("");
        }

        if (bottomWalled > 0 && player.velocity.Y > 0)
        {
            player.velocity.Y = 0;
        }
    }

    public void Hurt(float damage)
    {
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
        }
    }

    public void Heal(float amount)
    {
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
        //energyed = true;
        //activate shockwave animation
        //stun for duration of animation
        Stun(0.25f);
        if (superShockwave < superShockwaveHoldtime)
        {
            energy -= 30;
            game.CameraShake(0.1f, 5f);

            float distance;
            foreach (Enemy e in enemyList)
            {
                distance = Vector2.Distance(player.collider.Center.ToVector2(), e.collider.Center.ToVector2());
                if (distance < e.shockwaveMaxEffectDistance) { distance = e.shockwaveMaxEffectDistance; }
                if (e.collider != Rectangle.Empty && distance <= e.shockwaveEffectiveDistance)
                {
                    heading = (e.collider.Center - player.collider.Center).ToVector2();
                    heading.Normalize();
                    heading.Y += 1f;
                    e.Shockwave(distance, 1, heading);
                }
            }
        }
        else
        {
            energy -= 50;
            game.CameraShake(0.15f, 10f);

            float distance;
            foreach (Enemy e in enemyList)
            {
                distance = Vector2.Distance(player.collider.Center.ToVector2(), e.collider.Center.ToVector2());
                if (distance < e.shockwaveMaxEffectDistance) { distance = e.shockwaveMaxEffectDistance; }
                if (e.collider != Rectangle.Empty && distance <= e.shockwaveEffectiveDistance)
                {
                    heading = (e.collider.Center - player.collider.Center).ToVector2();
                    heading.Normalize();
                    heading.Y += 1f;
                    e.Shockwave(distance, 2, heading);
                }
            }
        }
        superShockwave = 0;
    }

    public void Hitbox()
    {
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
                game.CameraShake(0.075f, 0.1f * attackDamage);
            }
            else
            {
                if (direction == Direction.left)
                {
                    game.CameraSwing(game.swingDuration, game.swingMagnitude, new Vector2(-5,-(float)game.RAND.NextDouble())); //change the Y based on which attack animation is playing
                }
                else
                {
                    game.CameraSwing(game.swingDuration, game.swingMagnitude, new Vector2(5, -(float)game.RAND.NextDouble()));
                }
            }
            attackCollider = Rectangle.Empty;
            attackDamage = 0f;
        }
    }

    public void AttackMovement()
    {

    }

    public void Stun(float duration)
    {
        stunTime += duration;
        inputEnabled = false;
    }
    
    public void Load(PlayerSettings playerSettings)
    {
        pos = playerSettings.initialPosition;
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
        combat = true;
        if (direction == Direction.forward)
        {
            direction = Direction.right;
            idleTime = 0f;
        }
    }

    public void Draw(SpriteBatch sb)
    {
        if (game.debug) { colliderDrawer.Draw(sb, game.nullTex, depth - 0.05f); }
        if (attackCollider != Rectangle.Empty) { attackColliderDrawer.Draw(sb, game.nullTex, depth - 0.05f); }
        //sb.Draw(colliderTex, collider, null, Color.White, randomrotation, Vector2.Zero, SpriteEffects.None, depth);
        /*if (!game.debug) {*/ sb.Draw(tex, displayRect, animationSourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, depth); //}
        if (shielded) { sb.Draw(shieldTex, displayRect, shieldSourceRect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, depth + 0.1f); }
    }
}
