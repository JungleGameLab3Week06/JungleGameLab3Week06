using UnityEngine;

// TitleScene 클래스
public class TitleScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.TitleScene;

        Manager.UI.Clear();
        Manager.Sound.PlayBGM(Define.BGM.Title);
        Debug.Log("TitleScene 초기화");
    }
}