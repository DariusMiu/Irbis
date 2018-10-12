using Irbis;
using System;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[DataContract]
[Serializable]
public class LevelRecord
{
    [DataMember]
    public string level;
    [DataMember]
    public double time;
    [DataMember]
    public uint wins;
    public bool Win
    { get { return (wins > 0); } }

    public LevelRecord(string Level, double Time, uint Victories)
    { level = Level; time = Time; wins = Victories; }
}

[DataContract]
[Serializable]
public struct SaveFile
{
    [DataMember]
    public List<LevelRecord> winList;
    [DataMember]
    public List<LevelRecord> loseList;
    [DataMember]
    public int enchantSlots;
    [DataMember]
    public int bestOnslaughtWave;
    [DataMember]
    public string bestOnslaughtWaveLevel;
    [DataMember]
    public string lastPlayedLevel;

    public SaveFile(bool LastLevelEmpty)
	{
        winList = new List<LevelRecord>();
        loseList = new List<LevelRecord>();
        enchantSlots = 0;
        bestOnslaughtWave = 0;
        bestOnslaughtWaveLevel = string.Empty;
        if (LastLevelEmpty)
        { lastPlayedLevel = string.Empty; }
        else
        { lastPlayedLevel = "c1b1"; }
    }

    public int FindWinRecord(string record)
    {
        for (int i = 0; i < winList.Count; i++)
        {
            if (winList[i].level == record)
            { return i; }
        }
        return -1;
    }

    public int FindLoseRecord(string record)
    {
        for (int i = 0; i < loseList.Count; i++)
        {
            if (loseList[i].level == record)
            { return i; }
        }
        return -1;
    }

    public void Load(string filename)
    {
        FileStream stream = new FileStream(filename, FileMode.Open);
        XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(stream, new XmlDictionaryReaderQuotas());
        try
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SaveFile));
            SaveFile thisSave = (SaveFile)serializer.ReadObject(reader, true);
            this = thisSave;
        }
        catch (Exception e)
        {
            Console.WriteLine("load failed. " + e.Message);
            Irbis.Irbis.WriteLine("load failed. " + e.Message);
        }
        finally
        {
            reader.Close();
            stream.Close();
        }
    }

    public void Save(string filename)
    {
        Irbis.Irbis.WriteLine("saving " + filename + "...");
        FileStream stream = new FileStream(filename, FileMode.Create);
        try
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SaveFile));
            serializer.WriteObject(stream, this);
        }
        catch (Exception e)
        {
            Console.WriteLine("save failed. " + e.Message);
            Irbis.Irbis.WriteLine("save failed. " + e.Message);
            Irbis.Irbis.DisplayInfoText("save failed. " + e.Message, Color.Red);
        }
        finally
        { stream.Close(); }
    }

    private void AssignLocalVariables(SaveFile save)
    {
        winList = save.winList;
        Irbis.Irbis.WriteLine("winList: " + winList.Count);
        loseList = save.loseList;
        Irbis.Irbis.WriteLine("loseList: " + loseList.Count);
        enchantSlots = save.enchantSlots;
        Irbis.Irbis.WriteLine("enchantSlots: " + enchantSlots);
        bestOnslaughtWave = save.bestOnslaughtWave;
        Irbis.Irbis.WriteLine("bestOnslaughtWave: " + bestOnslaughtWave);
        bestOnslaughtWaveLevel = save.bestOnslaughtWaveLevel;
        Irbis.Irbis.WriteLine("bestOnslaughtWaveLevel: " + bestOnslaughtWaveLevel);
        lastPlayedLevel = save.lastPlayedLevel;
        Irbis.Irbis.WriteLine("lastPlayedLevel: " + lastPlayedLevel);
    }

    public void Print(string filename)
    {
        this.Load(filename);
        Irbis.Irbis.WriteLine("last level played: " + lastPlayedLevel);
        Irbis.Irbis.WriteLine("enchant slots (bosses): " + enchantSlots);
        Irbis.Irbis.WriteLine("best onslaught wave: " + bestOnslaughtWave + ", on level: " + bestOnslaughtWaveLevel);
        Irbis.Irbis.WriteLine("winList: " + winList.Count);
        Irbis.Irbis.WriteLine("loseList: " + loseList.Count);
    }
}
