using UnityEngine;

public class UI_ManualPanelCanvas : MonoBehaviour
{
    Canvas _manualPanelCanvas;
    void Start()
    {
        _manualPanelCanvas = GetComponent<Canvas>();
        Manager.UI.toggleManualPanelCanvasAction += ToggleManualPanelCanvas;
    }
    
    // 플레이 방법 토글
    public void ToggleManualPanelCanvas()
    {
        _manualPanelCanvas.enabled = !_manualPanelCanvas.enabled;
    }
}