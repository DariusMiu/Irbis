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
    public Rectangle Collider
    {
        get
        { return collider; }
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
    Rectangle collider;
    int radius;


    public ChargedBolt(Point Center, int Radius, int Count, float Speed, Color RenderColor, Color LightColor)
    {
        count = Count;
        speed = Speed;

        tails = new DotTail[count];
        positions = new Vector2[count];
        initialPositions = new Vector2[count];
        targets = new Vector2[count];
        lerptimes = new float[count];
        initialLerptimes = new float[count];

        position = Center.ToVector2();
        radius = Radius;
        collider = new Rectangle((int)(position.X - radius), (int)(position.Y - radius), 2 * radius, 2 * radius);
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

        collider.Location = new Point ((int)(position.X - radius), (int)(position.Y - radius));
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
                RectangleBorder.Draw(sb, collider, Color.Magenta, true);
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
