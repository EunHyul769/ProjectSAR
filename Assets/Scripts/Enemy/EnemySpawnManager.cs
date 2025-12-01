using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [Header("웨이브 설정")]
    public List<WaveData> waves;

    [Header("플레이어 기반 스폰 설정")]
    [SerializeField] private Transform playerTransform; // 플레이어의 transform
    [SerializeField] private float minSpawnDistance;
    [SerializeField] private float maxSpawnDistance;


    private int currentWaveIndex = 0;
    private EnemyObjectPoolManager enemyObjectPoolManager;


    private void Start()
    {
        if (waves == null || waves.Count == 0)
        {
            Debug.LogError("WaveData가 없음. WaveData 할당 필요");
            return;
        }

        if (playerTransform == null)
        {
            // GameManager등에서 player를 관리하면 해당 오브젝트를 가져올 것
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player에 Transform이 할당되지 않았고, 'Player' 태그를 가진 오브젝트가 없음");
                enabled = false;
                return;
            }
        }

        enemyObjectPoolManager = EnemyObjectPoolManager.Instance;
        if(enemyObjectPoolManager == null)
        {
            Debug.LogError("씬에 EnemyObjectPoolManager가 없음");
            enabled = false;
            return;
        }

        StartCoroutine(WaveProgressionRoutine());
    }

    IEnumerator WaveProgressionRoutine()
    {
        if (waves.Count > 0)
        {
            yield return new WaitForSeconds(waves[0].initialDelay);
        }

        while (currentWaveIndex < waves.Count)
        {
            WaveData currentWave = waves[currentWaveIndex];
            Debug.Log($"Wave {currentWaveIndex + 1}: {currentWave.waveName} 시작 ({currentWave.waveDuration}초 동안 진행)");

            Coroutine spawnCoroutine = StartCoroutine(SpawnEnemiesForWave(currentWave));

            yield return new WaitForSeconds(currentWave.waveDuration);

            if(spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
            }
            Debug.Log($"Wave {currentWaveIndex + 1} 종료");

            currentWaveIndex++;
        }

        Debug.Log("모든 웨이브 완료");
    }

    IEnumerator SpawnEnemiesForWave(WaveData wave)
    {
        foreach (var enemyEntry in wave.enemiesToSpawn)
        {
            if(enemyEntry.enemyData == null || enemyEntry.enemyData.enemyPrefab == null)
            {
                Debug.Log($"Wave{wave.waveName}에 enemyData 또는 enemyPrefab이 할당되지않음. 건너뜀.");
                continue;
            }

            for(int i = 0; i < enemyEntry.spawnCount; i++)
            {
                SpawnSingleEnemy(enemyEntry.enemyData);
                yield return new WaitForSeconds(wave.spawnInterval);
            }
        }
    }

    void SpawnSingleEnemy(EnemyData enemyData)
    {
        if (playerTransform == null) { Debug.LogError("플레이어 Transform이 없어 몬스터를 스폰할 수 없습니다."); return; }

        Vector2 spawnPosition = GetRandomSpawnPositionAroundPlayer();

        GameObject spawnedEnemyObject = enemyObjectPoolManager.SpawnFromPool(enemyData.enemyPrefab);
        if (spawnedEnemyObject == null) return; // 풀에서 가져오지 못했다면 중단

        spawnedEnemyObject.transform.position = spawnPosition;
        spawnedEnemyObject.transform.rotation = Quaternion.identity;

        Enemy enemyComponent = spawnedEnemyObject.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.Initialize(enemyData, playerTransform, enemyData.enemyPrefab); // 원본 프리팹 전달
            enemyComponent.OnDeathEvent += OnEnemyKilled; // 몬스터 사망 시 호출될 이벤트 연결
        }
        Debug.Log($"{enemyData.enemyName}이(가) 스폰되었습니다. 위치: {spawnPosition}");
    }

    // 플레이어 주변에 랜덤 스폰 위치를 계산하는 함수
    Vector2 GetRandomSpawnPositionAroundPlayer()
    {
        // (이전 코드와 동일)
        Vector2 playerPos = playerTransform.position;
        Vector2 spawnPos = Vector2.zero;

        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(minSpawnDistance, maxSpawnDistance);

        spawnPos.x = playerPos.x + randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        spawnPos.y = playerPos.y + randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        return spawnPos;
    }

    // 이 메서드는 몬스터 사망 시 Enemy 스크립트에서 호출됩니다.
    public void OnEnemyKilled(GameObject deadEnemyObject, GameObject originalPrefab)
    {
        // 몬스터 사망 이벤트 발생 시 해당 몬스터 오브젝트를 풀로 반환
        enemyObjectPoolManager.ReturnToPool(deadEnemyObject, originalPrefab);

        // 중요: 이벤트 구독 해제 (오브젝트가 재활용될 때 중복 구독 방지)
        Enemy enemyComponent = deadEnemyObject.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.OnDeathEvent -= OnEnemyKilled;
        }
    }
}
