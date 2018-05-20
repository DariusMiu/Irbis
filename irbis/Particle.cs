using Irbis;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class Particle
{
    public enum State
    {
        Birth = 0,
        Alive = 1,
        Dying = 2,
        Dead = 3,
    }

    // texture
    // animations
    // 0 = birth
    // 1 = life (loop)
    // 2 = death
    // 3 = light (loop?)
    ParticleSystem parentSystem;
    int tex;
    int texSize;

    // vectors
    Vector2 velocity;
    Vector2 position;
    Vector2 force;

    // floats
    // seconds in each state
    float[] stateTimes = new float[4];
    // size in each state (beginning of birth is birthsize, end of death is death size)
    float[] stateScales = new float[4];
    float[] stateLightScales = new float[4];
    float[] stateDepths = new float[4];

    // animation system
    float timeSinceLastFrame;
    int currentFrame;
    //int[] animationFrames = new int[4];
    float[] animationSpeed = new float[4];
    Rectangle animationSourceRect;
    Rectangle lightSourceRect;
    Color renderColor;
    Color lightColor;
    float renderScale;
    float lightScale;
    float depth;
    float currentStateTime;

    // colors

    // current state. controls Draw()
    public State state = State.Birth;
    State prevState = State.Birth;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Texture">animations. 0=birth, 1=life (loop), 2=death, 3=light (loop) (light resolution is double!)</param>
    /// <param name="Frames">number of frames in each animation</param>
    /// <param name="AnimationDelay">how long should each frame last? used for light and life loops.</param>
    /// <param name="InitialPosition">initial position</param>
    /// <param name="InitialVelocity">initial velocity</param>
    /// <param name="Force">force</param>
    /// <param name="Times">total time each state will last: [0]=birthTime, [1]=lifeTime, [2]=deathTime</param>
    /// <param name="Scales">[0]=birthSize, [1]=lifeSize, [2]=deathSize</param>
    /// <param name="Colors">[0]=birthColor, [1]=lifeColor, [2]=deathColor</param>
    public Particle(ParticleSystem ParentSystem, int Texture, Vector2 InitialPosition, Vector2 InitialVelocity, Vector2 Force,
        float[] Times, float[] Scales, float[] LightScales, float[] Depths)
    {
        parentSystem = ParentSystem;
        tex = Texture;
        texSize = ParentSystem.textures[tex].Height / 5;
        velocity = InitialVelocity;
        position = InitialPosition;
        force = Force;
        stateDepths = Depths;
        depth = Depths[0];

        animationSourceRect = new Rectangle(0, 0, texSize, texSize);
        lightSourceRect = new Rectangle(0, texSize*3, texSize*2, texSize*2);

        animationSpeed[0] = Times[0] / (ParentSystem.animationFrames[0] + 1);
        animationSpeed[1] = ParentSystem.animationDelay;
        animationSpeed[2] = Times[2] / (ParentSystem.animationFrames[2] + 1);
        animationSpeed[3] = ParentSystem.animationDelay;
        //animationFrames = ParentSystem.animationFrames;

        for (int i = 0; i < 4; i++)
        {
            if (Times.Length > i)
            { stateTimes[i] = Times[i]; }
            if (Scales.Length > i)
            { stateLightScales[i] = stateScales[i] = Scales[i]; }
            if (LightScales.Length > i)
            { stateLightScales[i] = LightScales[i]; }
        }

        renderColor = parentSystem.stateColors[0];
        lightColor = parentSystem.stateLightColors[0];
    }/**/

    public void Update(int factor)
    {
        velocity -= force * Irbis.Irbis.DeltaTime * factor;
        position += velocity * Irbis.Irbis.DeltaTime * factor;
        prevState = state;
        if (currentStateTime <= stateTimes[(int)state])
        {
            currentStateTime += Irbis.Irbis.DeltaTime * factor;
            if (currentStateTime >= stateTimes[(int)state])
            {
                currentStateTime -= stateTimes[(int)state];
                state++;
                currentFrame = 0;
            }
        }

        Animate(factor);
    }

    private void Animate(int factor)
    {
        timeSinceLastFrame += Irbis.Irbis.DeltaTime * factor;
        if (prevState != state)
        {
            timeSinceLastFrame -= animationSpeed[(int)state];
            animationSourceRect.X = 0;
            animationSourceRect.Y += texSize;
        }
        else
        {
            if (timeSinceLastFrame >= animationSpeed[(int)state])
            {
                currentFrame++;
                timeSinceLastFrame -= animationSpeed[(int)state];
                animationSourceRect.X = currentFrame * texSize;
            }
            if (currentFrame > parentSystem.animationFrames[(int)state])
            {
                currentFrame = 0;
                animationSourceRect.X = 0;
            }
        }

        //lightSourceRect.X = currentFrame * texSize * 2;
        if (state != State.Dead)
        {
            float lerppercent = currentStateTime / stateTimes[(int)state];
            renderColor = Color.Lerp(parentSystem.stateColors[(int)state], parentSystem.stateColors[(int)state + 1], lerppercent);
            lightColor = Color.Lerp(parentSystem.stateLightColors[(int)state], parentSystem.stateLightColors[(int)state + 1], lerppercent);
            renderScale = Irbis.Irbis.Lerp(stateScales[(int)state], stateScales[(int)state + 1], lerppercent);
            lightScale = Irbis.Irbis.Lerp(stateLightScales[(int)state], stateLightScales[(int)state + 1], lerppercent);
            depth = Irbis.Irbis.Lerp(stateDepths[(int)state], stateDepths[(int)state + 1], lerppercent);
        }
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(parentSystem.textures[tex], position * Irbis.Irbis.screenScale, animationSourceRect, renderColor, 0f, new Vector2(texSize / 2f), Irbis.Irbis.screenScale * renderScale, SpriteEffects.None, depth);
    }

    public void Light(SpriteBatch sb)
    {
        sb.Draw(parentSystem.textures[tex], (position) * Irbis.Irbis.screenScale, lightSourceRect, new Color(Color.Black, renderColor.A), 0f, new Vector2(texSize), Irbis.Irbis.screenScale * lightScale, SpriteEffects.None, depth);
    }

    public void ColoredLight(SpriteBatch sb)
    {
        sb.Draw(parentSystem.textures[tex], (position) * Irbis.Irbis.screenScale, lightSourceRect, lightColor, 0f, new Vector2(texSize), Irbis.Irbis.screenScale * lightScale, SpriteEffects.None, depth);
    }
}
