using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;

enum EnemySpawnerState
{
    None,
    Wait,
    Wave,
    Clear,
}

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
    public List<GameObject> enemies = new List<GameObject>();
    
    private EnemySpawnerState currentState;
    private EnemyWave currentWave = null;
    private int waveIndex = 0;
    private float waitTime = 0;


    private void ChangeState(EnemySpawnerState state)
    {
        switch (currentState)
        {
            case EnemySpawnerState.Wait:
                WaitEnd();
                break;
            case EnemySpawnerState.Wave:
                WaveEnd();
                break;
            case EnemySpawnerState.Clear:
                ClearEnd();
                break;
        }
        currentState = state;
        switch (currentState)
        {
            case EnemySpawnerState.Wait:
                WaitStart();
                break;
            case EnemySpawnerState.Wave:
                WaveStart();
                break;
            case EnemySpawnerState.Clear:
                ClearStart();
                break;
        }
    }
    private void Update()
    {
        switch (currentState)
        {
            case EnemySpawnerState.Wait:
                WaitUpdate();
                break;
            case EnemySpawnerState.Wave:
                WaveUpdate();
                break;
            case EnemySpawnerState.Clear:
                ClearUpdate();
                break;
        }
    }

    private void Start()
    {
        ChangeState(EnemySpawnerState.Wait);
    }


    // Wait State
    private void WaitStart()
    {
        currentWave = waves[waveIndex];
        waitTime = currentWave.waveWaitTime;
    }

    private void WaitUpdate()
    {
        waitTime -= Time.deltaTime;
        if (waitTime < 0)
        {
            ChangeState(EnemySpawnerState.Wave);
        }
    }

    private void WaitEnd()
    {

    }

    // Wave State
    private void WaveStart()
    {

    }

    private void WaveUpdate()
    {
        bool clearWave = true;  // 모든 적이 스폰하여 웨이브가 끝났다면 true가 유지된다

        foreach (SpawnData spawnData in currentWave.spawnDatas)
        {
            if (spawnData.count <= 0)
            {
                continue;
            }
            clearWave = false;

            int2 spawnPos = SpawnPoint[spawnData.point];
            if (StageManager.instance.units[spawnPos.x, spawnPos.y] == null)
            {
                // 유닛 생성
                SpawnEnemy(spawnPos, spawnData.code);
                break;
            }
        }

        if (clearWave)
        {
            if (waves.Count <= ++waveIndex)
            {
                ChangeState(EnemySpawnerState.Clear);
            }
            else
            {
                ChangeState(EnemySpawnerState.Wait);
            }
        }
    }

    private void SpawnEnemy(int2 pos, int code)
    {
        Vector3 worldPos = StageManager.instance.GridToWorldPosition(pos);
        Instantiate(enemies[code]);
    }

    private void WaveEnd()
    {

    }

    private void ClearStart()
    {
        Debug.Log("스폰 끝");
    }

    private void ClearUpdate()
    {

    }

    private void ClearEnd()
    {

    }

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
