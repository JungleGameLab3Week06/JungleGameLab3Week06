using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour, IStatus
{
    [Header("컴포넌트")]
    PlayerSkill _playerSkill;

    [Header("스테이터스")]
    public int Health => _hp;
    [SerializeField] int _hp = 100;
    [SerializeField] int _maxHp = 100;

    [Header("마법")]
    public Elemental PlayerElemental => _playerElemental;
    public bool HasInputThisBeat { get { return _hasInputThisBeat; } set { _hasInputThisBeat = value; } } // 현재 비트에서 입력 여부
    Elemental _playerElemental;
    bool _hasInputThisBeat = false;     // 현재 비트에서 입력 여부
    Friend _friend;                     // 친구

    [Header("비트 판정")]
    bool _isPerfect;

    void Start()
    {
        InputManager.Instance.selectElementalAction += SelectElemental; // 원소 선택 이벤트 등록
        _friend = FindAnyObjectByType<Friend>();
        _playerSkill = GetComponent<PlayerSkill>();
    }

    // 플레이어 마법 시전
    public void SelectElemental(Elemental elemental)
    {
        if (_hasInputThisBeat)
        {
            Manager.UI.showJudgeTextAction(false);
            Debug.Log("[즉시 Miss] 큰 박자 내 연속 입력");
            return;
        }

        _playerElemental = elemental;
        _isPerfect = RhythmManager.Instance.IsJudging;
        Manager.UI.showJudgeTextAction(_isPerfect);
        if (_isPerfect)
        {
            Attack();
        }
        _hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }

    // 마법 공격 (친구가 시전한 마법과 조합)
    public void Attack()
    {
        ElementalEffect interaction = _playerSkill.GetInteraction(_playerElemental, _friend.RealElemental);

        if (interaction != ElementalEffect.None)
        {
            _playerSkill.ApplyInteraction(interaction);
            // Debug.Log($"반응 발생: {interaction}");
            // Enemy firstEnemy = GameManager.Instance._currentEnemyList[0];
            //_friend.UpdatePreviewElemental();
            _friend.PrepareElemental(); // 친구 마법 예고 다시
        }
        else
        {
            //Debug.Log($"조합 실패: 플레이어({_playerElemental}) + 친구(visual: {_friend.VisualElemental} real: {_friend.RealElemental})는 잘못된 마법(기본 데미지 적용)");
            GameManager.Instance.CurrentEnemy.TakeDamage(10);
        }
    }

    // 플레이어 데미지 피해 
    public void TakeDamage(int amount)
    {
        _hp = Mathf.Clamp(_hp - amount, 0, _maxHp);
        //Debug.Log($"플레이어 HP: {_hp}");
        if (_hp <= 0)
            Die();
    }
    
    public void Die()
    {   
        gameObject.SetActive(false); // 플레이어 비활성화
        /* 
           사망 애니메이션 재생
           사망 이펙트 재생
           사망 사운드 재생
           UI 업데이트
        */
        Debug.Log("플레이어 사망 이벤트 발생");
    }
}