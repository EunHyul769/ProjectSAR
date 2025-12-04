using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController Instance { get; private set; } //UI연결문제로 추가

    private StatHandler statHandler;
    [SerializeField] private EquipmentData itemData; //테스트용: 장비 아이템 할당

    private void Awake()
    {
        Instance = this; //UI연결문제로 추가
        statHandler = GetComponent<StatHandler>();
    }

    private void Start()
    {
        // 테스트용: 게임 시작 시 아이템 1개 장착
        if (itemData != null)
        {
            EquipItem(itemData);
        }
    }

    // 아이템 획득 시 호출할 메서드
    public void EquipItem(EquipmentData data)
    {
        Debug.Log($"아이템 습득: {data.itemName} - {data.description}");

        foreach (var modifier in data.modifiers)
        {
            ApplyStat(modifier);
        }
    }

    private void ApplyStat(StatModifier modifier)
    {
        switch (modifier.type)
        {
            case StatType.MaxHealth:
                statHandler.AddMaxHealth((int)modifier.value);
                break;

            case StatType.Attack:
                statHandler.AddAttack(modifier.value);
                break;

            case StatType.Defense:
                statHandler.AddDefense(modifier.value);
                break;

            case StatType.MoveSpeedPercent:
                // 10% 증가는 0.1f로 데이터에 입력됨
                statHandler.AddSpeedPercent(modifier.value);
                break;

            case StatType.AttackSpeedPercent:
                statHandler.AddAttackSpeedPercent(modifier.value);
                break;

            case StatType.CooldownReduction:
                statHandler.AddCooldownReduction(modifier.value);
                break;
        }
    }
}
