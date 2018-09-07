using Irbis;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.Serialization.Formatters.Binary;

[DataContract(Name="Level")]
public struct NewLevel
{


    /*public Point[] VendingMachineLocations;
    public string[] VendingMachineTextures;
    public VendingType[] VendingMachineTypes;
    public Point[] SquareSpawnPoints;
    public Point[] BackgroundSquares;
    public Color BackgroundColor;
    public Rectangle?[] SquareColliders;
    public Vector2[] SquareOrigins;
    public Vector2 PlayerSpawn;
    public Vector2 BossSpawn;
    public Vector2[] EnemySpawnPoints;
    public ParticleSystem[] ParticleSystems;
    public Grass[] Grasses;*/

    public Trigger[] Triggers;

    public Level(bool construct)
    {

        Triggers = new Trigger[0];
    }

    public static uint ColorToUint(Color color)
    { return (uint)((color.A << 24) | (color.B << 16) | (color.G << 8) | (color.R << 0)); }

    private static Color[] LoadColorArray(uint[] colors)
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
        Irbis.Irbis.WriteLine("               level name: " + levelName);
        Irbis.Irbis.WriteLine("                  squares: " + squareTextures.Count);
        Irbis.Irbis.WriteLine("       background squares: " + backgroundSquareDepths.Count);
        if (Triggers != null)
        { Irbis.Irbis.WriteLine("                 triggers: " + Triggers.Length); }
        Irbis.Irbis.WriteLine("       enemy spawn points: " + EnemySpawnPoints.Length);
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
        returnstring += "\nEnemy Spawn Points: " + EnemySpawnPoints.Length;
        for (int i = 0; i < EnemySpawnPoints.Length; i++)
        { returnstring += "\nenemy[" + i + "] pos: " + EnemySpawnPoints[i]; }
        returnstring += "\nVending Machine Locations: " + VendingMachineLocations.Length;
        for (int i = 0; i < VendingMachineLocations.Length; i++)
        { returnstring += "\nvendingMachineLocations[" + i + "]: " + VendingMachineLocations[i]; }
        returnstring += "\nVending Machine Textures: " + VendingMachineTextures.Length;
        for (int i = 0; i < VendingMachineTextures.Length; i++)
        { returnstring += "\nVendingMachineTextures[" + i + "]: " + VendingMachineTextures[i]; }
        returnstring += "\nVending Machine Types: " + VendingMachineTypes.Length;
        for (int i = 0; i < VendingMachineTypes.Length; i++)
        { returnstring += "\nVendingMachineTypes[" + i + "]: " + VendingMachineTypes[i]; }
        return returnstring;
    }
}
