using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Graphics;

[DataContract]
public class Grass
{
    public Texture2D bladeTextures;
    [DataMember]
    private string texname;

    [DataMember]
    public float rotationTime;
    [DataMember]
    public float rotationMin;
    [DataMember]
    public float rotationMax;
    [DataMember]
    public Vector2 bladeOrigin;
    [DataMember]
    public float initialRotation;
    [DataMember]
    public float density;
    [DataMember]
    public float depth;
    [DataMember]
    public float[] randomness;
    [DataMember]
    public Point textureDimentions;
    [DataMember]
    public float brushDistanceSqr = 900;
    [DataMember]
    int efficiency;
    [DataMember]
    private Rectangle initialArea;



    float rotationRange;
    float rotationRandomness;
    List<GrassBlade> bladeList;
    public Rectangle area;
    /// <summary>
    /// returns the correct area, with negative density accounted for
    /// </summary>
    public Rectangle Area
    {
        get
        { return initialArea; }
    }
    public int Efficiency
    {
        get
        { return efficiency; }
    }
    int bladeCount;

    public float rotation;

    public static SpriteBatch spriteBatch;

    /// <summary>
    /// plant some grass in an area
    /// </summary>
    /// <param name="InitialRotation">initial position</param>
    /// <param name="RotationTime">how long it takes to move from one rotational position to another</param>
    /// <param name="Density">number of blades that should be fit into 100 px (negative will plant the grass from right to left)</param>
    /// <param name="Depth">draw depth</param>
    /// <param name="Randomness">[0]=InitialRotation, [1]=RotationTime, [2]=Density, [3]=Depth</param>
    /// <param name="RotationMin">minimum rotation</param>
    /// <param name="RotationMax">maximum rotation</param>
    /// <param name="OriginOffeset">where will the texture rotate from</param>
    /// <param name="Area">area to be filled with grass (randomly by origin)</param>
    /// <param name="BladeTextures">textures (every blade should be arranged horizontally in a row)</param>
    /// <param name="TextureDimentions">size of each individual blade in texture</param>
    /// <param name="Efficiency">how many blades can use the same rotational information? (1 = every blade is unique, 2 = every other blade uses the same rotation)</param>
    public Grass(float InitialRotation, float RotationTime, float Density, float Depth, float[] Randomness, float RotationMin, float RotationMax,
        Vector2 OriginOffset, Rectangle Area, Texture2D BladeTextures, Point TextureDimentions, float? BrushDistanceSqr, int Efficiency)
    {
        if (BrushDistanceSqr != null)
        { brushDistanceSqr = (float)BrushDistanceSqr; }
        rotationMax = RotationMax;
        textureDimentions = TextureDimentions;
        randomness = Randomness;
        depth = Depth;
        density = Density;
        initialRotation = InitialRotation;
        initialArea = area = Area;
        rotationTime = RotationTime;
        rotationRandomness = Randomness[1];
        bladeOrigin = OriginOffset;
        bladeTextures = BladeTextures;
        if (Efficiency > 0)
        { efficiency = Efficiency; }
        else
        { efficiency = 1; }
        rotationMin = RotationMin;
        rotationRange = RotationMax - rotationMin;

        List<float> posList = new List<float>();

        if (density > 0)
        {
            float currentXpos = 0;
            while (currentXpos < area.Width - (100f / density))
            {
                posList.Add(currentXpos);
                currentXpos += (100f / density);
            }
        }
        else
        {
            area.X += area.Width;
            float currentXpos = -area.Width;
            while (currentXpos < 0)
            {
                posList.Add(currentXpos);
                currentXpos -= (100f / density);
            }
        }

        bladeList = new List<GrassBlade>();

        while (posList.Count > 0)
        {
            int next = Irbis.Irbis.RandomInt(posList.Count);
            bladeList.Add(new GrassBlade(this, InitialRotation + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[0]),
                rotationMin + (Irbis.Irbis.RandomFloat * rotationRange),
                rotationTime + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * rotationRandomness),
                new Vector2(posList[next] + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[2]), Irbis.Irbis.RandomFloat * area.Height),
                new Rectangle(new Point(Irbis.Irbis.RandomInt(BladeTextures.Width / TextureDimentions.X) * TextureDimentions.X, 0), TextureDimentions),
                Depth + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[3])));
            posList.RemoveAt(next);
        }
        bladeCount = bladeList.Count;
    }

    [OnSerializing]
    void OnSerializing(StreamingContext c)
    { texname = bladeTextures.Name; }

    [OnSerialized]
    void OnSerialized(StreamingContext c)
    { texname = null; }

    [OnDeserializing]
    void OnDeserializing(StreamingContext c)
    { }

    [OnDeserialized]
    void OnDeserialized(StreamingContext c)
    {
        bladeTextures = Irbis.Irbis.LoadTexture(texname);
        texname = null;
    }

    public void Update()
    {
        for (int i = 0; i < bladeCount; i += efficiency)
        {
            bladeList[i].Update();
            if (bladeList[i].RotationTime <= 0)
            {
                bladeList[i].RotationTime = rotationTime + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * rotationRandomness);
                bladeList[i].TargetRotation = rotationMin + (Irbis.Irbis.RandomFloat * rotationRange);
            }

            for (int j = 1; j < efficiency; j++)
            { if (i+j < bladeCount) { bladeList[i+j].rotation = bladeList[i].rotation; } }
        }
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        { Update(); }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            { Irbis.Irbis.doneEvent.Set(); }
        }
    }

    public void PreDraw()
    { }

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        // notice that this is drawing Area and not area
        { RectangleBorder.Draw(sb, Area, Color.Green, true); }
        foreach (GrassBlade g in bladeList)
        { g.Draw(sb); }
    }
}