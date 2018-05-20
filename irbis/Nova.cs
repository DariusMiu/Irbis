using Irbis;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

public class Nova
{
    public Rectangle[] Colliders
    {
        get
        { return colliders; }
    }

    static Texture2D fireballtex;

    ParticleSystem[] particleSystems;
    ParticleSystem[] dottiboys;
    Rectangle[] colliders;
    Vector2[] locations;
    Vector2[] velocities;
    float[] damage;

    public Nova(Rectangle Collider, int Count, float AngleOffset, float Velocity, float Damage)
    {
        fireballtex = Irbis.Irbis.LoadTexture("fireball");
        damage = new float[Count];
        particleSystems = new ParticleSystem[Count];
        dottiboys = new ParticleSystem[Count];
        colliders = new Rectangle[Count];
        locations = new Vector2[Count];
        velocities = new Vector2[Count];
        for (int i = 0; i < Count; i++)
        {
            damage[i] = Damage;
            colliders[i] = Collider;
            locations[i] = Collider.Location.ToVector2();
            float angle = (i / (float)Count) * MathHelper.TwoPi + AngleOffset;
            velocities[i] = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * Velocity;
            particleSystems[i] = new ParticleSystem(Vector2.Zero, velocities[i], new float[] { 0f, 0.2f, 0.2f }, new float[] { 1, 1, 1, 1 },
                new float[] { 1, 1, 1, 1 }, 0.05f, new float[] { 0.6f, 0.59f, 0.58f, 0.57f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f },
                new Rectangle(Collider.Center.X - 2, Collider.Center.Y - 2, 4, 4), new Texture2D[] { fireballtex },
                new Color[] { Color.Transparent, Color.OrangeRed, Color.Black, Color.TransparentBlack },
                new Color[] { Color.Transparent, Color.White, Color.OrangeRed, Color.Transparent }, new int[] { 0, 0, 0, 0 }, 1f, 0f, 1);
            dottiboys[i] = new ParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.3f, 0.4f }, new float[] { 1f, 1f, 1f, 1f }, new float[] { 1f, 1f, 1f, 1f },
                0.05f, new float[] { 0.5f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f}, Collider, new Texture2D[] { Irbis.Irbis.dottex },
                new Color[] { Color.Transparent, Color.Red, Color.Black, Color.TransparentBlack }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
        }
    }

    public void Update()
    {
        if (particleSystems.Length == colliders.Length && colliders.Length == velocities.Length && velocities.Length == dottiboys.Length)
        {
            for (int i = colliders.Length - 1; i >= 0; i--)
            {
                locations[i] += velocities[i] * Irbis.Irbis.DeltaTime;
                colliders[i].Location = locations[i].ToPoint();
                particleSystems[i].spawnArea.Location = new Point(colliders[i].Center.X - 2, colliders[i].Center.Y - 2);
                dottiboys[i].spawnArea.Location = colliders[i].Location;
                particleSystems[i].Update();
                dottiboys[i].Update();
                if (damage[i] > 0 && Collision(colliders[i], i))
                {
                    damage[i] = 0;
                    colliders[i] = Rectangle.Empty;
                    velocities[i] = Vector2.Zero;
                    particleSystems[i].timeToLive = -1;
                    dottiboys[i].timeToLive = -1;
                }
            }
        }
        else
        { throw new ArraysNotSameLengthException(); }
    }

    public bool Collision(Rectangle collider, int index)
    {
        foreach (ICollisionObject s in Irbis.Irbis.collisionObjects)
        {
            if (collider.Intersects(s.Collider))
            {
                if (s.GetType() == typeof(Player))
                { ((Player)s).Hurt(damage[index], true); }
                return true;
            }
        }
        return false;
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack)
    {
        Irbis.Irbis.WriteLine("nova response:\nAttack:" + Attack);
        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            if (AttackCollider.Intersects(colliders[i]))
            {
                Irbis.Irbis.WriteLine("hit fireball[" + i + "]");
                damage[i] = 0;
                colliders[i] = Rectangle.Empty;
                velocities[i] = Vector2.Zero;
                particleSystems[i].timeToLive = -1;
                dottiboys[i].timeToLive = -1;
            }
        }
        return true;
    }

    public bool Enemy_OnPlayerShockwave(Point Origin, int RangeSquared, int Range, float Power)
    {

        return true;
    }

    public void Death()
    {
        Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack;
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        foreach (ParticleSystem p in particleSystems)
        { p.Light(sb, UseColor); }
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
                foreach(Rectangle r in colliders)
                { RectangleBorder.Draw(sb, r, Color.Purple, true); }
                goto case 2;
            case 2:
                goto case 1;
            case 1:
                goto default;
            default:
                foreach (ParticleSystem p in particleSystems)
                { p.Draw(sb); }
                foreach (ParticleSystem p in dottiboys)
                { p.Draw(sb); }
                //sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

}
