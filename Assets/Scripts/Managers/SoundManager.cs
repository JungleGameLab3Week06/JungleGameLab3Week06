using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SoundManager
{
    AudioSource _bgmSource;                                                             // BGM
    AudioSource _effectSource;                                                          // 효과음
    Dictionary<BGM, AudioClip> _bgmDict = new Dictionary<BGM, AudioClip>();             // BGM 딕셔너리
    Dictionary<Effect, AudioClip> _effectDict = new Dictionary<Effect, AudioClip>();    // 효과음 딕셔너리

    public void Init()
    {
        AudioSource[] audioSources = Manager.Instance.transform.GetComponentsInChildren<AudioSource>();
        _bgmSource = audioSources[0];
        _effectSource = audioSources[1];

        /* 추후에 Resources 특정 폴더 다 가져와서 등록하기 */

        // BGM 등록
        AudioClip audioClip =  Manager.Resource.Load<AudioClip>($"Sounds/BGM/{BGM.Main.ToString()}");
        _bgmDict.Add(BGM.Main, audioClip);

        // Effect 등록
    }

    // BGM 재생
    public void PlayBGM(BGM bgm)
    {
        _bgmSource.clip = _bgmDict[bgm];
        _bgmSource.Play();
    }

    // 효과음 재생
    public void PlayEffect(Effect effect)
    {
        _effectSource.PlayOneShot(_effectDict[effect]);
    }

    public void Clear()
    {
        _bgmSource.Stop();
        _effectSource.Stop();
    }
}