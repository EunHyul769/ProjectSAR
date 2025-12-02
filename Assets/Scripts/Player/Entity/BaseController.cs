using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }
    
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection { get { return lookDirection; } }

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;

    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;

    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 3f;   // 대쉬 이동 거리
    [SerializeField] private float dashDuration = 0.2f; // 대쉬하는데 걸리는 시간
    [SerializeField] private float dashCooldown = 1.0f; // 대쉬 쿨타임

    private bool isDashing = false;
    private float lastDashTime = -10f;

    public bool IsInvincible { get; private set; } = false;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();

        if (WeaponPrefab != null)
        {
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot);
        }
        else
        {
            weaponHandler = GetComponentInChildren<WeaponHandler>();
        }
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        HandleAttackDelay();
    }

    protected virtual void FixedUpdate()
    {
        if (isDashing) return; //대쉬 중일 때는 일반 이동 로직을 수행하지 않음

        Movement(movementDirection);
        if (knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    protected virtual void HandleAction()
    {

    }

    protected virtual void Movement(Vector2  direction)
    {
        direction = direction * statHandler.Speed;
        if (knockbackDuration > 0.0f)
        {
            direction *= 0.2f; //이동 방향의 힘 줄이기
            direction += knockback; //넉백의 힘 적용
        }
        _rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    public void AttemptDash()
    {
        // 쿨타임 체크 및 이미 대쉬 중인지 확인
        if (Time.time < lastDashTime + dashCooldown || isDashing) return;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        IsInvincible = true; // 무적 시작
        lastDashTime = Time.time;

        // 대쉬 방향 결정: 이동 중이면 이동 방향, 정지 중이면 바라보는 방향
        Vector2 dashDir = movementDirection.normalized;
        if (dashDir == Vector2.zero) dashDir = lookDirection.normalized;

        // 거리 = 속력 * 시간  =>  속력 = 거리 / 시간
        float dashSpeed = dashDistance / dashDuration;

        // 리지드바디 속도 강제 적용
        _rigidbody.velocity = dashDir * dashSpeed;

        // 대쉬 지속 시간 동안 대기
        yield return new WaitForSeconds(dashDuration);

        // 대쉬 종료
        _rigidbody.velocity = Vector2.zero;
        isDashing = false;
        IsInvincible = false; // 무적 종료
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        characterRenderer.flipX = isLeft;

        if (weaponPivot != null) 
        {
            weaponPivot.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
        weaponHandler?.Rotate(isLeft);
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        if (IsInvincible) return; //무적 상태(대쉬 중)라면 넉백 무시

        knockbackDuration = duration; //넉백의 시간
        knockback = -(other.position - transform.position).normalized * power; //넉백의 방향
    }

    private void HandleAttackDelay()
    {
        if (weaponHandler == null)
        {
            return;
        }
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay)
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (lookDirection != Vector2.zero)
            weaponHandler?.Attack();
    }
}
