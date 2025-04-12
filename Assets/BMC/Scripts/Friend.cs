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
    [SerializeField] Sprite[] _willCastSprites;                     // 동료 앞에 나타나야 할 속성 스프라이트

    // 마법 준비
    public void PrepareElemental()
    {
        // 무작위 속성 선택해서 예고
        _visualElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        TryMistake();

        // 적 머리 위에 캐스팅한 원소 표시
        Enemy firstEnemy = GameManager.Instance.enemyList[0];
        firstEnemy.ShowFriendElemental(_visualElemental, _willCastSprites);
        Debug.Log($"친구 예고: 비주얼({_visualElemental}), 실제({_realElemental})");
    }

    // 실수 시도
    void TryMistake()
    {
        Enemy firstEnemy = GameManager.Instance.enemyList[0];
        if (firstEnemy.EnemyType == EnemyType.Confuse)  // 적이 혼란 상태일 경우(실수하게 됨)
        {
            _realElemental = Random.value > _mistakeProbability ? (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1) : _visualElemental;
        }
        else // 실수 안 한 상태
        {
            _realElemental = _visualElemental;
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

        // 적 머리 위에 원소 표시
        GameManager.Instance.CurrentEnemy.ShowFriendElemental(_visualElemental, _willCastSprites);
        Debug.Log($"친구 새로운 예고: 비주얼({_visualElemental}), 실제({_realElemental})");
    }
}