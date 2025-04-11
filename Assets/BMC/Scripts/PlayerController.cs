using UnityEngine;
using static Define;

public class PlayerController : MonoBehaviour
{
    Elemental _playerElemental;
    public Elemental PlayerElemental => _playerElemental;

    bool _hasInputThisBeat = false; // 현재 비트에서 입력 여부

    public bool HasInputThisBeat { get { return _hasInputThisBeat; } set { _hasInputThisBeat = value; } } // 현재 비트에서 입력 여부

    void Start()
    {
        InputManager.Instance.selectElementalAction += SelectElemental; // 태그 선택 이벤트 등록
    }

    // 플레이어 마법 시전
    public void SelectElemental(Elemental tag)
    {
        double now = AudioSettings.dspTime;
        if (!GameManager.Instance.IsJudging)
        {
            Debug.Log("[무시됨] 현재 큰 박자 아님");
            return;
        }

        if (_hasInputThisBeat)
        {
            JudgeNotifier.Instance.ShowJudge("Miss");
            Debug.Log("[즉시 Miss] 큰 박자 내 연속 입력");
            return;
        }

        _playerElemental = tag;
        double beatTime = RhythmManager.Instance.LastBeatTime;
        double diff = System.Math.Abs(now - beatTime);
        string judge = GameManager.Instance.GetTimingJudgement(diff);

        JudgeNotifier.Instance.ShowJudge(judge);
        if (judge != "Miss")
        {
            GameManager.Instance.Friend.CastElemental();
        }
        else
        {
            
            Debug.Log($"[즉시 Miss] now: {now:F4} (성공 구간: {GameManager.Instance.JudgeWindowStart:F4}~{GameManager.Instance.JudgeWindowEnd:F4})");
        }
        _hasInputThisBeat = true; // 큰 박자 내 첫 입력 처리 완료
    }
}