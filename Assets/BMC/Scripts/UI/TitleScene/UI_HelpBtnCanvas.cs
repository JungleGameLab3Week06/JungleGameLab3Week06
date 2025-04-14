using UnityEngine;
using UnityEngine.UI;

public class UI_HelpBtnCanvas : MonoBehaviour
{
    Button _helpBtn;
    Button _gameStartBtn;
    void Start()
    {
        _gameStartBtn = FindAnyObjectByType<UI_GameStartBtnCanvas>().GetComponentInChildren<Button>();

        _helpBtn = GetComponentInChildren<Button>();
        _helpBtn.onClick.AddListener(() => { 
            Manager.Sound.PlayEffect(Define.Effect.BtnClick);
            Manager.UI.toggleHelpPanelCanvasAction?.Invoke();

            PlayerPrefs.SetInt("IsReadHelp", 1); // 도움말 본 적 있음
            _gameStartBtn.interactable = true;   // 버튼 활성화
        });
    }
}