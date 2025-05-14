using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class SpawnData
{
    public int point;
    public int code;
    public int count;
}

[Serializable]
public class EnemyWave
{
    public List<SpawnData> spawnDatas = new List<SpawnData>();
    public float waveWaitTime = 0;
}

public class EnemySpawner : MonoBehaviour
{
    public TextAsset waveDataFile;

    public List<int2> SpawnPoint = new List<int2>();
    public List<EnemyWave> waves = new List<EnemyWave>();

    [ContextMenu("Load Wave Data")]
    public void LoadWaveData()
    {
        SpawnPoint.Clear();
        waves.Clear();

        string[] lines = waveDataFile.text.Split("\n");
        string[] fields = lines[1].Split(',');

        for (int i = 0; i < fields.Length; i+=2)
        {
            if (fields.Length <= i + 1)
                break;

            int2 point;
            point.x = int.Parse(fields[i]);
            point.y = int.Parse(fields[i+1]);
            SpawnPoint.Add(point);
        }

        for (int i = 2; i < lines.Length; i += 3)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields0 = lines[i].Split(',');
            string[] fields1 = lines[i + 1].Split(',');
            string[] fields2 = lines[i + 2].Split(',');

            EnemyWave wave = new EnemyWave();
            wave.waveWaitTime = float.Parse(fields2[0]);

            for (int j = 2; j < fields0.Length; j++)
            {
                if (string.IsNullOrWhiteSpace(fields0[j]))
                    continue;
                int point = int.Parse(fields0[j]);
                int code = int.Parse(fields1[j]);
                int count = int.Parse(fields2[j]);

                SpawnData spawnData = new SpawnData
                {
                    point = int.Parse(fields0[j]),
                    code = int.Parse(fields1[j]),
                    count = int.Parse(fields2[j]),
                };
                wave.spawnDatas.Add(spawnData);
            }

            waves.Add(wave);
        }
    }
}
