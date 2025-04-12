using UnityEngine;
using UnityEngine.UI;

public class UI_ManualBtnCanvas : MonoBehaviour
{
    Button _manualBtn;
    void Start()
    {
        _manualBtn = GetComponentInChildren<Button>();
        _manualBtn.onClick.AddListener(() => { Manager.UI.toggleManualPanelCanvasAction?.Invoke(); });
    }
}