using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_MainMenuBtnCanvas : MonoBehaviour
{
    Canvas _mainMenuBtnCanvas;
    Button _mainMenuBtn;

    void Start()
    {
        _mainMenuBtnCanvas = GetComponent<Canvas>();
        _mainMenuBtn = GetComponentInChildren<Button>();
        _mainMenuBtn.onClick.AddListener(() => {
            Manager.Sound.PlayEffect(Effect.BtnClick);
            Manager.Scene.LoadScene(SceneType.TitleScene);
        });
    }
}
