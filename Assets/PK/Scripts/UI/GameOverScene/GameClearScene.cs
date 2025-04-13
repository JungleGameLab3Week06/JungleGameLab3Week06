using UnityEngine;

// GameClearScene Class based on BaseScene
public class GameClearScene : BaseScene
{
    protected override void Init()
    {
        SceneType = Define.SceneType.GameClearScene;
        Manager.Sound.PlayBGM(Define.BGM.Title);
        Debug.Log("GameClearScene 초기화");
    }
}
