using System;
using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour, IStatus
{
    public int Health => hp;
    [SerializeField] int hp = 100;
    Elemental _playerElemental;
    public Elemental PlayerElemental => _playerElemental;

    bool _hasInputThisBeat = false; // 현재 비트에서 입력 여부
    public event Action OnDie;

    public bool HasInputThisBeat { get { return _hasInputThisBeat; } set { _hasInputThisBeat = value; } } // 현재 비트에서 입력 여부

    void Start()
    {
        InputManager.Instance.selectElementalAction += SelectElemental; // 원소 선택 이벤트 등록
    }

    // 플레이어 마법 시전
    public void SelectElemental(Elemental tag)
    {
        double now = AudioSettings.dspTime;
        if (!RhythmManager.Instance.IsJudging)
        {
            Debug.Log("[무시됨] 현재 큰 박자 아님");
            return;
        }

        if (_hasInputThisBeat)
        {
            Manager.UI.showJudgeTextAction(false);
            Debug.Log("[즉시 Miss] 큰 박자 내 연속 입력");
            return;
        }

        _playerElemental = tag;
        double beatTime = RhythmManager.Instance.LastBeatTime;
        double diff = System.Math.Abs(now - beatTime);
        bool isPerfect = RhythmManager.Instance.CheckTimingJudgement(diff);

        Manager.UI.showJudgeTextAction(isPerfect);
        if (!isPerfect)
        {
            //GameManager.Instance.Friend.CastElemental();
        }
        else
        {
            Debug.Log($"[즉시 Miss] now: {now:F4} (성공 구간: {RhythmManager.Instance.JudgeWindowStart:F4}~{RhythmManager.Instance.JudgeWindowEnd:F4})");
        }
        _hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }
    public void TakeDamage(int amount) //플레이어 데미지 피해 
    {
        hp = Mathf.Clamp(hp - amount, 0, 100);
        Debug.Log($"적 HP: {hp}");
        if (hp <= 0)
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