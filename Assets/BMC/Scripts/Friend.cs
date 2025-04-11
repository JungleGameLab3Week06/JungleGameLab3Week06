using System;
using UnityEngine;
using Random = UnityEngine.Random;
using static Define;

public class Friend : MonoBehaviour
{
    public Elemental FriendElemental => _visualElemental;
    [SerializeField] Elemental _visualElemental = Elemental.None;   // 눈으로 보이는 속성
    [SerializeField] Elemental _realElemental = Elemental.None;     // 실제 속성
    [SerializeField] bool _isLying = false;                         // 동료가 속이고 있는지 여부
    [SerializeField] Sprite[] _willCastSprites;                     // 적 머리 위에 나타나는 약점 Sprite
    [SerializeField] float _mistakeProbability = 0.75f;             // 동료가 실수할 확률

    PlayerSkill _playerSkill;

    void Start()
    {
        _playerSkill = GameManager.Instance.PlayerController.GetComponent<PlayerSkill>();
    }

    // 마법 시전
    public void CastElemental()
    {
        // 무작위 속성 선택
        _visualElemental = (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1);
        TryMistake();
        ElementalEffect interaction = _playerSkill.GetInteraction(_realElemental, _visualElemental); // 나중에 _visual 말고 player거 넣어주면 됨
 
        if (interaction != null)
        {
            _playerSkill.ApplyInteraction(interaction);
            Debug.Log($"반응 발생: {interaction}");
            //UpdatePreviewElemental();
            Enemy firstEnemy = GameManager.Instance.enemyList[0];
            firstEnemy.ShowFriendElemental(_visualElemental, _willCastSprites);
        }
        else
        {
            Debug.Log($"조합 실패: {_realElemental} + {_visualElemental}는 잘못된 마법이다(기본 데미지 적용)");
            GameManager.Instance.CurrentEnemy.TakeDamage(10);
        }
    }

    // 실수 시도
    void TryMistake()
    {
        Enemy firstEnemy = GameManager.Instance.enemyList[0];
        if (firstEnemy.EnemyType == EnemyType.Confuse)  // 적이 혼란 상태일 경우(실수하게 됨)
        {
            _realElemental = Random.value > _mistakeProbability ? (Elemental)Random.Range(0, Enum.GetValues(typeof(Elemental)).Length - 1) : _visualElemental;
            Debug.LogError("동료는 헷갈려한다..!");
            _isLying = true;

            // 동료가 속이고 있는 상태로 설정
            //PlayerController 혹은 PlayerSkill쪽으로 가짜예고가 아닌 실제 속성 전달 {actualAllyTag}
            // 동료가 속였다! 예고: {_friendElemental}, 실제: {actualAllyTag}
            //Ui관련, 이펙트 관련 이벤트  전달하기위한 이벤트
            //적타입에 따라 헷갈려하는거만 구분하기.
        }
        else // 실수 안 한 상태
        {
            _realElemental = _visualElemental;
            _isLying = false;
        }
        Debug.Log($"조합 체크: (동료: {_realElemental.ToString()}, 플레이어: {_visualElemental})");
    }

    // 동료 마법 예고 업데이트 (공격 성공 시에만 호출)
    void UpdatePreviewElemental()
    {
        // 50% 확률로 예고 태그 변경
        if (Random.value > 0.5f)
        {
            _visualElemental = (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length - 1);
            Elemental newAllyTag = (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length - 1);
            if (newAllyTag != _visualElemental) // 이전과 다를 때만 업데이트
            {
                _visualElemental = newAllyTag;
                GameManager.Instance.CurrentEnemy.ShowFriendElemental(_visualElemental, _willCastSprites); // 올바른 매개변수 전달
                Debug.Log($"동료 예고 변경: {_visualElemental}");
            }
        }
    }
}