using UnityEngine;

// InGameScene 클래스
public class InGameScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.InGameScene;

        // 인게임 씬에서 할 것
        Debug.Log("InGameScene 초기화");
    }
}