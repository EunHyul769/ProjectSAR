using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemyEntry
{
    public EnemyData enemyData;
    public int spawnCount;
}

[CreateAssetMenu(fileName = "Wave", menuName = "WaveData")]
public class WaveData : ScriptableObject
{
    [Header("웨이브 기본 정보")]
    public int waveID;
    public string waveName;
    public bool isBossWave;

    [Header("몬스터 스폰 설정")]
    public List<WaveEnemyEntry> enemiesToSpawn;
    public float spawnInterval; // 적 개체 간 스폰 딜레이
    public float initialDelay;  // 웨이브 시작 전 초기 딜레이
    public float waveDuration;  // 웨이브 진행 시간 (해당 시간이 지나면 다음 웨이브로 자동 진행)
}
