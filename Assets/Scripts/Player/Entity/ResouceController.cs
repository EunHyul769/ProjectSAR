using UnityEngine;

public class ResouceController : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float healthChangeDelay = .5f;
    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth {  get; private set; }
    public float MaxHealth => statHandler.Health;
    
    [SerializeField] private GameObject expOrbPrefab;

    private void Awake()
    {
        baseController = GetComponent<BaseController>();
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd(); //무적 해제
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (baseController != null && baseController.IsInvincible)
        {
            // 무적이면 데미지 무시하고 리턴
            return false;
        }

        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false; //데미지를 받지 않음
        }

        timeSinceLastChange = 0f;
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;
        Debug.Log("체력 " + CurrentHealth);
        if (change < 0)
        {
            animationHandler.Damage();
        }
        // 체력 UI 갱신 필요한 지점 1.

        if (CurrentHealth <= 0f)
        {
            Death(); //플레이어 사망
        }

        return true;
    }

    private void Death()
    {
        Debug.Log("사망");
    }

    public void FullRecovery()
    {
        CurrentHealth = statHandler.Health; // 현재 체력을 최대 체력으로

        Debug.Log("레벨업 보너스! 체력 완전 회복: " + CurrentHealth);

        // 체력 UI 갱신 필요한 지점 2.
    }

}
