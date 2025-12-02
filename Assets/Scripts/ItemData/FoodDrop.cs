using UnityEngine;

public class FoodDrop : MonoBehaviour
{
    public FoodItem data;       // ScriptableObject 데이터 연결
    public float lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var stat = other.GetComponent<StatHandler>();
        if (stat != null)
        {
            stat.Health += data.healAmount;  // HP 회복
            Debug.Log($"[FoodDrop] {data.itemName} 먹음 → HP: {stat.Health} (+{data.healAmount})");
            Destroy(gameObject);             // 먹으면 제거
        }
    }
}