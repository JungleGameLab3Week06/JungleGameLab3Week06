using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;

public class Enemy : MonoBehaviour
{
    public Image tagIcon; // 이번에 시전할 태그 아이콘 (머리 위 UI)
    [SerializeField] int hp = 100;
    EnemyState state = EnemyState.None;    
    public TextMeshProUGUI previewTagUI; // 적 머리 위 UI
    float _moveSpeed = 2f; // 비트당 이동 거리
    string previewTag;
    bool hasDamaged = false; // 중복 피해 방지

    // 동료가 적 머리 위에 시전할 마법 표시
    public void SetPreviewTag(Elemental tag, Sprite[] tagSprites)
    {
        int tagIndex = (int)tag;
        if (tagIndex >= 0 && tagIndex < tagSprites.Length)
            tagIcon.sprite = tagSprites[tagIndex];
        previewTagUI.text = tag.ToString();
    }

    public void Move()
    {
        transform.position += (Vector3)(Vector2.left * _moveSpeed); // 왼쪽으로 이동
        //CheckDamagePosition();
    }
    
    // 적 상태 변경
    public void ApplyState(EnemyState newState)
    {
        state = newState;
        Debug.Log($"적 상태: {state}");
    }

    // 피해 입기
    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log($"적 HP: {hp}");
        if (hp <= 0) Destroy(gameObject);
    }
}