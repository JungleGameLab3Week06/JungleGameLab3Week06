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


        if(PlayerPrefs.HasKey("IsReadHelp"))
        {
            if(PlayerPrefs.GetInt("IsReadHelp") == 0)
            {
                _gameStartBtn.interactable = false;
            }
            else
            {
                _gameStartBtn.interactable = true;
            }
        }
        else
        {
            PlayerPrefs.SetInt("IsReadHelp", 0); // 도움말 본 적 없음
            _gameStartBtn.interactable = false; // 도움말 본 적 없음
        }
    }
}