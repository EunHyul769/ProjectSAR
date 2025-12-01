using UnityEngine;

public enum EnemyType
{
    Melee,
    Range,
    Boss,
}

[CreateAssetMenu(fileName = "Enemy", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("적 유닛 정보")]
    public string enemyName;
    public EnemyType enemyType;
    public GameObject enemyPrefab;

    [Header("적 유닛 스탯")]
    public float moveSpeed;
    public float attackRange;
    public float attackDamage;
    public float maxHealth;
    //public GameObject dropExpPrefab;
}
