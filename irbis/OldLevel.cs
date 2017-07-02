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
public struct OldLevel
{
    //public List<Square> squareList;
    private List<int> squareSpawnPointsX;
    private List<int> squareSpawnPointsY;
    private List<float> enemySpawnPointsX;
    private List<float> enemySpawnPointsY;
    private List<int> backgroundSquaresX;
    private List<int> backgroundSquaresY;

    public string[] squareTextures;
    public float squareDepth;
    public string[] backgroundTextures;
    public float[] backgroundSquareDepths;
    public string levelName;
    public bool isOnslaught;
    private float playerSpawnX;
    private float playerSpawnY;
    private float bossSpawnX;
    private float bossSpawnY;

    public Point[] SquareSpawnPoints
    {
        get
        {
            Point[] squareSpawns = new Point[squareSpawnPointsX.Count];
            for (int i = 0; i < squareSpawns.Length; i++)
            {
                squareSpawns[i] = new Point(squareSpawnPointsX[i], squareSpawnPointsY[i]);
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

    public Point[] BackgroundSquares
    {
        get
        {
            Point[] bgSquares = new Point[backgroundSquaresX.Count];
            for (int i = 0; i < backgroundSquaresX.Count; i++)
            {
                bgSquares[i] = new Point(backgroundSquaresX[i], backgroundSquaresY[i]);
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



    public OldLevel(bool construct)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.OldLevel"); }
        squareSpawnPointsX = new List<int>();
        squareSpawnPointsY = new List<int>();
        squareTextures = new string[0];
        backgroundSquaresX = new List<int>();
        backgroundSquaresY = new List<int>();
        backgroundTextures = new string[0];
        backgroundSquareDepths = new float[0];
        squareDepth = 0f;
        levelName = string.Empty;
        enemySpawnPointsX = new List<float>();
        enemySpawnPointsY = new List<float>();
        isOnslaught = false;
        playerSpawnX = 0;
        playerSpawnY = 0;
        bossSpawnX = 0;
        bossSpawnY = 0;
    }

    public void Load(string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.Load"); }
        OldLevel thisLevel = new OldLevel(true);
        Irbis.Irbis.WriteLine("loading " + filename + "...");
        FileStream stream = new FileStream(filename, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            thisLevel = (OldLevel)formatter.Deserialize(stream);
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
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.Save"); }
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

    public static void Save(OldLevel level, string filename)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.Save"); }
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filename, FileMode.Create);
        try
        {
            formatter.Serialize(stream, level);
        }
        catch (SerializationException e)
        {
            Console.WriteLine("failed.\n" + e.Message);
            throw;
        }
        finally
        {
            stream.Close();
        }
    }

    private void AssignLocalVariables(OldLevel OldLevel)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.AssignLocalVariables"); }
        squareSpawnPointsX = OldLevel.squareSpawnPointsX;
        squareSpawnPointsY = OldLevel.squareSpawnPointsY;
        squareTextures = OldLevel.squareTextures;
        squareDepth = OldLevel.squareDepth;
        Irbis.Irbis.WriteLine("           squares: " + squareTextures.Length);
        backgroundSquaresX = OldLevel.backgroundSquaresX;
        backgroundSquaresY = OldLevel.backgroundSquaresY;
        backgroundTextures = OldLevel.backgroundTextures;
        backgroundSquareDepths = OldLevel.backgroundSquareDepths;
        Irbis.Irbis.WriteLine("background squares: " + backgroundSquareDepths.Length);
        levelName = OldLevel.levelName;
        Irbis.Irbis.WriteLine("        OldLevel name: " + levelName);
        enemySpawnPointsX = OldLevel.enemySpawnPointsX;
        enemySpawnPointsY = OldLevel.enemySpawnPointsY;
        Irbis.Irbis.WriteLine("enemy spawn points: " + EnemySpawnPoints.Count);
        isOnslaught = OldLevel.isOnslaught;
        Irbis.Irbis.WriteLine("       isOnslaught: " + isOnslaught);
        playerSpawnX = OldLevel.playerSpawnX;
        playerSpawnY = OldLevel.playerSpawnY;
        Irbis.Irbis.WriteLine("      player spawn: " + PlayerSpawn);
        bossSpawnX = OldLevel.bossSpawnX;
        bossSpawnY = OldLevel.bossSpawnY;
        Irbis.Irbis.WriteLine("        boss spawn: " + BossSpawn);
    }

    public void Debug()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OldLevel.Debug"); }
        Irbis.Irbis.WriteLine("           squares: " + squareTextures.Length);
        for (int i = 0; i < squareTextures.Length; i++)
        {
            Irbis.Irbis.WriteLine("square[" + i + "] tex: " + squareTextures[i]);
            Irbis.Irbis.WriteLine("square[" + i + "] pos: {X:" + squareSpawnPointsX[i] + " Y:" + squareSpawnPointsY[i] + "}");
        }
        for (int i = 0; i < EnemySpawnPoints.Count; i++)
        {
            Irbis.Irbis.WriteLine("enemy[" + i + "] pos: " + EnemySpawnPoints[i]);
        }

    }

    public void Debug(bool butts)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("Level.Debug"); }
        Console.WriteLine("           squares: " + squareTextures.Length);
        for (int i = 0; i < squareTextures.Length; i++)
        {
            Console.WriteLine("square[" + i + "] tex: " + squareTextures[i]);
            Console.WriteLine("square[" + i + "] pos: {X:" + squareSpawnPointsX[i] + " Y:" + squareSpawnPointsY[i] + "}");
        }
        for (int i = 0; i < EnemySpawnPoints.Count; i++)
        {
            Console.WriteLine("enemy[" + i + "] pos: " + EnemySpawnPoints[i]);
        }
    }

    public static OldLevel LevelConverter(Level level)
    {
        OldLevel thislevel = new OldLevel(true);

        thislevel.SquareSpawnPoints = level.SquareSpawnPoints.ToArray();
        thislevel.squareTextures = level.squareTextures.ToArray();
        thislevel.squareDepth = level.squareDepth;
        thislevel.BackgroundSquares = level.BackgroundSquares.ToArray();
        thislevel.backgroundTextures = level.backgroundTextures.ToArray();
        thislevel.backgroundSquareDepths = level.backgroundSquareDepths.ToArray();
        thislevel.levelName = level.levelName;
        thislevel.EnemySpawnPoints = level.EnemySpawnPoints;
        thislevel.isOnslaught = level.isOnslaught;
        thislevel.PlayerSpawn = level.PlayerSpawn;
        thislevel.BossSpawn = level.BossSpawn;

        return thislevel;
    }
}
