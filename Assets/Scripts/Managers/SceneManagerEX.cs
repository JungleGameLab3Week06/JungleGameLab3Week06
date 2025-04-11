using UnityEngine;
using static Define;
using UnityEngine.SceneManagement;

public class SceneManagerEX
{
    public BaseScene CurrentScene { get { return GameObject.FindAnyObjectByType<BaseScene>(); } }

    public void Init()
    {

    }

    public void LoadScene(SceneType sceneType)
    {
        string sceneName = sceneType.ToString();
        SceneManager.LoadScene(sceneName);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}