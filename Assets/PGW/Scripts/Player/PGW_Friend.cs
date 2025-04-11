using UnityEngine;
using static Define;

public class PGW_Friend : MonoBehaviour
{
    Elemental _friendElemental;
    public Elemental FriendElemental => _friendElemental;

    [SerializeField] Sprite[] _weakSprites;                 // 적 머리 위에 나타나는 약점 Sprite

    // 동료 행동 (추후에 동료 스크립트 따로 만들어서 분리)
    public void CastElemental()
    {
        // 동료의 실제 태그 (70% 확률로 예고와 다름)
        Elemental actualAllyTag = Random.value > 1f ? (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length) : _friendElemental;
        Debug.Log($"조합 체크: (동료: {actualAllyTag.ToString()}, 플레이어: {_friendElemental})");
        ElementalEffect interaction = GameManager.Instance.GetInteraction(actualAllyTag, _friendElemental);

        if (actualAllyTag != _friendElemental)
        {
            Debug.Log($"동료가 속였다! 예고: {_friendElemental}, 실제: {actualAllyTag}");
        }
        else
        {
            Debug.Log($"동료가 '{actualAllyTag}'를 발사했다!");
        }
        if (interaction != null)
        {
            Debug.Log($"반응 발생: {interaction}");
            GameManager.Instance.ApplyInteraction(interaction);
        }
        else
        {
            Debug.Log($"조합 실패: {actualAllyTag} + {_friendElemental}는 정의된 반응 없음");
            GameManager.Instance.CurrentEnemy.TakeDamage(10);
        }
        UpdatePreviewElemental();
    }

    // 동료 마법 예고 업데이트
    void UpdatePreviewElemental()
    {
        // 50% 확률로 예고 태그 변경
        if (Random.value > 0.5f)
        {
            _friendElemental = (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length);
            Elemental newAllyTag = (Elemental)Random.Range(0, System.Enum.GetValues(typeof(Elemental)).Length);
            if (newAllyTag != _friendElemental) // 이전과 다를 때만 업데이트
            {
                _friendElemental = newAllyTag;
                GameManager.Instance.CurrentEnemy.SetPreviewTag(_friendElemental, _weakSprites); // 올바른 매개변수 전달
                Debug.Log($"동료 예고 변경: {_friendElemental}");
            }
        }
    }
}
