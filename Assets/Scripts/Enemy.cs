using UnityEngine;
using static Define;

public class Enemy : MonoBehaviour, IStatus
{
    Animator _anim;

    public EnemyType EnemyType => _enemyType;
    public EnemyState EnemyState => _enemyState;
    [SerializeField] EnemyType _enemyType;                           // 적 타입
    [SerializeField] EnemyState _enemyState = EnemyState.None;       // 적 상태

    [Header("스테이터스")]
    public int Health => _hp;
    [SerializeField] int _hp = 100;
    float _moveSpeed = 2f;               // 비트당 이동 거리

    [Header("UI")]
    [SerializeField] SpriteRenderer _friendCastElementalSpriteRenderer; // 친구가 시전할 마법 속성 스프라이트 렌더러

    void Awake()
    {
        _friendCastElementalSpriteRenderer = transform.GetChild(1).GetComponentInChildren<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // 동료가 적 머리 위에 시전할 마법 표시
    public void ShowFriendElemental(Elemental elemental, Sprite[] elementalSprites)
    {
        int elementalIndex = (int)elemental;
        if (0 <= elementalIndex && elementalIndex < elementalSprites.Length)
            _friendCastElementalSpriteRenderer.sprite = elementalSprites[elementalIndex];
    }

    // 적 이동
    public void Move()
    {
        Wriggle();
        if (_enemyState == EnemyState.Shock) // 스턴 상태일 때는 이동하지 않음
            return;
        transform.position += (Vector3)(Vector2.left * _moveSpeed); // 왼쪽으로 이동
    }
    
    // 적 상태 변경
    public void ApplyState(EnemyState newState)
    {
        _enemyState = newState;
        Debug.Log($"적 상태: {_enemyState}");
    }

    // 피해 입기
    public void TakeDamage(int amount)
    {
        _hp = Mathf.Clamp(_hp - amount, 0, 100);
        //Debug.Log($"적 HP: {_hp}");
        if (_hp <= 0)
            Die();
    }

    // 사망
    public void Die()
    {
        /*
        사망 
        애니메이션
        효과음 
         */
        Destroy(gameObject);
    }

    // 꿈틀거리기
    public void Wriggle()
    {
        _anim.SetTrigger("WriggleTrigger");
    }
}