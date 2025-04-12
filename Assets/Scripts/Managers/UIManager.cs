using System;
using UnityEngine;

public class UIManager
{
    #region Variables

    [Header("Status")]
    [SerializeField] BeatNotifier beatNotifier;
    Coroutine beatCoroutine;

    [Header("Title")]
    public Action toggleManualPanelCanvasAction;

    [Header("InGame")]
    public Action<bool> showJudgeTextAction;

    #endregion
}