using Irbis;
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

public class ParticleSystem
{
    List<Particle> particleList = new List<Particle>();
    public Vector2 initialVelocity;
    public Vector2 force;
    public Texture2D[] textures;
    public Rectangle spawnArea;
    public float spawnDelay;
    public float depth;
    public float timeSinceLastSpawn;
    public float timeToLive;

    float nextDelay;

    public float[] stateDepths = new float[4];
    public float[] stateTimes = new float[4];
    public float[] stateScales = new float[4];
    public float[] stateLightScales = new float[4];
    public Color[] stateColors = new Color[4];
    public Color[] stateLightColors = new Color[4];
    public int[] animationFrames = new int[4];
    public float animationDelay;
    int efficiency = 1;
    int updateIndex;

    // random factors
    // 0 initialVelocityRandomness;
    // 1 forceRandomness;
    // 2 timesRandomness;
    // 3 scalesRandomness;
    // 4 lightRandomness;
    // 5 delayRandomness;
    // 6 depthRandomness;
    public float[] randomness = new float[7];

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
    public ParticleSystem(Vector2 InitialVelocity, Vector2 Force, float[] Times, float[] Scales, float[] LightScales, float SpawnDelay, float[] Depths, float[] Randomness, 
        Rectangle Spawn, Texture2D[] Textures, Color[] Colors, Color[] LightColors, int[] Frames, float AnimationDelay, float TimeToLive, int Efficiency)
    {
        initialVelocity = InitialVelocity;
        force = Force;
        stateTimes = Times;
        stateScales = Scales;
        nextDelay = spawnDelay = SpawnDelay;
        for (int i = 0; i < 4; i++)
        {
            if (Times.Length > i)
            { stateTimes[i] = Times[i]; }
            if (Scales.Length > i)
            { stateScales[i] = Scales[i]; }
            if (LightScales.Length > i)
            { stateLightScales[i] = LightScales[i]; }
            if (Colors.Length > i)
            { stateColors[i] = Colors[i]; }
            else
            { stateColors[i] = Color.Transparent; }
            if (LightColors.Length > i)
            { stateLightColors[i] = LightColors[i]; }
            else
            { stateLightColors[i] = Color.Transparent; }
            if (Depths.Length > i)
            { stateDepths[i] = Depths[i]; }
            else if (i > 0)
            { stateDepths[i] = stateDepths[i-1]; }
        }
        for (int i = 0; i < 7; i++)
        {
            if (Randomness.Length > i)
            { randomness[i] = Randomness[i]; }
        }
        spawnArea = Spawn;
        textures = Textures;
        animationFrames = Frames;
        animationDelay = AnimationDelay;
        timeToLive = TimeToLive;

        if (Efficiency > 0)
        { efficiency = Efficiency; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>true if particle system should be terminated</returns>
    public bool Update()
    {
        timeSinceLastSpawn += Irbis.Irbis.DeltaTime;
        if (timeSinceLastSpawn >= nextDelay && timeToLive >= 0)
        {
            particleList.Add(
                new Particle(this, Irbis.Irbis.RandomInt(textures.Length),
                new Vector2(spawnArea.X + (Irbis.Irbis.RandomFloat * spawnArea.Width), spawnArea.Y + (Irbis.Irbis.RandomFloat * spawnArea.Height)),
                new Vector2((((Irbis.Irbis.RandomFloat*2f)-1f) * randomness[0]) + initialVelocity.X, (((Irbis.Irbis.RandomFloat*2f)-1f) * randomness[0]) + initialVelocity.Y),
                new Vector2((((Irbis.Irbis.RandomFloat*2f)-1f) * randomness[1]) + force.X, (((Irbis.Irbis.RandomFloat*2f)-1f) * randomness[1]) + force.Y),
                stateTimes, stateScales, stateLightScales,stateDepths)
            );
            timeSinceLastSpawn = 0;
            nextDelay = ((Irbis.Irbis.RandomFloat - 0.5f) * randomness[5]) + spawnDelay;
        }
        for (int i = particleList.Count - 1 - updateIndex; i >= 0; i-=efficiency)
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
        updateIndex++;
        if (updateIndex >= efficiency)
        { updateIndex = 0; }
        return false;
    }

    public void ThreadPoolCallback(Object threadContext)
    {
        try
        {
            if (Update())
            { Irbis.Irbis.particleSystems.Remove(this); }
        }
        finally
        {
            if (Interlocked.Decrement(ref Irbis.Irbis.pendingThreads) <= 0)
            { Irbis.Irbis.doneEvent.Set(); }
        }
    }

    public void Draw(SpriteBatch sb)
    {
        if (Irbis.Irbis.debug > 1)
        { RectangleBorder.Draw(sb, spawnArea, Color.Magenta, true); }
        for (int i = particleList.Count - 1; i >= 0; i--)
        { particleList[i].Draw(sb); }
    }

    public void Light(SpriteBatch sb, bool UseColor)
    {
        if (UseColor)
        {
            for (int i = particleList.Count - 1; i >= 0; i--)
            { particleList[i].ColoredLight(sb); }
        }
        else
        {
            for (int i = particleList.Count - 1; i >= 0; i--)
            { particleList[i].Light(sb); }
        }
    }
}
