﻿using Irbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Lazor
{
    Vector2 position;
    Vector2 velocity = Vector2.Zero;
    Vector2 force;
    ParticleSystem[] particleSystems;
    float damage;
    bool dead;
    int radius;
    int radiusSquared;

    public Lazor(Vector2 Position, int Radius, Vector2 Force, Texture2D[] Textures, float Damage)
    {
        radius = Radius;
        radiusSquared = radius * radius;
        position = Position;
        force = Force;
        damage = Damage;

        particleSystems = new ParticleSystem[2];
        particleSystems[0] = new CircularParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.3f, 0.25f }, new float[] { 1f,1f,1f,1f }, new float[] { 1f,1f,1f,1f },
            0.02f, new float[] { 0.5f, 0.45f, 0.4f }, new float[] { 1f, 1f, 0.01f, 0.1f, 0.1f, 0.1f, 0.1f },
            new Rectangle(position.ToPoint(), new Point(radius)), new Texture2D[] { Textures[0] },
            new Color[] { Color.Transparent, Color.Cyan, Color.DarkCyan, Color.TransparentBlack }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);

        particleSystems[1] = new CircularParticleSystem(Vector2.Zero, Vector2.Zero, new float[] { 0.1f, 0.8f, 0.25f }, new float[] { 1f,1f,1f,1f }, new float[] { 1f,1f,1f,1f },
            0.05f, new float[] { 0.5f }, new float[] { 1f, 10f, 0.01f, 0f, 0f, 0.1f, 0.1f },
            new Rectangle(position.ToPoint(), new Point(radius)), new Texture2D[] { Textures[1] },
            new Color[] { Color.Transparent, Color.Cyan, Color.White, Color.Transparent }, new Color[] { Color.White, Color.White, Color.White }, new int[] { 0, 0, 0, 0 }, 0.05f, 0f, 2);
    }

    public bool Update()
    {
        velocity += force * Irbis.Irbis.DeltaTime;
        position += velocity * Irbis.Irbis.DeltaTime;
        if (Collision())
        {
            dead = true;
            damage = 0;
            foreach (ParticleSystem P in particleSystems)
            { P.timeToLive = -1; }
        }
        else if (dead)
        {
            if (particleSystems[0].Update() && particleSystems[1].Update())
            { return true; }
        }
        else
        {
            foreach (ParticleSystem P in particleSystems)
            { P.Update(); P.Position = position; }
        }
        return false;
    }

    public bool Collision()
    {
        foreach (ICollisionObject s in Irbis.Irbis.collisionObjects)
        {
            if (s.GetType() != typeof(WizardGuy) && Irbis.Irbis.DistanceSquared(s.Collider, position) <= radiusSquared)
            {
                if (s.GetType() == typeof(Player))
                { ((Player)s).Hurt(damage, true, Irbis.Irbis.Directions(Irbis.Irbis.jamie.Collider.Center.ToVector2(), position)); }
                return true;
            }
        }
        return false;
    }

    public void Draw(SpriteBatch sb)
    {
        foreach (ParticleSystem P in particleSystems)
        { P.Draw(sb); }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        //foreach (ParticleSystem P in particleSystems)
        //{ P.Light(sb, UseColor); }
    }
}
