using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameManager gameManager; // 인스펙터에서 연결


   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 키보드 입력 감지
        if (Input.GetKeyDown(KeyCode.UpArrow)) // Flame (화염)
        {
            gameManager.SelectTag("화염");
            Debug.Log("Flame 키 입력: 화염");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Lightning (번개)
        {
            gameManager.SelectTag("번개");
            Debug.Log("Lightning 키 입력: 번개");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)) // Oil (기름)
        {
            gameManager.SelectTag("기름");
            Debug.Log("Oil 키 입력: 기름");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // Frost (냉기)
        {
            gameManager.SelectTag("냉기");
            Debug.Log("Frost 키 입력: 냉기");
        }
    }
}
