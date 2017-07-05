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
    public float squareDepth;
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



    public Level(bool construct)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Level.Level"); }
        squareSpawnPointsX = new List<int>();
        squareSpawnPointsY = new List<int>();
        squareTextures = new List<string>();
        backgroundSquaresX = new List<int>();
        backgroundSquaresY = new List<int>();
        backgroundTextures = new List<string>();
        backgroundSquareDepths = new List<float>();
        squareDepth = 0f;
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

    }

    public void Load(string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Level.Load"); }
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

    public void Save(string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Level.Save"); }
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Level.AssignLocalVariables"); }
        squareSpawnPointsX = level.squareSpawnPointsX;
        squareSpawnPointsY = level.squareSpawnPointsY;
        squareTextures = level.squareTextures;
        squareDepth = level.squareDepth;
        Irbis.Irbis.WriteLine("                  squares: " + squareTextures.Count);
        backgroundSquaresX = level.backgroundSquaresX;
        backgroundSquaresY = level.backgroundSquaresY;
        backgroundTextures = level.backgroundTextures;
        backgroundSquareDepths = level.backgroundSquareDepths;
        Irbis.Irbis.WriteLine("       background squares: " + backgroundSquareDepths.Count);
        levelName = level.levelName;
        Irbis.Irbis.WriteLine("               level name: " + levelName);
        enemySpawnPointsX = level.enemySpawnPointsX;
        enemySpawnPointsY = level.enemySpawnPointsY;
        Irbis.Irbis.WriteLine("       enemy spawn points: " + EnemySpawnPoints.Count);
        isOnslaught = level.isOnslaught;
        Irbis.Irbis.WriteLine("              isOnslaught: " + isOnslaught);
        playerSpawnX = level.playerSpawnX;
        playerSpawnY = level.playerSpawnY;
        Irbis.Irbis.WriteLine("             player spawn: " + PlayerSpawn);
        bossSpawnX = level.bossSpawnX;
        bossSpawnY = level.bossSpawnY;
        Irbis.Irbis.WriteLine("               boss spawn: " + BossSpawn);
        vendingMachineTextures = level.vendingMachineTextures;
        vendingMachineTypes = level.vendingMachineTypes;
        vendingMachineLocationsX = level.vendingMachineLocationsX;
        vendingMachineLocationsY = level.vendingMachineLocationsY;
        if (vendingMachineTextures.Length == vendingMachineTypes.Length && vendingMachineTextures.Length == vendingMachineLocationsX.Length && vendingMachineTextures.Length == vendingMachineLocationsY.Length)
        {
            Irbis.Irbis.WriteLine("         vending Machines: " + vendingMachineTextures.Length);
        }
        else
        {
            Irbis.Irbis.WriteLine("error loading vending machines, improper array lengths");
        }
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
