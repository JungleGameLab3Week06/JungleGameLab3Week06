using UnityEngine;
using static Define;

public class BaseScene : MonoBehaviour
{
    public SceneType SceneType { get; protected set; } = SceneType.None;

    void Awake()
    {
        Init();
    }

    // 씬 시작 시, 초기화 작업
    protected virtual void Init() {}

    // 씬 종료 시, 정리 작업
    public virtual void Clear() {}
}