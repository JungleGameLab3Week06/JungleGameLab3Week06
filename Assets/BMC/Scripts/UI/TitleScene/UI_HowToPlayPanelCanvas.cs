using UnityEngine;
using UnityEngine.UI;

public class UI_HowToPlayPanelCanvas : MonoBehaviour
{
    Canvas _howToPlayPanelCanvas;
    Button[] _tabButtons;
    void Start()
    {
        _howToPlayPanelCanvas = GetComponent<Canvas>();
        _tabButtons = GetComponentsInChildren<Button>();
        Manager.UI.toggleHelpPanelCanvasAction += ToggleHelpPanelCanvas;
    }
    
    // 플레이 방법 토글
    public void ToggleHelpPanelCanvas()
    {
        _howToPlayPanelCanvas.enabled = !_howToPlayPanelCanvas.enabled;
    }
}