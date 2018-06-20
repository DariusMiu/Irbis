using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class Grass
{
    public float rotationTime;
    float rotationRandomness;
    public float rotationMin;
    public float rotationMax;
    float rotationRange;
    public Vector2 bladeOrigin;
    public Texture2D bladeTextures;
    GrassBlade[] bladeList;
    public Rectangle area;
    int efficiency;
    int bladeCountOver2;
    public float initialRotation;
    public float density;
    public float depth1;
    public float depth2;
    public float[] randomness;
    public Point textureDimentions;
    public float brushDistanceSqr = 900;
    public static SpriteBatch spriteBatch;
    RenderTarget2D renderTarget1;
    RenderTarget2D renderTarget2;
    public Vector2 position;

    /// <summary>
    /// plant some grass in an area
    /// </summary>
    /// <param name="InitialRotation">initial position</param>
    /// <param name="RotationTime">how long it takes to move from one rotational position to another</param>
    /// <param name="Density">number of blades that should be fit into 100 px</param>
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
        Vector2 OriginOffset, Rectangle Area, Texture2D BladeTextures, Point TextureDimentions, float BrushDistanceSqr, int Efficiency)
    {
        brushDistanceSqr = BrushDistanceSqr;
        rotationMax = RotationMax;
        textureDimentions = TextureDimentions;
        randomness = Randomness;
        depth1 = Depth - Randomness[3] / 2;
        depth2 = Depth + Randomness[3] / 2;
        density = Density;
        initialRotation = InitialRotation;
        area = Area;
        rotationTime = RotationTime;
        rotationRandomness = Randomness[1];
        bladeOrigin = OriginOffset;
        bladeTextures = BladeTextures;

        renderTarget1 = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, area.Width + textureDimentions.Y * 2, area.Height * 2 + textureDimentions.Y);
        renderTarget2 = new RenderTarget2D(Irbis.Irbis.game.GraphicsDevice, area.Width + textureDimentions.Y * 2, area.Height * 2 + textureDimentions.Y);
        position = new Vector2(area.X - textureDimentions.Y, area.Y - textureDimentions.Y);

        if (Efficiency > 0)
        { efficiency = Efficiency; }
        else
        { efficiency = 1; }
        rotationMin = RotationMin;
        rotationRange = RotationMax - rotationMin;

        List<float> posList = new List<float>();
        float currentXpos = 0;

        while (currentXpos < (Area.Width) - (150f / Density))
        {
            posList.Add(currentXpos);
            currentXpos += (150f / Density);
        }

        bladeList = new GrassBlade[posList.Count];

        while (posList.Count > 0)
        {
            int next = Irbis.Irbis.RandomInt(posList.Count);
            bladeList[posList.Count - 1] = (new GrassBlade(this, InitialRotation + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[0]),
                rotationMin + (Irbis.Irbis.RandomFloat * rotationRange),
                rotationTime + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * rotationRandomness),
                new Vector2(posList[next] + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[2]) + textureDimentions.Y, textureDimentions.Y + (Irbis.Irbis.RandomFloat * Area.Height)),
                new Rectangle(new Point(Irbis.Irbis.RandomInt(BladeTextures.Width / TextureDimentions.X) * TextureDimentions.X, 0), TextureDimentions),
                Depth + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * Randomness[3])));
            posList.RemoveAt(next);
        }
        bladeCountOver2 = bladeList.Length / 2;
    }

    public void Update ()
    {
        for (int i = 0; i < bladeList.Length; i += efficiency)
        {
            bladeList[i].Update();
            if (bladeList[i].RotationTime <= 0)
            {
                bladeList[i].RotationTime = rotationTime + (((Irbis.Irbis.RandomFloat * 2f) - 1f) * rotationRandomness);
                bladeList[i].TargetRotation = rotationMin + (Irbis.Irbis.RandomFloat * rotationRange);
            }

            for (int j = 1; j < efficiency; j++)
            { if (i+j < bladeList.Length) { bladeList[i+j].rotation = bladeList[i].rotation; } }
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
    {
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget1);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
        for (int i = bladeList.Length - 1; i >= bladeCountOver2; i--)
        { bladeList[i].Draw(spriteBatch); }
        spriteBatch.End();
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(renderTarget2);
        Irbis.Irbis.game.GraphicsDevice.Clear(Color.Transparent);
        spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
        for (int i = bladeCountOver2; i >= 0; i--)
        { bladeList[i].Draw(spriteBatch); }
        spriteBatch.End();
        Irbis.Irbis.game.GraphicsDevice.SetRenderTarget(null);
    }

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        {
            RectangleBorder.Draw(sb, area, Color.Green, true);
            RectangleBorder.Draw(sb, new Rectangle(position.ToPoint(), renderTarget1.Bounds.Size), Color.Green, true);
        }
        sb.Draw(renderTarget1, position * Irbis.Irbis.screenScale, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth1);
        sb.Draw(renderTarget2, position * Irbis.Irbis.screenScale, null, Color.White, 0f, Vector2.Zero, Irbis.Irbis.screenScale, SpriteEffects.None, depth2);
    }
}