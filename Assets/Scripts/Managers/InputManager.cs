using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class InputManager : MonoBehaviour
{
    static InputManager _instance;
    public static InputManager Instance => _instance;

    public Action<Elemental> selectElementalAction;

    InputSystemActions inputSystemActions;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputSystemActions = new InputSystemActions();
        inputSystemActions.Enable();                        // InputSystemActions 활성화
        inputSystemActions.Player.Enable();                 // Player Action Map 활성화

        inputSystemActions.Player.Cast.performed += OnCast;
        inputSystemActions.Player.Cast.canceled += OnCast;

    }

    void OnCast(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Vector2 castValue = context.ReadValue<Vector2>();

            // 키보드 입력 감지
            if (castValue.y == 1) // Flame (화염)
            {

                selectElementalAction.Invoke(Elemental.Flame);
                Debug.Log("Flame 키 입력: 화염");
            }
            else if (castValue.x == -1) // Lightning (번개)
            {
                selectElementalAction.Invoke(Elemental.Lightning);
                Debug.Log("Lightning 키 입력: 번개");
            }
            else if (castValue.y == -1) // Oil (기름)
            {
                selectElementalAction.Invoke(Elemental.Ground);
                Debug.Log("Oil 키 입력: 기름");
            }
            else if (castValue.x == 1) // Frost (냉기)
            {
                selectElementalAction.Invoke(Elemental.Water);
                Debug.Log("Frost 키 입력: 냉기");
            }
        }
        else if (context.canceled)
        {
            // Cast 액션이 취소되었을 때의 처리
            Debug.Log("Cast 액션 취소됨");
        }
    }
}