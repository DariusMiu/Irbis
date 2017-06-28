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
    float timer;
    public float timeUntilNextSpawn;
    public int maxEnemies;
    public int wave;
    public int enemiesLeftThisWave;
    public int enemiesThisWave;
    public int points;
    public float enemyHealth;
    public float enemyDamage;
    public float enemySpeed;
    public bool waveStarted;


    public OnslaughtSpawner(int waveNumber)
    {
        wave = waveNumber;

        enemyHealth = 100f;
        enemyDamage = 10f;
        enemySpeed = 300f;

        maxEnemies = 5;
        enemiesLeftThisWave = enemiesThisWave = 4 + wave;
        timer = 1f;
        timeUntilNextSpawn = 30f;
    }

    public void NextWave()
    {
        waveStarted = false;
        wave++;

        timeUntilNextSpawn = 30f;
        maxEnemies = 5 + (wave / 3);
        enemiesLeftThisWave = enemiesThisWave = 4 + wave;
        enemyHealth = 100f + (wave * 5);
        enemyDamage = 10f + (wave);
        enemySpeed = 300f + (wave * 2);
    }

    public void SkipToWave(int waveNumber)
    {
        wave = waveNumber - 1;

        NextWave();
    }

    public bool enemySpawnTimer()
    {
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

