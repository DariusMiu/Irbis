using Irbis;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class OnslaughtSpawner
{
    public ulong Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }
    private ulong points;
    private float timer;
    public float timeUntilNextSpawn;
    public int maxEnemies;
    public int wave;
    public int enemiesLeftThisWave;
    public int enemiesThisWave;
    public float enemyHealth;
    public float enemyDamage;
    public float enemySpeed;
    public bool waveStarted;
    public int enemiesKilled;
    public uint pointsPerKill;
    public uint pointsPerWave;

    public List<VendingMachine> vendingMachineList;

    public OnslaughtSpawner()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OnslaughtSpawner.OnslaughtSpawner"); }
        wave = 1;

        enemyHealth = 100f;
        enemyDamage = 10f;
        enemySpeed = 150f;

        enemiesKilled = 0;
        pointsPerKill = 10;
        pointsPerWave = 100;

        maxEnemies = 5;
        enemiesLeftThisWave = enemiesThisWave = 4 + wave;
        timer = 1f;
        timeUntilNextSpawn = 30f;
        points = 0;
        waveStarted = false;

        vendingMachineList = new List<VendingMachine>();
    }

    public void NextWave()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OnslaughtSpawner.NextWave"); }
        waveStarted = false;
        wave++;
        points += pointsPerWave;

        pointsPerKill += 10;
        pointsPerWave += 100;

        enemiesKilled = 0;

        timeUntilNextSpawn = 30f;
        maxEnemies = 5 + (wave / 3);
        enemiesLeftThisWave = enemiesThisWave = 4 + wave;
        enemyHealth = 100f + (wave * 5);
        enemyDamage = 10f + (wave);
        enemySpeed = 150f + (wave / 2f);
    }

    public void SkipToWave(int waveNumber)
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OnslaughtSpawner.SkipToWave"); }
        wave = waveNumber - 1;
        NextWave();
    }

    public void EnemyKilled()
    {
        points += pointsPerKill;
        enemiesKilled++;
    }

    public override string ToString()
    {
        string debugstring = string.Empty;

        debugstring += "\npoints: " + points + "\ntimer: " + timer + "\ntimeUntilNextSpawn: " + timeUntilNextSpawn + "\nmaxEnemies: " + maxEnemies + "\nwave: " + wave
        + "\nenemiesLeftThisWave: " + enemiesLeftThisWave + "\nenemiesThisWave: " + enemiesThisWave + "\nenemyHealth: " + enemyHealth + "\nenemyDamage: " + enemyDamage
        +"\nenemySpeed: " + enemySpeed + "\nwaveStarted: " + waveStarted + "\nenemiesKilled: " + enemiesKilled + "\npointsPerKill: " + pointsPerKill
        + "\npointsPerWave: " + pointsPerWave +"\nvendingMachines: " + vendingMachineList.Count;

        foreach (VendingMachine v in vendingMachineList)
        {
            debugstring += "\nvendingMachine: " + v.ToString();
        }

        return debugstring;
    }

    public bool EnemySpawnTimer()
    {
        //if (Irbis.Irbis.debug > 4) { Irbis.Irbis.methodLogger.AppendLine("OnslaughtSpawner.EnemySpawnTimer"); }
        timeUntilNextSpawn -= Irbis.Irbis.DeltaTime;
        if (timeUntilNextSpawn <= 0)
        {
            waveStarted = true;
            timeUntilNextSpawn = timer;
            enemiesLeftThisWave--;
            return true;
        }
        return false;
    }
}

