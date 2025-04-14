using UnityEngine;
using UnityEngine.UI;

public class UI_HelpBtnCanvas : MonoBehaviour
{
    Button _helpBtn;
    void Start()
    {
        _helpBtn = GetComponentInChildren<Button>();
        _helpBtn.onClick.AddListener(() => { 
            Manager.Sound.PlayEffect(Define.Effect.BtnClick);
            Manager.UI.toggleHelpPanelCanvasAction?.Invoke(); 
        });
    }
}