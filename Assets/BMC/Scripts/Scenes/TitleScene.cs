using UnityEngine;

// TitleScene 클래스
public class TitleScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.TitleScene;

        // 인게임 씬에서 할 것
        Debug.Log("TitleScene 초기화");
    }
}