using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance;
    public static Manager Instance => _instance;

    
    ResourceManager _resource = new ResourceManager();
    SoundManager _sound = new SoundManager();
    SceneManagerEX _scene = new SceneManagerEX();
    UIManager _ui = new UIManager();

    public static ResourceManager Resource => Instance._resource;
    public static SoundManager Sound => Instance._sound;
    public static SceneManagerEX Scene => Instance._scene;
    public static UIManager UI => Instance._ui;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        _sound.Init();
    }

    void Init()
    {
        _sound.Init();
    }

    void Clear()
    {

    }
}