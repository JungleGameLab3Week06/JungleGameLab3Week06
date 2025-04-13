using UnityEngine;
using static Define;

public class Enemy : MonoBehaviour, IStatus
{
    Animator _anim;
    EnemyHearts _hearts; // 적 체력 UI

    public EnemyType EnemyType => _enemyType;
    public EnemyState EnemyState => _enemyState;
    [SerializeField] EnemyType _enemyType;                           // 적 타입
    [SerializeField] EnemyState _enemyState = EnemyState.None;       // 적 상태

    [SerializeField] int _moveType = 0;
    bool _isMoved = false;

    [Header("스테이터스")]
    public int Health => _hp;
    [SerializeField] int _hp = 3;
    public int SteelHealth => _steelHp;
    [SerializeField] int _steelHp = 0; // 강철 HP
    float _moveSpeed = 2f;                      // 비트당 이동 거리

    [SerializeField] int _sturnCoolCount = 0;   // 기절 쿨 카운트

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _hearts = GetComponentInChildren<EnemyHearts>();

    }

    // 적 이동
    public void Move()
    {
        Wriggle();
        if (_enemyState == EnemyState.Shock && _sturnCoolCount > 0) // 스턴 상태일 때는 이동하지 않음
        {
            _sturnCoolCount--;
            if(_sturnCoolCount == 0) // 스턴 상태 해제
                _enemyState = EnemyState.None;

            return;
        }
        switch(_moveType)
        {
            //일반몹
            case 0:
                transform.position += (Vector3)(Vector2.left * _moveSpeed); // 왼쪽으로 이동
                break;
            //빠른몹
            case 1:
                transform.position += (Vector3)(Vector2.left * _moveSpeed * 2); // 왼쪽으로 두배 이동
                break;
            //느린몹
            case 2:
                if (!_isMoved)
                {
                    transform.position += (Vector3)(Vector2.left * _moveSpeed); // 두턴마다 왼쪽으로 이동
                    _isMoved = true;
                }
                else
                {
                    _isMoved = false;
                }
                break;
            default:
                Debug.LogError("이동 타입이 잘못되었습니다.");
                break;
        }
    }
    
    // 적 상태 변경
    public void ApplyState(EnemyState newState)
    {
        _sturnCoolCount = 2;
        _enemyState = newState;
        Debug.Log($"적 상태: {_enemyState}");
    }

    // 피해 입기
    public void TakeDamage(int amount)
    {
        int steelDamage = amount - _hp; // 강철 HP에 입힐 데미지;
        if (_hp > 0)
        {
            _hp = Mathf.Clamp(_hp - amount, 0, 100);
        }
        if(_steelHp > 0 && amount >= 2 && steelDamage > 0)
        {
            _steelHp = Mathf.Clamp(_steelHp - steelDamage, 0, 100);
            _hp = 0;
        }
        _hearts.UpdateHearts(_hp + _steelHp); // 체력 UI 업데이트
        if (_hp <= 0 && _steelHp <= 0)
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

    //꿈틀거리기
    public void Wriggle()
    {
        _anim.SetTrigger("WriggleTrigger");
    }
}