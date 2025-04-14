using UnityEngine;

// GameOverScene Class based on BaseScene
public class GameOverScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.GameOverScene;
        Manager.Sound.PlayBGM(Define.BGM.Title);
        Debug.Log("GameOverScene 초기화");
    }
}