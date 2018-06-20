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

public class Fireball
{

    public static Texture2D fireballtex;

    public Rectangle collider;
    public float damage;

    public ParticleSystem particleSystem;
    public ParticleSystem dottiboy;
    public Vector2 location;
    public Vector2 velocity;

    public Fireball(Rectangle Collider, Vector2 Velocity, float Damage)
    {
        damage = Damage;
        collider = Collider;
        location = Collider.Location.ToVector2();
        velocity = Velocity;
        particleSystem = new ParticleSystem(Vector2.Zero, velocity, new float[] { 0f, 0.2f, 0.2f }, new float[] { 1, 1, 1, 1 },
            new float[] { 1, 1, 1, 1 }, 0.05f, new float[] { 0.6f, 0.59f, 0.58f, 0.57f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f },
            new Rectangle(Collider.Center.X - 2, Collider.Center.Y - 2, 4, 4), new Texture2D[] { fireballtex },
            new Color[] { Color.Transparent, Color.OrangeRed, Color.Black, Color.TransparentBlack },
            new Color[] { Color.Transparent, Color.White, Color.OrangeRed, Color.Transparent }, new int[] { 0, 0, 0, 0 }, 1f, 0f, 1);
        dottiboy = new ParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.3f, 0.4f }, new float[] { 1f, 1f, 1f, 1f }, new float[] { 1f, 1f, 1f, 1f },
            0.05f, new float[] { 0.5f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f }, Collider, new Texture2D[] { Irbis.Irbis.dottex },
            new Color[] { Color.Transparent, Color.Red, Color.Black, Color.TransparentBlack }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
        if (Irbis.Irbis.jamie != null)
        { Irbis.Irbis.jamie.OnPlayerAttack += Enemy_OnPlayerAttack; }
    }

    public void Update()
    {
        location += velocity * Irbis.Irbis.DeltaTime;
        collider.Location = location.ToPoint();
        particleSystem.spawnArea.Location = new Point(collider.Center.X - 2, collider.Center.Y - 2);
        dottiboy.spawnArea.Location = collider.Location;
        particleSystem.Update();
        dottiboy.Update();
        if (damage > 0 && Collision(collider))
        {
            damage = 0;
            collider = Rectangle.Empty;
            velocity = Vector2.Zero;
            particleSystem.timeToLive = -1;
            dottiboy.timeToLive = -1;
        }
    }

    public bool Enemy_OnPlayerAttack(Rectangle AttackCollider, Attacking Attack, Vector2 Damage)
    {
        if (AttackCollider.Intersects(collider))
        {
            Irbis.Irbis.WriteLine("hit fireball");
            damage = 0;
            collider = Rectangle.Empty;
            particleSystem.timeToLive = -1;
            dottiboy.timeToLive = -1;
        }
        return true;
    }

    public void Death()
    { Irbis.Irbis.jamie.OnPlayerAttack -= Enemy_OnPlayerAttack; }

    public bool Collision(Rectangle collider)
    {
        foreach (ICollisionObject s in Irbis.Irbis.collisionObjects)
        {
            if (collider.Intersects(s.Collider))
            {
                if (s.GetType() == typeof(Player))
                { ((Player)s).Hurt(damage, true, Irbis.Irbis.Directions(collider.Center, Irbis.Irbis.jamie.Collider.Center)); }
                return true;
            }
        }
        return false;
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {

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
                RectangleBorder.Draw(sb, collider, Color.Purple, true);
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

