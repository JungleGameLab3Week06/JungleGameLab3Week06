using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    static CameraController _instance;
    public static CameraController Instance => _instance;

    [SerializeField] CinemachineCamera _shakeCamera;
    [SerializeField] CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    float _startIntensity = 0.5f;   // 흔들기 시작할 때, 첫 강도
    float _shakeTimer;              // 흔드는 시간
    float _shakeTimerTotal;         // 전체 흔드는 시간

    void Awake()
    {
        if(_instance == null)
            _instance = this;

        _shakeCamera = FindAnyObjectByType<CinemachineCamera>();
        _cinemachineBasicMultiChannelPerlin = _shakeCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0)
            {
                _cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(_startIntensity, 0f, 1 - (_shakeTimer / _shakeTimerTotal));
            }
        }
    }

    // 카메라 흔들기
    public void ShakeCamera(float intensity = 5f, float time = 0.1f)
    {
        _cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        _startIntensity = intensity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }
}