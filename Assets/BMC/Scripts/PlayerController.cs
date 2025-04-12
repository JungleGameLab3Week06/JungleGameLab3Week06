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
    bool _hasInputThisBeat = false; // 현재 비트에서 입력 여부
    Friend _friend;                 // 친구

    [Header("비트 판정")]
    double dspTime;                 // 오디오 시스템에서 처리된 실제 오디오 샘플 수에 기반하여 반환되는 초 단위 시간 (실제 시간에 가까움)
    double lastBeatTime;
    double deltaTime;
    bool isPerfect;

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

        // 시간 계산
        dspTime = AudioSettings.dspTime; 
        lastBeatTime = RhythmManager.Instance.LastBeatTime;
        deltaTime = System.Math.Abs(dspTime - lastBeatTime);
        isPerfect = RhythmManager.Instance.CheckTimingJudgement(deltaTime);

        Manager.UI.showJudgeTextAction(isPerfect);
        if (isPerfect)
        {
            Attack();
        }
        else
        {
            Debug.Log($"[즉시 Miss] now: {dspTime:F4} (성공 구간: {RhythmManager.Instance.JudgeWindowStart:F4}~{RhythmManager.Instance.JudgeWindowEnd:F4})");
        }
        _hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }

    // 마법 공격 (친구가 시전한 마법과 조합)
    public void Attack()
    {
        ElementalEffect interaction = _playerSkill.GetInteraction(_playerElemental, _friend.RealElemental); 

        if (interaction != null)
        {
            _playerSkill.ApplyInteraction(interaction);
            //Debug.Log($"반응 발생: {interaction}");
            Enemy firstEnemy = GameManager.Instance.enemyList[0];
            _friend.UpdatePreviewElemental();
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