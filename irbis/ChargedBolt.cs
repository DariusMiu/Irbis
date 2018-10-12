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

public class ChargedBolt
{
    public Point Center
    {
        get
        { return center; }
    }

    DotTail[] tails;
    Vector2[] positions;
    Vector2[] initialPositions;
    Vector2[] targets;
    float[] lerptimes;
    float[] initialLerptimes;
    float speed;
    int count;
    public Vector2 position;
    private Texture2D circle;
    Point center;
    int radius;

    public ChargedBolt(Point Center, int Radius, int TailsCount, float Speed, Color RenderColor, Color LightColor)
    {
        count = TailsCount;
        speed = Speed;

        tails = new DotTail[count];
        positions = new Vector2[count];
        initialPositions = new Vector2[count];
        targets = new Vector2[count];
        lerptimes = new float[count];
        initialLerptimes = new float[count];

        center = Center;
        position = Center.ToVector2();
        radius = Radius;

        for (int i = 0; i < count; i++)
        {
            tails[i] = new DotTail(Irbis.Irbis.RandomPoint(radius).ToPoint(), (int)(5 * Irbis.Irbis.screenScale), RenderColor, LightColor, 0.8f);
            initialPositions[i] = positions[i] = tails[i].Position.ToVector2();
            targets[i] = Irbis.Irbis.RandomPoint(radius);
            initialLerptimes[i] = lerptimes[i] = Vector2.Distance(positions[i], targets[i]) / speed;
        }
    }

    public void Update()
    {
        for (int i = 0; i < count; i++)
        {
            lerptimes[i] -= Irbis.Irbis.DeltaTime;
            positions[i] = Irbis.Irbis.LerpNoClamp(initialPositions[i], targets[i], 1-(lerptimes[i] / initialLerptimes[i]));
            tails[i].Update((position + positions[i]).ToPoint());
            if (lerptimes[i] <= 0)
            {
                initialPositions[i] = positions[i];
                targets[i] = Irbis.Irbis.RandomPoint(radius);
                initialLerptimes[i] = lerptimes[i] = Vector2.Distance(positions[i], targets[i]) / speed;
            }
        }

        center = position.ToPoint();
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
                for (int i = 0; i < count; i++)
                { sb.Draw(Irbis.Irbis.dottex, (targets[i] + position) * Irbis.Irbis.screenScale, new Rectangle(0, 0, 1, 1), Color.Magenta, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, 0.9f); }
                if (circle != null)
                { sb.Draw(circle, position * Irbis.Irbis.screenScale, null, Color.White, 0f, new Vector2(radius * Irbis.Irbis.screenScale), 1, SpriteEffects.None, 0.8001f); }
                else
                { circle = Irbis.Irbis.GenerateCircle((int)(radius * Irbis.Irbis.screenScale), Color.Magenta); }
                goto case 1;
            case 1:
                goto default;
            default:
                for (int i = 0; i < count; i++)
                { tails[i].Draw(sb); }
                break;
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        if (UseColor)
        {
            for (int i = 0; i < 5; i++)
            { tails[i].Light(sb); }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            { tails[i].Light(sb); }
        }
    }
}
