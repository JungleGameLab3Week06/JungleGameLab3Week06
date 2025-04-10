using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    //적 체력
    //적 태그 반응
    public Image tagIcon; // 이번에 시전할 태그 아이콘 (머리 위 UI)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int hp = 100;
    public string state = "정상";    
    public TextMeshProUGUI previewTagUI; // 적 머리 위 UI
    public float moveSpeed = 2f; // 비트당 이동 거리
    private string previewTag;
    private bool hasDamaged = false; // 중복 피해 방지

    public void SetPreviewTag(string tag, Sprite[] tagSprites, string[] tags)
    {
        int tagIndex = System.Array.IndexOf(tags, tag);
        if (tagIndex >= 0 && tagIndex < tagSprites.Length)
            tagIcon.sprite = tagSprites[tagIndex];
        previewTagUI.text = tag;

    }

    public void Move()
    {
        transform.position += (Vector3)(Vector2.left * -moveSpeed); // 왼쪽으로 이동
        //CheckDamagePosition();
    }
    // private void CheckDamagePosition()
    // {
    //     if (transform.position.x <= -10f && !hasDamaged)
    //     {
    //         TakeDamage(20); // x가 -10에 도달하면 데미지
    //         hasDamaged = true; // 한 번만 피해 입히도록 플래그 설정
    //     }
    // }
    
    public void ApplyState(string newState)
    {
        state = newState;
        Debug.Log($"적 상태: {state}");
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        Debug.Log($"적 HP: {hp}");
        if (hp <= 0) Destroy(gameObject);
    }

    public string GetPreviewTag() => previewTag;
}
