using Irbis;
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class ParticleSystem
{
    List<Particle> particleList = new List<Particle>();
    Vector2 initialVelocity;
    Vector2 force;
    Texture2D[] textures;
    int particles;
    public Rectangle spawnArea;
    float spawnDelay;
    float nextDelay;
    float depth;
    float timeSinceLastSpawn;

    float[] stateTimes = new float[4];
    float[] stateScales = new float[4];
    Color[] stateColors = new Color[4];
    int[] animationFrames = new int[4];
    float animationDelay;

    // random factors
    // 0 initialVelocityRandomness;
    // 1 forceRandomness;
    // 2 timesRandomness;
    // 3 scalesRandomness;
    // 4 delayRandomness;
    // 5 depthRandomness;
    float[] randomness = new float[6];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="InitialVelocity">initial velocity</param>
    /// <param name="Force">force</param>
    /// <param name="Times">total time each state will last: [0]=birthTime, [1]=lifeTime, [2]=deathTime</param>
    /// <param name="Scales">scale for each state: [0]=birthSize, [1]=lifeSize, [2]=deathSize</param>
    /// <param name="Delay">time between spawns. seconds.</param>
    /// <param name="Depth">depth</param>
    /// <param name="Randomness">[0]=InitialVelocity [1]=Force [2]=Times [3]=Scales [4]=Delay [5]=Depth (Times, Scales currently unused)</param>
    /// <param name="Spawn">area in which particles can spawn</param>
    /// <param name="Textures">animations. 0=birth, 1=life (loop), 2=death, 3=light (loop) (light resolution is double!) passed directly to particles.</param>
    /// <param name="Colors">[0]=birthColor, [1]=lifeColor, [2]=deathColor. passed directly to particle.</param>
    /// <param name="Frames">number of frames in each animation. passed directly to particles.</param>
    public ParticleSystem(Vector2 InitialVelocity, Vector2 Force, float[] Times, float[] Scales, float SpawnDelay, float Depth, float[] Randomness, 
        Rectangle Spawn, Texture2D[] Textures, Color[] Colors, int[] Frames, float AnimationDelay)
    {
        initialVelocity = InitialVelocity;
        force = Force;
        stateTimes = Times;
        stateScales = Scales;
        nextDelay = spawnDelay = SpawnDelay;
        depth = Depth;
        for (int i = 0; i < 6; i++)
        {
            if (Randomness.Length > i)
            { randomness[i] = Randomness[i]; }
        }
        spawnArea = Spawn;
        textures = Textures;
        stateColors = Colors;
        animationFrames = Frames;
        animationDelay = AnimationDelay;

    }

    public void Update()
    {
        timeSinceLastSpawn += Irbis.Irbis.DeltaTime;
        if (timeSinceLastSpawn >= nextDelay)
        {
            particleList.Add(
                new Particle(textures[Irbis.Irbis.RandomInt(textures.Length)], animationFrames, animationDelay,
                new Vector2(spawnArea.X + (Irbis.Irbis.RandomFloat * spawnArea.Width), spawnArea.Y + (Irbis.Irbis.RandomFloat * spawnArea.Height)),
                new Vector2(((Irbis.Irbis.RandomFloat - 0.5f) * randomness[0]) + initialVelocity.X, ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[0]) + initialVelocity.Y),
                new Vector2(((Irbis.Irbis.RandomFloat - 0.5f) * randomness[1]) + force.X, ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[1]) + force.Y),
                stateTimes, stateScales, stateColors, ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[5]) + depth)
            ); particles++;
            timeSinceLastSpawn = 0;
            nextDelay = ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[4]) + spawnDelay;
        }
        for (int i = 0; i < particles; i++)
        {
            particleList[i].Update();
            if (particleList[i].state == Particle.State.Dead)
            { particleList.RemoveAt(i); i--; particles--; }
        }
    }

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        { RectangleBorder.Draw(sb, spawnArea, Color.Magenta, true); }
        foreach (Particle p in particleList)
        {
            p.Draw(sb);
        }
    }

    public void Light(SpriteBatch sb)
    {
        foreach (Particle p in particleList)
        {
            p.Light(sb);
        }
    }
}
