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
    Fireball[] fireballs;

    public Nova(Rectangle Collider, int Count, float AngleOffset, float Velocity, float Damage)
    {
        fireballs = new Fireball[Count];

        for (int i = 0; i < Count; i++)
        {
            float angle = (i / (float)Count) * MathHelper.TwoPi + AngleOffset;
            fireballs[i] = new Fireball(Collider, new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * Velocity, Damage);
        }
    }

    public void Update()
    {
        for (int i = fireballs.Length - 1; i >= 0; i--)
        { fireballs[i].Update(); }
    }

    public void Death()
    {
        for (int i = fireballs.Length - 1; i >= 0; i--)
        { fireballs[i].Death(); }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        foreach (Fireball f in fireballs)
        { f.Light(sb, UseColor); }
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
                goto default;
            default:
                for (int i = fireballs.Length - 1; i >= 0; i--)
                { fireballs[i].Draw(sb); }
                //sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth);
                break;
        }
    }

}
