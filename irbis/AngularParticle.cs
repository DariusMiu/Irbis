using Irbis;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class AngularParticle : Particle
{
    public AngularParticle(ParticleSystem ParentSystem, int Texture, Vector2 InitialPosition, Vector2 InitialVelocity, Vector2 Force, float Angle,
        float[] Times, float[] Scales, float[] LightScales, float[] Depths) :
        base(ParentSystem, Texture, InitialPosition, InitialVelocity, Force, Angle, Times, Scales, LightScales, Depths)
    { }

    public override void Draw(SpriteBatch sb)
    {
        sb.Draw(parentSystem.textures[tex], (parentSystem.Position + position) * Irbis.Irbis.screenScale, animationSourceRect, renderColor, angle, new Vector2(texSize / 2f), Irbis.Irbis.screenScale * renderScale, SpriteEffects.None, depth);
    }

    public override void Light(SpriteBatch sb)
    {

    }

    public override void ColoredLight(SpriteBatch sb)
    {

    }
}
