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

public class ChargedBolts
{
    ChargedBolt[] bolts;
    float[] damage;
    Vector2[] initialPositions;
    Vector2[] targets;
    Vector2[] directions;
    float[] lerptimes;
    float[] initialLerptimes;
    int[] lerpCounts;
    int count;
    float randomness;
    float velocity;
    float stunTime;
    Vector2 center;
    int boltRadius;
    int boltRadiusSquared;


    public ChargedBolts(Vector2 Center, int Count, float AngleOffset, float Randomness, float Velocity, float StunTime, float Damage, int BoltRadius, int Speed)
    {
        count = Count;
        randomness = Randomness;
        velocity = Velocity;
        center = Center;
        stunTime = StunTime;
        boltRadius = BoltRadius;
        boltRadiusSquared = boltRadius * boltRadius;

        bolts = new ChargedBolt[Count];
        damage = new float[Count];
        initialPositions = new Vector2[count];
        targets = new Vector2[count];
        directions = new Vector2[Count];
        lerptimes = new float[count];
        initialLerptimes = new float[count];
        lerpCounts = new int[count];

        for (int i = 0; i < count; i++)
        {
            lerpCounts[i] = 1;
            damage[i] = Damage;
            bolts[i] = new ChargedBolt(Center.ToPoint(), boltRadius, 5, Speed, Color.LightCyan, Color.LightCyan);
            initialPositions[i] = bolts[i].position;
            float angle = (i / (float)count) * MathHelper.Pi - MathHelper.PiOver2 + AngleOffset;
            directions[i] = new Vector2((float)Math.Sin(angle), (float)Math.Cos(angle)) * 10;
            targets[i] = new Vector2(directions[i].X + randomness * (Irbis.Irbis.RandomFloat - 1), directions[i].Y + randomness * (Irbis.Irbis.RandomFloat - 1)) * lerpCounts[i] + center;
            //targets[i] = directions[i] + center;
            initialLerptimes[i] = lerptimes[i] = Vector2.Distance(bolts[i].position, targets[i]) / velocity;
        }
    }

    public void Update()
    {
        for (int i = 0; i < count; i++)
        {
            if (damage[i] >= 0)
            {
                lerptimes[i] -= Irbis.Irbis.DeltaTime;
                bolts[i].position = Irbis.Irbis.LerpNoClamp(initialPositions[i], targets[i], 1 - (lerptimes[i] / initialLerptimes[i]));
                bolts[i].Update();

                if (lerptimes[i] <= 0)
                {
                    lerpCounts[i]++;
                    initialPositions[i] = bolts[i].position;
                    targets[i] = new Vector2(directions[i].X + randomness * (Irbis.Irbis.RandomFloat - 1), directions[i].Y + randomness * (Irbis.Irbis.RandomFloat - 1)) * lerpCounts[i] + center;
                    initialLerptimes[i] = lerptimes[i] = Vector2.Distance(bolts[i].position, targets[i]) / velocity;
                }
                if (Collision(bolts[i].Center, i))
                { damage[i] = -1; }
            }
        }
    }

    public bool Collision(Point Center, int index)
    {
        foreach (ICollisionObject s in Irbis.Irbis.collisionObjects)
        {
            if (Irbis.Irbis.DistanceSquared(s.Collider, Center) <= boltRadiusSquared)
            {
                if (s.GetType() == typeof(Player))
                { ((Player)s).Zap(damage[index], stunTime); }
                return true;
            }
        }
        return false;
    }

    public void Draw(SpriteBatch sb)
    {
        for (int i = 0; i < count; i++)
        {
            if (damage[i] >= 0)
            { bolts[i].Draw(sb); }
        }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {

    }
}
