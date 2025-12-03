using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayer;
    private RangeWeaponHandler rangeWeaponHandler;

    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestroy = true; //삭제시의 이펙트 출력 여부

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) return;

        currentDuration += Time.deltaTime;

        if (currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
            return;
        }
        _rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnEnable()
    {
        Invoke("Despawn", 1f);
    }

    private void Despawn()
    {
        DestroyProjectile(transform.position, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionLayer.value == (collisionLayer.value | (1 << collision.gameObject.layer))) //벽면 충돌체랑 같은 레이어인지 or 연산으로 확인
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestroy);
        } //collision.ClosestPoint(transform.position) -> 충돌체랑 가장 가까운 부분
        else if (rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer))) //타겟 충돌체와 같은지 확인 (웨폰 핸들러에서 지정)
        {
            IDamagable damagableObject = collision.gameObject.GetComponent<IDamagable>();

            if (damagableObject != null)
            {
                damagableObject.TakeDamage(rangeWeaponHandler.power);
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler)
    {
        rangeWeaponHandler = weaponHandler;

        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        transform.right = this.direction;

        if (direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        // TODO: createFx가 true일 때 파괴 이펙트(Particle)를 생성하는 코드를 여기에 추가할 수 있습니다.
        Destroy(this.gameObject);
    }
}
