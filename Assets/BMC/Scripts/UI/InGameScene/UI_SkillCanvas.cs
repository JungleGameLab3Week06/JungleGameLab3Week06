using System.Collections;
using TMPro;
using UnityEngine;

public class UI_SkillCanvas : MonoBehaviour
{
    Canvas _skillCanvas;
    TextMeshProUGUI _skillText;
    Coroutine _activateSkillCoroutine;
    float _disappearWeight = 1f;
    TextMeshProUGUI _descriptionText;
    void Start()
    {
        _skillCanvas = GetComponent<Canvas>();
        _skillText = GetComponentInChildren<TextMeshProUGUI>();
        //텍스트 하나 더 넣어서 거기에 effectDEscription을받아오는걸로하기
        Manager.UI.activateSkillTextAction += ActivateSkillText;
        _descriptionText = transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
    }

    public void ActivateSkillText(string effectTranslation, string effectDescription)
    {
        if(_activateSkillCoroutine != null)
            StopCoroutine(_activateSkillCoroutine);
        _activateSkillCoroutine = StartCoroutine(ActivateSkillTextCoroutine(effectTranslation, effectDescription));
    }

    public IEnumerator ActivateSkillTextCoroutine(string skillDescription, string effectDescription)
    {
        _descriptionText.text = effectDescription;
        _skillText.text = skillDescription;
        _skillCanvas.enabled = true;

        // Fade out text
        float alphaValue = 1f;
        _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
        while (alphaValue > 0)
        {
            alphaValue -= Time.deltaTime * _disappearWeight;
            _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
            yield return null;
            _descriptionText.color = new Color(_descriptionText.color.r, _descriptionText.color.g, _descriptionText.color.b, alphaValue);
        }

        // Hide Text
        alphaValue = 0;
        _skillText.text = "";
        _skillText.color = new Color(_skillText.color.r, _skillText.color.g, _skillText.color.b, alphaValue);
    }
}