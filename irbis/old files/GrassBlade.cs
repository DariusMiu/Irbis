using Irbis;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class GrassBlade
{
    public float RotationTime
    {
        get
        { return rotationTime; }
        set
        { rotationTimeMax = rotationTime = value; }
    }
    public float TargetRotation
    {
        get
        { return targetRotation; }
        set
        { targetRotation = value; oldRotation = rotation; }
    }
    public float rotation;
    float rotationTime;
    float oldRotation;
    float rotationTimeMax;
    float targetRotation;
    Vector2 position;
    Rectangle bladeTextureArea;
    float depth;
    float brushRotation;
    Grass parentGrass;

    public GrassBlade (Grass ParentGrass, float InitialRotation, float InitialTargetRotation, float InitialRotationTime, Vector2 Position, Rectangle BladeTextureArea, float Depth)
    {
        parentGrass = ParentGrass;
        oldRotation = rotation = InitialRotation;
        targetRotation = InitialTargetRotation;
        rotationTimeMax = rotationTime = InitialRotationTime;
        position = Position;
        bladeTextureArea = BladeTextureArea;
        depth = Depth;
    }

    public void Update()
    {
        rotationTime -= Irbis.Irbis.DeltaTime;
        rotation = Irbis.Irbis.SmootherStep(targetRotation, oldRotation, (rotationTime / rotationTimeMax));
        float distanceSqr = Irbis.Irbis.DistanceSquared(Irbis.Irbis.jamie.BottomCenter, position);
        if (distanceSqr <= parentGrass.brushDistanceSqr)
        {
            if (Irbis.Irbis.Directions(Irbis.Irbis.jamie.BottomCenter, position) == Direction.Right)
            { brushRotation = Irbis.Irbis.LerpNoClamp(brushRotation, -((distanceSqr / parentGrass.brushDistanceSqr) - 1f), Irbis.Irbis.DeltaTime * 15); }
            else
            { brushRotation = Irbis.Irbis.LerpNoClamp(brushRotation, (distanceSqr / parentGrass.brushDistanceSqr) - 1f, Irbis.Irbis.DeltaTime * 15); }
        }
        else if (brushRotation != 0f)
        { brushRotation = Irbis.Irbis.LerpNoClamp(brushRotation, 0f, Irbis.Irbis.DeltaTime * 15); }
    }

    public void Draw(SpriteBatch sb)
    { sb.Draw(parentGrass.bladeTextures, position * Irbis.Irbis.screenScale, bladeTextureArea, Color.White, rotation + brushRotation, parentGrass.bladeOrigin, Irbis.Irbis.screenScale, SpriteEffects.None, depth); }

}
