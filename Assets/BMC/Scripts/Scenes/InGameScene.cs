using UnityEngine;

// InGameScene 클래스
public class InGameScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.InGameScene;
        Manager.Sound.PlayBGM(Define.BGM.Main);
        Debug.Log("InGameScene 초기화");
    }
}