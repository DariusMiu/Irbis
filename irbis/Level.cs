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
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public struct Level
{
    //public List<Square> squareList;
    private List<int> squareSpawnPointsX;
    private List<int> squareSpawnPointsY;
    public List<string> squareTextures;
    public List<float> squareDepths;
    private List<int> backgroundSquaresX;
    private List<int> backgroundSquaresY;
    public List<string> backgroundTextures;
    public List<float> backgroundSquareDepths;
    public string levelName;
    private List<float> enemySpawnPointsX;
    private List<float> enemySpawnPointsY;
    public bool isOnslaught;
    private float playerSpawnX;
    private float playerSpawnY;
    private float bossSpawnX;
    private float bossSpawnY;

    private string[] vendingMachineTextures;
    private VendingType[] vendingMachineTypes;
    private int[] vendingMachineLocationsX;
    private int[] vendingMachineLocationsY;

    public string bossName;

    // Particle Systems
    float[] PSInitialVelocityX;
    float[] PSInitialVelocityY;
    float[] PSForceX;
    float[] PSForceY;
    float[][] PSTimes;
    float[][] PSScales;
    float[] PSSpawnDelay;
    float[][] PSDepths;
    float[][] PSRandomness;
    int[] PSSpawnX;
    int[] PSSpawnY;
    int[] PSSpawnW;
    int[] PSSpawnH;
    string[][] PSTextures;
    uint[][] PSColors;
    int[][] PSFrames;
    float[] PSAnimationDelay;
    float[][] PSLightScales;
    uint[][] PSLightColors;
    float[] PSTimeToLive;
    // lighting
    public float darkness;
    // Grass Systems
    float[] GinitialRotation;
    float[] GrotationTime;
    float[] Gdensity;
    float[] Gdepth;
    float[][] Grandomness;
    float[] GrotationMin;
    float[] GrotationMax;
    float[] GoriginOffsetX;
    float[] GoriginOffsetY;
    int[] GareaX;
    int[] GareaY;
    int[] GareaW;
    int[] GareaH;
    string[] GbladeTextures;
    int[] GtextureDimentionsX;
    int[] GtextureDimentionsY;


    public Point[] VendingMachineLocations
    {
        get
        {
            Point[] vendingMachines = new Point[vendingMachineLocationsX.Length];
            for (int i = 0; i < vendingMachineLocationsX.Length; i ++)
            {
                vendingMachines[i] = new Point(vendingMachineLocationsX[i], vendingMachineLocationsY[i]);
            }
            return vendingMachines;
        }
        set
        {
            vendingMachineLocationsX = new int[value.Length];
            vendingMachineLocationsY = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                vendingMachineLocationsX[i] = value[i].X;
                vendingMachineLocationsY[i] = value[i].Y;
            }
        }
    }
    public string[] VendingMachineTextures
    {
        get
        {
            return vendingMachineTextures;
        }
        set
        {
            vendingMachineTextures = value;
        }
    }
    public VendingType[] VendingMachineTypes
    {
        get
        {
            return vendingMachineTypes;
        }
        set
        {
            vendingMachineTypes = value;
        }
    }
    public List<Point> SquareSpawnPoints
    {
        get
        {
            List<Point> squareSpawns = new List<Point>();
            for (int i = 0; i < squareSpawnPointsX.Count; i++)
            {
                squareSpawns.Add(new Point(squareSpawnPointsX[i], squareSpawnPointsY[i]));
            }
            return squareSpawns;
        }
        set
        {
            squareSpawnPointsX.Clear();
            squareSpawnPointsY.Clear();
            foreach (Point P in value)
            {
                squareSpawnPointsX.Add(P.X);
                squareSpawnPointsY.Add(P.Y);
            }
        }
    }
    public List<Point> BackgroundSquares
    {
        get
        {
            List<Point> bgSquares = new List<Point>();
            for (int i = 0; i < backgroundSquaresX.Count; i++)
            {
                bgSquares.Add(new Point(backgroundSquaresX[i], backgroundSquaresY[i]));
            }
            return bgSquares;
        }
        set
        {
            backgroundSquaresX.Clear();
            backgroundSquaresY.Clear();
            foreach (Point P in value)
            {
                backgroundSquaresX.Add(P.X);
                backgroundSquaresY.Add(P.Y);
            }
        }
    }
    public Vector2 PlayerSpawn
    {
        get
        {
            return new Vector2(playerSpawnX, playerSpawnY);
        }
        set
        {
            playerSpawnX = value.X;
            playerSpawnY = value.Y;
        }
    }
    public Vector2 BossSpawn
    {
        get
        {
            return new Vector2(bossSpawnX, bossSpawnY);
        }
        set
        {
            bossSpawnX = value.X;
            bossSpawnY = value.Y;
        }
    }
    public List<Vector2> EnemySpawnPoints
    {
        get
        {
            List<Vector2> enemySpawns = new List<Vector2>();
            for (int i = 0; i < enemySpawnPointsX.Count; i++)
            {
                enemySpawns.Add(new Vector2(enemySpawnPointsX[i], enemySpawnPointsY[i]));
            }
            return enemySpawns;
        }
        set
        {
            enemySpawnPointsX.Clear();
            enemySpawnPointsY.Clear();
            foreach (Vector2 v2 in value)
            {
                enemySpawnPointsX.Add(v2.X);
                enemySpawnPointsY.Add(v2.Y);
            }
        }
    }
    public ParticleSystem[] ParticleSystems
    {
        get
        {
            try
            {
                int length = PSInitialVelocityX.Length;
                if (PSAnimationDelay.Length == length && PSFrames.Length == length && PSColors.Length == length && PSTextures.Length == length &&
                    PSSpawnX.Length == length && PSSpawnY.Length == length && PSSpawnW.Length == length && PSSpawnH.Length == length && PSRandomness.Length == length &&
                    PSDepths.Length == length && PSSpawnDelay.Length == length && PSScales.Length == length && PSTimes.Length == length && PSForceX.Length == length &&
                    PSForceY.Length == length && PSInitialVelocityY.Length == length && PSLightScales.Length == length)
                {
                    ParticleSystem[] particleSystems = new ParticleSystem[length];
                    for (int i = 0; i < length; i++)
                    {
                        particleSystems[i] = new ParticleSystem(new Vector2(PSInitialVelocityX[i],PSInitialVelocityY[i]), new Vector2(PSForceX[i],PSForceY[i]),PSTimes[i],PSScales[i],PSLightScales[i],PSSpawnDelay[i],PSDepths[i],
                            PSRandomness[i], new Rectangle(PSSpawnX[i],PSSpawnY[i],PSSpawnW[i],PSSpawnH[i]),LoadTextureArray(PSTextures[i]),LoadColorArray(PSColors[i]),LoadColorArray(PSLightColors[i]),PSFrames[i],PSAnimationDelay[i],PSTimeToLive[i],2);
                    }
                    return particleSystems;
                }
                else { throw new ArraysNotSameLengthException(); }
            }
            catch (Exception e)
            {
                Irbis.Irbis.WriteLine("exception caught during level get:" + e.Message + "\nStackTrace:\n" + e.StackTrace);
                Irbis.Irbis.DisplayInfoText("exception caught during level get:" + e.Message);
            }
            return new ParticleSystem[0];
        }
        set
        {
            int length = value.Length;
            InitializeParticleSystem(length);
            for (int i = 0; i < length; i++)
            {
                PSInitialVelocityX[i] = value[i].initialVelocity.X;
                PSInitialVelocityY[i] = value[i].initialVelocity.Y;
                PSForceX[i] = value[i].force.X;
                PSForceY[i] = value[i].force.Y;
                PSTimes[i] = value[i].stateTimes;
                PSScales[i] = value[i].stateScales;
                PSLightScales[i] = value[i].stateLightScales;
                PSSpawnDelay[i] = value[i].spawnDelay;
                PSDepths[i] = value[i].stateDepths;
                PSRandomness[i] = value[i].randomness;
                PSSpawnX[i] = value[i].spawnArea.X;
                PSSpawnY[i] = value[i].spawnArea.Y;
                PSSpawnW[i] = value[i].spawnArea.Width;
                PSSpawnH[i] = value[i].spawnArea.Height;

                PSTextures[i] = new string[value[i].textures.Length];
                for (int j = 0; j < value[i].textures.Length; j++)
                { PSTextures[i][j] = value[i].textures[j].Name; Console.WriteLine("texture:" + value[i].textures[j].Name); }

                PSColors[i] = new uint[value[i].stateColors.Length];
                for (int j = 0; j < value[i].stateColors.Length; j++)
                { PSColors[i][j] = ColorToUInt(value[i].stateColors[j]); }
                PSLightColors[i] = new uint[value[i].stateLightColors.Length];
                for (int j = 0; j < value[i].stateLightColors.Length; j++)
                { PSLightColors[i][j] = ColorToUInt(value[i].stateLightColors[j]); }

                PSFrames[i] = value[i].animationFrames;
                PSAnimationDelay[i] = value[i].animationDelay;
                PSTimeToLive[i] = value[i].timeToLive;
            }
        }
    }
    public Grass[] Grasses
    {
        get
        {
            try
            {
                int length = GinitialRotation.Length;
                if (GinitialRotation.Length == length && GrotationTime.Length == length && Gdensity.Length == length && Gdepth.Length == length &&
                    Grandomness.Length == length && GrotationMin.Length == length && GrotationMax.Length == length && GoriginOffsetX.Length == length &&
                    GoriginOffsetY.Length == length && GareaX.Length == length && GareaY.Length == length && GareaW.Length == length && GareaH.Length == length &&
                    GbladeTextures.Length == length && GtextureDimentionsX.Length == length && GtextureDimentionsY.Length == length)
                {
                    Grass[] grasses = new Grass[length];
                    for (int i = 0; i < length; i++)
                    {
                        grasses[i] = new Grass(GinitialRotation[i], GrotationTime[i], Gdensity[i], Gdepth[i], Grandomness[i], GrotationMin[i], GrotationMax[i],
                            new Vector2(GoriginOffsetX[i], GoriginOffsetY[i]), new Rectangle(GareaX[i], GareaY[i], GareaW[i], GareaH[i]),
                            Irbis.Irbis.LoadTexture(GbladeTextures[i]), new Point(GtextureDimentionsX[i], GtextureDimentionsY[i]), 900, 2);
                    }
                    return grasses;
                }
                else { throw new ArraysNotSameLengthException(); }
            }
            catch (Exception e)
            {
                Irbis.Irbis.WriteLine("exception caught during grass get:" + e.Message + "\nStackTrace:\n" + e.StackTrace);
                Irbis.Irbis.DisplayInfoText("exception caught during grass get:" + e.Message);
            }
            return new Grass[0];
        }
        set
        {
            int length = value.Length;
            InitializeGrass(length);
            for (int i = 0; i < length; i++)
            {
                GinitialRotation[i] = value[i].initialRotation;
                GrotationTime[i] = value[i].rotationTime;
                Gdensity[i] = value[i].density;
                Gdepth[i] = value[i].depth;
                Grandomness[i] = value[i].randomness;
                GrotationMin[i] = value[i].rotationMin;
                GrotationMax[i] = value[i].rotationMax;
                GoriginOffsetX[i] = value[i].bladeOrigin.X;
                GoriginOffsetY[i] = value[i].bladeOrigin.Y;
                GareaX[i] = value[i].area.X;
                GareaY[i] = value[i].area.Y;
                GareaW[i] = value[i].area.Width;
                GareaH[i] = value[i].area.Height;
                GbladeTextures[i] = value[i].bladeTextures.Name;
                GtextureDimentionsX[i] = value[i].textureDimentions.X;
                GtextureDimentionsY[i] = value[i].textureDimentions.Y;
            }
        }
    }


    public Level(bool construct)
    {
        squareSpawnPointsX = new List<int>();
        squareSpawnPointsY = new List<int>();
        squareTextures = new List<string>();
        backgroundSquaresX = new List<int>();
        backgroundSquaresY = new List<int>();
        backgroundTextures = new List<string>();
        backgroundSquareDepths = new List<float>();
        squareDepths = new List<float>();
        levelName = string.Empty;
        enemySpawnPointsX = new List<float>();
        enemySpawnPointsY = new List<float>();
        isOnslaught = false;
        playerSpawnX = 0;
        playerSpawnY = 0;
        bossSpawnX = 0;
        bossSpawnY = 0;

        vendingMachineTextures = new string[0];
        vendingMachineLocationsX = new int[0];
        vendingMachineLocationsY = new int[0];
        vendingMachineTypes = new VendingType[0];

        bossName = string.Empty;

        PSInitialVelocityX = new float[0];
        PSInitialVelocityY = new float[0];
        PSForceX = new float[0];
        PSForceY = new float[0];
        PSTimes = new float[0][];
        PSScales = new float[0][];
        PSLightScales = new float[0][];
        PSSpawnDelay = new float[0];
        PSDepths = new float[0][];
        PSRandomness = new float[0][];
        PSSpawnX = new int[0];
        PSSpawnY = new int[0];
        PSSpawnW = new int[0];
        PSSpawnH = new int[0];
        PSTextures = new string[0][];
        PSColors = new uint[0][];
        PSLightColors = new uint[0][];
        PSFrames = new int[0][];
        PSAnimationDelay = new float[0];
        PSTimeToLive = new float[0];

        darkness = 0.5f;

        GinitialRotation = new float[0];
        GrotationTime = new float[0];
        Gdensity = new float[0];
        Gdepth = new float[0];
        Grandomness = new float[0][];
        GrotationMin = new float[0];
        GrotationMax = new float[0];
        GoriginOffsetX = new float[0];
        GoriginOffsetY = new float[0];
        GareaX = new int[0];
        GareaY = new int[0];
        GareaW = new int[0];
        GareaH = new int[0];
        GbladeTextures = new string[0];
        GtextureDimentionsX = new int[0];
        GtextureDimentionsY = new int[0];
    }

    private uint ColorToUInt(Color color)
    { return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0)); }

    private Color[] LoadColorArray(uint[] colors)
    {
        Color[] colorArray = new Color[colors.Length];

        for (int i = 0; i < colors.Length; i++)
        { colorArray[i] = new Color(colors[i]); }

        return colorArray;
    }

    public Texture2D[] LoadTextureArray(string[] textures)
    {
        Texture2D[] textureArray = new Texture2D[textures.Length];

        for (int i = 0; i < textures.Length; i++)
        { textureArray[i] = Irbis.Irbis.LoadTexture(textures[i]); }

        return textureArray;
    }

    public void InitializeGrass(int length)
    {
        GinitialRotation = new float[length];
        GrotationTime = new float[length];
        Gdensity = new float[length];
        Gdepth = new float[length];
        Grandomness = new float[length][];
        GrotationMin = new float[length];
        GrotationMax = new float[length];
        GoriginOffsetX = new float[length];
        GoriginOffsetY = new float[length];
        GareaX = new int[length];
        GareaY = new int[length];
        GareaW = new int[length];
        GareaH = new int[length];
        GbladeTextures = new string[length];
        GtextureDimentionsX = new int[length];
        GtextureDimentionsY = new int[length];
    }

    public void InitializeParticleSystem(int length)
    {
        PSInitialVelocityX = new float[length];
        PSInitialVelocityY = new float[length];
        PSForceX = new float[length];
        PSForceY = new float[length];
        PSTimes = new float[length][];
        PSScales = new float[length][];
        PSLightScales = new float[length][];
        PSSpawnDelay = new float[length];
        PSDepths = new float[length][];
        PSRandomness = new float[length][];
        PSSpawnX = new int[length];
        PSSpawnY = new int[length];
        PSSpawnW = new int[length];
        PSSpawnH = new int[length];
        PSTextures = new string[length][];
        PSColors = new uint[length][];
        PSLightColors = new uint[length][];
        PSFrames = new int[length][];
        PSAnimationDelay = new float[length];
        PSTimeToLive = new float[length];
    }

    public void Load(string filename)
    {
        Level thisLevel = new Level(true);
        Irbis.Irbis.WriteLine("loading " + filename + "...");
        FileStream stream = new FileStream(filename, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            thisLevel = (Level)formatter.Deserialize(stream);
            AssignLocalVariables(thisLevel);
            Irbis.Irbis.WriteLine("load successful.");
        }
        catch (SerializationException e)
        {
            Console.WriteLine("load failed.\n" + e.Message);
            Irbis.Irbis.WriteLine("load failed.\n" + e.Message);
            Irbis.Irbis.WriteLine("attempting conversion...");
            thisLevel = Irbis.Irbis.ConvertOldLevelFileToNew(filename);
        }
        finally
        {
            Irbis.Irbis.WriteLine();
            stream.Close();
        }
    }

    public void Save(string filename)
    {
        Irbis.Irbis.WriteLine("saving " + filename + "...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filename, FileMode.Create);
        try
        {
            formatter.Serialize(stream, this);
            Irbis.Irbis.WriteLine("save successful.");
        }
        catch (SerializationException e)
        {
            Console.WriteLine("failed.\n" + e.Message);
            Irbis.Irbis.WriteLine("failed.\n" + e.Message);
            throw;
        }
        finally
        {
            Irbis.Irbis.WriteLine();
            stream.Close();
        }
    }

    private void AssignLocalVariables(Level level)
    {
        this = level;
        Irbis.Irbis.WriteLine("                  squares: " + squareTextures.Count);
        Irbis.Irbis.WriteLine("       background squares: " + backgroundSquareDepths.Count);
        Irbis.Irbis.WriteLine("               level name: " + levelName);
        Irbis.Irbis.WriteLine("       enemy spawn points: " + EnemySpawnPoints.Count);
        Irbis.Irbis.WriteLine("              isOnslaught: " + isOnslaught);
        Irbis.Irbis.WriteLine("             player spawn: " + PlayerSpawn);
        Irbis.Irbis.WriteLine("               boss spawn: " + BossSpawn);
        if (vendingMachineTextures.Length == vendingMachineTypes.Length && vendingMachineTextures.Length == vendingMachineLocationsX.Length && vendingMachineTextures.Length == vendingMachineLocationsY.Length)
        { Irbis.Irbis.WriteLine("         vending Machines: " + vendingMachineTextures.Length); }
        else
        { Irbis.Irbis.WriteLine("error loading vending machines, improper array lengths"); }
    }

    public override string ToString()
    {
        string returnstring = "squares: " + squareTextures.Count;
        for (int i = 0; i < squareTextures.Count; i++)
        {
            returnstring += "\nsquare[" + i + "] tex: " + squareTextures[i];
            returnstring += "\nsquare[" + i + "] pos: {X:" + squareSpawnPointsX[i] + " Y:" + squareSpawnPointsY[i] + "}";
        }
        returnstring += "\nEnemy Spawn Points: " + EnemySpawnPoints.Count;
        for (int i = 0; i < EnemySpawnPoints.Count; i++)
        {
            returnstring += "\nenemy[" + i + "] pos: " + EnemySpawnPoints[i];
        }
        returnstring += "\nVending Machine Locations: " + VendingMachineLocations.Length;
        for (int i = 0; i < VendingMachineLocations.Length; i++)
        {
            returnstring += "\nvendingMachineLocations[" + i + "]: " + VendingMachineLocations[i];
        }
        returnstring += "\nVending Machine Textures: " + VendingMachineTextures.Length;
        for (int i = 0; i < VendingMachineTextures.Length; i++)
        {
            returnstring += "\nVendingMachineTextures[" + i + "]: " + VendingMachineTextures[i];
        }
        returnstring += "\nVending Machine Types: " + VendingMachineTypes.Length;
        for (int i = 0; i < VendingMachineTypes.Length; i++)
        {
            returnstring += "\nVendingMachineTypes[" + i + "]: " + VendingMachineTypes[i];
        }
        return returnstring;
    }
}
