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
    Texture2D tex;
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

    // animation system
    float timeSinceLastFrame;
    int currentFrame;
    int[] animationFrames = new int[4];
    float[] animationSpeed = new float[4];
    Rectangle animationSourceRect;
    Rectangle lightSourceRect;
    Color renderColor;
    float renderScale;
    float depth;
    float currentStateTime;

    // colors
    Color[] stateColors = new Color[4];

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
    public Particle(Texture2D Texture, int[] Frames, float AnimationDelay, Vector2 InitialPosition, Vector2 InitialVelocity, Vector2 Force, float[] Times, float[] Scales, Color[] Colors, float Depth)
    {
        tex = Texture;
        texSize = Texture.Height / 5;
        velocity = InitialVelocity;
        position = InitialPosition;
        force = Force;
        depth = Depth;

        animationSourceRect = new Rectangle(0, 0, texSize, texSize);
        lightSourceRect = new Rectangle(0, texSize*3, texSize*2, texSize*2);

        animationSpeed[0] = Times[0] / (Frames[0] + 1);
        animationSpeed[1] = AnimationDelay;
        animationSpeed[2] = Times[2] / (Frames[2] + 1);
        animationSpeed[3] = AnimationDelay;
        animationFrames = Frames;

        for (int i = 0; i < 4; i++)
        {
            if (Times.Length > i)
            { stateTimes[i] = Times[i]; }
            if (Scales.Length > i)
            { stateScales[i] = Scales[i]; }
            if (Colors.Length > i)
            { stateColors[i] = Colors[i]; }
            else
            { stateColors[i] = Color.Transparent; }
        }

        renderColor = Colors[0];
    }

    public void Update()
    {
        velocity -= force * Irbis.Irbis.DeltaTime;
        position += velocity * Irbis.Irbis.DeltaTime;
        prevState = state;
        if (currentStateTime < stateTimes[(int)state])
        {
            currentStateTime += Irbis.Irbis.DeltaTime;
            if (currentStateTime >= stateTimes[(int)state])
            { state++; currentStateTime = 0; currentFrame = 0; }
        }

        if (state != State.Dead)
        { Animate(); }
    }

    private void Animate()
    {
        timeSinceLastFrame += Irbis.Irbis.DeltaTime;
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
            if (currentFrame > animationFrames[(int)state])
            {
                currentFrame = 0;
                animationSourceRect.X = 0;
            }
        }

        //lightSourceRect.X = currentFrame * texSize * 2;
        renderColor = Color.Lerp(stateColors[(int)state], stateColors[(int)state + 1], currentStateTime / stateTimes[(int)state]);
        renderScale = Irbis.Irbis.Lerp(stateScales[(int)state], stateScales[(int)state + 1], currentStateTime / stateTimes[(int)state]);
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(tex, position * Irbis.Irbis.screenScale, animationSourceRect, renderColor, 0f, new Vector2(texSize / 2f), Irbis.Irbis.screenScale * renderScale, SpriteEffects.None, depth);
    }

    public void Light(SpriteBatch sb)
    {
        sb.Draw(tex, position * Irbis.Irbis.screenScale, lightSourceRect,  Color.Black, 0f, new Vector2(texSize), Irbis.Irbis.screenScale * renderScale, SpriteEffects.None, depth);
    }
}
