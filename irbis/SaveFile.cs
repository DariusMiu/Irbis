using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveFile
{   //start at line 11 to make it easier to count, just subtract 10 from final line
    public List<string> winList;
    public List<string> loseList;
    public int enchantSlots;
    public int bestOnslaughtWave;
    public string bestOnslaughtWaveLevel;
    public string lastPlayedLevel;
    public List<double> timerWinList;
    public List<double> timerLoseList;

    //start at line 11 to make it easier to count, just subtract 10 from final line
    //static int numberOfVariables = 03;

    public SaveFile()
	{
              winList = new List<string>();
             loseList = new List<string>();
         timerWinList = new List<double>();
        timerLoseList = new List<double>();
        enchantSlots = 0;
        bestOnslaughtWave = 0;
        bestOnslaughtWaveLevel = string.Empty;
        lastPlayedLevel = "c0b0";
    }

    public void Load(string filename)
    {
        SaveFile thisSave = new SaveFile();
        Irbis.Irbis.WriteLine("loading " + filename + "...");
        FileStream stream = new FileStream(filename, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            thisSave = (SaveFile)formatter.Deserialize(stream);
            AssignLocalVariables(thisSave);
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

    private void AssignLocalVariables(SaveFile save)
    {
        winList = save.winList;
        Irbis.Irbis.WriteLine("          unlocked bosses: " + winList.Count);
        loseList = save.loseList;
        Irbis.Irbis.WriteLine("          unlocked bosses: " + loseList.Count);
        enchantSlots = save.enchantSlots;
        Irbis.Irbis.WriteLine("   enchant slots (bosses): " + enchantSlots);
        bestOnslaughtWave = save.bestOnslaughtWave;
        Irbis.Irbis.WriteLine("        bestOnslaughtWave: " + bestOnslaughtWave);
        bestOnslaughtWaveLevel = save.bestOnslaughtWaveLevel;
        if (string.IsNullOrWhiteSpace(bestOnslaughtWaveLevel)) { Irbis.Irbis.WriteLine("   bestOnslaughtWaveLevel: None"); }
        else { Irbis.Irbis.WriteLine("   bestOnslaughtWaveLevel: " + bestOnslaughtWaveLevel); }
        lastPlayedLevel = save.lastPlayedLevel;
        Irbis.Irbis.WriteLine("        last level played: " + lastPlayedLevel);
        timerWinList = save.timerWinList;
        Irbis.Irbis.WriteLine("             timerWinList: " + timerWinList.Count);
        timerLoseList = save.timerLoseList;
        Irbis.Irbis.WriteLine("            timerLoseList: " + timerLoseList.Count);
        //for (int i = 0; i < timerList.Count; i++)
        //{
        //    Irbis.Irbis.WriteLine("level[" + i + "] best time: " + timerList[i] + "s");
        //}
    }

    public void Print()
    {
        Irbis.Irbis.WriteLine("last level played: " + lastPlayedLevel);
        Irbis.Irbis.WriteLine("enchant slots (bosses): " + enchantSlots);
        Irbis.Irbis.WriteLine("best onslaught wave: " + bestOnslaughtWave + ", on level: " + bestOnslaughtWaveLevel);
        for (int i = 0; i < winList.Count; i++)
        {
            Irbis.Irbis.WriteLine(" win list[" + i + "]: " + winList[i] + ", best time: " + timerWinList[i]);
        }
        for (int i = 0; i < loseList.Count; i++)
        {
            Irbis.Irbis.WriteLine("lose list[" + i + "]: " + loseList[i] + ", best time: " + timerLoseList[i]);
        }
        Irbis.Irbis.WriteLine();
    }
}
