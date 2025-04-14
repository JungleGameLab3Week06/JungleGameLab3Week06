using System.Collections;
using TMPro;
using UnityEngine;

public class UI_WaveNotifier : MonoBehaviour
{
    public static UI_WaveNotifier Instance;
    CanvasGroup _waveCanvasGroup;
    TextMeshProUGUI _waveText;
    TextMeshProUGUI _waveTextShadows;
    Animator _anim;

    [SerializeField] string waveText = "WAVE ";

    void Start()
    {
        Instance = this;
        
        _waveCanvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();

        _waveText = _waveCanvasGroup.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _waveTextShadows = _waveCanvasGroup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        _anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    public void UpdateWave(int wave)
    {
        StartCoroutine(UpdateWaveCo(wave));
    }
    
    IEnumerator UpdateWaveCo(int wave)
    {
        _waveText.text = waveText + wave;
        _waveTextShadows.text = waveText + wave;
        _anim.SetTrigger("On");
        yield return new WaitForSeconds(1f);

        _anim.SetTrigger("Off");
        yield return new WaitForSeconds(0.5f);

        _waveText.text = "";
        _waveTextShadows.text = "";
    }
}
