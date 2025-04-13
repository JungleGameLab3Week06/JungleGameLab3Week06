using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_GameStartBtnCanvas : MonoBehaviour
{
    Canvas _gameStartBtnCanvas;
    Button _gameStartBtn;

    void Start()
    {
        _gameStartBtnCanvas = GetComponent<Canvas>();
        _gameStartBtn = GetComponentInChildren<Button>();
        _gameStartBtn.onClick.AddListener(() => {
            Manager.Sound.PlayEffect(Effect.BtnClick);
            Manager.Scene.LoadScene(SceneType.InGameScene); 
        });
    }
}