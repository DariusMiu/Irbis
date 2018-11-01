using Irbis;
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

class AngularParticleSystem : ParticleSystem
{
    float radius;
    float cosine;
    float sine;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InitialVelocity">initial velocity</param>
    /// <param name="Force">force</param>
    /// <param name="Times">total time each state will last: [0]=birthTime, [1]=lifeTime, [2]=dyingTime</param>
    /// <param name="Scales">scale for each state: [0]=birthSize, [1]=lifeSize, [2]=dyingSize, [3]=deathSize</param>
    /// <param name="Delay">time between spawns. seconds.</param>
    /// <param name="Depth">depth</param>
    /// <param name="Randomness">[0]=InitialVelocity [1]=Force [2]=Times [3]=Scales [4]=LightScales [5]=Delay(Times, Scales currently unused)</param>
    /// <param name="Spawn">area in which particles can spawn</param>
    /// <param name="Textures">animations. 0=birth, 1=life (loop), 2=dying, 3=light (loop) (light resolution is double!) passed directly to particles.</param>
    /// <param name="Colors">[0]=birthColor, [1]=lifeColor, [2]=dyingColor, [3]=deathColor. passed directly to particle.</param>
    /// <param name="Frames">number of frames in each animation. passed directly to particles.</param>
    /// <param name="TimeToLive">particle system's time to live, in seconds. pass zero for forever.</param>
    public AngularParticleSystem(float InitialVelocity, float Force, float[] Times, float[] Scales, float[] LightScales, float SpawnDelay, float[] Depths, float[] Randomness,
    Rectangle Spawn, Texture2D[] Textures, Color[] Colors, Color[] LightColors, int[] Frames, float AnimationDelay, float TimeToLive, int Efficiency) :
        base(new Vector2(InitialVelocity), new Vector2(Force), Times, Scales, LightScales, SpawnDelay, Depths, Randomness, Rectangle.Empty, Textures, Colors, LightColors, Frames, AnimationDelay, TimeToLive, Efficiency)
    {
        position = Spawn.Center.ToVector2();
        radius = (Spawn.Width + Spawn.Height) / 2f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if particle system should be terminated</returns>
    public override bool Update()
    {
        timeSinceLastSpawn += Irbis.Irbis.DeltaTime;
        while (timeSinceLastSpawn >= nextDelay && timeToLive >= 0)
        {
            float angle = Irbis.Irbis.RandomFloat * MathHelper.TwoPi; // in radians
            cosine = (float)Math.Cos(angle);
            sine = (float)Math.Sin(angle);
            particleList.Add(
                new AngularParticle(this, Irbis.Irbis.RandomInt(textures.Length),
                new Vector2(cosine * radius, sine * radius),
                new Vector2((((Irbis.Irbis.RandomFloat * 2f) - 1f) * randomness[0]) + (cosine * initialVelocity.X), (((Irbis.Irbis.RandomFloat * 2f) - 1f) * randomness[0]) + (sine * initialVelocity.Y)),
                new Vector2((((Irbis.Irbis.RandomFloat * 2f) - 1f) * randomness[1]) + (cosine * force.X), (((Irbis.Irbis.RandomFloat * 2f) - 1f) * randomness[1]) + (sine * force.Y)), angle,
                stateTimes, stateScales, stateLightScales, stateDepths)
            );
            timeSinceLastSpawn -= nextDelay;
            nextDelay = ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[5]) + spawnDelay;
        }

        //Func<Action<Particle>, Action> kernelBuilder = ParticleUpdate => () =>
        //{
        //    var start = blockIdx.x * blockDim.x + threadIdx.x;
        //    var stride = gridDim.x * blockDim.x;
        //    for (var i = start; i < result.Length; i += stride)
        //    {
        //        ParticleUpdate(i);
        //    }
        //};
        //Gpu.Default.Launch(kernelBuilder(i => result[i] = arg1[i] + arg2[i]), lp);

        for (int i = particleList.Count - 1; i >= 0; i--)
        {
            particleList[i].Update(efficiency);
            if (particleList[i].state == Particle.State.Dead)
            { particleList.RemoveAt(i); }
        }
        if (timeToLive > 0)
        {
            timeToLive -= Irbis.Irbis.DeltaTime;
            if (timeToLive <= 0)
            { timeToLive = -1; }
        }
        else if (timeToLive < 0 && particleList.Count <= 0)
        { return true; }
        return false;
    }

    private static void ParticleUpdate(Particle p)
    {
        p.velocity -= p.force * Irbis.Irbis.DeltaTime;
        p.position += p.velocity * Irbis.Irbis.DeltaTime;
        p.prevState = p.state;
        if (p.currentStateTime <= p.parentSystem.stateTimes[(int)p.state])
        {
            p.currentStateTime += Irbis.Irbis.DeltaTime;
            if (p.currentStateTime >= p.parentSystem.stateTimes[(int)p.state])
            {
                p.currentStateTime -= p.parentSystem.stateTimes[(int)p.state];
                p.state++;
                p.currentFrame = 0;
            }
        }
        p.timeSinceLastFrame += Irbis.Irbis.DeltaTime;
        if (p.prevState != p.state)
        {
            p.timeSinceLastFrame -= p.animationSpeed[(int)p.state];
            //p.animationSourceRect.X = 0;
            p.animationSourceRect.Y += p.texSize;
        }
        else
        {
            if (p.timeSinceLastFrame >= p.animationSpeed[(int)p.state])
            {
                p.currentFrame++;
                p.timeSinceLastFrame -= p.animationSpeed[(int)p.state];
                p.animationSourceRect.X = p.currentFrame * p.texSize;
            }
            if (p.currentFrame > p.parentSystem.animationFrames[(int)p.state])
            {
                p.currentFrame = 0;
                p.animationSourceRect.X = 0;
            }
        }

        //lightSourceRect.X = currentFrame * texSize * 2;
        if (p.state != Particle.State.Dead)
        {
            float lerppercent = p.currentStateTime / p.parentSystem.stateTimes[(int)p.state];
            p.renderColor = Color.Lerp(p.parentSystem.stateColors[(int)p.state], p.parentSystem.stateColors[(int)p.state + 1], lerppercent);
            p.lightColor = Color.Lerp(p.parentSystem.stateLightColors[(int)p.state], p.parentSystem.stateLightColors[(int)p.state + 1], lerppercent);
            p.renderScale = Irbis.Irbis.Lerp(p.stateScales[(int)p.state], p.stateScales[(int)p.state + 1], lerppercent);
            p.lightScale = Irbis.Irbis.Lerp(p.stateLightScales[(int)p.state], p.stateLightScales[(int)p.state + 1], lerppercent);
            p.depth = Irbis.Irbis.Lerp(p.parentSystem.stateDepths[(int)p.state], p.parentSystem.stateDepths[(int)p.state + 1], lerppercent);
        }

    }

}
