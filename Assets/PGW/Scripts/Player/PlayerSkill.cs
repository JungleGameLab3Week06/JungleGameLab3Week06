using UnityEngine;
using static Define;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using System.Collections;

public class PlayerSkill : MonoBehaviour
{
    Dictionary<ElementalEffect, ISkill> _skillMap;

    Dictionary<(Elemental, Elemental), ElementalEffect> _tagInteractions =
        new Dictionary<(Elemental, Elemental), ElementalEffect>
        {
            // 동일 요소 조합
            { (Elemental.Flame, Elemental.Flame), ElementalEffect.Fire },
            { (Elemental.Water, Elemental.Water), ElementalEffect.Water },
            { (Elemental.Lightning, Elemental.Lightning), ElementalEffect.Lightning },
            { (Elemental.Ground, Elemental.Ground), ElementalEffect.Wall },

            // 서로 다른 요소 조합
            { (Elemental.Flame, Elemental.Water), ElementalEffect.Fog },
            { (Elemental.Water, Elemental.Flame), ElementalEffect.Fog }, // 순서 반대
            { (Elemental.Flame, Elemental.Lightning), ElementalEffect.Diffusion },
            { (Elemental.Lightning, Elemental.Flame), ElementalEffect.Diffusion },
            { (Elemental.Flame, Elemental.Ground), ElementalEffect.Ignition },
            { (Elemental.Ground, Elemental.Flame), ElementalEffect.Ignition },
            { (Elemental.Water, Elemental.Lightning), ElementalEffect.ElectricShock },
            { (Elemental.Lightning, Elemental.Water), ElementalEffect.ElectricShock },
            { (Elemental.Water, Elemental.Ground), ElementalEffect.Grease },
            { (Elemental.Ground, Elemental.Water), ElementalEffect.Grease },
            { (Elemental.Lightning, Elemental.Ground), ElementalEffect.None },
            { (Elemental.Ground, Elemental.Lightning), ElementalEffect.None }
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
        _skillEffects = Manager.Resource.LoadAll<GameObject>($"Skills");
    }

    // 마법 조합 결과 반환
    public ElementalEffect GetInteraction(Elemental tag1, Elemental tag2)
    {
        if (_tagInteractions.TryGetValue((tag1, tag2), out ElementalEffect interaction))
            return interaction;
        if (_tagInteractions.TryGetValue((tag2, tag1), out interaction))
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
