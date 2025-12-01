using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void Initialize(EnemyData data, Transform targetPlayer, GameObject prefabOrigin, float healthMult, float damageMult)
    {
        enemyData = data; // 전달받은 MonsterData로 설정
        originalPrefab = prefabOrigin; // 원본 프리팹 저장 (풀 반환용)
        playerTransform = targetPlayer;

        currentHealth = Mathf.RoundToInt(enemyData.maxHealth * healthMult);
        enemyDamage = Mathf.RoundToInt(enemyData.attackDamage * damageMult);

        isActive = true; // 활성화 상태로 전환
        gameObject.SetActive(true);

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
        // 사망 애니메이션, 경험치 드랍 처리

        OnDeathEvent?.Invoke(this.gameObject, originalPrefab); // 사망 이벤트 발생 시 자신과 원본 프리팹 전달
        isActive = false; // 비활성화 상태
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            //TakeDamage();
        }

        if (other.CompareTag("Player"))
        {
            //player.ChangeHealth(enemyData.enemyDamage);
        }
    }

}
