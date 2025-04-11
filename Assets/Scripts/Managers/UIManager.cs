using System;
using UnityEngine;

public class UIManager
{
    #region Variables

    [Header("Status")]
    [SerializeField] BeatNotifier beatNotifier;
    Coroutine beatCoroutine;

    public Action<bool> showJudgeTextAction;

    #endregion
}