using System;
using UnityEngine;

public class UIManager
{
    #region Variables

    [Header("Status")]
    [SerializeField] BeatNotifier beatNotifier;
    Coroutine beatCoroutine;

    [Header("Title")]
    public Action toggleHelpPanelCanvasAction;

    [Header("InGame")]
    public Action<bool> showJudgeTextAction;
    public Action<string, string> activateSkillTextAction;
    public Action<int> updateWaveAction;

    #endregion

    public void Clear()
    {
        toggleHelpPanelCanvasAction = null;

        showJudgeTextAction = null;
        activateSkillTextAction = null;
        updateWaveAction = null;
    }
}