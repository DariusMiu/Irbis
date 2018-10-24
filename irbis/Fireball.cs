using Irbis;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Fireball
{

    public static Texture2D fireballtex;

    //private Texture2D circle;
    public float damage;

    public ParticleSystem particleSystem;
    public ParticleSystem dottiboy;
    public Vector2 truePosition;
    public Point position;
    public Vector2 velocity;
    public int radiusover2;
    public int radius;
    public int radiusSquared;

    public Fireball(Point Position, int Radius, Vector2 Velocity, float Damage)
    {
        radius = Radius;
        radiusover2 = radius / 2;
        radiusSquared = radius * radius;
        damage = Damage;
        position = Position;
        truePosition = position.ToVector2();
        velocity = Velocity;

        //circle = Irbis.Irbis.GenerateCircle((int)(radius * Irbis.Irbis.screenScale), Color.Purple);

        particleSystem = new CircularParticleSystem(Vector2.Zero, velocity, new float[] { 0f, 0.2f, 0.2f }, new float[] { 1, 1, 1, 1 },
            new float[] { 1, 1, 1, 1 }, 0.05f, new float[] { 0.6f, 0.59f, 0.58f, 0.57f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f },
            new Rectangle(position, new Point(4)), new Texture2D[] { fireballtex },
            new Color[] { Color.Transparent, Color.OrangeRed, Color.Black, Color.TransparentBlack },
            new Color[] { Color.Transparent, Color.White, Color.OrangeRed, Color.Transparent }, new int[] { 0, 0, 0, 0 }, 1f, 0f, 1);
        dottiboy = new CircularParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.3f, 0.4f }, new float[] { 1f, 1f, 1f, 1f }, new float[] { 1f, 1f, 1f, 1f },
            0.05f, new float[] { 0.5f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f },
            new Rectangle(position, new Point(2)), new Texture2D[] { Irbis.Irbis.dottex },
            new Color[] { Color.Transparent, Color.Red, Color.Black, Color.TransparentBlack }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
        if (Irbis.Irbis.jamie != null)
        { Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack; }
    }

    public void Update()
    {
        truePosition += velocity * Irbis.Irbis.DeltaTime;
        position = truePosition.ToPoint();
        particleSystem.Position = truePosition;
        dottiboy.Position = truePosition;
        particleSystem.Update();
        dottiboy.Update();
        if (damage > 0 && Collision())
        {
            damage = 0;
            radiusSquared = -1;
            velocity = Vector2.Zero;
            particleSystem.timeToLive = -1;
            dottiboy.timeToLive = -1;
        }
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack, Vector2 Damage)
    {
        if (Irbis.Irbis.DistanceSquared(AttackCollider, position) <= radiusSquared)
        {
            Irbis.Irbis.WriteLine("hit fireball");
            damage = 0;
            radiusSquared = -1;
            particleSystem.timeToLive = -1;
            dottiboy.timeToLive = -1;
        }
        return true;
    }

    public void Death()
    { Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack; }

    public bool Collision()
    {
        foreach (ICollisionObject s in Irbis.Irbis.collisionObjects)
        {
            if (Irbis.Irbis.DistanceSquared(s.Collider, position) <= radiusSquared)
            {
                if (s.GetType() == typeof(Player))
                { ((Player)s).Hurt(damage, true, Irbis.Irbis.Directions(position, Irbis.Irbis.jamie.Collider.Center)); }
                return true;
            }
        }
        return false;
    }

    public void Light(SpriteBatch sb, bool UseColor)
    { }

    public void Draw(SpriteBatch sb)
    {
        switch (Irbis.Irbis.debug)
        {
            case 5:
                goto case 4;
            case 4:
                goto case 3;
            case 3:
                //sb.Draw(circle, location * Irbis.Irbis.screenScale, null, Color.White, 0f, new Vector2(radius * Irbis.Irbis.screenScale), 1, SpriteEffects.None, 0.8001f);
                goto case 2;
            case 2:
                goto case 1;
            case 1:
                goto default;
            default:
                particleSystem.Draw(sb);
                dottiboy.Draw(sb);
                //sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }
}

