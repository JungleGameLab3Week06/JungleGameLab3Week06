using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    public GameManager gameManager; // 인스펙터에서 연결
    public Button fireButton, oilButton, iceButton, electricButton;
    PlayerInput playerInput;
    void Start()
    {
        
        oilButton.onClick.AddListener(() => gameManager.SelectTag("기름"));
        fireButton.onClick.AddListener(() => gameManager.SelectTag("화염"));
        iceButton.onClick.AddListener(() => gameManager.SelectTag("냉기"));
        electricButton.onClick.AddListener(() => gameManager.SelectTag("번개"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
