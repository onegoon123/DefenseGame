using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class SpawnData
{
    public int point;
    public int code;
    public float time;
}

[Serializable]
public class EnemyWave
{
    public List<SpawnData> spawnDatas = new List<SpawnData>();
}

public class EnemySpawner : MonoBehaviour
{
    public TextAsset waveDataFile;

    public float waveWaitTime = 10.0f;
    
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

        return;

        /*
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
                continue;

            string[] fields = lines[i].Split(',');

            SkillData skill = new SkillData
            {
                skillId = int.Parse(fields[0]),
                skillName = fields[1],
                range = int.Parse(fields[2]),
                information = fields[3],
                icon = fields[4]
            };

            skillDatas.Add(skill);
        }
        */
    }
}
