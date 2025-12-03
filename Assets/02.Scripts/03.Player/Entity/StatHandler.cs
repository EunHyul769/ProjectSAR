using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    [Range(1, 100)][SerializeField] private int health = 10;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, 100);
    }

    [Range(1f, 20f)][SerializeField] private float speed = 3;
    public float Speed
    {
        get => speed;
        set => speed = Mathf.Clamp(value, 0, 20);
    }

    [SerializeField] private CharacterData characterData; // 캐릭터 데이터 연결

    // 실제 게임에서 변동되는 스탯들
    public int MaxHealth { get; set; }
    public float Attack { get; set; }
    public float AttackSpeed { get; set; } // 공격 딜레이 (낮을수록 빠름) or 초당 공격 횟수
    public float GoldBonusMultiplier { get; set; } = 1.0f; // 1.0 = 100%

    // 패시브 효과용 플래그
    public bool HasExplosiveProjectile { get; set; } = false;
    public float ExplosiveChance { get; set; } = 0f;

    private void Awake()
    {
        if (characterData != null)
        {
            InitializeStats();
        }
    }

    private void InitializeStats()
    {
        MaxHealth = characterData.baseHealth;
        Speed = characterData.baseSpeed;
        Attack = characterData.baseAttack;
        AttackSpeed = characterData.baseAttackSpeed;
    }
}
