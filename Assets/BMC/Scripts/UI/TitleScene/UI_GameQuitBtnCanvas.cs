using UnityEngine;
using UnityEngine.UI;

public class UI_GameQuitBtnCanvas : MonoBehaviour
{
    Button _gameQuitBtn;
    void Start()
    {
        _gameQuitBtn = GetComponentInChildren<Button>();
        _gameQuitBtn.onClick.AddListener(() => Application.Quit());
    }
}