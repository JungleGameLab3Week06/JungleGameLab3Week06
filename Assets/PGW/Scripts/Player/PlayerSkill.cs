using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class PlayerSkill : MonoBehaviour
{
    // 스킬 딕셔너리
    Dictionary<ElementalEffect, ISkill> _skillDict = new Dictionary<ElementalEffect, ISkill>
    {
        { ElementalEffect.Flame, new Fire() },
        { ElementalEffect.Water, new Water() },
        { ElementalEffect.Lightning, new Lightning() },
        { ElementalEffect.Wall, new Wall() },
        { ElementalEffect.Ignition, new Ignition() },
        { ElementalEffect.ElectricShock, new ElectricShock() },
        { ElementalEffect.Fog, new Fog() },
        { ElementalEffect.Grease, new Grease() }
    };

    GameObject[] _skillEffects;

    void Start()
    {
        // Skills 폴더의 모든 GameObject 로드
        _skillEffects = Manager.Resource.LoadAll<GameObject>("Prefabs/Skills");
    }

    // 마법 조합 결과 반환
    public ElementalEffect GetInteraction(Elemental playerElemental, Elemental friendElemental)
    {
        ElementalEffect resultElementalEffect = ElementalEffect.None;                       // 없음

        int elementalEffect = (1 << (int)playerElemental) | (1 << (int)friendElemental);    // 마법 조합
        bool isExistEffect = Enum.IsDefined(typeof(ElementalEffect), elementalEffect);      // 조합이 ElementalEffect에 정의된 값인지 확인
        if (isExistEffect)
            resultElementalEffect = (ElementalEffect)elementalEffect;
        return resultElementalEffect;
    }

    // 마법 조합 효과 적용
    public void ApplyInteraction(ElementalEffect effect)
    {
        if (_skillDict.TryGetValue(effect, out ISkill skill))
        {
            skill.Execute();
            Manager.UI.activateSkillTextAction?.Invoke(effect.ToString());
            Debug.Log($"스킬 실행: {effect}");
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

    // 스킬 애니메이션 후, 파괴
    IEnumerator DestroyAfterAnimation(GameObject effectInstance, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (effectInstance != null)
        {
            Destroy(effectInstance);
        }
    }
}