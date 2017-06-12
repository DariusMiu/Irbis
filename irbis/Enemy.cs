using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



public class Enemy : ICollisionObject
{
    public Rectangle collider
    {
        get
        {
            return Collider;
        }
        set
        {
            Collider = value;
        }
    }
    public Rectangle Collider;

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
    public Vector2 pos;
    //public Vector2 previousPos;
    public Vector2 velocity;

    public float health;

    float speed;
    public float stunned;
    //floatdefaultSpeed;
    float wanderSpeed;
    bool previouslyWandered;
    public float wanderTime;
    float jumpTime;
    //floatjumpTimeMax;
    float deltaTime;
    float timeSinceLastFrame;
    float idleTime;
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

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 100f;
    public static float movementLerpAir = 5f;

    List<ICollisionObject> collided;
    List<Side> sideCollided;
    Irbis.Irbis game;
    GameTime gameTime;

    public Enemy(Texture2D t, Vector2 iPos, Irbis.Irbis masterGame)
	{
        game = masterGame;

        tex = t;
        colliderDrawer = new RectangleBorder(collider, Color.Magenta, 0.19f);

        AIenabled = true;

        pos = iPos * 32;
        direction = Direction.forward;
        location = Location.air;
        activity = Activity.idle;
        AIactivity = AI.wander;
        wanderSpeed = 200f;
        wanderTime = 0f;
        previouslyWandered = false;

        speed /*= defaultSpeed*/ = 500f;
        jumpTime = 0;
        //jumpTimeMax = 0.25f;
        animationNoLoop = false;
        XcolliderOffset = 44;
        YcolliderOffset = 38;
        colliderWidth = 38;
        colliderHeight = 86;

        pos.X -= XcolliderOffset;
        pos.Y -= YcolliderOffset;

        health = 100f;
        lastHitByAttackID = -1;

        stunned = 0;


        shockwaveMaxEffectDistance = game.geralt.shockwaveMaxEffectDistance;
        shockwaveEffectiveDistance = game.geralt.shockwaveEffectiveDistance;
        shockwaveStunTime = game.geralt.shockwaveStunTime;
        shockwaveKnockback = game.geralt.shockwaveKnockback;


        displayRect = new Rectangle((int)pos.X, (int)pos.Y, 128, 128);
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
        //animationFrames[7] = 2;             //jumpleft
        //animationFrames[8] = 2;             //jumpright
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

    public void Update(GameTime GT)
    {
        deltaTime = (float)GT.ElapsedGameTime.TotalSeconds;

        if (stunned <= 0)
        {
            stunned = 0;
            if (AIenabled)
            {
                switch (AIactivity)
                {
                    default:
                        Wander();
                        break;
                }
            }
            PlayerCollision(game.geralt, this);
        }
        else
        {
            stunned -= deltaTime;
            if (bottomWalled > 0 && Math.Abs(velocity.X) > 0)
            {
                velocity.X = game.Lerp(velocity.X, 0, movementLerpBuildup * deltaTime);
            }
        }

        PlayerAttackCollision(game.geralt, this);
        Movement();
        CalculateMovement();
        Animate();
        Collision(this, game.squareList);
    }

    public void Respawn(Vector2 initialPos)
    {
        pos = initialPos * 32;
        pos.X -= XcolliderOffset;
        pos.Y -= YcolliderOffset;
        velocity = Vector2.Zero;
        health = 100f;
        CalculateMovement();
    }

    public void Wander()
    {
        
        AIactivity = AI.wander;

        if (wanderTime > 0)
        {
            wanderTime -= deltaTime;
        }
        else
        {
            if (previouslyWandered)
            {
                input.X = 0;
                input.Y = 0;
                previouslyWandered = false;
            }
            else
            {
                speed = wanderSpeed;
                if (rightWalled <= 0 && leftWalled <= 0)
                {
                    if (game.RAND.NextDouble() > 0.5)
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
                else if (rightWalled <= 0)
                {
                    input.X++;
                    direction = Direction.right;
                }
                else if (leftWalled <= 0)
                {
                    input.X--;
                    direction = Direction.left;
                }
                
                previouslyWandered = true;
            }
            
            wanderTime = (float)game.RAND.NextDouble();
            wanderTime += 0.5f;
        }


        if (bottomWalled > 0)                                                               //movement
        {
            velocity.X = game.Lerp(velocity.X, input.X * speed, 10f * deltaTime);
        }
        else if (velocity.X == 0)
        {
            velocity.X = input.X * 0.1f * speed;
        }

    }

    public void Movement()
    {

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
            velocity.Y = -speed;
        }
        if (topWalled > 0 && velocity.Y < 0)
        {
            velocity.Y = 0;
            jumpTime = 0;
        }
        if (bottomWalled <= 0 && jumpTime <= 0)
        {
            velocity.Y += Irbis.Irbis.gravity * deltaTime;
        }

        //previousPos = pos;
        pos += velocity * deltaTime;
    }

    public void CalculateMovement()
    {
        displayRect.X = (int)pos.X;
        displayRect.Y = (int)pos.Y;
        Collider.X = (int)pos.X + XcolliderOffset;
        Collider.Y = (int)pos.Y + YcolliderOffset;
        Collider.Width = colliderWidth;
        Collider.Height = colliderHeight;
    }

    public void Animate()
    {
        previousAnimation = currentAnimation;                                                   //animator
        timeSinceLastFrame += deltaTime;
        if (timeSinceLastFrame > animationSpeed[currentAnimation])
        {
            currentFrame++;
            timeSinceLastFrame = 0;
        }

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

        if (input.X != 0 || input.Y != 0)
        {
            idleTime = 0;
            if (input.X != 0)
            {
                if (bottomWalled > 0)
                {
                    activity = Activity.running;
                }
            }
            if (input.Y != 0)
            {
                //nothing, yet
            }
        }
        else
        {
            activity = Activity.idle;
        }

        switch (activity)
        {
            case Activity.idle:
                if (direction != Direction.forward)
                {
                    currentAnimation = 3;
                }
                idleTime += deltaTime;
                if (idleTime > 10 && currentAnimation == 0)
                {
                    idleTime = 0;
                    currentAnimation = 1;
                    currentFrame = 0;
                    animationNoLoop = true;
                }
                else if (idleTime > 10)
                {
                    idleTime = 0;
                    currentAnimation = 0;
                    currentFrame = 0;
                    direction = Direction.forward;
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

                break;
            case Activity.landing:

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
        }


        //animationSourceRect = new Rectangle(64 * currentFrame, 64 * currentAnimation, 64, 64);
        animationSourceRect.X = 64 * currentFrame;
        animationSourceRect.Y = 64* currentAnimation;
    }

    public void PlayerCollision(Player player, Enemy enemy)
    {
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

    public void PlayerAttackCollision(Player player, Enemy enemy)
    {
        if (player.attackID != lastHitByAttackID)
        {
            if (player.attackCollider != Rectangle.Empty && player.attackCollider.Intersects(enemy.collider))
            {
                lastHitByAttackID = player.attackID;
                player.attackHit = true;
                Hurt(player.attackDamage);
                Stun(0.5f); 
                velocity.Y = -25f;
                velocity.X = 300f;
                player.heading = (collider.Center - player.collider.Center).ToVector2();
                player.heading.Normalize();
                velocity = velocity * player.heading;
            }
        }
    }

    public void Collision(Enemy player, List<Square> colliderList)
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
                    if (negAmountToMove.Y > s.collider.Top - player.collider.Bottom && (player.velocity.Y * timeSinceLastFrame) >= -(s.collider.Top - player.collider.Bottom))
                    {
                        negAmountToMove.Y = s.collider.Top - player.collider.Bottom;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.right))                               //RIGHT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.right);
                    player.rightWalled++;
                    if (negAmountToMove.X > s.collider.Left - player.collider.Right && (player.velocity.X * timeSinceLastFrame) >= -(s.collider.Left - player.collider.Right))
                    {
                        negAmountToMove.X = s.collider.Left - player.collider.Right;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.left))                                //LEFT
                {
                    collided.Add(s);
                    sideCollided.Add(Side.left);
                    player.leftWalled++;
                    if (amountToMove.X < s.collider.Right - player.collider.Left && (player.velocity.X * timeSinceLastFrame) <= -(s.collider.Right - player.collider.Left))
                    {
                        amountToMove.X = s.collider.Right - player.collider.Left;
                    }
                }
                if (game.IsTouching(player.collider, s.collider, Side.top))                                 //UP
                {
                    collided.Add(s);
                    sideCollided.Add(Side.top);
                    player.topWalled++;
                    if (amountToMove.Y < s.collider.Bottom - player.collider.Top && (player.velocity.Y * timeSinceLastFrame) <= -(s.collider.Bottom - player.collider.Top))
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
        health -= damage;
        //game.CameraShake(0.075f, 0.05f * damage);
    }

    public void Stun(float duration)
    {
        stunned += duration;
        AIactivity = AI.stunned;
    }

    public void Shockwave(float distance, float power, Vector2 heading)
    {
        Stun(((shockwaveEffectiveDistance - distance) * 2) / shockwaveStunTime);
        velocity = shockwaveKnockback * heading * (shockwaveEffectiveDistance - distance) * power;
    }

    public void Draw(SpriteBatch sb)
    {
        if (game.debug) { colliderDrawer.Draw(sb, game.nullTex, collider); }
        //sb.Draw(colliderTex, edgeCollider, Color.Blue);
        sb.Draw(tex, displayRect, animationSourceRect, Color.White);
    }
}
