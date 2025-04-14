using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class PlayerSkill : MonoBehaviour
{
    public bool IsFireStrong { get { return _isFireStrong; } set { _isFireStrong = value; } }
    public bool IsLightningStrong { get { return _isLightningStrong; } set { _isLightningStrong = value; } }
    [SerializeField] bool _isFireStrong = false;          // 불 속성 강화
    [SerializeField] bool _isLightningStrong = false;     // 전기 속성 강화
    GameObject[] _skillEffects;
    Dictionary<ElementalEffect, Skill> _skillDict;

    void Start()
    {
        // Skills 폴더의 모든 GameObject 로드
        _skillEffects = Manager.Resource.LoadAll<GameObject>("Prefabs/Skills");

        _skillDict = Manager.Data.SkillDict;
    }

    // 마법 조합 결과 반환
    public ElementalEffect GetInteraction(Elemental playerElemental, Elemental friendElemental)
    {
        ElementalEffect resultElementalEffect = ElementalEffect.None;                       // 없음

        int elementalEffect = (1 << (int)playerElemental) | (1 << (int)friendElemental);    // 마법 조합
        bool isExistEffect = Enum.IsDefined(typeof(ElementalEffect), elementalEffect);      // 조합이 ElementalEffect에 정의된 값인지 확인
        if (isExistEffect)
        {
            resultElementalEffect = (ElementalEffect)elementalEffect;
            
        }
        return resultElementalEffect;
    }

    // 마법 조합 효과 적용
    public void ApplyInteraction(ElementalEffect effect)
    {
        if (_skillDict.TryGetValue(effect, out Skill skill))
        {
            skill.Execute();

            string effectTranslation = ((Translation)effect).ToString();
            string description = Manager.Data.SkillInfoDict[effectTranslation].SkillDescription;
            string result = $"{effectTranslation}: {description}";

            Manager.UI.activateSkillTextAction?.Invoke(effectTranslation, description);
            Debug.Log($"(스킬) {result}");
        }
        else
        {
            Debug.LogWarning($"등록되지 않은 효과: {effect}");
        }
    }

    // 스킬 효과 GameObject 반환
    GameObject GetSkillEffect(ElementalEffect effect)
    {
        foreach (var skillEffect in _skillEffects)
        {
            if (skillEffect.name.Contains(effect.ToString()))
            {
                return skillEffect;
            }
        }
        Debug.LogWarning($"스킬 효과를 찾을 수 없습니다: {effect}");
        return null;
    }

    // 스킬 효과 실행
    public void ExcuteEffect(ElementalEffect effect, Vector3 pos)
    {
        GameObject skillEffect = GetSkillEffect(effect);
        if (skillEffect != null)
        {
            GameObject effectInstance = Instantiate(skillEffect, pos, Quaternion.identity);
            float delayTime = 5f;

            // 애니메이터 존재하는 경우
            if(effectInstance.TryGetComponent<Animator>(out Animator anim) && effect != ElementalEffect.EnemyShock)
            {
                AnimationClip clip = anim.runtimeAnimatorController.animationClips.FirstOrDefault();
                delayTime = clip.length;
            }
            Destroy(effectInstance, delayTime);
        }
    }

    public void DestroyFog()
    {
        Fog[] fogs = GameObject.FindObjectsOfType<Fog>();
        foreach (Fog fog in fogs)
        {
            if (fog != null)
            {
                Destroy(fog.gameObject);
            }
        }
    }

    public void DestroyGrease()
    {
        Grease[] greases = GameObject.FindObjectsOfType<Grease>();
        foreach (Grease grease in greases)
        {
            if (grease != null)
            {
                Destroy(grease.gameObject);
            }
        }
    }
}