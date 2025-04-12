using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Define;

public class Friend : MonoBehaviour
{
    public Elemental VisualElemental => _visualElemental;
    public Elemental RealElemental => _realElemental;
    [SerializeField] Elemental _visualElemental = Elemental.None;   // 눈으로 보이는 속성
    [SerializeField] Elemental _realElemental = Elemental.None;     // 실제 속성
    [SerializeField] float _mistakeProbability = 0.75f;             // 동료가 실수할 확률
    UI_FriendCastVisualCanvas _friendCastVisualCanvas;              // 동료 마법 예고 UI
    bool _isLying = false;                                          // 동료가 속이는 중인지 여부

    void Start()
    {
        _friendCastVisualCanvas = GetComponentInChildren<UI_FriendCastVisualCanvas>();
    }

    // 마법 준비
    public void PrepareElemental()
    {
        // 무작위 속성 선택해서 예고
        _visualElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        TryMistake();

        // 동료 마법 예고 UI에 원소 표시
        _friendCastVisualCanvas.SetElementalImage(_isLying, _visualElemental);

        Enemy firstEnemy = GameManager.Instance._currentEnemyList[0];
        Debug.Log($"친구 예고: 비주얼({_visualElemental}), 실제({_realElemental})");
    }

    // 실수 시도
    void TryMistake()
    {
        Enemy firstEnemy = GameManager.Instance._currentEnemyList[0];
        if (firstEnemy.EnemyType == EnemyType.Confuse)  // 적이 혼란 상태일 경우(실수하게 됨)
        {
            _realElemental = Random.value > _mistakeProbability ? (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1) : _visualElemental;

            if(_realElemental != _visualElemental)
                _isLying = true;
        }
        else // 실수 안 한 상태
        {
            _realElemental = _visualElemental;
            _isLying = false;
        }
        Debug.Log($"조합 체크: 비주얼({_visualElemental}), 실제({_realElemental})");
    }

    // 동료 마법 예고 업데이트 (공격 성공 시에만 호출)
    public void UpdatePreviewElemental()
    {
        // 비주얼적으로 보이는 것과 실제 속성 설정
        _visualElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        Elemental newRealElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        _realElemental = newRealElemental;

        // 원소 표시
        _isLying = (_realElemental != _visualElemental) ? true : false;
        _friendCastVisualCanvas.SetElementalImage(_isLying, _visualElemental);
        Debug.Log($"친구 새로운 예고: 비주얼({_visualElemental}), 실제({_realElemental})");
    }
}