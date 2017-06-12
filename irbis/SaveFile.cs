using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveFile
{   //start at line 11 to make it easier to count, just subtract 10 from final line
    int unlockedLevels;
    int lastPlayedLevel;
    public List<float> timerList;

    //start at line 11 to make it easier to count, just subtract 10 from final line
    //static int numberOfVariables = 03;

    public SaveFile()
	{
        unlockedLevels = -1;
        lastPlayedLevel = -1;
        timerList = new List<float>();
        //timerList.Add(-1f);
    }

    public void Load(string filename)
    {
        SaveFile thisSave = new SaveFile();
        Console.WriteLine("loading " + filename + "...");
        FileStream stream = new FileStream(filename, FileMode.Open);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            thisSave = (SaveFile)formatter.Deserialize(stream);
            Console.WriteLine("load successful.");
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

        AssignLocalVariables(thisSave);
    }

    public void Save(string filename)
    {
        Console.WriteLine("saving " + filename + "...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(filename, FileMode.Create);
        try
        {
            formatter.Serialize(stream, this);
            Console.WriteLine("save successful.");
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

    private void AssignLocalVariables(SaveFile save)
    {
        unlockedLevels = save.unlockedLevels;
        //Console.WriteLine("   unlocked levels: " + unlockedLevels);
        lastPlayedLevel = save.lastPlayedLevel;
        //Console.WriteLine(" last level played: " + lastPlayedLevel);
        timerList = save.timerList;
        //Console.WriteLine("         timerList: " + timerList.Count);
        //for (int i = 0; i < timerList.Count; i++)
        //{
        //    Console.WriteLine("level[" + i + "] best time: " + timerList[i] + "s");
        //}
    }
}
