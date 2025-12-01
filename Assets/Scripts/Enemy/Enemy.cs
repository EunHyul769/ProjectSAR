using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("적 유닛 설정")]
    [SerializeField] private EnemyData enemyData;
    private GameObject originalPrefab;

    private Rigidbody2D rb;
    private Transform playerTransform;

    private float currentHealth;
    private float enemyDamage;
    private bool isActive = false;

    public event Action<GameObject, GameObject> OnDeathEvent;

    [Header("장애물 설정")]
    public LayerMask obstacleLayer;

    private EnemyObjectPoolManager objectPoolManager;
    private DifficultyScaler difficultyScaler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        objectPoolManager = EnemyObjectPoolManager.Instance;
        if(objectPoolManager == null)
        {
            Debug.LogError("EnemyObjectPoolManager이 없음. 드랍 기능 사용 불가");
        }

        difficultyScaler = DifficultyScaler.Instance;
        if(difficultyScaler == null)
        {
            Debug.LogError("DifficultyScaler가 씬에 없음.");
        }
    }

    public void Initialize(EnemyData data, Transform targetPlayer, GameObject prefabOrigin, float healthMult, float damageMult)
    {
        enemyData = data; // 전달받은 MonsterData로 설정
        originalPrefab = prefabOrigin; // 원본 프리팹 저장 (풀 반환용)
        playerTransform = targetPlayer;

        // 두 가지 계수 (PhaseData 계수 * DifficultyScaler 계수)를 모두 적용
        float finalHealthMultiplier = healthMult * (difficultyScaler != null ? difficultyScaler.CurrentHealthMultiplier : 1.0f);
        float finalDamageMultiplier = damageMult * (difficultyScaler != null ? difficultyScaler.CurrentDamageMultiplier : 1.0f);

        currentHealth = Mathf.RoundToInt(enemyData.maxHealth * finalHealthMultiplier);
        enemyDamage = Mathf.RoundToInt(enemyData.attackDamage * finalDamageMultiplier);

        isActive = true; // 활성화 상태로 전환
        gameObject.SetActive(true);

        Debug.Log($"--- {enemyData.enemyName} 스탯 초기화 ---");
        Debug.Log($"- 초기 Max 체력: {enemyData.maxHealth} * Phase계수: {healthMult:F2} * 시간계수: {finalHealthMultiplier / healthMult:F2} = 최종 체력: {currentHealth}");
        Debug.Log($"- 초기 공격력: {enemyData.attackDamage} * Phase계수: {damageMult:F2} * 시간계수: {finalDamageMultiplier / damageMult:F2} = 최종 공격력: {enemyDamage}");

    }

    private void FixedUpdate()
    {
        if (!isActive || playerTransform == null || enemyData == null)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > enemyData.attackRange)
        {
            rb.velocity = directionToPlayer * enemyData.moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            Attack();
        }
    }

    private void Attack()
    {
        switch (enemyData.enemyType)
        {
            case EnemyType.Melee:
                break;
            case EnemyType.Range:
                // 플레이어의 체력 감소 로직에 enemyDamage 대입
                break;
            case EnemyType.Boss:
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isActive) return;

        currentHealth -= damage;
        Debug.Log($"{enemyData.enemyName}이 {damage} 데미지 입음. 남은 체력: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 사망 처리 메서드
    private void Die()
    {
        Debug.Log($"{enemyData.enemyName} 사망.");
        
        if(enemyData.expOrbPrefab != null && objectPoolManager != null)
        {
            GameObject expOrb = objectPoolManager.SpawnFromPool(enemyData.expOrbPrefab);
            if (expOrb != null)
            {
                expOrb.transform.position = transform.position;
                Debug.Log("경험치 오브젝트 드랍");
            }
        }
        else if (enemyData.expOrbPrefab == null)
        {
            Debug.LogWarning("EnemyData에 경험치 오브젝트 프리팹이 할당되지 않음");
        }

        if (enemyData.dropItemPrefab != null && objectPoolManager != null)
        {
            if (UnityEngine.Random.value <= enemyData.dropChance)
            {
                GameObject droppedItem = objectPoolManager.SpawnFromPool(enemyData.dropItemPrefab);

                if(droppedItem != null)
                {
                    droppedItem.transform.position = transform.position;
                }
            }
        }
        OnDeathEvent?.Invoke(this.gameObject, originalPrefab); // 사망 이벤트 발생 시 자신과 원본 프리팹 전달
        isActive = false; // 비활성화 상태
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //player.ChangeHealth(enemyData.enemyDamage);
            Debug.Log("플레이어와 접촉");
        }
    }

}
