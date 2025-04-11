using UnityEngine;
using UnityEngine.UI;

// 뒤로 가기 버튼 캔버스
public class UI_BackBtnCanvas : MonoBehaviour
{
    Canvas _backBtnCanvas;
    Button _backBtn;

    void Start()
    {
        _backBtnCanvas = GetComponent<Canvas>();
        _backBtn = GetComponentInChildren<Button>();
        _backBtn.onClick.AddListener(() => Manager.Scene.LoadScene(Define.SceneType.TitleScene));
    }
}