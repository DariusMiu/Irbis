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

public class DotTail
{
    public Point Position
    {
        get
        { return tail[current]; }
    }
    int current; // = 0
    Point[] tail;
    float depth;
    Color renderColor;
    Color lightColor;

    public DotTail(Point InitialPosition, int TailLength, Color RenderColor, Color LightColor, float Depth)
    {
        renderColor = RenderColor;
        lightColor = LightColor;
        tail = new Point[TailLength];
        depth = Depth;
        for (int i = 0; i < TailLength; i++)
        { tail[i] = InitialPosition; }
    }

    public void Update(Point NewPosition)
    {
        if (tail[current] != NewPosition)
        {
            current++;
            if (current >= tail.Length)
            { current = 0; }
            tail[current] = NewPosition;
        }
    }

    public void Light(SpriteBatch sb)
    {
        for (int i = tail.Length - 1; i >= 0; i--)
        { sb.Draw(Irbis.Irbis.dottex, tail[i].ToVector2() * Irbis.Irbis.screenScale, new Rectangle(0, 0, 1, 1), Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth); }
    }

    public void Draw(SpriteBatch sb)
    {
        for (int i = tail.Length - 1; i >= 0; i--)
        { sb.Draw(Irbis.Irbis.dottex, tail[i].ToVector2() * Irbis.Irbis.screenScale, new Rectangle(0, 0, 1, 1), renderColor, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth); }
    }
}
