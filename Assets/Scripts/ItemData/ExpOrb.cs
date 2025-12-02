using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int amount = 10; //경험치량
   
    public float moveSpeed = 5f; //경험치가 딸려오는 속도
    public float detectRange = 3f;  //경험치가 빨려오기 시작하는 거리
    
    private Transform player;
    private SpriteRenderer sr;

    private float pulseSpeed = 3f;  // 반짝임 속도
    private float pulseAmount = 0.2f; // 반짝임 크기

    private Vector3 originalScale;
    
    private EnemyObjectPoolManager poolManager;
    private GameObject originalPrefab;

    
    // 풀에서 꺼낼 때 초기화 
    public void OnSpawned(EnemyObjectPoolManager pm, GameObject prefab)
    {
        poolManager = pm;
        originalPrefab = prefab;

        if (player == null)
            player = GameObject.FindWithTag("Player").transform;

        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
            originalScale = transform.localScale;
        }
        transform.localScale = originalScale;

        // 투명도 리셋
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;
    }
    
    private void Update()
    {
        if (player == null) return;
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            //플레이어가 detectRange 안에 들어와야 이동 시작
            if (distance <= detectRange)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
            }

            // 경험치 흡수 거리
            if (distance < 0.5f)
            {
                var receiver = player.GetComponent<IExpReceiver>();
                if (receiver != null)
                    receiver.OnExpPickup(amount);

                ReturnToPool();
                return;
            }
            // 반짝임(Pulse) ---
            float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            transform.localScale = originalScale * scale;
            
            // 알파로 반짝임 추가
            Color c = sr.color;
            c.a = 0.5f + Mathf.Sin(Time.time * pulseSpeed) * 0.5f; // 0~1 범위로 투명도 변화
            sr.color = c;
        }
    }
    private void ReturnToPool()
    {
        if (poolManager != null && originalPrefab != null)
        {
            poolManager.ReturnToPool(gameObject, originalPrefab);
        }
        else
        {
            Destroy(gameObject); // 안전장치
        }
    }
}
