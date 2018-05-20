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
        Shockwave = 5,

        Dying = 19,
        Misc = 20,
    }

    public Vector2 TrueCenter
    {
        get
        { return new Vector2(position.X + standardCollider.X + (standardCollider.Width / 2f), position.Y + standardCollider.Y + (standardCollider.Height / 2f)); }
        set
        { position = new Vector2(value.X - standardCollider.X - (standardCollider.Width / 2f), value.Y - standardCollider.Y - (standardCollider.Height / 2f)); }
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
        { return maxHealth; }
        set
        { maxHealth = value; }
    }
    private float maxHealth;

    public float SpeedModifier
    {
        get
        { return speedModifier; }
        set
        { speedModifier = value; }
    }
    private float speedModifier;

    public List<Enchant> ActiveEffects
    {
        get
        { return activeEffects; }
    }
    private List<Enchant> activeEffects;

    public string Name
    {
        get
        { return name; }
    }
    private string name = "Wizard Guy (bweam)";

    public bool AIenabled
    {
        get
        { return aiEnabled; }
        set
        { aiEnabled = value; }
    }
    public bool aiEnabled = true;

    public float Mass
    {
        get
        { return mass; }
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

    public float speed;
    public float defaultSpeed;
    public WizardActivity previousActivity;
    public Point prevInput;
    public int nextAnimation;
    public float idleTime;
    public Print animationFrame;

    Texture2D tex;
    public Rectangle displayRect;
    public Rectangle animationSourceRect;
    public Rectangle standardCollider;

    public float terminalVelocity = 5000f;

    float depth;

    public float stunned;
    public float wanderTime;
    public float initialWanderTime;
    public float timeSinceLastFrame;
    int currentFrame;
    public int currentAnimation;
    int previousAnimation;
    public float[] animationSpeed = new float[10];
    public int[] animationFrames = new int[10];
    bool animationNoLoop;
    public Point input;
    public bool frameInput;

    public float shockwaveStunTime;
    public Vector2 shockwaveKnockback;

    public Direction direction;
    public Location location;
    public WizardActivity activity;

    public int combatCheckDistanceSqr;
    public int persueCheckDistanceSqr;
    public bool combat;

    public static float movementLerpBuildup = 10f;
    public static float movementLerpSlowdown = 50f;

    public Rectangle attackCollider;
    public int attackColliderWidth = 100;
    public int attackColliderHeight = 70;

    public float attackDamage;
    public float attack1Damage;

    public float attackCooldown;
    public float attackCooldownTimer;

    Vector2[] teleportPoints;
    int previousTeleport;

    public BossState bossState = 0;
    public int combatPhase = 0;
    Vector2 spawnPoint;
    Vector2 initialPoint;
    float stepTime;
    float stepTimeMax = 5f;

    int lazerindex = 0;
    List<Lazor> lazers = new List<Lazor>();
    Texture2D[] lazertextures = new Texture2D[5];

    float lazersFastTick;
    float novaFastTick = 62500;
    float boltsFastTick = 10000;
    float teleportFastTick = 2500;

    Rectangle bossArena;
    List<Nova> firenovas = new List<Nova>();
    List<float> firenovaTTL = new List<float>();
    ChargedBolts bolts = new ChargedBolts(new Vector2(100), 0, 0, 0, 0, 0, 0);

    Vector2 explosionLocation;
    float explosionDamage = 50;
    float explosionRadius = 64;
    float explosionRadiusSQR = 64 * 64;
    bool exploding;
    float timeSinceLastExplosionFrame;
    float explosionAnimationSpeed = 0.05f;
    int currentExplosionFrame;
    float explosionVelocity = 300;
    Rectangle explosionSourceRect = new Rectangle(Point.Zero, new Point(Irbis.Irbis.explosiontex.Height));

    float playerDistanceSQR;

    public int[] state = new int[5];
    float[] cooldown = new float[6];
    public float[] timer = new float[6];


    public WizardGuy(Texture2D t, int? iPos, float enemyHealth, float enemyDamage, Vector2[] TeleportPoints, Rectangle? BossArena, float drawDepth)
    {
        timer[0] = cooldown[0] = .5f; // idle
        timer[1] = cooldown[1] = 17f; // teleport
        timer[2] = cooldown[2] = 08f; // nova
        timer[3] = cooldown[3] = 09f; // bolt
        timer[4] = cooldown[4] = 15f; // lazers
      /*timer[5]*/ cooldown[5] = .3f; // lazer delay 





        idleTime = 0f;

        animationFrame = new Print((int)(Irbis.Irbis.font.charHeight * 2f * Irbis.Irbis.textScale), Irbis.Irbis.font, Color.White, true, Point.Zero, Direction.Left, drawDepth + 0.001f);

        tex = t;
        AIenabled = true;

        depth = drawDepth;

        direction = Direction.Forward;
        location = Location.Air;
        wanderTime = 1f;
        initialWanderTime = 0f;

        animationNoLoop = false;
        standardCollider.Location = new Point(47, 37);
        collider.Size = trueCollider.Size = standardCollider.Size = new Point(32, 56);

        maxHealth = health = enemyHealth;

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

        lazertextures[0] = Irbis.Irbis.LoadTexture("lazor0");
        lazertextures[1] = Irbis.Irbis.LoadTexture("lazor45");
        lazertextures[2] = Irbis.Irbis.LoadTexture("lazor90");
        lazertextures[3] = Irbis.Irbis.LoadTexture("lazor135");
        lazertextures[4] = Irbis.Irbis.LoadTexture("lazor180");

        shockwaveStunTime = Irbis.Irbis.jamie.shockwaveStunTime;
        shockwaveKnockback = Irbis.Irbis.jamie.shockwaveKnockback;

        displayRect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
        animationSourceRect = new Rectangle(0, 0, 128, 128);
        currentFrame = 0;
        currentAnimation = 0;
        animationSpeed[0] = 0.1f;
        for (int i = 1; i < animationSpeed.Length; i++)
        { animationSpeed[i] = animationSpeed[0]; }
         animationSpeed[4] = 0.0375f;
        animationFrames[0] = 0; // idle
        animationFrames[1] = 0; // charging
        animationFrames[2] = 0; // casting
        animationFrames[3] = 0; // hurt
        animationFrames[4] = 4; // teleport


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

        if (TeleportPoints == null || TeleportPoints.Length <= 1)
        {
            teleportPoints = new Vector2[5];
            teleportPoints[0] = new Vector2(bossArena.X + bossArena.Width * 0.1f, bossArena.Center.Y);
            teleportPoints[1] = new Vector2(bossArena.X + bossArena.Width * 0.3f, bossArena.Center.Y);
            teleportPoints[2] = new Vector2(bossArena.X + bossArena.Width * 0.5f, bossArena.Center.Y);
            teleportPoints[3] = new Vector2(bossArena.X + bossArena.Width * 0.7f, bossArena.Center.Y);
            teleportPoints[4] = new Vector2(bossArena.X + bossArena.Width * 0.9f, bossArena.Center.Y);
        }
        else
        { teleportPoints = TeleportPoints; }

        if (iPos == null)
        {
            if (TeleportPoints != null && TeleportPoints.Length == 1)
            {
                previousTeleport = -1;
                initialPoint = TeleportPoints[0];
                TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
            }
            else
            {
                previousTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length);
                initialPoint = teleportPoints[previousTeleport];
                TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
            }
        }
        else
        {
            previousTeleport = (int)iPos;
            initialPoint = teleportPoints[previousTeleport];
            TrueCenter = spawnPoint = new Vector2(initialPoint.X, initialPoint.Y - bossArena.Height);
        }

        Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave += Enemy_OnPlayerShockwave;
    }

    public bool Update()
    {
        prevInput = input;
        input = Point.Zero;
        frameInput = false;
        playerDistanceSQR = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.Collider, TrueCenter);

        if (Irbis.Irbis.GetMouseState.LeftButton == ButtonState.Pressed && collider.Contains(Irbis.Irbis.WorldSpaceMouseLocation))
        { position = new Vector2(Irbis.Irbis.WorldSpaceMouseLocation.X - standardCollider.X - (collider.Width/2), Irbis.Irbis.WorldSpaceMouseLocation.Y - standardCollider.Y - (collider.Height/2)); }


        switch (bossState)
        {
            case BossState.Spawn:       // 0
                stepTime += Irbis.Irbis.DeltaTime;
                TrueCenter = Irbis.Irbis.SmootherStep(spawnPoint, initialPoint, (stepTime / stepTimeMax));
                if (stepTime >= stepTimeMax)
                { bossState++; SetAnimation(4, true); }
                break;
            case BossState.Entrance:    // 1
                bossState++;
                break;
            case BossState.Engage:      // 2
                bossState++;
                break;
            case BossState.Combat:      // 3
                if (Irbis.Irbis.jamie != null)
                {
                    if (aiEnabled && timer[0] <= 0 && stunned <= 0)
                    {
                        if (timer[4] <= 0)
                        {
                            if (timer[5] <= 0)
                            {
                                switch (lazerindex)
                                {
                                    case 0:
                                        lazers.Clear();
                                        lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y - 15), new Point(30)), new Vector2(2000, 0), new Texture2D[] { lazertextures[0], Irbis.Irbis.dottex }, 20));
                                        timer[5] = cooldown[5];
                                        lazerindex++;
                                        break;
                                    case 1:
                                        lazers.Add(new Lazor(new Rectangle(new Point(collider.Right, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(1, 1)) * 2000f, new Texture2D[] { lazertextures[1], Irbis.Irbis.dottex }, 20));
                                        timer[5] = cooldown[5];
                                        lazerindex++;
                                        break;
                                    case 2:
                                        lazers.Add(new Lazor(new Rectangle(new Point(collider.Center.X - 15, collider.Center.Y + 15), new Point(30)), new Vector2(0, 2000), new Texture2D[] { lazertextures[2], Irbis.Irbis.dottex }, 20));
                                        timer[5] = cooldown[5];
                                        lazerindex++;
                                        break;
                                    case 3:
                                        lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y + 15), new Point(30)), Vector2.Normalize(new Vector2(-1, 1)) * 2000f, new Texture2D[] { lazertextures[3], Irbis.Irbis.dottex }, 20));
                                        timer[5] = cooldown[5];
                                        lazerindex++;
                                        break;
                                    case 4:
                                        lazers.Add(new Lazor(new Rectangle(new Point(collider.Left - 30, collider.Center.Y - 15), new Point(30)), new Vector2(-2000, 0), new Texture2D[] { lazertextures[4], Irbis.Irbis.dottex }, 20));
                                        timer[4] = cooldown[4];
                                        timer[0] = cooldown[0];
                                        lazerindex = 0;
                                        break;
                                }
                            }
                        }
                        else if (timer[1] <= 0)
                        {
                            SetAnimation(4, true);
                            // play teleport animation
                            // trigger "explosion"
                            // set position on noloop

                            timer[1] = cooldown[1];
                            timer[0] = cooldown[0];
                        }
                        else if (timer[3] <= 0)
                        {
                            bolts = new ChargedBolts(TrueCenter, 4, (MathHelper.Pi / 8), 1, 100, 1, 30);
                            timer[3] = cooldown[3];
                            timer[0] = cooldown[0];
                        }
                        else if (timer[2] <= 0)
                        {
                            firenovas.Add(new Nova(new Rectangle(new Point(collider.Center.X - 5, collider.Center.Y - 5), new Point(10)), 12, 0, 100, 20));
                            firenovaTTL.Add(30f);
                            timer[2] = cooldown[2];
                            timer[0] = cooldown[0];
                        }

                        if (timer[1] > 0) // teleport
                        { timer[1] -= Irbis.Irbis.DeltaTime; }
                        if (timer[2] > 0) // nova
                        { timer[2] -= Irbis.Irbis.DeltaTime; }
                        if (timer[3] > 0) // bolts
                        { timer[3] -= Irbis.Irbis.DeltaTime; }
                        if (timer[4] > 0) // lazers
                        { timer[4] -= Irbis.Irbis.DeltaTime; }
                        if (timer[5] > 0) // lazer delay
                        { timer[5] -= Irbis.Irbis.DeltaTime; }

                        if (teleportFastTick > playerDistanceSQR)
                        { timer[1] -= Irbis.Irbis.DeltaTime * 3; }
                        else if (boltsFastTick > playerDistanceSQR)
                        { timer[3] -= Irbis.Irbis.DeltaTime * 3; }
                        else if (novaFastTick > playerDistanceSQR)
                        { timer[2] -= Irbis.Irbis.DeltaTime * 3; }
                        else if (lazersFastTick > playerDistanceSQR)
                        { timer[4] -= Irbis.Irbis.DeltaTime * 3; }
                    }

                    if (timer[0] > 0)
                    { timer[0] -= Irbis.Irbis.DeltaTime; }
                }
                break;
            case BossState.Disengage:   // 4

                break;
            case BossState.Death:       // 5

                break;
        }

        for (int i = firenovas.Count - 1; i >= 0; i--)
        {
            firenovaTTL[i] -= Irbis.Irbis.DeltaTime;
            if (firenovaTTL[i] > 0)
            { firenovas[i].Update(); }
            else
            {
                firenovas[i].Death();
                firenovas.RemoveAt(i);
                firenovaTTL.RemoveAt(i);
            }
        }
        bolts.Update();
        for (int i = lazers.Count - 1; i >= 0; i--)
        {
            if (lazers[i].Update())
            { lazers.RemoveAt(i); }
        }


        //Movement();
        CalculateMovement();
        Animate();
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

    public void Death()
    {
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
        Irbis.Irbis.jamie.OnPlayerShockwave -= Enemy_OnPlayerShockwave;
        for (int i = firenovas.Count - 1; i >= 0; i--)
        { firenovas[i].Death(); }
        //Irbis.Irbis.jamie.OnPlayerShockwave -= firenova.Enemy_OnPlayerShockwave;
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

    public void CalculateMovement()
    {
        //displayRect = new Rectangle((int)position.X, (int)position.Y, 128, 128);
        //collider = new Rectangle((int)position.X + XcolliderOffset, (int)position.Y + YcolliderOffset, colliderSize.X, colliderSize.Y);
        trueCollider.X = (int)Math.Round((decimal)position.X) + standardCollider.X;
        trueCollider.Y = (int)Math.Round((decimal)position.Y) + standardCollider.Y;
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
                    case 4:
                        int nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length);
                        while (previousTeleport == nextTeleport)
                        { nextTeleport = Irbis.Irbis.RandomInt(teleportPoints.Length); }
                        previousTeleport = nextTeleport;
                        Explode();
                        TrueCenter = teleportPoints[previousTeleport];
                        SetAnimation(0, false);
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


        if (exploding)
        {
            timeSinceLastExplosionFrame += Irbis.Irbis.DeltaTime;
            if (timeSinceLastExplosionFrame >= explosionAnimationSpeed)
            {
                timeSinceLastExplosionFrame -= explosionAnimationSpeed;
                currentExplosionFrame++;
            }
            if (currentExplosionFrame * Irbis.Irbis.explosiontex.Height >= Irbis.Irbis.explosiontex.Width)
            {
                currentExplosionFrame = 0;
                timeSinceLastExplosionFrame = 0;
                exploding = false;
            }
            explosionSourceRect.X = currentExplosionFrame * Irbis.Irbis.explosiontex.Height;
        }


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

    protected void Explode()
    {
        exploding = true;
        explosionLocation = TrueCenter;
        if (playerDistanceSQR <= explosionRadiusSQR)
        {
            float Distance = (float)Math.Sqrt(playerDistanceSQR);
            Irbis.Irbis.jamie.Hurt(((explosionRadius - Distance) / explosionRadius) * explosionDamage, true);
            Irbis.Irbis.jamie.velocity += Vector2.Normalize(Irbis.Irbis.jamie.TrueCenter - explosionLocation) * explosionVelocity;
        }
    }

    public void Dying()
    {
        activity = WizardActivity.Dying;
        previousActivity = activity;
        trueCollider.Size = collider.Size = new Point(60, 32);
        standardCollider.Location = new Point(44, 93);
        SetAnimation(14, true);
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
                animationFrame.Draw(sb, ((position + (standardCollider.Location - new Point(24)).ToVector2()) * Irbis.Irbis.screenScale).ToPoint());
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
                goto case 1;
            case 1:
                goto default;
            default:
                for (int i = firenovas.Count - 1; i >= 0; i--)
                { firenovas[i].Draw(sb); }
                bolts.Draw(sb);
                foreach (Lazor l in lazers)
                { l.Draw(sb); }
                if (exploding)
                { sb.Draw(Irbis.Irbis.explosiontex, explosionLocation * Irbis.Irbis.screenScale, explosionSourceRect, Color.SteelBlue, 0f, new Vector2(64f), Irbis.Irbis.screenScale, SpriteEffects.None, depth); }
                sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        for (int i = firenovas.Count - 1; i >= 0; i--)
        { firenovas[i].Light(sb, UseColor); }
        foreach (Lazor l in lazers)
        { l.Light(sb, UseColor); }
    }
}
