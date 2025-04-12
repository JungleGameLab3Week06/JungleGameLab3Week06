using UnityEngine;
using UnityEngine.UI;

// 동료 마법 예고 UI
public class UI_FriendCastVisualCanvas : MonoBehaviour
{
    [SerializeField] Image _backgroundImage;              // 말풍선
    [SerializeField] Image _elementalImage;               // 원소 이미지
    [SerializeField] Sprite[] _elementalSprites;          // 원소 Sprite

    [Header("Color")]
    Color _colorCertain;
    Color _colorNotCertain;

    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        _backgroundImage = images[0]; // 말풍선
        _elementalImage = images[1];  // 원소 이미지

        ColorUtility.TryParseHtmlString("#00D0FF", out _colorCertain);
        ColorUtility.TryParseHtmlString("#FFBD1B", out _colorNotCertain);
    }

    // 원소 이미지 및 말풍선 색 설정
    public void SetElementalImage(bool isLying, Define.Elemental elemental)
    {
        int idx = (int)elemental;
        _elementalImage.sprite = _elementalSprites[idx];
        _backgroundImage.color = (isLying) ? _colorNotCertain : _colorCertain;
    }
}