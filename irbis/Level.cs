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
public class Level
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



    public Level()
	{
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
    }

    public void Load(string filename)
    {
        Level thisLevel = new Level();
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
        squareSpawnPointsX = level.squareSpawnPointsX;
        squareSpawnPointsY = level.squareSpawnPointsY;
        squareTextures = level.squareTextures;
        squareDepth = level.squareDepth;
        Irbis.Irbis.WriteLine("           squares: " + squareTextures.Count);
        backgroundSquaresX = level.backgroundSquaresX;
        backgroundSquaresY = level.backgroundSquaresY;
        backgroundTextures = level.backgroundTextures;
        backgroundSquareDepths = level.backgroundSquareDepths;
        Irbis.Irbis.WriteLine("background squares: " + backgroundSquareDepths.Count);
        levelName = level.levelName;
        Irbis.Irbis.WriteLine("        level name: " + levelName);
        enemySpawnPointsX = level.enemySpawnPointsX;
        enemySpawnPointsY = level.enemySpawnPointsY;
        Irbis.Irbis.WriteLine("enemy spawn points: " + EnemySpawnPoints.Count);
        isOnslaught = level.isOnslaught;
        Irbis.Irbis.WriteLine("       isOnslaught: " + isOnslaught);
        playerSpawnX = level.playerSpawnX;
        playerSpawnY = level.playerSpawnY;
        Irbis.Irbis.WriteLine("      player spawn: " + PlayerSpawn);
        bossSpawnX = level.bossSpawnX;
        bossSpawnY = level.bossSpawnY;
        Irbis.Irbis.WriteLine("        boss spawn: " + BossSpawn);
    }

    public void Debug()
    {
        Irbis.Irbis.WriteLine("           squares: " + squareTextures.Count);
        for (int i = 0; i < squareTextures.Count; i++)
        {
            Irbis.Irbis.WriteLine("square[" + i + "] tex: " + squareTextures[i]);
            Irbis.Irbis.WriteLine("square[" + i + "] pos: {X:" + squareSpawnPointsX[i] + " Y:" + squareSpawnPointsY[i] + "}");
        }
        for (int i = 0; i < EnemySpawnPoints.Count; i++)
        {
            Irbis.Irbis.WriteLine("enemy[" + i + "] pos: " + EnemySpawnPoints[i]);
        }

    }
}
