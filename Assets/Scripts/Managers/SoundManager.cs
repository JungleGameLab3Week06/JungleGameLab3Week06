using System;
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
        // AudioSource 컴포넌트 가져오기
        AudioSource[] audioSources = Manager.Instance.transform.GetComponentsInChildren<AudioSource>();
        _bgmSource = audioSources[0];
        _effectSource = audioSources[1];

        // Resources 폴더에서 AudioClip 로드
        // 1. BGM 등록
        AudioClip[] bgmClips = Manager.Resource.LoadAll<AudioClip>($"Sounds/BGM");
        foreach (AudioClip bgmClip in bgmClips)
        {
            string name = bgmClip.name;
            if (Enum.TryParse(name, out BGM bgmType))
            {
                _bgmDict.Add(bgmType, bgmClip);
            }
        }
        // 2. Effect 등록
        AudioClip[] effectClips = Manager.Resource.LoadAll<AudioClip>($"Sounds/Effect");
        foreach (AudioClip effectClip in effectClips)
        {
            string name = effectClip.name;
            if (Enum.TryParse(name, out Effect effectType))
            {
                _effectDict.Add(effectType, effectClip);
            }
        }
    }

    // BGM 재생 속도 조절
    public void SetBGMPitch(float bpm)
    {
        _bgmSource.pitch = bpm / 60;
    }

    // BGM 재생
    public void PlayBGM(BGM bgm)
    {
        _bgmSource.clip = _bgmDict[bgm];
        _bgmSource.pitch = 1f;
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