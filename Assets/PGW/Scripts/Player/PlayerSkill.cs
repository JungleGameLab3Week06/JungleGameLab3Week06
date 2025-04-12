using UnityEngine;
using static Define;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Collections;

public class PlayerSkill : MonoBehaviour
{
    Dictionary<ElementalEffect, ISkill> _skillMap;

    Dictionary<int, ElementalEffect> _tagInteractions =
        new Dictionary<int, ElementalEffect>
        {
            //화염1 물2 땅4 번개8
            {1, ElementalEffect.Fire },
            {2, ElementalEffect.Water },
            {4, ElementalEffect.Wall },
            {8, ElementalEffect.Lightning },
            {3, ElementalEffect.Fog }, // 화염 + 물
            {5, ElementalEffect.Ignition }, // 화염 + 땅
            {9, ElementalEffect.Diffusion }, // 화염 + 번개
            {6, ElementalEffect.Grease }, // 물 + 땅
            {10, ElementalEffect.ElectricShock }, // 물 + 번개
            {12, ElementalEffect.None }, // 땅 + 번개
        };

    GameObject[] _skillEffects;

    void Start()
    {
        // 스킬 초기화
        _skillMap = new Dictionary<ElementalEffect, ISkill>
        {
            { ElementalEffect.Fire, new Fire() },
            { ElementalEffect.Water, new Water() },
            { ElementalEffect.Lightning, new Lightning() },
            { ElementalEffect.Wall, new Wall() },
            { ElementalEffect.Ignition, new Ignition() },
            { ElementalEffect.ElectricShock, new ElectricShock() },
            { ElementalEffect.Fog, new Fog() },
            { ElementalEffect.Grease, new Grease() }
        };

        // Skills 폴더의 모든 GameObject 로드
        _skillEffects = Manager.Resource.LoadAll<GameObject>("Prefabs/Skills");
    }

    // 마법 조합 결과 반환
    public ElementalEffect GetInteraction(Elemental tag1, Elemental tag2)
    {
        int combinedTag = (int)tag1 | (int)tag2;
        if (_tagInteractions.TryGetValue(combinedTag, out ElementalEffect interaction))
            return interaction;
        return ElementalEffect.None;
    }

    // 마법 조합 효과 적용
    public void ApplyInteraction(ElementalEffect effect)
    {
        if (_skillMap.TryGetValue(effect, out ISkill skill))
        {
            skill.Execute();
            Debug.Log($"스킬 실행: {effect}");
        }
        else
        {
            Debug.LogWarning($"등록되지 않은 효과: {effect}");
        }
    }

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

    public void ExcuteEffect(ElementalEffect effect, Vector3 pos)
    {
        GameObject skillEffect = GetSkillEffect(effect);

        if (skillEffect != null)
        {
            GameObject effectInstance = Instantiate(skillEffect, pos, Quaternion.identity);
            Animator animator = effectInstance.GetComponent<Animator>();
            if (animator != null)
            {
                // 애니메이션 길이에 맞춰 삭제
                AnimationClip clip = animator.runtimeAnimatorController.animationClips.FirstOrDefault();
                if (clip != null)
                {
                    StartCoroutine(DestroyAfterAnimation(effectInstance, clip.length));
                }
            }
        }
    }

    private IEnumerator DestroyAfterAnimation(GameObject effectInstance, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }
}
