using UnityEngine;

public class UIManager
{
    #region Variables

    [Header("Status")]
    [SerializeField] BeatNotifier beatNotifier;
    Coroutine beatCoroutine;

    [Header("Interaction Judgements")]
    [SerializeField] JudgeNotifier judgeNotifier;
    Coroutine judgeCoroutine;

    #endregion

    #region Start

    void Start()
    {
        beatNotifier = UnityEngine.Object.FindAnyObjectByType<BeatNotifier>();
        judgeNotifier = UnityEngine.Object.FindAnyObjectByType<JudgeNotifier>();
    }

    #endregion

    #region Notifiers

    #endregion
}