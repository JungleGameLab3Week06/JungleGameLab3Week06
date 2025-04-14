using UnityEngine;
using static Define;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour, IStatus
{
    Animator _anim;
    EnemyHearts _hearts; // 적 체력 UI

    public EnemyType EnemyType => _enemyType;
    public EnemyState EnemyState => _enemyState;
    [SerializeField] EnemyType _enemyType;                           // 적 타입
    [SerializeField] EnemyState _enemyState = EnemyState.None;       // 적 상태

    [SerializeField] int _moveType = 0;
    public bool IsMoving => _isMoving; // 적 이동 타입
    bool _isMoving = true;
    bool _halfMove = false; // 반 박자 이동 여부
    [SerializeField] float _positionY1;
    [SerializeField] float _positionY2;

    [Header("스테이터스")]
    public int Health => _hp;
    [SerializeField] int _hp = 3;
    public int SteelHealth => _steelHp;
    [SerializeField] int _steelHp = 0; // 강철 HP
    float _moveSpeed = 2f;                      // 비트당 이동 거리
    [SerializeField] int _sturnCoolCount = 0;   // 기절 쿨 카운트
    Animator _animator;                         // 적 애니메이터

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _hearts = GetComponentInChildren<EnemyHearts>();
        _positionY1 = transform.position.y;
        _positionY2 = transform.position.y - 2f;
        _animator = transform.Find("Visual").GetComponent<Animator>();
    }

    // 적 이동
    public void Move()
    {
        Wriggle();
        // 스턴 판정
        if (_enemyState == EnemyState.Shock && _sturnCoolCount > 0) // 스턴 상태일 때는 이동하지 않음
        {
            _isMoving = false; // 이동 중지
            _sturnCoolCount--;
            if(_sturnCoolCount == 0) // 스턴 상태 해제
            {
                _enemyState = EnemyState.None;
                _isMoving = true;
            }
            return;
        }

        // 벽 체크: Wall 스크립트가 포함된 오브젝트가 x-2에 있으면 공격
        foreach (Wall obj in FindObjectsByType<Wall>(FindObjectsSortMode.None))
        {
            if (Mathf.Approximately(obj.transform.position.x, transform.position.x - 2))
            {
                Attack(obj.gameObject);
                _isMoving = false; // 이동 중지
                return;
            }
        }
        // 플레이어 체크
        if (Mathf.Approximately(PlayerController.Instance.transform.position.x, transform.position.x - 2))
        {
            Attack(PlayerController.Instance.gameObject);
            _isMoving = false; // 이동 중지
            return;
        }

        // 앞 열의 적이 2명이상이면 이동하지 않음
        switch (CheckFrontLine())
        {
            case 0: // 적이 2명 이상이면 이동하지 않음
                _isMoving = false; // 이동 중지
                return;
            case 1: // 윗길로 이동
                _isMoving = true; // 이동 재개
                transform.position = new Vector3(transform.position.x, _positionY1, transform.position.z);
                break;
            case 2: // 아랫길로 이동
                _isMoving = true; // 이동 재개
                transform.position = new Vector3(transform.position.x, _positionY2, transform.position.z);
                break;
        }

        switch (_moveType)
        {
            //일반몹
            case 0:
                transform.position += (Vector3)(Vector2.left * _moveSpeed); // 왼쪽으로 이동
                break;
            //빠른몹
            case 1:
                transform.position += (Vector3)(Vector2.left * _moveSpeed); // 왼쪽으로 이동
                if(!_halfMove)
                {
                    StartCoroutine(MoveHalfBeat()); // 반 박자 이동
                }
                _halfMove = false; // 반 박자 이동
                break;
            //느린몹
            case 2:
                if (_isMoving)
                {
                    transform.position += (Vector3)(Vector2.left * _moveSpeed); // 두턴마다 왼쪽으로 이동
                    _isMoving = false;
                }
                else
                {
                    _isMoving = true;
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
        
        if (_hp > 0 || _steelHp > 0)
        {
            if (_hp > 0)
            {
                _hp = Mathf.Clamp(_hp - amount, 0, 100);
            }
            if (_steelHp > 0 && amount >= 2 && steelDamage > 0)
            {
                _steelHp = Mathf.Clamp(_steelHp - steelDamage, 0, 100);
                _hp = 0;
            }
            _hearts.UpdateHearts(_hp + _steelHp); // 체력 UI 업데이트
            if (_hp <= 0 && _steelHp <= 0)
                Die();
        }
    }

    // 공격
    void Attack(GameObject target)
    {
        // 공격 애니메이션 재생
        _animator.SetTrigger("AttackTrigger");
        // 적이 벽에 닿으면 벽 파괴
        if (target.TryGetComponent<Wall>(out Wall wall))
        {
            Destroy(target);
        }
        if (target.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.TakeDamage(100); // 플레이어에게 데미지 입히기
        }

        if(_hearts != null)
            _hearts.UpdateHearts(_hp + _steelHp); // 체력 UI 업데이트

        if (_hp <= 0 && _steelHp <= 0)
            Die();
    }

    // 반 박자 이동
    IEnumerator MoveHalfBeat()
    {
        yield return new WaitForSeconds((float)RhythmManager.Instance.BeatInterval / 2);
        _halfMove = true;
        Move();
    }

    // 이동 전 앞라인 체크(정지0, 윗길로 이동1, 아랫길로 이동2)
    int CheckFrontLine()
    {
        List<Enemy> enemies = GameManager.Instance.CurrentEnemyList;
        int count = 0;
        int posY;
        //기본 직진
        if (transform.position.y == _positionY1)
            posY = 1;
        else
            posY = 2;
        foreach (Enemy e in enemies)
        {
            if (Mathf.Approximately(e.transform.position.x, transform.position.x - 2) && (!e.IsMoving || _halfMove))
            {
                count++;
                if(count == 1)
                    posY = e.transform.position.y == _positionY1 ? 2 : 1; // 적의 y좌표에 따라 이동할 y좌표 결정
                else if (count == 2)
                    return 0; // 적이 2명 이상이면 이동하지 않음
            }
        }
        return posY;
    }

    // 사망
    public void Die()
    {
        /*
        사망 
        애니메이션
        효과음 
         */
        _animator.SetTrigger("DeathTrigger");

        GameManager.Instance.CurrentEnemyList.Remove(this);
        Destroy(gameObject);
    }

    //꿈틀거리기
    public void Wriggle()
    {
        _anim.SetTrigger("WriggleTrigger");
    }
}